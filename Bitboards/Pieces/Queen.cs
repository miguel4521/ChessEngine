namespace ChessEngine.Bitboards.Pieces;

public class Queen : SlidingPiece
{
    public Queen(bool isWhite)
    {
        IsWhite = isWhite;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>();
        int startSquare = LSB();

        Bitboard rookAttacks =
            GetRookAttacks(startSquare, position.GetOccupiedSquares()) & ~position.GetWhitePieces();
        Bitboard bishopAttacks =
            GetBishopAttacks(startSquare, position.GetOccupiedSquares()) & ~position.GetWhitePieces();
       
        // Queen is just a combination of a bishop and a rook
        Bitboard attacks = rookAttacks | bishopAttacks;
        
        Bitboard captures = attacks & (IsWhite ? position.GetBlackPieces() : position.GetWhitePieces());
        Bitboard nonCaptures = attacks & ~captures;

        while (captures != 0)
        {
            int captureSquare = captures.LSB();
            moves.Add(new Move(startSquare, captureSquare, position));
            captures ^= 1UL << captureSquare;
        }

        while (nonCaptures != 0)
        {
            int nonCaptureSquare = nonCaptures.LSB();
            moves.Add(new Move(startSquare, nonCaptureSquare, position));
            nonCaptures ^= 1UL << nonCaptureSquare;
        }

        return moves;
    }
    
    public override char GetSymbol()
    {
        return IsWhite ? 'q' : 'Q';
    }
}