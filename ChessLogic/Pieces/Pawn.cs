using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Pawn : Piece 
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Colour { get; }
        private readonly Direction forward;
        public Pawn(Player colour)
        {
            Colour = colour;
            if (colour == Player.White)
            {
                forward = Direction.North;
            }
            else if (colour == Player.Black)
            {
                forward = Direction.South;
            }
        }
        public override Piece Copy()
        {
            Pawn copy = new Pawn(Colour);
            // Copy the moved state from the current piece
            if (Moved)
            {
                copy.MarkAsMoved(); // Set the moved state on the copy
            }

            return copy;
        }
        private static bool AbleToMoveTo(Board board, Position position)
        {
            return Board.IsInside(position) && board.IsEmpty(position);
        }
        private bool AbleToCapture(Board board, Position position)
        {
            if (!Board.IsInside(position) || board.IsEmpty(position))
            {
                return false;
            }
            return board[position].Colour != Colour;
        }
        private IEnumerable<MovementBaseClass> MoveForward(Board board,Position start)
            // ^^returns all forward or non capturing moves that the pawn can make
        {
            Position oneMoveForward = start + forward; //space directly infront of pawn
            if (AbleToMoveTo(board, oneMoveForward))
                // ^^checks if pawn can move there
            {

                if (oneMoveForward.Row == 0 || oneMoveForward.Row == 7)
                {
                    foreach (MovementBaseClass promotionMove in PromotionMoves(start, oneMoveForward))
                    {
                        yield return promotionMove;
                    }
                }
                else
                {
                    RegularMove normal = new RegularMove(start, oneMoveForward);
                    yield return normal;
                    // ^^creates a normal move which moves pawn to that position
                }
                
               

                Position twoMovesForward = oneMoveForward + forward; 
                if (!Moved && AbleToMoveTo(board, twoMovesForward))
                // ^^pawn can only move there if it hasn't been moved before
                {
                    yield return new RegularMove(start, twoMovesForward);
                }
            }
        }
        private IEnumerable<MovementBaseClass> DiagonalCapture(Board board, Position start)
        {
            foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
            {
                Position moveTo = start + direction + forward;
                // ^^ this position is diagonally in front of Pawn
                if (AbleToCapture(board, moveTo))
                {
                    if (moveTo.Row == 0 || moveTo.Row == 7)
                    {
                        foreach (MovementBaseClass promotionMove in PromotionMoves(start, moveTo))
                        {
                            yield return promotionMove;
                        }
                    }
                    else
                    {
                        RegularMove normal = new RegularMove(start, moveTo);
                        yield return normal;
                        // ^^creates a normal move which moves pawn to that position
                    }
                }
            }
        }
        public override IEnumerable<MovementBaseClass> GetMove(Board board, Position start)
            // ^^return legal forward moves
        {
            return MoveForward(board, start).Concat(DiagonalCapture(board, start));
        }

        public override bool AbleToCaptureOpponentsKing(Position start, Board board)
        {
            return DiagonalCapture(board, start).Any(move =>
            {
                Piece piece = board[move.EndingPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }
        private static IEnumerable<MovementBaseClass> PromotionMoves(Position start, Position end)
        {
            foreach (PieceType newType in new PieceType[] { PieceType.Bishop, PieceType.Knight, PieceType.Rook, PieceType.Queen })
            {
                yield return new Pawn_Promotion(start, end, newType);
            }
        }
       
    }
}
