using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1.Device
{
    //揺れ
    class Display
    {
        public bool is_x { set; get; }
        public bool is_y { set; get; }

        public bool is_wave { set; private get; }

        private Vector2 velocity;
        private int angle;

        private bool is_set;//揺れを小さくするか

        public Display(bool is_set)
        {
            this.is_set = is_set;

            Init();
        }

        //初期化
        private void Init()
        {
            is_wave = false;
            
            velocity = Vector2.Zero;
            angle = 0;
            speed = 0.0f;
        }
        
        public Vector2 Pos()
        {
            if (is_wave)
            {
                SetVelocity();

                if (is_set) GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);//振動
            }

            if (speed > 0)
            {
                angle += 360 / 4;//三角関数で揺らす
                if (angle % 360 == 0)
                {
                    SetVelocity();
                    if (is_set) speed -= 0.75f;//減速
                }
                
                if (speed < 2.0f)
                {
                    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                }
            }
            else
            {
                speed = 0;
            }

            float x = 0.0f;
            float y = 0.0f;
            //x成分
            if (is_x) x = velocity.X * (float)Math.Sin(MathHelper.ToRadians(angle)) * speed;
            //y成分
            if (is_y) y = velocity.Y * (float)Math.Sin(MathHelper.ToRadians(angle)) * speed;
            
            is_wave = false;

            return new Vector2(x, y);
        }

        //ベクトルを設定
        private void SetVelocity()
        {
            float radian = MathHelper.ToRadians(GameDevice.Instance().Random.Next(360));
            if (!is_x) velocity.Y = 1;//x成分は0
            if (!is_y) velocity.X = 1;//y成分は0
            if (is_x && is_y)
            {
                velocity = new Vector2(
                    (float)Math.Cos(radian),
                    (float)Math.Sin(radian));
            }
            angle = 0;
        }

        public float speed { set; private get; }
    }
}
