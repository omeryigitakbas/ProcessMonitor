using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessMonitor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var processName = args[0];
            var lifetime = int.Parse(args[1]);
            var frequency = int.Parse(args[2]);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Interval = frequency * 60 * 1000;
            timer.Elapsed += (sender, e) => { MonitorProcesses(processName, lifetime); };
            timer.Start();

            Console.ReadLine();
        }

        public static void MonitorProcesses(string processName, int lifetime)
        {
            var processesToKill = Process.GetProcesses().Where(p => p.ProcessName.ToLower().Equals(Path.GetFileNameWithoutExtension(processName)));
            if (processesToKill.ToList().Any())
            {
                var processToKill = processesToKill.FirstOrDefault();
                if ((DateTime.Now - processToKill.StartTime).TotalMinutes > lifetime)
                {
                    processToKill.Kill();
                    Console.WriteLine($"Process with name {processName} was killed!");
                }
            }
        }


        // public async Task Monitor(CancellationToken token = default(CancellationToken))
        // {
        //     while (!token.IsCancellationRequested)
        //     {
        //         DoMyMethod();
        //         try
        //         {
        //             await Task.Delay(TimeSpan.FromMinutes(1), token);
        //         }
        //         catch (TaskCanceledException)
        //         {
        //             break;
        //         }
        //     }
        // }
    }
}
