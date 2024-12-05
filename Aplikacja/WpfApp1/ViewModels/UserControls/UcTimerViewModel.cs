using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WpfApp1.Model.UserControls;
using WpfApp1.ViewModel.Commands;

namespace WpfApp1.ViewModels.UserControls;

public class UcTimerViewModel : Screen
{


    private int _secondOnes = 0;
    public int SecondOnes
    {
        get { return _secondOnes; }
        set { _secondOnes = value; NotifyOfPropertyChange(nameof(SecondOnes)); }
    }

    private int _secondTens = 0;
    public int SecondTens
    {
        get { return _secondTens; }
        set { _secondTens = value; NotifyOfPropertyChange(nameof(SecondTens)); }
    }
    private int _minuteOnes = 0;
    public int MinuteOnes
    {
        get { return _minuteOnes; }
        set { _minuteOnes = value; NotifyOfPropertyChange(nameof(MinuteOnes)); }
    }

    private int _minuteTens = 0;
    public int MinuteTens
    {
        get { return _minuteTens; }
        set { _minuteTens = value; NotifyOfPropertyChange(nameof(MinuteTens)); }
    }

    private Duration _duration = new Duration(TimeSpan.FromSeconds(60));
    public Duration Durationa
    {
        get { return _duration; }
        set { _duration = value; NotifyOfPropertyChange(nameof(Durationa)); }
    }
    private TimeSpan _beginOpacityZero = TimeSpan.FromSeconds(60);
    public TimeSpan BeginOpacityZero
    {
        get { return _beginOpacityZero; }
        set { _beginOpacityZero = value; NotifyOfPropertyChange(nameof(BeginOpacityZero)); Durationa = new Duration(BeginOpacityZero); }
    }
    public void SetValuesToZero()
    {
        SecondOnes = 0; MinuteOnes = 0; SecondTens = 0; MinuteTens = 0;
    }

    private bool _isStartVisible = true;
    public bool IsSStartVisible
    {
        get { return _isStartVisible; }
        set { _isStartVisible = value; NotifyOfPropertyChange(nameof(IsSStartVisible)); }
    }

    private CancellationTokenSource _cancellationTokenSource;

    private static readonly object _lockObject = new object();
    private static bool _isRunning = false;
    private static bool _isPaused = false;

    private readonly ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);


    public ICommand CountdownClick { get; set; }
    public ICommand CountdownCancelClick { get; set; }
    public ICommand PauseClick { get; set; }
    public UcTimerViewModel()
    {
        _cancellationTokenSource = new CancellationTokenSource(); // Tworzymy token przerwania
        CountdownClick = new RelayCommand(x => CountdownClicked(""));
        CountdownCancelClick = new RelayCommand(x => CountdownCancelClicked(""));
        PauseClick = new RelayCommand(x => PauseClicked(""));

        MinuteOnes = 5;


    }
    //public UcTimerViewModel(int minuteOnes, int minuteTens)
    //{
    //    _cancellationTokenSource = new CancellationTokenSource(); // Tworzymy token przerwania
    //    CountdownClick = new RelayCommand(x => CountdownClicked(""));
    //    CountdownCancelClick = new RelayCommand(x => CountdownCancelClicked(""));
    //    PauseClick = new RelayCommand(x => PauseClicked(""));
    //    ucTimerModel = new UcTimerModel();
    //    // _eventAggregator = eventAggregator;
    //}

    private void CountdownClicked(object parameter)
    {
        IsSStartVisible = false;
        if (_isPaused == false)
        {
            TiemCountdown(_minuteTens, _minuteOnes);
        }
        else if (_isPaused == true)
        {
            Resume();
        }

    }

    private void CountdownCancelClicked(object parameter)
    {

        IsSStartVisible = true;

        if (_isPaused == false)
        {
            Cancel();
        }
        else if (_isPaused == true)
        {
            Resume();
            Cancel();
        }

    }
    private void PauseClicked(object parameter)
    {
        IsSStartVisible = true;
        Pause();
    }

    public void Cancel()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel(); // Anulujemy token
        }
    }

    public void Pause()
    {
        _pauseEvent.Reset(); // Ustawia stan na "zatrzymany"
        _isPaused = true;
    }

    public void Resume()
    {
        _pauseEvent.Set(); // Ustawia stan na "gotowy do działania"
        _isPaused = false;

    }

    private async void TiemCountdown(int minuteTens, int minuteOnes)
    {

        SecondOnes = 0;
        SecondTens = 0;

        lock (_lockObject)
        {
            if (_isRunning)
            {
                Console.WriteLine("Task już działa. Nie można go uruchomić ponownie.");
                return;
            }
            _isRunning = true;
        }

        _cancellationTokenSource = new CancellationTokenSource(); // Tworzymy token przerwania
        var token = _cancellationTokenSource.Token;

        try
        {
            await Task.Run(() =>
            {


                while (MinuteTens >= 0)
                {
                    while (MinuteOnes >= 0)
                    {
                        while (SecondTens >= 0)
                        {
                            while (SecondOnes >= 0)
                            {
                                //TODO Bardzo słaby mechanizme, powinienem do waita też dać cancelation tocken i guess
                                if (token.IsCancellationRequested)
                                {
                                    SetValuesToZero();
                                    MinuteTens = _minuteTens;
                                    MinuteOnes = _minuteOnes;
                                    return;
                                }
                                _pauseEvent.Wait();
                                Thread.Sleep(1000);
                                _pauseEvent.Wait();
                                if (token.IsCancellationRequested)
                                {
                                    SetValuesToZero();
                                    MinuteTens = _minuteTens;
                                    MinuteOnes = _minuteOnes;
                                    return;
                                }

                                SecondOnes--;

                            }
                            SecondOnes = 9;
                            SecondTens--;
                        }
                        SecondTens = 5;
                        MinuteOnes--;
                    }
                    MinuteOnes = 9;
                    MinuteTens--;
                }
                SetValuesToZero();
                MinuteTens = _minuteTens;
                MinuteOnes = _minuteOnes;

                // _eventAggregator.GetEvent<CountdownEndEvent>().Publish("Countdown end!");
                Console.Beep(3000, 200);



            }, token);
        }
        catch (OperationCanceledException)
        {
            MessageBox.Show("anulowany");
        }
        finally
        {
            lock (_lockObject)
            {
                _isRunning = false;
            }
        }
    }
}
