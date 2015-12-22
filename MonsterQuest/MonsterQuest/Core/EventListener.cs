using System;
using MonsterQuest.Models.Entities.Characters;

namespace MonsterQuest.Core
{
    public class EventListener
    {
        private Character Charachter;

        public bool GameOver { get; set; }

        public EventListener(Character character)
        {
            Charachter = character;
            GameOver = false;
            // Add "ListChanged" to the Changed event on "List".
            Charachter.PointChanged += new GameOverEventHandler(HandlePointChanged);
        }

        // This will be called whenever the list changes.
        public void HandlePointChanged(object sender, EventArgs eventArgs)
        {
            this.GameOver = true;
        }

        public void Detach()
        {
            Charachter.PointChanged -= new GameOverEventHandler(HandlePointChanged);
        }
    }
}
