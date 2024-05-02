//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a circular water hazard that the player has to avoid, and
// if they don't, it will reset them to their previous position
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Puddle is a circular, static obstacle on the map that checks for 
    /// collision with a ball and resets the ball to its previous resting
    /// position if collision occurs
    /// </summary>-------------------------------------------------------------
    public class Puddle : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D puddle_sprite;
        private Vector2 puddle_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new lake using a specified position, sprite 
        /// batch, hitbox, and scale
        /// </summary>
        /// <param name="puddle_pos">the position of the puddle at the start of
        /// the game.</param>
        /// <param name="_sprite_batch">the sprite batch the puddle's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// <param name="hitbox">the hitbox the puddle uses for detecting
        /// collisions with the ball.</param>
        /// <param name="scale">the scale of the puddle's sprite and hitbox as 
        /// a factor of the puddle's default scale.</param>
        /// -------------------------------------------------------------------
        public Puddle(Vector2 puddle_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale) : base(puddle_pos, 
                _sprite_batch, hitbox, scale)
        {
            this.puddle_pos = puddle_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------
        public override void LoadContent(ContentManager _content)
        {
            puddle_sprite = _content.Load<Texture2D>("Puddle");
        }

        public override void Update(Ball ball)
        {
            if (hitbox.collisionPointToCircle(ball.center(), this))
            {
                collide(ball);
            }
        }

        public override void Draw()
        {
            _sprite_batch.Draw(puddle_sprite, new Rectangle((int)puddle_pos.X,
                (int)puddle_pos.Y, (int)(puddle_sprite.Width * scale.X),
                (int)(puddle_sprite.Height * scale.Y)), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Unique collision behavior for the mushroom obstacle: bounces the
        /// ball away from the mushroom with a different speed value than it
        /// initially had
        /// </summary>
        /// <param name="ball">the ball to be bounced back</param>
        /// -------------------------------------------------------------------
        public override void collide(Ball ball)
        {
            ball.resetPosition();
            ball.ballStop();
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the radius of the puddle from the size of its sprite and
        /// its scale factor
        /// </summary>
        /// <returns>the radius of the puddle.</returns>
        /// -------------------------------------------------------------------
        public override float radius()
        {
            return puddle_sprite.Width / 2 * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the width of the puddle from the size of its sprite and its
        /// scale factor
        /// </summary>
        /// <returns>the width of the puddle.</returns>
        /// -------------------------------------------------------------------
        public override float width()
        {
            return puddle_sprite.Width * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the height of the puddle from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the width of the puddle.</returns>
        /// -------------------------------------------------------------------
        public override float height()
        {
            return puddle_sprite.Height * scale.Y;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the center of the puddle using its position and dimensions
        /// </summary>
        /// <returns>the center of the puddle.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = puddle_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}