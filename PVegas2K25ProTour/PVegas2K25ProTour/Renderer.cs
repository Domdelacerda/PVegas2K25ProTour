//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Render our game view properly with any screen size and at the
// proper resolution
//-----------------------------------------------------------------------------


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Renderer creates a render target for the game to be rendered on based
    /// on a specified resolution, and continually maintains that resolution
    /// when the game window is resized
    /// </summary>-------------------------------------------------------------
    public class Renderer
    {
        private GraphicsDevice _device;
        private RenderTarget2D _render_target;
        private Vector2 screen_size;
        private Vector2 game_view_offset;
        private Rectangle render_target_rect;
        private float scale;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        public Renderer(GraphicsDevice _device, int width, int height)
        {
            this._device = _device;
            _render_target = new RenderTarget2D(_device, width, height);
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public void Draw(SpriteBatch _sprite_batch, Color background)
        {
            setInactive(background);
            _sprite_batch.Begin();
            _sprite_batch.Draw(_render_target, render_target_rect,
                Color.White);
            _sprite_batch.End();
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Activates the render target so that it can be drawn on
        /// </summary>---------------------------------------------------------
        public void setActive(Color background)
        {
            _device.SetRenderTarget(_render_target);
            _device.Clear(background);
        }

        /// <summary>----------------------------------------------------------
        /// Deactivates the render target so that the render target itself
        /// can be drawn
        /// </summary>---------------------------------------------------------
        public void setInactive(Color background)
        {
            _device.SetRenderTarget(null);
            _device.Clear(background);
        }

        /// <summary>----------------------------------------------------------
        /// Gets the current scale of the screen with respect to the default
        /// screen size
        /// </summary>---------------------------------------------------------
        public float getScale()
        {
            return scale;
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the current scale of the screen based on the current
        /// size of the screen and the default size. The minimum value of the
        /// scale vector is taken so that the game view never extends past
        /// the bounds of the screen
        /// </summary>---------------------------------------------------------
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

        /// <summary>----------------------------------------------------------
        /// Gets the current offset of the screen, meaning the amount of space
        /// not taken up by the game view (the black bars on the sides)
        /// </summary>---------------------------------------------------------
        public Vector2 getOffset()
        {
            return game_view_offset;
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the offset of the screen by comparing the actual scale
        /// the screen should be at against the minimum set scale
        /// </summary>---------------------------------------------------------
        public void setOffset()
        {
            game_view_offset = new Vector2(
                screen_size.X - _render_target.Width * scale,
                screen_size.Y - _render_target.Height * scale) * 0.5f;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the destination of the render target based on the calculated
        /// scale and offset
        /// </summary>---------------------------------------------------------
        public void setDestination()
        {
            setScale();
            setOffset();
            Vector2 bounds = new Vector2(_render_target.Width, 
                _render_target.Height) * scale;
            render_target_rect = new Rectangle((int)game_view_offset.X, 
                (int)game_view_offset.Y, (int)bounds.X, (int)bounds.Y);
        }
    }
}
