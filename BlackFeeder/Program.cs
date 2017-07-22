using Aimtec.SDK.Events;

namespace BlackFeeder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GameEvents.GameStart += OnStart;
        }

        private static void OnStart()
        {
            InitializeMenu.LoadMenu();
            Feeder.Load();
        }
    }
}