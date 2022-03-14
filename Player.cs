using System.Drawing;
using System.Windows.Forms;

namespace SeaWars
{
    class Player
    {
        public Player(int[,] enemyMap)
        {
            this.enemyMap = enemyMap;
        }

        private int[,] enemyMap;

        public bool Shoot(Button target)
        {
            bool hit;
            var position = (Point)target.Tag;

            if (enemyMap[position.Y, position.X] != 0)
            {
                hit = true;
                enemyMap[position.Y, position.X] = 0;
                target.BackColor = Color.Blue;
                target.Text = "X";
            }
            else
            {
                hit = false;
                target.BackColor = Color.Black;
            }

            return hit;
        }
    }
}
