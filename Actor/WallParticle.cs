using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    class WallParticle : Particle
    {
        private int particle_num;
        private Vector2[] pos;

        public WallParticle(Vector2 position) : base(position) { }

        public override void Init()
        {
            width = height = 3;

            position = new Vector2(
                position.X + random.Next(-10, 0),
                position.Y + random.Next(-3, 3));

            particle_num = 15;
            pos = new Vector2[particle_num];
            for (int i = 0; i < particle_num; i++)
            {
                pos[i] = new Vector2(
                    position.X + random.Next(3) * width,
                    position.Y + random.Next(3) * height);
            }

            vel = new Vector2(
                0,
                -random.Next(3));
            speed = 0.2f;
            scale = 0.75f;
            alpha = random.Next(4) + 2;
            life_timer = new CountDownTimer(3);
        }

        public override void Update(GameTime game_time)
        {
            life_timer.Update(game_time);

            alpha -= 0.2f;
            if (alpha < 0)
            {
                life_timer.SetTime(0.0f);
            }

            for (int i = 0; i < particle_num; i++)
            {
                pos[i] += vel * speed;
            }
        }

        public override void Draw(Render render)
        {
            for (int i = 0; i < particle_num; i++)
            {
                render.Draw(
                    "smoke_p",
                    Camera.Position(pos[i]) + GameDevice.Instance().pos,
                    null,
                    render.color * alpha,
                    render.rotation,
                    new Vector2(width / 2, height / 2),
                    new Vector2(scale) * render.scale);
            }
        }

        public override void Hit(GameObject game_obj)
        {

        }

        public WallParticle(WallParticle wall_particle) : this(wall_particle.position) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new WallParticle(this);
        }
    }
}
