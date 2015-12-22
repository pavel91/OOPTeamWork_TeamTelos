using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonsterQuest.Core.Factories;
using MonsterQuest.Enums;
using MonsterQuest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Entities.Characters
{
    public delegate void GameOverEventHandler(object sender, EventArgs e);

    public abstract class Character : Entity, ICharacter
    {
        private const int DefaultPlayerScore = 0;
        private const int millisecondPerFrame = 80;
        private int timeSinceLastFrame = 0;
        private int score;

        private int rows;
        private int cols;

        private int currentFrame;
        private int firstFrame;
        private int lastFrame;

        private Vector2 boundOffset;
        private CharacterState characterState;

        private int walkingLeftInitialFrame;
        private int walkingRightInitialFrame;
        private int walkingLeftLastFrame;
        private int walkingRightLastFrame;

        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;

        private bool jumping;
        private float jumpspeed = 0;
        private float startY;
        private int gold = 0;

        private Vector2 characterPosition;

        private ICollection<IBullet> bullets = new List<IBullet>();
        private BulletType currentBulletType;

        private IBulletFactory bulletFactory;
        private IData data;
        private Texture2D bulletImage;

        public event GameOverEventHandler PointChanged;

        protected Character(Texture2D image,
            int health, int damage, int rows, int cols,
            int walkingLeftInitialFrame,
            int walkingRightInitialFrame,
            int walkingLeftLastFrame,
            int walkingRightLastFrame,
            IBulletFactory weaponFactory,
            IData data)
            : base(image, health, damage)
        {
            this.Score = DefaultPlayerScore;
            this.rows = rows;
            this.cols = cols;
            //this.boundOffset = offset;

            this.walkingLeftInitialFrame = walkingLeftInitialFrame;
            this.walkingLeftLastFrame = walkingLeftLastFrame;
            this.walkingRightInitialFrame = walkingRightInitialFrame;
            this.walkingRightLastFrame = walkingRightLastFrame;
            this.startY = this.Position.Y;

            this.characterPosition = this.Position;

            this.bulletFactory = weaponFactory;
            this.data = data;

            this.Width = this.Image.Width / this.cols;
            this.Height = this.Image.Height / this.rows;


            //
            //this.CharacterPosition = DefaultElfPosition;
            //this.Velocity = DefaultElfVelocity;
            //this.BoundsOffset = DefaultElfOffset;
        }

        public Vector2 CharacterPosition
        {
            get { return this.characterPosition; }
            set { this.characterPosition = value; }
        }

        public int Gold { get; private set; }

        public ICollection<IBullet> Bullets
        {
            get { return this.bullets; }
        }

        public BulletType CurrentWeapon
        {
            get { return this.currentBulletType; }
        }

        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }

        public Vector2 BoundsOffset { get; protected set; }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > millisecondPerFrame)
            {
                //Moveing Logic
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    characterState = CharacterState.WalkingLeft;
                    SetFrames(characterState);
                    currentFrame++;

                    //new logic
                    if (currentFrame > this.walkingLeftLastFrame)
                    {
                        currentFrame = this.walkingLeftInitialFrame;
                    }
                    //new logic

                    this.CharacterPosition -= this.Velocity;
                    if (CharacterPosition.X < 0)
                    {
                        this.CharacterPosition += this.Velocity;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    characterState = CharacterState.WalkingRight;
                    SetFrames(characterState);
                    currentFrame++;

                    //new logic
                    if (currentFrame > this.walkingRightLastFrame)
                    {
                        currentFrame = this.walkingRightInitialFrame;
                    }
                    //new logic

                    this.CharacterPosition += this.Velocity;
                    if (this.characterPosition.X > 700)
                    {
                        this.characterPosition -= this.Velocity;
                    }
                }
                else if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left) && characterState == CharacterState.WalkingRight)
                {
                    characterState = CharacterState.StandingRight;
                }
                else if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left) && characterState == CharacterState.WalkingLeft)
                {
                    characterState = CharacterState.StandingLeft;
                }

                //Jumping logic
                if (jumping)
                {
                    startY = 330;
                    characterPosition.Y += jumpspeed;//Making it go up
                    jumpspeed += 5;
                    if (characterPosition.Y >= startY)
                    //If it's farther than ground
                    {
                        characterPosition.Y = startY;//Then set it on
                        jumping = false;
                    }
                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.Up))
                    {
                        jumping = true;
                        jumpspeed = -34;//Give it upward thrust
                    }
                }

                //Shooting logic
                if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                {
                    BulletDirection bulletDirection;
                    if (characterState == CharacterState.StandingLeft || characterState == CharacterState.WalkingLeft)
                    {
                        bulletDirection = BulletDirection.Left;
                    }
                    else
                    {
                        bulletDirection = BulletDirection.Right;
                    }

                    //foreach (var image in data.BulletImages)
                    //{
                    //    string imageName = image.Name;
                    //    if (image.Equals(this.currentBulletType.ToString()))
                    //    {
                    //        this.bulletImage = image;
                    //        break;
                    //    }
                    //}
                    this.bulletImage = this.data.BulletImages[0];
                    IBullet bullet = this.bulletFactory.CreateBullet(this.currentBulletType.ToString(), this.Position, this.bulletImage, bulletDirection);

                    this.AddNewBullet(bullet);
                }

                //TO DO : change jumpspeed and startY types.Worh trough the property Position of ENtity
                this.Position = this.characterPosition;
                previousKeyboardState = keyboardState;
                timeSinceLastFrame -= millisecondPerFrame;

                if (this.Health <= 0)
                {
                    OnPointChanged(EventArgs.Empty);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int row = (int)((float)this.currentFrame / this.cols);
            int column = this.currentFrame % this.cols;

            this.Bounds = new Rectangle((int)this.characterPosition.X + (int)boundOffset.X,
                                            (int)this.characterPosition.Y + (int)boundOffset.Y,
                                            this.Width - 2 * (int)boundOffset.X,
                                            this.Height - 2 * (int)boundOffset.Y);


            if (this.characterState == CharacterState.StandingLeft)
            {
                var sourceRectangle = new Rectangle(this.Width * 0, this.Height * 0, this.Width, this.Height);
                var destinationRectangle = new Rectangle((int)this.characterPosition.X, (int)this.characterPosition.Y, this.Width, this.Height);

                spriteBatch.Draw(this.Image, destinationRectangle, sourceRectangle, Color.White);
            }
            else if (this.characterState == CharacterState.StandingRight)
            {
                var sourceRectangle = new Rectangle(this.Width * 4, this.Height * 1, this.Width, this.Height);
                var destinationRectangle = new Rectangle((int)this.characterPosition.X, (int)this.characterPosition.Y, this.Width, this.Height);

                spriteBatch.Draw(this.Image, destinationRectangle, sourceRectangle, Color.White);
            }
            else
            {
                var sourceRectangle = new Rectangle(this.Width * column, this.Height * row, this.Width, this.Height);
                var destinationRectangle = new Rectangle((int)this.characterPosition.X, (int)this.characterPosition.Y, this.Width, this.Height);
                spriteBatch.Draw(this.Image, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        private void SetFrames(CharacterState state)
        {
            if (state == CharacterState.WalkingRight)
            {
                if (currentFrame <= this.walkingRightInitialFrame && currentFrame > this.walkingRightLastFrame)
                {
                    currentFrame = this.walkingRightInitialFrame;
                }
            }
            else if (state == CharacterState.WalkingLeft)
            {
                if (currentFrame <= this.walkingLeftInitialFrame && currentFrame > this.walkingLeftLastFrame)
                {
                    currentFrame = this.walkingLeftInitialFrame;
                }
            }
        }

        private void AddNewBullet(IBullet bullet)
        {
            this.bullets.Add(bullet);
        }


        public void CollectItem(IItem item)
        {
            var itemName = item.GetType().Name;

            switch (itemName)
            {
                case "Potion":
                    int health = (int)item.GetType().GetProperty("Health").GetValue(item, null);
                    this.Health += health;
                    break;
                case "Gold":
                    this.Gold++;
                    break;
                default:
                    throw new ArgumentException("Neshto se schupi v Character v CollectItem!!!");
            }
        }

        public void RemoveInactiveBullets()
        {
            var activeBullets = this.bullets.Where(b => b.IsActive == true).ToList();
            this.bullets = activeBullets;
        }

        public void IncrementScore(int score)
        {
            this.Score += score;
        }

        protected virtual void OnPointChanged(EventArgs e)
        {
            if (PointChanged != null)
                PointChanged(this, e);
        }

    }
}
