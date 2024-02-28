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
            using var newGame = new GameControl();
            newGame.Run();
            Ball golfBallReference = newGame.getBall();
            Vector2 golfBallCenter = golfBallReference.center();
            bool overlap = golfBallReference.isPointOverBall(golfBallCenter);
            Assert.IsTrue(overlap);
        }

        /// <summary>
        /// Checks to see if the point at the edge of the ball overlaps the 
        /// ball itself, which it always should.
        /// </summary>
        [TestMethod]
        public void TestIsPointOverSpriteEdge()
        {
            using var newGame = new GameControl();
            newGame.Run();
            Ball golfBallReference = newGame.getBall();
            Vector2 golfBallEdge = golfBallReference.center();
            golfBallEdge.Y += (golfBallReference.radius());
            bool overlap = golfBallReference.isPointOverBall(golfBallEdge);
            Assert.IsTrue(overlap);
        }

        /// <summary>
        /// Checks to see if a point off of the ball overlaps the ball itself, 
        /// which it never should.
        /// </summary>
        [TestMethod]
        public void TestIsPointNotOverSprite()
        {
            using var newGame = new GameControl();
            newGame.Run();
            Ball golfBallReference = newGame.getBall();
            Vector2 pointOnMap = golfBallReference.center();
            // Add the size of the radius to each coordinate so that the point
            // is in the top right corner of the ball's square sprite but is
            // not actually overlapping the circular ball itself
            pointOnMap.X += golfBallReference.radius();
            pointOnMap.Y += golfBallReference.radius();
            bool overlap = golfBallReference.isPointOverBall(pointOnMap);
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
            using var newGame = new GameControl();
            newGame.Run();
            Ball golfBallReference = newGame.getBall();
            Shot shotReference = newGame.getShot();

            // Set the mouse's position to a point close to the center of the
            // golf ball and record the shot's power at that point
            Vector2 pointOnMap = golfBallReference.center();
            pointOnMap.X += golfBallReference.radius();
            pointOnMap.Y += golfBallReference.radius();
            shotReference.windupShot(pointOnMap, 
                golfBallReference.center());
            float closeShotPower = shotReference.launchPower();

            // Set the mouse's position to a point farther from the center of
            // the golf ball and record the shot's power at that point
            pointOnMap.X += golfBallReference.radius();
            pointOnMap.Y += golfBallReference.radius();
            shotReference.windupShot(pointOnMap,
                golfBallReference.center());
            float farShotPower = shotReference.launchPower();

            Assert.IsTrue(farShotPower > closeShotPower);
        }
    }
}