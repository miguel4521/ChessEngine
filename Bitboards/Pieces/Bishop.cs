namespace ChessEngine.Bitboards.Pieces;

public class Bishop : Piece
{
    private static readonly Bitboard[] BishopMagics =
    {
        0x40040844404084UL,
        0x2004208a004208UL,
        0x10190041080202UL,
        0x108060845042010UL,
        0x581104180800210UL,
        0x2112080446200010UL,
        0x1080820820060210UL,
        0x3c0808410220200UL,
        0x4050404440404UL,
        0x21001420088UL,
        0x24d0080801082102UL,
        0x1020a0a020400UL,
        0x40308200402UL,
        0x4011002100800UL,
        0x401484104104005UL,
        0x801010402020200UL,
        0x400210c3880100UL,
        0x404022024108200UL,
        0x810018200204102UL,
        0x4002801a02003UL,
        0x85040820080400UL,
        0x810102c808880400UL,
        0xe900410884800UL,
        0x8002020480840102UL,
        0x220200865090201UL,
        0x2010100a02021202UL,
        0x152048408022401UL,
        0x20080002081110UL,
        0x4001001021004000UL,
        0x800040400a011002UL,
        0xe4004081011002UL,
        0x1c004001012080UL,
        0x8004200962a00220UL,
        0x8422100208500202UL,
        0x2000402200300c08UL,
        0x8646020080080080UL,
        0x80020a0200100808UL,
        0x2010004880111000UL,
        0x623000a080011400UL,
        0x42008c0340209202UL,
        0x209188240001000UL,
        0x400408a884001800UL,
        0x110400a6080400UL,
        0x1840060a44020800UL,
        0x90080104000041UL,
        0x201011000808101UL,
        0x1a2208080504f080UL,
        0x8012020600211212UL,
        0x500861011240000UL,
        0x180806108200800UL,
        0x4000020e01040044UL,
        0x300000261044000aUL,
        0x802241102020002UL,
        0x20906061210001UL,
        0x5a84841004010310UL,
        0x4010801011c04UL,
        0xa010109502200UL,
        0x4a02012000UL,
        0x500201010098b028UL,
        0x8040002811040900UL,
        0x28000010020204UL,
        0x6000020202d0240UL,
        0x8918844842082200UL,
        0x4010011029020020UL
    };

    private static readonly int[] BishopRelevantBits =
    {
        6, 5, 5, 5, 5, 5, 5, 6,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 7, 7, 7, 7, 5, 5,
        5, 5, 7, 9, 9, 7, 5, 5,
        5, 5, 7, 9, 9, 7, 5, 5,
        5, 5, 7, 7, 7, 7, 5, 5,
        5, 5, 5, 5, 5, 5, 5, 5,
        6, 5, 5, 5, 5, 5, 5, 6
    };

    private static readonly Bitboard[] BishopMasks = new Bitboard[64];

    private static readonly Bitboard[,] BishopAttacks = new Bitboard[64, 512];

    private static Bitboard MaskBishopAttacks(int square)
    {
        // result attacks bitboard
        ulong attacks = 0UL;

        // init ranks & files
        int r, f;

        // init target rank & files
        int tr = square / 8;
        int tf = square % 8;

        // mask relevant bishop occupancy bits
        for (r = tr + 1, f = tf + 1; r <= 6 && f <= 6; r++, f++) attacks |= (1UL << (r * 8 + f));
        for (r = tr - 1, f = tf + 1; r >= 1 && f <= 6; r--, f++) attacks |= (1UL << (r * 8 + f));
        for (r = tr + 1, f = tf - 1; r <= 6 && f >= 1; r++, f--) attacks |= (1UL << (r * 8 + f));
        for (r = tr - 1, f = tf - 1; r >= 1 && f >= 1; r--, f--) attacks |= (1UL << (r * 8 + f));

        // return attack map
        return attacks;
    }

    private static Bitboard BishopAttacksOnTheFly(int square, Bitboard block)
    {
        // result attacks bitboard
        ulong attacks = 0UL;

        // init ranks & files
        int r, f;

        // init target rank & files
        int tr = square / 8;
        int tf = square % 8;

        // generate bishop attacks
        for (r = tr + 1, f = tf + 1; r <= 7 && f <= 7; r++, f++)
        {
            attacks |= (1UL << (r * 8 + f));
            if (((1UL << (r * 8 + f)) & block) != 0) break;
        }

        for (r = tr - 1, f = tf + 1; r >= 0 && f <= 7; r--, f++)
        {
            attacks |= (1UL << (r * 8 + f));
            if (((1UL << (r * 8 + f)) & block) != 0) break;
        }

        for (r = tr + 1, f = tf - 1; r <= 7 && f >= 0; r++, f--)
        {
            attacks |= (1UL << (r * 8 + f));
            if (((1UL << (r * 8 + f)) & block) != 0) break;
        }

        for (r = tr - 1, f = tf - 1; r >= 0 && f >= 0; r--, f--)
        {
            attacks |= (1UL << (r * 8 + f));
            if (((1UL << (r * 8 + f)) & block) != 0) break;
        }

        // return attack map
        return attacks;
    }
    
    // init slider piece's attack tables
    public static void InitBishopAttacks()
    {
        // loop over 64 board squares
        for (int square = 0; square < 64; square++)
        {
            // init bishop & rook masks
            BishopMasks[square] = MaskBishopAttacks(square);

            // init current mask
            Bitboard attackMask = BishopMasks[square];

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
                Bitboard magicIndex = (occupancy * BishopMagics[square]) >> (64 - BishopRelevantBits[square]);

                // init bishop attacks
                BishopAttacks[square, magicIndex] = BishopAttacksOnTheFly(square, occupancy);
            }
        }
    }

    private Bitboard GetBishopAttacks(int square, Bitboard occupancy)
    {
        // get bishop attacks assuming current board occupancy
        occupancy &= BishopMasks[square];
        occupancy *= BishopMagics[square];
        occupancy >>= 64 - BishopRelevantBits[square];

        // return bishop attacks
        return BishopAttacks[square, occupancy];
    }

    public Bishop(bool isWhite)
    {
        IsWhite = isWhite;
        bits = isWhite ? 0x0000000000000024UL : 0x2400000000000000UL;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>();
        int startSquare = LSB();
        Bitboard bishopAttacks =
            GetBishopAttacks(startSquare, ~position.GetEmptySquares()) & ~position.GetWhitePieces();

        Bitboard captures = bishopAttacks & (IsWhite ? position.GetBlackPieces() : position.GetWhitePieces());
        Bitboard nonCaptures = bishopAttacks & ~captures;

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