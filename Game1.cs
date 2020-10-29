using Game1.Device;
using Game1.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Effect effect;

        private GameDevice game_device;//ゲームデバイス
        private Bgm bgm;//BGM
        private Render render;//レンダラー
        private Se se;//SE

        private SceneManager scene_m;//シーン管理

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Define.SCREEN_WIDTH;//幅
            graphics.PreferredBackBufferHeight = Define.SCREEN_HEIGHT;//高さ

            Window.Title = "Sprinter";//タイトル

            //9IsMouseVisible = true;//マウスカーソルを表示
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            game_device = GameDevice.Instance(Content, GraphicsDevice);

            scene_m = new SceneManager();
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            //BGM読み込み
            bgm = game_device.Bgm;
            //bgm.Load("");

            //画像読み込み
            render = game_device.Render;
            render.Load("player_l");
            render.Load("player_r");
            render.Load("idle_player_l");
            render.Load("idle_player_r");
            render.Load("walk_player_l");
            render.Load("walk_player_r");
            render.Load("smoke_p");
            render.Load("dead_p");
            render.Load("dush_p");
            render.Load("clear_p_1");
            render.Load("clear_p_2");
            render.Load("item_p");
            render.Load("snow");
            render.Load("bg_1");
            render.Load("bg_2");
            render.Load("block");
            render.Load("bg_block");
            render.Load("break_block");
            render.Load("guide_block");
            render.Load("thorn");
            render.Load("through_block");
            render.Load("through_block_l");
            render.Load("through_block_r");
            render.Load("goal");
            render.Load("watch");
            render.Load("item");
            render.Load("skull");
            render.Load("title");
            render.Load("clear");
            render.Load("press");
            render.Load("a");
            render.Load("s");
            render.Load("pixel");
            render.Load("w_pixel");
            render.Load("black_pixel");
            render.Load("band");
            render.Load("band_s");
            render.Load("band_2");
            render.Load("clear_time_band");
            render.Load("clear_time_band_s");
            render.Load("time");
            render.Load("number");
            render.Load("how_to");
            render.Load("start");
            render.Load("kakeru");
            render.Load("fade");
            render.Load("quit");
            render.Load("resume");
            render.Load("bg");
            render.Load("new");
            render.Load("rank");
            render.Load("1");
            render.Load("2");
            render.Load("3");
            render.Load("kettei");
            render.Load("syuuryou");
            render.Load("pause");
            render.Load("slash");

            //SE読み込み
            se = game_device.Se;
            //se.Load("");
            
            //effect = Content.Load<Effect>("Effect1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            game_device.Update(gameTime);

            scene_m.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            render.Begin();
            
            scene_m.Draw(render);

            render.End();

            base.Draw(gameTime);
        }
    }
}
