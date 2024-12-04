using System;
using System.Threading;

class AlarmClock
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Alarm Clock ===");

        // Ask the user for the alarm time
        Console.Write("Set the alarm (HH:mm:ss): ");
        string alarmTimeInput = Console.ReadLine();

        // Validate and parse the input time
        if (!TimeSpan.TryParse(alarmTimeInput, out TimeSpan alarmTime))
        {
            Console.WriteLine("Invalid time format. Please use HH:mm:ss.");
            return;
        }

        Console.WriteLine($"Alarm set for {alarmTime}. Waiting...");

        while (true)
        {
            // Get the current time
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            // Check if the current time matches the alarm time
            if (currentTime >= alarmTime)
            {
                Console.WriteLine("Alarm ringing!");
                PlayAlarmSound();
                break;
            }

            // Sleep for 1 second to reduce CPU usage
            Thread.Sleep(1000);
        }
    }

    static void PlayAlarmSound()
    {
        // Play a series of beeps
        for (int i = 0; i < 5; i++)
        {
            Console.Beep(1000, 500); // Frequency: 1000 Hz, Duration: 500 ms
            Thread.Sleep(500);       // Pause for 500 ms
        }
    }
}
