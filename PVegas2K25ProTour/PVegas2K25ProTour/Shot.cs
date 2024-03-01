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
                releaseShot();
            }
            else
            {
                windupShot(mouse_pos, ball.center());
            }
            resizeArrow(ball.position());
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
        public void releaseShot()
        {
            launch_speed = Vector2.Zero;
        }

        /// <summary>
        /// Creates a new rectangle representing the dimensions of the shot
        /// display arrow and sets the arrows dimensions to that rectangle
        /// </summary>
        /// <param name="ball_center">the ball's center used for positioning
        /// the arrow.</param>
        public void resizeArrow(Vector2 ball_center)
        {
            arrow_rect = new Rectangle((int)ball_center.X, 
                (int)ball_center.Y - (int)launch_speed.Length(),
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

        //---------------------------------------------------------------------
        // FOR TEST PURPOSES ONLY
        //---------------------------------------------------------------------

        /// <summary>
        /// Gets the length of the arrow representing the shot's speed and
        /// direction
        /// </summary>
        public float arrowLength()
        {
            return arrow_rect.Height;
        }
    }
}
