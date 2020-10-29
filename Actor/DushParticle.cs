using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    class DushParticle : Particle
    {
        public DushParticle(Vector2 position, Vector2 vel) : base(position)
        {
            this.vel = vel;
        }

        public override void Init()
        {
            width = height = 3;

            position = new Vector2(
                position.X + random.Next(-10, 10),
                position.Y + random.Next(-10, 10));

            speed = (float)random.Next(50, 100) / 100;
            scale = (float)random.Next(5, 15) / 10;
            alpha = (float)random.Next(5) / 10 + 0.5f;
            life_timer = new CountDownTimer(5);
        }

        public override void Update(GameTime game_time)
        {
            life_timer.Update(game_time);

            alpha -= 0.02f;
            if (alpha < 0)
            {
                life_timer.SetTime(0.0f);
            }

            position += vel * speed;
        }

        public override void Draw(Render render)
        {
            render.Draw(
                "dush_p",
                Camera.Position(position) + GameDevice.Instance().pos,
                null,
                render.color * alpha,
                render.rotation,
                new Vector2(width / 2, height / 2),
                new Vector2(scale) * render.scale);
        }

        public override void Hit(GameObject game_obj)
        {

        }

        public DushParticle(DushParticle dush_particle) : this(dush_particle.position, dush_particle.vel) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new DushParticle(this);
        }
    }
}
