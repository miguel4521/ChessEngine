namespace ChessEngine.Bitboards.Pieces;

public class Rook : Piece
{
    private static readonly Bitboard[] RookMagics =
    {
        0x8a80104000800020,
        0x140002000100040,
        0x2801880a0017001,
        0x100081001000420,
        0x200020010080420,
        0x3001c0002010008,
        0x8480008002000100,
        0x2080088004402900,
        0x800098204000,
        0x2024401000200040,
        0x100802000801000,
        0x120800800801000,
        0x208808088000400,
        0x2802200800400,
        0x2200800100020080,
        0x801000060821100,
        0x80044006422000,
        0x100808020004000,
        0x12108a0010204200,
        0x140848010000802,
        0x481828014002800,
        0x8094004002004100,
        0x4010040010010802,
        0x20008806104,
        0x100400080208000,
        0x2040002120081000,
        0x21200680100081,
        0x20100080080080,
        0x2000a00200410,
        0x20080800400,
        0x80088400100102,
        0x80004600042881,
        0x4040008040800020,
        0x440003000200801,
        0x4200011004500,
        0x188020010100100,
        0x14800401802800,
        0x2080040080800200,
        0x124080204001001,
        0x200046502000484,
        0x480400080088020,
        0x1000422010034000,
        0x30200100110040,
        0x100021010009,
        0x2002080100110004,
        0x202008004008002,
        0x20020004010100,
        0x2048440040820001,
        0x101002200408200,
        0x40802000401080,
        0x4008142004410100,
        0x2060820c0120200,
        0x1001004080100,
        0x20c020080040080,
        0x2935610830022400,
        0x44440041009200,
        0x280001040802101,
        0x2100190040002085,
        0x80c0084100102001,
        0x4024081001000421,
        0x20030a0244872,
        0x12001008414402,
        0x2006104900a0804,
        0x1004081002402
    };
    
    private static readonly int[] RookRelevantBits =
    {
        12, 11, 11, 11, 11, 11, 11, 12,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        11, 10, 10, 10, 10, 10, 10, 11,
        12, 11, 11, 11, 11, 11, 11, 12
    };

    private static readonly Bitboard[] RookMasks = new Bitboard[64];

    private static readonly Bitboard[,] RookAttacks = new Bitboard[64, 4096];

    private static Bitboard MaskRookAttacks(int square)
    {
        // result attacks bitboard
        Bitboard attacks = 0L;

        // init ranks & files
        int r, f;

        // init target rank & files
        int tr = square / 8;
        int tf = square % 8;

        // mask relevant rook occupancy bits
        for (r = tr + 1; r <= 6; r++) attacks |= (1UL << (r * 8 + tf));
        for (r = tr - 1; r >= 1; r--) attacks |= (1UL << (r * 8 + tf));
        for (f = tf + 1; f <= 6; f++) attacks |= (1UL << (tr * 8 + f));
        for (f = tf - 1; f >= 1; f--) attacks |= (1UL << (tr * 8 + f));

        // return attack map
        return attacks;
    }

    // generate rook attacks on the fly
    private static Bitboard RookAttacksOnTheFly(int square, Bitboard block)
    {
        // result attacks bitboard
        Bitboard attacks = 0UL;

        // init ranks & files
        int r, f;

        // init target rank & files
        int tr = square / 8;
        int tf = square % 8;

        // generate rook attacks
        for (r = tr + 1; r <= 7; r++)
        {
            attacks |= (1UL << (r * 8 + tf));
            if (((1UL << (r * 8 + tf)) & block) != 0) break;
        }

        for (r = tr - 1; r >= 0; r--)
        {
            attacks |= (1UL << (r * 8 + tf));
            if (((1UL << (r * 8 + tf)) & block) != 0) break;
        }

        for (f = tf + 1; f <= 7; f++)
        {
            attacks |= (1UL << (tr * 8 + f));
            if (((1UL << (tr * 8 + f)) & block) != 0) break;
        }

        for (f = tf - 1; f >= 0; f--)
        {
            attacks |= (1UL << (tr * 8 + f));
            if (((1UL << (tr * 8 + f)) & block) != 0) break;
        }

        // return attack map
        return attacks;
    }

// init slider piece's attack tables
    public static void InitRookAttacks()
    {
        // loop over 64 board squares
        for (int square = 0; square < 64; square++)
        {
            // init bishop & rook masks
            RookMasks[square] = MaskRookAttacks(square);

            // init current mask
            Bitboard attackMask = RookMasks[square];

            // init relevant occupancy bit count
            int relevantBitsCount = attackMask.Count();

            // init occupancy indices
            int occupancyIndices = 1 << relevantBitsCount;

            // loop over occupancy indices
            for (int index = 0; index < occupancyIndices; index++)
            {
                
                    // init current occupancy variation
                    Bitboard occupancy = SetOccupancy(index, relevantBitsCount, attackMask);

                    // init magic index
                    Bitboard magicIndex = (occupancy * RookMagics[square]) >> (64 - RookRelevantBits[square]);

                    // init bishop attacks
                    RookAttacks[square, magicIndex] = RookAttacksOnTheFly(square, occupancy);
            }
        }
    }
    
    private Bitboard GetRookAttacks(int square, Bitboard occupancy)
    {
        // get bishop attacks assuming current board occupancy
        occupancy &= RookMasks[square];
        occupancy *= RookMagics[square];
        occupancy >>= 64 - RookRelevantBits[square];

        // return bishop attacks
        return RookAttacks[square, occupancy];
    }

    public Rook(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000081UL : 0x8100000000000000UL;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>();
        int startSquare = LSB();
        Bitboard rookAttacks =
            GetRookAttacks(startSquare, ~position.GetEmptySquares()) & ~position.GetWhitePieces();

        Bitboard captures = rookAttacks & (IsWhite ? position.GetBlackPieces() : position.GetWhitePieces());
        Bitboard nonCaptures = rookAttacks & ~captures;

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