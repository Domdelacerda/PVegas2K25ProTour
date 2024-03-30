//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have an obstacle that balls can collide with and bounce off of
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Obstacle checks for collisions with a ball and upon collision inverts
    /// that ball's velocity to appear as though it bounces off
    /// </summary>-------------------------------------------------------------
    public class Obstacle : GameObject
    {
        private SpriteBatch _sprite_batch;
        private Texture2D obstacle_sprite;
        private Vector2 obstacle_pos;
        private Hitbox hitbox;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new obstacle using a specified position and sprite 
        /// batch
        /// </summary>
        /// <param name="obstacle_pos">the position of the obstacle at the 
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the obstacle's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public Obstacle(Vector2 obstacle_pos, SpriteBatch _sprite_batch) : 
            base(obstacle_pos, _sprite_batch)
        {
            this.obstacle_pos = obstacle_pos;
            this._sprite_batch = _sprite_batch;
        }

        /// <summary>----------------------------------------------------------
        /// Constructs a new obstacle using a specified position, sprite 
        /// batch, and hitbox
        /// </summary>
        /// <param name="obstacle_pos">the position of the obstacle at the 
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the obstacle's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// <param name="hitbox">the hitbox the obstacle uses for detecting
        /// collisions with other game objects.</param>
        /// -------------------------------------------------------------------
        public Obstacle(Vector2 obstacle_pos, SpriteBatch _sprite_batch, 
            Hitbox hitbox) : base(obstacle_pos, _sprite_batch, hitbox)
        {
            this.obstacle_pos = obstacle_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public virtual void LoadContent(ContentManager _content)
        {
            obstacle_sprite = _content.Load<Texture2D>("Sand");
        }

        public virtual void Draw()
        {
            _sprite_batch.Draw(obstacle_sprite, obstacle_pos, Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>
        /// Checks for collisions with a ball game object every frame
        /// </summary>
        /// <param name="ball">the ball that can collide with this obstacle.
        /// </param>
        public void Update(Ball ball)
        {
            if (hitbox.collisionCircleToCircle(ball, this))
            {
                collide(ball);
            }
        }

        public virtual void collide(Ball ball)
        {
            ball.setSpeed(Vector2.Zero);
        }

        public override float radius()
        {
            return obstacle_sprite.Width / 2;
        }
    }
}
