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
        private Vector2 scale;
        private const float MIN_BOUNCE_SPEED = 30f;

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
            Hitbox hitbox, Vector2 scale) : base(obstacle_pos, _sprite_batch, 
                hitbox, scale)
        {
            this.obstacle_pos = obstacle_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public virtual void LoadContent(ContentManager _content)
        {
            obstacle_sprite = _content.Load<Texture2D>("Wall");
        }

        public virtual void Draw()
        {
            _sprite_batch.Draw(obstacle_sprite, new Rectangle((int)obstacle_pos.X, 
                (int)obstacle_pos.Y, (int)scale.X, (int)scale.Y), Color.Black);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>
        /// Checks for collisions with a ball game object every frame
        /// </summary>
        /// <param name="ball">the ball that can collide with this obstacle.
        /// </param>
        public virtual void Update(Ball ball)
        {
            if (hitbox.collisionCircleToRect(ball, this))
            {
                collide(ball);
            }
        }

        public virtual void collide(Ball ball)
        {
            if (ball.getSpeed().Length() < MIN_BOUNCE_SPEED)
            {
                ball.ballStop();
            }
            else
            {
                ball.setSpeed(reflect(ball.getSpeed(),
                hitbox.rectCollisionNormal(ball, this)));
            }
        }

        public override float radius()
        {
            return scale.X / 2;
        }

        public override float width()
        {
            return scale.X;
        }

        public override float height()
        {
            return scale.Y;
        }

        public override Vector2 center()
        {
            Vector2 center = obstacle_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }

        public float dotProduct(Vector2 vector1, Vector2 vector2)
        {
            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y);
        }

        public virtual Vector2 reflect(Vector2 direction, Vector2 normal)
        {
            normal.Normalize();
            return direction - 2 * dotProduct(direction, normal) * normal;
        }

        public void setScale(Vector2 new_scale)
        {
            scale = new_scale;
        }
    }
}
