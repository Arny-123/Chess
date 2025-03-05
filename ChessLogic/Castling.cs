using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Castling : MovementBaseClass
    {
        public override MovementType Type => MovementType.CastleKing;
        public override Position StartingPos { get; }
        public override Position EndingPos { get; }
        private readonly Position rookStart;
        private readonly Position rookEnd;
        public Castling(Position kingStart, Position kingEnd, Position rookStart, Position rookEnd)
        {
            StartingPos = kingStart;
            EndingPos = kingEnd;
            this.rookStart = rookStart;
            this.rookEnd = rookEnd;
        }
        public override void ApplyMove(Board board)
        {
            Piece king = board[StartingPos];
            Piece rook = board[rookStart];
            board[StartingPos] = null; // Remove the king from its starting position
            board[EndingPos] = null; // Clear the ending position
            board[rookStart] = null; // Remove the rook from its starting position
            board[rookEnd] = null; // Clear the ending position
            king.MarkAsMoved();
            rook.MarkAsMoved();
            board[EndingPos] = king; // Place the king on the ending position
            board[rookEnd] = rook; // Place the rook on the ending position
        }
    }
    
    
}
