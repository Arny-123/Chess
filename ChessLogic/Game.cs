namespace ChessLogic
{
    internal class Game
    {
        private Func<Board> simulatedBoard;
        private Board simulatedBoard1;

        public Game(Func<Board> simulatedBoard)
        {
            this.simulatedBoard = simulatedBoard;
        } //this is a delegate that will be used to create a new board when needed

        public Game(Board simulatedBoard1)
        {
            this.simulatedBoard1 = simulatedBoard1;
        } //this is a constructor that will be used to create a new board when needed

        internal IEnumerable<MovementBaseClass> LegalMoves(Position nextPosition, int v)
        {
            throw new NotImplementedException();
        } //this method will return all the legal moves that a piece can make
    }
}