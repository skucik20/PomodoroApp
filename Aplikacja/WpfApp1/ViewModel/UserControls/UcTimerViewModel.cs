using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model.UserControls;
using WpfApp1.ViewModel.Commands;

namespace WpfApp1.ViewModel.UserControls;

public class UcTimerViewModel
{

    private CancellationTokenSource _cancellationTokenSource;

    private static readonly object _lockObject = new object();
    private static bool _isRunning = false;
    private static bool _isPaused = false;
    public int _minuteOnes { get; set; }
    public int _minuteTens { get; set; }

    private readonly ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);

    public UcTimerModel ucTimerModel { get; set; }
    public ICommand CountdownClick { get; set; }
    public ICommand CountdownCancelClick { get; set; }
    public ICommand PauseClick { get; set; }

    public UcTimerViewModel() 
    {
        CountdownClick = new RelayCommand(x => CountdownClicked(""));
        CountdownCancelClick = new RelayCommand(x => CountdownCancelClicked(""));
        PauseClick = new RelayCommand(x => PauseClicked(""));
        ucTimerModel = new UcTimerModel();
        
    }
    public UcTimerViewModel(int minuteOnes, int minuteTens) 
    {
        CountdownClick = new RelayCommand(x => CountdownClicked(""));
        CountdownCancelClick = new RelayCommand(x => CountdownCancelClicked(""));
        PauseClick = new RelayCommand(x => PauseClicked(""));
        ucTimerModel = new UcTimerModel();
        _minuteOnes = minuteOnes;
        _minuteTens = minuteTens;

        ucTimerModel.MinuteOnes = minuteOnes;
        ucTimerModel.MinuteTens = minuteTens;
        // _eventAggregator = eventAggregator;
    }

    private void CountdownClicked(object parameter)
    {
        ucTimerModel.IsSStartVisible = false;
        if(_isPaused == false)
        {
            TiemCountdown(_minuteTens, _minuteOnes);
        }
        else if(_isPaused == true)
        {
            Resume();
        }

    }
    
    private void CountdownCancelClicked(object parameter)
    {

        ucTimerModel.IsSStartVisible = true;

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
        ucTimerModel.IsSStartVisible = true;
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

        ucTimerModel.SecondOnes = 0;
        ucTimerModel.SecondTens = 0;

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


                while (ucTimerModel.MinuteTens >= 0)
                {
                    while (ucTimerModel.MinuteOnes >= 0)
                    {
                        while (ucTimerModel.SecondTens >= 0)
                        {
                            while (ucTimerModel.SecondOnes >= 0)
                            {
                                //TODO Bardzo słaby mechanizme, powinienem do waita też dać cancelation tocken i guess
                                if (token.IsCancellationRequested)
                                {
                                    ucTimerModel.SetValuesToZero();
                                    ucTimerModel.MinuteTens = _minuteTens;
                                    ucTimerModel.MinuteOnes = _minuteOnes;
                                    return;
                                }
                                _pauseEvent.Wait();
                                Thread.Sleep(1000);
                                _pauseEvent.Wait();
                                if (token.IsCancellationRequested)
                                {
                                    ucTimerModel.SetValuesToZero();
                                    ucTimerModel.MinuteTens = _minuteTens;
                                    ucTimerModel.MinuteOnes = _minuteOnes;
                                    return;
                                }

                                ucTimerModel.SecondOnes--;
                                
                            }
                            ucTimerModel.SecondOnes = 9;
                            ucTimerModel.SecondTens--;
                        }
                        ucTimerModel.SecondTens = 5;
                        ucTimerModel.MinuteOnes--;
                    }
                    ucTimerModel.MinuteOnes = 9;
                    ucTimerModel.MinuteTens--;
                }
                ucTimerModel.SetValuesToZero();
                ucTimerModel.MinuteTens = _minuteTens;
                ucTimerModel.MinuteOnes = _minuteOnes;

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
