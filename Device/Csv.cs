using System;
using System.Collections.Generic;
using System.IO;

namespace Game1.Device
{
    //csv読み込み
    class Csv
    {
        private List<string[]> list;

        public Csv()
        {
            list = new List<string[]>();
        }

        //csv読み込み
        public string[][] Load(string csv_name)
        {
            list.Clear();//リストを初期化

            StreamReader sr = null;

            try
            {
                sr = new StreamReader(@"Content/" + "./csv/" + csv_name + ".csv");//ファイルを開く

                //ストリームからリストに追加
                while (true)
                {
                    if (sr.EndOfStream)//ストリームの末尾
                    {
                        break;
                    }

                    list.Add(sr.ReadLine().Split(','));//リストに追加
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ファイルが見つかりません");
            }

            if (sr != null)
            {
                sr.Close();//ファイルを閉じる
            }

            //出力にデータを表示
            foreach (var line in list)
            {
                foreach (var data in line)
                {
                    //Console.Write(data);
                }
                //Console.WriteLine();
            }

            return list.ToArray();
        }

        //csv1ファイルのデータをint[][]に変換
        public int[][] IntArray()
        {
            string[][] sa = list.ToArray();

            int[][] int_array = new int[sa.Length][];//行の配列
            for (int i = 0; i < sa.Length; i++)
            {
                int_array[i] = new int[sa[i].Length];//列の配列

                for (int j = 0; j < sa[i].Length; j++)
                {
                    int_array[i][j] = int.Parse(sa[i][j]);//int型にパース
                }
            }

            return int_array;
        }
    }
}
