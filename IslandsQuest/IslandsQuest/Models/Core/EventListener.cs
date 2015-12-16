﻿using System;
using System.Collections.Generic;
using IslandsQuest.Models.EntityModels;
using IslandsQuest.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IslandsQuest.Models.Core
{
    public class EventListener
    {
        private Character Charachter;
        
        public Level GameLevel { get; set; }

        public EventListener(Character character, Level level)
        {
            Charachter = character;
            GameLevel = level;
            // Add "ListChanged" to the Changed event on "List".
            Charachter.PointChanged += new GameOverEventHandler(HandlePointChanged);
        }

        // This will be called whenever the list changes.
        public void HandlePointChanged(object sender, EventArgs eventArgs)
        {
            this.GameLevel = Level.GameOver;
        }

        public void Detach()
        {
            Charachter.PointChanged -= new GameOverEventHandler(HandlePointChanged);
        }
    }
}
