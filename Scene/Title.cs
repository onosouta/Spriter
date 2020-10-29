using Game1.Actor;
using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1.Scene
{
    class Title : Scene
    {
        private float fade_scale;
        private Vector2 draw_pos;
        private Vector2 vel;
        private Timer move_timer;
        private int move_counter;

        private float band_easing_t;
        private float easing_t;
        private bool is_easing;

        public Title(SceneManager scene_m) : base(scene_m) { }

        public override void Init()
        {
            is_end = false;
            next = SceneName.Game;

            fade_scale = 9.0f;

            GameDevice.Instance().Render.scale = Vector2.One;
            GameDevice.Instance().Render.c_pos = 1;

            scene_m.game_obj_m.Init();

            scene_m.map.Unload();
            scene_m.map.Load(6);//マップ数
            scene_m.game_obj_m.Add(scene_m.map);
            scene_m.bg_map.Unload();
            scene_m.bg_map.Load(6);
            scene_m.game_obj_m.item_counter = 0;
            scene_m.map.current_map = 0;

            Camera.is_clear = false;
            Input.key_lock = false;

            scene_m.dead_counter = 0;

            draw_pos = Vector2.Zero;
            vel = new Vector2(0, -3);
            move_timer = new CountDownTimer(0.8f);
            move_counter = 1;

            band_easing_t = 0.0f;
            easing_t = 50;
            is_easing = false;
        }

        public override void Update(GameTime game_time)
        {
            move_timer.Update(game_time);
            if (move_timer.IsTime())
            {
                move_counter++;
                if (move_counter >= 2)
                {
                    move_counter = 0;
                    vel.Y *= -1;
                }

                draw_pos += vel;
                move_timer.SetTime(0.8f);
            }

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

            if (scene_m.title_fade)
            {
                fade_scale -= 0.08f;

                if (fade_scale <= 0)
                {
                    fade_scale = 0;
                }
            }
            else fade_scale = 0.0f;

            //カメラ位置設定
            Camera.position =
                new Vector2(5 * Define.MAP_WIDTH, 70 * Define.MAP_HEIGHT) -
                new Vector2(Define.SCREEN_WIDTH / 2, Define.SCREEN_HEIGHT / 2);
            Camera.position = Vector2.Clamp(Camera.position, Camera.min, Camera.max);//範囲指定

            if (fade_scale == 0)
            {
                //終了
                if (Input.GetKeyDown(Keys.Z) ||
                    Input.GetKeyDown(PlayerIndex.One, Buttons.A))
                {
                    is_easing = true;
                }
            }

            if (is_easing && band_easing_t <= easing_t)
            {
                band_easing_t++;
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
            scene_m.bg_map.AfterDraw(render);
            scene_m.map.Draw(render);

            Vector2 title_pos;
            Vector2 press_pos;
            Vector2 a_pos;
            if (is_easing)
            {
                float s = 1.70158f;
                float t = band_easing_t / easing_t;
                float u = t * t * ((s + 1f) * t - s);

                title_pos = Vector2.Lerp(new Vector2(Define.SCREEN_WIDTH / 2, 300), new Vector2(Define.SCREEN_WIDTH / 2, -500), u);
                if (title_pos.Y - -500 == 0)
                {
                    is_end = true;
                }
                
                press_pos = Vector2.Lerp(new Vector2(Define.SCREEN_WIDTH / 2 - 164, 450), new Vector2(Define.SCREEN_WIDTH / 2 - 164, -350), u);
                a_pos = Vector2.Lerp(new Vector2(Define.SCREEN_WIDTH / 2 + 98, 450), new Vector2(Define.SCREEN_WIDTH / 2 + 98, -350), u);
            }
            else
            {
                title_pos = new Vector2(Define.SCREEN_WIDTH / 2, 300) + draw_pos;
                press_pos = new Vector2(Define.SCREEN_WIDTH / 2 - 164, 450) + draw_pos;
                a_pos = new Vector2(Define.SCREEN_WIDTH / 2 + 98, 450) + draw_pos;
            }

            render.Draw(
                "title",
                title_pos,
                new Vector2(840 / 2, 176 / 2));

            render.Draw(
                "press",
                press_pos,
                render.origin);
            render.Draw(
                "a",
                a_pos,
                render.origin);

            scene_m.snow_bg.FlontDraw(render);

            if (scene_m.title_fade)
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
                            new Vector2(x * 150, y * 150),
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
