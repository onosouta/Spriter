using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game1.Device
{
    //レンダラー
    class Render
    {
        private ContentManager content;//コンテンツ管理
        private SpriteBatch sprite;//スプライト

        private Dictionary<string, Texture2D> tex;//画像管理

        //デフォルト
        public Color color = Color.White;
        public float rotation = 0;
        public Vector2 origin = Vector2.Zero;
        public Vector2 scale = Vector2.One;

        public float c_pos { set; get; }

        public Render(ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            sprite = new SpriteBatch(graphics);

            tex = new Dictionary<string, Texture2D>();

            c_pos = 1.0f;
        }

        //画像読み込み
        public void Load(string tex_name)
        {
            if (tex.ContainsKey(tex_name))//追加済み
            {
                return;//終了
            }

            tex.Add(tex_name, content.Load<Texture2D>("./texture/" + tex_name));
        }

        public void Unload()
        {
            tex.Clear();
        }

        //Game1クラスのみ
        public void Begin()
        {
            sprite.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend);
        }

        //Game1クラスのみ
        public void End()
        {
            sprite.End();
        }

        #region 画像を描画

        /// <summary>
        /// 画像を描画
        /// </summary>
        /// <param name="tex_name">画像名</param>
        /// <param name="pos">座標</param>
        /// <param name="origin">中心座標</param>
        public void Draw(string tex_name, Vector2 pos, Vector2 origin)
        {
            if (!tex.ContainsKey(tex_name))//画像がない
            {
                Debug.Assert(tex.ContainsKey(tex_name), tex_name + "の画像が見つかりません");
            }

            sprite.Draw(tex[tex_name], pos * c_pos, null, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// 画像を描画
        /// </summary>
        /// <param name="tex_name">画像名</param>
        /// <param name="pos">座標</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="color">色</param>
        /// <param name="rotation">角度</param>
        /// <param name="origin">中心座標</param>
        /// <param name="scale">大きさ</param>
        public void Draw(string tex_name, Vector2 pos, Rectangle? rect, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            if (!tex.ContainsKey(tex_name))//画像がない
            {
                Debug.Assert(tex.ContainsKey(tex_name), tex_name + "の画像が見つかりません");
            }
            
            sprite.Draw(tex[tex_name], pos * c_pos, rect, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// 画像を描画
        /// </summary>
        /// <param name="num">数字</param>
        /// <param name="format">桁</param>
        /// <param name="tex_name">画像名</param>
        /// <param name="pos">座標</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public void DrawNumber(float num, string format, string tex_name, Vector2 pos, int width, int height, float scale)
        {
            if (!tex.ContainsKey(tex_name))//画像がない
            {
                Debug.Assert(tex.ContainsKey(tex_name), tex_name + "の画像が見つかりません");
            }

            foreach (var n in num.ToString(format))
            {
                if (n == '.')
                {
                    Rectangle rect = new Rectangle(10 * width, 0, width / 2, height);
                    sprite.Draw(tex[tex_name], pos, rect, color, rotation, origin, scale, SpriteEffects.None, 0);
                    pos.X += 12 * scale / 0.25f;
                }
                else
                {
                    Rectangle rect = new Rectangle((n - '0') * width, 0, width, height);
                    sprite.Draw(tex[tex_name], pos, rect, color, rotation, origin, scale, SpriteEffects.None, 0);
                    pos.X += 18 * scale / 0.25f;
                }
            }
        }

        #endregion 画像を描画
    }
}
