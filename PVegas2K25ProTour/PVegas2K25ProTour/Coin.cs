//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have coins in the game that the user can collect in levels and
// earn rewards from
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Coin checks for collisions with a ball every frame, and if a collision
    /// is detected, a random amount of money is awarded to the player
    /// </summary>-------------------------------------------------------------
    public class Coin : GameObject
    {
        private Texture2D coin_texture;
        private Vector2 pos;
        //private float rad;
        private Hitbox hitbox;
        private SpriteBatch sprite_batch;
        private Vector2 scale;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        public Coin(Vector2 pos,SpriteBatch spriteBatch) : base(pos, 
            spriteBatch)
        {
            this.pos = pos;
            //this.spriteBatch = spriteBatch;
            scale = Vector2.One;
            hitbox = new Hitbox();
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public virtual void LoadContent(ContentManager content_)
        {
            coin_texture = content_.Load<Texture2D>("CoinPVegas");
        }

        public bool Update(Ball ball)
        {
            if (hitbox.collisionCircleToCircle(ball, this))
            {
                //collide(ball);
                return true;
            }
            return false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.sprite_batch = spriteBatch;
            this.sprite_batch.Draw(coin_texture, new Rectangle((int)pos.X, 
                (int)pos.Y, (int)coin_texture.Width / 10, 
                (int)coin_texture.Height / 10), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------


        /// <summary>----------------------------------------------------------
        /// Gets the radius of the coin based on the size of its sprite
        /// </summary>
        /// <returns>the radius of the coin.</returns>
        /// -------------------------------------------------------------------
        public override float radius()
        {
            return (coin_texture.Height / 10) / 2; 
        }

        /// <summary>----------------------------------------------------------
        /// Gets the position of the center of the coin based on it's drawn
        /// position and its radius
        /// </summary>
        /// <returns>the position of the center of the coin.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = new Vector2();
            center.X = pos.X;
            center.Y = pos.Y;
            center.X += radius();
            center.Y += radius();
            return center;
        }

        /// <summary>----------------------------------------------------------
        /// Generates a random amount of money to give the player and returns
        /// it
        /// </summary>
        /// <returns>the amount of money the coin is worth.</returns>
        /// -------------------------------------------------------------------
        public int moneyAmount()
        {
            Random rand = new Random();
            int coinCost = (int)rand.NextInt64(2,5);
            return coinCost;
        }
    }

   
}
