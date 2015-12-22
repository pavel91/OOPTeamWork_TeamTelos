using System;

namespace MonsterQuest.Struct
{
    public struct GameState
    {
        public int level;
       
        public GameState(int level)
            : this()
        {
            this.Level = level;
        }

        public int Level
        {
            get { return this.level; }
            set
            {
                if (value < 0 || value > 5)
                {
                    throw new ArgumentOutOfRangeException("Level should be in range [0...4].");
                }
                this.level = value;
            }
        }

    }
}
