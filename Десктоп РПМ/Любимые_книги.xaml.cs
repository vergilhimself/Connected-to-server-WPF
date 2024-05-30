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
using System.Windows.Shapes;

namespace Десктоп_РПМ
{
    /// <summary>
    /// Логика взаимодействия для Любимые_книги.xaml
    /// </summary>
    public partial class Любимые_книги : Window
    {
        public Любимые_книги()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow taskWindow = new MainWindow();
            taskWindow.Show();
            this.Close();
        }
    }
}
