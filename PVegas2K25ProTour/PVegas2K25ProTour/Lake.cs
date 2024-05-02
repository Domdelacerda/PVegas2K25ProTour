//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a rectangular water hazard that the player has to avoid, and
// if they don't, it will reset them to their previous position
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Lake is a rectanglular, static obstacle on the map that checks for 
    /// collision with a ball and resets the ball to its previous resting
    /// position if collision occurs
    /// </summary>-------------------------------------------------------------
    public class Lake : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D lake_sprite;
        private Vector2 lake_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new lake using a specified position, sprite 
        /// batch, hitbox, and scale
        /// </summary>
        /// <param name="lake_pos">the position of the lake at the start of the
        /// game.</param>
        /// <param name="_sprite_batch">the sprite batch the lake's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// <param name="hitbox">the hitbox the lake uses for detecting
        /// collisions with the ball.</param>
        /// <param name="scale">the scale of the lake's sprite and hitbox as a
        /// factor of the lake's default scale.</param>
        /// -------------------------------------------------------------------
        public Lake(Vector2 lake_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale) : base(lake_pos,
                _sprite_batch, hitbox, scale)
        {
            this.lake_pos = lake_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------
        public override void LoadContent(ContentManager _content)
        {
            lake_sprite = _content.Load<Texture2D>("Lake");
        }

        public override void Update(Ball ball)
        {
            if (hitbox.collisionPointToRect(ball.center(), this))
            {
                collide(ball);
            }
        }

        public override void Draw()
        {
            _sprite_batch.Draw(lake_sprite, new Rectangle((int)lake_pos.X,
                (int)lake_pos.Y, (int)(lake_sprite.Width * scale.X),
                (int)(lake_sprite.Height * scale.Y)), Color.White);
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
        /// Obtains the width of the lake from the size of its sprite and its
        /// scale factor
        /// </summary>
        /// <returns>the width of the lake.</returns>
        /// -------------------------------------------------------------------
        public override float width()
        {
            return lake_sprite.Width * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the height of the lake from the size of its sprite and its
        /// scale factor
        /// </summary>
        /// <returns>the height of the lake.</returns>
        /// -------------------------------------------------------------------
        public override float height()
        {
            return lake_sprite.Height * scale.Y;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the center of the lake using its position and dimensions
        /// </summary>
        /// <returns>the center of the lake.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = lake_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}