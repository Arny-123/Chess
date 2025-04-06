using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class EnPassant : MovementBaseClass
    {
        public override MovementType Type => MovementType.EnPassant;
        public override Position StartingPos { get; }
        public override Position EndingPos { get; }
        private readonly Position capturePosition;
        public EnPassant(Position start, Position end)
        {
            StartingPos = start;
            EndingPos = end;
            capturePosition = new Position((start.Row + end.Row) / 2, end.Column);
        }
        public override bool ApplyMove(Board board)
        {
            new RegularMove(StartingPos, EndingPos).ApplyMove(board);
            board[capturePosition] = null;
            return true;
        }
    }
}
