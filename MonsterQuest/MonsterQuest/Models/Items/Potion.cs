using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Items
{
    public class Potion : Item
    {
        private const int activeTimeLimit = 5000;
        private Vector2 boundOffset = new Vector2(15, 10);
        private Vector2 defaultVelocity = new Vector2(0, 5f);
        private const int numOfRows = 8;
        private const int numOfCols = 12;
        private const int row = 0;
        private const int col = 9;
        private int health = 15;


        public int Health { get { return this.health; } set { this.health = value; } }

        //int activeTimeLimit, Texture2D image,
        // int numOfRows, int numOfCols, int row, int col
        public Potion(Texture2D image)
            : base(activeTimeLimit, image, numOfRows, numOfCols, row, col)
        {
            this.BoundsOffset = boundOffset;
            this.Velocity = defaultVelocity;
        }

    }
}
