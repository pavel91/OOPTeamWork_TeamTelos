using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Entities.Enemies
{
    public class Skeleton : Enemy
    {
        private const int DefaultSkeletonHealth = 75;
        private const int DefaultSkeletonDamage = 15;
        private const int DefaultSkeletionSpriteRows = 8;
        private const int DefaultSkeletonSpriteCols = 9;
        private Vector2 DefaultSkeletonOffset = new Vector2(15, 4);
        private Vector2 DefaultSkeletonVelocity = new Vector2(7.5f, 0);
        private Vector2 DefaultSkeletonPosition = new Vector2(900, 330);
        private const int WalkingLeftInitialFrame = 18;
        private const int WalkingRightInitialFrame = 55;
        private const int WalkingLeftLastFrame = 23;
        private const int WalkingRightLastFrame = 60;
        private const int DefaultSkeletonScore = 15;

        public Skeleton(Texture2D image)
            : base(image, DefaultSkeletonHealth, DefaultSkeletonDamage, DefaultSkeletionSpriteRows, DefaultSkeletonSpriteCols,
            WalkingLeftInitialFrame, WalkingRightInitialFrame, WalkingLeftLastFrame, WalkingRightLastFrame)
        {
            this.Position = DefaultSkeletonPosition;
            this.Velocity = DefaultSkeletonVelocity;
            this.BoundsOffset = DefaultSkeletonOffset;
            this.Score = DefaultSkeletonScore;
        }

        public override void Attack()
        {
            throw new NotImplementedException();
        }
    }
}
