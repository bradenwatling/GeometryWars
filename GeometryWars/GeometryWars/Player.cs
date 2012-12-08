using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryWars
{
    public class Player
    {
        #region Variables
        public struct Bullet
        {
            public Vector2 Position;
            public Vector2 direction;
            public float angle;
            public const float speed = 20.0f;
            public const float bulletDelay = 100;
        };

        public static Texture2D playerTexture;
        public static Texture2D bulletTexture;

        public List<Bullet> bullets = new List<Bullet>();

        ParticleEngine particleEngine;

        public Vector2 Position = new Vector2(250);
        public Vector2 direction = Vector2.UnitX;
        public float angle = 0.0f;
        float speed = 7.5f;
        float rotationSpeed = MathHelper.ToRadians(5.0f);
        #endregion

        #region Functions
        public Player(ParticleEngine particle)
        {
            particleEngine = particle;
        }

        public void Move()
        {
            Position += direction * speed;
        }

        public void Move(float amount)
        {
            Position += amount > 0 ? direction * speed * amount : Vector2.Zero;
        }

        public void Rotate(bool dir)
        {
            angle += dir ? rotationSpeed : -rotationSpeed;
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void Rotate(float rot)
        {
            angle += rot * rotationSpeed;
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, Position, playerTexture.Bounds, Color.White, angle, new Vector2(playerTexture.Bounds.Center.X, playerTexture.Bounds.Center.Y), 1.0f, SpriteEffects.None, 0.0f);
        }

        public void AddBullet(Vector2 dir)
        {
            //Recalculate direction vector to give a decimal speed
            float bulletAngle = (float)Math.Atan((dir.Y - Position.Y) / (dir.X - Position.X));
            if (dir.X < Position.X)
                bulletAngle += (float)Math.PI;

            Bullet bullet = new Bullet();

            bullet.direction = new Vector2((float)Math.Cos(bulletAngle), (float)Math.Sin(bulletAngle));
            bullet.Position = Position;
            bullet.angle = bulletAngle;
            bullets.Add(bullet);
        }

        public void UpdateBullets(GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet bullet = bullets[i];
                if (graphicsDevice.Viewport.Bounds.Contains(new Point((int)bullet.Position.X, (int)bullet.Position.Y)))
                {
                    bullet.Position += bullet.direction * Bullet.speed;
                    bullets[i] = bullet;
                }
                else
                    bullets.Remove(bullet);
            }
        }

        public void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in bullets)
                spriteBatch.Draw(bulletTexture, bullet.Position, bulletTexture.Bounds, Color.White, bullet.angle, new Vector2(bulletTexture.Bounds.Center.X, bulletTexture.Bounds.Center.Y), 1.0f, SpriteEffects.None, 0.0f);
        }
        #endregion
    }
}
