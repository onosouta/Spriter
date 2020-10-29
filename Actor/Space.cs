using Game1.Device;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    //スペース
    class Space : GameObject
    {
        public Space(Vector2 position) : base(position) { }

        public Space(Space space) : this(space.position) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new Space(this);//コピー
        }

        public override void Init()
        {
            width = Define.MAP_WIDTH;
            height = Define.MAP_HEIGHT;
        }

        public override void Update(GameTime game_time)
        {
            //何もしない
        }

        public override void Draw(Render render)
        {
            //何もしない
        }

        public override void Hit(GameObject game_obj)
        {
            //何もしない
        }
    }
}
