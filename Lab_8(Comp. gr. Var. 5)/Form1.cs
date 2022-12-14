using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_8_Comp.gr.Var._5_
{
    public partial class Form1 : Form
    {

        const int UFOMax = 2;
        const int LasersMax = 5;
        const int PillarsMax = 5;

        bool StartFlag;
        bool FireFlag;

        int cooldownFire = 30;
        int Shooted = 0;

        PictureBox[] UFOs = new PictureBox[UFOMax];
        int UFOShootedCount = 0;
        int UFOSpeed = 5;

        PictureBox[] Lasers = new PictureBox[LasersMax];
        int LaserSpeed = 7;
        int LasersCount = 0;

        PictureBox[] Pillars = new PictureBox[PillarsMax];
        int PillarsFrequency = 9;
        int PillarsCount = 0;
        int PillarSpeed = 1;

        DateTime timeOfGame = DateTime.MinValue;

        public Form1()
        {
            InitializeComponent();
        }

        // Удаление столбов с изображения
        private void DeletePillar(int i)
        {
            Pillars[i].Dispose(); // освобождение ресурсов Pillars[i]
            for (int j = i; j < PillarsCount - 1; j++) { Pillars[j] = Pillars[j + 1]; }
            PillarsCount--;
        }

        // Удаление лазера
        private void DeleteLaser(int i)
        {
            Lasers[i].Dispose(); // освобождение ресурсов Laser[i]
            for (int j = i; j < LasersCount - 1; j++)
            { Lasers[j] = Lasers[j + 1]; }
            LasersCount--;
        }

        // Обработка события загрузки формы
        private void Form1_Load(object sender, EventArgs e)
        {
            // Устнавливаем все флажки при загрузке формы в false
            StartFlag = false; FireFlag = false;
            
            // Загружаем в pictureBoxGame изображение пушки
            for (int i = 0; i < UFOMax; i++)
            {
                UFOs[i] = new PictureBox();
                UFOs[i].Image = Image.FromFile(@"UFO_Serviceable.png");

                UFOs[i].BackColor = Color.Transparent; // Прозрачный фон НЛО

                UFOs[i].Size = new Size(UFOs[i].Image.Width, UFOs[i].Image.Height);

                if (i == 0)
                {
                    UFOs[i].Location = new Point(100, (pictureBoxGame.Height / 2) - UFOs[i].Image.Height);
                    UFOs[i].Name = "UFO_Allied";
                }
                else
                {
                    UFOs[i].Location = new Point(pictureBoxGame.Width - 200, (pictureBoxGame.Height / 2) - UFOs[i].Image.Height);
                    UFOs[i].Name = "UFO_Enemy";
                }

                pictureBoxGame.Controls.Add(UFOs[i]); // Добавляем НЛО на поле игры

                UFOs[i].BringToFront();
            }

        }

        // *** Таймер для создания столбов ***
        private void timer1_Tick_1(object sender, EventArgs e)
        {

            // Если флаг игры установлен
            if (StartFlag)
            { // Получаем случайное целое число меньше 900
                Random a = new Random();
                int RandomEvent = a.Next(900);
                // Новый самолёт
                if ((RandomEvent >= 0) & (RandomEvent < PillarsFrequency) &
                (PillarsCount < PillarsMax))
                { // Увеличиваем счётчик количества столбы на экране
                    PillarsCount++;
                    // Переименовываем столбы со старшими номерами
                    for (int i = PillarsCount - 1; i > 0; i--) { Pillars[i] = Pillars[i - 1]; }
                    // Создаём изображение нового столба
                    Pillars[0] = new PictureBox();

                    // который будет расположен сверху вниз
                    if ((RandomEvent > -1) & (RandomEvent <= PillarsFrequency / 2))
                    {
                        Pillars[0].Image = Image.FromFile(@"Pillar_180.png");
                        Pillars[0].Image.Tag = "TB";
                        Pillars[0].Location = new Point(pictureBoxGame.Width - Pillars[0].Image.Width, 0);
                    }
                    else // который будет расположен снизу вверх
                    {
                        Pillars[0].Image = Image.FromFile(@"Pillar_0.png");
                        Pillars[0].Image.Tag = "BT";
                        Pillars[0].Location = new Point(pictureBoxGame.Width - Pillars[0].Image.Width, pictureBoxGame.Height - Pillars[0].Image.Height);
                    }

                    // Устанавливаем прозрачный цвет фона столба
                    Pillars[0].BackColor = Color.Transparent;
                    Pillars[0].Size = new Size(Pillars[0].Image.Width,
                    Pillars[0].Image.Height);
                    Pillars[0].Name = "Pillar" + PillarsCount.ToString();
                    // Добавляем столб на поле игры
                    pictureBoxGame.Controls.Add(Pillars[0]);
                    Pillars[0].BringToFront();
                }
            }
        }

        // *** Таймер для движения объектов и стрельбы ***
        private void timer2_Tick(object sender, EventArgs e)
        {
            // Если игра запущена
            if (StartFlag)
            {
                // Уменьшаем тики времени до перезарядки НЛО
                if (Shooted > 0) { Shooted--; }
                // Если установлен флаг выстрела
                if (FireFlag)
                {
                    // и если пушка на НЛО готова к выстрелу
                    if (Shooted == 0 && LasersCount < LasersMax)
                    {
                        // Увеличиваем количество выпущенных лазеров
                        LasersCount++;
                        // переименовываем лазеры со старшими номерами
                        for (int i = LasersCount - 1; i > 0; i--)
                        {
                            Lasers[i] = Lasers[i - 1];
                        }
                        // Создаём изображение нового лазера
                        Lasers[0] = new PictureBox();
                        Lasers[0].Image = Image.FromFile(@"Laser.png");
                        Lasers[0].Location = new Point(UFOs[0].Location.X + UFOs[0].Image.Width / 2, UFOs[0].Location.Y + UFOs[0].Image.Height/2);
                        Lasers[0].Size = new Size(Lasers[0].Image.Width, Lasers[0].Image.Height);
                        Lasers[0].Name = "Lasers" + LasersCount.ToString();
                        // Добавляем снаряд на поле игры
                        pictureBoxGame.Controls.Add(Lasers[0]);
                        Lasers[0].BringToFront();
                        // Устанавливаем количество тиков до нового выстрела
                        Shooted = cooldownFire;
                    }
                }

                // Перемещение столбов и контроль на выход за границы
                for (int i = 0; i < PillarsCount; i++)
                {
                    // Смещаем столб влево на PillarSpeed точек
                    Pillars[i].Left = Pillars[i].Left - PillarSpeed;
                    // Если столб оказался у левой границы
                    if (Pillars[i].Left < 0 - Pillars[i].Width)
                    {
                        DeletePillar(i); // Удаляем столб
                    }

                }

                // Проверка на попадаение НЛО в Столб
                for (int i = 0; i < PillarsCount; i++)
                {
                    Rectangle r3 = Pillars[i].DisplayRectangle;

                    Rectangle r4 = UFOs[0].DisplayRectangle;

                    r3.Location = Pillars[i].Location;
                    r4.Location = UFOs[0].Location;

                    if (r3.IntersectsWith(r4))
                    {
                        timer2.Enabled = false;

                        MessageBox.Show("You LOSE!!");
                        
                        // Производим очистку и возвращаем все на место
                        buttonStart.Text = "Start the Game!"; StartFlag = false;
                        buttonStart.Focus();// Устанавливаем фокус на клавишу "Старт"

                        if (PillarsCount > 0)
                        {
                            for (int z = 0; z < PillarsMax; z++)
                            {
                                if (Pillars[z] != null) DeletePillar(z);
                                if (z + 1 == PillarsMax) Pillars[0].Dispose();

                            }
                            PictureBox[] NewPillars = new PictureBox[PillarsMax];
                            Pillars = NewPillars;
                        }

                        UFOs[1].Location = new Point(pictureBoxGame.Width - 200, (pictureBoxGame.Height / 2) - UFOs[1].Image.Height);
                        UFOs[0].Location = new Point(100, (pictureBoxGame.Height / 2) - UFOs[0].Image.Height);
                        UFOShootedCount = 0;
                        
                        timeOfGame = DateTime.MinValue;
                        
                        countOfLife.Text = (3 - UFOShootedCount).ToString();
                    }

                }

                Random random = new Random();
                int Random = random.Next(900);
                int countOfActions = random.Next(5);

                // Рандомное перемещение компьютера по полю
                if ( Random < 50 || Random >850 ) {
                    if (Random > 450)
                    {
                        for (int i = 0; i < countOfActions; i++)
                        {
                            if (UFOs[1].Top + UFOs[1].Height + UFOSpeed < pictureBoxGame.Height)
                            { UFOs[1].Top = UFOs[1].Top + UFOSpeed; }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < countOfActions; i++)
                        {
                            if (UFOs[1].Top >= UFOSpeed) { UFOs[1].Top = UFOs[1].Top - UFOSpeed; }
                        }
                    }
                }

                //Перемещение лазеров, контроль на выход за границы и попадание
                for (int i = 0; i < LasersCount; i++)
                { // Перемещаем лазер вправо на LasersSpeed точек
                    Lasers[i].Location = new Point(Lasers[i].Location.X + LaserSpeed, Lasers[i].Location.Y);

                    // Если лазер долетел до правой границы
                    if (Lasers[i].Location.X > pictureBoxGame.Width - Lasers[i].Image.Width)
                        DeleteLaser(i);  // Удаляем лазер


                    // Получаем координаты прямоугольной области лазер
                    Rectangle r1 = Lasers[i].DisplayRectangle;

                    // Проверка на попадание в НЛО
                    Rectangle r2 = UFOs[1].DisplayRectangle;

                    for (int j = 0; j < LasersCount; j++)
                    {
                        r1.Location = Lasers[i].Location;
                        r2.Location = UFOs[1].Location;
                        // Если прямоугольные области пересекаются
                        if (r1.IntersectsWith(r2))
                        {

                            DeleteLaser(i); // Удаляем лазер

                            UFOShootedCount++; //Увеличиваем количество попаданий
                            
                            // Если кол-во HP = 0
                            if (3 - UFOShootedCount == 0)
                            {
                                timer2.Enabled = false;

                                MessageBox.Show("You WIN!");

                                // Производим очистку и возвращаем все на места
                                buttonStart.Text = "Start the Game!";
                                buttonStart.Focus();// Устанавливаем фокус на клавишу "Старт"
                                StartFlag = false;

                                if (PillarsCount > 0)
                                {
                                    for (int z = 0; z < PillarsMax; z++)
                                    {
                                        if (Pillars[z] != null) DeletePillar(z);
                                        if (z + 1 == PillarsMax) Pillars[0].Dispose();
                                    }
                                    PictureBox[] NewPillars = new PictureBox[PillarsMax];
                                    Pillars = NewPillars;
                                }

                                UFOs[1].Location = new Point(pictureBoxGame.Width - 200, (pictureBoxGame.Height / 2) - UFOs[1].Image.Height);
                                UFOs[0].Location = new Point(100, (pictureBoxGame.Height / 2) - UFOs[0].Image.Height);
                                UFOShootedCount = 0;
                                
                                timeOfGame = DateTime.MinValue;
                                countOfLife.Text = (3 - UFOShootedCount).ToString();
                            }

                        }
                    }
                }

                // Изменяем значения счётчиков игры на экране
                countOfLife.Text = (3 - UFOShootedCount).ToString();
                timeOfGame = timeOfGame.AddSeconds(1);
                timer.Text = timeOfGame.ToString("HH:mm:ss");
            }
        }

        // События клавиатуры будут проходить через обработчики pictureBoxGame
        private void pictureBoxGame_PreviewKeyDown(object sender,
        PreviewKeyDownEventArgs e)
        { // чтобы обрабатывались клавиши со стрелками "вверх-вниз"
            e.IsInputKey = true;
        }

        // Обработка нажатия клавиш
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        { // Перемещение НЛО вниз на UFOSpeed точек
            if (e.KeyCode == Keys.Down)
            {
                if (UFOs[0].Top + UFOs[0].Height + UFOSpeed < pictureBoxGame.Height)
                { UFOs[0].Top = UFOs[0].Top + UFOSpeed; }
            }
            // Перемещение НЛО вверх на UFOSpeed точек
            if (e.KeyCode == Keys.Up)
            {
                if (UFOs[0].Top >= UFOSpeed) { UFOs[0].Top = UFOs[0].Top - UFOSpeed; }
            }

            // Установка флага FireFlag по нажатию клавиши "пробел"
            if (e.KeyCode == Keys.Space) { FireFlag = true; }
            
            // Остановка и возобновление игры по нажатию клавиши "Enter"
            if (e.KeyCode == Keys.Enter) { ButtonStart_Click(sender, e); }
        }

        // Обработка отпускания клавиш
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        { // Сброс флага FireFlag по отпусканию клавиши "пробел"
            if (e.KeyCode == Keys.Space) { FireFlag = false; }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "Start the Game!")
            {
                timer2.Enabled = true;
                buttonStart.Text = "Pause"; StartFlag = true;
                pictureBoxGame.Focus(); // Устанавливаем фокус на поле игры
                return;
            }
            if (buttonStart.Text == "Pause")
            {
                buttonStart.Text = "Start the Game!"; StartFlag = false;
                buttonStart.Focus();// Устанавливаем фокус на клавишу "Start the Game!"
            }
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            if (PillarsCount > 0)
            {
                for (int z = 0; z < PillarsMax ; z++)
                {
                    if (Pillars[z] != null) DeletePillar(z);
                    if (z+1 == PillarsMax) Pillars[0].Dispose();

                }

                PictureBox[] NewPillars = new PictureBox[PillarsMax];
                Pillars = NewPillars;
            }

            UFOs[1].Location = new Point(pictureBoxGame.Width - 200, (pictureBoxGame.Height / 2) - UFOs[1].Image.Height);
            UFOs[0].Location = new Point(100, (pictureBoxGame.Height / 2) - UFOs[0].Image.Height);
            UFOShootedCount = 0;
            
            timeOfGame = DateTime.MinValue;
            countOfLife.Text = (3 - UFOShootedCount).ToString();
        }


    }
}
