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
using ChessLogic;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, MovementBaseClass> movementCache = new Dictionary<Position, MovementBaseClass>();


        private GameState gameState;
        private Position selectedPosition = null;
        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            gameState = new GameState(Player.White, Board.Initially()); //because white always begins

            DrawBoard(gameState.Board);

            StartMenu(); //displays start menu

            SetCursor(gameState.CurrentPlayer);

            this.KeyDown += new KeyEventHandler(CheckPause); // enter to pause the game
        }
        private void InitializeBoard()
        {
            for (int row = 0; row <8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Image image = new Image();
                    pieceImages[row, column] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle highlight = new Rectangle();
                    highlights[row, column] = highlight;
                    HighLightGrid.Children.Add(highlight);
                }
            }
        }
        private void DrawBoard(Board board)
            //takes board as parameter and sets source of all image controls so they match pieces on board
        {
            for (int row = 0;row < 8; row++)
            {
                for (int col = 0;col < 8; col++)
                {
                    Piece piece = board[row, col];
                    pieceImages[row, col].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)//handles mouse input
        {
            if (MenuOnScreen())
            {
                return;
            }
            Point point = e.GetPosition(BoardGrid);
            Position position = EndSquarePosition(point);

            if (selectedPosition == null)
            {
                SelectedStartPosition(position);
            }
            else
            {
                SelectedEndPosition(position);
            }

        } 

        private Position EndSquarePosition(Point point)
        {
            double squaresize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squaresize);
            int column = (int)(point.X / squaresize);
            return new Position (row, column);
        }

        private void SelectedStartPosition(Position position)
        {
            IEnumerable<MovementBaseClass> moves = gameState.LegalMoves(position); 
            if (moves.Any())
            {
                selectedPosition = position;
                CacheForMovement(moves);
                ShowHighlightedCells();
            }
        }

        private void SelectedEndPosition(Position position)
        {
            selectedPosition = null;
            HideHighlightedCells();

            if (movementCache.TryGetValue(position, out MovementBaseClass move))
            {
                HandleMove(move);
            }
        }

        private void HandleMove(MovementBaseClass move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);

            if (gameState.GameIsOver())
            {
                DisplayGameOver();
            }
        }
        private void CacheForMovement(IEnumerable<MovementBaseClass> moves)
        {
            movementCache.Clear();

            foreach (MovementBaseClass move in moves)
            {
                movementCache[move.EndingPos] = move;
            }
        } //temporary  memory to store movements of copied board

        private void ShowHighlightedCells()
        {
            Color color = Color.FromArgb(175, 217, 89, 166);

            foreach (Position end in movementCache.Keys)
            {
                highlights[end.Row, end.Column].Fill = new SolidColorBrush(color);
            }
        } //highlights cells which piece can move to
        private void HideHighlightedCells()
        {
            foreach (Position end in movementCache.Keys)
            {
                highlights[end.Row, end.Column].Fill = Brushes.Transparent;
            }
        } //stops highlighting cells
        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = Cursors.WhiteCursor;
            }
            else if (player == Player.Black)
            {
                Cursor = Cursors.BlackCursor;
            }
        } //sets cursor colour to represent players turn
        private bool MenuOnScreen()
        {
            return MenuContainer.Content != null;
        } //returns whether menu is on screen or not
        private void StartMenu()
        {
            StartMenu startMenu = new StartMenu(gameState);
            MenuContainer.Content = startMenu;
            startMenu.ChoiceSelected += choice =>
            {
                if (choice == Choices.Resume)
                {
                    MenuContainer.Content = null;
                    ResumeGame();
                }
                else if (choice == Choices.Load)
                {
                    //load logic here
                }
                else if (choice == Choices.Exit)
                {
                    Application.Current.Shutdown(); //closes application
                }
            };
        }
        private void CheckPause(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PauseMenu();
            }
        }
        private void PauseMenu()
        {
            PauseMenu pauseMenu = new PauseMenu(gameState);
            MenuContainer.Content = pauseMenu;
            pauseMenu.ChoiceSelected += choice =>
            {
                if (choice == Choices.Resume)
                {
                    MenuContainer.Content = null;
                    ResumeGame();
                }
                else if (choice == Choices.Load)
                {
                    //load logic here
                }
                else if (choice == Choices.Save)
                {
                    //save logic here
                }
                else if (choice == Choices.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else if (choice == Choices.Exit)
                {
                    Application.Current.Shutdown(); //closes application
                }
            };
        }
        private void DisplayGameOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(gameState);
            MenuContainer.Content = gameOverMenu;
            gameOverMenu.ChoiceSelected += choice =>
            {
                if (choice == Choices.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    Application.Current.Shutdown(); //closes application
                }
            };
        }
        private void ResumeGame()
        {
            /*HideHighlightedCells(); //stops highlighting cells
            movementCache.Clear(); //clears movement cache*/ //remove l8r? (kept for bugtesting)
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }
        private void RestartGame()
        {
            HideHighlightedCells(); //stops highlighting cells
            movementCache.Clear(); //clears movement cache
            gameState = new GameState(Player.White,Board.Initially());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        } //resets the game to initial layout

        private void InGameMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
