﻿using IslandsQuest.Exceptions;
using System;
using System.Windows.Forms;

namespace IslandsQuest
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new IslandsQuest())
                {
                    game.Run();
                }
            }
            catch (ResourceNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
#endif
}
