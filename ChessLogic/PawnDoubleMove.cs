using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    internal class PawnDoubleMove :MovementBaseClass
    {
        public override MovementType Type => MovementType.PawnDoubleMove;
        public override Position StartingPos { get; }
        public override Position EndingPos { get; }
        private readonly Position _jumpedpositions;
        public PawnDoubleMove(Position start, Position end)
        {
            StartingPos = start;
            EndingPos = end;
            _jumpedpositions = new Position((end.Row + start.Row) / 2, end.Column);
        }
        public override bool ApplyMove(Board board)
        {
            new RegularMove(StartingPos, EndingPos).ApplyMove(board);
            board.SetPawnJumpPositions(board[EndingPos].Colour, _jumpedpositions);
            Console.WriteLine($"PawnDoubleMove applied: Jumped Position set to {_jumpedpositions}");
            return true;
        }
    }
}
