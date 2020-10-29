using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    //衝突パーティクル
    class SmokeParticle : Particle
    {
        private int particle_num;
        private Vector2[] pos;

        public SmokeParticle(Vector2 position) : base(position) { }

        public override void Init()
        {
            width = height = 3;

            position = new Vector2(
                position.X + random.Next(-20, 20),
                position.Y + random.Next(-5, 5));

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

        public SmokeParticle(SmokeParticle hit_particle) : this(hit_particle.position) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new SmokeParticle(this);
        }
    }
}