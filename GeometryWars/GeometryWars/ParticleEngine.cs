using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryWars
{
    public class ParticleEngine
    {
        #region Variables
        public struct Particle
        {
            public double BirthTime;
            public float MaxAge;
            public float Acceleration;
            public float Direction;
            public Vector2 Velocity;
            public Vector2 Position;
            public float Scaling;
            public Color Color;
            public float Alpha;
        }

        public static Texture2D particleTexture;

        List<Particle> particles = new List<Particle>();
        Random rand = new Random();
        #endregion

        #region Functions
        public ParticleEngine()
        {
        }

        public void NewParticleEngine(Vector2 pos, GameTime gameTime)
        {
            int num = 1;//rand.Next(50);
            for (int i = 0; i < num; i++)
                AddParticle(pos, gameTime);
        }

        public void UpdateParticles(GameTime gameTime)
        {
            double now = gameTime.TotalGameTime.TotalMilliseconds;
            for (int i = 0; i < particles.Count; i++)
            {
                Particle particle = particles[i];
                if (now - particle.BirthTime > particle.MaxAge)
                {
                    particles.Remove(particle);
                    //AddParticle(particle.OriginalPosition, gameTime);
                }
                else
                {
                    particle.Direction += 0.1f;
                    particle.Velocity = new Vector2(particle.Acceleration * (float)Math.Cos(particle.Direction), particle.Acceleration * (float)Math.Sin(particle.Direction));
                    particle.Velocity.X -= particle.Velocity.X / 100 / particle.MaxAge;
                    particle.Velocity.Y -= particle.Velocity.Y / 100 / particle.MaxAge;
                    //particle.Position -= particleList[i].Velocity;
                    particle.Alpha += 2 / particle.MaxAge;
                    particle.Color = Color.Lerp(particle.Color, Color.Transparent, particle.Alpha);
                    particles[i] = particle;
                }
            }
        }

        public void DrawParticles(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                Particle particle = particles[i];
                    spriteBatch.Draw(particleTexture, particle.Position, null, particle.Color, particle.Direction, Vector2.Zero, particle.Scaling, SpriteEffects.None, 1);
            }
        }

        private void AddParticle(Vector2 pos, GameTime gameTime)
        {
            Particle particle = new Particle();
            particle.Position = pos;

            particle.BirthTime = gameTime.TotalGameTime.TotalMilliseconds;
            particle.MaxAge =  rand.Next(2000);
            particle.Scaling = (float)rand.NextDouble() / 50;
            particle.Color = new Color(rand.Next(255), rand.Next(255), rand.Next(255));
            particle.Alpha = 0.0f;

            float angle = MathHelper.ToRadians(rand.Next(360));
            particle.Direction = MathHelper.ToRadians(rand.Next(360));
            particle.Acceleration = (float)rand.NextDouble() * rand.Next(20);

            particle.Velocity = new Vector2(particle.Acceleration * (float)Math.Cos(particle.Direction), particle.Acceleration * (float)Math.Sin(particle.Direction));

            particles.Add(particle);
        }
        #endregion
    }
}
