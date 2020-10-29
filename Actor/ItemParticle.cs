using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    class ItemParticle : Particle
    {
        private GameObjectManager game_obj_m;

        public ItemParticle(Vector2 position, GameObjectManager game_obj_m) : base(position)
        {
            this.game_obj_m = game_obj_m;
        }

        public override void Init()
        {
            position = position + new Vector2(
                GameDevice.Instance().Random.Next(-40, 40),
                GameDevice.Instance().Random.Next(-40, 40));
            vel = new Vector2(
                0,
                -1);
            speed = (float)random.Next(5, 10) / 10;
            scale = 1;
            alpha = 2.0f;
            life_timer = new CountDownTimer(0.5f);//プレイヤーの復活時間

            width = height = 9;
        }

        public override void Update(GameTime game_time)
        {
            if (alpha > 0.0f)
            {
                alpha -= 0.05f;
            }
            else
            {
                alpha = 0.0f;
            }

            position += vel * speed;
        }

        public override void Draw(Render render)
        {
            render.Draw(
                "item_p",
                Camera.Position(position) + GameDevice.Instance().pos,
                null,
                render.color * alpha,
                render.rotation,
                new Vector2(width / 2, height / 2),
                new Vector2(scale) * render.scale);
        }

        public override void Hit(GameObject game_obj)
        {

        }

        public ItemParticle(ItemParticle item_particle) : this(item_particle.position, item_particle.game_obj_m) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new ItemParticle(this);
        }
    }
}
