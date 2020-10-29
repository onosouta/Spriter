using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    //エミッター
    abstract class Emitter
    {
        protected GameObjectManager game_obj_m;//ゲームオブジェクト管理

        protected Vector2 generate_pos;//パーティクルを生成する座標
        protected int particle_num;//パーティクルの数
        public bool is_dead { set; get; }//寿命

        public Emitter(GameObjectManager game_obj_m, Vector2 generate_pos, int particle_num)
        {
            this.game_obj_m = game_obj_m;
            this.generate_pos = generate_pos;
            this.particle_num = particle_num;

            is_dead = false;
        }

        public abstract void Update(GameTime game_time);
    }
}
