using System;
using System.Drawing;

namespace Game
{
    class Asteroid : BaseObject
    {
        /// <summary>
        /// Астероид
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        /// <param name="path"></param>
        public Asteroid(Point pos, Point dir, Size size, string path) : base(pos, dir, size, path)
        {

        }

        public override void Draw()
        {
            if (Disconnect)
                return;
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            if (Disconnect)
                return;
            Pos.Y = Pos.Y + Dir.Y;
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width - Size.Width;
            if (Pos.X > Game.Width - Size.Width) Pos.X = 0;
            if (Pos.Y > Game.Height) IsVisible = false;
        }
    }
}
