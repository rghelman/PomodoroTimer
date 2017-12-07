namespace PomodoroTimer
{
    using System;
    internal class ConsoleUtils
    {
        public static bool showConsoleUpdates = true;

        /// <summary>
        /// Writes a message at the top right corner of the screen
        /// </summary>
        /// <param name="s">The string to be written</param>
        public static void WriteAtTopRightCorner(string s)
        {
            try
            {
                if (!showConsoleUpdates)
                {
                    return;
                }

                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                Console.SetCursorPosition(Console.WindowWidth - (s.Length + 1), 0);
                Console.Write(s);
                Console.CursorLeft = left;
                Console.CursorTop = top;
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
    }
}
