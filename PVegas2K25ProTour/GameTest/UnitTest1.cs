using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using PVegas2K25ProTour;

namespace GameTest
{

    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Checks to see if the point at the center of the ball overlaps the 
        /// ball itself, which it always should.
        /// </summary>
        [TestMethod]
        public void TestIsPointOverSpriteCenter()
        {
            using var new_game = new GameControl();
            new_game.Run();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 golf_ball_center = golf_ball_reference.center();
            bool overlap = 
                golf_ball_reference.isPointOverBall(golf_ball_center);
            new_game.quit();
            Assert.IsTrue(overlap);
        }

        /// <summary>
        /// Checks to see if the point at the edge of the ball overlaps the 
        /// ball itself, which it always should.
        /// </summary>
        [TestMethod]
        public void TestIsPointOverSpriteEdge()
        {
            using var new_game = new GameControl();
            new_game.Run();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 golf_ball_edge = golf_ball_reference.center();
            golf_ball_edge.Y += (golf_ball_reference.radius());
            bool overlap = golf_ball_reference.isPointOverBall(golf_ball_edge);
            new_game.quit();
            Assert.IsTrue(overlap);
        }

        /// <summary>
        /// Checks to see if a point off of the ball overlaps the ball itself, 
        /// which it never should.
        /// </summary>
        [TestMethod]
        public void TestIsPointNotOverSprite()
        {
            using var new_game = new GameControl();
            new_game.Run();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 point_on_map = golf_ball_reference.center();
            // Add the size of the radius to each coordinate so that the point
            // is in the top right corner of the ball's square sprite but is
            // not actually overlapping the circular ball itself
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            bool overlap = golf_ball_reference.isPointOverBall(point_on_map);
            new_game.quit();
            Assert.IsFalse(overlap);
        }

        /// <summary>
        /// Examines 2 different points away from the ball--one farther and 
        /// one closer--to determine if the farther point has more shot power, 
        /// which it always should
        /// </summary>
        [TestMethod]
        public void TestWindupShot()
        {
            using var new_game = new GameControl();
            new_game.Run();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // Set the mouse's position to a point close to the center of the
            // golf ball and record the shot's power at that point
            Vector2 point_on_map = golf_ball_reference.center();
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());
            float close_shot_power = shot_reference.launchPower();

            // Set the mouse's position to a point farther from the center of
            // the golf ball and record the shot's power at that point
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());
            float far_shot_power = shot_reference.launchPower();

            new_game.quit();
            Assert.IsTrue(far_shot_power > close_shot_power);
        }

        /// <summary>
        /// Examines 2 different points away from the ball--one farther and 
        /// one closer--to determine if the farther point has a larger
        /// arrow display, which it always should
        /// </summary>
        [TestMethod]
        public void TestArrowSize()
        {
            using var new_game = new GameControl();
            new_game.Run();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // Set the mouse's position to a point close to the center of the
            // golf ball and record the shot's arrow length at that point
            Vector2 point_on_map = golf_ball_reference.center();
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());
            shot_reference.resizeArrow(golf_ball_reference.center());
            float close_shot_arrow_length = shot_reference.arrowLength();

            // Set the mouse's position to a point farther from the center of
            // the golf ball and record the shot's power at that point
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());
            shot_reference.resizeArrow(golf_ball_reference.center());
            float far_shot_arrow_length = shot_reference.arrowLength();

            new_game.quit();
            Assert.IsTrue(far_shot_arrow_length > close_shot_arrow_length);
        }

        /// <summary>
        /// Determines if the shot arrow's length is zero (not visible) when
        /// the mouse is dragged to the center of the ball, meaning that the
        /// shot power is zero
        /// </summary>
        [TestMethod]
        public void TestArrowSizeZero()
        {
            using var new_game = new GameControl();
            new_game.Run();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();
            Vector2 golf_ball_center = golf_ball_reference.center();
            shot_reference.windupShot(golf_ball_center,
                golf_ball_center);
            shot_reference.resizeArrow(golf_ball_reference.center());
            float shot_arrow_length = shot_reference.arrowLength();
            new_game.quit();
            Assert.IsTrue(shot_arrow_length == 0);
        }

        [TestMethod]
        public void TestBallStops()
        {
            using var newGame = new GameControl();
            newGame.Run();
            Ball golfBallReference = newGame.getBall();
            Shot shotReference = newGame.getShot();

            // set mouse position and ball center position to different 
            // then call windup shot

            Vector2 point_on_map = golfBallReference.center();
            point_on_map.X += golfBallReference.radius();
            point_on_map.Y += golfBallReference.radius();
            shotReference.windupShot(point_on_map,
                golfBallReference.center());

            // Release the Shot to give it some movement
            shotReference.releaseShot(golfBallReference);

            // Stop the ball
            golfBallReference.ballStop();

            Assert.IsTrue(golfBallReference.getBallSpeed() == Vector2.Zero);
        }

        [TestMethod]
        public void TestBallFriction()
        {
            Vector2 ballSpeed1;
            Vector2 ballSpeed2;
            using var newGame = new GameControl();
            newGame.Run();
            Ball golfBallReference = newGame.getBall();
            Shot shotReference = newGame.getShot();

            // set mouse position and ball center position to different 
            // then call windup shot

            Vector2 point_on_map = golfBallReference.center();
            point_on_map.X += golfBallReference.radius();
            point_on_map.Y += golfBallReference.radius();
            shotReference.windupShot(point_on_map,
                golfBallReference.center());

            shotReference.releaseShot(golfBallReference);

            ballSpeed1 = golfBallReference.getBallSpeed();
            Thread.Sleep(300); // Waits for 3 seconds
            ballSpeed2 = golfBallReference.getBallSpeed();

            Assert.AreNotEqual(ballSpeed1, ballSpeed2);
        }

        [TestMethod]
        public void TestBallLaunch()
        {
            using var newGame = new GameControl();
            newGame.Run();
            Ball golfBallReference = newGame.getBall();
            Shot shotReference = newGame.getShot();

            // set mouse position and ball center position to different 
            // then call windup shot
 
            Vector2 point_on_map = golfBallReference.center();
            point_on_map.X += golfBallReference.radius();
            point_on_map.Y += golfBallReference.radius();
            shotReference.windupShot(point_on_map,
                golfBallReference.center());

            shotReference.releaseShot(golfBallReference);

            Assert.IsTrue(golfBallReference.getBallSpeed().X != 0 || 
                golfBallReference.getBallSpeed().Y != 0);
        }
    }
}