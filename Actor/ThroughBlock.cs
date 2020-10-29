using Game1.Device;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    //すり抜けブロック
    class ThroughBlock : GameObject
    {
        private string name;//名前

        public ThroughBlock(Vector2 position, string name) : base(position)
        {
            this.name = name;
        }

        public ThroughBlock(ThroughBlock slip_through) : this(slip_through.position, slip_through.name) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new ThroughBlock(this);//コピー
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
            //ブロック描画
            render.Draw(
                name,
                Camera.Position(position) + GameDevice.Instance().pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                render.scale);
        }

        public override Rectangle Rectangle()
        {
            return new Rectangle
            {
                X = (int)position.X - width / 2,//左上のx座標
                Y = (int)position.Y - height / 2,//左上のy座標
                Width = width,//幅
                Height = 5,//高さ
            };
        }

        public override void Hit(GameObject game_obj)
        {
            //何もしない
        }
    }
}
