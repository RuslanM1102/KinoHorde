using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.MVVM.ViewModel
{
    class ProfileViewModel : ReactiveObject
    {

        public Action AuthSuccessed { get; set; }
        public IReactiveCommand AuthCommand { get;set; }
        public ProfileViewModel()
        {
            AuthCommand = ReactiveCommand.Create(() =>  AuthSuccessed?.Invoke());
        }
    }
}
