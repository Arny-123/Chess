using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class PieceCounting
    {
        private readonly Dictionary<PieceType, int> whiteCount = new();
        private readonly Dictionary<PieceType, int> blackCount = new();
        public int totalCount { get; private set; }
        public PieceCounting()
        {

            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                whiteCount[type] = 0;
                blackCount[type] = 0;
            }
        }
        public void Increase(Player colour, PieceType pieceType)
        {
            if (colour == Player.White)
            {
                whiteCount[pieceType]++;
            }
            else if (colour == Player.Black)
            {
                blackCount[pieceType]++;
            }
            totalCount++;
        }
        public int White(PieceType pieceType)
        {
            return whiteCount[pieceType];
        }
        public int Black(PieceType pieceType)
        {
            return blackCount[pieceType];
        }
    }
}
