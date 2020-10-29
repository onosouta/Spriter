using Game1.Device;
using Game1.Utility;
using Microsoft.Xna.Framework;
using System;

namespace Game1.Actor
{
    class ItemBlock : GameObject
    {
        private GameObjectManager game_obj_m;
        private bool is_hit;//衝突したか

        private Vector2 draw_pos;
        private Vector2 vel;
        private Timer move_timer;
        private int move_counter;
        private float angle;
        private Motion motion;
        private ItemParticleEmitter emitter;
        private ItemBlockParticle particle;
        private float alpha;

        public ItemBlock(Vector2 position, GameObjectManager game_obj_m) : base(position)
        {
            this.game_obj_m = game_obj_m;
        }

        public ItemBlock(ItemBlock item_block) : this(item_block.position, item_block.game_obj_m) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new ItemBlock(this);//コピー
        }

        public override void Init()
        {
            width = 44;
            height = 40;

            is_hit = false;

            draw_pos = position;
            vel = new Vector2(0, -3);
            move_timer = new CountDownTimer(0.5f);
            move_counter = 1;
            angle = 0;
            alpha = 3.0f;
            motion = new Motion(new Range(0, 5), new CountDownTimer(0.03f));
            motion.Add(0, new Rectangle(0 * 44, 0, 44, 40));
            motion.Add(1, new Rectangle(1 * 44, 0, 44, 40));
            motion.Add(2, new Rectangle(2 * 44, 0, 44, 40));
            motion.Add(3, new Rectangle(3 * 44, 0, 44, 40));
            motion.Add(4, new Rectangle(4 * 44, 0, 44, 40));
            motion.Add(5, new Rectangle(5 * 44, 0, 44, 40));

            particle = new ItemBlockParticle(position + new Vector2(Define.MAP_WIDTH / 2, Define.MAP_HEIGHT / 2), game_obj_m);
            particle.Init();
        }

        public override void Update(GameTime game_time)
        {
            if (!is_hit)
            {
                move_timer.Update(game_time);
                if (move_timer.IsTime())
                {
                    move_counter++;
                    if (move_counter >= 2)
                    {
                        move_counter = 0;
                        vel.Y *= -1;
                    }

                    draw_pos += vel;
                    move_timer.SetTime(0.5f);
                }

                particle.Update(game_time);
            }
            else
            {
                angle += 5;
                if (angle <= 180)
                {
                    draw_pos.Y = position.Y - (float)Math.Sin(MathHelper.ToRadians(angle)) * 100;
                    motion.Update(game_time);
                }
                alpha -= 0.1f;
                //else
                //{
                //    position = Vector2.Lerp(position, Camera.position + new Vector2(0, 70), 0.1f);
                //    draw_pos = position;
                //}
            }

            if (emitter != null)
            {
                emitter.Update(game_time);
                emitter = null;
            }
        }

        public override void Draw(Render render)
        {
            if (angle <= 180)
            {
                render.Draw(
                    "item",
                    Camera.Position(draw_pos) + GameDevice.Instance().pos + new Vector2(Define.MAP_WIDTH / 2, Define.MAP_HEIGHT / 2) - new Vector2(width / 2, height / 2),
                    motion.DrawRectangle(),
                    render.color * alpha,
                    render.rotation,
                    render.origin,
                    render.scale);
            }

            if (!is_hit)
            {
                particle.Draw(render);
            }
        }

        public override void Hit(GameObject game_obj)
        {
            if (!is_hit)
            {
                game_obj_m.item_counter++;
                emitter = new ItemParticleEmitter(game_obj_m, position + new Vector2(Define.MAP_WIDTH / 2, Define.MAP_HEIGHT / 2), 8);
            }

            is_hit = true;
        }
    }
}
