namespace SimpleShootingGame
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            playerPictureBox = new PictureBox();
            timer1 = new System.Windows.Forms.Timer(components);
            scoreLabel = new Label();
            startLabel = new Label();
            startButton = new Button();
            restartButton = new Button();
            ((System.ComponentModel.ISupportInitialize)playerPictureBox).BeginInit();
            SuspendLayout();
            // 
            // playerPictureBox
            // 
            playerPictureBox.BackColor = SystemColors.ActiveCaption;
            playerPictureBox.Location = new Point(340, 455);
            playerPictureBox.Name = "playerPictureBox";
            playerPictureBox.Size = new Size(50, 50);
            playerPictureBox.TabIndex = 0;
            playerPictureBox.TabStop = false;
            // 
            // timer1
            // 
            timer1.Interval = 16;
            timer1.Tick += timer1_Tick;
            // 
            // scoreLabel
            // 
            scoreLabel.AutoSize = true;
            scoreLabel.Location = new Point(711, 18);
            scoreLabel.Name = "scoreLabel";
            scoreLabel.Size = new Size(54, 15);
            scoreLabel.TabIndex = 1;
            scoreLabel.Text = "SCORE: 0";
            // 
            // startLabel
            // 
            startLabel.AutoSize = true;
            startLabel.Location = new Point(302, 266);
            startLabel.Name = "startLabel";
            startLabel.Size = new Size(144, 15);
            startLabel.TabIndex = 2;
            startLabel.Text = "SIMPLE SHOOTING GAME";
            // 
            // startButton
            // 
            startButton.Location = new Point(325, 312);
            startButton.Name = "startButton";
            startButton.Size = new Size(75, 23);
            startButton.TabIndex = 3;
            startButton.Text = "START";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // restartButton
            // 
            restartButton.Location = new Point(325, 354);
            restartButton.Name = "restartButton";
            restartButton.Size = new Size(75, 23);
            restartButton.TabIndex = 4;
            restartButton.Text = "RESTART";
            restartButton.UseVisualStyleBackColor = true;
            restartButton.Click += restartButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(restartButton);
            Controls.Add(startButton);
            Controls.Add(startLabel);
            Controls.Add(scoreLabel);
            Controls.Add(playerPictureBox);
            Name = "Form1";
            Text = "Form1";
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp_1;
            ((System.ComponentModel.ISupportInitialize)playerPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox playerPictureBox;
        private System.Windows.Forms.Timer timer1;
        private Label scoreLabel;
        private Label startLabel;
        private Button startButton;
        protected Button restartButton;
    }
}
