using AdmitOne.ViewModel;
using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace AdmitOne.View
{
    /// <summary>
    /// Interaction logic for CreateTicketsView.xaml
    /// </summary>
    public partial class CreateTicketsView : UserControl, IViewFor<CreateTicketsViewModel>
    {
        public CreateTicketsView()
        {
            InitializeComponent();
        }

        #region IViewFor Boilerplate
        private static DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CreateTicketsViewModel), typeof(CreateTicketsView), new PropertyMetadata(null));
        public CreateTicketsViewModel ViewModel
        {
            get { return (CreateTicketsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (CreateTicketsViewModel)value; }
        }
        #endregion
    }
}
