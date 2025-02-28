namespace ChessLogic
{
    internal class Game
    {
        private Func<Board> simulatedBoard;
        private Board simulatedBoard1;

        public Game(Func<Board> simulatedBoard)
        {
            this.simulatedBoard = simulatedBoard;
        }
      
        public Game(Board simulatedBoard1)
        {
            this.simulatedBoard1 = simulatedBoard1;
        }

        internal IEnumerable<MovementBaseClass> LegalMoves(Position nextPosition, int v)
        {
            throw new NotImplementedException();
        }
    }
}