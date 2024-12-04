using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model.UserControls;
using WpfApp1.ViewModel.UserControls;

namespace WpfApp1.ViewModel;

public class MainWindowViewModel
{
    public UcTimerViewModel ucTimerViewModel { get; set; }
    public MainWindowViewModel() 
    {
        ucTimerViewModel = new UcTimerViewModel();
    }    
}
