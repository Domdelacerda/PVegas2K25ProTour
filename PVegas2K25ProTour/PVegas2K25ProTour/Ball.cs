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
    }
}
