using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    class ClearParticleEmitter : Emitter
    {
        private string name;
        private int width, height;

        public ClearParticleEmitter(GameObjectManager game_obj_m, Vector2 generate_pos, int particle_num, string name, int width, int height)
            : base(game_obj_m, generate_pos, particle_num)
        {
            this.name = name;
            this.width = width;
            this.height = height;
        }

        public override void Update(GameTime game_time)
        {
            if (!is_dead)
            {
                for (int i = 0; i < particle_num; i++)
                {
                    game_obj_m.particle_m.Add(new ClearParticle(generate_pos, game_obj_m, name, width, height));
                }
            }

            is_dead = true;
        }
    }
}
