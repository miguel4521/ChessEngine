namespace ChessEngine;

public class GameState
{
    public GameState(string fen)
    {
        Board = new Board(fen);
        // Split the FEN string into parts
        var parts = fen.Split(' ');
        // Set the turn
        Turn = parts[1] == "w" ? 0 : 1;
    }

    public Board Board { get; set; }
    public int Turn { get; set; }

    public string GameStateToFen()
    {
        var fen = "";
        // Add the board
        fen += Board.ToFen();
        // Add the turn
        fen += " " + (Turn == 1 ? "b" : "w");
        return fen;
    }

    public void MakeMove(Move move)
    {
        var startPosition = 1Ul << move.StartSquare;
        var endPosition = 1Ul << move.EndSquare;

        for (var side = 0; side < 2; side++)
        for (var piece = 0; piece < 6; piece++)
        {
            var bitboard = Board.Bitboards[side, piece];
            if ((bitboard & startPosition) != 0)
            {
                // Remove the piece from the start position
                Board.Bitboards[side, piece] &= ~startPosition;
                // Add the piece to the end position
                Board.Bitboards[side, piece] |= endPosition;
            }
        }
    }
}