using System;

namespace Backend
{
    public enum TeamColor { WHITE, BLACK }

    public class ChessPiece
    {
        public TeamColor Color { get; private set; }

        public ChessPiece(TeamColor Color)
        {
            this.Color = Color;
        }
    }

    public class Pawn : ChessPiece { public Pawn(TeamColor Color) : base(Color) { } }
    public class Rook : ChessPiece
    {
        public Boolean hasMoved = false;
        public Rook(TeamColor Color) : base(Color) { }
    }
    public class Knight : ChessPiece { public Knight(TeamColor Color) : base(Color) { } }
    public class Bishop : ChessPiece { public Bishop(TeamColor Color) : base(Color) { } }
    public class Queen : ChessPiece { public Queen(TeamColor Color) : base(Color) { } }
    public class King : ChessPiece
    {
        public Boolean hasMoved = false;
        public King(TeamColor Color) : base(Color) { }
    }
}
