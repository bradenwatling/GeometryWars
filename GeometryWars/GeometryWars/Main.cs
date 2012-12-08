using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GeometryWars
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Effect particleEffect;
        Effect bulletEffect;
        Effect playerEffect;
        Effect screenEffect;

        Texture2D screenEffectTexture;
        Texture2D screenTexture;

        ParticleEngine particleEngine;
        Player player;
        Enemy enemy;
        Enemy enemy2;

        Input input;

        int num = 0;
        #endregion

        #region Functions
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 780;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            particleEffect = Content.Load<Effect>("particleEffect");
            bulletEffect = Content.Load<Effect>("bulletEffect");
            playerEffect = Content.Load<Effect>("playerEffect");
            screenEffect = Content.Load<Effect>("ScreenEffect");

            ParticleEngine.particleTexture = Content.Load<Texture2D>("ParticleTexture");
            Enemy.enemyTexture = Content.Load<Texture2D>("PlayerTexture");
            Enemy.enemyBlockerTexture = Content.Load<Texture2D>("PlayerTexture");
            Player.playerTexture = Content.Load<Texture2D>("PlayerTexture");
            Player.bulletTexture = Content.Load<Texture2D>("BulletTexture");

            particleEngine = new ParticleEngine();
            enemy = new Blocker(new Vector2(500));
            enemy2 = new Enemy(new Vector2(100));
            player = new Player(particleEngine);
            
            screenEffectTexture = Content.Load<Texture2D>("ScreenEffectTexture");
            screenTexture = Content.Load<Texture2D>("ScreenTexture");

            input = new Input();
        }

        bool collision = false;

        protected override void Update(GameTime gameTime)
        {
            particleEngine.NewParticleEngine(player.Position, gameTime);
            particleEngine.UpdateParticles(gameTime);

            foreach (Player.Bullet bullet in player.bullets)
            {
                collision = Collision.CheckCollision(bullet.Position, Player.bulletTexture, bullet.angle, enemy.Position, enemy.texture, player.angle) ? true : collision;
            }

            if (!collision)
            {
                enemy.UpdateEnemy(player);
                enemy2.UpdateEnemy(player);
            }

            player.UpdateBullets(graphics.GraphicsDevice);

            //if (Collision.RectCollision(player.Position, Player.playerTexture, enemy.Position, enemy.texture))
                //Collision.PixelCollision(player.Position, Player.playerTexture, player.angle, enemy.Position, enemy.texture, player.angle);

            if (!input.Handle(gameTime, player, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight))
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            graphics.GraphicsDevice.Textures[1] = screenEffectTexture;

            num++;
            particleEffect.Parameters["num"].SetValue(num);
            bulletEffect.Parameters["num"].SetValue(num);
            playerEffect.Parameters["num"].SetValue(num);
            screenEffect.Parameters["num"].SetValue(num);

            //Particle engine
            spriteBatch.Begin(0, null, null, null, null, particleEffect);
            particleEngine.DrawParticles(spriteBatch);
            spriteBatch.End();

            //Bullets
            spriteBatch.Begin(0, null, null, null, null, bulletEffect);
            player.DrawBullets(spriteBatch);
            spriteBatch.End();

            //Enemy
            spriteBatch.Begin(0, null, null, null, null, playerEffect);
            if(!collision)
                enemy.Draw(spriteBatch);
            enemy2.Draw(spriteBatch);
            spriteBatch.End();

            //Player
            spriteBatch.Begin(0, null, null, null, null, playerEffect);
            player.Draw(spriteBatch);
            spriteBatch.End();

            //Screen
            spriteBatch.Begin(0, null, null, null, null, screenEffect);
            spriteBatch.Draw(screenTexture, graphics.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
