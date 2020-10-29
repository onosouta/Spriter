using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game1.Device
{
    //SE
    class Se
    {
        private ContentManager content;//コンテンツ管理

        private Dictionary<string, SoundEffect> se;//SE管理

        public Se(ContentManager content)
        {
            this.content = content;

            se = new Dictionary<string, SoundEffect>();

            SoundEffect.MasterVolume = 0.1f;//音量
        }

        //SE読み込み
        public void Load(string se_name)
        {
            if (se.ContainsKey(se_name))//追加済み
            {
                return;//終了
            }

            se.Add(se_name, content.Load<SoundEffect>("./" + se_name));
        }

        public void Unload()
        {
            se.Clear();
        }

        //SE再生
        public void Play(string se_name)
        {
            if (!se.ContainsKey(se_name))//SEがない
            {
                Debug.Assert(se.ContainsKey(se_name), se_name + "のSEが見つかりません");
            }

            se[se_name].Play();//再生
        }
    }
}
