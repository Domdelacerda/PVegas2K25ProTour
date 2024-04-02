using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    public class Puddle : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D puddle_sprite;
        private Vector2 puddle_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        private const float BOUNCINESS = 1.5f;

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

        public override void Draw()
        {
            _sprite_batch.Draw(puddle_sprite, new Rectangle((int)puddle_pos.X,
                (int)puddle_pos.Y, (int)(puddle_sprite.Width * scale.X),
                (int)(puddle_sprite.Height * scale.Y)), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        public override void Update(Ball ball)
        {
            if (hitbox.collisionPointToCircle(ball.center(), this))
            {
                collide(ball);
            }
        }

        /// <summary>
        /// Unique collision behavior for the mushroom obstacle: bounces the
        /// ball away from the mushroom with a different speed value than it
        /// initially had
        /// </summary>
        /// <param name="ball">the ball to be bounced back</param>
        public override void collide(Ball ball)
        {
            ball.resetPosition();
            ball.ballStop();
        }

        public override float radius()
        {
            return puddle_sprite.Width / 2 * scale.X;
        }

        public override float width()
        {
            return puddle_sprite.Width * scale.X;
        }

        public override float height()
        {
            return puddle_sprite.Height * scale.Y;
        }

        public override Vector2 center()
        {
            Vector2 center = puddle_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}