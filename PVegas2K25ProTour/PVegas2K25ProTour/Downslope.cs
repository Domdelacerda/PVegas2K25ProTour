//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a rectangular slope that the player will roll down while
// colliding with it
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Downslope is a rectangular, static obstacle on the map that checks for 
    /// collision with a ball and gives the ball downward speed while
    /// colliding with it
    /// </summary>-------------------------------------------------------------
    public class Downslope : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D slope_sprite;
        private Vector2 slope_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        private const float INCLINE = 2.5f;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new downslope using a specified position, sprite 
        /// batch, hitbox, and scale
        /// </summary>
        /// <param name="slope_pos">the position of the downslope at the 
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the downslope's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// <param name="hitbox">the hitbox the downslope uses for detecting
        /// collisions with the ball.</param>
        /// <param name="scale">the scale of the downslope's sprite and hitbox 
        /// as a factor of the downslope's default scale.</param>
        /// -------------------------------------------------------------------
        public Downslope(Vector2 slope_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale) : base(slope_pos,
                _sprite_batch, hitbox, scale)
        {
            this.slope_pos = slope_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------
        public override void LoadContent(ContentManager _content)
        {
            slope_sprite = _content.Load<Texture2D>("Downslope");
        }

        public override void Update(Ball ball)
        {
            if (hitbox.collisionPointToRect(ball.center(), this))
            {
                ball.setRolling(true);
                collide(ball);
            }
            else
            {
                ball.setRolling(false);
            }
        }

        public override void Draw()
        {
            _sprite_batch.Draw(slope_sprite, new Rectangle((int)slope_pos.X,
                (int)slope_pos.Y, (int)(slope_sprite.Width * scale.X),
                (int)(slope_sprite.Height * scale.Y)), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Unique collision behavior for the mushroom obstacle: bounces the
        /// ball away from the mushroom with a different speed value than it
        /// initially had
        /// </summary>
        /// <param name="ball">the ball to slide down the slope.</param>
        /// -------------------------------------------------------------------
        public override void collide(Ball ball)
        {
            if (ball.getSpeed().Length() != 0)
            {
                ball.setSpeed(new Vector2(ball.getSpeed().X, 
                    ball.getSpeed().Y + INCLINE));
            }
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the width of the downslope from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the width of the downslope.</returns>
        /// -------------------------------------------------------------------
        public override float width()
        {
            return slope_sprite.Width * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the height of the downslope from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the width of the downslope.</returns>
        /// -------------------------------------------------------------------
        public override float height()
        {
            return slope_sprite.Height * scale.Y;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the center of the downslope using its position and 
        /// dimensions
        /// </summary>
        /// <returns>the center of the downslope.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = slope_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}