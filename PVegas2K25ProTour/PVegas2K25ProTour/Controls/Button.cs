using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVegas2K25ProTour.Controls
{
    public class Button : Component
    {

        private MouseState _mouseState;

        private SpriteFont _font;

        private bool _isHovering;

        private MouseState _previousMouse;

        private Texture2D _texture;

        private float local_scale;

        private Vector2 local_offset;


        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;

            if(_isHovering) 
            {
                color = Color.Red;            
            }

            spriteBatch.Draw(_texture, rectangle, color);

            if (!string.IsNullOrEmpty(Text)) 
            { 
                var x = (rectangle.X + (rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (rectangle.Y + (rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }

        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _mouseState;
            _mouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(_mouseState.X, _mouseState.Y, 1, 1);
            var scaledRectangle = new Rectangle((int)(rectangle.X * local_scale + local_offset.X),
                (int)(rectangle.Y * local_scale + local_offset.Y), (int)(rectangle.Width * local_scale),
                (int)(rectangle.Height * local_scale));
            
            _isHovering= false;

            if(mouseRectangle.Intersects(scaledRectangle)) 
            { 
                _isHovering= true;

                if(_mouseState.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed) 
                { 
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public void setLocalScale(float scale)
        {
            local_scale = scale;
        }

        public void setOffset(Vector2 offset)
        {
            local_offset = offset;
        }
    }
}
