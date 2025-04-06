using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; } 
        public GameOver GameOver { get; private set; } = null;
        private int NoCapturesOrPawnMovements = 0; //used to check for fifty move rule
        private string stateString;
        private readonly Dictionary<string, int> HistoryOfStates = new Dictionary<string, int>();

        public GameState (Player player, Board board) //constructor
        {
            CurrentPlayer = player;
            Board = board;
            stateString = new StateString(CurrentPlayer,board).ToString();
            HistoryOfStates[stateString] = 1; //initialize the state string with 1
        }   
        public List<MovementBaseClass> MoveHistory { get; private set; } = new List<MovementBaseClass>();
        public void MakeMove (MovementBaseClass move) 

        {
            Board.SetPawnJumpPositions(CurrentPlayer, null);
            bool captureOrPawn = move.ApplyMove(Board);
            if (captureOrPawn)
            {
                NoCapturesOrPawnMovements = 0; //reset the counter if a capture or pawn movement is made
                HistoryOfStates.Clear();
            }
            else
            {
                NoCapturesOrPawnMovements++;
            }
            CurrentPlayer = CurrentPlayer.Opponent();
            UpdateStateString(); //update the state string with the current state of the game
            CheckForGameOver(); //need to check if the game has ended after every move.
           // MoveHistory.Add(move); //add the move to the move history
        }
        public IEnumerable<MovementBaseClass> LegalMovesFor (Player player)
        {
            IEnumerable<MovementBaseClass> possibleMoves = Board.PiecePositionsFor(player).SelectMany(position =>
            {
                Piece piece = Board[position]; //get piece at current position
                return piece.GetMove(Board, position); //get all possible moves for that piece
            });
            return possibleMoves.Where(move => move.Legal(Board)); //filters out illegal moves
        } //returns all legal moves for a player
        private void CheckForGameOver() //checks to see if game is over
        {
            if (!LegalMovesFor(CurrentPlayer).Any()) //if current player has no legal moves left
            {
                if (Board.InCheck(CurrentPlayer))
                {
                     GameOver = GameOver.Win(CurrentPlayer.Opponent()); //opponent wins 
                }
                else
                {
                    GameOver = GameOver.Draw(GameOverReason.Stalemate); //its a draw due to stalemate
                }
            }
            else if(Board.InsufficientMaterial())
            {
                 GameOver = GameOver.Draw(GameOverReason.InsufficientMaterial);
            }
            else if (IsFiftyMoveRuleApplicable())
            {
                GameOver = GameOver.Draw(GameOverReason.FiftyMoveRule);
            }
            else if (ThreeFoldRepetition())
            {
                GameOver = GameOver.Draw(GameOverReason.ThreefoldRepetition);
            }
        }
        public bool GameIsOver() //returns whether game is over or not
        {
            return GameOver != null;
        }

        public List<MovementBaseClass> LegalMoves(Position position, int depth) //depth-first search, returns list of legal moves

        {
            //base case: if depth is zero, stop recursion
            if (depth == 0)
            {
                return new List<MovementBaseClass>();
            }

            //if the position is invalid (empty or not the current player's piece), return an empty list
            if (Board.IsEmpty(position) || Board[position].Colour != CurrentPlayer)
            {
                return new List<MovementBaseClass>();
            }

            //get the piece at the current position
            Piece piece = Board[position];
            List<MovementBaseClass> possibleMoves = piece.GetMove(Board, position).ToList();
            List<MovementBaseClass> legalMoves = new List<MovementBaseClass>();
            //iterate through possible moves
            foreach (var move in possibleMoves)
            {
                if (move.Legal(Board)) //check if the move is legal
                {
                    legalMoves.Add(move); //add to the legal moves list
                    //simulate the move by calling the ApplyMove function on the piece's move
                    var simulatedBoard = Board.Copy();
                    move.ApplyMove(simulatedBoard); //apply the move
                    //recursively explore further moves from this new board state
                    var nextPosition = move.EndingPos;
                    var deeperMoves = new GameState(CurrentPlayer.Opponent(), simulatedBoard).LegalMoves(nextPosition, depth - 1);
                    legalMoves.AddRange(deeperMoves);
                }
            }

            return legalMoves;
        }
       /* public void undoMove(MovementBaseClass move) //undo the move
        {
            move.ApplyMove(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
        }
        public void HistoryOfMoves() //prints the move history
        {
             // Prints the move history
        
            foreach (var move in MoveHistory)
            {
                Console.WriteLine(move);
            }
        }
       /* public void UndoLastMove()
        {
            if (MoveHistory.Count > 0)
            {
                MovementBaseClass lastMove = MoveHistory.Last();
                MoveHistory.RemoveAt(MoveHistory.Count - 1);

                // Restore the board state
                Board[lastMove.Start] = Board[lastMove.End]; // Move piece back
                Board[lastMove.End] = lastMove.CapturedPiece; // Restore captured piece, if any
            } 
        }*/
        private bool IsFiftyMoveRuleApplicable()
        {
            int fullMoves = NoCapturesOrPawnMovements /2; //each player has made a move
            return fullMoves ==50 ; //if no captures or pawn movements have been made in the last 50 moves
        }
        private void UpdateStateString()
        {
            stateString = new StateString(CurrentPlayer, Board).ToString();
            if (HistoryOfStates.ContainsKey(stateString))
            {
                HistoryOfStates[stateString]++;
            }
            else
            {
                HistoryOfStates[stateString] = 1;
            }

        } //updates the state string with the current state of the game
        private bool ThreeFoldRepetition()
        {
            return HistoryOfStates[stateString] == 3; //if the state string has been seen 3 times, its a draw
        }
    }
    
}
