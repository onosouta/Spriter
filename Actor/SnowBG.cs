using Game1.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game1.Actor
{
    class SnowBG
    {
        struct Snow
        {
            public Vector2 position;
            public float x;
            public float y;
            public Vector2 scroll_vel;
            public int angle;
            public int add_angle;
            public float scale;
            public bool is_dead;
        }

        private List<Snow> snows;
        private int snow_amount;

        private int width, height;

        public SnowBG()
        {
            width = height = 5;

            snows = new List<Snow>();
            snow_amount = 60;
            for (int i = 0; i < snow_amount; i++)
            {
                Snow snow = new Snow();
                snow.position = new Vector2(
                    GameDevice.Instance().Random.Next(0, Define.SCREEN_WIDTH + width / 2),
                    GameDevice.Instance().Random.Next(0, Define.SCREEN_HEIGHT + height / 2));
                snow.x = -(float)GameDevice.Instance().Random.Next(10, 80) / 10;
                snow.y = snow.x / 6.0f;
                snow.angle = GameDevice.Instance().Random.Next(360);
                snow.add_angle = GameDevice.Instance().Random.Next(1, 4);
                snow.scale = -snow.x / 10.0f + 0.3f;
                snow.is_dead = false;
                snows.Add(snow);
            }
        }

        private void InitSnow(ref Snow snow)
        {
            snow.position = new Vector2(
                Define.SCREEN_WIDTH + width / 2,
                GameDevice.Instance().Random.Next(0, Define.SCREEN_HEIGHT + height / 2));
            snow.x = -(float)GameDevice.Instance().Random.Next(10, 80) / 10;
            snow.y = snow.x / 6.0f;
            snow.angle = GameDevice.Instance().Random.Next(360);
            snow.add_angle = GameDevice.Instance().Random.Next(1, 4);
            snow.scale = -snow.x / 10.0f + 0.3f;
            snow.is_dead = false;
        }

        public void Update(GameTime game_time)
        {
            if (!Camera.is_move)
            {
                for (int i = 0; i < snow_amount; i++)
                {
                    Snow s = new Snow();

                    int angle = snows[i].angle;
                    angle += snows[i].add_angle;
                    s.angle = angle;
                    s.add_angle = snows[i].add_angle;

                    s.position = snows[i].position;
                    s.x = snows[i].x;
                    s.y = snows[i].y;
                    s.position += new Vector2(
                        s.x,
                        (float)Math.Sin(MathHelper.ToRadians(s.angle)) * s.y);

                    s.scale = snows[i].scale;

                    s.is_dead = snows[i].is_dead;
                    if (s.position.X <= -width)
                    {
                        s.is_dead = true;
                        InitSnow(ref s);
                    }

                    snows[i] = s;
                }
            }

        }

        public void FlontDraw(Render render)
        {
            for (int i = 0; i < snow_amount; i++)
            {
                if (snows[i].scale >= 0.5f)
                {
                    render.Draw(
                        "snow",
                        snows[i].position + GameDevice.Instance().pos,
                        null,
                        render.color,
                        render.rotation,
                        new Vector2(width / 2, height / 2),
                        new Vector2(snows[i].scale));
                }
            }
        }

        public void BackDraw(Render render)
        {
            for (int i = 0; i < snow_amount; i++)
            {
                if (snows[i].scale < 0.5f)
                {
                    render.Draw(
                    "snow",
                    snows[i].position + GameDevice.Instance().pos,
                    null,
                    render.color,
                    render.rotation,
                    new Vector2(width / 2, height / 2),
                    new Vector2(snows[i].scale) * render.scale);
                }
            }
        }
    }
}
