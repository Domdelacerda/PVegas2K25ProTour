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
        public float Rotation;
        public Vector2 Scale;
        




        public Sprite(Texture2D golfBall)
        {
            this.golfBall = golfBall;
            
        }

        public void Update()
        {
           
        }

        

        public void Draw(SpriteBatch spriteBatch) {
            
            spriteBatch.Draw(this.golfBall, this.Position, null, Color.White, Rotation, Origin, 1, SpriteEffects.None, 0f);
        
        
        }
    }
}
