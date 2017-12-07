namespace PomodoroTimer
{
    using System;
    using System.Timers;
    using System.Threading;
    class PomodoroTimer
    {
        private DateTime roundTimerEnds;
        private DateTime breakTimerEnds;
        private System.Timers.Timer roundTimer = new System.Timers.Timer();
        private System.Timers.Timer breakTimer = new System.Timers.Timer();
        private int round = 0;
        public bool AutoRestartWhenBreakEnds = true;

        public void CompleteTask()
        {
            if (roundTimer.Enabled)
            {
                roundTimerEnds = DateTime.Now;
            }
        }

        /// <summary>
        /// Returns if the Pomodoro timer can be restarted and if no in how many seconds can it be restarted
        /// </summary>
        /// <param name="secondsToWait"></param>
        /// <returns>True if the break can be stopped (the timer can be restarted)</returns>
        private bool CanStopBreakTimer(out int secondsToWait)
        {
            DateTime now = DateTime.Now;
            secondsToWait = 0;
            DateTime minimumBreak;
            if (round % Constants.NUMBER_OF_ROUNDS_IN_A_SESSION == 0)
            {
                minimumBreak = breakTimerEnds.AddSeconds(Constants.MIN_BREAK_TIME_BETWEEN_ROUND_SESSIONS_IN_SECONDS - Constants.MAX_BREAK_TIME_BETWEEN_ROUND_SESSIONS_IN_SECONDS);
            }
            else
            {
                minimumBreak = breakTimerEnds.AddSeconds(Constants.MIN_BREAK_TIME_BETWEEN_ROUNDS_IN_SECONDS - Constants.MAX_BREAK_TIME_BETWEEN_ROUNDS_IN_SECONDS);
            }
            if (now > minimumBreak)
            {
                return true;
            }

            secondsToWait = (int)minimumBreak.Subtract(now).TotalSeconds;
            return false;
        }

        /// <summary>
        /// Starts the Pomodoro timer
        /// </summary>
        public void Start()
        {
            // If the timer is already started, return
            if (roundTimer.Enabled)
            {
                Console.Beep();
                return;
            }
            // If we are in a break between timers, check if the timer can be restarted
            if (breakTimer.Enabled)
            {
                int secondsToWait;
                if (!CanStopBreakTimer(out secondsToWait))
                {
                    breakTimer.Enabled = false;
                    ConsoleUtils.WriteAtTopRightCorner(string.Format(Constants.WAIT_TO_RESTART, secondsToWait));
                    Console.Beep();
                    // Waiting 1 second to give the user an opportunity to read the message
                    Thread.Sleep(1000);
                    breakTimer.Enabled = true;
                    return;
                }
            }
            round++;
            breakTimer.AutoReset = false;
            breakTimer.Enabled = false;
            roundTimerEnds = DateTime.Now.AddSeconds(Constants.ROUND_TIME_IN_SECONDS);
            roundTimer.Elapsed += RoundTimerCountDownEvent;
            roundTimer.AutoReset = true;
            roundTimer.Enabled = true;
        }

        /// <summary>
        /// Stops the Pomodoro timer
        /// </summary>
        public void Stop()
        {
            breakTimer.AutoReset = false;
            breakTimer.Enabled = false;
            roundTimer.AutoReset = false;
            roundTimer.Enabled = false;
            roundTimerEnds = DateTime.Now;
            breakTimerEnds = DateTime.Now;
        }

        /// <summary>
        /// Starts the break timer
        /// </summary>
        private void StartBreakTimer()
        {
            roundTimer.AutoReset = false;
            roundTimer.Enabled = false;
            if (round % Constants.NUMBER_OF_ROUNDS_IN_A_SESSION == 0)
            {
                breakTimerEnds = DateTime.Now.AddSeconds(Constants.MAX_BREAK_TIME_BETWEEN_ROUND_SESSIONS_IN_SECONDS);
            }
            else
            {
                breakTimerEnds = DateTime.Now.AddSeconds(Constants.MAX_BREAK_TIME_BETWEEN_ROUNDS_IN_SECONDS);
            }
            breakTimer.Elapsed += BreakTimerCountEvent;
            breakTimer.AutoReset = true;
            breakTimer.Enabled = true;
        }

        /// <summary>
        /// Counts down the number of minutes/seconds remaining for this round to end.
        /// </summary>
        private void RoundTimerCountDownEvent(Object source, ElapsedEventArgs e)
        {
            if (!roundTimer.Enabled)
            {
                return;
            }

            DateTime now = DateTime.Now;
            TimeSpan elapsedTime = roundTimerEnds.Subtract(now);
            if (elapsedTime < new TimeSpan(0, 0, 1))
            {
                ConsoleUtils.WriteAtTopRightCorner(Constants.THIRTY_SPACES + Constants.TIME_IS_UP);
                Console.Beep();
                breakTimer.AutoReset = false;
                breakTimer.Enabled = false;
                StartBreakTimer();
            }
            else
            {
                ConsoleUtils.WriteAtTopRightCorner(string.Format(Constants.ROUND_DETAILS, Constants.THIRTY_SPACES, round, elapsedTime));
            }
        }

        /// <summary>
        /// Counts the number of minutes/seconds elapsed since the break started
        /// </summary>
        private void BreakTimerCountEvent(Object source, ElapsedEventArgs e)
        {
            if (!breakTimer.Enabled)
            {
                return;
            }

            DateTime now = DateTime.Now;
            if (now > breakTimerEnds)
            {
                breakTimer.AutoReset = false;
                breakTimer.Enabled = false;
                ConsoleUtils.WriteAtTopRightCorner(Constants.THIRTY_SPACES + Constants.BREAK_TIME_IS_UP);
                Console.Beep();
                if (AutoRestartWhenBreakEnds)
                {
                    Start();
                }
                return;
            }

            TimeSpan elapsedTime;
            if (round % Constants.NUMBER_OF_ROUNDS_IN_A_SESSION == 0)
            {
                elapsedTime = now.Subtract(breakTimerEnds.AddSeconds(Constants.MAX_BREAK_TIME_BETWEEN_ROUND_SESSIONS_IN_SECONDS * -1));
            }
            else
            {
                elapsedTime = now.Subtract(breakTimerEnds.AddSeconds(Constants.MAX_BREAK_TIME_BETWEEN_ROUNDS_IN_SECONDS * -1));
            }
            ConsoleUtils.WriteAtTopRightCorner(string.Format(Constants.BREAK_DETAILS, Constants.THIRTY_SPACES, elapsedTime));
        }
    }
}
