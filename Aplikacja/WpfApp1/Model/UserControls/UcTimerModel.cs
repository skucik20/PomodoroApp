using ControlzEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.Model.UserControls
{
    public class UcTimerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int _secondOnes;
        public int SecondOnes
        {
            get { return _secondOnes; } set { _secondOnes = value; OnPropertyChanged(nameof(SecondOnes)); }
        }
        
        private int _secondTens;
        public int SecondTens
        {
            get { return _secondTens; } set { _secondTens = value; OnPropertyChanged(nameof(SecondTens)); }
        }
        private int _minuteOnes;
        public int MinuteOnes
        {
            get { return _minuteOnes; } set { _minuteOnes = value; OnPropertyChanged(nameof(MinuteOnes)); }
        }
        
        private int _minuteTens;
        public int MinuteTens
        {
            get { return _minuteTens; } set { _minuteTens = value; OnPropertyChanged(nameof(MinuteTens)); }
        }

        private Duration _duration = new Duration(TimeSpan.FromSeconds(60));
        public Duration Durationa
        {
            get { return _duration; } set { _duration = value; OnPropertyChanged(nameof(Durationa)); } 
        }
        private TimeSpan _beginOpacityZero = TimeSpan.FromSeconds(60);
        public TimeSpan BeginOpacityZero
        {
            get { return _beginOpacityZero; } set { _beginOpacityZero = value; OnPropertyChanged(nameof(BeginOpacityZero)); Durationa = new Duration(BeginOpacityZero); } 
        }
        public void SetValuesToZero()
        {
            SecondOnes = 0; MinuteOnes = 0; SecondTens = 0; MinuteTens = 0;
        }

        private bool _isStartVisible = true;
        public bool IsSStartVisible
        {
            get { return _isStartVisible; } set { _isStartVisible = value; OnPropertyChanged(nameof(IsSStartVisible)); }
        }
    }
}
