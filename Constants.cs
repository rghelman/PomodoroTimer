namespace PomodoroTimer
{
    class Constants
    {
        internal const string START_TIMER = "S - To start the timer (to do list can't be empty)";
        internal const string ADD_ITEM = "A - Add an item to the do list";
        internal const string COMPLETE_ITEM = "C - Complete the current item in the to do list";
        internal const string TOGGLE_AUTO_RESTART_UPON_BREAK_END = "T - Toggle auto restart when break ends";
        internal const string TOGGLE_AUTO_RESTART_UPON_BREAK_END_ON = TOGGLE_AUTO_RESTART_UPON_BREAK_END + " (ON)";
        internal const string TOGGLE_AUTO_RESTART_UPON_BREAK_END_OFF = TOGGLE_AUTO_RESTART_UPON_BREAK_END + " (OFF)";
        internal const string EXIT = "E - Exit";
        internal const string WAIT_TO_RESTART = "Wait at least {0}s to restart the timer";
        internal const int MENU_SIZE = 7;

        internal const string THIRTY_SPACES = "                              ";
        internal const string TIME_IS_UP = "Time is up!";
        internal const string BREAK_TIME_IS_UP = "Break time is up!";
        internal const string CHOOSE_MENU_ITEM = "Please, choose one of the options above: ";
        internal const string ROUND_DETAILS = @"{0}Round {1} - {2:mm\:ss}";
        internal const string BREAK_DETAILS = @"{0}Break - {1:mm\:ss}";

        internal const int ROUND_TIME_IN_SECONDS = 25 * 60;
        internal const int MAX_BREAK_TIME_BETWEEN_ROUNDS_IN_SECONDS = 5 * 60;
        internal const int MIN_BREAK_TIME_BETWEEN_ROUNDS_IN_SECONDS = 3 * 60;
        internal const int NUMBER_OF_ROUNDS_IN_A_SESSION = 4;
        internal const int MAX_BREAK_TIME_BETWEEN_ROUND_SESSIONS_IN_SECONDS = 30 * 60;
        internal const int MIN_BREAK_TIME_BETWEEN_ROUND_SESSIONS_IN_SECONDS = 15 * 60;
    }
}
