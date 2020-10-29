using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1.Actor
{
    //プレイヤー
    class Player : GameObject
    {
        private GameObjectManager game_obj_m;
        private Map map;
        
        //ダッシュ、ジャンプ
        private bool is_jump, is_dush;
        private Vector2 velocity, dush_velocity;
        private float speed, dush_speed;
        private int jump_counter;//ジャンプ量の調整
        
        private Vector2[] check1, check2, check3, check4;

        //スケール
        private Vector2 scale;
        private Timer scale_timer;

        //残像
        private List<Vector2> afterimage_scale, afterimage_pos;
        private List<float> afterimage_alpha;

        //パーティクル
        private List<Emitter> emitter;//エミッター
        private Timer emitter_timer;//エミッター生成タイマー
        
        private Timer wall_timer;

        //死亡
        private bool is_dead;
        public Timer revival_timer;

        //モーション
        private Motion player_l, idle_l, walk_l, player_r, idle_r, walk_r;
        //画像
        private Dictionary<string, Motion> motions;
        private string cur_tex;
        private string pre_tex;

        //クリア
        public bool is_clear { set; get; }
        private int clear_counter;
        private int clear_speed;

        public Player(GameObjectManager game_obj_m, Vector2 position, Map map) : base(position)
        {
            this.game_obj_m = game_obj_m;
            this.map = map;
        }

        //初期化
        public override void Init()
        {
            width = height = 30;
            state = State.Active;

            is_jump = is_dush = false;
            velocity = dush_velocity = Vector2.Zero;
            speed = 5.0f;
            dush_speed = 0.0f;
            jump_counter = 0;

            check1 = new Vector2[3] { new Vector2(0, -1), new Vector2(width - 1, -1), new Vector2(width / 2, -1) };//上
            check2 = new Vector2[3] { new Vector2(0, height), new Vector2(width - 1, height), new Vector2(width / 2, height) };//下
            check3 = new Vector2[3] { new Vector2(-1, 0), new Vector2(-1, height - 1), new Vector2(-1, height / 2) };//左
            check4 = new Vector2[3] { new Vector2(width, 0), new Vector2(width, height - 1), new Vector2(width, height / 2) };//右

            scale = Vector2.One;
            scale_timer = new CountUpTimer(0);

            afterimage_pos = new List<Vector2>();
            afterimage_scale = new List<Vector2>();
            afterimage_alpha = new List<float>();

            emitter = new List<Emitter>();
            emitter_timer = new CountDownTimer(0.3f);
            
            wall_timer = new CountDownTimer(0.1f);

            is_dead = false;
            revival_timer = new CountDownTimer(0.9f);

            //モーション追加
            player_r = new Motion(new Range(0, 0), new CountDownTimer(0));
            player_r.Add(0, new Rectangle(0 * 50, 0, 50, 55));
            player_l = new Motion(new Range(0, 0), new CountDownTimer(0));
            player_l.Add(0, new Rectangle(0 * 50, 0, 50, 55));
            idle_l = new Motion(new Range(0, 1), new CountDownTimer(0.45f));
            idle_l.Add(0, new Rectangle(0 * 50, 0, 50, 55));
            idle_l.Add(1, new Rectangle(1 * 50, 0, 50, 55));
            walk_l = new Motion(new Range(0, 3), new CountDownTimer(0.1f));
            walk_l.Add(0, new Rectangle(0 * 50, 0, 50, 55));
            walk_l.Add(1, new Rectangle(1 * 50, 0, 50, 55));
            walk_l.Add(2, new Rectangle(2 * 50, 0, 50, 55));
            walk_l.Add(3, new Rectangle(3 * 50, 0, 50, 55));
            idle_r = new Motion(new Range(0, 1), new CountDownTimer(0.45f));
            idle_r.Add(0, new Rectangle(0 * 50, 0, 50, 55));
            idle_r.Add(1, new Rectangle(1 * 50, 0, 50, 55));
            walk_r = new Motion(new Range(0, 3), new CountDownTimer(0.1f));
            walk_r.Add(0, new Rectangle(0 * 50, 0, 50, 55));
            walk_r.Add(1, new Rectangle(1 * 50, 0, 50, 55));
            walk_r.Add(2, new Rectangle(2 * 50, 0, 50, 55));
            walk_r.Add(3, new Rectangle(3 * 50, 0, 50, 55));

            motions = new Dictionary<string, Motion>()
            {
                { "player_l", player_l },
                { "player_r", player_r },
                { "idle_player_l", idle_l },
                { "idle_player_r", idle_r },
                { "walk_player_l", walk_l },
                { "walk_player_r", walk_r },
            };
            cur_tex = "idle_player_r";
            pre_tex = cur_tex;

            is_clear = false;
            clear_counter = 0;
            clear_speed = 10;
        }

        //更新
        public override void Update(GameTime game_time)
        {
            if (!is_dead)
            {
                //テクスチャの設定
                //左
                if (is_jump && velocity.X < 0)
                    cur_tex = "player_l";
                else if (velocity.X < 0)
                    cur_tex = "walk_player_l";
                else if ((velocity.X == 0 && pre_tex == "walk_player_l" && !is_jump) ||
                         (velocity.X == 0 && pre_tex == "player_l" && !is_jump))
                    cur_tex = "idle_player_l";
                //右
                pre_tex = cur_tex;
                if (is_jump && velocity.X > 0)
                    cur_tex = "player_r";
                else if (velocity.X > 0)
                    cur_tex = "walk_player_r";
                else if ((velocity.X == 0 && pre_tex == "walk_player_r" && !is_jump) ||
                         (velocity.X == 0 && pre_tex == "player_r" && !is_jump))
                    cur_tex = "idle_player_r";

                //モーションを更新
                if (is_clear)
                {
                    clear_counter++;
                    if (clear_counter % clear_speed == 0)
                    {
                        motions[cur_tex].Update(game_time);
                    }
                }
                else
                {
                    motions[cur_tex].Update(game_time);
                }

                //移動
                Move();
                //ジャンプ
                Jump();
                //ダッシュ
                Dush();

                //スケール設定
                ScaleAnime(game_time);

                //移動時のパーティクル
                if (!is_jump && velocity.X != 0)
                {
                    emitter_timer.Update(game_time);
                    if (emitter_timer.IsTime())
                    {
                        emitter_timer.SetTime(0.3f);
                        emitter.Add(new HitParticleEmitter(
                            game_obj_m,
                            new Vector2(position.X + Rectangle().Width / 2, position.Y + Rectangle().Height - 8),
                            2));
                    }
                }

                wall_timer.Update(game_time);

                //残像の位置とスケールを保存
                if (is_clear)
                {
                    if (clear_counter % clear_speed == 0)
                    {
                        if (dush_speed > 0.0f)
                        {
                            afterimage_pos.Insert(0, position);
                            afterimage_scale.Insert(0, scale);
                            afterimage_alpha.Insert(0, 1.0f);
                        }

                        //残像のアルファ値を設定
                        for (int i = 0; i < afterimage_alpha.Count; i++)
                        {
                            afterimage_alpha[i] -= 0.08f;
                            if (afterimage_alpha[i] <= 0)
                            {
                                afterimage_pos.Remove(afterimage_pos[i]);
                                afterimage_scale.Remove(afterimage_scale[i]);
                                afterimage_alpha.Remove(afterimage_alpha[i]);
                            }
                        }
                    }
                }
                else
                {
                    if (dush_speed > 0.0f)
                    {
                        afterimage_pos.Insert(0, position);
                        afterimage_scale.Insert(0, scale);
                        afterimage_alpha.Insert(0, 1.0f);
                    }

                    //残像のアルファ値を設定
                    for (int i = 0; i < afterimage_alpha.Count; i++)
                    {
                        afterimage_alpha[i] -= 0.08f;
                        if (afterimage_alpha[i] <= 0)
                        {
                            afterimage_pos.Remove(afterimage_pos[i]);
                            afterimage_scale.Remove(afterimage_scale[i]);
                            afterimage_alpha.Remove(afterimage_alpha[i]);
                        }
                    }
                }

                //座標最小値
                if (position.X < 0) position = new Vector2(0, position.Y);
                if (position.Y < 0) position = new Vector2(position.X, 0);

                //配列座標に変換
                int x = ((int)position.X + width / 2) / Define.MAP_WIDTH;
                int y = ((int)position.Y + height / 2) / Define.MAP_HEIGHT;

                bool is_range = false;//範囲内か
                //プレイヤーのマップ番号を計算
                for (int i = 0; i < map.map_data.Length; i++)
                {
                    if (x >= map.map_data[i][1] &&
                        x <= map.map_data[i][1] + map.map_data[i][3] - 1 &&
                        y >= map.map_data[i][2] &&
                        y <= map.map_data[i][2] + map.map_data[i][4] - 1)
                    {
                        map.current_map = map.map_data[i][0];
                        is_range = true;
                    }
                }

                //配列の範囲外
                if (!is_range)
                {
                    Dead();
                }
            }
            else
            {
                revival_timer.Update(game_time);
                if (revival_timer.IsTime())
                {
                    state = State.Dead;
                }
            }

            //エミッター
            for (int i = 0; i < emitter.Count; i++)
            {
                emitter[i].Update(game_time);
                if (emitter[i].is_dead)
                {
                    emitter.Remove(emitter[i]);
                }
            }
            
        }

        #region 移動

        //移動
        private void Move()
        {
            float acceleration = 0.2f;//加速度
            if (is_jump)
            {
                acceleration = 0.06f;//空中の加速度
            }

            //左
            if (Input.GetKey(Keys.Left) ||
                Input.GetKey(PlayerIndex.One, Buttons.DPadLeft) ||
                Input.GetKey(PlayerIndex.One, Buttons.LeftThumbstickLeft))
            {
                if (velocity.X > -1) velocity.X = Math.Max(velocity.X - acceleration, -1.0f);
                else velocity.X = Math.Max(velocity.X - acceleration, velocity.X);
            }
            else if (velocity.X < 0)
            {
                velocity.X = Math.Min(velocity.X + 0.1f, 0);//急に止まらない
            }

            //右
            if (Input.GetKey(Keys.Right) ||
                Input.GetKey(PlayerIndex.One, Buttons.DPadRight) ||
                Input.GetKey(PlayerIndex.One, Buttons.LeftThumbstickRight))
            {
                if (velocity.X < 1) velocity.X = Math.Min(velocity.X + acceleration, 1.0f);
                else velocity.X = Math.Min(velocity.X + acceleration, velocity.X);
            }
            else if (velocity.X > 0)
            {
                velocity.X = Math.Max(velocity.X - 0.1f, 0);//急に止まらない
            }

            if (velocity.X > 1) velocity.X -= 0.2f;
            if (velocity.X < -1) velocity.X += 0.2f;

            //ブロックとの当たり判定
            CheckX(velocity.X * speed);
        }

        //x方向スクロール
        private void ScrollX(float v)
        {
            //画面の5分の3移動でスクロール
            if (v > 0 && Camera.Position(position).X > Define.SCREEN_WIDTH * 3 / 5 ||
                v < 0 && Camera.Position(position).X < Define.SCREEN_WIDTH * 2 / 5)
            {
                Camera.Move(new Vector2(v, 0));
            }
        }

        //y方向のスクロール
        private void ScrollY(float v)
        {
            //画面の5分の3移動でスクロール
            if (v > 0 && Camera.Position(position).Y > Define.SCREEN_HEIGHT * 3 / 5 ||
                v < 0 && Camera.Position(position).Y < Define.SCREEN_HEIGHT * 2 / 5)
            {
                Camera.Move(new Vector2(0, v));
            }
        }

        #endregion 移動

        #region ジャンプ

        //ジャンプ
        private void Jump()
        {
            if (!is_jump)
            {
                if (Input.GetKeyDown(Keys.Z) ||
                    Input.GetKeyDown(PlayerIndex.One, Buttons.A))
                {
                    is_jump = true;
                }
            }

            if (is_jump)
            {
                if (Input.GetKey(Keys.Z) ||
                    Input.GetKey(PlayerIndex.One, Buttons.A))
                {
                    jump_counter++;

                    if (jump_counter < 15)//15フレーム
                    {
                        velocity.Y = -8.0f;

                        //最初
                        if (jump_counter == 1)
                        {
                            scale_timer.SetTime(0.15f);
                            scale = Vector2.One;

                            //パーティクル
                            emitter.Add(new HitParticleEmitter(
                                game_obj_m,
                                new Vector2(position.X + Rectangle().Width / 2, position.Y + Rectangle().Height / 2),
                                5));
                        }
                    }
                }
                else
                {
                    jump_counter = 15;
                }
            }

            if (velocity.Y > 0)//空中(落下中)
            {
                is_jump = true;
            }

            if (dush_speed == 0.0f)
            {
                if (is_clear) velocity.Y = Math.Min(velocity.Y + 0.8f / clear_speed, 8.0f);//落下
                else velocity.Y = Math.Min(velocity.Y + 0.8f, 8.0f);//落下
            }

            if (is_clear)
            {
                CheckY(velocity.Y / clear_speed);
            }
            else
            {
                CheckY(velocity.Y);
            }
        }

        #endregion ジャンプ

        #region ダッシュ

        private void Dush()
        {
            //ダッシュ
            if (!is_dush)
            {
                if (Input.GetKeyDown(Keys.X) ||
                   Input.GetKeyDown(PlayerIndex.One, Buttons.B))
                {
                    is_dush = true;//ダッシュ状態

                    if (Input.GetKey(Keys.Up) ||
                        Input.GetKey(PlayerIndex.One, Buttons.DPadUp) ||
                        Input.GetKey(PlayerIndex.One, Buttons.LeftThumbstickUp))
                    {
                        dush_velocity.Y -= 1.0f;//上
                        is_jump = true;
                    }
                    if (Input.GetKey(Keys.Down) ||
                        Input.GetKey(PlayerIndex.One, Buttons.DPadDown) ||
                        Input.GetKey(PlayerIndex.One, Buttons.LeftThumbstickDown))
                    {
                        dush_velocity.Y += 1.0f;//下
                    }
                    if (Input.GetKey(Keys.Left) ||
                        Input.GetKey(PlayerIndex.One, Buttons.DPadLeft) ||
                        Input.GetKey(PlayerIndex.One, Buttons.LeftThumbstickLeft))
                    {
                        dush_velocity.X -= 1.0f;//左
                    }
                    if (Input.GetKey(Keys.Right) ||
                        Input.GetKey(PlayerIndex.One, Buttons.DPadRight) ||
                        Input.GetKey(PlayerIndex.One, Buttons.LeftThumbstickRight))
                    {
                        dush_velocity.X += 1.0f;//右
                    }

                    if (dush_velocity == Vector2.Zero)//何もキーが入力されていない
                    {
                        dush_velocity.Y -= 1.0f;//上
                        is_jump = true;
                    }

                    dush_velocity.Normalize();//正規化

                    //スケール値
                    scale = Vector2.One;

                    //画面揺れ
                    Display display = GameDevice.Instance().Display;
                    display.is_wave = true;

                    //画面揺れ設定
                    if (dush_velocity.X != 0) display.is_x = true;//x方向に揺れるか
                    else display.is_x = false;
                    if (dush_velocity.Y != 0) display.is_y = true;//y方向に揺れるか
                    else display.is_y = false;
                    display.speed = 7.0f;//揺れ幅

                    dush_speed = 24.0f;//ダッシュ速度設定
                }
            }

            if (is_dush)
            {
                if (dush_speed > 0.0f)
                {
                    velocity.Y = 0.0f;//ダッシュ中は重力無効
                    if (is_clear)
                    {
                        dush_speed -= 1.5f / clear_speed;//減速
                    }
                    else
                    {
                        dush_speed -= 1.5f;//減速
                    }

                    if (dush_speed % 6 == 0)
                    {
                        Vector2 particle_vel = Vector2.Zero;
                        if (dush_velocity.X > 0) particle_vel.X = 1;
                        if (dush_velocity.X < 0) particle_vel.X = -1;
                        if (dush_velocity.Y > 0) particle_vel.Y = 1;
                        if (dush_velocity.Y < 0) particle_vel.Y = -1;
                        //emitter.Add(new DushParticleEmitter(
                        //    game_obj_m,
                        //    position,
                        //    3,
                        //    particle_vel));
                    }
                }
                else
                {
                    dush_speed = 0.0f;
                }

                if (dush_speed == 0.0f)
                {
                    dush_velocity = Vector2.Zero;
                }

                //ブロックとの当たり判定
                if (is_clear)
                {
                    CheckX(dush_velocity.X * dush_speed / clear_speed);
                    CheckY(dush_velocity.Y * dush_speed / clear_speed);
                }
                else
                {
                    CheckX(dush_velocity.X * dush_speed);
                    CheckY(dush_velocity.Y * dush_speed);
                }
            }
        }

        #endregion ダッシュ

        #region ブロックとの当たり判定

        //x方向のチェック
        private void CheckX(float velocity_x)
        {
            //進む方向がブロックかチェックする
            for (int x = 0; x < Math.Abs(velocity_x); x++)
            {
                if (velocity_x <= 0)
                {
                    //プレイヤーの左をチェック
                    foreach (var pos in check3)
                    {
                        if ((map.IsBlock(position + pos) is Block ||
                            map.IsBlock(position + pos) is BreakBlock))
                        {
                            //壁ずり落ち
                            if (velocity.Y > 1.5f)
                            {
                                velocity.Y = 0.0f;
                            }
                            
                            cur_tex = "player_l";

                            //ダッシュ停止
                            dush_velocity.X = 0.0f;

                            //落下中
                            if (velocity.Y != 0 && wall_timer.IsTime())
                            {
                                //パーティクル
                                emitter.Add(new WallParticleEmitter(
                                    game_obj_m,
                                    new Vector2(position.X, position.Y + Rectangle().Height / 2),
                                    2));
                                wall_timer.SetTime(0.1f);
                            }

                            //壁ジャンプ
                            if (Input.GetKeyDown(Keys.Z) ||
                                Input.GetKeyDown(PlayerIndex.One, Buttons.A))
                            {
                                //下にブロックがあったら壁ジャンプできない
                                foreach (var down_pos in check2)
                                {
                                    if (map.IsBlock(position + down_pos) is Block ||
                                        map.IsBlock(position + down_pos) is ThroughBlock ||
                                        map.IsBlock(position + down_pos) is BreakBlock)
                                    {
                                        return;
                                    }
                                }

                                //壁ジャンプ
                                is_jump = true;
                                velocity.X = 3.0f;  //x
                                velocity.Y = -10.0f;//y
                                
                                scale = Vector2.One;
                            }

                            return;
                        }

                        if (map.IsBlock(position + pos) is ThornBlock)
                        {
                            Dead();
                        }

                        if (map.IsBlock(position + pos) is ClearBlock)
                        {
                            if (!is_clear)
                            {
                                map.IsBlock(position + pos).Hit(this);
                            }
                            is_clear = true;
                            Camera.is_clear = true;
                        }

                        if (map.IsBlock(position + pos) is ItemBlock)
                        {
                            map.IsBlock(position + pos).Hit(this);
                        }
                    }
                }
                if (velocity_x >= 0)
                {
                    //プレイヤーの右をチェック
                    foreach (var pos in check4)
                    {
                        if ((map.IsBlock(position + pos) is Block ||
                            map.IsBlock(position + pos) is BreakBlock))
                        {
                            //壁ずり落ち
                            if (velocity.Y > 1.5f)
                            {
                                velocity.Y = 0.0f;
                            }

                            cur_tex = "player_r";

                            //ダッシュ停止
                            dush_velocity.X = 0.0f;

                            //落下中
                            if (velocity.Y != 0 && wall_timer.IsTime())
                            {
                                //パーティクル
                                emitter.Add(new WallParticleEmitter(
                                    game_obj_m,
                                    new Vector2(position.X + Rectangle().Width, position.Y + Rectangle().Height / 2),
                                    2));
                                wall_timer.SetTime(0.1f);
                            }

                            //壁ジャンプ
                            if (Input.GetKeyDown(Keys.Z) ||
                                Input.GetKeyDown(PlayerIndex.One, Buttons.A))
                            {
                                //下にブロックがあったら壁ジャンプできない
                                foreach (var down_pos in check2)
                                {
                                    if (map.IsBlock(position + down_pos) is Block ||
                                        map.IsBlock(position + down_pos) is ThroughBlock ||
                                        map.IsBlock(position + down_pos) is BreakBlock)
                                    {
                                        return;
                                    }
                                }

                                //壁ジャンプ
                                is_jump = true;
                                velocity.X = -3.0f;//x
                                velocity.Y = -10.0f;//y

                                //scale_timer.SetTime(0.1f);
                                scale = Vector2.One;
                            }

                            return;
                        }

                        if (map.IsBlock(position + pos) is ThornBlock)
                        {
                            Dead();
                        }

                        if (map.IsBlock(position + pos) is ClearBlock)
                        {
                            if (!is_clear)
                            {
                                map.IsBlock(position + pos).Hit(this);
                            }
                            is_clear = true;
                            Camera.is_clear = true;
                        }

                        if (map.IsBlock(position + pos) is ItemBlock)
                        {
                            map.IsBlock(position + pos).Hit(this);
                        }
                    }
                }

                position = new Vector2(
                    position.X + Math.Sign(velocity_x),
                    position.Y);
            }

            ScrollX(velocity_x);//スクロール
        }

        //y方向のチェック
        private void CheckY(float velocity_y)
        {
            //進む方向がブロックかチェックする
            for (int y = 0; y <= Math.Abs(velocity_y); y++)
            {
                if (velocity_y < 0)
                {
                    //プレイヤーの上をチェック
                    foreach (var pos in check1)
                    {
                        if ((map.IsBlock(position + pos) is Block ||
                            map.IsBlock(position + pos) is BreakBlock))
                        {
                            //y方向の移動を停止
                            velocity.Y = 0.0f;
                            dush_velocity.Y = 0.0f;

                            return;
                        }

                        if (map.IsBlock(position + pos) is ThornBlock)
                        {
                            Dead();
                        }

                        if (map.IsBlock(position + pos) is ClearBlock)
                        {
                            if (!is_clear)
                            {
                                map.IsBlock(position + pos).Hit(this);
                            }
                            is_clear = true;
                            Camera.is_clear = true;
                        }

                        if (map.IsBlock(position + pos) is ItemBlock)
                        {
                            map.IsBlock(position + pos).Hit(this);
                        }
                    }
                }
                else
                {
                    //プレイヤーの下をチェック
                    foreach (var pos in check2)
                    {
                        if ((map.IsBlock(position + pos) is Block ||
                            map.IsBlock(position + pos) is BreakBlock ||
                            (map.IsBlock(position + pos) is ThroughBlock &&
                            map.IsBlock(position + pos).position.Y >= position.Y + Rectangle().Height &&
                            (dush_velocity.Y + velocity.Y) >= 0)))
                        {
                            GameObject obj = map.IsBlock(position + pos);

                            if (obj is BreakBlock)
                            {
                                if (obj.state != State.Active)
                                {
                                    break;
                                }
                            }

                            //消えるブロックの衝突処理
                            if (map.IsBlock(position + pos) is BreakBlock)
                            {
                                map.IsBlock(position + pos).Hit(this);
                            }

                            //y方向の移動を停止
                            velocity.Y = 0.0f;
                            dush_velocity.Y = 0.0f;

                            if (dush_speed == 0.0f)
                            {
                                is_dush = false;
                            }

                            //着地した瞬間
                            if (is_jump)
                            {
                                scale_timer.SetTime(0.1f);
                                scale = Vector2.One;

                                //パーティクル
                                emitter.Add(new HitParticleEmitter(
                                    game_obj_m,
                                    new Vector2(position.X + Rectangle().Width / 2, position.Y + Rectangle().Height / 2),
                                    5));
                            }

                            is_jump = false;
                            jump_counter = 0;

                            return;
                        }

                        if (map.IsBlock(position + pos) is ThornBlock)
                        {
                            Dead();
                        }

                        if (map.IsBlock(position + pos) is ClearBlock)
                        {
                            if (!is_clear)
                            {
                                map.IsBlock(position + pos).Hit(this);
                            }
                            is_clear = true;
                            Camera.is_clear = true;
                        }

                        if (map.IsBlock(position + pos) is ItemBlock)
                        {
                            map.IsBlock(position + pos).Hit(this);
                        }
                    }
                }

                position = new Vector2(
                    position.X,
                    position.Y + Math.Sign(velocity_y));
            }

            ScrollY(velocity_y);//スクロール
        }

        #endregion ブロックとの当たり判定

        #region スケールアニメーション

        //スケール設定
        private void ScaleAnime(GameTime game_time)
        {
            scale_timer.Update(game_time);

            //ジャンプしたとき
            if (is_jump)//空中のみ
            {
                if (!scale_timer.IsTime())
                {
                    scale += new Vector2(-0.04f, 0.04f);
                }
                else if (scale.X < 1.0f)
                {
                    scale += new Vector2(0.04f, -0.04f);
                }
            }
            //着地したとき
            else
            {
                if (!scale_timer.IsTime())
                {
                    scale += new Vector2(0.2f, -0.0f);
                }
                else if (scale.X > 1.0f)
                {
                    scale += new Vector2(-0.2f, 0.0f);
                }
            }

            if (velocity.Y > 0)
            {
                scale = Vector2.One;
            }
        }

        #endregion スケールアニメーション

        //死亡処理
        private void Dead()
        {
            if (!is_dead)
            {
                is_dead = true;

                //パーティクル
                emitter.Add(new DeadParticleEmitter(game_obj_m, position, 12));

                //画面揺れ
                Display display = GameDevice.Instance().Display;
                display.is_wave = true;

                //画面揺れ設定
                display.is_x = true;//x方向に揺れるか
                display.is_y = true;//y方向に揺れるか
                display.speed = 7.0f;//揺れ幅
            }
        }

        //描画
        public override void Draw(Render render)
        {
            if (!is_dead)
            {
                //画像の幅と高さ
                int tex_w = 50;
                int tex_h = 55;

                //残像の描画
                for (int i = 0; i < afterimage_pos.Count; i++)
                {
                    render.Draw(
                        cur_tex,
                        Camera.Position(new Vector2(afterimage_pos[i].X + Rectangle().Width / 2, afterimage_pos[i].Y + Rectangle().Height - tex_h / 2)) + GameDevice.Instance().pos,
                        motions[cur_tex].DrawRectangle(),
                        render.color * afterimage_alpha[i],
                        render.rotation,
                        new Vector2(tex_w / 2, tex_h / 2),
                        afterimage_scale[i] * render.scale);
                }

                render.Draw(
                    cur_tex,
                    Camera.Position(new Vector2(position.X + Rectangle().Width / 2, position.Y + Rectangle().Height - tex_h / 2)) + GameDevice.Instance().pos,
                    motions[cur_tex].DrawRectangle(),
                    render.color,
                    render.rotation,
                    new Vector2(tex_w / 2, tex_h / 2),
                    scale * render.scale);
            }
        }

        public Vector2 vel
        {
            set { velocity = value; }
            get { return velocity; }
        }

        //衝突処理
        public override void Hit(GameObject game_obj)
        {

        }

        public Player(Player player) : this(player.game_obj_m, player.position, player.map) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new Player(this);
        }
    }
}
