using System;
using System.Windows.Forms;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 500;
            form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}
