//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Checks for collisions with other hitboxes every frame
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using System;
using System.Runtime.Intrinsics.Arm;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Hitbox determines if it is overlapping any other hitbox objects at any
    /// given time
    /// </summary>-------------------------------------------------------------
    public class Hitbox
    {
        private Vector2 VECTOR_UP = new Vector2(0, 1);
        private Vector2 VECTOR_RIGHT = new Vector2(1, 0);

        private GraphicsDeviceManager gdm;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>
        /// Sinks up the GraphicsDevice Managers in the Constructor
        /// </summary>
        /// <param name="gdm">GraphicsDeviceManager from main</param>
        public Hitbox(GraphicsDeviceManager gdm)
        {
            this.gdm = gdm;
        }

        //---------------------------------------------------------------------
        // COLLISION METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Determines if 2 circle objects are colliding with each other
        /// </summary>
        /// <param name="circle1">the first circle for its position and radius.
        /// </param>
        /// <param name="circle2">the second circle for its position and 
        /// radius.</param>
        /// <returns>whether or not the two circles are colliding.</returns>
        /// -------------------------------------------------------------------
        public bool collisionCircleToCircle(GameObject circle1, 
            GameObject circle2)
        {
            float radius = circle1.radius() + circle2.radius();
            return Vector2.Distance(circle1.center(), 
                circle2.center()) <= radius;
        }

        /// <summary>----------------------------------------------------------
        /// Determines whether or not a point in space overlaps a circle based
        /// on the position of its center and the size of its sprite
        /// </summary>
        /// <param name="point">the point to be checked.</param>
        /// <param name="circle">the circle the point is/isn't overlapping.
        /// </param>
        /// <returns>whether or not the point overlaps the circle.</returns>
        /// -------------------------------------------------------------------
        public bool collisionPointToCircle(Vector2 point, GameObject circle)
        {
            float pointToCenter = distance(point, circle.center());
            return pointToCenter <= circle.radius();
        }

        /// <summary>----------------------------------------------------------
        /// Determines whether or not a point in space overlaps a rectangle
        /// based on the position of its center and its sprite sprite
        /// </summary>
        /// <param name="point">the point to be checked.</param>
        /// <param name="circle">the circle the point is/isn't overlapping.
        /// </param>
        /// <returns>whether or not the point overlaps the circle.</returns>
        /// -------------------------------------------------------------------
        public bool collisionPointToRect(Vector2 point, GameObject rect)
        {
            bool collision = false;
            if (point.X <= rect.center().X + rect.width() / 2
                && point.X >= rect.center().X - rect.width() / 2
                && point.Y <= rect.center().Y + rect.height() / 2
                && point.Y >= rect.center().Y - rect.height() / 2)
            {
                collision = true;
            }
            return collision;
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
        /// tells when the ball "hits" the boarder 
        /// </summary>
        /// <param name="ball">the ball for its position and circumfrance</param>
        /// <returns></returns>
        public bool isBallOutOfbounds(Ball ball)
        {
            Vector2 ballPos=ball.position();
            float ballCircumfrance = ball.radius() * 2;
            return (ballPos.X <= 0 || ballPos.X + ballCircumfrance >= 
                gdm.PreferredBackBufferWidth
                || ballPos.Y <= 0 || ballPos.Y + ballCircumfrance >= 
                gdm.PreferredBackBufferHeight);
        }

        /// <summary>
        /// Calculates the distance vector between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance vector between the two points.</returns>
        public Vector2 distanceVector(Vector2 point1, Vector2 point2)
        {
            Vector2 distance = point2;
            distance.X -= point1.X;
            distance.Y -= point1.Y;
            return distance;
        }

        /// <summary>
        /// Calculates the absolute value of a float and returns it
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance vector between the two points.</returns>
        public float absValue(float value)
        {
            return MathF.Abs(value);
        }

        public float dotProduct(Vector2 vector1, Vector2 vector2)
        {
            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y);
        }

        //***********************************SPECIAL NOTE!*************************************
        // All code below this point was created by user: "eJames" On Stack Overflow
        // This code was posted on 12-31-08 at 1:14
        // Here is a link to that resource: https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection
        // ************************************************************************************

        /// <summary>
        /// tells when a circle object hits a rectangle object
        /// </summary>
        /// <param name="circle">the circle's position and circumference</param>
        /// <param name="rect">the rectangle that it may be colliding with</param>
        /// <returns></returns>
        public bool collisionCircleToRect(GameObject circle, GameObject rect)
        {
            bool collision = false;
            Vector2 distance = distanceVector(rect.center(), circle.center());
            float distance_x = absValue(distance.X);
            float distance_y = absValue(distance.Y);
            float corner_dist_sq = MathF.Pow(distance_x -
                (rect.width() / 2), 2) + MathF.Pow(distance_y -
                (rect.height() / 2), 2);
            if (distance_x > (rect.width() / 2 + circle.radius()) ||
                distance_y > (rect.height() / 2 + circle.radius()))
            {
                collision = false;
            }
            else if (distance_x <= (rect.width() / 2) ||
                distance_y <= (rect.height() / 2) ||
                corner_dist_sq <= MathF.Pow(circle.radius(), 2))
            {
                collision = true;
            }
            return collision;
        }

        //***********************************SPECIAL NOTE!*************************************
        // All code below this point was created by user: "Biggy Smith" On Stack Overflow
        // This code was posted on 01-19-17 at 23:10
        // Here is a link to that resource: https://gamedev.stackexchange.com/questions/136073/how-does-one-calculate-the-surface-normal-in-2d-collisions
        // ************************************************************************************

        public Vector2 rectCollisionNormal(GameObject circle, GameObject rect)
        {
            Vector2 distance = distanceVector(rect.center(), circle.center());
            float bound_x = rect.width() / 2;
            float bound_y = rect.height() / 2;
            float collision_x = dotProduct(distance, VECTOR_RIGHT);
            if (collision_x > bound_x)
            {
                collision_x = bound_x;
            }
            else if (collision_x < -bound_x)
            {
                collision_x = -bound_x;
            }
            float collision_y = dotProduct(distance, VECTOR_UP);
            if (collision_y > bound_y)
            {
                collision_y = bound_y;
            }
            else if (collision_y < -bound_y)
            {
                collision_y = -bound_y;
            }
            Vector2 closest_point = rect.center() + collision_x * VECTOR_RIGHT
                + collision_y * VECTOR_UP;
            return circle.center() - closest_point;
        }
    }
}
