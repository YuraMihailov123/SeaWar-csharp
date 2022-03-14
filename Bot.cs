using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaWars
{
    public class Bot
    {
        public Bot(int[,] map, int[,] playerMap, Button[,] playerButtons)
        {
            this.map = map;
            this.playerMap = playerMap;
            this.playerButtons = playerButtons;
        }

        private readonly int[,] map;
        private readonly int[,] playerMap;
        private readonly Button[,] playerButtons;

        public void SetShips()
        {
            int lengthShip = 4;
            int cycleValue = 1;
            int shipsCount = 10;
            Random r = new Random();

            int posX;
            int posY;

            while (shipsCount > 0)
            {
                for (int i = 0; i < cycleValue; i++)
                {
                    do
                    {
                        posX = r.Next(1, MainForm.mapSize);
                        posY = r.Next(1, MainForm.mapSize);
                    }
                    while (!IsInsideMap(posX, posY + lengthShip - 1) || !IsEmpty(posX, posY, lengthShip));

                    for (int k = posY; k < posY + lengthShip; k++)
                    {
                        map[posX, k] = 1;
                    }

                    shipsCount--;
                    if (shipsCount <= 0)
                        break;
                }
                cycleValue++;
                lengthShip--;
            }
        }

        public bool Shoot()
        {
            Random r = new Random();

            int posX = r.Next(1, MainForm.mapSize);
            int posY = r.Next(1, MainForm.mapSize);

            while (playerButtons[posX, posY].BackColor == Color.Blue || playerButtons[posX, posY].BackColor == Color.Black)
            {
                posX = r.Next(1, MainForm.mapSize);
                posY = r.Next(1, MainForm.mapSize);
            }

            if (playerMap[posX, posY] != 0)
            {
                playerMap[posX, posY] = 0;
                playerButtons[posX, posY].BackColor = Color.Blue;
                playerButtons[posX, posY].Text = "X";
                return true;
            }
            else
            {
                playerButtons[posX, posY].BackColor = Color.Black;
                return false;
            }
        }

        private bool IsInsideMap(int i, int j)
        {
            if (i < 0 || j < 0 || i >= MainForm.mapSize || j >= MainForm.mapSize)
            {
                return false;
            }
            return true;
        }

        private bool IsEmpty(int i, int j, int length)
        {
            bool isEmpty = true;

            for (int k = j; k < j + length; k++)
            {
                if (map[i, k] != 0)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }
    }
}
