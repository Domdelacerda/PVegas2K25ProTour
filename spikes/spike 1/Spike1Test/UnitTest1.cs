//-----------------------------------------------------------------------------
// Name: Dominic De La Cerda
// Project: Spike 1 - Mouse controls and shot power
// Purpose: Test functionality for the mouse being able to click on the ball
// and for shot power to dynamically change based on the mouse's position
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spike1;

namespace Spike1Test
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// The TestIsPointOverSpriteCenter test method checks to see if the 
        /// point at the center of the ball overlaps the ball itself, which it
        /// always should.
        /// </summary>
        [TestMethod]
        public void TestIsPointOverSpriteCenter()
        {
            using var newGame = new Spike1.GameControl();
            newGame.Run();
            Texture2D golfBallReference = newGame.golfBall();
            Vector2 golfBallCenter = newGame.ballCenter();
            bool overlap = newGame.isPointOverCircle(golfBallCenter, 
                golfBallReference, newGame.ballCenter());
            Assert.IsTrue(overlap);
        }

        /// <summary>
        /// The TestIsPointOverSpriteEdge test method checks to see if the 
        /// point at the edge of the ball overlaps the ball itself, which it 
        /// always should.
        /// </summary>
        [TestMethod]
        public void TestIsPointOverSpriteEdge()
        {
            using var newGame = new Spike1.GameControl();
            newGame.Run();
            Texture2D golfBallReference = newGame.golfBall();
            Vector2 golfBallEdge = newGame.ballCenter();
            golfBallEdge.Y += (golfBallReference.Bounds.Height / 2);
            bool overlap = newGame.isPointOverCircle(golfBallEdge,
                golfBallReference, newGame.ballCenter());
            Assert.IsTrue(overlap);
        }

        /// <summary>
        /// The TestIsPointNotOverSprite test method checks to see if a point
        /// far from the ball overlaps the ball itself, which it never should.
        /// </summary>
        [TestMethod]
        public void TestIsPointNotOverSprite()
        {
            using var newGame = new Spike1.GameControl();
            newGame.Run();
            Texture2D golfBallReference = newGame.golfBall();
            Vector2 pointOnMap = newGame.ballCenter();
            // Add the size of each bound to the center position so that no
            // matter what size the ball is, the point will never overlap the
            // ball
            pointOnMap.X += golfBallReference.Bounds.Width;
            pointOnMap.Y += golfBallReference.Bounds.Height;
            bool overlap = newGame.isPointOverCircle(pointOnMap,
                golfBallReference, newGame.ballCenter());
            Assert.IsFalse(overlap);
        }

        /// <summary>
        /// The TestWindupShot test method examines 2 different points away 
        /// from the ball--one farther and one closer--to determine if the 
        /// farther point has more shot power, which it always should
        /// </summary>
        [TestMethod]
        public void TestWindupShot()
        {
            using var newGame = new Spike1.GameControl();
            newGame.Run();
            Texture2D golfBallReference = newGame.golfBall();

            // Set the mouse's position to a point close to the center of the
            // golf ball and record the shot's power at that point
            Vector2 pointOnMap = newGame.ballCenter();
            pointOnMap.X += golfBallReference.Bounds.Width;
            pointOnMap.Y += golfBallReference.Bounds.Height;
            newGame.moveMouseTo(pointOnMap.X, pointOnMap.Y);
            newGame.windupShot();
            float closeShotPower = newGame.shotPower();

            // Set the mouse's position to a point far from the center of the
            // golf ball and record the shot's power at that point
            pointOnMap.X += golfBallReference.Bounds.Width;
            pointOnMap.Y += golfBallReference.Bounds.Height;
            newGame.moveMouseTo(pointOnMap.X, pointOnMap.Y);
            newGame.windupShot();
            float farShotPower = newGame.shotPower();

            Assert.IsTrue(farShotPower > closeShotPower);
        }
    }
}