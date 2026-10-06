using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleShootingGame
{
    public partial class Form1 : Form
    {
        // 間隔は timer1 の Tick 回数で指定する。
        private const int PlayerSpeed = 10;
        private const int BulletSpeed = 10;
        private const int EnemySpeed = 3;
        private const int ShotInterval = 12;
        private const int EnemySpawnInterval = 60;
        private const int EnemySize = 40;
        private const int ScorePerEnemy = 100;

        private readonly List<PictureBox> bullets = new List<PictureBox>();
        private readonly List<PictureBox> enemies = new List<PictureBox>();
        private readonly Random random = new Random();

        private bool leftKey;
        private bool rightKey;
        private bool spaceKey;
        private bool isPlaying;
        private int shotCooldown;
        private int enemySpawnCooldown;
        private int score;

        public Form1()
        {
            InitializeComponent();

            timer1.Stop();
            KeyPreview = true;
            InitializeTitleScreen();
            ResetPlayerPosition();
            UpdateScoreLabel();
        }

        private void InitializeTitleScreen()
        {
            startLabel.Text = "SIMPLE SHOOTING GAME";
            startLabel.AutoSize = false;
            startLabel.Size = new Size(400, 60);
            startLabel.Location = new Point((ClientSize.Width - startLabel.Width) / 2, 180);
            startLabel.TextAlign = ContentAlignment.MiddleCenter;
            startLabel.Visible = true;

            ConfigureButton(startButton, "START");
            ConfigureButton(restartButton, "RESTART");
            startButton.Visible = true;
            restartButton.Visible = false;
        }

        private void ConfigureButton(Button button, string text)
        {
            button.Text = text;
            button.Size = new Size(160, 50);
            button.Location = new Point((ClientSize.Width - button.Width) / 2, 270);
        }

        // デザイナー側のイベント接続をそのまま利用する。
        private void startButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            timer1.Stop();
            ClearObjects(bullets);
            ClearObjects(enemies);
            ResetKeys();
            ResetPlayerPosition();

            score = 0;
            shotCooldown = 0;
            enemySpawnCooldown = EnemySpawnInterval;
            UpdateScoreLabel();

            startLabel.Visible = false;
            startButton.Visible = false;
            restartButton.Visible = false;

            CreateEnemy();
            isPlaying = true;
            ActiveControl = null;
            Focus();
            timer1.Start();
        }

        private void EndGame()
        {
            isPlaying = false;
            timer1.Stop();
            ResetKeys();
            restartButton.Visible = true;
            restartButton.BringToFront();
            MessageBox.Show(this, "GAME OVER");
        }

        private void ResetPlayerPosition()
        {
            playerPictureBox.Left = (ClientSize.Width - playerPictureBox.Width) / 2;
            playerPictureBox.Top = ClientSize.Height - playerPictureBox.Height - 20;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isPlaying)
            {
                SetKeyState(e.KeyCode, true);
            }
        }

        private void Form1_KeyUp_1(object sender, KeyEventArgs e)
        {
            SetKeyState(e.KeyCode, false);
        }

        private void SetKeyState(Keys key, bool isPressed)
        {
            switch (key)
            {
                case Keys.Left:
                    leftKey = isPressed;
                    break;
                case Keys.Right:
                    rightKey = isPressed;
                    break;
                case Keys.Space:
                    spaceKey = isPressed;
                    break;
            }
        }

        private void ResetKeys()
        {
            leftKey = false;
            rightKey = false;
            spaceKey = false;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            // ウィンドウを切り替えたときのキー押しっぱなしを防ぐ。
            ResetKeys();
            base.OnDeactivate(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                return;
            }

            MovePlayer();
            UpdateShooting();
            UpdateBullets();
            UpdateEnemySpawning();
            UpdateEnemies();
        }

        private void MovePlayer()
        {
            int direction = (rightKey ? 1 : 0) - (leftKey ? 1 : 0);
            int maxLeft = Math.Max(0, ClientSize.Width - playerPictureBox.Width);
            int nextLeft = playerPictureBox.Left + direction * PlayerSpeed;
            playerPictureBox.Left = Math.Max(0, Math.Min(maxLeft, nextLeft));
        }

        private void UpdateShooting()
        {
            if (shotCooldown > 0)
            {
                shotCooldown--;
            }

            if (spaceKey && shotCooldown == 0)
            {
                Shoot();
                shotCooldown = ShotInterval;
            }
        }

        private void Shoot()
        {
            PictureBox bullet = new PictureBox
            {
                Size = new Size(5, 15),
                BackColor = Color.Red
            };
            bullet.Left = playerPictureBox.Left + playerPictureBox.Width / 2 - bullet.Width / 2;
            bullet.Top = playerPictureBox.Top - bullet.Height;

            Controls.Add(bullet);
            bullets.Add(bullet);
        }

        private void UpdateBullets()
        {
            // 削除しても未処理の要素の添字がずれないよう、末尾から処理する。
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                PictureBox bullet = bullets[i];
                bullet.Top -= BulletSpeed;

                int hitIndex = FindHitEnemy(bullet);
                if (hitIndex >= 0)
                {
                    RemoveObject(bullets, i);
                    RemoveObject(enemies, hitIndex);
                    ChangeScore(ScorePerEnemy);
                    continue;
                }

                if (bullet.Bottom < 0)
                {
                    RemoveObject(bullets, i);
                }
            }
        }

        private int FindHitEnemy(PictureBox bullet)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (bullet.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    return i;
                }
            }

            return -1;
        }

        private void UpdateEnemySpawning()
        {
            if (enemySpawnCooldown > 0)
            {
                enemySpawnCooldown--;
            }

            if (enemySpawnCooldown == 0)
            {
                CreateEnemy();
                enemySpawnCooldown = EnemySpawnInterval;
            }
        }

        private void CreateEnemy()
        {
            int maxLeft = Math.Max(0, ClientSize.Width - EnemySize);
            PictureBox enemy = new PictureBox
            {
                Size = new Size(EnemySize, EnemySize),
                BackColor = Color.Blue,
                Left = random.Next(maxLeft + 1),
                Top = -EnemySize
            };

            Controls.Add(enemy);
            enemies.Add(enemy);
        }

        private void UpdateEnemies()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                PictureBox enemy = enemies[i];
                enemy.Top += EnemySpeed;

                if (enemy.Bounds.IntersectsWith(playerPictureBox.Bounds))
                {
                    EndGame();
                    return;
                }

                if (enemy.Top > ClientSize.Height)
                {
                    RemoveObject(enemies, i);
                    ChangeScore(-ScorePerEnemy);
                }
            }
        }

        private void ChangeScore(int amount)
        {
            score += amount;
            UpdateScoreLabel();
        }

        private void UpdateScoreLabel()
        {
            scoreLabel.Text = "SCORE: " + score;
        }

        private void RemoveObject(List<PictureBox> objects, int index)
        {
            PictureBox pictureBox = objects[index];
            objects.RemoveAt(index);
            Controls.Remove(pictureBox);
            pictureBox.Dispose();
        }

        private void ClearObjects(List<PictureBox> objects)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                RemoveObject(objects, i);
            }
        }
    }
}
