using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Actor
{
    class ClearBlock : GameObject
    {
        private GameObjectManager game_obj_m;
        private ClearBlockParticle particle;

        private Vector2 velocity;
        private Timer move_timer;
        private int move_counter;

        private bool is_hit;

        private Motion motion;
        private int r, g, b;
        private int color_counter;

        public ClearBlock(Vector2 position, GameObjectManager game_obj_m) : base(position)
        {
            this.game_obj_m = game_obj_m;
        }

        public override void Init()
        {
            width = 40;
            height = 44;
            
            velocity = new Vector2(0, -3);
            move_timer = new CountDownTimer(0.5f);
            move_counter = 1;
            
            motion = new Motion(new Range(0, 5), new CountDownTimer(0.06f));
            motion.Add(0, new Rectangle(0 * 44, 0, 44, 40));
            motion.Add(1, new Rectangle(1 * 44, 0, 44, 40));
            motion.Add(2, new Rectangle(2 * 44, 0, 44, 40));
            motion.Add(3, new Rectangle(3 * 44, 0, 44, 40));
            motion.Add(4, new Rectangle(4 * 44, 0, 44, 40));
            motion.Add(5, new Rectangle(5 * 44, 0, 44, 40));

            is_hit = false;
            r = 255;
            g = b = 0;
            color_counter = 0;

            particle = new ClearBlockParticle(position + new Vector2(Define.MAP_WIDTH / 2, Define.MAP_HEIGHT / 2), game_obj_m);
            particle.Init();
        }

        public override void Update(GameTime game_time)
        {
            //上下移動
            move_timer.Update(game_time);
            if (move_timer.IsTime())
            {
                move_counter++;
                if (move_counter >= 2)
                {
                    move_counter = 0;
                    velocity.Y *= -1;
                }

                position += velocity;
                move_timer.SetTime(0.5f);
            }

            //衝突したらアニメーション
            if (is_hit)
            {
                motion.Update(game_time);
            }

            //色設定
            int add = 2;
            if (color_counter == 0)
            {
                r = 255;
                g += add;
                b = 0;
                if (g >= 255) color_counter++;
            }
            if (color_counter == 1)
            {
                r -= add;
                g = 255;
                b = 0;
                if (r <= 0) color_counter++;
            }
            if (color_counter == 2)
            {
                r = 0;
                g = 255;
                b += add;
                if (b >= 255) color_counter++;
            }
            if (color_counter == 3)
            {
                r = 0;
                g -= add;
                b = 255;
                if (g <= 0) color_counter++;
            }
            if (color_counter == 4)
            {
                r += add;
                g = 0;
                b = 255;
                if (r >= 255) color_counter++;
            }
            if (color_counter == 5)
            {
                r = 255;
                g = 0;
                b -= add;
                if (b <= 0) color_counter = 0;
            }

            particle.Update(game_time);
        }

        public override void Draw(Render render)
        {
            render.Draw(
                "item",
                Camera.Position(position) + GameDevice.Instance().pos + new Vector2(Define.MAP_WIDTH / 2, Define.MAP_HEIGHT / 2),
                motion.DrawRectangle(),
                new Color(r, g, b),
                render.rotation,
                new Vector2(width / 2, height / 2),
                new Vector2(1.5f) * render.scale);

            particle.Draw(render);
        }

        public override void Hit(GameObject game_obj)
        {
            //画面揺れ
            Display display = GameDevice.Instance().Display;
            display.is_wave = true;

            //画面揺れ設定
            display.is_x = true;//x方向に揺れるか
            display.is_y = true;//y方向に揺れるか
            display.speed = 16.0f;//揺れ幅

            is_hit = true;
        }

        public ClearBlock(ClearBlock clear_block) : this(clear_block.position, clear_block.game_obj_m) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new ClearBlock(this);
        }
    }
}
