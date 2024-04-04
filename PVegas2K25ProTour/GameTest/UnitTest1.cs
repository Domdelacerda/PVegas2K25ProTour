//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Continuously test the functionality of our game 
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PVegas2K25ProTour;

namespace GameTest
{
    /// <summary>--------------------------------------------------------------
    /// Tests all expected behaviors in our game to ensure that when code is
    /// refactored that functionality does not change
    /// </summary>-------------------------------------------------------------
    [TestClass]
    public class UnitTest1
    {
        /// <summary>----------------------------------------------------------
        /// Checks to see if the point at the center of the ball overlaps the 
        /// ball itself, which it always should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestIsPointOverSpriteCenter()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 golf_ball_center = golf_ball_reference.center();
            bool overlap =
                golf_ball_reference.isPointOverBall(golf_ball_center);

            Assert.IsTrue(overlap);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the point at the edge of the ball overlaps the 
        /// ball itself, which it always should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestIsPointOverSpriteEdge()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 golf_ball_edge = golf_ball_reference.center();
            golf_ball_edge.Y += (golf_ball_reference.radius());
            bool overlap = golf_ball_reference.isPointOverBall(golf_ball_edge);

            Assert.IsTrue(overlap);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a point off of the ball overlaps the ball itself, 
        /// which it never should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestIsPointNotOverSprite()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 point_on_map = golf_ball_reference.center();
            // Add the size of the radius to each coordinate so that the point
            // is in the top right corner of the ball's square sprite but is
            // not actually overlapping the circular ball itself
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            bool overlap = golf_ball_reference.isPointOverBall(point_on_map);

            Assert.IsFalse(overlap);
        }

        /// <summary>----------------------------------------------------------
        /// Examines 2 different points away from the ball--one farther and 
        /// one closer--to determine if the farther point has more shot power, 
        /// which it always should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestWindupShot()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
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

            Assert.IsTrue(far_shot_power > close_shot_power);
        }

        /// <summary>----------------------------------------------------------
        /// Examines 2 different points away from the ball--one farther and 
        /// one closer--to determine if the farther point has a larger
        /// arrow display, which it always should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestArrowSize()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
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

            Assert.IsTrue(far_shot_arrow_length > close_shot_arrow_length);
        }

        /// <summary>----------------------------------------------------------
        /// Determines if the shot arrow's length is zero (not visible) when
        /// the mouse is dragged to the center of the ball, meaning that the
        /// shot power is zero
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestArrowSizeZero()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();
            Vector2 golf_ball_center = golf_ball_reference.center();
            shot_reference.windupShot(golf_ball_center,
                golf_ball_center);
            shot_reference.resizeArrow(golf_ball_reference.center());
            float shot_arrow_length = shot_reference.arrowLength();

            Assert.IsTrue(shot_arrow_length == 0);
        }

        [TestMethod]
        public void TestBallStops()
        {
            using var newGame = new GameControl();
            newGame.RunOneFrame();
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

            Assert.IsTrue(golfBallReference.getSpeed() == Vector2.Zero);
        }

        [TestMethod]
        public void TestBallFriction()
        {
            Vector2 ballSpeed1;
            Vector2 ballSpeed2;
            using var newGame = new GameControl();
            newGame.RunOneFrame();
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

            ballSpeed1 = golfBallReference.getSpeed();
            // update reduces speed by drag reduction scale
            golfBallReference.updateSpeed();
            ballSpeed2 = golfBallReference.getSpeed();

            Assert.AreNotEqual(ballSpeed1, ballSpeed2);
        }

        [TestMethod]
        public void TestBallLaunch()
        {
            using var newGame = new GameControl();
            newGame.RunOneFrame();
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

            Assert.IsTrue(golfBallReference.getSpeed().X != 0 || 
                golfBallReference.getSpeed().Y != 0);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if files are being correctly saved to and loaded from
        /// an xml file by creating a playerRecord and assigning them data, 
        /// saving and loading that data to an xml file, and assigning the 
        /// contents of the loaded file to a new record. Then we check to make
        /// sure that the original record is equal to the loaded record. 
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestFileSaveAndLoad()
        {
            PlayerRecord my_first_player = new PlayerRecord();
            PlayerRecord my_second_player = new PlayerRecord();

            my_first_player.Strokes = 6;
            my_first_player.User = "Bob Sullivan";
            
            SaveLoadSystem.Save(my_first_player);
            my_second_player = SaveLoadSystem.Load<PlayerRecord>();

            Assert.IsTrue(my_first_player.Strokes == my_second_player.Strokes &&
                my_first_player.User == my_second_player.User);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a shot power greater than the maximum is truncated
        /// to the maximum value
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestClampShotPower()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();
            Vector2 far_point = new Vector2(10000f, 10000f);
            shot_reference.Update(true, far_point, golf_ball_reference);

            Assert.IsTrue(shot_reference.launchPower() == 
                shot_reference.maxShotPower());
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a vector is reflected correctly when its angle
        /// of incidence is equal to the normal vector of collision, which
        /// means that the angle between incidence and normal should be the
        /// same as the angle between normal and reflection
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestVectorReflectionStraight()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Obstacle obstacle_reference = new Obstacle(Vector2.Zero, 
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            Vector2 incidence = Vector2.UnitX;
            Vector2 normal = Vector2.UnitX;
            Vector2 reflection = obstacle_reference.reflectVector(incidence, 
                normal);
            float angle_in = obstacle_reference.angleBetweenVectors(incidence,
                normal);
            float angle_ir = (float)Math.PI - 
                obstacle_reference.angleBetweenVectors(normal, reflection);
            Assert.IsTrue(angle_in == angle_ir);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a vector is reflected correctly when its angle
        /// of incidence is different from the normal vector of collision, 
        /// which means that the angle between incidence and normal should be 
        /// the same as the angle between normal and reflection
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestVectorReflectionSkew()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Obstacle obstacle_reference = new Obstacle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            Vector2 incidence = -Vector2.One;
            Vector2 normal = Vector2.UnitY;
            Vector2 reflection = obstacle_reference.reflectVector(incidence,
                normal);
            float angle_in = obstacle_reference.angleBetweenVectors(incidence,
                normal);
            float angle_ir = (float)Math.PI - 
                obstacle_reference.angleBetweenVectors(normal, reflection);
            Assert.IsTrue(angle_in == angle_ir);
        }
    }
}