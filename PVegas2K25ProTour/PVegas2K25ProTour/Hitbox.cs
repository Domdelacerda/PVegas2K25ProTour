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
        /// <param name="ball">the ball for its position and radius</param>
        /// <param name="hole">the hole for its position and radius</param>
        /// <returns></returns>
        public bool isBallCollidingWithHole(Ball ball, Hole hole)
        {
            float radius = ball.radius()*hole.radius();
            return (!((Vector2.DistanceSquared(ball.position(), hole.position()) > (radius * radius))));
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
    }
}
