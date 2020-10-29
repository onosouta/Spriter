using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1.Device
{
    //ゲームデバイス
    sealed class GameDevice
    {
        private ContentManager content;//コンテンツ管理
        private GraphicsDevice graphics;//グラフィックデバイス
        
        private static GameDevice instance;//インスタンス

        private GameDevice(ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            this.graphics = graphics;

            Bgm = new Bgm(content);
            Random = new Random();
            Render = new Render(content, graphics);
            Se = new Se(content);

            Display = new Display(true);
        }

        //Game1クラスのみ
        public static GameDevice Instance(
            ContentManager content,
            GraphicsDevice graphics)
        {
            if (instance == null)
            {
                instance = new GameDevice(content, graphics);//インスタンス生成
            }

            return instance;
        }
        
        public static GameDevice Instance()
        {
            return instance;
        }

        //Game1クラスのみ
        public void Update(GameTime game_time)
        {
            Input.Update(game_time);
            Camera.Update(game_time);

            pos = Display.Pos();
        }

        public Bgm Bgm { get; }//BGM

        public Random Random { get; }//ランダム

        public Render Render { get; }//レンダラー

        public Se Se { get; }//SE

        public Display Display { get; }

        public Vector2 pos { set; get; }
    }
}
