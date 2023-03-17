namespace ChessEngine.Bitboards.Pieces;

public abstract class SlidingPiece : Piece
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
    
    private static void PopBit(ref Bitboard bitboard, int square)
    {
        bitboard &= ~(1UL << square);
    }
    
    private static Bitboard SetOccupancy(int index, int bitsInMask, Bitboard attackMask)
    {
        // occupancy map
        ulong occupancy = 0UL;

        // loop over the range of bits within attack mask
        for (int count = 0; count < bitsInMask; count++)
        {
            // get LS1B index of attacks mask
            int square = attackMask.LSB();

            // pop LS1B in attack map
            PopBit(ref attackMask, square);

            // make sure occupancy is on board
            if ((index & (1 << count)) != 0)
                // populate occupancy map
                occupancy |= 1UL << square;
        }

        // return occupancy map
        return occupancy;
    }

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
    private static void InitSlidingAttacks(bool isBishop)
    {
        // loop over 64 board squares
        for (int square = 0; square < 64; square++)
        {
            // init bishop & rook masks
            BishopMasks[square] = MaskBishopAttacks(square);
            RookMasks[square] = MaskRookAttacks(square);

            // init current mask
            Bitboard attackMask = isBishop ? BishopMasks[square] : RookMasks[square];

            // init relevant occupancy bit count
            int relevantBitsCount = attackMask.Count();

            // init occupancy indices
            int occupancyIndices = 1 << relevantBitsCount;

            // loop over occupancy indices
            for (int index = 0; index < occupancyIndices; index++)
            {
                if (isBishop)
                {
                    // init current occupancy variation
                    Bitboard occupancy = SetOccupancy(index, relevantBitsCount, attackMask);

                    // init magic index
                    Bitboard magicIndex = (occupancy * BishopMagics[square]) >> (64 - BishopRelevantBits[square]);

                    // init bishop attacks
                    BishopAttacks[square, magicIndex] = BishopAttacksOnTheFly(square, occupancy);
                }
                else
                {
                    // init current occupancy variation
                    Bitboard occupancy = SetOccupancy(index, relevantBitsCount, attackMask);

                    // init magic index
                    Bitboard magicIndex = (occupancy * RookMagics[square]) >> (64 - RookRelevantBits[square]);

                    // init rook attacks
                    RookAttacks[square, magicIndex] = RookAttacksOnTheFly(square, occupancy);
                }
            }
        }
    }
    
    public static void InitAttacks() {
        // init bishop & rook attack tables
        InitSlidingAttacks(true);
        InitSlidingAttacks(false);
    }

    protected Bitboard GetBishopAttacks(int square, Bitboard occupancy)
    {
        // get bishop attacks assuming current board occupancy
        occupancy &= BishopMasks[square];
        occupancy *= BishopMagics[square];
        occupancy >>= 64 - BishopRelevantBits[square];

        // return bishop attacks
        return BishopAttacks[square, occupancy];
    }
    
    protected Bitboard GetRookAttacks(int square, Bitboard occupancy)
    {
        // get bishop attacks assuming current board occupancy
        occupancy &= RookMasks[square];
        occupancy *= RookMagics[square];
        occupancy >>= 64 - RookRelevantBits[square];

        // return bishop attacks
        return RookAttacks[square, occupancy];
    }
}