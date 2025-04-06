 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class RegularMove : MovementBaseClass
    {
        public override MovementType Type => MovementType.Normal;
        public override Position StartingPos { get; }
        public override Position EndingPos { get; }
        public RegularMove(Position start, Position end) //constructor
        {
            StartingPos = start;
            EndingPos = end;
        }
        public override bool ApplyMove(Board board) //makes the move happen
        {
            //retrieve the piece from the starting position
            Piece movingPiece = board[StartingPos];
            bool capture = !board.IsEmpty(EndingPos); //check if the ending position is empty or not
            //update the board with the new positions
            board[EndingPos] = movingPiece; 
            board[StartingPos] = null;

            //mark the piece as having moved
            if (movingPiece != null)
            {
                movingPiece.MarkAsMoved();
            }
            return capture || movingPiece.Type == PieceType.Pawn; //return true if the move was a capture or if the piece is a pawn
        }
    }
}
