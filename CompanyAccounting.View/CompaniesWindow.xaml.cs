using CompanyAccounting.ViewModel;
using GalaSoft.MvvmLight;
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

namespace CompanyAccounting.View
{
    /// <summary>
    /// Логика взаимодействия для CompaniesWindow.xaml
    /// </summary>
    public partial class CompaniesWindow : Window
    {
        public CompaniesWindow()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(DataContext is CompaniesViewModel companiesVM) || !(e.NewValue is ViewModelBase newValue))
                return;

            companiesVM.SelectedItem = newValue;
        }
    }
}
