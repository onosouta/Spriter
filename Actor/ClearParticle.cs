using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using System;

namespace Game1.Actor
{
    class ClearParticle : Particle
    {
        private GameObjectManager game_obj_m;
        private Map map;
        private string name;
        private float rotation;
        private float rotation_radian;
        private float speed_radian;
        private float radian;
        private Vector2 scale_v;

        public ClearParticle(Vector2 position, GameObjectManager game_obj_m, string name, int width, int height) : base(position)
        {
            this.game_obj_m = game_obj_m;
            this.name = name;
            this.width = width;
            this.height = height;
        }

        public override void Init()
        {
            radian = (float)random.Next(360) / 180 * (float)Math.PI;
            speed_radian = 0;

            vel = new Vector2(
                (float)Math.Cos(radian) * (float)Math.Cos(speed_radian),
                (float)Math.Sin(radian) * (float)Math.Cos(speed_radian));
            position = position + vel * 80;
            speed = (float)random.Next(10, 60) / 10;
            scale_v = new Vector2(0.5f, 0.01f);
            alpha = 1.0f;
            life_timer = new CountDownTimer(1.0f);//プレイヤーの復活時間

            rotation = (float)random.Next(360) / 180 * (float)Math.PI;
            rotation_radian = (float)random.Next(-5, 5) / 180 * (float)Math.PI;
        }

        public override void Update(GameTime game_time)
        {
            speed_radian += 0.5f / 180 * (float)Math.PI;
            if (Math.Cos(speed_radian) <= 0)
            {
                Player player;
                if (game_obj_m.Player().position != null)
                {
                    player = game_obj_m.Player();
                }
                else
                {
                    player = null;
                }
                //position = Vector2.Lerp(position, player.position + new Vector2(player.Rectangle().Width / 2, player.Rectangle().Height / 2), 0.2f);
                //alpha -= 0.2f;
            }
            else
            {
                //scale_v.X = 0.2f + (float)Math.Cos(speed_radian) / ((7.4f - speed));
                if (scale_v.X <= 0.2f)
                {
                    scale_v.X = 0.2f;
                }

                vel = new Vector2(
                    (float)Math.Cos(radian) * (float)Math.Cos(speed_radian),
                    (float)Math.Sin(radian) * (float)Math.Cos(speed_radian));

                position += vel * speed;
            }
            rotation += rotation_radian;
        }

        public override void Draw(Render render)
        {
            render.Draw(
                name,
                Camera.Position(position) + GameDevice.Instance().pos,
                null,
                render.color * alpha,
                radian,
                new Vector2(width / 2, height / 2),
                scale_v * render.scale);
        }

        public override void Hit(GameObject game_obj)
        {

        }

        public ClearParticle(ClearParticle clear_particle) : this(clear_particle.position, clear_particle.game_obj_m, clear_particle.name, clear_particle.width, clear_particle.height) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new ClearParticle(this);
        }
    }
}
