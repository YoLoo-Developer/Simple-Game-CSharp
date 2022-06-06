using System;
using System.Drawing;

namespace Game
{
    abstract class BaseObject : ICollision
    {
        /// <summary>
        /// Изображение
        /// </summary>
        protected Image image;
        /// <summary>
        /// Название файла с картинкой
        /// </summary>
        protected string Path = "Null";
        /// <summary>
        /// Улетел ли за пределы экрана(true-виден, false-не виден)
        /// </summary>
        public bool IsVisible = true;
        /// <summary>
        /// Позиция
        /// </summary>
        public Point Pos;
        /// <summary>
        /// Направление
        /// </summary>
        public Point Dir;
        /// <summary>
        /// Размер
        /// </summary>
        public Size Size;
        /// <summary>
        /// Делегат сообщений
        /// </summary>
        public delegate void Message();
        /// <summary>
        /// Включение/отключение отрисовки и обновление данных
        /// </summary>
        public bool Disconnect;

        public static event Action<string> CreateObject;
        public static event Action<string> CollisionObject;

        /// <summary>
        /// Базовый объект
        /// </summary>
        /// <param name="pos">Позиция</param>
        /// <param name="dir">Направление движения</param>
        /// <param name="size">Размер</param>
        public BaseObject(Point pos, Point dir, Size size, string path)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
            Path = path;
            image = Image.FromFile("Pictures/" + path + ".png");
            CreateObject?.Invoke($"{DateTime.Now}: " + Path + " created.");
        }

        /// <summary>
        /// Отрисовка графики
        /// </summary>
        public virtual void Draw()
        {  
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, 20, 20);
        }

        /// <summary>
        /// Обновления данных раз в кадр
        /// </summary>
        public virtual void Update()
        {

        }

        public void Collision(string collObj)
        {
            CollisionObject?.Invoke($"{DateTime.Now}: " + Path + " collided with " + collObj);
        }

        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);
    }
}
