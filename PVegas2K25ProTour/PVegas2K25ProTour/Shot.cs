//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have an interface to interact with a ball by changing its speed and
// provide visual feedback of potential shot power
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Shot takes input from the game to determine the mouse's drag state and
    /// position, creates an arrow to visualize its current shot power, and 
    /// provides output to a ball by altering its speed.
    /// </summary>-------------------------------------------------------------
    public class Shot : GameObject
    {
        private SpriteBatch _sprite_batch;
        private bool drawSprite;

        private Vector2 launch_speed;
        private Vector2 draw_point;
        private Texture2D arrow_sprite;
        private Rectangle arrow_rect;
        private int stroke_count = -1;

        private bool shot_released_bool = false;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new shot with no sprite specified, since it is always
        /// an arrow, and no position specified, since it will always depend on
        /// a ball's position
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch the shot's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control)</param>
        /// -------------------------------------------------------------------
        public Shot(SpriteBatch _sprite_batch) : base(_sprite_batch)
        {
            this._sprite_batch = _sprite_batch;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public void LoadContent(ContentManager _content)
        {
            arrow_sprite = _content.Load<Texture2D>("Shot");
        }

        public void Draw(Ball golf_ball)
        {
            if (drawSprite)
            {
                draw_point = new Vector2(0, arrow_rect.Height / 2);
                _sprite_batch.Draw(arrow_sprite, arrow_rect, null, Color.Red,
                    vectorAngle(launch_speed), draw_point, SpriteEffects.None, 0f);
            }
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Determines if the mouse is still being dragged or not; if it is, 
        /// continue changing the shot power based on the mouse's position. If
        /// it is not, release the shot. After the state is checked and the
        /// launch power is updated, change the size of the arrow sprite to
        /// reflect the new shot power.
        /// </summary>
        /// <param name="drag_state">the current dragging state of the 
        /// mouse.</param>
        /// <param name="mouse_pos">the mouse's current position.</param>
        /// <param name="ball">the ball to be potentially launched.</param>
        /// -------------------------------------------------------------------
        public void Update(bool drag_state, Vector2 mouse_pos, Ball ball)
        {
            MouseState mouse_state = Mouse.GetState();

            if (mouse_state.RightButton == ButtonState.Pressed && drag_state)
            {
                drawSprite = false;
                cancelShot();
                return;
            }

            if (drag_state == false)
            {
                if (!shot_released_bool)
                {
                    releaseShot(ball);
                    stroke_count++;
                    shot_released_bool = true;
                }
            }

            else
            {
                drawSprite = true;
                windupShot(mouse_pos, ball.center());
                shot_released_bool = false;
            }
            resizeArrow(ball.center());
        }

        /// <summary>----------------------------------------------------------
        /// Sets the current shot power to the distance vector between the
        /// position of the mouse and the position of the center of the ball
        /// </summary>---------------------------------------------------------
        public void windupShot(Vector2 mouse_pos, Vector2 ball_center)
        {
            launch_speed = distanceVector(mouse_pos, ball_center);
        }

        /// <summary>----------------------------------------------------------
        /// Launches a specified ball according to this shot's power, then sets 
        /// the shot power to 0
        /// </summary>---------------------------------------------------------
        public void releaseShot(Ball myBall)
        {
            myBall.launchBall(this);
            launch_speed = Vector2.Zero;
        }


        // Checks to see if the shot has been released
        public bool shotReleased()
        {
            bool shotReleased = false;

            if (launch_speed == Vector2.Zero)
                shotReleased = true;

            return shotReleased;
        }

        // getter method for the stroke_count var
        public int getStrokeCount()
        {
            return stroke_count;
        }

        /// <summary>----------------------------------------------------------
        /// Creates a new rectangle representing the dimensions of the shot
        /// display arrow and sets the arrows dimensions to that rectangle
        /// </summary>
        /// <param name="ball_center">the ball's center used for positioning
        /// the arrow.</param>
        /// -------------------------------------------------------------------
        public void resizeArrow(Vector2 ball_center)
        {
            arrow_rect = new Rectangle((int)ball_center.X, 
                (int)ball_center.Y, (int)launch_speed.Length(), 
                arrow_sprite.Height);
        }


        /// <summary>----------------------------------------------------------
        /// Gets the power of the launch by determining the launch speed
        /// vector's magnitude
        /// </summary>
        /// <returns>the current launch power of the shot.</returns>
        /// -------------------------------------------------------------------
        public float launchPower()
        {
            return launch_speed.Length();
        }

        public Vector2 getLaunchSpeed()
        {
            return launch_speed;
        }

        /// <summary>
        /// Resets the power of ball back to zero effectivly canceling the 
        /// ball from being released
        /// </summary>
        public void cancelShot()
        {
            launch_speed = Vector2.Zero;
        }

        //---------------------------------------------------------------------
        // FOR TEST PURPOSES ONLY
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Gets the length of the arrow sprite representing the shot's speed 
        /// and direction
        /// </summary>---------------------------------------------------------
        public float arrowLength()
        {
            return arrow_rect.Width;
        }
    }
}
