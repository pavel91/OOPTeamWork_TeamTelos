using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IItemFactory
    {
        IItem CreateItem(string itemName, Texture2D itemImage);
    }
}
