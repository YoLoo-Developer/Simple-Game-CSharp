using System;
using System.Drawing;

namespace Game
{
    class Rocket : BaseObject
    {
        /// <summary>
        /// Ракета
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        /// <param name="path"></param>
        public Rocket(Point pos, Point dir, Size size, string path) : base(pos, dir, size, path)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.Y < -Size.Height) IsVisible = false;
        }
    }
}
