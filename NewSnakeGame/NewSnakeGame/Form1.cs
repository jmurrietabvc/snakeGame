using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewSnakeGame
{
    public partial class Form1 : Form
    {
        List<PictureBox> gameList = new List<PictureBox>();
        int MainPieceSize = 26;
        int time = 10;
        PictureBox Food = new PictureBox();
        string Direction = "right";

        public Form1()
        {
            InitializeComponent();
            gameStart();
            // Hook up the KeyDown event to handle user input for snake movement
            this.KeyDown += Form1_KeyDown;
            this.Focus();
        }

        public void gameStart()
        {
            time = 10;
            Direction = "right";
            timer1.Interval = 200;
            gameList = new List<PictureBox>();

            // Starting Snake Pieces
            for (int i = 2; i > 0; i--)
            {
                CreateSnake(gameList, this, (i * MainPieceSize) + 70, 80);
            }
            CreateFood();
        }

        public void CreateSnake(List<PictureBox> ball_List, Form form, int position_x, int position_y)
        {
            PictureBox pb = new PictureBox();
            pb.Location = new Point(position_x, position_y);
            pb.Image = Properties.Resources.body_horizontal;
            pb.BackColor = Color.Transparent;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            ball_List.Add(pb);
            form.Controls.Add(pb);
        }

        private void CreateFood()
        {
            Random rnd = new Random();
            int f_width = rnd.Next(1, this.Width - MainPieceSize - 10);
            int f_height = rnd.Next(1, this.Height - MainPieceSize - 40);

            PictureBox pb = new PictureBox();
            pb.Location = new Point(f_width, f_height);
            pb.Image = Properties.Resources.apple;
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            Food = pb;
            this.Controls.Add(pb);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int nx = gameList[0].Location.X;
            int ny = gameList[0].Location.Y;

            // Set the snake's head image
            string headResourceName = "head_right_" + Direction;
            gameList[0].Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(headResourceName);

            for (int i = gameList.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    // Update the snake head's position
                    if (Direction == "right") nx += MainPieceSize;
                    else if (Direction == "left") nx -= MainPieceSize;
                    else if (Direction == "up") ny -= MainPieceSize;
                    else if (Direction == "down") ny += MainPieceSize;
                    gameList[0].Location = new Point(nx, ny);
                }
                else
                {
                    // Move the rest of the snake body
                    gameList[i].Location = new Point(gameList[i - 1].Location.X, gameList[i - 1].Location.Y);
                }
            }

            //Apple Hit detection
            for (int countPieces = 1; countPieces < gameList.Count; countPieces++)
            {
                if (gameList[countPieces].Bounds.IntersectsWith(Food.Bounds))
                {
                    this.Controls.Remove(Food); // Remove food from the map
                    time = Convert.ToInt32(timer1.Interval); // Add more time to the timer
                    if (time > 10)
                    {
                        timer1.Interval = time - 10;
                    }
                    lblPoints.Text = (Convert.ToInt32(lblPoints.Text) + 1).ToString();
                    CreateFood(); // Create new food
                    CreateSnake(gameList, this, gameList[gameList.Count - 1].Location.X * MainPieceSize, 0); // Create a new part of the snake
                }
            }

            //wall hit detection
            if ((gameList[0].Location.X >= this.Width - 15) || (gameList[0].Location.Y >= this.Height - 50) || (gameList[0]).Location.Y < -10 || (gameList[0].Location.X < -30))
            {
                restartGame();
            }

            for (int countPieces = 1; countPieces < gameList.Count; countPieces++)
            {
                if (gameList[0].Bounds.IntersectWith(gameList[coun]))
            }
        }

        public void restartGame()
        {
            foreach (PictureBox Snake in gameList) { this.Controls.Remove(Snake); }
            this.Controls.Remove(Food);
            gameStart();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle user input to change the snake's direction
            Direction = ((e.KeyCode & Keys.Up) == Keys.Up) ? "up" : Direction;
            Direction = ((e.KeyCode & Keys.Down) == Keys.Down) ? "down" : Direction;
            Direction = ((e.KeyCode & Keys.Left) == Keys.Left) ? "left" : Direction;
            Direction = ((e.KeyCode & Keys.Right) == Keys.Right) ? "right" : Direction;
        }


    }
}
