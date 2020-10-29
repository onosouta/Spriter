using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1.Device
{
    //入力処理
    class Input
    {
        public static bool key_lock;

        #region キーボード

        private static KeyboardState key;
        private static KeyboardState pre_key;//1フレーム前

        //キーを押した瞬間か
        public static bool GetKeyDown(Keys keys)
        {
            if (key_lock) return false;

            return
                key.IsKeyDown(keys) &&
                pre_key.IsKeyUp(keys);
        }

        //キーを押しているか
        public static bool GetKey(Keys keys)
        {
            if (key_lock) return false;

            return
                key.IsKeyDown(keys);
        }

        //キーを離した瞬間か
        public static bool GetKeyUp(Keys keys)
        {
            if (key_lock) return false;

            return
                key.IsKeyUp(keys) &&
                pre_key.IsKeyDown(keys);
        }

        #endregion キーボード
        
        #region ゲームパッド

        //ゲームパッド番号
        private static List<PlayerIndex> player_index = new List<PlayerIndex>
        {
            PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four,
        };

        private static Dictionary<PlayerIndex, GamePadState> cur_pad = new Dictionary<PlayerIndex, GamePadState>
        {
            { PlayerIndex.One, GamePad.GetState(PlayerIndex.One) },
            { PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two) },
            { PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three) },
            { PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four) },
        };

        //1フレーム前
        private static Dictionary<PlayerIndex, GamePadState> pre_pad = new Dictionary<PlayerIndex, GamePadState>
        {
            { PlayerIndex.One, GamePad.GetState(PlayerIndex.One) },
            { PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two) },
            { PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three) },
            { PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four) },
        };

        //ボタンを押した瞬間か
        public static bool GetKeyDown(PlayerIndex index, Buttons button)
        {
            if (!cur_pad[index].IsConnected) return false;

            if (key_lock) return false;
            
            return
                cur_pad[index].IsButtonDown(button) &&
                pre_pad[index].IsButtonUp(button);
        }

        //ボタンを押しているか
        public static bool GetKey(PlayerIndex index, Buttons button)
        {
            if (!cur_pad[index].IsConnected) return false;

            if (key_lock) return false;

            return
                cur_pad[index].IsButtonDown(button);
        }

        #endregion ゲームパッド

        //Game1クラスのみ
        public static void Update(GameTime game_time)
        {
            //キーボード更新
            pre_key = key;
            key = Keyboard.GetState();

            //ゲームパッド更新
            for (int i = 0; i < cur_pad.Count; i++)
            {
                if (!cur_pad[player_index[i]].IsConnected) continue;

                pre_pad[player_index[i]] = cur_pad[player_index[i]];
                cur_pad[player_index[i]] = GamePad.GetState(player_index[i]);
            }

        }
    }
}
