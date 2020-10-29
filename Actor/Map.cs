using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Actor
{
    //マップ
    class Map
    {
        private GameObjectManager game_obj_m;

        //ソース
        private Dictionary<string, GameObject> duplication_source;
        
        private int map_width;  //幅
        private int map_height; //高さ
        
        private Csv csv;

        //プロパティ
        //マップ
        public List<List<GameObject>> map { set; get; } //オブジェクト
        public int[][] numeric_map { set; get; }        //数値
        public int[][] map_data { set; get; }

        public int current_map { set; get; }

        public Map(GameObjectManager game_obj_m)
        {
            //this.game_obj_m = game_obj_m;

            //複製元
            duplication_source = new Dictionary<string, GameObject>
            {
                //スペース
                { "0", new Space(Vector2.Zero) },
                { "1", new Space(Vector2.Zero) },//チェックポイント
                { "2", new Space(Vector2.Zero) },//初期化
                
                { "3", new ClearBlock(Vector2.Zero, game_obj_m) },  //クリア
                { "4", new ItemBlock(Vector2.Zero, game_obj_m) },   //アイテム

                //ブロック
                { "10", new Block(Vector2.Zero, 0) },
                { "11", new Block(Vector2.Zero, 1) },
                { "12", new Block(Vector2.Zero, 2) },
                { "13", new Block(Vector2.Zero, 3) },
                { "14", new Block(Vector2.Zero, 4) },
                { "15", new Block(Vector2.Zero, 5) },
                { "16", new Block(Vector2.Zero, 6) },
                { "17", new Block(Vector2.Zero, 7) },
                { "18", new Block(Vector2.Zero, 8) },
                
                { "20", new ThornBlock(Vector2.Zero, 0) },//右
                { "21", new ThornBlock(Vector2.Zero, 1) },//下
                { "22", new ThornBlock(Vector2.Zero, 2) },//左
                { "23", new ThornBlock(Vector2.Zero, 3) },//上
                
                { "30", new ThroughBlock(Vector2.Zero, "through_block") },
                { "31", new ThroughBlock(Vector2.Zero, "through_block_l") },//左
                { "32", new ThroughBlock(Vector2.Zero, "through_block_r") },//右
                
                { "40", new BreakBlock(Vector2.Zero, this) },
            };

            map = new List<List<GameObject>>();

            map_width = 200;//マップの幅
            map_height = 200;//マップ高さ
            //マップ生成
            numeric_map = new int[map_height][];
            for (int i = 0; i < numeric_map.Length; i++)
            {
                numeric_map[i] = new int[map_width];
                for (int j = 0; j < numeric_map[i].Length; j++)
                {
                    numeric_map[i][j] = 14;//ブロックで埋める
                }
            }

            csv = new Csv();
            csv.Load("map_info");
            map_data = csv.IntArray();

            current_map = 0;//最初のマップ
        }

        /// <summary>
        /// マップ読み込み
        /// </summary>
        /// <param name="map_num">マップの数</param>
        public void Load(int map_num)
        {
            //数値で読み込み
            for (int i = 0; i < map_num; i++)
            {
                string[][] csv_map = csv.Load("map" + (i + 1));//マップ番号
                
                for (int row = 0; row < csv_map.Length; row++)//列
                {
                    for (int col = 0; col < csv_map[row].Length; col++)//行
                    {
                        numeric_map
                            [map_data[i][2] + row]
                            [map_data[i][1] + col] =
                            csv.IntArray()[row][col];
                    }
                }
            }

            //オブジェクト生成
            for (int y = 0; y < numeric_map.Length; y++)//列
            {
                List<GameObject> map_row = new List<GameObject>();//マップ1行

                for (int x = 0; x < numeric_map[y].Length; x++)//行
                {
                    //複製
                    GameObject copy_obj =　(GameObject)duplication_source[numeric_map[y][x].ToString()].Clone();
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
            //カメラ

            //カメラ最小値
            Camera.min = new Vector2(
                map_data[current_map][1] * Define.MAP_WIDTH,
                map_data[current_map][2] * Define.MAP_HEIGHT);
            //カメラ最大値
            Camera.max = new Vector2(
                (map_data[current_map][1] + map_data[current_map][3]) * Define.MAP_WIDTH,
                (map_data[current_map][2] + map_data[current_map][4]) * Define.MAP_HEIGHT) -
                new Vector2(Define.SCREEN_WIDTH, Define.SCREEN_HEIGHT);

            //更新する範囲
            int x_min = (int)Camera.position.X / Define.MAP_WIDTH - 2;
            int x_max = (int)(Camera.position.X + Define.SCREEN_WIDTH) / Define.MAP_WIDTH + 2;
            int y_min = (int)Camera.position.Y / Define.MAP_HEIGHT - 2;
            int y_max = (int)(Camera.position.Y + Define.SCREEN_HEIGHT) / Define.MAP_HEIGHT + 2;

            for (int x = x_min; x < x_max; x++)
            {
                if (x < 0 || x > map_width - 1) continue;

                for (int y = y_min; y < y_max; y++)
                {
                    if (y < 0 || y > map_height - 1) continue;

                    map[y][x].Update(game_time);//更新
                }
            }
        }

        //描画
        public void Draw(Render render)
        {
            //描画する範囲
            int x_min = (int)Camera.position.X / Define.MAP_WIDTH - 2;
            int x_max = (int)(Camera.position.X + Define.SCREEN_WIDTH) / Define.MAP_WIDTH + 2;
            int y_min = (int)Camera.position.Y / Define.MAP_HEIGHT - 2;
            int y_max = (int)(Camera.position.Y + Define.SCREEN_HEIGHT) / Define.MAP_HEIGHT + 2;

            for (int x = x_min; x < x_max; x++)
            {
                if (x < 0 || x > map_width - 1) continue;

                for (int y = y_min; y < y_max; y++)
                {
                    if (y < 0 || y > map_height - 1) continue;

                    map[y][x].Draw(render);//描画
                }
            }
        }

        //ブロックの種類を返す
        public GameObject IsBlock(Vector2 position)
        {
            position = Vector2.Clamp(
                position,
                Camera.min,
                Camera.max + new Vector2(Define.SCREEN_WIDTH, Define.SCREEN_HEIGHT));

            //座標を配列に変換
            int x = (int)position.X / Define.MAP_WIDTH;
            int y = (int)position.Y / Define.MAP_HEIGHT;

            Range x_range = new Range(0, map_width);//行の範囲
            Range y_range = new Range(0, map_height);//列の範囲

            if (!x_range.IsRange(x) ||
                !y_range.IsRange(y))//配列外
            {
                return null;
            }

            if (x >= map_data[current_map][1] &&
                x <= map_data[current_map][1] + map_data[current_map][3] - 1 &&
                y >= map_data[current_map][2] &&
                y <= map_data[current_map][2] + map_data[current_map][4] - 1)
            {
                if (map[y][x].state == State.Active)
                {
                    return map[y][x];
                }
                else return null;
            }

            return null;
        }
    }
}
