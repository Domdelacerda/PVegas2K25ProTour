//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Checks for collisions with other hitboxes every frame
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using System.Runtime.Intrinsics.Arm;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Hitbox determines if it is overlapping any other hitbox objects at any
    /// given time
    /// </summary>-------------------------------------------------------------
    public class Hitbox
    {
        private GraphicsDeviceManager gdm;
        /// <summary>
        /// Sinks up the GraphicsDevice Managers in the Constructor
        /// </summary>
        /// <param name="gdm">GraphicsDeviceManager from main</param>
        public Hitbox(GraphicsDeviceManager gdm)
        {
            this.gdm = gdm;
        }
        /// <summary>
        /// Tells when ball and Holw collide with each other
        /// </summary>
        /// <param name="circle1">the ball for its position and radius</param>
        /// <param name="circle2">the hole for its position and radius</param>
        public bool collisionCircleToCircle(GameObject circle1, 
            GameObject circle2)
        {
            float radius = circle1.radius() + circle2.radius();
            return Vector2.Distance(circle1.center(), 
                circle2.center()) <= radius;
        }
        /// <summary>
        /// tells when the ball "hits" the boarder 
        /// </summary>
        /// <param name="ball">the ball for its position and circumfrance</param>
        /// <returns></returns>
        public bool isBallOutOfbounds(Ball ball)
        {
            Vector2 ballPos=ball.position();
            float ballCircumfrance = ball.radius() * 2;
            return (ballPos.X <= 0 || ballPos.X + ballCircumfrance >= gdm.PreferredBackBufferWidth
                || ballPos.Y <= 0 || ballPos.Y + ballCircumfrance >= gdm.PreferredBackBufferHeight);
        }
        /// <summary>
        /// tells when the ball hits a rectangle object
        /// </summary>
        /// <param name="ball">the ball position and circumfrance</param>
        /// <param name="rect">the rectang that it may be colliding with</param>
        /// <returns></returns>
        public bool isCollidingWithRectangle(Ball ball, Rectangle rect)
        {
            //right->left->bottom->top
            float radius = ball.radius() * 2;
            if (ball.position().X + radius + 24 >= rect.X && ball.position().X <= rect.X + rect.Width
                && ball.position().Y - radius <= rect.Y && ball.position().Y + 24 >= rect.Y - rect.Height)
                return true;
            return false;
        }
    }
}
