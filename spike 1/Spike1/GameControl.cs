//-----------------------------------------------------------------------------
// Name: Dominic De La Cerda
// Project: Spike 1 - Mouse controls and shot power
// Purpose: Demonstrate functionality for augmenting shot power based on
// distance from ball to mouse
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Threading;

namespace Spike1
{
    public class GameControl : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _sprite_batch;
        private SpriteFont _font;
        private Texture2D golf_ball;

        private const int BALL_START_POINT_X = 400;
        private const int BALL_START_POINT_Y = 200;

        private Vector2 mouse_pos;
        private Vector2 ball_pos = new Vector2(BALL_START_POINT_X, 
            BALL_START_POINT_Y);

        private float shot_power = 0;
        private bool dragging = false;

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public GameControl()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _sprite_batch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            golf_ball = Content.Load<Texture2D>("GolfBall");
            _font = Content.Load<SpriteFont>("PowerFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            MouseState mouseState = Mouse.GetState();
            moveMouseTo(mouseState.X, mouseState.Y);
            updateDragState(isDraggingBall(mouseState));
            updateShotState(dragging);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            // TODO: Add your drawing code here
            _sprite_batch.Begin();
            _sprite_batch.Draw(golf_ball, ball_pos, Color.White);
            _sprite_batch.DrawString(_font, "Shot Power: " + 
                shot_power.ToString(), Vector2.Zero, Color.Black);
            _sprite_batch.End();

            base.Draw(gameTime);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN FUNCTIONS
        //---------------------------------------------------------------------

        /// <summary>
        /// Determines whether or not a point in space overlaps a circle in the
        /// scene, designated by a position and a size pulled from a sprite
        /// </summary>
        /// <param name="point">the point to be checked.</param>
        /// <param name="circleSprite">the sprite texture containing data 
        /// about the circle's radius.</param>
        /// <param name="circleCenter">the point where the circle's center is 
        /// located.</param>
        /// <returns>whether or not the point overlaps the circle.</returns>
        public bool isPointOverCircle(Vector2 point, Texture2D circleSprite, 
            Vector2 circleCenter)
        {
            float pointToSpriteDistance = distance(point, circleCenter);
            Vector2 edgePos = radiusPos(circleSprite, circleCenter);
            float spriteToEdgeDistance = distance(circleCenter, edgePos);
            return (pointToSpriteDistance <= spriteToEdgeDistance);
        }

        /// <summary>
        /// Find the position vector of a point on the circumference of a 
        /// circle centered at the origin
        /// </summary>
        /// <param name="circleSprite">the circle sprite that contains data
        /// about the circle's radius.</param>
        /// <param name="spriteCenter">the point where the sprite's 
        /// center is located.</param>
        /// <returns>the position vector starting at the circle's center and
        /// ending at a point on the circle's circumference.</returns>
        public Vector2 radiusPos(Texture2D circleSprite, Vector2 spriteCenter)
        {
            return new Vector2(spriteCenter.X +
                (circleSprite.Bounds.Width / 2), spriteCenter.Y);
        }

        /// <summary>
        /// Determines if the mouse is being dragged from the ball or
        /// not
        /// </summary>
        /// <param name="mouse"> the mouse state that input data is pulled
        /// from.</param>
        /// <returns> if the mouse is dragging the ball.</returns>
        public bool isDraggingBall(MouseState mouse)
        {
            bool dragState = false;
            if (mouse.LeftButton == ButtonState.Released)
            {
                dragState = false;
            }
            // If the player's left mouse button is down AND the mouse is over
            // the sprite OR if the player was already dragging the cursor from
            // the ball previously
            else if ((mouse.LeftButton == ButtonState.Pressed &&
                isPointOverCircle(mouse_pos, golf_ball, ballCenter()))
                || dragging == true)
            {
                dragState = true;
            }
            return dragState;
        }

        /// <summary>
        /// Sets the current shot power to 0
        /// </summary>
        public void releaseShot()
        {
            updateShotPower(0);
        }

        /// <summary>
        /// Sets the current shot power to the calculated distance between the
        /// mouse and the center of the ball
        /// </summary>
        public void windupShot()
        {
            updateShotPower(distance(mouse_pos, ballCenter()));
        }

        /// <summary>
        /// Set the state of the current shot. If the mouse is released,
        /// release the shot. If the mouse is still being dragged from
        /// the ball, continue winding up the shot
        /// </summary>
        /// <param name="dragState"> whether the mouse is being dragged
        /// from the ball or not.</param>
        public void updateShotState(bool dragState)
        {
            if (dragState == false)
            {
                releaseShot();
            }
            else
            {
                windupShot();
            }
        }

        /// <summary>
        /// Calculates the distance between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance between the two points.</returns>
        public float distance(Vector2 point1, Vector2 point2)
        {
            return Vector2.Distance(point1, point2);
        }

        /// <summary>
        /// Gets the sprite texture for the golf ball that is currently in the 
        /// game; mainly used for determining the ball's size
        /// </summary>
        /// <returns>the reference to the golf ball's sprite texture.</returns>
        public Texture2D golfBall()
        {
            return golf_ball;
        }

        /// <summary>
        /// Gets the current position of the mouse
        /// </summary>
        /// <returns>the current mouse position.</returns>
        public Vector2 mousePos()
        {
            return mouse_pos;
        }

        /// <summary>
        /// Sets the current position of the mouse
        /// </summary>
        /// <param name="newMousePos">the new position vector for the 
        /// mouse.</param>
        public void moveMouseTo(Vector2 newMousePos)
        {
            mouse_pos = newMousePos;
        }

        /// <summary>
        /// Sets the current position of the mouse
        /// </summary>
        /// <param name="x">the new horizontal position of the mouse.</param>
        /// <param name="y">the new vertical position of the mouse.</param>
        public void moveMouseTo(float x, float y)
        {
            mouse_pos.X = x;
            mouse_pos.Y = y;
        }

        /// <summary>
        /// Gets the current position of the ball
        /// </summary>
        /// <returns>the current ball position.</returns>
        public Vector2 ballPos()
        {
            return ball_pos;
        }

        /// <summary>
        /// Sets the current position of the ball
        /// </summary>
        /// <returns>the current ball position.</returns>
        public Vector2 ballCenter()
        {
            Vector2 center = ball_pos;
            center.X += golf_ball.Width / 2;
            center.Y += golf_ball.Height / 2;
            return center;
        }

        /// <summary>
        /// Sets the current position of the ball
        /// </summary>
        /// <param name="newBallPos">the new position vector for the 
        /// ball.</param>
        public void moveBallTo(Vector2 newBallPos)
        {
            ball_pos = newBallPos;
        }

        /// <summary>
        /// Sets the current position of the ball
        /// </summary>
        /// <param name="x">the new horizontal position of the ball.</param>
        /// <param name="y">the new vertical position of the ball.</param>
        public void moveBallTo(float x, float y)
        {
            ball_pos.X = x;
            ball_pos.Y = y;
        }

        /// <summary>
        /// Gets the current shot power of the swing being wound up
        /// </summary>
        /// <returns>the current shot power.</returns>
        public float shotPower()
        {
            return shot_power;
        }

        /// <summary>
        /// Sets the shot power of the swing being wound up
        /// </summary>
        /// <param name="newShotPower">the new power of the shot.</param>
        public void updateShotPower(float newShotPower)
        {
            shot_power = newShotPower;
        }

        /// <summary>
        /// Gets the current dragging state of the mouse, meaning if the user 
        /// is currently drawing back a shot or not
        /// </summary>
        /// <returns>the current dragging state of the mouse.</returns>
        public bool dragState()
        {
            return dragging;
        }

        /// <summary>
        /// Sets the current dragging state of the mouse
        /// </summary>
        /// <param name="newDraggingState">the new power of the shot.</param>
        public void updateDragState(bool newDraggingState)
        {
            dragging = newDraggingState;
        }
    }
}