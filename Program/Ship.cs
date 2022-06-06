using System;
using System.Drawing;

namespace Game
{
    class Ship : BaseObject
    {
        /// <summary>
        /// Энергия корабля
        /// </summary>
        private int energy = 100;
        /// <summary>
        /// Ссылка на энергию корабля
        /// </summary>
        public int Energy => energy;
        /// <summary>
        /// событие уничтожения
        /// </summary>
        public static event Message MessageDestroy;

        /// <summary>
        /// Добавление или убавление энергии корабля
        /// </summary>
        /// <param name="e"></param>
        public void AddEnergy(int e)
        {
            energy += e;
        }
        /// <summary>
        /// Корабль игрока
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        public Ship(Point pos, Point dir, Size size, string path) : base(pos, dir, size, path)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.Y > Game.Height) Pos.Y = 0 - Size.Height;
        }

        /// <summary>
        /// Сдвиг корабля вправо
        /// </summary>
        public void Right()
        {
            if (Pos.X < Game.Width - Size.Width) Pos.X = Pos.X + Dir.X;
        }
        /// <summary>
        /// Сдвиг корабля влево
        /// </summary>
        public void Left()
        {
            if (Pos.X > 0) Pos.X = Pos.X - Dir.X;
        }
        public void Destroy()
        {
            MessageDestroy.Invoke();
        }
    }
}
