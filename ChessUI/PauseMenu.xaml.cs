using ChessLogic;
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
namespace ChessUI
{
    /// <summary>
    /// Interaction logic for InGameMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        public event Action<Choices> ChoiceSelected;
        public PauseMenu(GameState gameState)
        {
            InitializeComponent();
        }


        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            ChoiceSelected?.Invoke(Choices.Resume);
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            ChoiceSelected?.Invoke(Choices.Load);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ChoiceSelected?.Invoke(Choices.Save);
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            ChoiceSelected?.Invoke(Choices.Restart);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ChoiceSelected?.Invoke(Choices.Exit);
        }
    }
}
