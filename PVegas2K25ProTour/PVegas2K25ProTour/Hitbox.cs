//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Checks for collisions with other hitboxes every frame
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using System;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Hitbox determines if it is overlapping any other hitbox objects at any
    /// given time
    /// </summary>-------------------------------------------------------------
    public class Hitbox
    {
        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Creates a new hitbox object
        /// </summary>---------------------------------------------------------
        public Hitbox() { }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
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

        //***************************SPECIAL NOTE!*****************************
        // The method below, "collisionCircleToRect," was created by user:
        // "eJames" On Stack Overflow. This code was posted on 12-31-08 at 1:14
        // Here is a link to that resource:
        // https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection
        // ********************************************************************

        /// <summary>----------------------------------------------------------
        /// Tells when a circle object hits a rectangle object.
        /// </summary>
        /// <param name="circle">the circle's position and circumference.
        /// </param>
        /// <param name="rect">the rectangle that it is/isn't colliding with.
        /// </param>
        /// <returns>whether or not the circle and the rectangle are colliding.
        /// </returns>
        /// -------------------------------------------------------------------
        public bool collisionCircleToRect(GameObject circle, GameObject rect)
        {
            bool collision = false;
            Vector2 distance = distanceVector(rect.center(), circle.center());
            distance = absVector(distance);
            Vector2 rect_corner = new Vector2(rect.width() / 2,
                rect.height() / 2);
            float corner_dist_sq = distanceSquared(distance, rect_corner);
            // If the circle is far enough away from the rectangle for no
            // collision from any angle to be possible, there is no collision
            if (distance.X > (rect.width() / 2 + circle.radius()) ||
                distance.Y > (rect.height() / 2 + circle.radius()))
            {
                collision = false;
            }
            // If the circle is close enough to the rectangle in any of the
            // cardinal directions or if the distance from the rectangle's
            // corners to the original distance vector is less than the radius
            // of the circle squared, then a collision must be occurring
            else if (distance.X <= (rect.width() / 2) ||
                distance.Y <= (rect.height() / 2) ||
                corner_dist_sq <= MathF.Pow(circle.radius(), 2))
            {
                collision = true;
            }
            return collision;
        }

        /// <summary>----------------------------------------------------------
        /// Determines whether or not a point in space overlaps a rectangle
        /// based on the position of its center and its sprite's size
        /// </summary>
        /// <param name="point">the point to be checked.</param>
        /// <param name="rect">the rectangle the point is/isn't overlapping.
        /// </param>
        /// <returns>whether or not the point overlaps the rectangle.</returns>
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

        /// <summary>----------------------------------------------------------
        /// Tells when the ball "hits" the screen border
        /// </summary>
        /// <param name="ball">the ball whose position is being tracked.
        /// </param>
        /// <returns>whether the ball is out of bounds or not.</returns>
        /// -------------------------------------------------------------------
        public bool isBallOutOfbounds(Ball ball, GraphicsDeviceManager gdm)
        {
            Vector2 ballPos = ball.position();
            float ballCircumference = ball.radius() * 2;
            return (ballPos.X <= 0 || ballPos.X + ballCircumference >=
                gdm.PreferredBackBufferWidth
                || ballPos.Y <= 0 || ballPos.Y + ballCircumference >=
                gdm.PreferredBackBufferHeight);
        }

        //***************************SPECIAL NOTE!*****************************
        // The method below, "rectCollisionNormal," was created by user:
        // "Biggy Smith" On Stack Overflow. This code was posted on 01-19-17 at
        // 23:10. Here is a link to that resource:
        // https://gamedev.stackexchange.com/questions/136073/how-does-one-calculate-the-surface-normal-in-2d-collisions
        // ********************************************************************

        /// <summary>----------------------------------------------------------
        /// Calculates the collision normal of two objects, the dynamic object
        /// being a circle and the static object being a rectangle
        /// </summary>
        /// <param name="circle">the dynamic circle object.</param>
        /// <param name="rect">the static rectangle object.</param>
        /// <returns>the collision normal as a vector.</returns>
        /// -------------------------------------------------------------------
        public Vector2 rectCollisionNormal(GameObject circle, GameObject rect)
        {
            Vector2 distance = distanceVector(circle.center(), rect.center());
            float bound_x = rect.width() / 2;
            float bound_y = rect.height() / 2;
            // Determine which vertical face of the rectangle the ball came
            // in contact with
            float collision_x = dotProduct(distance, Vector2.UnitX);
            if (collision_x > bound_x)
            {
                collision_x = bound_x;
            }
            else if (collision_x <= -bound_x)
            {
                collision_x = -bound_x;
            }
            // Determine which horizontal face of the rectangle the ball came
            // in contact with
            float collision_y = dotProduct(distance, Vector2.UnitY);
            if (collision_y > bound_y)
            {
                collision_y = bound_y;
            }
            else if (collision_y <= -bound_y)
            {
                collision_y = -bound_y;
            }
            // Find the closest point to the circle on the surface collided
            // with
            Vector2 closest_point = rect.center() + collision_x * Vector2.UnitX
                + collision_y * Vector2.UnitY;
            return circle.center() - closest_point;
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the distance between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance between the two points.</returns>
        /// -------------------------------------------------------------------
        public float distance(Vector2 point1, Vector2 point2)
        {
            return Vector2.Distance(point1, point2);
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the square distance between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the square distance between the two points.</returns>
        /// -------------------------------------------------------------------
        public float distanceSquared(Vector2 point1, Vector2 point2)
        {
            return Vector2.DistanceSquared(point1, point2);
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the distance vector between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance vector between the two points.</returns>
        /// -------------------------------------------------------------------
        public Vector2 distanceVector(Vector2 point1, Vector2 point2)
        {
            Vector2 distance = point1;
            distance.X -= point2.X;
            distance.Y -= point2.Y;
            return distance;
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the dot product of two vectors
        /// </summary>
        /// <param name="vector1">the first operand in the dot product.</param>
        /// <param name="vector2">the second operand in the dot product.
        /// </param>
        /// <returns>the dot product of the two vectors.</returns>
        /// -------------------------------------------------------------------
        public float dotProduct(Vector2 vector1, Vector2 vector2)
        {
            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y);
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the absolute value of a vector's parameters and returns
        /// the vector with those calculated values in place
        /// </summary>
        /// <param name="vector">the vector whose absolute value will be taken.
        /// </param>
        /// <returns>the absolute value of the vector.</returns>
        /// -------------------------------------------------------------------
        public Vector2 absVector(Vector2 vector)
        {
            Vector2 abs_vector = vector;
            abs_vector.X = MathF.Abs(abs_vector.X);
            abs_vector.Y = MathF.Abs(abs_vector.Y);
            return abs_vector;
        }
    }
}