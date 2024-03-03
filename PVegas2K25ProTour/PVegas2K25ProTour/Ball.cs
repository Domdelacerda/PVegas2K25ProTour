using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PVegas2K25ProTour
{
    public class Ball : GameObject
    {
        private GraphicsDevice _device;
        private SpriteBatch _sprite_batch;

        private const int BALL_START_POINT_X = 400;
        private const int BALL_START_POINT_Y = 200;
        private const float DRAG_REDUCTION_FACTOR = 0.98f;
        private const float SHOT_POWER_MULTIPLIER = 1.25f;
        private const float MIN_BALL_SPEED = 10f;

        private Vector2 ball_pos;
        private Vector2 ball_speed;

        private Texture2D ball_sprite;

        public Ball(GraphicsDevice _device, SpriteBatch
            _sprite_batch) : base(_device, _sprite_batch)
        {
            ball_pos = new Vector2(BALL_START_POINT_X, BALL_START_POINT_Y);
            this._device = _device;
            this._sprite_batch = _sprite_batch;
        }

        public Ball(Vector2 ball_pos, GraphicsDevice _device, SpriteBatch 
            _sprite_batch) : base(ball_pos, _device, _sprite_batch)
        {
            this.ball_pos = ball_pos;
            this._device = _device;
            this._sprite_batch = _sprite_batch;
        }

        public void LoadContent(ContentManager _content)
        {
            ball_sprite = _content.Load<Texture2D>("GolfBall");
        }

        public void Draw()
        {
            _sprite_batch.Draw(ball_sprite, ball_pos, Color.White);
        }

        /// <summary>
        /// Determines whether or not a point in space overlaps the ball based
        /// on the position of its center and the size of its sprite
        /// </summary>
        /// <param name="point">the point to be checked.</param>
        /// <returns>whether or not the point overlaps the circle.</returns>
        public bool isPointOverBall(Vector2 point)
        {
            float pointToCenter = distance(point, center());
            return (pointToCenter <= radius());
        }

        /// <summary>
        /// Obtains the radius of the ball from the size of its sprite
        /// </summary>
        /// <returns>the radius of the ball</returns>
        public float radius()
        {
            return (ball_sprite.Width / 2);
        }

        /// <summary>
        /// Gets the position where the center of the ball is by using its
        /// drawn position and its radius
        /// </summary>
        /// <returns>the position of the ball's center.</returns>
        public Vector2 center()
        {
            Vector2 center = ball_pos;
            center.X += radius();
            center.Y += radius();
            return center;
        }

        /// <summary>
        /// Gets the position where the ball is drawn (ball_pos)
        /// </summary>
        /// <returns>the position of the ball.</returns>
        public Vector2 position()
        {
            return ball_pos;
        }

        // gives the ball friction and makes it slow down over time
        // DRAG_REDUCTION_FACTOR is the % amount that the ball will reduce in speed per update
        public void updateSpeed()
        {
            // Make sure that neither X or Y in speed Vector is already 0
            if(!(ball_speed.Length() == 0))
            {
                ball_speed *= DRAG_REDUCTION_FACTOR;
            }
            ballStop();
        }

        // Position updates based on the ball speed
        public void updatePosition(GameTime gametime)
        {
            // the new position is based on old position and speed. += relationship

            float time = (float)gametime.ElapsedGameTime.TotalSeconds;

            // Calculate the movement vector
            Vector2 movement = ball_speed * time;

            // Update the position
            ball_pos += movement;
        }

        public void launchBall(Shot myShot)
        {
            // Potential issue? If the shot has been released when is launch power set to zero?
            // There's a chance this could never evaluate to true. 
            if (myShot.launchPower() > 0)
            {
                // get the angle of the user's shot using Vector2 launch_speed: 
                
                //  launch power?
                /*float launchPower = myShot.launchPower();
                ball_speed.X = launchPower;
                ball_speed.Y = launchPower;*/

                ball_speed = myShot.getLaunchSpeed() * SHOT_POWER_MULTIPLIER;

                // update the ball position with power according to launch power
            }
        }

        /// <summary>
        /// If the ball's speed dips below a specified threshold, this function
        /// will automatically round it down to zero so that speed doesn't
        /// become an infintely small float (the ball never truly stops)
        /// </summary>
        public void ballStop()
        {
            if (ball_speed.Length() < MIN_BALL_SPEED)
            {
                ball_speed = Vector2.Zero;
            }
        }

        public Vector2 getBallSpeed()
        {
            return ball_speed;
        }

        public void setBallSpeed(Vector2 newSpeed)
        {
            ball_speed = newSpeed;
        }
    }
}
