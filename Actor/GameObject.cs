using Game1.Device;
using Microsoft.Xna.Framework;
using System;

namespace Game1.Actor
{
    //衝突方向
    public enum Direction
    {
        Up, Down, Left, Right,
    }

    public enum State
    {
        Active, Pause, Dead,
    }

    //ゲームオブジェクト
    abstract class GameObject : ICloneable//複製できる
    {
        public Vector2 position { set; get; }

        public int width { set; get; }//幅
        public int height { set; get; }//高さ

        public State state { set; get; }

        public bool a_draw;

        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        public abstract object Clone();//ICloneableで必要

        public abstract void Init();

        public abstract void Update(GameTime game_time);

        public abstract void Draw(Render render);

        public abstract void Hit(GameObject game_obj);//衝突処理
        
        //矩形
        public virtual Rectangle Rectangle()
        {
            return new Rectangle
            {
                X = (int)position.X - width / 2,//左上のx座標
                Y = (int)position.Y - height / 2,//左上のy座標
                Width = width,//幅
                Height = height,//高さ
            };
        }

        //衝突判定
        public virtual bool IsCollision(GameObject game_obj)
        {
            //矩形
            Rectangle rect = Rectangle();
            Rectangle other_rect = game_obj.Rectangle();

            //衝突判定
            if (rect.X <= other_rect.X + other_rect.Width &&
                other_rect.X <= rect.X + rect.Width &&
                rect.Y <= other_rect.Y + other_rect.Height &&
                other_rect.Y <= rect.Y + rect.Height)
                return true;//衝突している

            return false;//衝突していない
        }
    }
}
