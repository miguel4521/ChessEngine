using System.Text;
using ChessEngine.Bitboards;

namespace ChessEngine;

public struct Position
{
    // An array of bitboards, one for each piece type and color
    public Piece[] pieces;
    public bool whiteToMove = true;
    
    private readonly Dictionary<char, int> pieceIndexFromChar = new()
    {
        { 'p', 0 },
        { 'P', 1 },
        { 'n', 2 },
        { 'N', 3 },
        { 'b', 4 },
        { 'B', 5 },
        { 'r', 6 },
        { 'R', 7 },
        { 'q', 8 },
        { 'Q', 9 },
        { 'k', 10 },
        { 'K', 11 }
    };

    // The constructor that initializes the bitboards to the initial position
    public Position(string fen)
    {
        pieces = new Piece[12];
        pieces[0] = new Pawn(true); // white pawns
        pieces[1] = new Pawn(false); // black pawns
        pieces[2] = new Knight(true); // white knights
        pieces[3] = new Knight(false); // black knights
        pieces[4] = new Bishop(true); // white bishops
        pieces[5] = new Bishop(false); // black bishops
        pieces[6] = new Rook(true); // white rooks
        pieces[7] = new Rook(false); // black rooks
        pieces[8] = new Queen(true); // white queens
        pieces[9] = new Queen(false); // black queens
        pieces[10] = new King(true); // white king
        pieces[11] = new King(false); // black king
        FromFEN(fen);
    }

    public void MakeMove(Move move)
    {
        // Move the piece
        pieces[move.PieceIndex][move.To] = true;
        pieces[move.PieceIndex][move.From] = false;

        // If the move is a capture, remove the captured piece
        if (move.CapturedPieceIndex != -1)
            pieces[move.CapturedPieceIndex][move.To] = false;

        // Switch sides to move
        whiteToMove = !whiteToMove;
    }

    public List<Move> GenerateMoves()
    {
        List<Move> moves = new List<Move>();
        // Generate moves for all pieces, depending on the side to move
        for (int i = whiteToMove ? 0 : 1; i < 12; i += 2)
            moves.AddRange(pieces[i].GenerateMoves(this));
        return moves;
    }

    public int GetPieceIndexAt(int square)
    {
        foreach (Piece piece in pieces)
        {
            if (piece[square])
                return pieceIndexFromChar[piece.GetSymbol()];
        }

        return -1;
    }

    public void MakeMove(Move move)
    {
        // Move the piece
        pieces[move.PieceIndex][move.To] = true;
        pieces[move.PieceIndex][move.From] = false;

        // If the move is a capture, remove the captured piece
        if (move.CapturedPieceIndex != -1)
            pieces[move.CapturedPieceIndex][move.To] = false;
        
        // Switch sides to move
        whiteToMove = !whiteToMove;
    }

    public List<Move> GenerateMoves()
    {
        List<Move> moves = new List<Move>();
        // Generate moves for all pieces, depending on the side to move
        for (int i = whiteToMove ? 0 : 1; i < 12; i += 2)
            moves.AddRange(pieces[i].GenerateMoves(this));
        return moves;
    }

    public int GetPieceIndexAt(int square)
    {
        Dictionary<Type, int> pieceIndex = new Dictionary<Type, int>()
        {
            { typeof(Pawn), 0 },
            { typeof(Knight), 2 },
            { typeof(Bishop), 4 },
            { typeof(Rook), 6 },
            { typeof(Queen), 8 },
            { typeof(King), 10 }
        };
        foreach (Piece piece in pieces)
        {
            if (piece[square])
                return pieceIndex[piece.GetType()];
        }
        
        return -1;
    }

    public Bitboard GetWhitePieces()
    {
        Bitboard whitePieces = new Bitboard();
        for (int i = 0; i < 12; i += 2)
            whitePieces |= pieces[i];
        return whitePieces;
    }

    public Bitboard GetBlackPieces()
    {
        Bitboard blackPieces = new Bitboard();
        for (int i = 1; i < 12; i += 2)
            blackPieces |= pieces[i];
        return blackPieces;
    }

    public Bitboard GetOccupiedSquares()
    {
        Bitboard occupied = new Bitboard();
        foreach (Piece piece in pieces)
            occupied |= piece;
        return occupied;
    }

    public Bitboard GetEmptySquares()
    {
        return GetOccupiedSquares().GetEmptySquares();
    }

    public string ToFen()
    {
        StringBuilder fen = new StringBuilder();

        // Loop through ranks from top to bottom
        for (int rank = 7; rank >= 0; rank--)
        {
            int emptyCount = 0;

            // Loop through files from left to right
            for (int file = 0; file < 8; file++)
            {
                int square = rank * 8 + file;
                int pieceIndex = GetPieceIndexAt(square);

                if (pieceIndex == -1)
                    emptyCount++;
                else
                {
                    if (emptyCount != 0)
                    {
                        fen.Append(emptyCount);
                        emptyCount = 0;
                    }

                    char pieceSymbol = pieces[pieceIndex].GetSymbol();
                    fen.Append(pieceSymbol);
                }
            }

            if (emptyCount != 0)
                fen.Append(emptyCount);

            if (rank > 0)
                fen.Append('/');
        }

        fen.Append(' ');

        // Side to move
        fen.Append(whiteToMove ? 'w' : 'b');

        // TODO: Add castling rights, en passant square, half-move clock, and full-move number information.
        // For simplicity, assuming no castling rights, no en passant square, and starting from move 1.
        fen.Append(" - - 0 1");

        return fen.ToString();
    }
    
    public void FromFEN(string fen)
    {
        string[] fenParts = fen.Trim().Split(' ');

        // Clear the board
        foreach (var piece in pieces)
            piece.Clear();

        int rank = 7;
        int file = 0;

        // Loop through the piece placement section of the FEN string
        foreach (char c in fenParts[0])
        {
            if (char.IsDigit(c))
            {
                file += int.Parse(c.ToString());
            }
            else if (c == '/')
            {
                rank--;
                file = 0;
            }
            else
            {
                int pieceIndex = pieceIndexFromChar[c];
                bool isWhite = !char.IsUpper(c);

                pieces[pieceIndex][rank * 8 + file] = true;
                pieces[pieceIndex].IsWhite = isWhite;

                file++;
            }
        }

        // Set side to move
        whiteToMove = fenParts[1] == "w";

        // TODO: Set castling rights, en passant square, halfmove clock, and fullmove number from the FEN string.
        // For simplicity, assuming no castling rights, no en passant square, and starting from move 1.
    }
}