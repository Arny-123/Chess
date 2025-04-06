using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class StateString
    {
        private readonly StringBuilder stringbuilder = new StringBuilder();
        public StateString(Player currentPlayer, Board board)
        {
            AddData_PiecePlacement(board);
            stringbuilder.Append(" ");
            AddData_CurrentPlayer(currentPlayer);
            stringbuilder.Append(" ");
            AddData_Castling(board);
            stringbuilder.Append(" ");
            AddData_EnPassant(board, currentPlayer);

        }
        private static char PieceChar(Piece piece)
        {
            char c = piece.Type switch
            {
                PieceType.Pawn => 'p',
                PieceType.Knight => 'n',
                PieceType.Bishop => 'b',
                PieceType.Rook => 'r',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => ' '
            };
            if (piece.Colour == Player.White)
            {
                return char.ToUpper(c);
            }
            else
            {
                return c;
            }
        }
        private void AddData_Row(Board board, int row)
        {
            int empty = 0;
            for (int column = 0; column < 8; column++)
            {
                Piece piece = board[new Position(row, column)];
                if (piece == null)
                {
                    empty++;
                }
                else
                {
                    if (empty > 0)
                    {
                        stringbuilder.Append(empty);
                        empty = 0;
                    }
                    stringbuilder.Append(PieceChar(piece));
                }
            }
            if (empty > 0)
            {
                stringbuilder.Append(empty); // important for trailing empty squares
            }
        }
        private void AddData_PiecePlacement(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                if (row != 0)
                {
                    stringbuilder.Append('/');
                }
                AddData_Row(board, row);
            }
        }
        private void AddData_CurrentPlayer(Player currentPlayer)
        {
            if (currentPlayer == Player.White)
            {
                stringbuilder.Append(" w");
            }
            else
            {
                stringbuilder.Append(" b");
            }
        }
        private void AddData_Castling(Board board)
        {
            bool castlingKingSide_White = board.RightToCastleKingside(Player.White);
            bool castlingQueenSide_White = board.RightToCastleQueenside(Player.White);
            bool castlingKingSide_Black = board.RightToCastleKingside(Player.Black);
            bool castlingQueenSide_Black = board.RightToCastleQueenside(Player.Black);
            if (!(castlingKingSide_White || castlingQueenSide_White || castlingKingSide_Black || castlingQueenSide_Black))
            {
                stringbuilder.Append("-");
                return;
            }
            if (castlingKingSide_White)
            {
                stringbuilder.Append("K");
            }
            if (castlingQueenSide_White)
            {
                stringbuilder.Append("Q");
            }
            if (castlingKingSide_Black)
            {
                stringbuilder.Append("k");
            }
            if (castlingQueenSide_Black)
            {
                stringbuilder.Append("q");
            }
        }
        private void AddData_EnPassant(Board board, Player currentPlayer)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                stringbuilder.Append("-");
                return;
            }
            Position position = board.RetrievePawnJumpPositions(currentPlayer.Opponent());
            char file = (char)('a' + position.Column);
            int rank = 8 - position.Row;
            stringbuilder.Append(file);
            stringbuilder.Append(rank);
        }
        public override string ToString()
        {
            return stringbuilder.ToString();
        }
    }
}
