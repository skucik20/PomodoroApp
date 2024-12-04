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

    public UcTimerModel ucTimerModel { get; set; }
    public ICommand CountdownClick { get; set; }
    public ICommand CountdownCancelClick { get; set; }
    public UcTimerViewModel() 
    {
        CountdownClick = new RelayCommand(x => CountdownClicked(""));
        CountdownCancelClick = new RelayCommand(x => CountdownCancelClicked(""));
        ucTimerModel = new UcTimerModel();
    }

    private void CountdownClicked(object parameter)
    {
        TiemCountdown(0,1);
    }
    
    private void CountdownCancelClicked(object parameter)
    {
        Cancel();
    }

    public void Cancel()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel(); // Anulujemy token
        }
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
                                Thread.Sleep(10);
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
                //SystemSounds.Beep.Play();
                //SystemSounds.Question.Play();
                Console.Beep(3000, 200);
                //SystemSounds.Exclamation.Play();
                //SystemSounds.Asterisk.Play();
                //SystemSounds.Hand.Play();

                //while (ucTimerModel.MinuteTens <= minuteTens)
                //{
                //    while (ucTimerModel.MinuteOnes < 10)
                //    {
                //        while (ucTimerModel.SecondTens < 6)
                //        {
                //            while (ucTimerModel.SecondOnes < 10)
                //            {
                //                if (token.IsCancellationRequested)
                //                {
                //                    ucTimerModel.SetValuesToZero();
                //                    return;
                //                }
                //                Thread.Sleep(10);
                //                ucTimerModel.SecondOnes++;
                //                if (ucTimerModel.MinuteOnes == minuteOnes && ucTimerModel.MinuteTens == minuteTens)
                //                {
                //                    int i = 0;
                //                }
                //            }
                //            ucTimerModel.SecondOnes = 0;
                //            ucTimerModel.SecondTens++;
                //        }
                //        ucTimerModel.SecondTens = 0;
                //        ucTimerModel.MinuteOnes++;
                //    }
                //    ucTimerModel.MinuteOnes = 0;
                //    ucTimerModel.MinuteTens++;
                //}
                //ucTimerModel.MinuteTens = 0;


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
