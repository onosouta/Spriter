using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    class DushParticleEmitter : Emitter
    {
        private Vector2 particle_vel;

        public DushParticleEmitter(GameObjectManager game_obj_m, Vector2 generate_pos, int particle_num, Vector2 particle_vel)
            : base(game_obj_m, generate_pos, particle_num)
        {
            this.particle_vel = particle_vel;
        }

        public override void Update(GameTime game_time)
        {
            for (int i = 0; i < particle_num; i++)
            {
                game_obj_m.particle_m.Add(new DushParticle(generate_pos, particle_vel));
            }

            is_dead = true;
        }
    }
}
