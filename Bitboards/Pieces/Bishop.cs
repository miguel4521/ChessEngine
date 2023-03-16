using System.Numerics;

namespace ChessEngine;

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

    private Bitboard MaskBishopAttacks(int square)
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

    private Bitboard BishopAttacksOnTheFly(int square, Bitboard block)
    {
        // result attacks bitboard
        ulong attacks = 0UL;

        // init ranks & files
        int r, f;

        // init target rank & files
        int tr = square / 8;
        int tf = square % 8;

        // generate bishop atacks
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

    private Bitboard SetOccupancy(int index, int bitsInMask, Bitboard attackMask)
    {
        // occupancy map
        ulong occupancy = 0UL;

        // loop over the range of bits within attack mask
        for (int count = 0; count < bitsInMask; count++)
        {
            // get LS1B index of attacks mask
            int square = attackMask.LSB();

            // pop LS1B in attack map
            attackMask &= attackMask - 1;

            // make sure occupancy is on board
            if ((index & (1 << count)) != 0)
                // populate occupancy map
                occupancy |= (1UL << square);
        }

        // return occupancy map
        return occupancy;
    }

    private uint randomState = 1804289383;

    private uint GetRandomU32Number()
    {
        // get current state
        uint number = randomState;

        // XOR shift algorithm
        number ^= number << 13;
        number ^= number >> 17;
        number ^= number << 5;

        // update random number state
        randomState = number;

        // return random number
        return number;
    }


    private ulong GetRandomU64Number()
    {
        // define 4 random numbers
        ulong n1, n2, n3, n4;

        // init random numbers slicing 16 bits from MS1B side
        n1 = (ulong)(GetRandomU32Number()) & 0xFFFF;
        n2 = (ulong)(GetRandomU32Number()) & 0xFFFF;
        n3 = (ulong)(GetRandomU32Number()) & 0xFFFF;
        n4 = (ulong)(GetRandomU32Number()) & 0xFFFF;

        // return random number
        return n1 | (n2 << 16) | (n3 << 32) | (n4 << 48);
    }

    // generate magic number candidate
    Bitboard GenerateMagicNumber()
    {
        return GetRandomU64Number() & GetRandomU64Number() & GetRandomU64Number();
    }


    private ulong FindMagicNumber(int square, int relevantBits)
    {
        // init occupancies
        ulong[] occupancies = new ulong[4096];

        // init attack tables
        ulong[] attacks = new ulong[4096];

        // init used attacks
        ulong[] usedAttacks = new ulong[4096];

        // init attack mask for a current piece
        ulong attackMask = MaskBishopAttacks(square);

        // init occupancy indices
        int occupancyIndices = 1 << relevantBits;

        // loop over occupancy indices
        for (int index = 0; index < occupancyIndices; index++)
        {
            // init occupancies
            occupancies[index] = SetOccupancy(index, relevantBits, attackMask);

            // init attacks
            attacks[index] = BishopAttacksOnTheFly(square, occupancies[index]);
        }

        // test magic numbers loop
        for (int randomCount = 0; randomCount < 100000000; randomCount++)
        {
            // generate magic number candidate
            ulong magicNumber = GenerateMagicNumber();

            // skip inappropriate magic numbers
            Bitboard test = (attackMask * magicNumber) & 0xFF00000000000000UL;
            if (test.Count() < 6) continue;

            // init used attacks
            Array.Clear(usedAttacks, 0, usedAttacks.Length);

            // init index & fail flag
            int index;
            bool fail;

            // test magic index loop
            for (index = 0, fail = false; !fail && index < occupancyIndices; index++)
            {
                // init magic index
                int magicIndex = (int)((occupancies[index] * magicNumber) >> (64 - relevantBits));

                // if magic index works
                if (usedAttacks[magicIndex] == 0UL)
                {
                    // init used attacks
                    usedAttacks[magicIndex] = attacks[index];
                }
                // otherwise
                else if (usedAttacks[magicIndex] != attacks[index])
                {
                    // magic index doesn't work
                    fail = true;
                }
            }

            // if magic number works
            if (!fail)
            {
                // return it
                return magicNumber;
            }
        }

        // if magic number doesn't work
        Console.WriteLine("  Magic number fails!");
        return 0UL;
    }

    // init slider piece's attack tables
    private void InitSlidersAttacks()
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
            int occupancyIndices = (1 << relevantBitsCount);

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
        InitSlidersAttacks();
        Bitboard attacks = GetBishopAttacks(18, position.GetWhitePieces()) & ~position.GetEmptySquares();
        Console.WriteLine(attacks.ToString());
        
        /*List<Move> moves = new List<Move>();
        
        Bitboard bishops = this;

        while (bishops != 0)
        {
            int square = LSB();
            attacks = GetBishopAttacks(square, emptySquares) & ~emptySquares;
            while (attacks != 0)
            {
                int to = attacks.LSB();
                moves.Add(new Move(square, to));
                attacks &= attacks - 1;
            }
            bishops &= bishops - 1;
        }
        return moves;*/

        return null;
    }
}