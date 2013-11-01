using AdmitOne.ViewModel;
using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace AdmitOne.View
{
    /// <summary>
    /// Interaction logic for LoginWidgetView.xaml
    /// </summary>
    public partial class LoginWidgetView : UserControl, IViewFor<LoginWidgetViewModel>
    {
        public LoginWidgetView()
        {
            InitializeComponent();
        }
        
        #region IViewFor Boilerplate
        private static DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(LoginWidgetViewModel), typeof(LoginWidgetView), new PropertyMetadata(null));
        public LoginWidgetViewModel ViewModel
        {
            get { return (LoginWidgetViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LoginWidgetViewModel)value; }
        }
        #endregion
    }
}
