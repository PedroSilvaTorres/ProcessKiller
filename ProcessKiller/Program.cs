using System;
using System.Diagnostics;
using System.Timers;

namespace ProcessKiller
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program killer = new Program();
            killer.Start(args);
        }

        public void Start(string[] args)
        {
            //The process to check
            String processName = args.Length <= 0 ? "notepad++" : args[0];
            double maxLifetime;
            double timeInterval;
            try
            {
                //Maximum Lifetime of the process in minutes
                maxLifetime = args.Length <= 1 ? 1 : double.Parse(args[1]);
                maxLifetime = MakeArgumentValid(maxLifetime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                maxLifetime = 1;
            }

            try
            {
                //Check the process every X minutes
                timeInterval = args.Length <= 2 ? 1 : double.Parse(args[2]);
                timeInterval = MakeArgumentValid(timeInterval);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                timeInterval = 1;
            }

            Console.WriteLine("Terminating process: \'" + processName + "\' if older than {0} minute/s, checking every {1} minute/s", maxLifetime, timeInterval);

            //Setup my timer
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += (sender, e) => OnTimerTrigger(processName, maxLifetime);

            //Timer interval in milliseconds
            timer.Interval = timeInterval * 60 * 1000;
            timer.Enabled = true;

            Console.WriteLine("Press \'q\' to stop");
            while (Console.Read() != 'q') ;
        }

        public double MakeArgumentValid(double argument)
        {
            if (argument <= 0)
            {
                return 1;
            }
            return argument;
        }

        private void OnTimerTrigger(string processName, double maxLifetime)
        {

            //List of all the processes
            var processList = Process.GetProcesses();

            foreach (Process currentProcess in processList)
            {

                //Check the list for the correct process
                if (IsSameProcess(processName, currentProcess.ProcessName))
                {

                    //Check if process surpassed the lifetime limit
                    if (IsExpired(maxLifetime, currentProcess.StartTime))
                    {

                        Console.WriteLine("Terminating Process: {0} with ID: {1}", currentProcess.ProcessName, currentProcess.Id);

                        //Terminate the process
                        currentProcess.Kill();

                        return;
                    }
                }

            }
            Console.WriteLine("No process to terminate");
            return;
        }
        /*Receives the name of the process that we are looking for and the name of the current
         process on our list, and checks if they are equal*/
        public bool IsSameProcess(string processName, string currentProcess)
        {
            return currentProcess.Equals(processName);
        }

        /*Receives the maximum lifetime of the process and the time when the process started,
         and checks if the current time is over the expiration time*/
        public bool IsExpired(double maxLifetime, DateTime startTime)
        {
            return startTime.AddMinutes(maxLifetime) <= DateTime.Now;
        }
    }
}
