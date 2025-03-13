using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Direction
    {
        //the different possible directions pieces can move in
        public readonly static Direction North = new Direction(-1, 0); 
        public readonly static Direction South = new Direction(1, 0);
        public readonly static Direction East = new Direction(0, 1);
        public readonly static Direction West = new Direction(0, -1);
        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = North + West;
        public readonly static Direction SouthEast = South + East;
        public readonly static Direction SouthWest = South + West;
        public int ChangeInRowNum { get; }
        public int ChangeInColumnNum { get; }
        //store how much a piece can move in a given direction
        public Direction (int numChangeRow, int numChangeColumn)//constructor
        {
            ChangeInRowNum = numChangeRow;
            ChangeInColumnNum = numChangeColumn;
        } 

        //'overwriting' the plus and multiply commands to increase efficiency later on for piece movements
        public static Direction operator + (Direction dir1, Direction dir2) //overloading the + operator
        {
            return new Direction(dir1.ChangeInRowNum + dir2.ChangeInRowNum, dir1.ChangeInColumnNum + dir2.ChangeInColumnNum);
        } 

        public static Direction operator * (int scalar, Direction dir)
        {
            return new Direction (scalar * dir.ChangeInRowNum, scalar * dir.ChangeInColumnNum);
        } //overloading the * operator
    }
}
