using AdmitOne.ViewModel;
using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace AdmitOne.View
{
    /// <summary>
    /// Interaction logic for MyTicketsView.xaml
    /// </summary>
    public partial class MyTicketsView : UserControl, IViewFor<MyTicketsViewModel>
    {
        public MyTicketsView()
        {
            InitializeComponent();
        }

        #region IViewFor Boilerplate
        private static DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MyTicketsViewModel), typeof(MyTicketsView), new PropertyMetadata(null));
        public MyTicketsViewModel ViewModel
        {
            get { return (MyTicketsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MyTicketsViewModel)value; }
        }
        #endregion
        
    }
}
