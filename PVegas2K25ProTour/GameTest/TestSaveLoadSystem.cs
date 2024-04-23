//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Continuously test the functionality of the save/load system
//-----------------------------------------------------------------------------

using PVegas2K25ProTour;

namespace GameTest
{
    /// <summary>--------------------------------------------------------------
    /// Tests all expected behaviors in the save/load system to ensure that 
    /// when code is refactored that functionality does not change
    /// </summary>-------------------------------------------------------------
    [TestClass]
    public class TestSaveLoadSystem
    {
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
    }
}
