namespace ChessEngine;

public class LegalMoveGenerator
{
    public List<Move> GenerateLegalMoves(GameState gameState)
    {
        var legalMoves = new List<Move>();

        legalMoves.AddRange(GeneratePawnMoves(gameState, legalMoves));

        return legalMoves;
    }

    private void bitBoardPosition(ulong bitboard)
    {
        for (var i = 0; i < 64; i++)
        {
            var square = 1UL << i;
            if ((bitboard & square) != 0) Console.WriteLine((char)('a' + i % 8) + (8 - i / 8).ToString());
        }
    }

    private List<Move> GeneratePawnMoves(GameState gameState, List<Move> legalMoves)
    {
        var board = gameState.Board;

        var pawnBitboard = board.Bitboards[gameState.Turn, 0];

        // Pawns can move forward one square if the square is empty
        var moves = (pawnBitboard >> 8) & board.EmptySquares();
        bitBoardPosition(moves);

        // Pawns on the second rank (from the bottom) can also move forward two squares if those squares are empty
        // ulong pawnsOnSecondRank = pawnBitboard & 1UL << 8;
        // moves |= (pawnsOnSecondRank << 16) & board.EmptySquares() & (board.EmptySquares() >> 8);

        // A list to hold the moves
        List<Move> pawnMoves = new();

        while (moves != 0)
        {
            var move = moves & ~moves;
            var to = board.BitScanForward(move);
            var from = to - 8;
            pawnMoves.Add(new Move(from, to));
            moves &= moves - 1;
        }

        return pawnMoves;
    }
}