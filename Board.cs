namespace ChessEngine;

public class Board
{
    // [side][piece type]
    // 0 = white, 1 = black
    // 0 = pawns, 1 = knights, 2 = bishops, 3 = rooks, 4 = queens, 5 = kings
    public ulong[,] Bitboards = new ulong[2, 6];

    public Board(string fen)
    {
        FenToBitboard(fen);
    }

    private void FenToBitboard(string fen)
    {
        // Split the FEN string into parts
        var parts = fen.Split(' ');

        var pieceMapping = new Dictionary<char, int>
        {
            { 'p', 0 },
            { 'n', 1 },
            { 'b', 2 },
            { 'r', 3 },
            { 'q', 4 },
            { 'k', 5 }
        };

        // Parse the piece placement part of the FEN string
        var piecePlacement = parts[0];
        var rank = 7;
        var file = 0;
        foreach (var c in piecePlacement)
            if (c == '/')
            {
                rank--;
                file = 0;
            }
            else if (char.IsDigit(c))
            {
                file += int.Parse(c.ToString());
            }
            else
            {
                var position = 1UL << (rank * 8 + file);
                var side = char.IsUpper(c) ? 1 : 0;
                Bitboards[side, pieceMapping[char.ToLower(c)]] |= position;
                file++;
            }
    }

    public string ToFen()
    {
        var fen = "";
        var pieceMapping = new Dictionary<char, ulong>
        {
            { 'p', Bitboards[0, 0] },
            { 'n', Bitboards[0, 1] },
            { 'b', Bitboards[0, 2] },
            { 'r', Bitboards[0, 3] },
            { 'q', Bitboards[0, 4] },
            { 'k', Bitboards[0, 5] },
            { 'P', Bitboards[1, 0] },
            { 'N', Bitboards[1, 1] },
            { 'B', Bitboards[1, 2] },
            { 'R', Bitboards[1, 3] },
            { 'Q', Bitboards[1, 4] },
            { 'K', Bitboards[1, 5] }
        };
        // Iterate through the ranks (rows) of the board, starting from the 8th rank
        for (var rank = 7; rank >= 0; rank--)
        {
            var emptySquares = 0;
            for (var file = 0; file < 8; file++)
            {
                var position = 1UL << (rank * 8 + file);

                var foundPiece = false;
                foreach (var kvp in pieceMapping)
                    if ((kvp.Value & position) != 0)
                    {
                        // Add the empty squares, if any, to the FEN string
                        if (emptySquares > 0)
                        {
                            fen += emptySquares.ToString();
                            emptySquares = 0;
                        }

                        fen += kvp.Key;
                        foundPiece = true;
                        break;
                    }

                if (!foundPiece)
                    emptySquares++;
            }

            if (emptySquares > 0)
                fen += emptySquares.ToString();
            if (rank > 0)
                fen += "/";
        }

        return fen;
    }

    public int BitScanForward(ulong bb)
    {
        var index = 0;
        if (bb != 0)
            while ((bb & 1) == 0)
            {
                bb >>= 1;
                index++;
            }

        return index;
    }

    private ulong WhitePieces()
    {
        return Bitboards[0, 0] | Bitboards[0, 1] | Bitboards[0, 2] | Bitboards[0, 3] | Bitboards[0, 4] |
               Bitboards[0, 5];
    }

    private ulong BlackPieces()
    {
        return Bitboards[1, 0] | Bitboards[1, 1] | Bitboards[1, 2] | Bitboards[1, 3] | Bitboards[1, 4] |
               Bitboards[1, 5];
    }

    private ulong AllPieces()
    {
        return WhitePieces() | BlackPieces();
    }

    public ulong EmptySquares()
    {
        return ~AllPieces();
    }
}