using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MultilanguageChat.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string userName;
        public string UserName
        {
            get => userName;
            set => Set(ref userName, value, broadcast: true);
        }

        public MainViewModel()
        {
            UserName = DeviceInfo.Name;
        }

        public override void Activate(object parameter)
        {
            base.Activate(parameter);
        }
    }
}
