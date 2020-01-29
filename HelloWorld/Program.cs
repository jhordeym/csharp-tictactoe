using System;
using System.Drawing;
using System.Collections.Generic;
using Console = Colorful.Console;

namespace HelloWorld
{
    class Board
    {
        public class Coordinates
        {
            private int x, y;

            public Coordinates(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public int getX()
            {
                return this.x;
            }
            public int getY()
            {
                return this.y;
            }

            public override string ToString()
            {
                return "X: " + x + ",Y: " + y;
            }

        }

        private string[,] board;
        public Board()
        {
            this.board = new string[3, 3];
            this.clearBoard();
        }

        public void clearBoard()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    this.board[i, j] = " ";
                }
            }
        }

        public void printBoard()
        {
            Console.WriteLine("# Board:");

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write("[" + this.board[i, j] + "]");
                }
                Console.WriteLine("");
            }
        }

        public bool updateBoard(Coordinates coordinates, string playerToken)
        {
            // should never happen
            int x = coordinates.getX(), y = coordinates.getY();
            if (x < 0 || x > 2 || y < 0 || y > 2)
            {
                throw new Exception("Invalid coordinates");
            }

            if (this.board[x, y].Equals(" "))
            {
                this.board[x, y] = playerToken;
                return true;
            }
            return false;
        }

        public bool checkWinner(string playerToken)
        {
            // rows
            if (this.board[0, 0].Equals(playerToken)
                && this.board[0, 1].Equals(playerToken)
                && this.board[0, 2].Equals(playerToken)) { return true; }

            if (this.board[1, 0].Equals(playerToken)
                && this.board[1, 1].Equals(playerToken)
                && this.board[1, 2].Equals(playerToken)) { return true; }

            if (this.board[2, 0].Equals(playerToken)
                && this.board[2, 1].Equals(playerToken)
                && this.board[2, 2].Equals(playerToken)) { return true; }

            // columns
            if (this.board[0, 0].Equals(playerToken)
                && this.board[1, 0].Equals(playerToken)
                && this.board[2, 0].Equals(playerToken)) { return true; }

            if (this.board[0, 1].Equals(playerToken)
                && this.board[1, 1].Equals(playerToken)
                && this.board[2, 1].Equals(playerToken)) { return true; }

            if (this.board[0, 2].Equals(playerToken)
                && this.board[1, 2].Equals(playerToken)
                && this.board[2, 2].Equals(playerToken)) { return true; }

            // diagonal
            if (this.board[0, 0].Equals(playerToken)
                && this.board[1, 1].Equals(playerToken)
                && this.board[2, 2].Equals(playerToken)) { return true; }

            if (this.board[0, 2].Equals(playerToken)
                && this.board[1, 1].Equals(playerToken)
                && this.board[2, 0].Equals(playerToken)) { return true; }

            return false;
        }
    }

    enum PlayerId
    {
        p1, p2
    }

    class Player
    {
        public PlayerId id;
        public Color color;

        public Player(PlayerId id, Color color)
        {
            this.id = id;
            this.color = color;
        }

        public Board.Coordinates makeMove()
        {
            bool validInput = false;
            ConsoleKeyInfo inputkey;
            Board.Coordinates result;

            var keysMap = new Dictionary<ConsoleKey, Board.Coordinates>();
            keysMap.Add(ConsoleKey.Q, new Board.Coordinates(0, 0));
            keysMap.Add(ConsoleKey.W, new Board.Coordinates(0, 1));
            keysMap.Add(ConsoleKey.E, new Board.Coordinates(0, 2));

            keysMap.Add(ConsoleKey.A, new Board.Coordinates(1, 0));
            keysMap.Add(ConsoleKey.S, new Board.Coordinates(1, 1));
            keysMap.Add(ConsoleKey.D, new Board.Coordinates(1, 2));

            keysMap.Add(ConsoleKey.Z, new Board.Coordinates(2, 0));
            keysMap.Add(ConsoleKey.X, new Board.Coordinates(2, 1));
            keysMap.Add(ConsoleKey.C, new Board.Coordinates(2, 2));

            do
            {
                Console.Write(
                "[Q][W][E] \n" +
                "[A][S][D] \n" +
                "[Z][X][C] \n" +
                "# Select one: ");
                inputkey = Console.ReadKey();
                Console.WriteLine("\n");
                validInput = keysMap.TryGetValue(inputkey.Key, out result);
            } while (validInput == false);
            return result;
        }
    }

    class Game
    {
        public PlayerId turn = PlayerId.p1;

        private Board board;
        private Player p1;
        private Player p2;

        private bool gameOver = false;

        public Game()
        {
            this.printWelcome();
            this.board = new Board();
            this.p1 = new Player(PlayerId.p1, Color.Red);
            this.p2 = new Player(PlayerId.p2, Color.Blue);
            this.board.printBoard();
        }

        public void printWelcome()
        {
            int DA = 208;
            int V = 140;
            int ID = 255;
            Console.WriteAscii("TIC-TAC-TOE BY JHAVIS", Color.FromArgb(DA, V, ID));
        }

        public void won(Player p)
        {
            Console.WriteAscii(p.id + "WON!", p.color);
        }

        public void draw()
        {
            Console.WriteAscii("DRAW!");
        }

        public void start()
        {
            int moves = 0;
            while (!gameOver)
            {
                if(moves == 8)
                {
                    gameOver = true;
                }
                bool checkWinnerP1 = this.board.checkWinner("X");
                bool checkWinnerP2 = this.board.checkWinner("O");
                if (checkWinnerP1)
                {
                    won(p1);
                    return;
                }
                else if (checkWinnerP2)
                {
                    won(p2);
                    return;
                }

                switch (turn)
                {
                    case PlayerId.p1:
                        {
                            Console.WriteAscii(PlayerId.p1.ToString() + " - X", p1.color);

                            bool updateMove = false;
                            do
                            {
                                var move = this.p1.makeMove();
                                updateMove = this.board.updateBoard(move, "X");
                            } while (updateMove == false);

                            this.turn = PlayerId.p2;
                            this.board.printBoard();
                            break;
                        }
                    case PlayerId.p2:
                        {
                            Console.WriteAscii(PlayerId.p2.ToString() + " - O", p2.color);
                            bool updateMove = false;
                            do
                            {
                                var move = this.p2.makeMove();
                                updateMove = this.board.updateBoard(move, "O");
                            } while (updateMove == false);

                            this.turn = PlayerId.p1;
                            this.board.printBoard();
                            break;
                        }
                }
                moves++;
            }
            draw();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.start();
        }
    }
}
