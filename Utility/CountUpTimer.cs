using Microsoft.Xna.Framework;
using System;

namespace Game1.Utility
{
    class CountUpTimer : Timer
    {
        public CountUpTimer(float sec) : base(sec) { }

        public override void Init()
        {
            cur_time = 0;
        }

        public override void Update(GameTime game_time)
        {
            cur_time = Math.Min(cur_time + 1.0f, lim_time);
        }

        public override bool IsTime()
        {
            return cur_time >= lim_time;
        }

        public override float Rate()
        {
            return cur_time / lim_time;
        }
    }
}
