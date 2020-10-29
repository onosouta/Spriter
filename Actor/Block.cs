using Game1.Device;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    //ブロック
    class Block : GameObject
    {
        private int chip_num;//マップチップの番号

        public Block(Vector2 position, int chip_num) : base(position)
        {
            this.chip_num = chip_num;
        }

        public Block(Block block) : this(block.position, block.chip_num) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new Block(this);//コピー
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
            Rectangle draw_rect;

            //マップチップの描画する範囲を指定
            draw_rect = new Rectangle(
                Define.MAP_WIDTH * (chip_num % 3),
                Define.MAP_HEIGHT * (chip_num / 3),
                Define.MAP_WIDTH,
                Define.MAP_HEIGHT);

            //ブロック描画
            render.Draw(
                "block",
                Camera.Position(position) + GameDevice.Instance().pos,
                draw_rect,
                render.color,
                render.rotation,
                render.origin,
                render.scale);
        }

        public override void Hit(GameObject game_obj)
        {
            //何もしない
        }
    }
}
