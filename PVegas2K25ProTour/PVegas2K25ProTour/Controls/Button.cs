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
        public Color _isHoveringColour {get; set;}
        public Color color { get; set; }
        private MouseState _previousMouse;

        private Texture2D _texture;
        private Texture2D _texture2;


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
            _isHoveringColour = Color.Red;
            color = Color.White;
        }
        public Button(Texture2D texture, Texture2D texture2)
        {
            _texture = texture;
            _texture2= texture2;
            PenColour = Color.Black;
            _isHoveringColour = Color.Red;
            color= Color.White;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tempColor = color;
            
            if(_isHovering) 
            {

                color = _isHoveringColour;           
            }

            spriteBatch.Draw(_texture, rectangle, color);

            if (!string.IsNullOrEmpty(Text)) 
            { 
                var x = (rectangle.X + (rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (rectangle.Y + (rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
            else
            {
                spriteBatch.Draw(_texture2, rectangle, color);
            }
            color = tempColor;
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _mouseState;
            _mouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(_mouseState.X, _mouseState.Y, 1, 1);
            
            _isHovering= false;

            if(mouseRectangle.Intersects(rectangle)) 
            { 
                _isHovering= true;

                if(_mouseState.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed) 
                { 
                    Click?.Invoke(this, new EventArgs());


                }
            }
            

        }
    }
}
