using Game1.Device;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Actor
{
    //パーティクル管理
    class ParticleManager
    {
        private List<Particle> particles;//パーティクル

        public ParticleManager()
        {
            InitList(ref particles);
        }

        //リストを初期化
        public void InitList(ref List<Particle> particle)
        {
            if (particle != null)
            {
                particle.Clear();
            }
            else
            {
                particle = new List<Particle>();
            }
        }

        //ゲームオブジェクトを追加
        public void Add(Particle particle)
        {
            if (particle == null)
            {
                return;
            }

            particle.Init();//初期化して追加
            particles.Add(particle);
        }

        //更新
        public void Update(GameTime game_time)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].state == State.Active)
                {
                    particles[i].Update(game_time);

                    //パーティクル削除
                    if (particles[i].life_timer.IsTime())
                    {
                        particles.Remove(particles[i]);
                    }
                }
            }
        }

        //描画
        public void Draw(Render render)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(render);
            }
        }

        public void ParticleActive()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].state = State.Active;
            }
        }

        public void ParticlePause()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].state = State.Pause;
            }
        }
    }
}
