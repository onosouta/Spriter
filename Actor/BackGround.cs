using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;

namespace Game1.Actor
{
    class BackGround
    {
        private Vector2 position;
        private int width, height;
        public Motion motion { set; get; }
        public bool is_dead { set; get; }

        public BackGround()
        {
            position = new Vector2(
                GameDevice.Instance().Random.Next(0, Define.SCREEN_WIDTH),
                GameDevice.Instance().Random.Next(0, Define.SCREEN_HEIGHT));
            width = height = 10;

            motion = new Motion(new Range(0, 4), new CountDownTimer(0.2f));
            motion.Add(0, new Rectangle(width * 0, 0, width, height));
            motion.Add(1, new Rectangle(width * 1, 0, width, height));
            motion.Add(2, new Rectangle(width * 2, 0, width, height));
            motion.Add(3, new Rectangle(width * 3, 0, width, height));
            motion.Add(4, new Rectangle(width * 4, 0, width, height));
            is_dead = false;
            motion.cur_num = 0;
        }

        public void Update(GameTime game_time)
        {
            if (!Camera.is_move)
            {
                motion.Update(game_time);
                if (motion.cur_num == 4)
                {
                    is_dead = true;
                }
            }
        }

        public void Draw(Render render)
        {
            if (!is_dead)
            {
                render.Draw(
                    "bg_1",
                    position,
                    motion.DrawRectangle(),
                    render.color,
                    render.rotation,
                    new Vector2(width / 2, height / 2),
                    render.scale);
            }
        }
    }
}
