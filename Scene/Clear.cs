using Game1.Device;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1.Scene
{
    class Clear : Scene
    {
        private bool is_fade;
        private float fade_scale;

        private Display display;

        private float band_easing_t;
        private float easing_t;

        private int counter_count;
        private bool new_flag;

        private Vector2 rank_pos;

        public Clear(SceneManager scene_m) : base(scene_m)
        {
        }

        public override void Init()
        {
            next = SceneName.Title;
            is_end = false;
            is_fade = false;
            fade_scale = 0;
            display = new Display(true);

            //振動を設定
            display.is_wave = true;
            display.is_x = true;
            display.is_y = true;
            display.speed = 5.0f;

            band_easing_t = 0.0f;
            easing_t = 50;
            counter_count = 0;
            new_flag = false;

            //ランク
            if (-scene_m.game_timer >= -scene_m.time_1)
            {
                scene_m.time_3 = scene_m.time_2;
                scene_m.time_2 = scene_m.time_1;
                scene_m.time_1 = scene_m.game_timer;
                new_flag = true;
            }
            else if (-scene_m.game_timer >= -scene_m.time_2)
            {
                scene_m.time_3 = scene_m.time_2;
                scene_m.time_2 = scene_m.game_timer;
            }
            else if (-scene_m.game_timer >= -scene_m.time_3)
            {
                scene_m.time_3 = scene_m.game_timer;
            }
        }

        public override void Update(GameTime game_time)
        {
            Input.key_lock = false;

            if (Input.GetKeyDown(Keys.Z) ||
                Input.GetKeyDown(PlayerIndex.One, Buttons.A))
            {
                if (rank_pos.X - Define.SCREEN_WIDTH / 2 == 0)
                is_fade = true;
            }

            if (is_fade)
            {
                fade_scale += 0.1f;

                if (fade_scale >= 6.5f)
                {
                    is_end = true;
                    scene_m.title_fade = true;
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
                    }
                }
            }

            counter_count++;
            if (band_easing_t <= easing_t && counter_count >= 30)
            {
                band_easing_t++;
                counter_count = 30;
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
            scene_m.map.Draw(render);
            scene_m.game_obj_m.particle_m.Draw(render);
            scene_m.bg_map.AfterDraw(render);

            scene_m.snow_bg.FlontDraw(render);

            render.Draw(
                "black_pixel",
                Vector2.Zero,
                null,
                render.color * 0.5f,
                render.rotation,
                render.origin,
                new Vector2(Define.SCREEN_WIDTH, Define.SCREEN_HEIGHT));

            render.Draw(
                "clear",
                (new Vector2(Define.SCREEN_WIDTH / 2, 120)) / render.c_pos + display.Pos() * 5,
                null,
                render.color,
                render.rotation,
                new Vector2(335 / 2, 95 / 2),
                new Vector2(1));


            Vector2 time_pos_s = new Vector2(-207 - 500, 200);
            Vector2 time_pos_l = new Vector2(Define.SCREEN_WIDTH / 2 - 207, 200);
            Vector2 item_pos_s = new Vector2(-207 - 1500, 250);
            Vector2 item_pos_l = new Vector2(Define.SCREEN_WIDTH / 2 - 207, 250);
            Vector2 dead_pos_s = new Vector2(-207 - 2500, 300);
            Vector2 dead_pos_l = new Vector2(Define.SCREEN_WIDTH / 2 - 207, 300);

            float u;
            float t = (band_easing_t / easing_t) - 1f;
            u = t * t * t * t * t + 1f;

            Vector2 time_pos = Vector2.Lerp(time_pos_s, time_pos_l, u);

            render.Draw(
                "clear_time_band_s",
                time_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            render.Draw(
                "watch",
                (time_pos + new Vector2(130, 7)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));
            render.DrawNumber(
                (float)scene_m.game_timer / 60,
                "0.00",
                "number",
                time_pos + new Vector2(100, 7) + new Vector2(100, 5),
                60,
                70,
                0.3f);
            Vector2 s_pos = new Vector2(110, 5);
            if ((float)scene_m.game_timer / 60 >= 100.0f)
            {
                s_pos = new Vector2(130, 5);
            }
            render.Draw(
                "s",
                (time_pos + new Vector2(100, 7) + new Vector2(100, 5) + s_pos) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));

            Vector2 item_pos = Vector2.Lerp(item_pos_s, item_pos_l, u);

            render.Draw(
                "clear_time_band_s",
                item_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            render.Draw(
                "item",
                (item_pos + new Vector2(130, 7)) / render.c_pos,
                new Rectangle(0, 0, 44, 40),
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));
            render.Draw(
                "kakeru",
                (item_pos + new Vector2(100, 7) + new Vector2(100, 9)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.6f));
            render.DrawNumber(
                scene_m.game_obj_m.item_counter,
                "0",
                "number",
                item_pos + new Vector2(100, 7) + new Vector2(130, 5),
                60,
                70,
                0.3f);
            render.Draw(
                "slash",
                (item_pos + new Vector2(100, 7) + new Vector2(163, 5)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.45f));
            render.DrawNumber(
                8,
                "0",
                "number",
                item_pos + new Vector2(100, 7) + new Vector2(190, 5),
                60,
                70,
                0.3f);

            Vector2 dead_pos = Vector2.Lerp(dead_pos_s, dead_pos_l, u);
            render.Draw(
                "clear_time_band_s",
                dead_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            render.Draw(
                 "skull",
                 (dead_pos + new Vector2(130, 7)) / render.c_pos,
                 null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.9f));
            render.Draw(
                "kakeru",
                (dead_pos + new Vector2(100, 7) + new Vector2(100, 9)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.6f));
            render.DrawNumber(
                scene_m.dead_counter,
                "0",
                "number",
                dead_pos + new Vector2(100, 7) + new Vector2(130, 5),
                60,
                70,
                0.3f);

            Vector2 rank_pos_s = new Vector2(-242 - 100000, 460);
            Vector2 rank_pos_l = new Vector2(Define.SCREEN_WIDTH / 2 - 242, 460);
            rank_pos = Vector2.Lerp(rank_pos_s, rank_pos_l, u);
            render.Draw(
                "clear_time_band",
                rank_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            render.Draw(
                "1",
                (rank_pos + new Vector2(120, 8)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.6f));
            render.DrawNumber(
                scene_m.time_1 / 60,
                "0.00",
                "number",
                rank_pos + new Vector2(200, 10),
                60,
                70,
                0.3f);
            s_pos = new Vector2(110, 5);
            if ((float)scene_m.time_1 / 60 >= 100.0f)
            {
                s_pos = new Vector2(130, 5);
            }
            render.Draw(
                "s",
                (rank_pos + new Vector2(100, 7) + new Vector2(100, 5) + s_pos) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));

            if (new_flag)
            {
                render.Draw(
                    "new",
                    (rank_pos + new Vector2(500, 10)) / render.c_pos,
                    null,
                    render.color,
                    render.rotation,
                    render.origin,
                    new Vector2(0.5f));
            }

            render.Draw(
                "clear_time_band",
                (rank_pos + new Vector2(0, 50)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            render.Draw(
                "2",
                (rank_pos + new Vector2(120, 8) + new Vector2(0, 50)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.6f));
            render.DrawNumber(
                scene_m.time_2 / 60,
                "0.00",
                "number",
                rank_pos + new Vector2(0, 50) + new Vector2(200, 10),
                60,
                70,
                0.3f);
            s_pos = new Vector2(110, 5);
            if ((float)scene_m.time_2 / 60 >= 100.0f)
            {
                s_pos = new Vector2(130, 5);
            }
            render.Draw(
                "s",
                (rank_pos + new Vector2(100, 7) + new Vector2(100, 5) + s_pos + new Vector2(0, 50)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));

            render.Draw(
                "clear_time_band",
                (rank_pos + new Vector2(0, 100)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                Vector2.One);
            render.Draw(
                "3",
                (rank_pos + new Vector2(120, 8) + new Vector2(0, 100)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.6f));
            render.DrawNumber(
                scene_m.time_3 / 60,
                "0.00",
                "number",
                rank_pos + new Vector2(0, 100) + new Vector2(200, 10),
                60,
                70,
                0.3f);
            s_pos = new Vector2(110, 5);
            if ((float)scene_m.time_3 / 60 >= 100.0f)
            {
                s_pos = new Vector2(130, 5);
            }
            render.Draw(
                "s",
                (rank_pos + new Vector2(100, 7) + new Vector2(100, 5) + s_pos + new Vector2(0, 100)) / render.c_pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                new Vector2(0.8f));

            rank_pos = Vector2.Lerp(new Vector2(Define.SCREEN_WIDTH / 2 - 100000, 430), new Vector2(Define.SCREEN_WIDTH / 2, 430), u);
            render.Draw(
                "rank",
                rank_pos / render.c_pos,
                null,
                render.color,
                render.rotation,
                new Vector2(168 / 2, 50 / 2),
                new Vector2(0.8f));

            if (rank_pos.X - Define.SCREEN_WIDTH / 2 == 0)
            render.Draw(
                "syuuryou",
                new Vector2(1100, 680) / render.c_pos,
                null,
                render.color,
                render.rotation,
                Vector2.Zero,
                new Vector2(1));

            if (is_fade)
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
