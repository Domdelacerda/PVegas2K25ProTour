using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PVegas2K25ProTour
{
    public class Renderer
    {
        private GraphicsDevice _device;
        private RenderTarget2D _render_target;
        private Vector2 screen_size;
        private Vector2 game_view_offset;
        private Rectangle render_target_rect;
        int width;
        int height;
        private float scale;

        public Renderer(GraphicsDevice _device, int width, int height)
        {
            this._device = _device;
            this.width = width;
            this.height = height;
            _render_target = new RenderTarget2D(_device, width, height);
        }

        public float getScale()
        {
            return scale;
        }

        public void setScale()
        {
            screen_size = new 
                Vector2(_device.PresentationParameters.Bounds.Width, 
                _device.PresentationParameters.Bounds.Height);
            Vector2 scale_vector = new Vector2(screen_size.X / 
                _render_target.Width, screen_size.Y / 
                _render_target.Height);
            scale = MathF.Min(scale_vector.X, scale_vector.Y);
        }

        public Vector2 getOffset()
        {
            return game_view_offset;
        }

        public void setOffset()
        {
            game_view_offset = new Vector2(
                screen_size.X - _render_target.Width * scale,
                screen_size.Y - _render_target.Height * scale) * 0.5f;
        }

        public void setDestination()
        {
            setScale();
            setOffset();
            Vector2 bounds = new Vector2(_render_target.Width, 
                _render_target.Height) * scale;
            render_target_rect = new Rectangle((int)game_view_offset.X, 
                (int)game_view_offset.Y, (int)bounds.X, (int)bounds.Y);
        }

        public void setActive(Color background)
        {
            _device.SetRenderTarget(_render_target);
            _device.Clear(background);
        }

        public void setInactive(Color background)
        {
            _device.SetRenderTarget(null);
            _device.Clear(background);
        }

        public void Draw(SpriteBatch _sprite_batch, Color background)
        {
            setInactive(background);
            _sprite_batch.Begin();
            _sprite_batch.Draw(_render_target, render_target_rect, 
                Color.White);
            _sprite_batch.End();
        }
    }
}
