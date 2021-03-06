﻿using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    //煙パーティクルエミッター
    class HitParticleEmitter : Emitter
    {
        public HitParticleEmitter(GameObjectManager game_obj_m, Vector2 generate_pos, int particle_num)
            : base(game_obj_m, generate_pos, particle_num)
        { }

        public override void Update(GameTime game_time)
        {
            for (int i = 0; i < particle_num; i++)
            {
                game_obj_m.particle_m.Add(new SmokeParticle(generate_pos));
            }

            is_dead = true;
        }
    }
}