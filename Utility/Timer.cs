using Microsoft.Xna.Framework;

namespace Game1.Utility
{
    //タイマー
    abstract class Timer
    {
        protected float lim_time;//制限時間
        protected float cur_time;//現在の時間

        public Timer(float sec)
        {
            lim_time = 60.0f * sec;//60FPS×秒数
            Init();
        }

        public abstract void Init();

        public abstract void Update(GameTime game_time);

        public abstract bool IsTime();//制限時間になったか

        public abstract float Rate();//残り時間の割合
        
        public void SetTime(float sec)
        {
            lim_time = 60.0f * sec;
            Init();
        }
        
        public float GetTime()
        {
            return cur_time / 60.0f;
        }
    }
}
