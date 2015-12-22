using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Core.Factories
{
    public class ItemFactory : IItemFactory
    {
        public IItem CreateItem(string itemName,Texture2D itemImage)
        {
            switch (itemName)
            {
                case "Gold":
                    return new Gold(itemImage);
                case "Potion":
                    return new Potion(itemImage);
                default:
                    throw new ArgumentException("Invalid item type");
            }
        }
    }
}
