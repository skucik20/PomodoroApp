using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModels;

public class ShellViewModel : Screen
{
	private string _firstName = "tim";
    private string _lastName = "";

    public ShellViewModel()
    {
			
    }

    public string FirstName
    {
		get { return _firstName; }
		set 
		{ 
			_firstName = value; 
			NotifyOfPropertyChange(() => FirstName);
            NotifyOfPropertyChange(() => LastName);
        }
	}

	public string LastName
    {
		get { return _lastName; }
		set 
		{
			_lastName = value; 
			NotifyOfPropertyChange(() => LastName);
            NotifyOfPropertyChange(() => FirstName);
        }
	}

	public string FullName
    {
		get { return $"{FirstName} {LastName}"; }
	}



}
