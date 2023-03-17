namespace ChessEngine.Bitboards.Pieces;

public class Queen : SlidingPiece
{
    public Queen(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000008UL : 0x0800000000000000UL;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>();
        int startSquare = LSB();

        Bitboard rookAttacks =
            GetRookAttacks(startSquare, position.GetOccupiedSquares()) & ~position.GetWhitePieces();
        Bitboard bishopAttacks =
            GetBishopAttacks(startSquare, position.GetOccupiedSquares()) & ~position.GetWhitePieces();
       
        Bitboard attacks = rookAttacks | bishopAttacks;
        
        Console.WriteLine(attacks.ToString());

        Bitboard captures = attacks & (IsWhite ? position.GetBlackPieces() : position.GetWhitePieces());
        Bitboard nonCaptures = attacks & ~captures;

        while (captures != 0)
        {
            int captureSquare = captures.LSB();
            moves.Add(new Move(startSquare, captureSquare));
            captures ^= 1UL << captureSquare;
        }

        while (nonCaptures != 0)
        {
            int nonCaptureSquare = nonCaptures.LSB();
            moves.Add(new Move(startSquare, nonCaptureSquare));
            nonCaptures ^= 1UL << nonCaptureSquare;
        }

        return moves;
    }
}