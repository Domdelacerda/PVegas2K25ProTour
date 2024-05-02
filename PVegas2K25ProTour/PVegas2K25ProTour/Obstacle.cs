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
        /// <param name="scale">the scale of the obstacle's sprite and hitbox.
        /// Unique to generic obstacle, scale represents actual size instead of
        /// a multiple of the sprite's original size.</param>
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
            _sprite_batch.Draw(obstacle_sprite, new 
                Rectangle((int)obstacle_pos.X, 
                (int)obstacle_pos.Y, (int)scale.X, (int)scale.Y), Color.Black);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Checks for collisions with a ball game object every frame
        /// </summary>
        /// <param name="ball">the ball that can collide with this obstacle.
        /// </param>
        /// -------------------------------------------------------------------
        public virtual void Update(Ball ball)
        {
            if (hitbox.collisionCircleToRect(ball, this))
            {
                collide(ball);
            }
        }

        /// <summary>----------------------------------------------------------
        /// On collision with a ball, enact unique collision behavior. The
        /// generic obstacle's collision behavior bounces the ball off of
        /// itself at a reflected angle
        /// </summary>
        /// <param name="ball">the ball that will be bounced off this obstacle.
        /// </param>
        /// -------------------------------------------------------------------
        public virtual void collide(Ball ball)
        {
            if (ball.getSpeed().Length() < MIN_BOUNCE_SPEED)
            {
                ball.ballStop();
            }
            else
            {
                ball.setSpeed(reflectVector(ball.getSpeed(),
                hitbox.rectCollisionNormal(ball, this)));
            }
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the width of the obstacle from the size of its sprite
        /// </summary>
        /// <returns>the width of the obstacle.</returns>
        /// -------------------------------------------------------------------
        public override float width()
        {
            return scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the height of the obstacle from the size of its sprite
        /// </summary>
        /// <returns>the width of the obstacle.</returns>
        /// -------------------------------------------------------------------
        public override float height()
        {
            return scale.Y;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the center of the obstacle using its position and 
        /// dimensions
        /// </summary>
        /// <returns>the center of the object.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = obstacle_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the reflection of a vector over a provided normal line
        /// </summary>
        /// <param name="direction">the vector of incidence.</param>
        /// <param name="normal">the normal line that the vector of incidence 
        /// is reflected across.</param>
        /// <returns>the vector of reflection.</returns>
        /// -------------------------------------------------------------------
        public virtual Vector2 reflectVector(Vector2 direction, Vector2 normal)
        {
            normal.Normalize();
            return Vector2.Reflect(direction, normal);
        }
    }
}