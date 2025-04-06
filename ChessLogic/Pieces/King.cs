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
            if (AbleToCastleOnKingSide(start, board))
            {
                yield return new Castling(MovementType.CastleKing,start);
            }
            if (AbleToCastleOnQueenSide(start, board))
            {
                yield return new Castling(MovementType.CastleQueen,start);
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
        private static bool RookNotMoved(Board board, Position position)
        {
            if (board.IsEmpty(position))
            {
                return false;
            }
            Piece piece = board[position];
            return piece.Type == PieceType.Rook && !piece.Moved;
        } //checks if the rook has moved
        private static bool EmptySpacesBetween(IEnumerable<Position> positions, Board board)
        {
            return positions.All(position => board.IsEmpty(position));
        }
        private bool AbleToCastleOnKingSide(Position start, Board board)
        {
            if (Moved)
            {
                return false;
            }
            Position rookPosition = new Position(start.Row, 7);
            Position[] positionsInBetween = new Position[]
            {
                new Position(start.Row, 5),
                new Position(start.Row, 6),
            };
            return RookNotMoved(board, rookPosition) && EmptySpacesBetween(positionsInBetween, board);
            // Additional logic for castling on the king side
        }
        private bool AbleToCastleOnQueenSide(Position start, Board board)
        {
            if (Moved)
            {
                return false;
            }
            Position rookPosition = new Position(start.Row, 0);
            Position[] positionsInBetween = new Position[]
            {
                new Position(start.Row, 1),
                new Position(start.Row, 2),
                new Position(start.Row, 3),
            };
            return RookNotMoved(board, rookPosition) && EmptySpacesBetween(positionsInBetween, board);
            // Additional logic for castling on the queen side
        }

    }
}
