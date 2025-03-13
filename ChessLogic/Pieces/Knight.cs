using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Colour { get; }
        public Knight(Player colour)
        {
            Colour = colour;
        } //constructor for knight
        public override Piece Copy()
        {
            Knight copy = new Knight(Colour);
            // Copy the moved state from the current piece
            if (Moved)
            {
                copy.MarkAsMoved(); // Set the moved state on the copy
            }

            return copy;
        } //makes a new instance of a knight with the same colour as the original knight
        private static IEnumerable<Position> PotentialEndPositions(Position start)
        {
            foreach (Direction verticaldir in new Direction[] { Direction.South, Direction.North})
            {
                foreach (Direction horizontaldir in new Direction[] {Direction.East, Direction.West })
                {
                    yield return start + (2 * verticaldir) + horizontaldir;
                    yield return start + (2 * horizontaldir) + verticaldir;
                    // ^^returns all 8 potential places knight can move to 
                }
            }
        } //Finds all potential knight moves (without checking validity).
        private IEnumerable<Position> MovePositions(Board board, Position start)
        {
            return PotentialEndPositions(start)
                .Where(position => IsValidMove(board, position));
        } //Filters valid moves from the potential ones.

        private bool IsValidMove(Board board, Position position)
        {
            return Board.IsInside(position) && (board.IsEmpty(position) || board[position].Colour != Colour);
        } //Checks if a position is inside the board and not occupied by the same color.
        public override IEnumerable<MovementBaseClass> GetMove(Board board, Position start)
        {
            return (IEnumerable < MovementBaseClass > )MovePositions(board, start).Select(end => new RegularMove(start, end)); //returns all the moves that the knight can make
        } //Converts valid moves into move objects 
    }
}
