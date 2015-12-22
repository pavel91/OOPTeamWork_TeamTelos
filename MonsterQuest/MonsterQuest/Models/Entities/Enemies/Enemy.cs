using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Enums;
using MonsterQuest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Entities.Enemies
{
    public abstract class Enemy : Entity, IDamageApplied
    {
        private const int millisecondPerFrame = 80;
        private int timeSinceLastFrame = 0;

        private int rows;
        private int cols;

        private int currentFrame;
        private int firstFrame;
        private int lastFrame;

        private Vector2 boundOffset;
        private EnemyState enemyState;

        private int walkingLeftInitialFrame;
        private int walkingRightInitialFrame;
        private int walkingLeftLastFrame;
        private int walkingRightLastFrame;
        private bool hasAppliedDamage;

        private Vector2 enemyPosition;

        protected Enemy(Texture2D image,
            int health, int damage, int rows, int cols,
            int walkingLeftInitialFrame,
            int walkingRightInitialFrame,
            int walkingLeftLastFrame,
            int walkingRightLastFrame)
            : base(image, health, damage)
        {
            this.rows = rows;
            this.cols = cols;
            //this.boundOffset = offset;

            this.walkingLeftInitialFrame = walkingLeftInitialFrame;
            this.walkingLeftLastFrame = walkingLeftLastFrame;
            this.walkingRightInitialFrame = walkingRightInitialFrame;
            this.walkingRightLastFrame = walkingRightLastFrame;

            this.enemyPosition = this.Position;
            this.hasAppliedDamage = false;

            this.Width = this.Image.Width / this.cols;
            this.Height = this.Image.Height / this.rows;
        }

        public bool HasAppliedDamage { get { return this.hasAppliedDamage; } set { this.hasAppliedDamage = value; } }

        public Vector2 BoundsOffset { get { return this.boundOffset; } protected set { this.boundOffset = value; } }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondPerFrame)
            {

                //not dissappearing logic
                if (this.Position.X < 1)
                {
                    this.enemyState = EnemyState.WalkingRight;
                    this.SetFrames(this.enemyState);
                }
                if (this.Position.X > 800)
                {
                    this.enemyState = EnemyState.WalkingLeft;
                    this.SetFrames(this.enemyState);
                }

                if (this.enemyState == EnemyState.WalkingLeft)
                {
                    this.Position -= this.Velocity;

                    if (currentFrame >= this.walkingLeftLastFrame || currentFrame<this.walkingLeftInitialFrame)
                    {
                        currentFrame = walkingLeftInitialFrame;
                    }
                }
                else
                {
                    this.Position += this.Velocity;

                    if (currentFrame > this.walkingRightLastFrame)
                    {
                        currentFrame = walkingRightInitialFrame;
                    }
                }

                currentFrame++;
                timeSinceLastFrame -= millisecondPerFrame;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int row = (int)((float)this.currentFrame / this.cols);
            int column = this.currentFrame % this.cols;

            Rectangle sourceRectangle = new Rectangle(this.Width * column, this.Height * row, this.Width, this.Height);

            this.Bounds = new Rectangle((int)this.Position.X + (int)boundOffset.X,
                                            (int)this.Position.Y + (int)boundOffset.Y,
                                            this.Width - 2 * (int)boundOffset.X,
                                            this.Height - 2 * (int)boundOffset.Y);

            Rectangle target = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);

            spriteBatch.Draw(this.Image, this.Bounds, sourceRectangle, Color.White);
        }

        private void SetFrames(EnemyState state)
        {
            if (state == EnemyState.WalkingRight)
            {
                if (currentFrame < this.walkingRightInitialFrame || currentFrame >= this.walkingRightLastFrame)
                {
                    currentFrame = this.walkingRightInitialFrame;
                }
            }
            else if (state == EnemyState.WalkingLeft)
            {
                if (currentFrame <= this.walkingLeftInitialFrame || currentFrame > this.walkingLeftLastFrame)
                {
                    currentFrame = this.walkingLeftInitialFrame;
                }
            }
        }


    }
}
