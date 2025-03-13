
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public abstract class Piece //all pieces inherit from this class
    {
        public abstract PieceType Type { get; }
        public abstract Player Colour { get; }
        public bool Moved { get; private set; }
        public void MarkAsMoved()//method to mark the piece as moved
        {
            Moved = true;
        }


        public abstract Piece Copy(); //overriden in other classes to copy all pieces 
        //allows board class to create a copy of the board to check for illegal moves

        public abstract IEnumerable<MovementBaseClass> GetMove ( Board board, Position start);
        // ^^takes current position and board and return all the available moves for the piece
        protected IEnumerable<Position> MovePositionsInDirection(Board board, Direction direction, Position start)
        {
            for (Position position = start + direction; Board.IsInside(position); position += direction)
            //piece moves one step at a time in the given direction 
            //loops as far as a piece can go until it reaches the edges of the board
            {
                if (board.IsEmpty(position)) //if that cell is empty, it is a valid move
                {
                    yield return position;
                    continue; //skips the rest of the loop and moves onto next position
                }
                Piece piece = board[position];
                if (piece.Colour != Colour) //checks to see if 

                {
                    yield return position;
                    
                }
                yield break;
            }
        }

        protected IEnumerable<Position> MovePositionsInDirections(Board board, Direction[] directions, Position start)
            // ^^takes an array of directions rather than just one direction
        {
            return directions.SelectMany(direction => MovePositionsInDirection(board, direction, start));
            // ^^collection of all reachable positions in the given directions
        }
        public virtual bool AbleToCaptureOpponentsKing(Position start, Board board)
        {

            return GetMove(board, start).Any(move =>
            {
                Piece piece = board[move.EndingPos];
                return piece != null && piece.Type == PieceType.King;
            });


        } //checks if the piece can capture the opponents king
    }
}
