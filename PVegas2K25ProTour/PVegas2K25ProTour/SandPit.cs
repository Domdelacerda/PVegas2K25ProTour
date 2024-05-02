//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a circular sand hazard that the player has to avoid, and
// if they don't, it will drastically slow down the ball's movement
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Sand Pit is a circular, static obstacle on the map that checks for 
    /// collision with a ball and reduces the ball's speed by an additional
    /// factor each frame collision is detected
    /// </summary>-------------------------------------------------------------
    public class SandPit : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D sand_pit_sprite;
        private Vector2 sand_pit_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        private const float SAND_FRICTION = 0.955f;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new sand pit using a specified position, sprite 
        /// batch, hitbox, and scale
        /// </summary>
        /// <param name="sand_pit_pos">the position of the sand pit at the 
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the sand pit's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// <param name="hitbox">the hitbox the sand pit uses for detecting
        /// collisions with the ball.</param>
        /// <param name="scale">the scale of the sand pit's sprite and hitbox 
        /// as a factor of the sand pit's default scale.</param>
        /// -------------------------------------------------------------------
        public SandPit(Vector2 sand_pit_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale) : base(sand_pit_pos, 
                _sprite_batch, hitbox, scale)
        {
            this.sand_pit_pos = sand_pit_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------
        public override void LoadContent(ContentManager _content)
        {
            sand_pit_sprite = _content.Load<Texture2D>("Sand");
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
            _sprite_batch.Draw(sand_pit_sprite, new 
                Rectangle((int)sand_pit_pos.X, (int)sand_pit_pos.Y, 
                (int)(sand_pit_sprite.Width * scale.X),
                (int)(sand_pit_sprite.Height * scale.Y)), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Unique collision behavior for the sand pit obstacle: slows down the
        /// ball faster while on sand
        /// </summary>
        /// <param name="ball">the ball to be slowed down</param>
        /// -------------------------------------------------------------------
        public override void collide(Ball ball)
        {
            ball.setSpeed(ball.getSpeed() * SAND_FRICTION);
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the radius of the sand pit from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the radius of the sand pit.</returns>
        /// -------------------------------------------------------------------
        public override float radius()
        {
            return sand_pit_sprite.Width / 2 * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the width of the sand pit from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the width of the sand pit.</returns>
        /// -------------------------------------------------------------------
        public override float width()
        {
            return sand_pit_sprite.Width * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the height of the sand pit from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the width of the sand pit.</returns>
        /// -------------------------------------------------------------------
        public override float height()
        {
            return sand_pit_sprite.Height * scale.Y;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the center of the sand pit using its position and 
        /// dimensions
        /// </summary>
        /// <returns>the center of the sand pit.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = sand_pit_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}
