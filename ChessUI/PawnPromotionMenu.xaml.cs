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
    /// Interaction logic for PawnPromotionMenu.xaml
    /// </summary>
    public partial class PawnPromotionMenu : UserControl
    {
        public event Action<PieceType> PromotionChosen;
        public PawnPromotionMenu(Player player)
        {
            InitializeComponent();
            QueenImg.Source = Images.GetImage(player, PieceType.Queen);
            RookImg.Source = Images.GetImage(player, PieceType.Rook);
            BishopImg.Source = Images.GetImage(player, PieceType.Bishop);
            KnightImg.Source = Images.GetImage(player, PieceType.Knight);
        }

        private void QueenImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PromotionChosen?.Invoke(PieceType.Queen);
        }
        private void RookImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PromotionChosen?.Invoke(PieceType.Rook);
        }
        private void BishopImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PromotionChosen?.Invoke(PieceType.Bishop);
        }

        private void KnightImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PromotionChosen?.Invoke(PieceType.Knight);
        }
    }
}
