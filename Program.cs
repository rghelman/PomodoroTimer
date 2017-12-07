/// <summary>
/// Console application to implement a Pomodoro Timer https://cirillocompany.de/pages/pomodoro-technique
/// 
/// Design decisions
/// 1.	The timer could be started at any time or only when there is at least one task in the to do list.
///     Choosing the later as I believe the user shouldn't use the timer with no assigned tasks
/// 2.	Completing a task could immediately give you a break or noto.
///     Choosing the immediate break as the user should feel good after completing a task
/// 3.	Tasks can be added while the timer is running? I assumed, yes.
/// 4.	Stopping the timer if there are no pending tasks in the to do list
/// 5.	The timer may start automatically after the maximum break timeout and it can be restarted after the minimum break timeout.
///     The timer can't be started before the minimum break timeout to ensure the user rests enough
/// 6.  One or more items can be completed during the break. That is to allow a user that managed to complete 2 or more items during
///     one pomodoro timer
/// </summary>
namespace PomodoroTimer
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        private static PomodoroTimer pomodoroTimer = new PomodoroTimer();
        private static List<string> todoList = new List<string>();
        static void Main(string[] args)
        {
            RefreshScreen();
            HandleMenuOptions();
        }

        /// <summary>
        /// Waits for a valid menu option input
        /// </summary>
        private static void HandleMenuOptions()
        {
            for (;;)
            {
                var key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case 'a':
                    case 'A':
                        AddItem(todoList);
                        break;
                    case 'c':
                    case 'C':
                        CompleteItem(todoList);
                        break;
                    case 's':
                    case 'S':
                        StartTimer(todoList);
                        break;
                    case 't':
                    case 'T':
                        ToggleAutoRestart();
                        break;
                    case 'e':
                    case 'E':
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Refreshes the whole screen
        /// </summary>
        private static void RefreshScreen()
        {
            ConsoleUtils.showConsoleUpdates = false;
            Console.Clear();
            Console.CursorVisible = false;
            WriteMenu();

            WriteToDoList(todoList);
            ConsoleUtils.showConsoleUpdates = true;
        }

        /// <summary>
        /// Prints all the menu items
        /// </summary>
        /// <returns>The number of lines used by the menu</returns>
        private static int WriteMenu()
        {
            Console.WriteLine(Constants.ADD_ITEM);
            Console.WriteLine(Constants.COMPLETE_ITEM);
            Console.WriteLine(Constants.START_TIMER);
            if (pomodoroTimer.AutoRestartWhenBreakEnds)
            {
                Console.WriteLine(Constants.TOGGLE_AUTO_RESTART_UPON_BREAK_END_ON);
            }
            else
            {
                Console.WriteLine(Constants.TOGGLE_AUTO_RESTART_UPON_BREAK_END_OFF);
            }

            Console.WriteLine(Constants.EXIT);
            Console.WriteLine();
            Console.Write(Constants.CHOOSE_MENU_ITEM);
            return 7;
        }

        /// <summary>
        /// Writes the to do list to the screen avoiding scrolling
        /// </summary>
        /// <param name="list">To do list</param>
        private static void WriteToDoList(IList<string> list)
        {
            // Need to capture the current cursor position to move it back in the end
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            // +6 is to leave space for the add item question and a very long to do item.
            Console.SetCursorPosition(0, Constants.MENU_SIZE + 6);
            Console.WriteLine("To do list");
            Console.WriteLine("==========");
            if (todoList.Count == 0)
            {
                Console.WriteLine("The list is empty.");
            }
 
            int itemsWritten = 0;
            foreach (var item in list)
            {
                try
                {
                    string itemTrimmed = item;
                    if (itemTrimmed.Length > Console.WindowWidth)
                    {
                        itemTrimmed = itemTrimmed.Substring(0, Console.WindowWidth - 3) + "...";
                    }

                    // Limiting the number of items to be written to the console based on the console height and avoiding scrolling
                    if (itemsWritten < Console.WindowHeight - (Constants.MENU_SIZE + 8))
                    {
                        Console.WriteLine(itemTrimmed);
                        itemsWritten++;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to show to do list item. Error message: {0}", e.Message);
                    itemsWritten++;
                }
            }
            Console.CursorLeft = left;
            Console.CursorTop = top;
        }

        /// <summary>
        /// Adds an item to the to do list
        /// </summary>
        /// <param name="list">The list to add the item</param>
        private static void AddItem(IList<string> list)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = Constants.MENU_SIZE + 1;
            Console.WriteLine("Type your new to do item and press enter:");

            // Unfortunately if we move the cursor while the user is typing we might have the user type in the wrong place.
            // Disabling timer updates to avoid cursor movement
            ConsoleUtils.showConsoleUpdates = false;
            Console.CursorVisible = true;
            string item = Console.ReadLine();
            Console.CursorVisible = false;
            ConsoleUtils.showConsoleUpdates = true;
            list.Add(item);
            RefreshScreen();
        }

        /// <summary>
        /// Completes the first item from the to do list. If the timer was started and a round is ongoing, finishes the current round
        /// </summary>
        /// <param name="list">The list to complete the item</param>
        private static void CompleteItem(IList<string> list)
        {
            if (list.Count > 0)
            {
                list.RemoveAt(0);
                pomodoroTimer.CompleteTask();
                if (list.Count == 0)
                {
                    pomodoroTimer.Stop();
                }
                RefreshScreen();
            }
            else
            {
                Console.Beep();
            }
        }

        /// <summary>
        /// Start the timer if there is at least one item in the list
        /// </summary>
        /// <param name="list">The list to complete the item</param>
        private static void StartTimer(IList<string> list)
        {
            if (list.Count > 0)
            {
                pomodoroTimer.Start();
            }
            else
            {
                Console.Beep();
            }
        }

        /// <summary>
        /// Toggles the auto restart upon break timer completion
        /// </summary>
        private static void ToggleAutoRestart()
        {
            pomodoroTimer.AutoRestartWhenBreakEnds = !pomodoroTimer.AutoRestartWhenBreakEnds;
            RefreshScreen();
        }
    }
}
