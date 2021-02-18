using System;
using System.Drawing;
using System.Windows.Forms;
using static TTTGame;

namespace TTTGame2
{
    public partial class GuiTTTGame : Form
    {
        private RadioButton[,] buttons;
        private Color buttonBaseColor;
        private string resetButtonText;
        TTTGame curGame = new TTTGame();

        public GuiTTTGame()
        {
            InitializeComponent();
            SaveButtons();
            buttonBaseColor = resetButton.BackColor;
            resetButtonText = resetButton.Text;
        }

        private void SaveButtons()
        {
            buttons = new RadioButton[3, 3] {
                { radioButton1, radioButton2, radioButton3 },
                { radioButton4, radioButton5, radioButton6 },
                { radioButton7, radioButton8, radioButton9 }};
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            curGame.ResetGame();
            ResetButtons();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked)
            {
                // If pressed, rename & deactivate the button.
                button.Text = curGame.GetCurrentPlayer().ToString();
                button.Enabled = false;

                // Check which button was pressed.
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (button == buttons[x, y])
                        {
                            curGame.TryToPlacePiece(x, y);
                            ColorButton(button, Color.Black);
                            break;
                        }
                    }
                }

                // push-down all the buttons if the game is over.
                if (curGame.IsGameFinished())
                {
                    FinishTasks();
                }
            }
        }

        private void ResetButtons()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    buttons[x, y].Text = string.Empty;
                    buttons[x, y].Enabled = true;
                    ColorButton(buttons[x, y], buttonBaseColor);
                    buttons[x, y].Checked = false;
                }
            }
            resetButton.Text = resetButtonText;
        }

        private void FinishTasks()
        {
            // Painting the winning buttons.
            Point[] winIndices = curGame.GetWinnerIndexes();
            if (winIndices != null)
            {
                foreach (Point point in winIndices)
                {
                    ColorButton(buttons[point.X, point.Y], Color.Red);
                }
            }

            SetAllGameButtonsTo(false);
            resetButton.Text = GetWinnerString();
        }

        private string GetWinnerString()
        {
            Player winner = curGame.GetWinner();
            return (winner == Player.NONE) ? "תיקו !" : string.Format("המנצח הוא {0}!", winner);
        }

        private void SetAllGameButtonsTo(bool state)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    buttons[x, y].Enabled = state;
                }
            }
        }

        private void ColorButton(RadioButton curButton, Color color)
        {
            curButton.ForeColor = color;
        }
    }
}
