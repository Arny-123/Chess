using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;
        public override Player Colour { get; }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.SouthEast,
            Direction.SouthWest,
            Direction.NorthEast,
            Direction.NorthWest,
        }; //array of directions the bishop can move in
        public Bishop(Player colour) //constructor
        {
            Colour = colour;
        }
        public override Piece Copy() //referenced in board class to make a copy of the board so that illegal moves can be found
        {
            Bishop copy = new Bishop(Colour);
            //makes a new instance of a bishop with the same colour as the original bishop
            if (Moved)
            {
                copy.MarkAsMoved(); //set the moved state on the copy
            }

            return copy;
        }
        public override IEnumerable<MovementBaseClass> GetMove(Board board, Position start)
        {
            List<MovementBaseClass> possibleMoves = new List<MovementBaseClass>(); //list to store all possible moves
            IEnumerable<Position> movePositions = MovePositionsInDirections(board, directions, start);//get all possible positions the piece can move to based on its movement directions
            foreach (Position end in movePositions)//iterate over each valid position
            {
                RegularMove move = new RegularMove(start, end);//create a new move from the start position to the end position
                possibleMoves.Add(move);//add the move to the list of possible moves
            }
            return possibleMoves;
        } //returns all the moves that the bishop can make
    }
}
