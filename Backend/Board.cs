﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class Board
    {
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

        public void MovePiece(int initalX, int initalY, int endX, int endY)
        {
            // Moves pieces. Overwrites any piece in the  destination
            ChessPiece temp = squares[initalX, initalY];
            squares[initalX, initalY] = null;
            squares[endX, endY] = temp;
        }

        public void DestroyBoard()
        {
            // Method to remove all references to ChessPiece objects so that the garbage collector can clean them up
            // Just in case it becomes a good idea to have this method
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    squares[x, y] = null;
                }
            }
        }

        public int[,] GetValidMoves(int x, int y)
        {
            // Returns an array with the legal moves for the piece at (x, y)
            throw new NotImplementedException();
        }

        private int[,] ExpandAndAddCoordinates(int[,] array, int x, int y)
        {
            // Expands an array's size by one and adds (x, y) to the list
            throw new NotImplementedException();
        }
    }
}
