using Game1.Device;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    //棘ブロック
    class ThornBlock : GameObject
    {
        public int direction { set; get; }//棘の方向

        public ThornBlock(Vector2 position, int direction) : base(position)
        {
            this.direction = direction;

            width = Define.MAP_WIDTH;
            height = Define.MAP_HEIGHT;
        }

        public ThornBlock(ThornBlock thorn) : this(thorn.position, thorn.direction) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new ThornBlock(this);//コピー
        }

        public override void Init()
        {

        }

        public override void Update(GameTime game_time)
        {
            //何もしない
        }

        public override void Draw(Render render)
        {
            //ブロック描画
            render.Draw(
                "thorn",
                Camera.Position(position) + new Vector2(Define.MAP_WIDTH / 2, Define.MAP_HEIGHT / 2) + GameDevice.Instance().pos,
                null,
                render.color,
                MathHelper.ToRadians(90 * direction),
                new Vector2(Define.MAP_WIDTH / 2, Define.MAP_HEIGHT / 2),
                render.scale);
        }

        public override void Hit(GameObject game_obj)
        {
            //何もしない
        }
    }
}
