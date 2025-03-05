using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Pawn_Promotion : MovementBaseClass
    {
        public override MovementType Type => MovementType.PawnPromotion;
        public override Position StartingPos { get; }
        public override Position EndingPos { get; }
        private readonly PieceType newType;
        public Pawn_Promotion(Position start, Position end, PieceType newType)
        {
            StartingPos = start;
            EndingPos = end;
            this.newType = newType;
        }
        private Piece CreatePieceToPromote(Player colour)
        {
            return newType switch
            {
                PieceType.Bishop => new Bishop(colour),
                PieceType.Knight => new Knight(colour),
                PieceType.Rook => new Rook(colour),
                _ => new Queen(colour),
            };
        }
        public override void ApplyMove(Board board)
        {
            Piece pawn = board[StartingPos];
            board[StartingPos] = null; // Remove the pawn from its starting position
            board[EndingPos] = null; // Clear the ending position
            Piece pieceToPromote = CreatePieceToPromote(pawn.Colour);
            pieceToPromote.MarkAsMoved();
            board[EndingPos] = pieceToPromote; // Place the promoted piece on the ending position
        }
 
    }
}
