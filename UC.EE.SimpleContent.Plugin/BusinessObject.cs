using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UC.EE.SimpleContent.Plugin
{
    public class BusinessObject : INotifyPropertyChanged
    {
        private string _regNr;
        private string _name;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string registrikood
        {
            get { return _regNr; }
            set
            {
                if (value != _regNr)
                {
                    _regNr = value;
                    OnPropertyChanged("RegNr");
                }
            }
        }

        public string nimi
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
    }
}
