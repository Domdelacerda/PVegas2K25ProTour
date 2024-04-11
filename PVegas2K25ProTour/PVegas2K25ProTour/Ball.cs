//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a ball object that has realistic physics
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Ball is able to be moved according to it's position value, which is
    /// where the ball will be drawn on screen. Position is augmented by the
    /// ball's speed each frame. The ball's speed decreases exponentially so
    /// that it will eventually come to a stop
    /// </summary>-------------------------------------------------------------
    public class Ball : GameObject
    {
        private SpriteBatch _sprite_batch;

        private const float DRAG_REDUCTION_FACTOR = 0.98f;
        private const float SHOT_POWER_MULTIPLIER = 3f;
        private const float MIN_BALL_SPEED = 10f;
        private const float MAX_BALL_SPEED = 1000f;

        private Vector2 ball_pos;
        private Vector2 ball_speed;
        private Vector2 previous_ball_pos;
        private bool rolling = false;

        private Texture2D ball_sprite;
        private Hitbox hitbox;
        private int strokeCount = 0;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new ball using a default starting point inherent to
        /// all ball objects
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch the ball's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public Ball(SpriteBatch _sprite_batch) : base(_sprite_batch)
        {
            this._sprite_batch = _sprite_batch;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public void LoadContent(ContentManager _content)
        {
            ball_sprite = _content.Load<Texture2D>("GolfBall");
        }

        public void Draw()
        {
            _sprite_batch.Draw(ball_sprite, ball_pos, Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Determines whether or not a point in space overlaps the ball based
        /// on the position of its center and the size of its sprite
        /// </summary>
        /// <param name="point">the point to be checked.</param>
        /// <returns>whether or not the point overlaps the ball.</returns>
        /// -------------------------------------------------------------------
        public bool isPointOverBall(Vector2 point)
        {
            float pointToCenter = distance(point, center());
            return pointToCenter <= radius();
        }

        /// <summary>----------------------------------------------------------
        /// Gets the position where the ball is drawn (ball_pos)
        /// </summary>
        /// <returns>the position of the ball.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 position()
        {
            return ball_pos;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the radius of the ball from the size of its sprite
        /// </summary>
        /// <returns>the radius of the ball.</returns>
        /// -------------------------------------------------------------------
        public override float radius()
        {
            return ball_sprite.Width / 2;
        }

        /// <summary>----------------------------------------------------------
        /// Gets the position where the center of the ball is by using its
        /// drawn position and its radius
        /// </summary>
        /// <returns>the position of the ball's center.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = ball_pos;
            center.X += radius();
            center.Y += radius();
            return center;
        }

        /// <summary>----------------------------------------------------------
        /// Composite date function to update speed and position every frame
        /// </summary>
        /// <param name="time">the time used for physics calculations.</param>
        /// -------------------------------------------------------------------
        public void Update(GameTime time)
        {
            updateSpeed();
            updatePosition(time);
        }

        /// <summary>----------------------------------------------------------
        /// Gives the ball friction and makes it slow down over time
        /// DRAG_REDUCTION_FACTOR is the % amount that the ball will reduce in 
        /// speed per update
        /// </summary>---------------------------------------------------------
        public void updateSpeed()
        {
            // Make sure that neither X or Y in speed Vector is already 0
            if (ball_speed.Length() != 0)
            {
                ball_speed *= DRAG_REDUCTION_FACTOR;
                truncateSpeedLower();
                truncateSpeedUpper();
            }
            else
            {
                setPreviousPosition(ball_pos);
            }
        }

        /// <summary>----------------------------------------------------------
        /// Position updates based on the ball speed
        /// </summary>
        /// <param name="game_time">the time used for physics calculations.
        /// </param>
        /// -------------------------------------------------------------------
        public void updatePosition(GameTime game_time)
        {
            // the new position is based on old position and speed. += relationship

            float time = (float)game_time.ElapsedGameTime.TotalSeconds;

            // Calculate the movement vector
            Vector2 movement = ball_speed * time;

            // Update the position
            ball_pos += movement;
        }

        /// <summary>----------------------------------------------------------
        /// Give the ball a specified speed vector as long as the magnitude of
        /// that vector is not zero
        /// </summary>
        /// <param name="launch_power">the magnitude of the speed the ball will
        /// be given.</param>
        /// <param name="launch_speed">the speed vector the ball is given.
        /// </param>
        /// -------------------------------------------------------------------
        public void launchBall(float launch_power, Vector2 launch_speed)
        {
            // Potential issue? If the shot has been released when is launch power set to zero?
            // There's a chance this could never evaluate to true. 
            if (launch_power > 0)
            {
                // get the angle of the user's shot using Vector2 launch_speed: 

                //  launch power?
                /*float launchPower = myShot.launchPower();
                ball_speed.X = launchPower;
                ball_speed.Y = launchPower;*/

                ball_speed = launch_speed * SHOT_POWER_MULTIPLIER;

                // update the ball position with power according to launch power
                strokeCount += 1;
            }
        }

        /// <summary>----------------------------------------------------------
        /// Stop the ball immediately by setting its speed vector to zero
        /// </summary>---------------------------------------------------------
        public void ballStop()
        {
            ball_speed = Vector2.Zero;
        }

        /// <summary>----------------------------------------------------------
        /// If the ball's speed dips below a specified threshold, this function
        /// will automatically round it down to zero so that speed doesn't
        /// become an infintely small float (A.K.A. the ball never truly stops)
        /// </summary>---------------------------------------------------------
        public void truncateSpeedLower()
        {
            if (belowMinSpeed() && rolling == false)
            {
                ballStop();
            }
        }

        /// <summary>----------------------------------------------------------
        /// If the ball's speed dips below a specified threshold, this function
        /// will automatically round it down to zero so that speed doesn't
        /// become an infintely small float (A.K.A. the ball never truly stops)
        /// </summary>---------------------------------------------------------
        public void truncateSpeedUpper()
        {
            if (aboveMaxSpeed())
            {
                Vector2 direction = ball_speed / ball_speed.Length();
                setSpeed(direction * MAX_BALL_SPEED);
            }
        }

        /// <summary>----------------------------------------------------------
        /// Determines if the magnitude of the ball's speed is below its set
        /// minimum value
        /// </summary>---------------------------------------------------------
        public bool belowMinSpeed()
        {
            return ball_speed.Length() < MIN_BALL_SPEED;
        }

        /// <summary>----------------------------------------------------------
        /// Determines if the magnitude of the ball's speed is above its set
        /// maximum value
        /// </summary>---------------------------------------------------------
        public bool aboveMaxSpeed()
        {
            return ball_speed.Length() > MAX_BALL_SPEED;
        }

        /// <summary>----------------------------------------------------------
        /// Gets the current speed of the ball
        /// <returns>the speed of the ball.</returns>
        /// </summary>---------------------------------------------------------
        public Vector2 getSpeed()
        {
            return ball_speed;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the current speed of the ball
        /// <param name="new_speed">the new speed the ball is set to.</param>
        /// </summary>---------------------------------------------------------
        public void setSpeed(Vector2 new_speed)
        {
            ball_speed = new_speed;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the current position of the ball in the scene
        /// <param name="new_position">the new position the ball is set to.
        /// </param>
        /// </summary>---------------------------------------------------------
        public void setPosition(Vector2 new_position)
        {
            ball_pos = new_position;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the save state for the ball's previous position, used when
        /// resetting the ball after hitting an out-of-bounds obstacle such as
        /// lake or puddle
        /// <param name="new_prev_position">the new previous position of the 
        /// ball.</param>
        /// </summary>---------------------------------------------------------
        public void setPreviousPosition(Vector2 new_prev_position)
        {
            previous_ball_pos = new_prev_position;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the ball's position to the value stored in the previous
        /// position save state, effectively undoing the ball's most recent
        /// movement
        /// </summary>---------------------------------------------------------
        public void resetPosition()
        {
            ball_pos = previous_ball_pos;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the rolling state for the ball, which is used when on a
        /// downslope so that the ball's speed isn't truncated when it
        /// approaches zero and still rolls down even if it has no speed
        /// </summary>---------------------------------------------------------
        public void setRolling(bool new_roll_state)
        {
            rolling = new_roll_state;
        }

        public void setStrokeCount(int count)
        {
            strokeCount = count;
        }

        internal int getStrokeCount()
        {

            return strokeCount;
        }
    }
}   