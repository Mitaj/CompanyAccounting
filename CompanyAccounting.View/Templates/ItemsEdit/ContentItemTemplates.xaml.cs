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

namespace CompanyAccounting.View.Templates.ItemsEdit
{
    /// <summary>
    /// Логика взаимодействия для ContentItemTemplates.xaml
    /// </summary>
    public partial class ContentItemTemplates : ResourceDictionary
    {
        public ContentItemTemplates()
        {
            InitializeComponent();
        }

        private void TextBoxNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var updatedValue = ((TextBox)sender).Text + e.Text;
            e.Handled = !uint.TryParse(updatedValue, out _);
        }
    }
}
