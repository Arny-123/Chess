using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
        private readonly Dictionary<Player, Position> pawnJumpMoves = new Dictionary<Player, Position>
        {
            { Player.White, null },
            { Player.Black, null}
        };
        public Piece this[int row, int column]
        {
            get { return pieces[row, column]; }
            set { pieces[row, column] = value; }
        }
        public Piece this[Position position]
        {
            get { return this[position.Row, position.Column]; }
            set { this[position.Row, position.Column] = value; }
        }

        public static Board Initially() //creates a new board at the beginning of the game
        {
            Board board = new Board();
            board.AddInitialPieces();
            return board;
        }

        private void AddInitialPieces() //adds pieces onto board at beginning of game
        {
            Player[] players = { Player.Black, Player.White };
            int[] backRowPositions = { 0, 7 }; //row indices for Black and White back rows.
            int[] pawnRows = { 1, 6 }; //row indices for Black and White pawn rows.
            //define the order of pieces in the back row.
            Type[] backRowPieces = {
                                        typeof(Rook), typeof(Knight), typeof(Bishop),
                                        typeof(Queen), typeof(King), typeof(Bishop),
                                        typeof(Knight), typeof(Rook)
            };
            //loop through each player (Black and White).
            for (int playerIndex = 0; playerIndex < 2; playerIndex++)
            {
                Player currentPlayer = players[playerIndex];
                //set up the back row.
                for (int col = 0; col < 8; col++)
                {
                    //use reflection to create instances of the pieces.
                    this[backRowPositions[playerIndex], col] =
                        (Piece)Activator.CreateInstance(backRowPieces[col], currentPlayer);
                }
                //set up the pawns.
                for (int col = 0; col < 8; col++)
                {
                    this[pawnRows[playerIndex], col] = new Pawn(currentPlayer);
                }
            }
        }
        public static bool IsInside(Position position) //checks to see if position is inside board
        {
            bool isRowValid = position.Row >= 0 && position.Row < 8;//check if the row is within the valid range
            bool isColumnValid = position.Column >= 0 && position.Column < 8;//check if the column is within the valid range 
            if (isRowValid && isColumnValid)
            {
                return true;//return true only if both row and column are valid
            }
            else
            {
                return false;
            }
        }
        public bool IsEmpty(Position position) //cecks to see if cell is empty
        {
            return this[position] == null;
        }
        public List<Position> PositionsOfPiece() //returns piece positions
        {
            List<Position> positions = new List<Position>();//create a list for all positions
            for (int row = 0; row < 8; row++)//iterate over all rows and columns
            {
                for (int column = 0; column < 8; column++)
                {
                    Position position = new Position(row, column);//create a new position for the current row and column

                    if (!IsEmpty(position))//if position is not empty
                    {
                        positions.Add(position);//add the position to the list
                    }
                }
            }
            return positions;//return positions
        }
        public List<Position> PiecePositionsFor(Player player) //creates a list of positions for the pieces of a player
        {
            List<Position> playerPiecePositions = new List<Position>();//create a list to store positions of pieces belonging to the specified player
            List<Position> allPositions = PositionsOfPiece().ToList();//get all positions of pieces on the board.  
            foreach (Position position in allPositions)//iterate through each position.
            {
                if (this[position].Colour == player)//check if the piece at the current position belongs to the specified player.
                {
                    playerPiecePositions.Add(position);//add the position to the player's piece positions list.
                }
            }
            return playerPiecePositions;//return the list of positions for the player's pieces.
        }
        public bool InCheck(Player player) //checks to see if player is in check
        {
            var opponentPositions = PiecePositionsFor(player.Opponent());//get all positions for the opponent's pieces

            foreach (var position in opponentPositions)//iterate through each opponent position
            {
                Piece piece = this[position];//get the piece at the current position
                if (piece.AbleToCaptureOpponentsKing(position, this))//check if the piece can capture the player's king
                {
                    return true;//if any piece can capture the king, the player is in check
                }
            }
            return false;//if no piece can capture the king, the player is not in check
        }
        public Board Copy()//creates a copy of board to help in finding illegal moves
        {
            Board copy = new Board();
            foreach (Position position in PositionsOfPiece())
            {
                copy[position] = this[position].Copy();
            }
            return copy;
        }
        public Position RetrievePawnJumpPositions(Player player)
        {
            return pawnJumpMoves[player];
        }
        public void SetPawnJumpPositions(Player player, Position position)
        {
            pawnJumpMoves[player] = position;
        }
        public PieceCounting CountPieces()
        {
            PieceCounting pieceCount = new PieceCounting();
            foreach (Position position in PositionsOfPiece())
            {
                Piece piece = this[position];
                pieceCount.Increase(piece.Colour, piece.Type);
                // ^^increment the count of the piece type for the player
            }
            return pieceCount;
        } //counts the number of pieces on the board for each player
        public bool InsufficientMaterial()
        {
            PieceCounting pieceCount = CountPieces();
            return OnlyTwoKings(pieceCount) ||
                   OnlyKingsAndBishop(pieceCount) ||
                   OnlyKingsAndKnight(pieceCount) ||
                   OnlyKingsAndBishops(pieceCount);
        }
        private static bool OnlyTwoKings(PieceCounting pieceCount)
        {
            return pieceCount.totalCount == 2;
        } //checks if there are only two kings on the board
        private static bool OnlyKingsAndBishop(PieceCounting pieceCount)
        {
            return pieceCount.totalCount == 3 && (pieceCount.White(PieceType.Bishop) == 1 || pieceCount.Black(PieceType.Bishop) == 1);
        } //checks if there are only two kings and two bishops on the board
        private static bool OnlyKingsAndKnight(PieceCounting pieceCount)
        {
            return pieceCount.totalCount == 3 && (pieceCount.White(PieceType.Knight) == 1 || pieceCount.Black(PieceType.Knight) == 1);
        } //checks if there are only two kings and two knights on the board
        private bool OnlyKingsAndBishops(PieceCounting pieceCount)
        {
            if (pieceCount.totalCount != 4)
            {
                return false;
            }
            if (pieceCount.White(PieceType.Bishop) != 1 || pieceCount.Black(PieceType.Bishop) != 1)
            {
                return false;
            }
            Position whiteBishopPosition = FindPiece(Player.White, PieceType.Bishop);
            Position blackBishopPosition = FindPiece(Player.Black, PieceType.Bishop);
            return whiteBishopPosition.CellColour() == blackBishopPosition.CellColour();
        } //checks if there are only two kings and two bishops on the board
        private Position FindPiece(Player colour, PieceType type)
        {
            return (PiecePositionsFor(colour).First(position => this[position].Type == type));
        }
        private bool KingAndRookHaveNotMoved(Position kingPosition, Position rookPosition)
        {
            if (IsEmpty(kingPosition) || IsEmpty(rookPosition))
            {
                return false;
            }
            Piece king = this[kingPosition];
            Piece rook = this[rookPosition];
            return king.Type == PieceType.King && !king.Moved &&
                   rook.Type == PieceType.Rook && !rook.Moved;
        }
        public bool RightToCastleKingside(Player player)
        {
            return player switch
            {
                Player.White => KingAndRookHaveNotMoved(new Position(7, 4), new Position(7, 7)),
                Player.Black => KingAndRookHaveNotMoved(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }
        public bool RightToCastleQueenside(Player player)
        {
            return player switch
            {
                Player.White => KingAndRookHaveNotMoved(new Position(7, 4), new Position(7, 0)),
                Player.Black => KingAndRookHaveNotMoved(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        } //checks if the player has the right to castle queenside
        public bool CanCaptureEnPassant(Player player)
        {
            Position SkipPosition = RetrievePawnJumpPositions(player.Opponent());
            if (SkipPosition == null)
            {
                return false;
            }
            Position[] pawnPositions = player switch
            {
                Player.White => new Position[] { SkipPosition + Direction.SouthWest, SkipPosition + Direction.SouthEast },
                Player.Black => new Position[] { SkipPosition + Direction.NorthWest, SkipPosition + Direction.NorthEast },
                _ => Array.Empty<Position>()
            };
            return PawnInPosition(player, pawnPositions, SkipPosition);
        }
        private bool PawnInPosition(Player player, Position[] pawnPosition, Position SkipPositions)
        {
            foreach (Position position in pawnPosition.Where(IsInside))
            {
                Piece piece = this[position];
                if (piece == null || piece.Type != PieceType.Pawn || piece.Colour != player)
                {
                    continue;
                }
                EnPassant enpassantmove = new EnPassant(position, SkipPositions);
                if (enpassantmove.Legal(this))
                {
                    return true;
                }
            }
            return false;
        }
    }
}