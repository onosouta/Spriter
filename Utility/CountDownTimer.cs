using Microsoft.Xna.Framework;
using System;

namespace Game1.Utility
{
    class CountDownTimer : Timer
    {
        public CountDownTimer(float sec) : base(sec) { }

        public override void Init()
        {
            cur_time = lim_time;
        }

        public override void Update(GameTime game_time)
        {
            cur_time = Math.Max(cur_time - 1.0f, 0);
        }

        public override bool IsTime()
        {
            return cur_time <= 0.0f;
        }

        public override float Rate()
        {
            return 1.0f - cur_time / lim_time;
        }
    }
}
