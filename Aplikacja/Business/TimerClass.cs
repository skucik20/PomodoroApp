using System.Timers;


namespace Business;

public class TimerClass
{
    private System.Timers.Timer _timer; // Obiekt timer
    private int _secondsRemaining; // Liczba sekund do odliczenia

    // Konstruktor klasy
    public TimerClass(int seconds)
    {
        if (seconds <= 0)
        {
            throw new ArgumentException("Liczba sekund musi być większa od zera.");
        }

        _secondsRemaining = seconds;
    }

    // Rozpoczęcie odliczania
    public void Start()
    {
        _timer = new System.Timers.Timer(1000); // Interwał timera wynosi 1000 ms (1 sekunda)
        _timer.Elapsed += OnTimedEvent; // Subskrybujemy zdarzenie Elapsed
        _timer.AutoReset = true; // Timer będzie automatycznie resetować się po każdym zdarzeniu
        _timer.Enabled = true; // Włączamy timer

        Console.WriteLine("Odliczanie rozpoczęte...");
    }

    // Zdarzenie wykonywane co sekundę
    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        if (_secondsRemaining > 0)
        {
            Console.WriteLine($"Pozostało: {_secondsRemaining} sekund");
            _secondsRemaining--;
        }
        else
        {
            Console.WriteLine("Odliczanie zakończone!");
            Stop(); // Zatrzymujemy timer
        }
    }

    // Zatrzymanie odliczania i zwolnienie zasobów
    public void Stop()
    {
        if (_timer != null)
        {
            _timer.Stop(); // Zatrzymujemy timer
            _timer.Dispose(); // Zwolnienie zasobów
            _timer = null;
        }
    }
}
