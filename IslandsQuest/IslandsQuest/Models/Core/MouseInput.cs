﻿namespace IslandsQuest.Models.Core
{
    using Microsoft.Xna.Framework.Input;

    public static class MouseInput
    {
        private static MouseState mouseState;
        private static MouseState lastMouseState;


        public static MouseState MouseState
        {
            get { return mouseState; }
            set { mouseState = value; }
        }

        public static MouseState LastMouseState
        {
            get
            {
                return lastMouseState;
            }
            set
            {
                lastMouseState = value;
            }
        }

        
        public static int getMouseX()
        {
            return Mouse.GetState().X;
        }

        public static int getMouseY()
        {
            return Mouse.GetState().Y;
        }
    }
}
