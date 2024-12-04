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

    private void CountdownClicked(object parameter)
    {
        if(_isPaused == false)
        {
            
            int minuteOnes = 5;
            int minuteTens = 0;
            int sec = (minuteTens * 600) + (minuteOnes * 60);
            ucTimerModel.BeginOpacityZero = TimeSpan.FromSeconds(sec);
            TiemCountdown(minuteTens, minuteOnes);

            

            

        }
        else if(_isPaused == true)
        {
            Resume();
        }

    }
    
    private void CountdownCancelClicked(object parameter)
    {

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
        ucTimerModel.MinuteOnes = minuteOnes;
        ucTimerModel.MinuteTens = minuteTens;

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
                                if (token.IsCancellationRequested)
                                {
                                    ucTimerModel.SetValuesToZero();
                                    return;
                                }
                                Thread.Sleep(1000);
                                ucTimerModel.SecondOnes--;
                                _pauseEvent.Wait();
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
