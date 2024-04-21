using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;



namespace PVegas2K25ProTour
{
    public class Coin : GameObject
    {
        private Texture2D coinTexture;
        private Vector2 pos;
        //private float rad;
        private Hitbox hitbox;
        private SpriteBatch spriteBatch;
        private Vector2 scale;

        public Coin(Vector2 pos,SpriteBatch spriteBatch) : base(pos, spriteBatch)
        {
            this.pos = pos;
            //this.spriteBatch = spriteBatch;
            scale = Vector2.One;
            hitbox = new Hitbox();
            
        }
        public override float  radius()
        {
            return (coinTexture.Height / 10) / 2; 
        }
        public virtual void LoadContent(ContentManager content_)
        {
            coinTexture = content_.Load<Texture2D>("CoinPVegas");
        }
        public override Vector2 center()
        {
            Vector2 center = new Vector2();
            center.X = pos.X;
            center.Y = pos.Y;
            center.X += radius();
            center.Y += radius();
            return center;
        }
        public bool Update(Ball ball)
        {
            if(hitbox.collisionCircleToCircle(ball, this))
            {
                //collide(ball);
                return true;
            }
            return false;
        }
        public virtual void collide(Ball b)
        {
            pos.X = 100000000.0f;
            pos.Y = 100000000.0f;
        }
        public float moneyAmount()
        {
            Random rand = new Random();
            float coinCost = (float)rand.NextInt64(1,16);
            return coinCost;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.spriteBatch.Draw(coinTexture,new Rectangle((int)pos.X,(int)pos.Y,(int)coinTexture.Width/10,(int)coinTexture.Height/10), Color.White);

        }
    }
}
