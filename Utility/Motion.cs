using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Utility
{
    //アニメーション
    class Motion
    {
        private Dictionary<int, Rectangle> rect;//アニメ管理
        public int cur_num { set; get; }//現在のアニメ

        private Range range;//アニメの範囲
        private Timer timer;//アニメのフレームレート

        public Motion(Range range, Timer timer)
        {
            this.range = range;
            this.timer = timer;

            rect = new Dictionary<int, Rectangle>();
            cur_num = range.first;
        }

        //アニメ追加
        public void Add(int key, Rectangle rectangle)
        {
            if (rect.ContainsKey(key))//追加済み
            {
                return;//終了
            }

            rect.Add(key, rectangle);//追加
        }

        public void Update(GameTime game_time)
        {
            timer.Update(game_time);

            if (timer.IsTime())
            {
                timer.Init();

                cur_num++;//アニメ切り替え

                if (!range.IsRange(cur_num))
                {
                    cur_num = range.first;
                }
            }
        }

        //描画範囲
        public Rectangle DrawRectangle()
        {
            return rect[cur_num];
        }
    }
}
