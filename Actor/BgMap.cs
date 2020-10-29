using Game1.Device;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Actor
{
    class BgMap
    {
        private Csv csv;//csvファイル読み込み用

        public int[][] map_info { set; get; }//マップの情報

        public List<List<GameObject>> map { set; get; }//マップのオブジェクト

        private int map_width;
        private int map_height;
        public int[][] g_map { set; get; }//グローバルマップの配列

        public int cur_map { set; get; }//プレイヤーが現在いるマップ番号

        //複製元
        private Dictionary<string, GameObject> duplication_source;

        private GameObjectManager game_obj_m;

        public BgMap(GameObjectManager game_obj_m)
        {
            this.game_obj_m = game_obj_m;

            map = new List<List<GameObject>>();

            //複製元
            duplication_source = new Dictionary<string, GameObject>
            {
                //スペース
                { "0", new Space(Vector2.Zero) },

                //ブロック
                { "10", new BgBlock(Vector2.Zero, 0) },
                { "11", new BgBlock(Vector2.Zero, 1) },
                { "12", new BgBlock(Vector2.Zero, 2) },
                { "13", new BgBlock(Vector2.Zero, 3) },
                { "14", new BgBlock(Vector2.Zero, 4) },
                { "15", new BgBlock(Vector2.Zero, 5) },
                { "16", new BgBlock(Vector2.Zero, 6) },
                { "17", new BgBlock(Vector2.Zero, 7) },
                { "18", new BgBlock(Vector2.Zero, 8) },

                { "20", new BgBlock(Vector2.Zero, 10) },
                { "21", new BgBlock(Vector2.Zero, 11) },
                { "22", new BgBlock(Vector2.Zero, 12) },
                { "23", new BgBlock(Vector2.Zero, 13) },
                { "24", new BgBlock(Vector2.Zero, 14) },
                { "25", new BgBlock(Vector2.Zero, 15) },
                { "26", new BgBlock(Vector2.Zero, 16) },
                { "27", new BgBlock(Vector2.Zero, 17) },
                { "28", new BgBlock(Vector2.Zero, 18) },
            };

            map_width = map_height = 300;
            g_map = new int[map_height][];
            //ブロックで埋める
            for (int i = 0; i < g_map.Length; i++)
            {
                g_map[i] = new int[map_width];

                for (int j = 0; j < g_map[i].Length; j++)
                {
                    g_map[i][j] = 0;
                }
            }

            csv = new Csv();
            csv.Load("map_info");
            map_info = csv.IntArray();

            cur_map = 0;
        }

        /// <summary>
        /// マップ読み込み
        /// </summary>
        /// <param name="map_num">マップの数</param>
        public void Load(int map_num)
        {
            for (int i = 0; i < map_num; i++)
            {
                string[][] csv_map = csv.Load("bg_map" + (i + 1));//マップ番号を指定して読み込み

                //csvのデータをg_mapに追加
                for (int row = 0; row < csv_map.Length; row++)//列
                {
                    for (int col = 0; col < csv_map[row].Length; col++)//行
                    {
                        g_map
                            [map_info[i][2] + row]
                            [map_info[i][1] + col] =
                            csv.IntArray()[row][col];
                    }
                }
            }

            //オブジェクト生成
            for (int y = 0; y < g_map.Length; y++)
            {
                List<GameObject> map_row = new List<GameObject>();//マップ1行分

                for (int x = 0; x < g_map[y].Length; x++)
                {
                    GameObject copy_obj = (GameObject)duplication_source[g_map[y][x].ToString()].Clone();//複製
                    copy_obj.position = new Vector2(
                        x * Define.MAP_WIDTH,
                        y * Define.MAP_HEIGHT);//複製する座標
                    copy_obj.Init();

                    map_row.Add(copy_obj);
                }

                map.Add(map_row);//マップに追加
            }
        }

        public void Unload()
        {
            map.Clear();
        }

        //更新
        public void Update(GameTime game_time)
        {
            //カメラ位置更新
            //カメラ最小値
            //Camera.min = new Vector2(
            //    map_info[cur_map][1] * Define.MAP_WIDTH,
            //    map_info[cur_map][2] * Define.MAP_HEIGHT);// - new Vector2(10000);
            //カメラ最大値
            //Camera.max = new Vector2(
            //    (map_info[cur_map][1] + map_info[cur_map][3]) * Define.MAP_WIDTH,
            //    (map_info[cur_map][2] + map_info[cur_map][4]) * Define.MAP_HEIGHT) -
            //    new Vector2(Define.SCREEN_WIDTH, Define.SCREEN_HEIGHT);// + new Vector2(10000);

            //for (int y = 0; y < g_map.Length; y++)
            //{
            //    for (int x = 0; x < g_map[y].Length; x++)
            //    {
            //        if (!(map[y][x] is BreakBlock || map[y][x] is ClearBlock)) continue;

            //        //カメラの範囲内だけ更新
            //        if (x * Define.MAP_WIDTH > Camera.position.X - Define.MAP_WIDTH &&
            //            x * Define.MAP_WIDTH < Camera.position.X + Define.SCREEN_WIDTH + Define.MAP_WIDTH &&
            //            y * Define.MAP_HEIGHT > Camera.position.Y - Define.MAP_HEIGHT * 2 &&
            //            y * Define.MAP_HEIGHT < Camera.position.Y + Define.SCREEN_HEIGHT + Define.MAP_HEIGHT)
            //        {
            //            if (map[y][x] is BreakBlock || map[y][x] is ClearBlock)
            //            {
            //                if (map[y][x].state == State.Active)
            //                {
            //                    map[y][x].Update(game_time);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public void AfterDraw(Render render)
        {
            for (int y = 0; y < g_map.Length; y++)
            {
                for (int x = 0; x < g_map[y].Length; x++)
                {
                    if (map[y][x] is Space) continue;

                    //カメラの範囲内だけ描画
                    if (x * Define.MAP_WIDTH > Camera.position.X - Define.MAP_WIDTH &&
                        x * Define.MAP_WIDTH < Camera.position.X + Define.SCREEN_WIDTH + Define.MAP_WIDTH &&
                        y * Define.MAP_HEIGHT > Camera.position.Y - Define.MAP_HEIGHT * 2 &&
                        y * Define.MAP_HEIGHT < Camera.position.Y + Define.SCREEN_HEIGHT + Define.MAP_HEIGHT)
                        if (map[y][x].a_draw) map[y][x].Draw(render);
                }
            }
        }

        //描画
        public void Draw(Render render)
        {
            for (int y = 0; y < g_map.Length; y++)
            {
                for (int x = 0; x < g_map[y].Length; x++)
                {
                    if (map[y][x] is Space) continue;

                    //カメラの範囲内だけ描画
                    if (x * Define.MAP_WIDTH > Camera.position.X - Define.MAP_WIDTH &&
                        x * Define.MAP_WIDTH < Camera.position.X + Define.SCREEN_WIDTH + Define.MAP_WIDTH &&
                        y * Define.MAP_HEIGHT > Camera.position.Y - Define.MAP_HEIGHT * 2 &&
                        y * Define.MAP_HEIGHT < Camera.position.Y + Define.SCREEN_HEIGHT + Define.MAP_HEIGHT)
                        if (!map[y][x].a_draw) ;//map[y][x].Draw(render);
                }
            }
        }

        //public void MapActive()
        //{
        //    for (int y = 0; y < g_map.Length; y++)
        //    {
        //        for (int x = 0; x < g_map[y].Length; x++)
        //        {
        //            if (map[y][x] is Space) continue;

        //            //カメラの範囲内だけ変更
        //            if (x * Define.MAP_WIDTH > Camera.position.X - Define.MAP_WIDTH &&
        //                x * Define.MAP_WIDTH < Camera.position.X + Define.SCREEN_WIDTH + Define.MAP_WIDTH &&
        //                y * Define.MAP_HEIGHT > Camera.position.Y - Define.MAP_HEIGHT * 2 &&
        //                y * Define.MAP_HEIGHT < Camera.position.Y + Define.SCREEN_HEIGHT + Define.MAP_HEIGHT)
        //                map[y][x].state = State.Active;
        //        }
        //    }
        //}

        //public void MapPause()
        //{
        //    for (int y = 0; y < g_map.Length; y++)
        //    {
        //        for (int x = 0; x < g_map[y].Length; x++)
        //        {
        //            if (map[y][x] is Space) continue;

        //            //カメラの範囲内だけ変更
        //            if (x * Define.MAP_WIDTH > Camera.position.X - Define.MAP_WIDTH &&
        //                x * Define.MAP_WIDTH < Camera.position.X + Define.SCREEN_WIDTH + Define.MAP_WIDTH &&
        //                y * Define.MAP_HEIGHT > Camera.position.Y - Define.MAP_HEIGHT * 2 &&
        //                y * Define.MAP_HEIGHT < Camera.position.Y + Define.SCREEN_HEIGHT + Define.MAP_HEIGHT)
        //                map[y][x].state = State.Pause;
        //        }
        //    }
        //}

        //public void Hit(GameObject game_obj)
        //{
            ////座標を配列に変換
            //int x = ((int)game_obj.position.X + game_obj.width / 2) / Define.MAP_WIDTH;
            //int y = ((int)game_obj.position.Y + game_obj.height / 2) / Define.MAP_HEIGHT;

            //Range x_range = new Range(
            //    map_info[cur_map][1],
            //    map_info[cur_map][1] + map_info[cur_map][3] - 1);//行の範囲
            //Range y_range = new Range(
            //    map_info[cur_map][2],
            //    map_info[cur_map][2] + map_info[cur_map][4] - 1);//列の範囲

            ////9×9マス
            //for (int row = y - 1; row <= (y + 1); row++)//列
            //{
            //    for (int col = x - 1; col <= (x + 1); col++)//行
            //    {
            //        if (!x_range.IsRange(map_info[cur_map][1] + row) ||
            //            !y_range.IsRange(map_info[cur_map][2] + col))//配列外
            //        {
            //            continue;
            //        }

            //        if (map[row][col].IsCollision(game_obj))
            //        {
            //            game_obj.Hit(map[row][col]);
            //        }
            //    }
            //}
        //}

        //ブロックの番号を返す
        //public GameObject IsBlock(Vector2 position)
        //{
        //    position = Vector2.Clamp(
        //        position,
        //        Camera.min,
        //        Camera.max + new Vector2(Define.SCREEN_WIDTH, Define.SCREEN_HEIGHT));

        //    //座標を配列に変換
        //    int x = (int)position.X / Define.MAP_WIDTH;
        //    int y = (int)position.Y / Define.MAP_HEIGHT;

        //    Range x_range = new Range(0, map_width);//行の範囲
        //    Range y_range = new Range(0, map_height);//列の範囲

        //    if (!x_range.IsRange(x) ||
        //        !y_range.IsRange(y))//配列外
        //    {
        //        return null;
        //    }

        //    if (x >= map_info[cur_map][1] &&
        //        x <= map_info[cur_map][1] + map_info[cur_map][3] - 1 &&
        //        y >= map_info[cur_map][2] &&
        //        y <= map_info[cur_map][2] + map_info[cur_map][4] - 1)
        //    {
        //        if (map[y][x].state == State.Active)//アクティブなブロックのみ
        //        {
        //            return map[y][x];
        //        }
        //        else return null;
        //    }

        //    return null;
        //}
    }
}
