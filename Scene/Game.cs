using Game1.Actor;
using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1.Scene
{
    class Game : Scene
    {
        enum PauseMenu
        {
            Resume,//ゲームに戻る
            Quit,//タイトルに戻る
            Max,
        }

        private bool is_clear;
        private Timer clear_timer;

        private float fade_alpha;

        private bool is_pause;
        private PauseMenu menu;
        private Dictionary<PauseMenu, string> menu_name;
        private int counter;

        private float band_easing_t;
        private float easing_t;
        private bool is_count;
        private bool s_frame;

        private bool title_flag;
        private float fade_scale;

        private Timer appear_timer;

        private float scale_up;

        public Game(SceneManager scene_m) : base(scene_m) { }
        
        public override void Init()
        {
            next = SceneName.Clear;
            is_end = false;

            scene_m.game_timer = 0;

            //チェックポイントにプレイヤーを生成
            for (int y = 0; y < scene_m.map.numeric_map.Length; y++)
            {
                for (int x = 0; x < scene_m.map.numeric_map[y].Length; x++)
                {
                    if (scene_m.map.numeric_map[y][x] == 3)
                    {
                        if (x >= scene_m.map.map_data[scene_m.map.current_map][1] &&
                            x <= scene_m.map.map_data[scene_m.map.current_map][1] + scene_m.map.map_data[scene_m.map.current_map][3] - 1 &&
                            y >= scene_m.map.map_data[scene_m.map.current_map][2] &&
                            y <= scene_m.map.map_data[scene_m.map.current_map][2] + scene_m.map.map_data[scene_m.map.current_map][4] - 1)
                        {
                            scene_m.game_obj_m.Add(new Player(scene_m.game_obj_m, new Vector2(x * Define.MAP_WIDTH, y * Define.MAP_HEIGHT), scene_m.map));
                            Camera.correct_pos =
                                new Vector2(x * Define.MAP_WIDTH, y * Define.MAP_HEIGHT) -
                                new Vector2(Define.SCREEN_WIDTH / 2, Define.SCREEN_HEIGHT / 2);
                        }
                    }
                }
            }

            Camera.position =
                new Vector2(5 * Define.MAP_WIDTH, 70 * Define.MAP_HEIGHT) -
                new Vector2(Define.SCREEN_WIDTH / 2, Define.SCREEN_HEIGHT / 2);
            Camera.position = Vector2.Clamp(Camera.position, Camera.min, Camera.max);//範囲指定

            is_clear = false;
            clear_timer = new CountDownTimer(2.5f);

            fade_alpha = 0.0f;

            is_pause = false;
            menu = PauseMenu.Resume;

            menu_name = new Dictionary<PauseMenu, string>
            {
                { PauseMenu.Resume, "resume"},
                { PauseMenu.Quit, "quit" },
            };

            band_easing_t = 0.0f;
            easing_t = 50;
            is_count = false;
            appear_timer = new CountDownTimer(0.8f);
            s_frame = true;
            title_flag = false;
            fade_scale = 0;
            scale_up = 1;
        }

        public override void Update(GameTime game_time)
        {
            appear_timer.Update(game_time);
            if (!appear_timer.IsTime())
            {
                if (scene_m.game_obj_m.Player() != null)
                {
                    Input.key_lock = true;

                    Vector2 pos = scene_m.game_obj_m.Player().position;
                    if (s_frame) scene_m.game_obj_m.Player().vel += new Vector2(0, -15);
                    scene_m.game_obj_m.Player().position = pos;
                    s_frame = false;
                }
            }
            else if (!is_clear)
            {
                Input.key_lock = false;
            }


            counter = Math.Min(counter += 12, 180);

            if (!is_pause)
            {
                scene_m.game_obj_m.Update(game_time);
                scene_m.map.Update(game_time);

                scene_m.snow_bg.Update(game_time);

                for (int i = 0; i < scene_m.bg.Count; i++)
                {
                    scene_m.bg[i].Update(game_time);

                    if (scene_m.bg[i].is_dead)
                    {
                        scene_m.bg.Remove(scene_m.bg[i]);
                        BackGround new_bg = new BackGround();
                        scene_m.bg.Add(new_bg);
                    }
                }
            }

            //死んだら復活
            if (!is_clear)
            {
                if (scene_m.game_obj_m.Player() == null)
                {
                    for (int y = 0; y < scene_m.map.numeric_map.Length; y++)
                    {
                        for (int x = 0; x < scene_m.map.numeric_map[y].Length; x++)
                        {
                            if (scene_m.map.numeric_map[y][x] == 1)//チェックポイント
                            {
                                if (x >= scene_m.map.map_data[scene_m.map.current_map][1] &&
                                    x <= scene_m.map.map_data[scene_m.map.current_map][1] + scene_m.map.map_data[scene_m.map.current_map][3] - 1 &&
                                    y >= scene_m.map.map_data[scene_m.map.current_map][2] &&
                                    y <= scene_m.map.map_data[scene_m.map.current_map][2] + scene_m.map.map_data[scene_m.map.current_map][4] - 1)
                                {
                                    scene_m.dead_counter++;
                                    scene_m.game_obj_m.Add(new Player(scene_m.game_obj_m, new Vector2(x * Define.MAP_WIDTH, y * Define.MAP_HEIGHT), scene_m.map));

                                    Camera.correct_pos =
                                        new Vector2(x * Define.MAP_WIDTH, y * Define.MAP_HEIGHT) -
                                        new Vector2(Define.SCREEN_WIDTH / 2, Define.SCREEN_HEIGHT / 2);
                                }
                            }
                        }
                    }
                }
            }

            if (scene_m.game_obj_m.Player() != null && !is_clear)
            {
                if (scene_m.game_obj_m.Player().is_clear)
                {
                    is_clear = true;
                    Input.key_lock = true;

                    GameDevice.Instance().Render.scale *= 1.6f;
                    GameDevice.Instance().Render.c_pos = 1.6f;
                }
            }

            if (Input.GetKeyDown(Keys.LeftShift) ||//左シフト
                Input.GetKeyDown(PlayerIndex.One, Buttons.Start))//スタートボタン
            {
                is_pause = !is_pause;

                if (is_pause) scene_m.game_obj_m.ObjectPause();
                else scene_m.game_obj_m.ObjectActive();
            }

            if (is_clear)
            {
                clear_timer.Update(game_time);

                if (clear_timer.IsTime())
                {
                    if (fade_alpha < 0.5f)
                    {
                        fade_alpha += 0.02f;
                    }
                    else
                    {
                        is_end = true;
                    }
                }

                //カメラ座標
                for (int y = 0; y < scene_m.map.numeric_map.Length; y++)
                {
                    for (int x = 0; x < scene_m.map.numeric_map[y].Length; x++)
                    {
                        if (scene_m.map.numeric_map[y][x] == 3)//クリア
                        {
                            Camera.position = new Vector2(
                                scene_m.map.map[y][x].position.X + Define.MAP_WIDTH * GameDevice.Instance().Render.scale.X / 2 - Define.SCREEN_WIDTH / (2 * 1.6f),
                                scene_m.map.map[y][x].position.Y + Define.MAP_HEIGHT * GameDevice.Instance().Render.scale.Y / 2 - Define.SCREEN_HEIGHT / (2 * 1.6f));
                            Camera.is_move = false;
                        }
                    }
                }
            }
            else
            {
                //タイマーカンスト
                if (!is_pause && is_count) scene_m.game_timer += 1;
                if (scene_m.game_timer >= 999.99f * 60)
                {
                    scene_m.game_timer = (int)999.99f * 60;
                }
                if (is_pause)
                {
                    if (fade_alpha < 0.8f)
                    {
                        fade_alpha += 0.1f;
                    }
                    else
                    {
                        fade_alpha = 0.8f;

                        if (Input.GetKeyDown(Keys.Up) ||//上キー
                            Input.GetKeyDown(PlayerIndex.One, Buttons.DPadUp) ||//十字キーの上
                            Input.GetKeyDown(PlayerIndex.One, Buttons.LeftThumbstickUp))//アナログスティックの上
                        {
                            menu--;
                            if (menu < 0)
                            {
                                menu = PauseMenu.Max - 1;
                            }

                            counter = 0;
                        }
                        if (Input.GetKeyDown(Keys.Down) ||//下キー
                            Input.GetKeyDown(PlayerIndex.One, Buttons.DPadDown) ||//十字キーの下
                            Input.GetKeyDown(PlayerIndex.One, Buttons.LeftThumbstickDown))//アナログスティックの下
                        {
                            menu++;
                            if (menu >= PauseMenu.Max)
                            {
                                menu = 0;
                            }

                            counter = 0;
                        }

                        if (Input.GetKeyDown(Keys.Z) ||//Zキー
                            Input.GetKeyDown(PlayerIndex.One, Buttons.A))//Aボタン
                        {
                            if (menu == PauseMenu.Resume)
                            {
                                is_pause = false;
                                scene_m.game_obj_m.ObjectActive();
                            }
                            if (menu == PauseMenu.Quit)
                            {
                                next = SceneName.Title;
                                title_flag = true;
                            }
                        }
                    }
                }
                else
                {
                    if (fade_alpha > 0)
                    {
                        fade_alpha -= 0.1f;
                    }
                    else
                    {
                        fade_alpha = 0.0f;
                    }
                }

                if (band_easing_t < easing_t)
                    band_easing_t++;
            }



            if (title_flag)
            {
                fade_scale += 0.1f;

                if (fade_scale >= 6.5f)
                {
                    is_end = true;
                    scene_m.title_fade = true;
                }
            }
        }

        public override void Draw(Render render)
        {

            foreach (var b in scene_m.bg)
            {
                b.Draw(render);
            }

            scene_m.snow_bg.BackDraw(render);

            scene_m.bg_map.Draw(render);
            scene_m.game_obj_m.Draw(render);
            scene_m.bg_map.AfterDraw(render);
            scene_m.map.Draw(render);
            scene_m.game_obj_m.particle_m.Draw(render);

            scene_m.snow_bg.FlontDraw(render);

            Vector2 band_pos_s = new Vector2(-300, 20);
            Vector2 band_pos_l = new Vector2(0, 20);
            float u;
            float t = (band_easing_t / easing_t) - 1f;
            u = t * t * t * t * t + 1f;
            Vector2 band_pos = Vector2.Lerp(band_pos_s, band_pos_l, u);
            if (band_pos.X - band_pos_l.X == 0)
            {
                is_count = true;
            }
            render.Draw(
                "band",
                band_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            band_pos = Vector2.Lerp(new Vector2(-290, 25), new Vector2(10, 25), u);
            render.Draw(
                "watch",
                band_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));
            band_pos = Vector2.Lerp(new Vector2(-240, 30), new Vector2(60, 30), u);
            render.DrawNumber(
                (float)scene_m.game_timer / 60,
                "0.00",
                "number",
                band_pos,
                60,
                70,
                0.28f);
            Vector2 s_pos = new Vector2(140, 37);
            if ((float)scene_m.game_timer / 60 >= 10.0f)
            {
                s_pos = new Vector2(160, 37);
            }
            if ((float)scene_m.game_timer / 60 >= 100.0f)
            {
                s_pos = new Vector2(180, 37);
            }
            band_pos = Vector2.Lerp(s_pos - new Vector2(300, 0), s_pos, u);
            render.Draw(
                "s",
                band_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.6f));

            band_pos_s = new Vector2(-300, 70);
            band_pos = Vector2.Lerp(band_pos_s, new Vector2(0, 70), u);
            render.Draw(
                "band_s",
                band_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            band_pos = Vector2.Lerp(new Vector2(-300, 75), new Vector2(10, 75), u);
            render.Draw(
                "item",
                band_pos / render.c_pos,
                new Rectangle(0, 0, 44, 40),
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));
            band_pos = Vector2.Lerp(new Vector2(-240, 84), new Vector2(60, 84), u);
            render.Draw(
                "kakeru",
                band_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.5f));
            band_pos = Vector2.Lerp(new Vector2(-220, 80), new Vector2(80, 80), u);
            render.DrawNumber(
                scene_m.game_obj_m.item_counter,
                "0",
                "number",
                band_pos,
                60,
                70,
                0.28f);

            Vector2 band_2_pos_s = new Vector2(Define.SCREEN_WIDTH + 300, Define.SCREEN_HEIGHT - 20 - 10);
            Vector2 band_2_pos_l = new Vector2(Define.SCREEN_WIDTH - 206, Define.SCREEN_HEIGHT - 20 - 10);
            Vector2 band_2_pos = Vector2.Lerp(band_2_pos_s, band_2_pos_l, u);
            render.Draw(
                "band_2",
                band_2_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            render.Draw(
                "band_2",
                (band_2_pos - new Vector2(0, 30)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            render.Draw(
                "how_to",
                (band_2_pos - new Vector2(-40, 33)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));
            render.Draw(
                "start",
                (band_2_pos - new Vector2(-40, 3)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));

            render.Draw(
                "black_pixel",
                Vector2.Zero,
                null,
                render.color * fade_alpha,
                render.rotation,
                render.origin,
                new Vector2(Define.SCREEN_WIDTH, Define.SCREEN_HEIGHT));

            Color non_color = new Color(100, 100, 100);
            Vector2 origin = new Vector2(252 / 2, 48 / 2);
            Vector2 non_scale = new Vector2(0.5f);

            if (is_pause)
            {
                for (int i = 0; i < menu_name.Count; i++)
                {
                    if ((int)menu == i)
                    {
                        render.Draw(
                            menu_name[(PauseMenu)i],
                            new Vector2(Define.SCREEN_WIDTH / 2, Define.SCREEN_HEIGHT / 2 - 20) + new Vector2(0, 70 * i),
                            null,
                            render.color,
                            render.rotation,
                            origin,
                            new Vector2(0.7f));

                        for (int j = 0; j < counter; j++)
                        {
                            int x = 1;
                            int y = 1;

                            for (int k = 0; k < 4; k++)
                            {
                                x *= -1;
                                y *= x * -1;

                                for (int l = 0; l < 3; l++)
                                {
                                    render.Draw(
                                        "w_pixel",
                                        new Vector2(Define.SCREEN_WIDTH / 2, Define.SCREEN_HEIGHT / 2 - 20) + new Vector2(-j * x, 70 * i - 30 * y + l),
                                        null,
                                        render.color * (1 - ((float)j / 180)),
                                        render.rotation,
                                        render.origin,
                                        render.scale);
                                }
                            }
                        }
                        continue;
                    }

                    render.Draw(
                        menu_name[(PauseMenu)i],
                        new Vector2(Define.SCREEN_WIDTH / 2, Define.SCREEN_HEIGHT / 2 - 20) + new Vector2(0, 70 * i),
                        null,
                        non_color,
                        render.rotation,
                        origin,
                        non_scale);

                    render.Draw(
                        "kettei",
                        new Vector2(720, 480),
                        Vector2.Zero);

                    render.Draw(
                        "pause",
                        new Vector2(Define.SCREEN_WIDTH / 2, 150),
                        new Vector2(270 / 2, 60 / 2));
                }
            }

            if (title_flag)
            {
                for (int y = 0; y < Define.SCREEN_HEIGHT / 100; y++)
                {
                    for (int x = 0; x < Define.SCREEN_WIDTH / 100; x++)
                    {
                        float scale = fade_scale - (float)x / 3 - (float)y / 3;
                        if (scale <= 0)
                        {
                            scale = 0;
                        }
                        if (scale >= 4)
                        {
                            scale = 4;
                        }

                        //scale = 1.0f;
                        render.Draw(
                            "fade",
                            new Vector2(x * 150, y * 150) / render.c_pos,
                            null,
                            render.color,
                            render.rotation,
                            new Vector2(100 / 2),
                            new Vector2(scale));
                    }
                }
            }
        }
    }
}
