using Game1.Device;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    class BgBlock : GameObject
    {
        private int chip_num;//マップチップの番号

        public BgBlock(Vector2 position, int chip_num) : base(position)
        {
            this.chip_num = chip_num;
            if (chip_num >= 10) a_draw = true;
        }

        public BgBlock(BgBlock bg_block) : this(bg_block.position, bg_block.chip_num) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new BgBlock(this);//コピー
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

            if (chip_num < 10)
            {
                //マップチップの描画する範囲を指定
                draw_rect = new Rectangle(
                    Define.MAP_WIDTH * (chip_num % 3),
                    Define.MAP_HEIGHT * (chip_num / 3),
                    Define.MAP_WIDTH,
                    Define.MAP_HEIGHT);

                //ブロック描画
                render.Draw(
                    "bg_block",
                    Camera.Position(position) + GameDevice.Instance().pos,
                    draw_rect,
                    render.color,
                    render.rotation,
                    render.origin,
                    render.scale);
            }
            else if (chip_num < 20)
            {
                int num = chip_num - 10;

                //マップチップの描画する範囲を指定
                draw_rect = new Rectangle(
                    Define.MAP_WIDTH * (num % 3),
                    Define.MAP_HEIGHT * (num / 3),
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
        }

        public override void Hit(GameObject game_obj)
        {
            //何もしない
        }
    }
}
