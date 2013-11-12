using AdmitOne.ViewModel;
using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace AdmitOne.View
{
    /// <summary>
    /// Interaction logic for AssignedTicketsView.xaml
    /// </summary>
    public partial class AssignedTicketsView : UserControl, IViewFor<AssignedTicketsViewModel>
    {
        public AssignedTicketsView()
        {
            InitializeComponent();
        }

        #region IViewFor Boilerplate
        private static DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(AssignedTicketsViewModel), typeof(AssignedTicketsView), new PropertyMetadata(null));
        public AssignedTicketsViewModel ViewModel
        {
            get { return (AssignedTicketsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AssignedTicketsViewModel)value; }
        }
        #endregion
        
    }
}
