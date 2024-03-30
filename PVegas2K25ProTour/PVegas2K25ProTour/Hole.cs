//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a hole that the ball can enter in order to complete the level
// and move on to the next
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Hole is a static object on the map that checks for collision with a
    /// ball to determine if the level is complete or not
    /// </summary>-------------------------------------------------------------
    public class Hole : GameObject
    {
        private SpriteBatch _sprite_batch;
        private Texture2D hole_sprite;
        private Hitbox hitbox;
        private Vector2 hole_pos;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new hole using a specified position and sprite batch
        /// </summary>
        /// <param name="hole_pos">the position of the hole at the start of the
        /// game.</param>
        /// <param name="_sprite_batch">the sprite batch the hole's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public Hole(Vector2 hole_pos, SpriteBatch _sprite_batch, 
            Hitbox hitbox) : base(hole_pos, _sprite_batch, hitbox)
        {
            this.hole_pos = hole_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public void LoadContent(ContentManager _content)
        {
            hole_sprite = _content.Load<Texture2D>("Hole");
        }

        public void Draw()
        {
            _sprite_batch.Draw(hole_sprite, hole_pos, Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>
        /// Checks for collisions with a ball game object every frame
        /// </summary>
        /// <param name="ball">the ball that can collide with this obstacle.
        /// </param>
        public void Update(Ball ball)
        {
            if (isPointOverHole(ball.center()) == true)
            {
                collide(ball);
            }
        }

        /// <summary>----------------------------------------------------------
        /// Determines whether or not a point in space overlaps the hole based
        /// on the position of its center and the size of its sprite
        /// </summary>
        /// <param name="point">the point to be checked.</param>
        /// <returns>whether or not the point overlaps the hole.</returns>
        /// -------------------------------------------------------------------
        public bool isPointOverHole(Vector2 point)
        {
            float pointToCenter = distance(point, center());
            return pointToCenter <= radius();
        }

        public virtual void collide(Ball ball)
        {
            ball.setSpeed(new Vector2(100000f, 0f));
        }

        public override Vector2 position()
        {
            return hole_pos;
        }

        public override float radius()
        {
            return hole_sprite.Width / 2;
        }

        public override Vector2 center()
        {
            Vector2 center = hole_pos;
            center.X += radius();
            center.Y += radius();
            return center;
        }
    }
}
