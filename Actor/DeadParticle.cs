using Game1.Device;
using Microsoft.Xna.Framework;
using System;

namespace Game1.Actor
{
    class DeadParticle : Particle
    {
        private GameObjectManager game_obj_m;
        private Map map;

        private Vector2 revival_pos;

        public DeadParticle(Vector2 position, GameObjectManager game_obj_m) : base(position)
        {
            this.game_obj_m = game_obj_m;
        }

        public override void Init()
        {
            float radian = (float)random.Next(360) / 180 * (float)Math.PI;

            vel = new Vector2(
                (float)Math.Cos(radian),
                (float)Math.Sin(radian));
            speed = random.Next(8, 15) + 10;
            scale = (float)random.Next(70, 140) / 100;
            alpha = 1.0f;
            life_timer = game_obj_m.Player().revival_timer;//プレイヤーの復活時間

            width = height = 40;

            map = game_obj_m.map;

            //プレイヤーが復活する座標を計算
            for (int y = 0; y < map.numeric_map.Length; y++)
            {
                for (int x = 0; x < map.numeric_map[y].Length; x++)
                {
                    if (map.numeric_map[y][x] == 1)
                    {
                        if (x >= map.map_data[map.current_map][1] &&
                            x <= map.map_data[map.current_map][1] + map.map_data[map.current_map][3] - 1 &&
                            y >= map.map_data[map.current_map][2] &&
                            y <= map.map_data[map.current_map][2] + map.map_data[map.current_map][4] - 1)
                        {
                            revival_pos = new Vector2(x * Define.MAP_WIDTH, y * Define.MAP_HEIGHT);
                        }
                    }
                }
            }
        }

        public override void Update(GameTime game_time)
        {
            if (speed > 0.0f)
            {
                speed -= 0.5f;
                position += vel * speed;
            }
            else
            {
                speed = 0.0f;
            }

            //プレイヤーが復活する座標に移動
            position = Vector2.Lerp(position, revival_pos, life_timer.Rate() / 3);
        }

        public override void Draw(Render render)
        {
            render.Draw(
                "dead_p",
                Camera.Position(position) + GameDevice.Instance().pos,
                null,
                render.color * alpha,
                render.rotation,
                new Vector2(width / 2, height / 2),
                new Vector2(scale) * render.scale);
        }

        public override void Hit(GameObject game_obj)
        {

        }

        public DeadParticle(DeadParticle dead_particle) : this(dead_particle.position, dead_particle.game_obj_m) { }//コピーコンストラクタ

        public override object Clone()
        {
            return new DeadParticle(this);
        }
    }
}
