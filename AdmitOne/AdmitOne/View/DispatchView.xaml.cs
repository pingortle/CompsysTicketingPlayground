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

namespace AdmitOne.View
{
    /// <summary>
    /// Interaction logic for DispatchView.xaml
    /// </summary>
    public partial class DispatchView : UserControl, IViewFor<DispatchViewModel>
    {
        public DispatchView()
        {
            InitializeComponent();
        }
            
        #region IViewFor Boilerplate
        private static DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DispatchViewModel), typeof(DispatchView), new PropertyMetadata(null));
        public DispatchViewModel ViewModel
        {
            get { return (DispatchViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (DispatchViewModel)value; }
        }
        #endregion
        
    }
}
