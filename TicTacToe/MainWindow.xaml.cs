using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
           DataContext = _viewModel  = new MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.NextHumanMove((Cell) ((Button)sender).DataContext);
        }

        private void Host_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((s) =>
                {
                    Server server = new Server();
                    server.Run();
                });

            JoinGame();
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            JoinGame();
        }

        private void JoinGame()
        {

        }
    }
}
