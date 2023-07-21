using System.Collections.Generic;

namespace War
{
    public class General : Piece
    {
        #region Public Methods

        public override List<Square> GetMoves(Board board)
        {
            var moves = new List<Square>();
            var pos = _square.BoardPosition;

            foreach (var direction in Point.GetAllDirections())
            {
                var curr = pos + direction;
                if (board.TryGetSquare(curr, out var potentialMove))
                {
                    if (!potentialMove.Occupied)
                    {
                        moves.Add(potentialMove);
                    }
                    else if (potentialMove.Piece.Team != Team)
                    {
                        moves.Add(potentialMove);
                    }
                }
            }

            return moves;
        }

        public override void Move(Board board, Square move)
        {
            OccupySquare(move);
        }

        #endregion

        #region Private Methods

        protected override string GetSpriteName() { return "General"; }

        #endregion
    }
}