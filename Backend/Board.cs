using System;

namespace Backend
{
    public class Board
    {
        public delegate void BoardDelegate(Board board);
        public event BoardDelegate UpdateBoard;

        public ChessPiece[,] squares { get; private set; }

        public Board()
        {
            squares = new ChessPiece[8, 8];

            for(int x = 0; x < 8; x++)
            {
                squares[x, 1] = new Pawn(TeamColor.BLACK);
                squares[x, 6] = new Pawn(TeamColor.WHITE);
            }

            squares[0, 0] = new Rook(TeamColor.BLACK);
            squares[7, 0] = new Rook(TeamColor.BLACK);
            squares[0, 7] = new Rook(TeamColor.WHITE);
            squares[7, 7] = new Rook(TeamColor.WHITE);

            squares[1, 0] = new Knight(TeamColor.BLACK);
            squares[6, 0] = new Knight(TeamColor.BLACK);
            squares[1, 7] = new Knight(TeamColor.WHITE);
            squares[6, 7] = new Knight(TeamColor.WHITE);

            squares[2, 0] = new Bishop(TeamColor.BLACK);
            squares[5, 0] = new Bishop(TeamColor.BLACK);
            squares[2, 7] = new Bishop(TeamColor.WHITE);
            squares[5, 7] = new Bishop(TeamColor.WHITE);

            squares[3, 0] = new Queen(TeamColor.BLACK);
            squares[4, 0] = new King(TeamColor.BLACK);
            squares[3, 7] = new Queen(TeamColor.WHITE);
            squares[4, 7] = new King(TeamColor.WHITE);
        }
        public Board(Board copy)
        {
            // Copy constructor
            // For use alongside Check()
            squares = new ChessPiece[8, 8];
            String temp;

            for(int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if(copy.squares[x, y] != null)
                    {
                        temp = copy.squares[x, y].GetType().ToString();

                        switch(temp)
                        {
                            case "Backend.Pawn":
                                squares[x,y] = new Pawn(copy.squares[x,y].Color);
                                break;
                            case "Backend.Rook":
                                squares[x, y] = new Rook(copy.squares[x, y].Color);
                                break;
                            case "Backend.Knight":
                                squares[x, y] = new Knight(copy.squares[x, y].Color);
                                break;
                            case "Backend.Bishop":
                                squares[x, y] = new Bishop(copy.squares[x, y].Color);
                                break;
                            case "Backend.Queen":
                                squares[x, y] = new Queen(copy.squares[x, y].Color);
                                break;
                            case "Backend.King":
                                squares[x, y] = new King(copy.squares[x, y].Color);
                                break;
                            default:
                                throw new ArgumentException();
                        }
                    }
                }
            }
        }

        public void DestroyBoard()
        {
            // Method to remove all references to ChessPiece objects so that the garbage collector can clean the chess pieces up
            //     in case the lingering memory pointers stop collection

            // For use in conjunction with the copy constructor
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    squares[x, y] = null;
                }
            }
        }

        public void MovePiece(int initalX, int initalY, int endX, int endY)
        {
            // Moves pieces. Overwrites any piece in the destination.
            ChessPiece temp = squares[initalX, initalY];
            squares[initalX, initalY] = null;
            squares[endX, endY] = temp;
        }
        public int[,] GetValidMoves(int x, int y)
        {
            // Returns an array with the legal moves for the piece at (x, y)

            if(squares[x ,y] == null)
            {
                // Define a PieceNotFoundExeption and throw it here
            }

            String pieceType = squares[x, y].GetType().ToString();

            switch(pieceType.ToString())
            {
                case "Backend.Pawn":
                    return GetValidPawnMoves(x, y);
                case "Backend.Rook":
                    return GetValidRookMoves(x, y);
                case "Backend.Knight":
                    return GetValidKnightMoves(x, y);
                case "Backend.Bishop":
                    return GetValidBishopMoves(x, y);
                case "Backend.Queen":
                    return GetValidQueenMoves(x, y);
                case "Backend.King":
                    return GetValidKingMoves(x, y);
                default:
                    throw new ArgumentException();
            }
        }
        private int[,] GetValidPawnMoves(int x, int y)
        {
            int[,] moves = new int[0, 2];

            int colorSwitch = 1;
            if(squares[x, y].Color == TeamColor.WHITE)
            {
                colorSwitch = -colorSwitch; // Pawns only move in one direction. colorSwitch is flips to acomidate this
            }

            try
            {
                if (squares[x, y + colorSwitch] == null)
                {
                    ExpandAndAddCoordinates(moves, x, y + colorSwitch);

                    if (colorSwitch == 1 && y == 6 || colorSwitch == -1 && y == 1)
                    {
                        ExpandAndAddCoordinates(moves, x, y + (colorSwitch * 2));
                    }
                }
            }
            catch { }
            try // Capture cases
            {
                if (squares[x + 1, y + colorSwitch] != null)
                {
                    if (colorSwitch == 1 && squares[x + 1, y + colorSwitch].Color == TeamColor.WHITE ||
                       colorSwitch == -1 && squares[x + 1, y + colorSwitch].Color == TeamColor.BLACK)
                    {
                        ExpandAndAddCoordinates(moves, x + 1, y + colorSwitch);
                    }
                }
            }
            catch { }
            try
            {
                if (squares[x - 1, y + colorSwitch] != null)
                {
                    if (colorSwitch == 1 && squares[x - 1, y + colorSwitch].Color == TeamColor.WHITE ||
                       colorSwitch == -1 && squares[x - 1, y + colorSwitch].Color == TeamColor.BLACK)
                    {
                        ExpandAndAddCoordinates(moves, x + 1, y + colorSwitch);
                    }
                }
            }
            catch { }

            return moves;
        }
        private int[,] GetValidRookMoves(int x, int y)
        {
            int[,] moves = new int[0, 2];
            int tempX, tempY;

            for(int i = 0; i < 4; i++)
            {
                tempX = x;
                tempY = y;

                try
                {
                    while (true)
                    {
                        switch (i) // Loop control
                        {
                            case 0:
                                tempX++; // Look right
                                break;
                            case 1:
                                tempX--; // Look left
                                break;
                            case 2:
                                tempY++; // Look down
                                break;
                            case 3:
                                tempY--; // Look up;
                                break;
                        }

                        if (squares[tempX, tempY] == null) // Empty space;
                        {
                            ExpandAndAddCoordinates(moves, tempX, tempY);
                        }
                        else if (squares[tempX, tempY].Color != squares[x, y].Color) // Capture enemy piece
                        {
                            ExpandAndAddCoordinates(moves, tempX, tempY);
                            break;
                        }
                        else // Allied piece on checked space
                        {
                            break;
                        }
                    }
                }
                catch { } // Space does not exist on the board
            }

            return moves;
        }
        private int[,] GetValidKnightMoves(int x, int y)
        {
            int[,] moves = new int[0, 2];
            int tempX, tempY;

            for (int i = 0; i < 8; i++)
            {
                try
                {
                    switch (i) // Loop control
                    {          // Each case is one of the 8 directions a knight can move
                        case 0:
                            tempX = x + 2;
                            tempY = y + 1;
                            break;
                        case 1:
                            tempX = x + 2;
                            tempY = y - 1;
                            break;
                        case 2:
                            tempX = x - 2;
                            tempY = y + 1;
                            break;
                        case 3:
                            tempX = x - 2;
                            tempY = y - 1;
                            break;
                        case 4:
                            tempX = x + 1;
                            tempY = y + 2;
                            break;
                        case 5:
                            tempX = x + 1;
                            tempY = y - 2;
                            break;
                        case 6:
                            tempX = x - 1;
                            tempY = y + 2;
                            break;
                        case 7:
                            tempX = x - 1;
                            tempY = y - 2;
                            break;
                        default:
                            throw new NotImplementedException(); // This line should never execute, but is here so that the if logic below doesn't yell at me
                    }

                    if (squares[tempX, tempY] == null) // Empty space;
                    {
                        ExpandAndAddCoordinates(moves, tempX, tempY);
                    }
                    else if (squares[tempX, tempY].Color != squares[x, y].Color) // Capture enemy piece
                    {
                        ExpandAndAddCoordinates(moves, tempX, tempY);
                    }
                }
                catch { } // Space does not exist on the board
            }

            return moves;
        }
        private int[,] GetValidBishopMoves(int x, int y)
        {
            int[,] moves = new int[0, 2];
            int tempX, tempY;

            for (int i = 0; i < 4; i++)
            {
                tempX = x;
                tempY = y;

                try
                {
                    while (true)
                    {
                        switch (i) // Loop control
                        {
                            case 0:
                                tempX++; // Look down and to the right
                                tempY++;
                                break;
                            case 1:
                                tempX++; // Look up and to the right
                                tempY--;
                                break;
                            case 2:
                                tempX--; // Look down and to the left
                                tempY++;
                                break;
                            case 3:
                                tempX--; // Look up and to the left
                                tempY--;
                                break;
                        }

                        if (squares[tempX, tempY] == null) // Empty space;
                        {
                            ExpandAndAddCoordinates(moves, tempX, tempY);
                        }
                        else if (squares[tempX, tempY].Color != squares[x, y].Color) // Capture enemy piece
                        {
                            ExpandAndAddCoordinates(moves, tempX, tempY);
                            break;
                        }
                        else // Allied piece on checked space
                        {
                            break;
                        }
                    }
                }
                catch { } // Space does not exist on the board
            }

            return moves;
        }
        private int[,] GetValidQueenMoves(int x, int y)
        {
            int[,] moves = new int[0, 2];
            int tempX, tempY;

            for (int i = 0; i < 8; i++)
            {
                tempX = x;
                tempY = y;

                try
                {
                    while (true)
                    {
                        switch (i) // Loop control
                        {
                            case 0:
                                tempX++; // Look right
                                break;
                            case 1:
                                tempX--; // Look left
                                break;
                            case 2:
                                tempY++; // Look down
                                break;
                            case 3:
                                tempY--; // Look up;
                                break;
                            case 4:
                                tempX++; // Look down and to the right
                                tempY++;
                                break;
                            case 5:
                                tempX++; // Look up and to the right
                                tempY--;
                                break;
                            case 6:
                                tempX--; // Look down and to the left
                                tempY++;
                                break;
                            case 7:
                                tempX--; // Look up and to the left
                                tempY--;
                                break;
                        }

                        if (squares[tempX, tempY] == null) // Empty space;
                        {
                            ExpandAndAddCoordinates(moves, tempX, tempY);
                        }
                        else if (squares[tempX, tempY].Color != squares[x, y].Color) // Capture enemy piece
                        {
                            ExpandAndAddCoordinates(moves, tempX, tempY);
                            break;
                        }
                        else // Allied piece on checked space
                        {
                            break;
                        }
                    }
                }
                catch { } // Space does not exist on the board
            }

            return moves;
        }
        private int[,] GetValidKingMoves(int x, int y)
        {
            int[,] moves = new int[0, 2];
            int tempX, tempY;

            for (int i = 0; i < 8; i++)
            {
                tempX = x;
                tempY = y;

                try
                {
                    switch (i) // Loop control
                    {
                        case 0:
                            tempX++; // Look right
                            break;
                        case 1:
                            tempX--; // Look left
                            break;
                        case 2:
                            tempY++; // Look down
                            break;
                        case 3:
                            tempY--; // Look up;
                            break;
                        case 4:
                            tempX++; // Look down and to the right
                            tempY++;
                            break;
                        case 5:
                            tempX++; // Look up and to the right
                            tempY--;
                            break;
                        case 6:
                            tempX--; // Look down and to the left
                            tempY++;
                            break;
                        case 7:
                            tempX--; // Look up and to the left
                            tempY--;
                            break;
                    }
                    if (squares[tempX, tempY] == null) // Empty space;
                    {
                        ExpandAndAddCoordinates(moves, tempX, tempY);
                    }
                    else if (squares[tempX, tempY].Color != squares[x, y].Color) // Capture enemy piece
                    {
                        ExpandAndAddCoordinates(moves, tempX, tempY);
                    }
                }
                catch { } // Space does not exist on the board
            }

            return moves;
        }
        private int[,] ExpandAndAddCoordinates(int[,] array, int insertX, int insertY) // Helper method to deal with arrays
        {
            // Expands an array's size by one and adds (x, y) to the list
            int[,] temp = new int[array.GetLength(0) + 1, 2];

            for(int x = 0; x < array.GetLength(0); x++)
            {
                temp[x, 0] = array[x, 0];
                temp[x, 1] = array[x, 1];
            }

            temp[array.GetLength(0), 0] = insertX;
            temp[array.GetLength(0), 1] = insertY;

            return temp;
        }

        public Boolean Check(TeamColor team)
        {
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    if(squares[x,y].Color == team && squares[x,y].GetType().ToString().Equals("Backend.King"))
                    {
                        if(CheckLineOfSightThreats(team, x, y).Length == 0) // Checks for Rooks, Bishops, and Queens
                        {
                            if(CheckKnightThreats(team, x, y).Length == 0) // Checks for those pesky Knights
                            {
                                if (CheckPawnThreats(team, x, y).Length == 0) // Hey, you never know!
                                {
                                    return false;
                                }
                            }
                        }
                        return true;
                    }
                }
            }
            throw new MissingFieldException("I'm not sure how you pulled it off, but there isn't a king for " + team);
        }
        private int[,] CheckLineOfSightThreats(TeamColor team, int x, int y)
        {
            int[,] threats = new int[0, 0];
            int tempX, tempY;

            for (int i = 0; i < 8; i++)
            {
                tempX = x;
                tempY = y;

                try
                {
                    while (true)
                    {
                        switch (i) // Loop control
                        {
                            case 0:
                                tempX++; // Look right
                                break;
                            case 1:
                                tempX--; // Look left
                                break;
                            case 2:
                                tempY++; // Look down
                                break;
                            case 3:
                                tempY--; // Look up;
                                break;
                            case 4:
                                tempX++; // Look down and to the right
                                tempY++;
                                break;
                            case 5:
                                tempX++; // Look up and to the right
                                tempY--;
                                break;
                            case 6:
                                tempX--; // Look down and to the left
                                tempY++;
                                break;
                            case 7:
                                tempX--; // Look up and to the left
                                tempY--;
                                break;
                        }
                        
                        if (squares[tempX, tempY].Color != squares[x, y].Color) // Enemy piece in LOS
                        {
                            switch(i)
                            {
                                case 0: // Straight-Line check; Looks for Rooks and Queens
                                case 1:
                                case 2:
                                case 3:
                                    if(squares[tempX, tempY].GetType().ToString().Equals("Backend.Rook") ||
                                       squares[tempX, tempY].GetType().ToString().Equals("Backend.Queen"))
                                    {
                                        ExpandAndAddCoordinates(threats, tempX, tempY);
                                    }
                                    break;
                                case 4: // Diagonal check; Looks for Bishops and queens
                                case 5:
                                case 6:
                                case 7:
                                    if (squares[tempX, tempY].GetType().ToString().Equals("Backend.Bishop") ||
                                       squares[tempX, tempY].GetType().ToString().Equals("Backend.Queen"))
                                    {
                                        ExpandAndAddCoordinates(threats, tempX, tempY);
                                    }
                                    break;
                            }
                            break;
                        }
                        else // Allied piece on checked space
                        {
                            break;
                        }
                    }
                }
                catch // Space does not exist on the board
                {
                    continue;
                }
            }
            return threats;
        }
        private int[,] CheckKnightThreats(TeamColor team, int x, int y)
        {
            int[,] threats = new int[0, 0];
            int tempX, tempY;

            for (int i = 0; i < 8; i++)
            {
                try
                {
                    switch (i) // Loop control
                    {          // Each case is one of the 8 directions a knight can move
                        case 0:
                            tempX = x + 2;
                            tempY = y + 1;
                            break;
                        case 1:
                            tempX = x + 2;
                            tempY = y - 1;
                            break;
                        case 2:
                            tempX = x - 2;
                            tempY = y + 1;
                            break;
                        case 3:
                            tempX = x - 2;
                            tempY = y - 1;
                            break;
                        case 4:
                            tempX = x + 1;
                            tempY = y + 2;
                            break;
                        case 5:
                            tempX = x + 1;
                            tempY = y - 2;
                            break;
                        case 6:
                            tempX = x - 1;
                            tempY = y + 2;
                            break;
                        case 7:
                            tempX = x - 1;
                            tempY = y - 2;
                            break;
                        default:
                            throw new NotImplementedException(); // See GetValidKnightMoves; Should never execute
                    }

                    if (squares[tempX, tempY].Color != squares[x, y].Color &&
                        squares[tempX, tempY].GetType().ToString().Equals("Backend.Knight")) // Enemy Knight on proper attack vector
                    {
                        ExpandAndAddCoordinates(threats, tempX, tempY);
                    }
                }
                catch { } // Space does not exist on the board
            }
            return threats;
        }
        private int[,] CheckPawnThreats(TeamColor team, int x, int y)
        {
            int[,] threats = new int[0, 0];

            int colorSwitch = 1;
            if (squares[x, y].Color == TeamColor.BLACK)
            {
                colorSwitch = -colorSwitch; // Control switch for pawn direction. Inverted compared to GetValidPawnMoves
            }

            try
            {
                if (squares[x + 1, y + colorSwitch].Color != team &&
                    squares[x + 1, y + colorSwitch].GetType().ToString().Equals("Backend.Pawn"))
                {
                    ExpandAndAddCoordinates(threats, x + 1, y + colorSwitch);
                }
            }
            catch { }
            try
            {
                if (squares[x - 1, y + colorSwitch].Color != team &&
                    squares[x - 1, y + colorSwitch].GetType().ToString().Equals("Backend.Pawn"))
                {
                    ExpandAndAddCoordinates(threats, x + 1, y + colorSwitch);
                }
            }
            catch { }

            return threats;
        }

        public Boolean Checkmate(TeamColor team)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (squares[x, y].Color == team && squares[x, y].GetType().ToString().Equals("Backend.King"))
                    {
                        if (!CheckmateMovementCheck(x, y))
                        {
                            if(!CheckmateBlockCheck(x, y))
                            {
                                if(!CheckmateCaptureCheck(x, y))
                                {
                                    return true;
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            throw new MissingFieldException("I'm not sure how you pulled it off, but there isn't a king for " + team);
        }
        private Boolean CheckmateMovementCheck(int x, int y)
        {
            int[,] moves = GetValidKingMoves(x, y);
            Board temp;

            for(int i = 0; i < moves.Length; i++)
            {
                temp = new Board(this);
                temp.MovePiece(x, y, moves[i, 0], moves[i, 1]);
                
                if(!temp.Check(squares[x,y].Color)) // if it is possible to move the king out of check
                {
                    return false;
                }
            }
            return true;
        }
        private Boolean CheckmateBlockCheck(int x, int y)
        {
            throw new NotImplementedException();
        }
        private Boolean CheckmateCaptureCheck(int x, int y)
        {
            throw new NotImplementedException();
        }
        private int[,] CompileThreatList(TeamColor team, int x, int y)
        {
            int[,] threats = new int[0, 2];
            for (int a = 0; a < 3; a++)
            {
                switch (a)
                {
                    case 0:
                        threats = CheckLineOfSightThreats(team, x, y);
                        break;
                    case 1:
                        threats = CheckKnightThreats(team, x, y);
                        break;
                    case 2:
                        threats = CheckPawnThreats(team, x, y);
                        break;
                    default:
                        throw new NotImplementedException(); // See GetValidKnightMoves; Should never execute
                }

                for(int b = 0; b < threats.Length; b++)
                {
                    ExpandAndAddCoordinates(threats, threats[b, 0], threats[b, 1]);
                }
            }
            return threats;
        }

        public void Update()
        {
            UpdateBoard.Invoke(this); // TODO: How exactly ami I going to call this in the code?
        }
    }
}
