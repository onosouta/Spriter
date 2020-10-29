using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    class ItemParticleEmitter : Emitter
    {
        public ItemParticleEmitter(GameObjectManager game_obj_m, Vector2 generate_pos, int particle_num)
            : base(game_obj_m, generate_pos, particle_num)
        { }

        public override void Update(GameTime game_time)
        {
            for (int i = 0; i < particle_num; i++)
            {
                game_obj_m.particle_m.Add(new ItemParticle(generate_pos, game_obj_m));
            }

            is_dead = true;
        }
    }
}
