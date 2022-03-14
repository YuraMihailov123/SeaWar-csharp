using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaWars
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Text = "Морской бой";
            CreateControls();
            bot = new Bot(enemyMap, playerMap, playerButtons);
            player = new Player(enemyMap);
            NewGame();
        }

        public const int mapSize = 10;

        private const int cellSize = 30;
        private const string alphabet = "АБВГДЕЖЗИК";

        private readonly int[,] playerMap = new int[mapSize, mapSize];
        private readonly int[,] enemyMap = new int[mapSize, mapSize];
        private readonly Player player;
        private readonly Bot bot;

        private Button[,] playerButtons;
        private Button[,] enemyButtons;
        private bool isPlaying = false;

        public void NewGame()
        {
            isPlaying = false;
            ClearMaps();
            bot.SetShips();
        }

        private void ClearMaps()
        {
            Array.Clear(playerMap, 0, playerMap.Length);
            Array.Clear(enemyMap, 0, enemyMap.Length);

            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    playerButtons[i, j].BackColor = Color.White;
                    playerButtons[i, j].Text = string.Empty;
                    enemyButtons[i, j].BackColor = Color.White;
                    enemyButtons[i, j].Text = string.Empty;
                }
            }
        }

        public void CreateControls()
        {
            int mapSizePx = mapSize * cellSize;
            Width = mapSizePx * 2 + 60;
            Height = mapSizePx + 115;

            playerButtons = CreateMap(10, 30, SetOrRemoveShip);
            enemyButtons = CreateMap(mapSizePx + 35, 30, Turn);

            Label lbMap1 = new Label
            {
                Text = "Карта игрока",
                Location = new Point(10, 10)
            };
            Controls.Add(lbMap1);

            Label lbMap2 = new Label
            {
                Text = "Карта противника",
                Location = new Point(mapSizePx + 35, 10)
            };
            Controls.Add(lbMap2);

            Button btStart = new Button
            {
                Text = "Начать",
                Location = new Point(10, mapSizePx + 40)
            };
            btStart.Click += new EventHandler(Start);
            Controls.Add(btStart);
        }

        private Button[,] CreateMap(int offsetX, int offsetY, EventHandler handler)
        {
            var buttons = new Button[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Button button = new Button
                    {
                        Location = new Point(j * cellSize + offsetX, i * cellSize + offsetY),
                        Size = new Size(cellSize, cellSize),
                    };
                    if (i == 0 || j == 0)
                    {
                        button.BackColor = Color.Gray;
                        if (i == 0 && j > 0)
                            button.Text = alphabet[j - 1].ToString();
                        if (j == 0 && i > 0)
                            button.Text = i.ToString();
                    }
                    else
                    {
                        button.Click += handler;
                        button.Tag = new Point(j, i);
                    }
                    buttons[i, j] = button;
                    Controls.Add(button);
                }
            }
            return buttons;
        }

        private void Start(object sender, EventArgs e)
        {
            isPlaying = true;
        }

        private bool CheckMapIsEmpty(int[,] map)
        {
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    if (map[i, j] != 0)
                        return false;
                }
            }
            return true;
        }

        private void SetOrRemoveShip(object sender, EventArgs e)
        {
            if (isPlaying) return;

            var cell = sender as Button;
            var cellPosition = (Point)cell.Tag;

            if (playerMap[cellPosition.Y, cellPosition.X] == 0)
            {
                cell.BackColor = Color.Red;
                playerMap[cellPosition.Y, cellPosition.X] = 1;
            }
            else
            {
                cell.BackColor = Color.White;
                playerMap[cellPosition.Y, cellPosition.X] = 0;
            }
        }

        private void Turn(object sender, EventArgs e)
        {
            if (!isPlaying) return;

            Button target = sender as Button;
            bool hit = player.Shoot(target);
            if (CheckMapIsEmpty(enemyMap))
            {
                MessageBox.Show("ПОБЕДА");
                NewGame();
                return;
            }

            if (!hit) TurnOfBot();
        }
        private void TurnOfBot()
        {
            bool isKeepShoting;
            do
            {
                isKeepShoting = bot.Shoot();
                if (CheckMapIsEmpty(playerMap))
                {
                    MessageBox.Show("ПОРАЖЕНИЕ");
                    NewGame();
                    return;
                }
            }
            while (isKeepShoting);
        }
    }
}
