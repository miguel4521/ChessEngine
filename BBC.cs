using System.Numerics;

namespace ChessEngine;

/*
* I AM FOLLOWING THE TUTORIAL BY CHESS PROGRAMMING ON YOUTUBE
* https://youtu.be/QUNP-UjujBM
* OOP WILL BE TAKEN ADVANTAGE OF LATER AND IMPLEMENTED INTO ASP.NET
*/

public class Bbc
{
    /*
     * Attacks
     */

    // not a file
    private const ulong NotAFile = 18374403900871474942UL;

    // not h file
    private const ulong NotHFile = 9187201950435737471UL;

    // not hg file
    private const ulong NotHgFile = 4557430888798830399UL;

    // not ab file
    private const ulong NotAbFile = 18229723555195321596UL;

    // bishop relevant occupancy bit count for every square on the board
    private int[] bishopRelevantBits =
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

    // rook relevant occupancy bit count for every square on board
    private int[] rookRelevantBits =
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

    // rook magic numbers
    private ulong[] rookMagicNumbers =
    {
        0x8a80104000800020UL,
        0x140002000100040UL,
        0x2801880a0017001UL,
        0x100081001000420UL,
        0x200020010080420UL,
        0x3001c0002010008UL,
        0x8480008002000100UL,
        0x2080088004402900UL,
        0x800098204000UL,
        0x2024401000200040UL,
        0x100802000801000UL,
        0x120800800801000UL,
        0x208808088000400UL,
        0x2802200800400UL,
        0x2200800100020080UL,
        0x801000060821100UL,
        0x80044006422000UL,
        0x100808020004000UL,
        0x12108a0010204200UL,
        0x140848010000802UL,
        0x481828014002800UL,
        0x8094004002004100UL,
        0x4010040010010802UL,
        0x20008806104UL,
        0x100400080208000UL,
        0x2040002120081000UL,
        0x21200680100081UL,
        0x20100080080080UL,
        0x2000a00200410UL,
        0x20080800400UL,
        0x80088400100102UL,
        0x80004600042881UL,
        0x4040008040800020UL,
        0x440003000200801UL,
        0x4200011004500UL,
        0x188020010100100UL,
        0x14800401802800UL,
        0x2080040080800200UL,
        0x124080204001001UL,
        0x200046502000484UL,
        0x480400080088020UL,
        0x1000422010034000UL,
        0x30200100110040UL,
        0x100021010009UL,
        0x2002080100110004UL,
        0x202008004008002UL,
        0x20020004010100UL,
        0x2048440040820001UL,
        0x101002200408200UL,
        0x40802000401080UL,
        0x4008142004410100UL,
        0x2060820c0120200UL,
        0x1001004080100UL,
        0x20c020080040080UL,
        0x2935610830022400UL,
        0x44440041009200UL,
        0x280001040802101UL,
        0x2100190040002085UL,
        0x80c0084100102001UL,
        0x4024081001000421UL,
        0x20030a0244872UL,
        0x12001008414402UL,
        0x2006104900a0804UL,
        0x1004081002402UL,
    };


// bishop magic numbers
    private ulong[] bishopMagicNumbers =
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

    // king attacks table
    private readonly ulong[] kingAttacks = new ulong[64];

    // knight attacks table
    private readonly ulong[] knightAttacks = new ulong[64];

    // pawn attacks table
    private readonly ulong[,] pawnAttacks = new ulong[2, 64];

    public void PrintBitboard(ulong bitboard)
    {
        Console.WriteLine();
        for (var rank = 0; rank < 8; rank++)
        {
            for (var file = 0; file < 8; file++)
            {
                var square = rank * 8 + file;

                if (file == 0)
                    Console.Write($" {8 - rank}  ");

                Console.Write($" {GetBit(bitboard, square)} ");
            }

            Console.WriteLine();
        }

        // print board files
        Console.WriteLine("     a  b  c  d  e  f  g  h");
        Console.WriteLine("     Bitboard: " + bitboard);
    }

    /*
     * Changing/retrieving bits on the bitboard
     */
    private ulong GetBit(ulong bitboard, int square)
    {
        return (bitboard >> square) & 1;
    }

    private ulong SetBit(ulong bitboard, int square)
    {
        return bitboard | (1UL << square);
    }

    private ulong PopBit(ulong bitboard, int square)
    {
        return bitboard & ~(1UL << square);
    }

    // count bits within a bitboard (Brian Kernighan's way)
    public int CountBits(ulong bitboard)
    {
        // bit counter
        int count = 0;

        // consecutively reset least significant 1st bit
        while (bitboard != 0)
        {
            // increment count
            count++;

            // reset least significant 1st bit
            bitboard &= bitboard - 1;
        }

        // return bit count
        return count;
    }

    // get least significant 1st bit index
    private int GetLs1BitIndex(ulong bitboard)
    {
        // return least significant 1st bit index
        return BitOperations.TrailingZeroCount(bitboard);
    }

    public int ChessNotationToSquare(string notation)
    {
        // Convert the file (letter) to a number (a = 0, b = 1, etc.)
        int file = notation[0] - 'a';

        // Convert the rank (number) to a number (8 = 0, 7 = 1, etc.)
        int rank = 8 - int.Parse(notation[1].ToString());

        // Calculate the square number as 8 * rank + file
        return 8 * rank + file;
    }

    public string SquareToChessNotation(int square)
    {
        // Convert the square number to a file (letter) and rank (number)
        int file = square % 8;
        int rank = 8 - square / 8;

        // Convert the file (letter) to a character (0 = a, 1 = b, etc.)
        char fileChar = (char)('a' + file);

        // Convert the rank (number) to a string
        string rankString = rank.ToString();

        // Return the file and rank as a string
        return fileChar + rankString;
    }

    private ulong MaskPawnAttacks(int side, int square)
    {
        // result attacks bb
        ulong attacks = 0;

        // piece bitboard
        ulong bitboard = 0;

        // set piece on board
        bitboard = SetBit(bitboard, square);

        // white pawn attacks
        if (side == 0)
        {
            if (((bitboard >> 7) & NotAFile) != 0)
                attacks |= bitboard >> 7;
            if (((bitboard >> 9) & NotHFile) != 0)
                attacks |= bitboard >> 9;
        } // black pawns
        else
        {
            if (((bitboard << 7) & NotHFile) != 0)
                attacks |= bitboard << 7;
            if (((bitboard << 9) & NotAFile) != 0)
                attacks |= bitboard << 9;
        }

        // return attack map
        return attacks;
    }

    private ulong MaskKnightAttacks(int square)
    {
        // result attacks bb
        ulong attacks = 0;

        // piece bitboard
        ulong bitboard = 0;

        // set piece on board
        bitboard = SetBit(bitboard, square);

        // knight attacks
        if (((bitboard >> 17) & NotHFile) != 0)
            attacks |= bitboard >> 17;
        if (((bitboard >> 15) & NotAFile) != 0)
            attacks |= bitboard >> 15;
        if (((bitboard >> 10) & NotHgFile) != 0)
            attacks |= bitboard >> 10;
        if (((bitboard >> 6) & NotAbFile) != 0)
            attacks |= bitboard >> 6;
        if (((bitboard << 17) & NotAFile) != 0)
            attacks |= bitboard << 17;
        if (((bitboard << 15) & NotHFile) != 0)
            attacks |= bitboard << 15;
        if (((bitboard << 10) & NotAbFile) != 0)
            attacks |= bitboard << 10;
        if (((bitboard << 6) & NotHgFile) != 0)
            attacks |= bitboard << 6;

        // return attack map
        return attacks;
    }

    private ulong MaskKingAttacks(int square)
    {
        // result attacks bb
        ulong attacks = 0;

        // piece bitboard
        ulong bitboard = 0;

        // set piece on board
        bitboard = SetBit(bitboard, square);

        // king attacks
        if (bitboard >> 8 != 0)
            attacks |= bitboard >> 8;
        if (((bitboard >> 9) & NotHFile) != 0)
            attacks |= bitboard >> 9;
        if (((bitboard >> 7) & NotAFile) != 0)
            attacks |= bitboard >> 7;
        if (((bitboard >> 1) & NotHFile) != 0)
            attacks |= bitboard >> 1;
        if (bitboard << 8 != 0)
            attacks |= bitboard << 8;
        if (((bitboard << 9) & NotAFile) != 0)
            attacks |= bitboard << 9;
        if (((bitboard << 7) & NotHFile) != 0)
            attacks |= bitboard << 7;
        if (((bitboard << 1) & NotAFile) != 0)
            attacks |= bitboard << 1;

        // return attack map
        return attacks;
    }

    // mask bishop attacks
    private ulong MaskBishopAttacks(int square)
    {
        // result attacks bb
        ulong attacks = 0;

        // init ranks and files
        int r, f;

        // init target rank and file
        var tr = square / 8;
        var tf = square % 8;

        // mask relevant bishop occupancy bits
        for (r = tr + 1, f = tf + 1; r <= 6 && f <= 6; r++, f++) attacks |= 1UL << (r * 8 + f);
        for (r = tr - 1, f = tf + 1; r >= 1 && f <= 6; r--, f++) attacks |= 1UL << (r * 8 + f);
        for (r = tr + 1, f = tf - 1; r <= 6 && f >= 1; r++, f--) attacks |= 1UL << (r * 8 + f);
        for (r = tr - 1, f = tf - 1; r >= 1 && f >= 1; r--, f--) attacks |= 1UL << (r * 8 + f);

        // return attack map
        return attacks;
    }

    // mask rook attacks
    public ulong MaskRookAttacks(int square)
    {
        // result attacks bb
        ulong attacks = 0;

        // init ranks and files
        int r, f;

        // init target rank and file
        var tr = square / 8;
        var tf = square % 8;

        // mask relevant rook occupancy bits
        for (r = tr + 1; r <= 6; r++) attacks |= 1UL << (r * 8 + tf);
        for (r = tr - 1; r >= 1; r--) attacks |= 1UL << (r * 8 + tf);
        for (f = tf + 1; f <= 6; f++) attacks |= 1UL << (tr * 8 + f);
        for (f = tf - 1; f >= 1; f--) attacks |= 1UL << (tr * 8 + f);

        // return attack map
        return attacks;
    }

    // generate bishop attacks on the fly
    private ulong BishopAttacksOnTheFly(int square, ulong block)
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
            attacks |= 1UL << (r * 8 + f);
            if ((block & (1UL << (r * 8 + f))) != 0) break;
        }

        for (r = tr - 1, f = tf + 1; r >= 0 && f <= 7; r--, f++)
        {
            attacks |= 1UL << (r * 8 + f);
            if ((block & (1UL << (r * 8 + f))) != 0) break;
        }

        for (r = tr + 1, f = tf - 1; r <= 7 && f >= 0; r++, f--)
        {
            attacks |= 1UL << (r * 8 + f);
            if ((block & (1UL << (r * 8 + f))) != 0) break;
        }

        for (r = tr - 1, f = tf - 1; r >= 0 && f >= 0; r--, f--)
        {
            attacks |= 1UL << (r * 8 + f);
            if ((block & (1UL << (r * 8 + f))) != 0) break;
        }

        return attacks;
    }

    // generate rook attacks on the fly
    public ulong RookAttacksOnTheFly(int square, ulong block)
    {
        // result attacks bitboard
        ulong attacks = 0UL;

        // init ranks & files
        int r, f;

        // init target rank & files
        int tr = square / 8;
        int tf = square % 8;

        // generate rook attacks
        for (r = tr + 1; r <= 7; r++)
        {
            attacks |= 1UL << (r * 8 + tf);
            if ((block & (1UL << (r * 8 + tf))) != 0) break;
        }

        for (r = tr - 1; r >= 0; r--)
        {
            attacks |= 1UL << (r * 8 + tf);
            if ((block & (1UL << (r * 8 + tf))) != 0) break;
        }

        for (f = tf + 1; f <= 7; f++)
        {
            attacks |= 1UL << (tr * 8 + f);
            if ((block & (1UL << (tr * 8 + f))) != 0) break;
        }

        for (f = tf - 1; f >= 0; f--)
        {
            attacks |= 1UL << (tr * 8 + f);
            if ((block & (1UL << (tr * 8 + f))) != 0) break;
        }

        return attacks;
    }

    // init leaper pieces attacks
    public void InitLeapersAttacks()
    {
        // Loop over all squares
        for (var square = 0; square < 64; square++)
        {
            // init pawn attacks
            pawnAttacks[0, square] = MaskPawnAttacks(0, square);
            pawnAttacks[1, square] = MaskPawnAttacks(1, square);

            // init knight attacks
            knightAttacks[square] = MaskKnightAttacks(square);

            // init king attacks
            kingAttacks[square] = MaskKingAttacks(square);
        }
    }

    // set occupancies
    public ulong SetOccupancy(int index, int bitsInMask, ulong attackMask)
    {
        // occupancy map
        ulong occupancy = 0;

        // loop over all bits in mask
        for (var i = 0; i < bitsInMask; i++)
        {
            int square = GetLs1BitIndex(attackMask);

            // set occupancy
            attackMask = PopBit(attackMask, square);

            // make sure occupancy is on the board
            if ((index & (1 << i)) != 0)
                // populate occupancy map
                occupancy |= 1UL << square;
        }

        return occupancy;
    }

    // find appropriate magic number
    public ulong find_magic_number(int square, int relevantBits, bool bishop)
    {
        // init occupancies
        ulong[] occupancies = new ulong[64];

        // init attack tables
        ulong[] attacks = new ulong[4096];

        // init used attacks
        ulong[] usedAttacks = new ulong[4096];

        // init attack mask for a current piece
        ulong attackMask = bishop ? MaskBishopAttacks(square) : MaskRookAttacks(square);

        // init occupancy indicies
        int occupancyIndicies = 1 << relevantBits;

        // loop over occupancy indicies
        for (int index = 0; index < occupancyIndicies; index++)
        {
            // init occupancies
            occupancies[index] = SetOccupancy(index, relevantBits, attackMask);

            // init attacks
            attacks[index] = bishop
                ? BishopAttacksOnTheFly(square, occupancies[index])
                : RookAttacksOnTheFly(square, occupancies[index]);
        }

        // test magic numbers loop
        for (int randomCount = 0; randomCount < 100000000; randomCount++)
        {
            // generate magic number candidate
            ulong magicNumber = GenerateMagicNumber();

            // skip inappropriate magic numbers
            if (CountBits((attackMask * magicNumber) & 0xFF00000000000000) < 6) continue;

            // init used attacks
            for (int i = 0; i < usedAttacks.Length; i++)
                usedAttacks[i] = 0UL;

                // init index & fail flag
            int index;
            bool fail;

            // test magic index loop
            for (index = 0, fail = false; !fail && index < occupancyIndicies; index++)
            {
                // init magic index
                int magicIndex = (int)((occupancies[index] * magicNumber) >> (64 - relevantBits));

                // if magic index works
                if (usedAttacks[magicIndex] == 0UL)
                    // init used attacks
                    usedAttacks[magicIndex] = attacks[index];

                // otherwise
                else if (usedAttacks[magicIndex] != attacks[index])
                    // magic index doesn't work
                    fail = true;
            }

            // if magic number works
            if (!fail)
                // return it
                return magicNumber;
        }

        // if magic number doesn't work
        Console.WriteLine("  Magic number fails!");
        return 0UL;
    }

    // pseudo random number state
    private uint state = 1804289383;

    // generate 32-bit pseudo legal numbers
    public uint GetRandomNumber()
    {
        // get current state
        uint number = state;

        // XOR shift algorithm
        number ^= number << 13;
        number ^= number >> 17;
        number ^= number << 5;

        // update random number state
        state = number;

        // return random number
        return number;
    }

    // generate 32-bit pseudo legal numbers
    private uint GetRandomU32Number()
    {
        // get current state
        uint number = state;

        // XOR shift algorithm
        number ^= number << 13;
        number ^= number >> 17;
        number ^= number << 5;

        // update random number state
        state = number;

        // return random number
        return number;
    }

    // generate 64-bit pseudo legal numbers
    private ulong GetRandomU64Number()
    {
        // define 4 random numbers
        ulong n1, n2, n3, n4;

        // init random numbers slicing 16 bits from MS1B side
        n1 = GetRandomU32Number() & 0xFFFF;
        n2 = GetRandomU32Number() & 0xFFFF;
        n3 = GetRandomU32Number() & 0xFFFF;
        n4 = GetRandomU32Number() & 0xFFFF;

        // return random number
        return n1 | (n2 << 16) | (n3 << 32) | (n4 << 48);
    }

// generate magic number candidate
    public ulong GenerateMagicNumber()
    {
        return GetRandomU64Number() & GetRandomU64Number() & GetRandomU64Number();
    }
}