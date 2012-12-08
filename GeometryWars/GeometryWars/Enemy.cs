using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryWars
{
    public class Enemy
    {
        #region Variables
        public static  Texture2D enemyTexture;
        public static Texture2D enemyBlockerTexture;

        public Texture2D texture;

        public Vector2 Position = Vector2.Zero;
        protected Vector2 direction = Vector2.Zero;
        protected float angle;
        protected float speed = 6.5f;
        #endregion

        #region Functions
        public Enemy(Vector2 pos)
        {
            Position = pos;

            texture = enemyTexture;
            switch (this.ToString().Split('.')[1])
            {
                case "Blocker":
                    texture = enemyBlockerTexture;
                    break;
            }
        }

        public virtual void Calculate(Player player)
        {
            angle = (float)Math.Atan((player.Position.Y - Position.Y) / (player.Position.X - Position.X));
            if (player.Position.X < Position.X)
                angle += (float)Math.PI;
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            //direction = player.Position - Position;
            Position += direction * speed;
        }

        public void UpdateEnemy(Player player)
        {
            Calculate(player);

            angle = (float)Math.Atan((direction.Y) / (direction.X)) + (Position.X > player.Position.X ? (float)Math.PI : 0.0f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, texture.Bounds, Color.White, angle, new Vector2(texture.Bounds.Center.X, texture.Bounds.Center.Y), 1.0f, SpriteEffects.None, 0.0f);
        }
        #endregion
    }

    class Blocker : Enemy
    {
        const float BlockerStartDistance = 2000.0f;
        const float BlockerStopDistance = 150.0f;
        new float speed = 4.0f;

        public Blocker(Vector2 pos) : base(pos)
        {
        }

        public override void Calculate(Player player)
        {
            angle = (float)Math.Atan((player.Position.Y - Position.Y) / (player.Position.X - Position.X));
            if (player.Position.X < Position.X)
                angle += (float)Math.PI;
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            if (Vector2.Distance(Position, player.Position) < Blocker.BlockerStartDistance && Vector2.Distance(Position, player.Position) > Blocker.BlockerStopDistance)
            {
                angle = (float)Math.Atan(direction.Y / direction.X);
                angle += MathHelper.ToRadians(90);
                Vector2 perpendicular = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                direction += angle < Math.PI ? -perpendicular : perpendicular;
            }

            Position += direction * speed;
        }
    };
}
