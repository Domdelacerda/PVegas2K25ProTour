using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spike1
{
    internal class Sprite
    {
        private Texture2D golfBall;


        public Vector2 Position;
        public Vector2 Origin;
        private float rotation;

        public int distance;

        public float RotationVelocity = 10f;



        public Sprite(Texture2D golfBall)
        {
            this.golfBall = golfBall;

        }

        public void Update()
        {

        }



        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(golfBall, Position, null, Color.White, rotation, Origin, 1, SpriteEffects.None, 0f);


        }
    }
}
