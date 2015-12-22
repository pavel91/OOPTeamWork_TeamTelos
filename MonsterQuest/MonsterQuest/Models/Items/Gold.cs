using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Items
{
    public class Gold : Item
    {
        private const int activeTimeLimit = 5000;
        private Vector2 boundOffset = new Vector2(15, 10);
        private Vector2 defaultVelocity = new Vector2(0, 5f);
        private const int numOfRows = 8;
        private const int numOfCols = 12;
        private const int row = 6;
        private const int col = 8;

        //int activeTimeLimit, Texture2D image,
        // int numOfRows, int numOfCols, int row, int col
        public Gold(Texture2D image)
            : base(activeTimeLimit, image, numOfRows, numOfCols, row, col)
        {
            this.BoundsOffset = boundOffset;
            int xPosition = this.GenerateRandomPosition();
            this.Position = new Vector2(xPosition, 0);
            this.Velocity = defaultVelocity;
        }

        
    }
}
