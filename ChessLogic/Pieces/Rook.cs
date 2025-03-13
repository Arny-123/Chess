using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override Player Colour { get; }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.East,
            Direction.North,
            Direction.South,
            Direction.West,
        };
        public Rook(Player colour)
        {
            Colour = colour;
        }
        public override Piece Copy()
        {
            Rook copy = new Rook(Colour);
            // Copy the moved state from the current piece
            if (Moved)
            {
                copy.MarkAsMoved(); // Set the moved state on the copy
            }

            return copy;
        } //makes a new instance of a rook with the same colour as the original rook
        public override IEnumerable<MovementBaseClass> GetMove(Board board, Position start)
        {
            // Get all possible end positions based on the directions
            IEnumerable<Position> endPositions = MovePositionsInDirections(board, directions, start);

            // Create a normal move for each end position
            IEnumerable<MovementBaseClass> moves = (IEnumerable<MovementBaseClass>)endPositions.Select(end => new RegularMove(start, end));

            return moves;
        } //returns all the moves that the rook can make
    }
}
