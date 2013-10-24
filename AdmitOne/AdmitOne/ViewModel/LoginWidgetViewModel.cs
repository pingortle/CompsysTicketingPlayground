using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace AdmitOne.ViewModel
{
    public sealed class LoginWidgetViewModel : ReactiveObject
    {
        public LoginWidgetViewModel(ILogPeopleIn loginManager)
        {
            Login = new ReactiveCommand(this.WhenAnyValue(x => x.Username, y => y.Password, (x, y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y)));
            Login.RegisterAsyncFunction(_ => loginManager.TryLogin(Username, Password))
                .Subscribe(x => Status = x ? "Success!" : "Failed.  Please try again.");

            Login.IsExecuting.Where(x => x).Select(_ => "Working...").BindTo(this, x => x.Status);

            _isEnabled = Login.IsExecuting.Select(x => !x).ToProperty(this, x => x.IsEnabled);
        }

        public IReactiveCommand Login { get; private set; }

        private string _username;
        public string Username
        {
            get { return _username ?? ""; }
            set { this.RaiseAndSetIfChanged(ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password ?? ""; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        private string _status;
        public string Status
        {
            get { return _status ?? ""; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        private ObservableAsPropertyHelper<bool> _isEnabled;
        public bool IsEnabled { get { return _isEnabled.Value; } }
    }
}
