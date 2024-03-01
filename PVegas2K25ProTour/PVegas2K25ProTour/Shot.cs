using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PVegas2K25ProTour
{
    public class Shot : GameObject
    {
        private GraphicsDevice _device;
        private SpriteBatch _sprite_batch;

        private Vector2 launch_speed;
        private Texture2D arrow_sprite;
        private Rectangle arrow_rect;

        public Shot(GraphicsDevice _device, SpriteBatch _sprite_batch) : 
            base(_device, _sprite_batch)
        {
            this._device = _device;
            this._sprite_batch = _sprite_batch;
        }

        public void LoadContent(ContentManager _content)
        {
            arrow_sprite = _content.Load<Texture2D>("Shot");
        }

        public void Draw()
        {
            _sprite_batch.Draw(arrow_sprite, arrow_rect, Color.Red);
        }

        public void Update(bool drag_state, Vector2 mouse_pos, Ball ball)
        {
            if (drag_state == false)
            {
                releaseShot(ball);
            }
            else
            {
                windupShot(mouse_pos, ball.center());
            }
            arrow_rect = resizeArrow(ball.position());
        }

        /// <summary>
        /// Sets the current shot power to the distance vector between the
        /// position of the mouse and the position of the center of the ball
        /// </summary>
        public void windupShot(Vector2 mouse_pos, Vector2 ball_center)
        {
            launch_speed = distanceVector(mouse_pos, ball_center);
        }

        /// <summary>
        /// Sets the current shot power to 0
        /// </summary>
        public void releaseShot(Ball myBall)
        {
            myBall.setBallSpeed(launch_speed);
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

        /// <summary>
        /// Creates a new rectangle representing the dimensions of the shot
        /// display arrow
        /// </summary>
        /// <param name="ball">the ball used for positioning the arrow.</param>
        /// <returns>the new size and position of the display arrow.</returns>
        public Rectangle resizeArrow(Vector2 ball_pos)
        {
            return new Rectangle((int)ball_pos.X, 
                (int)ball_pos.Y - (int)launch_speed.Length(),
                arrow_sprite.Width, (int)launch_speed.Length());
        }


        /// <summary>
        /// Gets the power of the launch by determining the launch speed
        /// vector's magnitude
        /// </summary>
        public float launchPower()
        {
            return launch_speed.Length();
        }

        public Vector2 getLaunchSpeed()
        {
            return launch_speed;
        }
    }
}
