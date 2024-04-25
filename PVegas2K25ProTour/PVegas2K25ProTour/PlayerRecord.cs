using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVegas2K25ProTour
{
    public class PlayerRecord
    {
        public string User { get; set; }

        // Strokes, if we choose to implement, should really be level-based data
        public int Strokes { get; set; }
        public int Coins { get; set; }
        public int TotalHolesCompleted { get; set; }
        public int TotalStrokesLifetime {  get; set; }

        // Level Based Data
        // IMPLEMENT: Levels lock/unlock accessability
        public bool isLevelOneUnlocked { get; set; }
        public int playerScoreLevelOne { get; set; }

        public bool isLevelTwoUnlocked { get; set; }
        public int playerScoreLevelTwo { get; set; }

        public bool isLevelThreeUnlocked { get; set; }
        public int playerScoreLevelThree { get; set; }

        public bool isLevelFourUnlocked { get; set; }
        public int playerScoreLevelFour { get; set; }

        public bool isLevelFiveUnlocked { get; set; }
        public int playerScoreLevelFive { get; set; }

        // Cosmetics Based Data
        public bool isCosmeticOneUnlocked { get; set; }
        public bool isCosmeticTwoUnlocked { get; set; }
        public bool isCosmeticThreeUnlocked { get; set; }

        public string currentCosmetic {  get; set; }
        public Color currentColor { get; set; }

        // Settings Based Data
        public int swingSensitivityPreference { get; set; } = 5;
        public bool audioEnabled { get; set; }
        public int volumePreference { get; set; } = 5;
        public int holeSize { get; set; } = 5;
    }
}
