using System;
using System.Threading;

public class Stopwatch
{
    public delegate void StopwatchEventHandler(string message);

    public event StopwatchEventHandler OnStarted;
    public event StopwatchEventHandler OnStopped;
    public event StopwatchEventHandler OnReset;

    private TimeSpan timeElapsed;
    private bool isRunning;
    private Thread tickingThread;
    private DateTime startTime;
    private bool wasReset;

    public Stopwatch()
    {
        timeElapsed = TimeSpan.Zero;
        isRunning = false;
        wasReset = false;
    }
   public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            if (timeElapsed != TimeSpan.Zero && !wasReset)
            {
                startTime = DateTime.Now - timeElapsed;
            }
            else
            {
                startTime = DateTime.Now;
            }
            tickingThread = new Thread(Tick);
            tickingThread.Start();
            OnStarted?.Invoke("Stopwatch Started!");
            wasReset = false;
        }
    }
         public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            tickingThread.Join();
            timeElapsed = DateTime.Now - startTime;
            OnStopped?.Invoke("Stopwatch Stopped!");
        }
    }
    public void Reset()
    {
        timeElapsed = TimeSpan.Zero;
        wasReset = true;
        if (isRunning)
        {
            Stop();
        }
        OnReset?.Invoke("Stopwatch Reset!");
    }
    private void Tick()
    {
        while (isRunning)
        {
            Thread.Sleep(1000);
            timeElapsed = DateTime.Now - startTime;
            Console.Clear();
            Console.WriteLine("Time: " + timeElapsed.ToString(@"hh\:mm\:ss"));
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.OnStarted += (message) => Console.WriteLine(message);
        stopwatch.OnStopped += (message) => Console.WriteLine(message);
        stopwatch.OnReset += (message) => Console.WriteLine(message);

        bool running = true;
        while (running)
        {
            Console.WriteLine("\nPress S to start, T to stop, R to reset, or Q to quit:");

            char input = Console.ReadKey(true).KeyChar;
            Console.WriteLine($"You pressed: {input}");

            switch (input)
            {
                case 'S':
                case 's':
                    stopwatch.Start();
                    break;
                case 'T':
                case 't':
                    stopwatch.Stop();
                    break;
                case 'R':
                case 'r':
                    stopwatch.Reset();
                    break;
                case 'Q':
                case 'q':
                    stopwatch.Stop();
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid input, try again.");
                    break;
            }
        }
    }
}

