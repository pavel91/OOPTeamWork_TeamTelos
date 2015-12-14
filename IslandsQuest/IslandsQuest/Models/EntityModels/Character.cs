using IslandsQuest.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IslandsQuest.Models.EntityModels
{
    class Character
    {
        private const int DefaultPlayerHealth = 100;

        public Texture2D sprite;
        private Vector2 characterPosition;
        private KeyboardState keyboardState;
        private State characterState;
        public int health;

        private int currentFrame;
        private int firstFrame;
        private int lastFrame;
        public Rectangle Bounds { get; set; }

        private int timeSinceLastFrame = 0;
        private int millisecondPerFrame = 80;

        //set-vat se ruchno
        private int Columns = 6;
        private int Rows = 3;

        bool jumping;
        float startY, jumpspeed = 0;

        public Vector2 CharacterPosition { get { return this.characterPosition; } }

        public Character(Texture2D sprite, Vector2 location)
        {
            this.sprite = sprite;
            this.characterPosition = location;
            this.health = DefaultPlayerHealth;
        }

        public void Update(GameTime gameTime, Vector2 location)
        {
            keyboardState = Keyboard.GetState();
            characterPosition = location;

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > millisecondPerFrame)
            {
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    characterState = State.WalkingLeft;
                    SetFrames(characterState);
                    currentFrame++;
                    characterPosition.X -= 8;
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    characterState = State.WalkingRight;
                    SetFrames(characterState);
                    currentFrame++;
                    characterPosition.X += 8;
                }
                else if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left) && characterState == State.WalkingRight)
                {
                    characterState = State.StandingRight;
                    SetFrames(characterState);
                }
                else if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left) && characterState == State.WalkingLeft)
                {
                    characterState = State.StandingLeft;
                    SetFrames(characterState);
                }

                //Jumping logic
                if (jumping)
                {
                    startY = 260;
                    characterPosition.Y += jumpspeed;//Making it go up
                    jumpspeed += 3;//Some math (explained later)
                    if (characterPosition.Y >= startY)
                    //If it's farther than ground
                    {
                        characterPosition.Y = startY;//Then set it on
                        jumping = false;
                    }
                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        jumping = true;
                        jumpspeed = -24;//Give it upward thrust
                    }
                }

                timeSinceLastFrame -= millisecondPerFrame;

                if (currentFrame == lastFrame)
                {
                    currentFrame = firstFrame;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            var width = sprite.Width / Columns;
            var height = sprite.Height / Rows;
            int row = (int)((float)currentFrame / Columns);
            int column = currentFrame % Columns;

            var sourceRectangle = new Rectangle(width * column, height * row, width, height);
            //var sourceRectangle = new Rectangle((int)this.CharacterPosition.X + Bounds.X, (int)this.characterPosition.Y + Bounds.Y, Bounds.Width, Bounds.Height);
            //var destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
            this.Bounds = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(sprite, this.Bounds, sourceRectangle, Color.White);
        }

        private void SetFrames(State state)
        {
            if (state == State.WalkingLeft)
            {
                firstFrame = 6;
                lastFrame = 12;
                if (currentFrame <= 6 && currentFrame > 12)
                {
                    currentFrame = firstFrame;
                }
            }
            else if (state == State.WalkingRight)
            {
                firstFrame = 0;
                lastFrame = 6;
                if (currentFrame > 6)
                {
                    currentFrame = firstFrame;
                }
            }
            else if (state == State.StandingLeft)
            {
                firstFrame = 13;
                lastFrame = 15;
            }
            else if (state == State.StandingRight)
            {
                firstFrame = 12;
                lastFrame = 13;
            }
        }
    }
}
