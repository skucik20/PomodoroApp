using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.UserControls;
using WpfApp1.ViewModel.UserControls;

namespace WpfApp1.ViewModel;

public class MainWindowViewModel
{
    private readonly IEventAggregator _eventAggregator;
    public ObservableCollection<UcTimerViewModel> UcTimerViewModelCollection { get; set; }
    public ObservableCollection<ToDoListModel> ToDoListModelCollection { get; set; }
    public MainWindowViewModel() 
    {
        ToDoListModelCollection = new ObservableCollection<ToDoListModel>()
        {
            new ToDoListModel() 
            { 
                Id = 0, TaskTitle = "Lorem ipsum" ,TaskDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            },
            new ToDoListModel() 
            { 
                Id = 1, TaskTitle = "Lorem ipsum" ,TaskDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            },
        };
        //ucTimerViewModel = new UcTimerViewModel(5,2);
        UcTimerViewModelCollection = new ObservableCollection<UcTimerViewModel>()
        {
            new UcTimerViewModel(5,2),
            new UcTimerViewModel(5,0),
            new UcTimerViewModel(5,1)
        };
    }    
}
