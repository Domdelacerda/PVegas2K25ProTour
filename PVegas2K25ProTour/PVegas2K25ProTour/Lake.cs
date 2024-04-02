using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    public class Lake : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D lake_sprite;
        private Vector2 lake_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        private const float BOUNCINESS = 1.5f;

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

        public override void Draw()
        {
            _sprite_batch.Draw(lake_sprite, new Rectangle((int)lake_pos.X,
                (int)lake_pos.Y, (int)(lake_sprite.Width * scale.X),
                (int)(lake_sprite.Height * scale.Y)), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        public override void Update(Ball ball)
        {
            if (hitbox.collisionPointToRect(ball.center(), this))
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
            return lake_sprite.Width / 2 * scale.X;
        }

        public override float width()
        {
            return lake_sprite.Width * scale.X;
        }

        public override float height()
        {
            return lake_sprite.Height * scale.Y;
        }

        public override Vector2 center()
        {
            Vector2 center = lake_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}