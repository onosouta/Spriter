using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game1.Device
{
    //BGM
    class Bgm
    {
        private ContentManager content;//コンテンツ管理

        private Dictionary<string, Song> bgm;//BGM管理
        private string cur_bgm;//現在のBGM

        public Bgm(ContentManager content)
        {
            this.content = content;

            bgm = new Dictionary<string, Song>();
            cur_bgm = null;

            MediaPlayer.IsRepeating = true;//リピートする
            MediaPlayer.Volume = 0.075f;//音量
        }

        //BGM読み込み
        public void Load(string bgm_name)
        {
            if (bgm.ContainsKey(bgm_name))//追加済み
            {
                return;//終了
            }

            bgm.Add(bgm_name, content.Load<Song>("./" + bgm_name));
        }

        public void Unload()
        {
            bgm.Clear();
        }

        //BGM再生
        public void Play(string bgm_name)
        {
            if (!bgm.ContainsKey(bgm_name))//BGMがない
            {
                Debug.Assert(bgm.ContainsKey(bgm_name), bgm_name + "のBGMが見つかりません");
            }

            if (MediaPlayer.State == MediaState.Playing)//再生中
            {
                Stop();//停止
            }

            cur_bgm = bgm_name;
            MediaPlayer.Play(bgm[cur_bgm]);//再生
        }

        //BGM停止
        public void Stop()
        {
            MediaPlayer.Stop();//停止
            cur_bgm = null;
        }
    }
}
