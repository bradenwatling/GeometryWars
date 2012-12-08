using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryWars
{
    public class Input
    {
        #region Variables
        double lastBulletTime = 0;
        #endregion

        #region Functions
        public Input()
        {
            
        }

        public bool Handle(GameTime gameTime, Player player, int width, int height)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                return false;

            if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
            {
                GamePadState padState = GamePad.GetState(PlayerIndex.One);

                if (padState.Buttons.Start == ButtonState.Pressed)
                    return false;

                double now = gameTime.TotalGameTime.TotalMilliseconds;
                if (now - lastBulletTime > Player.Bullet.bulletDelay)
                {
                    lastBulletTime = now;
                    if (padState.ThumbSticks.Right != Vector2.Zero)
                    {
                        player.AddBullet(new Vector2(player.Position.X + padState.ThumbSticks.Right.X, player.Position.Y - padState.ThumbSticks.Right.Y));
                    }
                }

                if (padState.ThumbSticks.Left != Vector2.Zero)
                {
                    player.direction.Y = -padState.ThumbSticks.Left.Y;
                    player.direction.X = padState.ThumbSticks.Left.X;
                    player.angle = (float)Math.Atan(player.direction.Y / player.direction.X);
                    player.angle -= player.direction.X < 0 ? (float)Math.PI : 0.0f;
                    player.Move();
                }
            }
            else
            {
                double now = gameTime.TotalGameTime.TotalMilliseconds;
                if (now - lastBulletTime > Player.Bullet.bulletDelay)
                {
                    lastBulletTime = now;
                    MouseState mouseState = Mouse.GetState();
                    if(mouseState.LeftButton.HasFlag(ButtonState.Pressed))
                        player.AddBullet(new Vector2(mouseState.X, mouseState.Y));
                }

                Keys[] keys = Keyboard.GetState().GetPressedKeys();

                foreach (Keys key in keys)
                {
                    switch (key)
                    {
                        case Keys.W:
                            player.Move();
                            break;
                        case Keys.A:
                            player.Rotate(false);
                            break;
                        case Keys.D:
                            player.Rotate(true);
                            break;
                        case Keys.Escape:
                            return false;
                    }
                }
            }
            return true;

        }
        #endregion
    }
}
