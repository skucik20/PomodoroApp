using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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


        public void SetValuesToZero()
        {
            SecondOnes = 0; MinuteOnes = 0; SecondTens = 0; MinuteTens = 0;
        }

    }
}
