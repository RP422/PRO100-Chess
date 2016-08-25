using System;
using System.Windows;
using System.Windows.Controls;
using Backend;

namespace ChessWPF
{
    /// <summary>
    /// Interaction logic forMainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Button[,] squares = new Button[8, 8];
        Board board;

        int storedX = -1;
        int storedY = -1;

        TeamColor currentColor = TeamColor.WHITE;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSquares();
            board = new Board();
        }

        private void InitializeSquares()
        {
            squares[0, 0] = Square00;
            squares[1, 0] = Square10;
            squares[2, 0] = Square20;
            squares[3, 0] = Square30;
            squares[4, 0] = Square40;
            squares[5, 0] = Square50;
            squares[6, 0] = Square60;
            squares[7, 0] = Square70;

            squares[0, 1] = Square01;
            squares[1, 1] = Square11;
            squares[2, 1] = Square21;
            squares[3, 1] = Square31;
            squares[4, 1] = Square41;
            squares[5, 1] = Square51;
            squares[6, 1] = Square61;
            squares[7, 1] = Square71;

            squares[0, 2] = Square02;
            squares[1, 2] = Square12;
            squares[2, 2] = Square22;
            squares[3, 2] = Square32;
            squares[4, 2] = Square42;
            squares[5, 2] = Square52;
            squares[6, 2] = Square62;
            squares[7, 2] = Square72;

            squares[0, 3] = Square03;
            squares[1, 3] = Square13;
            squares[2, 3] = Square23;
            squares[3, 3] = Square33;
            squares[4, 3] = Square43;
            squares[5, 3] = Square53;
            squares[6, 3] = Square63;
            squares[7, 3] = Square73;

            squares[0, 4] = Square04;
            squares[1, 4] = Square14;
            squares[2, 4] = Square24;
            squares[3, 4] = Square34;
            squares[4, 4] = Square44;
            squares[5, 4] = Square54;
            squares[6, 4] = Square64;
            squares[7, 4] = Square74;

            squares[0, 5] = Square05;
            squares[1, 5] = Square15;
            squares[2, 5] = Square25;
            squares[3, 5] = Square35;
            squares[4, 5] = Square45;
            squares[5, 5] = Square55;
            squares[6, 5] = Square65;
            squares[7, 5] = Square75;

            squares[0, 6] = Square06;
            squares[1, 6] = Square16;
            squares[2, 6] = Square26;
            squares[3, 6] = Square36;
            squares[4, 6] = Square46;
            squares[5, 6] = Square56;
            squares[6, 6] = Square66;
            squares[7, 6] = Square76;

            squares[0, 7] = Square07;
            squares[1, 7] = Square17;
            squares[2, 7] = Square27;
            squares[3, 7] = Square37;
            squares[4, 7] = Square47;
            squares[5, 7] = Square57;
            squares[6, 7] = Square67;
            squares[7, 7] = Square77;
        }

        public void UpdateBoard(Board board)
        {
            string mark = "";

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (!mark.Equals(""))
                    {
                        mark = ""; // Clear the mark
                    }

                    if (board.squares[x, y] != null)
                    {
                        if (board.squares[x, y].Color == TeamColor.WHITE) // Color forthe mark
                        {
                            mark += "W";
                        }
                        else
                        {
                            mark += "B";
                        }

                        switch (board.squares[x, y].GetType().ToString()) // Type of piece forthe mark
                        {
                            case "Backend.Pawn":
                                mark += "P";
                                break;
                            case "Backend.Rook":
                                mark += "R";
                                break;
                            case "Backend.Knight":
                                mark += "N";
                                break;
                            case "Backend.Bishop":
                                mark += "B";
                                break;
                            case "Backend.Queen":
                                mark += "Q";
                                break;
                            case "Backend.King":
                                mark += "K";
                                break;
                            default:
                                throw new ArgumentException();
                        }
                    }
                    squares[x, y].Content = mark;
                }
            }
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            Button buttonSender = (Button)sender;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (buttonSender == squares[x, y])
                    {
                        ClickLogic(x, y);
                        return;
                    }
                }
            }
        }
        private void ClickLogic(int x, int y) // Branches logic for click events
        {
            if (storedX == x && storedY == y)
            {
                storedX = -1;
                storedY = -1;
            }
            else if (board.squares[x, y] == null)
            {
                if (storedX != -1 && storedY != -1)
                {
                    moveLogic(x, y);
                }
            }
            else
            {
                if (storedX != -1 && storedY != -1)
                {
                    if (board.squares[x, y].Color != currentColor)
                    {
                        moveLogic(x, y);
                    }
                    else
                    {
                        storedX = -1;
                        storedY = -1;
                    }
                }
                else
                {
                    if (board.squares[x, y].Color == currentColor)
                    {
                        storedX = x;
                        storedY = y;
                    }
                }
            }
        }
        private void moveLogic(int x, int y) // Handles movement
        {
            Board tempBoard;
            int[,] possibleMoves;

            tempBoard = new Board(board);
            possibleMoves = board.GetValidMoves(storedX, storedY);

            for (int i = 0; i < possibleMoves.GetLength(0); i++)
            {
                if (board.squares[storedX, storedY].GetType().ToString().Equals("Backend.King")) // Logic branch for Castling
                {
                    King tempKing = (King)board.squares[storedX, storedY];
                    Rook tempRook;

                    if (!tempKing.hasMoved && y == storedY && Math.Abs(x - storedX) == 2)
                    {
                        int rookIndex = 0;
                        int direction = -1;

                        if (x > storedX)
                        {
                            rookIndex = 7;
                            direction = 1;
                        }

                        if (board.squares[rookIndex, y].GetType().ToString().Equals("Backend.Rook"))
                        {
                            tempRook = (Rook)board.squares[rookIndex, y];
                            if (!tempRook.hasMoved)
                            {
                                if (direction == -1 &&             // Scan for pieces blocking the way in the proper direction
                                    board.squares[y, 1] == null &&
                                    board.squares[y, 2] == null &&
                                    board.squares[y, 3] == null ||
                                    direction == 1 &&
                                    board.squares[y, 5] == null &&
                                    board.squares[y, 6] == null)
                                {
                                    if (!tempBoard.Check(board.squares[storedX, storedY].Color)) // The next 3 IF statements are for the rules that you cannot castle into, out of, or through check
                                    {
                                        tempBoard.MovePiece(storedX, y, storedX + direction, y);

                                        if (!tempBoard.Check(board.squares[storedX + direction, storedY].Color))
                                        {
                                            tempBoard.MovePiece(storedX, y, storedX + (direction * 2), y);

                                            if (!tempBoard.Check(board.squares[storedX + (direction * 2), storedY].Color))
                                            {
                                                board.MovePiece(storedX, storedY, x, y);                           // Congratulations, sucessful castle
                                                board.MovePiece(rookIndex, storedY, storedX + direction, storedY);
                                                storedX = -1;
                                                storedY = -1;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (possibleMoves[i, 0] == x && possibleMoves[i, 1] == y)
                {
                    tempBoard.MovePiece(storedX, storedY, x, y);
                    if (!tempBoard.Check(tempBoard.squares[x, y].Color))
                    {
                        board.MovePiece(storedX, storedY, x, y);
                        UpdateBoard(board);

                        if(board.squares[x,y].GetType().ToString().Equals("Backend.King"))
                        {
                            King tempKing = (King)board.squares[x, y];
                            tempKing.hasMoved = true;
                        }
                        if (board.squares[x, y].GetType().ToString().Equals("Backend.Rook"))
                        {
                            Rook tempRook = (Rook)board.squares[x, y];
                            tempRook.hasMoved = true;
                        }

                        if (currentColor == TeamColor.WHITE)
                        {
                            if (board.Check(TeamColor.BLACK) && board.Checkmate(TeamColor.BLACK))
                            {
                                MessageBox.Show("WHITE wins!");
                                Environment.Exit(0); // Enviroment.Exit closes the program. Game ends at checkmate, right?
                            }
                            currentColor = TeamColor.BLACK;

                        }
                        else
                        {
                            if (board.Check(TeamColor.WHITE) && board.Checkmate(TeamColor.BLACK))
                            {
                                MessageBox.Show("BLACK wins!");
                                Environment.Exit(0);
                            }
                            currentColor = TeamColor.WHITE;
                        }

                        this.Title = currentColor.ToString() + " Turn!";
                    }
                    break;
                }
                

                tempBoard.DestroyBoard();
                storedX = -1;
                storedY = -1;
            }
        }
    }
}