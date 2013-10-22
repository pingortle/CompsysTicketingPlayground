using AdmitOne.ViewModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdmitOne
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

        private static DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(LoginWidgetViewModel), typeof(LoginWidgetView), new PropertyMetadata(null));
        public LoginWidgetViewModel ViewModel
        {
            get
            {
                return (LoginWidgetViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty, value);
            }
        }

        object IViewFor.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (LoginWidgetViewModel)value;
            }
        }
    }
}
