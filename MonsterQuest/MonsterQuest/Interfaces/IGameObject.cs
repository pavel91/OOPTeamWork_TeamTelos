using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IGameObject : IMovable,IDrawable,ICollide
    {
        Texture2D Image { get; }

        Rectangle Bounds { get; }

        Vector2 Position { get; }

        Vector2 Velocity { get; }

        //int Width { get; }

        //int Height { get; }
    }
}
