using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using System;

namespace Game1.Actor
{
    //パーティクル
    abstract class Particle : GameObject
    {
        protected Vector2 vel;//速度
        protected float speed;//速さ
        protected float scale;//スケール
        protected float alpha;//アルファ
        public Timer life_timer { set; get; }//寿命

        //public State state { set; get; }//状態

        protected Random random;//ランダム

        public Particle(Vector2 position) : base(position)
        {
            this.position = position;

            random = GameDevice.Instance().Random;

            state = State.Active;
        }
    }
}