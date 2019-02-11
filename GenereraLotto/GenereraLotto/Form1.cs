using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenereraLotto
{
    public partial class frmMain : Form
    {
        private Point[] gridPointsVertical = null;
        private Point[] gridPointsHorizontal = null;
        List<int> randomNumbers = null;
        int generationCount = 0;

        public frmMain()
        {            
            InitializeComponent();
            initGridPoints();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblGeneratedDescr.Text = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {            
            reDrawBoard();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            System.Security.Cryptography.RandomNumberGenerator rnd = System.Security.Cryptography.RandomNumberGenerator.Create();
            byte[] randomData = new byte[10];
            randomNumbers = new List<int>();
            int ranmdomNumber;
            int fairNumbers = byte.MaxValue / 35;
            int rndByteMax = fairNumbers * 35;

            while (randomNumbers.Count < 7)
            {
                // Generate a random number between 1 and 35
                rnd.GetBytes(randomData);
                for (int i = 0; i < randomData.Length; i++)
                {
                    if (randomData[i] < rndByteMax)
                    {
                        ranmdomNumber = (randomData[i] % 35) + 1;
                        if (randomNumbers.Count == 7)
                            break;
                        else if (!randomNumbers.Contains(ranmdomNumber))
                            randomNumbers.Add(ranmdomNumber);
                    }
                }
            }

            pnlBoard.Refresh();
            lblGeneratedDescr.Text = "Genererade rader: " + (++generationCount);
        }

        private void reDrawBoard()
        {
            randomNumbers = null;
            pnlBoard.Refresh();            
        }

        private void initGridPoints()
        {
            gridPointsVertical = new Point[10];
            gridPointsHorizontal = new Point[10];

            int gridOffset = pnlBoard.Height / 6;
            int gridAdd = gridOffset;

            for (int i = 0; i < gridPointsHorizontal.Length; i += 2)
            {
                gridPointsHorizontal[i] = new Point(0, gridOffset);
                gridPointsHorizontal[i + 1] = new Point(pnlBoard.Width, gridOffset);

                gridPointsVertical[i] = new Point(gridOffset, 0);
                gridPointsVertical[i + 1] = new Point(gridOffset, pnlBoard.Height);

                gridOffset += gridAdd;
            }
        }

        private void pnlBoard_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Brushes.Black);
            for (int i = 0; i < gridPointsVertical.Length; i += 2)
            {
                e.Graphics.DrawLine(p, gridPointsHorizontal[i], gridPointsHorizontal[i + 1]);
                e.Graphics.DrawLine(p, gridPointsVertical[i], gridPointsVertical[i + 1]);
            }

            if (randomNumbers != null)
            {
                Font font = new System.Drawing.Font("Verdana", 20, FontStyle.Bold);
                int gridOffset = pnlBoard.Height / 6;
                int offsetX1 = 20;
                int offsetX2 = 10;
                int offsetY = 20;
                for (int i = 0; i < randomNumbers.Count; i++)
                {
                    Point coordinate;
                    if (randomNumbers[i] < 10)
                        coordinate = new Point((((randomNumbers[i] - 1) % 6) * gridOffset) + offsetX1, (((randomNumbers[i] - 1) / 6) * gridOffset) + offsetY);
                    else
                        coordinate = new Point((((randomNumbers[i] - 1) % 6) * gridOffset) + offsetX2, (((randomNumbers[i] - 1) / 6) * gridOffset) + offsetY);

                    e.Graphics.DrawString(randomNumbers[i].ToString(), font, Brushes.Black, coordinate);
                }
            }
        }

        private void lnkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Programmet är skapat av Martin Dahl 2012.\nMail: iddt@hotmail.com");
        }
    }
}
