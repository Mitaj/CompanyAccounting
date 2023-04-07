using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace CompanyAccounting
{
    /// <summary>
    /// Логика взаимодействия для StartView.xaml
    /// </summary>
    public partial class StartView : Window
    {
        public StartView()
        {
            InitializeComponent();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is bool visible) || visible || 
                !(StartViewGrid.DataContext is StartViewModel startVM))
                return;

            startVM.ShowCompaniesViewCommand.Execute(this);
        }
    }
}
