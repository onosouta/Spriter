using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Actor
{
    class ClearBlockParticle : Particle
    {
        struct Particles
        {
            public Vector2 position;
            public Color color;
            public Motion motion;
            public bool is_dead;
        }

        private GameObjectManager game_obj_m;
        public bool owner_dead { set; get; }

        private List<Particles> particles;

        private int r, g, b;

        public ClearBlockParticle(Vector2 position, GameObjectManager game_obj_m) : base(position)
        {
            this.game_obj_m = game_obj_m;
        }

        public override void Init()
        {
            position = position + new Vector2(
                GameDevice.Instance().Random.Next(-20, 20),
                GameDevice.Instance().Random.Next(-20, 20));
            vel = new Vector2(
                0,
                0);
            speed = (float)random.Next(5, 10) / 10;
            scale = 1;
            alpha = 2.0f;
            life_timer = new CountDownTimer(0.5f);//プレイヤーの復活時間

            width = height = 20;

            particles = new List<Particles>();
            for (int i = 0; i < 3; i++)
            {
                Particles p = new Particles();

                Vector2 pos = position + new Vector2(
                    GameDevice.Instance().Random.Next(-40, 40),
                    GameDevice.Instance().Random.Next(-40, 40));
                p.position = pos;

                do
                {
                    r = GameDevice.Instance().Random.Next(2);
                    g = GameDevice.Instance().Random.Next(2);
                    b = GameDevice.Instance().Random.Next(2);
                } while ((r == g) && (g == b) && (b == r));

                p.color = new Color(r * 255, g * 255, b * 255);

                p.motion = new Motion(new Range(0, 9), new CountDownTimer(0.15f));
                p.motion.Add(0, new Rectangle(width * 0, 0, width, height));
                p.motion.Add(1, new Rectangle(width * 0, 0, width, height));
                p.motion.Add(2, new Rectangle(width * 1, 0, width, height));
                p.motion.Add(3, new Rectangle(width * 1, 0, width, height));
                p.motion.Add(4, new Rectangle(width * 2, 0, width, height));
                p.motion.Add(5, new Rectangle(width * 2, 0, width, height));
                p.motion.Add(6, new Rectangle(width * 3, 0, width, height));
                p.motion.Add(7, new Rectangle(width * 3, 0, width, height));
                p.motion.Add(8, new Rectangle(width * 4, 0, width, height));
                p.motion.Add(9, new Rectangle(width * 4, 0, width, height));
                p.motion.cur_num = random.Next(0, 8);

                p.is_dead = false;

                particles.Add(p);
            }

            owner_dead = false;
        }

        public override void Update(GameTime game_time)
        {
            for (int i = 0; i < 3; i++)
            {
                Particles p = particles[i];
                p.motion.Update(game_time);
                if (p.motion.cur_num == 9)
                {
                    p.is_dead = true;
                }
                if (p.is_dead)
                {
                    Particles new_p = new Particles();

                    Vector2 pos = position + new Vector2(
                        GameDevice.Instance().Random.Next(-40, 40),
                        GameDevice.Instance().Random.Next(-40, 40));
                    new_p.position = pos;

                    do
                    {
                        r = GameDevice.Instance().Random.Next(2);
                        g = GameDevice.Instance().Random.Next(2);
                        b = GameDevice.Instance().Random.Next(2);
                    } while ((r == g) && (g == b) && (b == r));

                    new_p.color = new Color(r * 255, g * 255, b * 255);

                    new_p.motion = new Motion(new Range(0, 9), new CountDownTimer(0.08f));
                    new_p.motion.Add(0, new Rectangle(width * 0, 0, width, height));
                    new_p.motion.Add(1, new Rectangle(width * 0, 0, width, height));
                    new_p.motion.Add(2, new Rectangle(width * 1, 0, width, height));
                    new_p.motion.Add(3, new Rectangle(width * 1, 0, width, height));
                    new_p.motion.Add(4, new Rectangle(width * 2, 0, width, height));
                    new_p.motion.Add(5, new Rectangle(width * 2, 0, width, height));
                    new_p.motion.Add(6, new Rectangle(width * 3, 0, width, height));
                    new_p.motion.Add(7, new Rectangle(width * 3, 0, width, height));
                    new_p.motion.Add(8, new Rectangle(width * 4, 0, width, height));
                    new_p.motion.Add(9, new Rectangle(width * 4, 0, width, height));

                    p.is_dead = false;

                    particles[i] = new_p;
                }
            }
        }

        public override void Draw(Render render)
        {
            if (!owner_dead)
            {
                for (int i = 0; i < 3; i++)
                {
                    render.Draw(
                        "bg_2",
                        Camera.Position(particles[i].position) + GameDevice.Instance().pos,
                        particles[i].motion.DrawRectangle(),
                        particles[i].color * alpha,
                        render.rotation,
                        new Vector2(width / 2, height / 2),
                        new Vector2(scale) * render.scale);
                }
            }
        }

        public override void Hit(GameObject game_obj)
        {

        }

        public ClearBlockParticle(ClearBlockParticle clear_block_particle) : this(clear_block_particle.position, clear_block_particle.game_obj_m) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new ClearBlockParticle(this);
        }
    }
}
