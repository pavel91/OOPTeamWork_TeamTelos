using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Core.Data
{
    public class Data : IData
    {
        private IList<Enemy> enemies;
        private IList<IItem> items;
        private IList<Texture2D> bulletImages = new List<Texture2D>();

        public Data()
        {
            this.enemies = new List<Enemy>();
            this.items = new List<IItem>();
            this.bulletImages = new List<Texture2D>();
        }

        public IList<Enemy> Enemies { get{return this.enemies;} private set { this.enemies = value; } }

        public IList<IItem> Items { get { return this.items; } private set { this.items = value; } }

        public IList<Texture2D> BulletImages { get { return this.bulletImages; } private set { this.bulletImages = value; } }

        public void AddEnemies(Enemy enemy)
        {
            this.enemies.Add(enemy);
        }

        public void AddItems(IItem item)
        {
            this.items.Add(item);
        }

        public void AddBulletImage(Texture2D bulletImage)
        {
            this.bulletImages.Add(bulletImage);
        }

        public void RemoveInactiveElements()
        {
            var activeEnemies = this.Enemies.Where(e => e.IsAlive == true).ToList();
            this.Enemies = activeEnemies;

            var activeItems = this.Items.Where(i => i.IsActive == true).ToList();
            this.Items = activeItems;
        }
    }
}
