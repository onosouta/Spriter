using Game1.Device;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Actor
{
    //ゲームオブジェクト管理
    class GameObjectManager
    {
        private List<GameObject> game_obj;//ゲームオブジェクト
        private List<GameObject> add_game_obj;//追加するゲームオブジェクト

        public Map map { set; get; }//マップ
        public ParticleManager particle_m { set; get; }//パーティクル管理

        public int item_counter { set; get; }

        public GameObjectManager()
        {
            Init();
        }

        public void Init()
        {
            InitList(ref game_obj);
            InitList(ref add_game_obj);

            particle_m = new ParticleManager();
        }

        //リストを初期化
        private void InitList(ref List<GameObject> game_obj)
        {
            if (game_obj != null)
            {
                game_obj.Clear();
            }
            else
            {
                game_obj = new List<GameObject>();
            }
        }

        //ゲームオブジェクトを追加
        public void Add(GameObject obj)
        {
            if (game_obj == null)
            {
                return;
            }

            obj.Init();//初期化して追加
            add_game_obj.Add(obj);
        }

        //マップを追加
        public void Add(Map map)
        {
            if (map == null)
            {
                return;
            }

            if (this.map != null)
            {
                this.map = null;
            }

            this.map = map;
        }

        //更新
        public void Update(GameTime game_time)
        {
            //更新
            for (int i = 0; i < game_obj.Count; i++)
            {
                //カメラ
                if (Camera.is_move)
                {
                    game_obj[i].state = State.Pause;
                }
                else
                {
                    if (game_obj[i] is BreakBlock) continue;
                    game_obj[i].state = State.Active;
                }

                if (game_obj[i].state == State.Active)
                {
                    game_obj[i].Update(game_time);
                }

                //リストから削除
                if (game_obj[i].state == State.Dead)
                {
                    game_obj.Remove(game_obj[i]);

                    continue;
                }
            }

            //パーティクル
            particle_m.Update(game_time);

            //追加
            add_game_obj.ForEach(obj => game_obj.Add(obj));
            add_game_obj.Clear();

            //衝突判定
            //HitMap();
            HitGameObject();
        }

        //ゲームオブジェクトとの当たり判定
        private void HitGameObject()
        {
            foreach (var obj1 in game_obj)
            {
                foreach (var obj2 in game_obj)
                {
                    //同じオブジェクトかオブジェクトが死んでいる
                    if (obj1.Equals(obj2) ||
                        obj1.state == State.Dead ||
                        obj2.state == State.Dead)
                    {
                        continue;
                    }

                    //衝突判定
                    if (obj1.IsCollision(obj2))
                    {
                        obj1.Hit(obj2);//衝突処理
                        obj2.Hit(obj1);//衝突処理
                    }
                }
            }
        }

        //マップとの当たり判定
        //private void HitMap()
        //{
        //    if (map == null)
        //    {
        //        return;
        //    }

        //    game_obj.ForEach(obj => map.Hit(obj));
        //}

        //描画
        public void Draw(Render render)
        {
            game_obj.ForEach(obj => obj.Draw(render));

            //particle_m.Draw(render);
        }

        //全オブジェクトをアクティブにする
        public void ObjectActive()
        {
            for (int i = 0; i < game_obj.Count; i++)
            {
                game_obj[i].state = State.Active;
            }
            for (int i = 0; i < add_game_obj.Count; i++)
            {
                add_game_obj[i].state = State.Active;
            }

            //map.MapActive();

            particle_m.ParticleActive();
        }

        //全オブジェクトを一時停止
        public void ObjectPause()
        {
            for (int i = 0; i < game_obj.Count; i++)
            {
                game_obj[i].state = State.Pause;
            }
            for (int i = 0; i < add_game_obj.Count; i++)
            {
                add_game_obj[i].state = State.Pause;
            }

            //map.MapPause();

            particle_m.ParticlePause();
        }

        public Player Player()
        {
            return
                (Player)game_obj.Find(obj => obj is Player);
        }
    }
}
