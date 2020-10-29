using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1.Device
{
    //カメラ
    public static class Camera
    {
        public static Vector2 correct_pos;
        public static Vector2 position { set; get; }//カメラの座標
        public static Vector2 min { set; get; }//最小値
        public static Vector2 max { set; get; }//最大値

        public static bool is_move;
        public static bool is_clear { set; get; }

        //カメラ移動
        public static void Move(Vector2 velocity)
        {
            correct_pos += velocity;
            correct_pos = Vector2.Clamp(correct_pos, min, max);//範囲指定
        }

        public static void Update(GameTime game_time)
        {
            //カメラ移動
            if (Input.GetKey(PlayerIndex.One, Buttons.RightThumbstickUp) ||
                Input.GetKey(Keys.W))
                correct_pos.Y -= 10.0f;
            if (Input.GetKey(PlayerIndex.One, Buttons.RightThumbstickDown) ||
                Input.GetKey(Keys.S))
                correct_pos.Y += 10.0f;
            if (Input.GetKey(PlayerIndex.One, Buttons.RightThumbstickLeft) ||
                Input.GetKey(Keys.A))
                correct_pos.X -= 10.0f;
            if (Input.GetKey(PlayerIndex.One, Buttons.RightThumbstickRight) ||
                Input.GetKey(Keys.D))
                correct_pos.X += 10.0f;
            correct_pos = Vector2.Clamp(correct_pos, min, max);//範囲指定

            position = Vector2.Lerp(position, correct_pos, 0.08f);

            if (Vector2.Distance(position, correct_pos) > 150 && !is_clear)
            {
                is_move = true;
            }
            else
            {
                is_move = false;
            }
        }

        public static Vector2 Position(Vector2 pos)
        {
            return pos - position;
        }
    }
}