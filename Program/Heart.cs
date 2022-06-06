using System;
using System.Drawing;

namespace Game
{
    class Heart : BaseObject
    {
        /// <summary>
        /// Аптечка
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        public Heart(Point pos, Point dir, Size size, string path) : base(pos, dir, size, path)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.Y > Game.Height) IsVisible = true;
        }
    }
}
