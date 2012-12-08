using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeometryWars
{
    class Collision
    {
        public static bool CheckCollision(Vector2 posA, Texture2D texA, float angleA, Vector2 posB, Texture2D texB, float angleB)
        {
            if (!RectCollision(posA, texA, posB, texB))
                return false;
            if (PixelCollision(posA, texA, angleA, posB, texB, angleB))
                return true;
            return false;
        }

        public static bool RectCollision(Vector2 posA, Texture2D texA, Vector2 posB, Texture2D texB)
        {
            Rectangle A = new Rectangle((int)posA.X, (int)posA.Y, texA.Width, texA.Height);
            Rectangle B = new Rectangle((int)posB.X, (int)posB.Y, texB.Width, texB.Height);
            return A.Intersects(B);
        }

        public static bool PixelCollision(Vector2 posA, Texture2D texA, float angleA, Vector2 posB, Texture2D texB, float angleB)
        {            
            Color[] dataA = new Color[texA.Width * texA.Height];
            Color[] dataB = new Color[texB.Width * texB.Height];

            texA.GetData(dataA);
            texB.GetData(dataB);

            Vector3 originA = new Vector3(texA.Width / 2, texA.Height / 2, 0.0f);
            Vector3 originB = new Vector3(texB.Width / 2, texB.Height / 2, 0.0f);

            Matrix transformA = Matrix.CreateTranslation(-originA) * Matrix.CreateRotationZ(angleA) * Matrix.CreateTranslation(originA + new Vector3(posA, 0.0f));
            Matrix transformB = Matrix.CreateTranslation(-originB) * Matrix.CreateRotationZ(angleB) * Matrix.CreateTranslation(originB + new Vector3(posB, 0.0f));
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            Vector2 xUnitInB = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 yUnitInB = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            for (int yA = 0; yA < texA.Height; yA++)
            {
                Vector2 posInB = yPosInB;

                for (int xA = 0; xA < texA.Width; xA++)
                {
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    if (xB >= 0 && xB < texB.Width && yB >= 0 && yB < texA.Height)
                    {
                        Color colorA = dataA[xA * texA.Width + yA];
                        Color colorB = dataB[xB * texB.Width + yB];

                        if (colorA != Color.Transparent && colorB != Color.Transparent)
                            return true;
                    }

                    posInB += xUnitInB;
                }

                yPosInB += yUnitInB;
            }

            return false;
        }
    }
}
