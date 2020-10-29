using Game1.Actor;
using Game1.Device;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Scene
{
    //シーン名
    public enum SceneName
    {
        Title,//タイトル
        Game,//ゲーム
        Clear,//クリア
    }

    //シーン管理
    class SceneManager
    {
        public GameObjectManager game_obj_m { set; get; }//ゲームオブジェクト管理
        public Map map { set; get; }//マップ
        public BgMap bg_map { set; get; }

        public SnowBG snow_bg { set; get; }

        private Dictionary<SceneName, Scene> scene;//シーン管理
        private Scene cur_scene;//現在のシーン

        public List<BackGround> bg { set; get; }
        public int game_timer { set; get; }
        public int item_counter { set; get; }
        public bool title_fade { set; get; }
        public int dead_counter { set; get; }

        public float time_1 { set; get; }
        public float time_2 { set; get; }
        public float time_3 { set; get; }

        public SceneManager()
        {
            game_obj_m = new GameObjectManager();

            map = new Map(game_obj_m);
            bg_map = new BgMap(game_obj_m);

            snow_bg = new SnowBG();
            title_fade = false;

            time_1 = time_2 = time_3 = 999.99f * 60;

            bg = new List<BackGround>();
            for (int i = 0; i < 30; i++)
            {
                BackGround b = new BackGround();
                b.motion.cur_num = GameDevice.Instance().Random.Next(5);
                bg.Add(b);
            }

            scene = new Dictionary<SceneName, Scene>()
            {
                { SceneName.Title, new Title(this) },
                { SceneName.Game, new Game(this) },
                { SceneName.Clear, new Clear(this) },
            };

            Change(SceneName.Title);//最初のシーンを設定
        }

        //シーンを変更
        public void Change(SceneName scene_name)
        {
            cur_scene = scene[scene_name];
            cur_scene.Init();
        }

        //更新
        public void Update(GameTime game_time)
        {
            if (cur_scene == null)
            {
                return;
            }

            cur_scene.Update(game_time);

            if (cur_scene.is_end)//シーンが終了したら
            {
                Change(cur_scene.next);//シーン切り替え
            }
        }

        //描画
        public void Draw(Render render)
        {
            if (cur_scene == null)
            {
                return;
            }

            render.Draw("bg", Vector2.Zero, Vector2.Zero);
            cur_scene.Draw(render);
        }
    }
}
