using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    //消えるブロック
    class BreakBlock : GameObject
    {
        private Map map;

        private bool is_hit;//衝突したか
        private Timer apper_timer;//出現する時間
        private Timer disapper_timer;//消える時間

        private Display display;//ブロックの振動

        public BreakBlock(Vector2 position, Map map) : base(position)
        {
            this.map = map;
        }

        public BreakBlock(BreakBlock disapper) : this(disapper.position, disapper.map) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new BreakBlock(this);//コピー
        }

        public override void Init()
        {
            width = Define.MAP_WIDTH;
            height = Define.MAP_HEIGHT;

            is_hit = false;
            apper_timer = new CountDownTimer(2.0f);
            disapper_timer = new CountDownTimer(1.3f);
            display = new Display(false);
        }

        public override void Update(GameTime game_time)
        {
            //消える処理
            if (is_hit)
            {
                disapper_timer.Update(game_time);
                if (disapper_timer.IsTime())
                {
                    //揺らさない
                    display.is_wave = false;
                    display.is_x = false;
                    display.is_y = false;
                    display.speed = 0.0f;
                }
                if (disapper_timer.GetTime() < 0.5f)
                {
                    state = State.Pause;
                }
            }

            //出現する処理
            if (state == State.Pause)
            {
                apper_timer.Update(game_time);

                if (apper_timer.IsTime())
                {
                    disapper_timer.SetTime(1.3f);
                    apper_timer.SetTime(2.0f);

                    is_hit = false;

                    state = State.Active;
                }
            }
        }

        public override void Draw(Render render)
        {
            render.Draw(
                "guide_block",
                Camera.Position(position) + GameDevice.Instance().pos,
                null,
                render.color,
                render.rotation,
                render.origin,
                render.scale);

            //ブロック描画
            render.Draw(
                "break_block",
                Camera.Position(position) + GameDevice.Instance().pos + display.Pos(),
                null,
                render.color * disapper_timer.GetTime(),
                render.rotation,
                render.origin,
                render.scale);
        }

        public override void Hit(GameObject game_obj)
        {
            is_hit = true;

            //振動を設定
            display.is_wave = true;
            display.is_x = true;
            display.is_y = true;
            display.speed = 5.0f;
        }
    }
}