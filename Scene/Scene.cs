using Game1.Device;
using Microsoft.Xna.Framework;

namespace Game1.Scene
{
    abstract class Scene
    {
        protected SceneManager scene_m;

        public Scene(SceneManager scene_m)
        {
            this.scene_m = scene_m;

            is_end = false;
        }

        public abstract void Init();

        public abstract void Update(GameTime game_time);

        public abstract void Draw(Render render);

        public bool is_end { protected set; get; }//シーンが終了したか

        public SceneName next { protected set; get; }//次のシーン
    }
}
