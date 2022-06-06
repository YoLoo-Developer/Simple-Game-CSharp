using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Game
{
    static class Game
    {
        private static Timer timerUpdate = new Timer { Interval = 50 };
        private static Timer timerLevel = new Timer { Interval = 2500 };
        private static BufferedGraphicsContext context;
        public static BufferedGraphics Buffer;
        /// <summary>
        /// Ширина игрового поля
        /// </summary> 
        public static int Width { get; set; }
        /// <summary>
        /// Высота игрового поля
        /// </summary> 
        public static int Height { get; set; }
        /// <summary>
        /// Массив звезд
        /// </summary>
        public static Star[] stars;
        /// <summary>
        /// Список астероидов
        /// </summary>
        public static List<Asteroid> asteroids = new List<Asteroid>();
        /// <summary>
        /// Список пуль
        /// </summary>
        public static List<Rocket> rockets = new List<Rocket>();
        /// <summary>
        /// Список аптечек
        /// </summary>
        public static List<Heart> hearts = new List<Heart>();
        /// <summary>
        /// Подключения рандома
        /// </summary>
        public static Random random = new Random();
        /// <summary>
        /// Корабль игрока
        /// </summary>
        public static Ship ship;
        /// <summary>
        /// Очки за сбитые астероиды
        /// </summary>
        public static int score;
        /// <summary>
        /// Количество асстероидов
        /// </summary>
        public static int allAsteroid = 15;
        /// <summary>
        /// Колличество асстероидов отображаемые на экране
        /// </summary>
        public static int amountA = 0;
        /// <summary>
        /// Отображать какой уровень
        /// </summary>
        public static bool display = true;
        /// <summary>
        /// Текущий уровень сложности
        /// </summary>
        public static int level = 1;
        /// <summary>
        /// Текущий номер ракеты, и сердца
        /// </summary>
        public static int numberR = 0, numberH = 0;

        static Game()
        {

        }

        /// <summary>
        /// Остановка игры(таймеров)
        /// </summary>
        public static void GameOver()
        {
            timerUpdate.Stop();
            Buffer.Graphics.DrawString("GameOver", new Font(FontFamily.GenericSansSerif, 30, FontStyle.Underline), Brushes.White, Game.Width / 3f, Game.Height / 3f);
            Buffer.Graphics.DrawString("Score:" + score, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Underline), Brushes.White, Game.Width / 3f, Game.Height / 3f + 50);
            Buffer.Graphics.DrawString("Level:" + level, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Underline), Brushes.White, Game.Width / 3f, Game.Height / 3f + 75);
            Buffer.Render();
        }

        /// <summary>
        /// Загрузка объектов
        /// </summary>
        public static void Load()
        {
            BaseObject.CreateObject += s => Console.WriteLine(s);
            BaseObject.CollisionObject += s => Console.WriteLine(s);
            int i = 0;
            stars = new Star[(Height / 100) * (Width / 100)];          
            for (int y = 0; y < Height / 100; y++)
            {
                for (int x = 0; x < Width / 100; x++)
                {
                    stars[i] = new Star(new Point(x * 120 + random.Next(10, 50), y * 120 + random.Next(0, 50)), new Point(0, random.Next(2, 4)), new Size(10, 10), "Star");
                    i++;
                }
            }
            for (int a = 0; a < allAsteroid; a++)
            {
                int size = random.Next(25, 40);
                asteroids.Add(new Asteroid(new Point(random.Next(0, Width), -random.Next(100, 500)), new Point(random.Next(-2, 2),
                    random.Next(2, 4)), new Size(size, size), "Asteroid"));
            }
            for (int r = 0; r < 30; r++)
            {
                rockets.Add(new Rocket(new Point(0, 0), new Point(0, -1), new Size(20, 20), "Rocket"));
                Disconnect(rockets[r]);
            }
            for (int h = 0; h < 10; h++)
            {
                hearts.Add(new Heart(new Point(0, 0), new Point(0, 3), new Size(15, 15), "Heart"));
                Disconnect(hearts[h]);
            }
            amountA = allAsteroid;
            ship = new Ship(new Point(Width / 2, Height - 30), new Point(7, 7), new Size(30, 30), "Ship");
        }

        /// <summary>
        /// Иницилизация обьектов
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            Graphics g; // Графическое устройство для вывода 
            context = BufferedGraphicsManager.Current; // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            g = form.CreateGraphics(); // Создаем объект (поверхность рисования) и связываем его с формой
            Width = form.ClientSize.Width; 
            Height = form.ClientSize.Height;
            if (Width > 1000 || Height > 1000)
                throw new ArgumentOutOfRangeException();
            Buffer = context.Allocate(g, new Rectangle(0, 0, Width, Height)); // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            timerUpdate.Start();
            timerUpdate.Tick += TimerUpdate;
            timerLevel.Start();
            timerLevel.Tick += TimerLevel;
            form.KeyDown += FormKeyDown;
            Ship.MessageDestroy += GameOver;
            Load();
        }

        /// <summary>
        /// Обработка нажатий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                TurnOn(rockets[numberR], new Point((ship.Rect.X + 5), (ship.Rect.Y + 4)));
                if (numberR < 29)
                    numberR++;
                else
                    numberR = 0;
            }
            if (e.KeyCode == Keys.Right)
                ship.Right();
            if (e.KeyCode == Keys.Left)
                ship.Left();
        }

        /// <summary>
        /// Отрисовка графики на экран
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in stars)
                obj.Draw();
            foreach (BaseObject obj in asteroids)
                obj.Draw();
            foreach (BaseObject obj in rockets)
                obj.Draw();
            foreach (BaseObject obj in hearts)
                obj.Draw();
            ship?.Draw();
            if (ship != null)
                Buffer.Graphics.DrawString("Energy:" + ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Graphics.DrawString("Score:" + score, SystemFonts.DefaultFont, Brushes.White, 0, 15);           
            if (display)
                Buffer.Graphics.DrawString("Level:" + level, new Font(FontFamily.GenericSansSerif, 30, FontStyle.Underline), Brushes.White, Game.Width / 3f, Game.Height / 2.5f);
            Buffer.Render();
        }

        /// <summary>
        /// Обновление раз в 100 миллисекунд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerUpdate(object sender, EventArgs e)
        {          
            Draw();
            Update();
        }
        /// <summary>
        /// Обновление раз в 2500 миллисекунд(2.5 секунд)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerLevel(object sender, EventArgs e)
        {
            display = false;
            timerLevel.Stop();
        }

        /// <summary>
        /// Обновления раз в кадр
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in stars)
                obj.Update();
            for (int a = 0; a < asteroids.Count; a++)
            {
                asteroids[a].Update();
                if (ship.Collision(asteroids[a]))
                {
                    ship.Collision("Asteroid");
                    ship.AddEnergy(-random.Next(10, 25));
                    amountA--;
                    Disconnect(asteroids[a]);
                    if (ship.Energy <= 0) ship.Destroy();
                }
                for (int r = 0; r < rockets.Count; r++)
                {
                    rockets[r].Update();
                    try
                    {
                        if (!rockets[r].Disconnect && !asteroids[a].Disconnect && asteroids[a].Collision(rockets[r]))
                        {
                            rockets[r].Collision("Asteroid");
                            score = score + 300;
                            if (random.Next(0, 9) == 0)
                            {
                                TurnOn(hearts[numberH], new Point(asteroids[a].Rect.X, asteroids[a].Rect.Y));
                                if (numberH < 10)
                                    numberH++;
                                else
                                    numberH = 0;                              
                            }
                            amountA--;
                            Disconnect(asteroids[a]);
                            Disconnect(rockets[r]);
                        }
                    }
                    catch
                    {
                        
                    }
                }
            }
            for (int h = 0; h < hearts.Count; h++)
            {
                hearts[h].Update();
                if (ship.Collision(hearts[h]))
                {
                    ship.Collision("Heart");
                    ship.AddEnergy(10);
                    Disconnect(hearts[h]);
                }
            }
            CheckVisible();
            if (amountA <= 0)
                RespawnAsteroids();
        }

        /// <summary>
        /// Проверка на отображение
        /// </summary>
        private static void CheckVisible()
        {
            for (int a = 0; a < asteroids.Count; a++)
            {
                if (asteroids[a].IsVisible == false && asteroids[a].Disconnect == false)
                {
                    amountA--;
                    Disconnect(asteroids[a]);
                }
            }
            for (int r = 0; r < rockets.Count; r++)
            {
                if (rockets[r].IsVisible == false)
                {
                    Disconnect(rockets[r]);
                }
            }
            for (int h = 0; h < hearts.Count; h++)
            {
                if (hearts[h].IsVisible == false)
                {
                    Disconnect(hearts[h]);
                }
            }
        }

        /// <summary>
        /// Включение старых асстероидов и добавление к ним новые значение, создание дополнительных асстероидов(для сложности)
        /// </summary>
        private static void RespawnAsteroids()
        {
            for (int a = 0; a < asteroids.Count; a++)
            {
                int size = random.Next(25, 40);
                asteroids[a].Pos = new Point(random.Next(0, Width), -random.Next(100, 1000));
                asteroids[a].Dir = new Point(random.Next(-2, 2), random.Next(4, 6));
                asteroids[a].Size = new Size(size, size);
                asteroids[a].Disconnect = false;
                asteroids[a].IsVisible = true;
            }
            for (int a = 0; a < 5; a++)
            {
                int size = random.Next(25, 40);
                asteroids.Add(new Asteroid(new Point(random.Next(0, Width), -random.Next(100, 500)), new Point(random.Next(-2, 2),
                    random.Next(4, 6)), new Size(size, size), "Asteroid"));
            }
            allAsteroid = asteroids.Count + 5;
            amountA = asteroids.Count;
            level = level + 1;
            display = true;
            timerLevel.Start();
        }

        /// <summary>
        /// Отключение обьектов
        /// </summary>
        /// <param name="asteroid"></param>
        private static void Disconnect(BaseObject obj)
        {
            obj.Disconnect = true;
            obj.Pos = new Point(-50, 50);
        }

        /// <summary>
        /// Включение обьектов
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="point"></param>
        private static void TurnOn(BaseObject obj, Point point)
        {
            obj.Disconnect = false;
            obj.IsVisible = true;
            obj.Pos = new Point(point.X, point.Y);
        }
    }
}
