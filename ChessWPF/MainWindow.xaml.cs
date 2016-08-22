using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Backend;

namespace ChessWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Label[,] squares = new Label[8, 8];
        public MainWindow()
        {
            InitializeComponent();
            InitializeSquares();
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

            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    if(!mark.Equals(""))
                    {
                        mark = "";
                    }

                    if(board.squares[x,y] != null)
                    {
                        if(board.squares[x,y].Color == TeamColor.WHITE)
                        {
                            mark += "W";
                        }
                        else
                        {
                            mark += "B";
                        }

                        switch(board.squares[x, y].GetType().ToString())
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
    }
}
