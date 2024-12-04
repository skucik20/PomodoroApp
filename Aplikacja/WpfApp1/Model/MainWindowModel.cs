using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model;

public class ToDoListModel
{
    private int id;
    public int Id { get { return id; } set { id = value; } }

    private string isTaskDone;
    public string IsTaskDone { get { return isTaskDone; } set { isTaskDone = value; } }

    private string taskTitle;
    public string TaskTitle { get { return taskTitle; } set { taskTitle = value; } }

    private string taskDescription;
    public string TaskDescription { get { return taskDescription; } set { taskDescription = value; } }
    
    private double taskTimeEstymation;
    public double TaskTimeEstymation { get { return taskTimeEstymation; } set { taskTimeEstymation = value; } }
}
public class MainWindowModel
{

}
