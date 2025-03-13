using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;
        public override Player Colour { get; }
        private static readonly Direction[] directions = new Direction[]
       {
            Direction.SouthEast,
            Direction.SouthWest,
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
       };
        public Queen(Player colour)
        {
            Colour = colour;
        } //constructor
        public override Piece Copy()
        {
            Queen copy = new Queen(Colour);
            // Copy the moved state from the current piece
            if (Moved)
            {
                copy.MarkAsMoved(); // Set the moved state on the copy
            }

            return copy;
        } //makes a new instance of a queen with the same colour as the original queen
        public override IEnumerable<MovementBaseClass> GetMove(Board board, Position start)
        {
            // Get all possible end positions based on the directions
            IEnumerable<Position> endPositions = MovePositionsInDirections(board, directions, start);

            // Create a normal move for each end position
            IEnumerable<MovementBaseClass> moves = (IEnumerable<MovementBaseClass>)endPositions.Select(end => new RegularMove(start, end));

            return moves;
        } //returns all the moves that the queen can make
    }
}
