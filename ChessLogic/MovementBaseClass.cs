using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public abstract class MovementBaseClass
    {
        public abstract MovementType Type { get; } 
        public abstract Position StartingPos { get; } //pieces move from here
        public abstract Position EndingPos { get; } //pieces move to here

        public abstract bool ApplyMove(Board board); //general function to apply move to a piece either on simulated board or actual board
        //overriden
        public virtual bool Legal(Board board)  
        //virtual so that child classes can override as they may require additional conditions such as castling etc.
        {
            return IsMoveLegal(board, StartingPos);
        } 
        private bool IsMoveLegal(Board board, Position startingPos)
        {
            Player currentPlayer = board[startingPos].Colour;
            Board copyOfBoard = board.Copy();
            ApplyMove(copyOfBoard); //applies each move to the simulated board
            return !copyOfBoard.InCheck(currentPlayer); //if that move results in the player still being in check, the move is not legal and will not be highlighted
        }
    }

}
