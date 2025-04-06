using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Castling : MovementBaseClass
    {
        public override MovementType Type { get; }
        public override Position StartingPos { get; }
        public override Position EndingPos { get; }
        private readonly Direction KingMovement;
        private readonly Position RookStartingPosition;
        private readonly Position RookEndingPosition;
        public Castling(MovementType type, Position KingPosition)
        {
            Type = type;
            StartingPos = KingPosition;

            if (type == MovementType.CastleKing)
            {
                EndingPos = new Position(KingPosition.Row, KingPosition.Column + 2);
                KingMovement = Direction.East;
                RookStartingPosition = new Position(KingPosition.Row, 7);
                RookEndingPosition = new Position(KingPosition.Row, 5);
            }
            else
            {
                EndingPos = new Position(KingPosition.Row, KingPosition.Column - 2);
                KingMovement = Direction.West;
                RookStartingPosition = new Position(KingPosition.Row, 0);
                RookEndingPosition = new Position(KingPosition.Row, 3);
            }
        }
        public override bool ApplyMove(Board board)
        {
            new RegularMove(StartingPos, EndingPos).ApplyMove(board);
            new RegularMove(RookStartingPosition, RookEndingPosition).ApplyMove(board);
            return false;
        }
        public override bool Legal(Board board)
        {
            Player player = board[StartingPos].Colour;
            if (board.InCheck(player))
            {
                return false;
            }
            Board boardCopy = board.Copy();
            Position copiedKingPosition = StartingPos;
            for (int i = 0; i < 2; i++)
            {
                new RegularMove(copiedKingPosition, copiedKingPosition + KingMovement).ApplyMove(boardCopy);
                copiedKingPosition += KingMovement;
                if (boardCopy.InCheck(player))
                {
                    return false;
                }
            }
            return true;
        }


    }
}
