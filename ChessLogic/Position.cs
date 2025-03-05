using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Position
    {
        public int Row { get; set; } //inc top -> bottom
        public int Column { get; } //inc left -> right

        public Position(int row, int column)
        {  
            Row = row; 
            Column = column;
        } //constructor

        public Player CellColour () //returns the colour of the square that piece is on
        {
            if ((Row + Column ) % 2 != 0)
            { 
                return Player.Black;
            }
            return Player.White;
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public static int CustomHash(int value)
        {
            unchecked
            {
                value = (value ^ 79 ) ^ (value >> 16); //XOR followed by right shift of 16
                value = value + (value << 4); //left shift 4
                value = value ^ (value >> 8); //right shift 8
                value = (int)(value * 0x9e3779b9); 
                value = value ^ (value >> 16);
                return value;
            }
        }
        public override int GetHashCode()//allows use of == and != to compare positions
        {
            unchecked
            {
                int hash = 17; //using prime numbers reduces chances of hash collisions.
                hash = hash * 23 + CustomHash(Row); 
                hash = hash * 23 + CustomHash(Column); 
                return hash;
            }
        }
        
        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public static Position operator +(Position pos,Direction dir)
        {
            return new Position(pos.Row +dir.ChangeInRowNum, pos.Column +dir.ChangeInColumnNum);
        }
       
    }
}
