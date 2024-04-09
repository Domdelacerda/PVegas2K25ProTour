using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;




namespace PVegas2K25ProTour
{
    public class Coin : GameObject
    {
        private Texture2D coinTexture;
        private Vector2 pos;
        private float rad;
        private Hitbox hitbox;
        private SpriteBatch spriteBatch;
        private Vector2 scale;

        public Coin(Vector2 pos, SpriteBatch spriteBatch)
        {
            this.pos = pos;
            this.spriteBatch = spriteBatch;
            scale = Vector2.One;
            
        }
        public override float  radius()
        {
            return coinTexture.Height / 2;
        }
        public void LoadContent(ContentManager content_)
        {
            coinTexture = content_.Load<Texture2D>("CoinPVegas");
        }
        public void Update(Ball ball)
        {
            if(hitbox.collisionCircleToCircle(ball, this))
            {
                collide(ball);
            }
        }
        public virtual void collide(Ball b)
        {

        }
        public void Draw()
        {
            spriteBatch.Draw(coinTexture, pos, Color.White);
        }
    }
}
