//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a circular, bouncy hazard that the player can avoid or collide
// with to bounce them in the opposite direction
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Mushroom is a circular, static obstacle on the map that checks for 
    /// collision with a ball, and if collision is detected, the ball is
    /// bounced in the opposite direction with additional speed
    /// </summary>-------------------------------------------------------------
    public class Mushroom : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D mushroom_sprite;
        private Vector2 mushroom_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        private const float BOUNCINESS = 1.5f;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new mushroom using a specified position, sprite 
        /// batch, hitbox, and scale
        /// </summary>
        /// <param name="mushroom_pos">the position of the mushroom at the 
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the mushroom's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// <param name="hitbox">the hitbox the mushroom uses for detecting
        /// collisions with the ball.</param>
        /// <param name="scale">the scale of the mushroom's sprite and hitbox 
        /// as a factor of the mushroom's default scale.</param>
        /// -------------------------------------------------------------------
        public Mushroom(Vector2 mushroom_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale) : base(mushroom_pos, 
                _sprite_batch, hitbox, scale)
        {
            this.mushroom_pos = mushroom_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------
        public override void LoadContent(ContentManager _content)
        {
            mushroom_sprite = _content.Load<Texture2D>("Mushroom");
        }

        public override void Update(Ball ball)
        {
            if (hitbox.collisionCircleToCircle(ball, this))
            {
                collide(ball);
            }
        }

        public override void Draw()
        {
            _sprite_batch.Draw(mushroom_sprite, new
                Rectangle((int)mushroom_pos.X, (int)mushroom_pos.Y,
                (int)(mushroom_sprite.Width * scale.X),
                (int)(mushroom_sprite.Height * scale.Y)), Color.White);
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
            Vector2 distance_vector = distanceVector(ball.center(),
                    center());
            distance_vector.Normalize();
            ball.setSpeed(reflectVector(ball.getSpeed(), distance_vector) * 
                BOUNCINESS);
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the radius of the mushroom from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the radius of the mushroom.</returns>
        /// -------------------------------------------------------------------
        public override float radius()
        {
            return mushroom_sprite.Width / 2 * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the width of the mushroom from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the width of the mushroom.</returns>
        /// -------------------------------------------------------------------
        public override float width()
        {
            return mushroom_sprite.Width * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the height of the mushroom from the size of its sprite and 
        /// its scale factor
        /// </summary>
        /// <returns>the height of the mushroom.</returns>
        /// -------------------------------------------------------------------
        public override float height()
        {
            return mushroom_sprite.Height * scale.Y;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the center of the mushroom using its position and 
        /// dimensions
        /// </summary>
        /// <returns>the center of the mushroom.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = mushroom_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}
