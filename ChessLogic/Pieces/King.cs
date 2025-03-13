using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Colour { get; }
        private static readonly Direction[] directions = new Direction[]
        {
            Direction.East,
            Direction.North,
            Direction.South,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.NorthWest,
            Direction.SouthWest,
        }; //array of directions the king can move in
        public King(Player colour)
        {
            Colour = colour;
        } //constructor
        public override Piece Copy()
        {
            King copy = new King(Colour);
            //copy the moved state from the current piece
            if (Moved)
            {
                copy.MarkAsMoved(); //set the moved state on the copy
            }

            return copy;
        } //makes a new instance of a king with the same colour as the original king
        private IEnumerable<Position> MovePositions(Board board, Position start)
        {
            foreach (Direction directions in directions)
            {
                Position end = start + directions;

                if (!Board.IsInside(end))
                {
                    continue;
                }
                if (board.IsEmpty(end) || board[end].Colour != Colour)
                {
                    yield return end;
                }
            }
        } //find valid positions the king can move to
        public override IEnumerable<MovementBaseClass> GetMove(Board board, Position start)
        {
            foreach (Position end in MovePositions(board,start)) 
            {
                yield return new RegularMove(start, end);
            }
        } //returns all the moves that the king can make
        public override bool AbleToCaptureOpponentsKing(Position start, Board board)
        {
            return MovePositions(board, start).Any(end =>
            {
                Piece piece = board[end];
                return piece != null && piece.Type == PieceType.King;
            });
        } //checks if the king can capture the opponent's king
    }
}
