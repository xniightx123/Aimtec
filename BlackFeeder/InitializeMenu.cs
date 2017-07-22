using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace BlackFeeder
{
    internal class InitializeMenu
    {
        public static Menu RootMenu = new Menu("BlackFeeder", "BlackFeeder", true);

        public static void LoadMenu()
        {
            RootMenu.Add(new MenuBool("Feeding.Enabled", "Enable Feeding", false));
            RootMenu.Add(new MenuList("Feeding.Mode", "Feeding Mode:",
                new[] {"Middle Lane", "Bottom Lane", "Top Lane", "Follow Enemy", "Random Lane"}, 1));

            var feedingMenu = new Menu("Feeding.Options", "Feeding Options");
            {
                feedingMenu.Add(new MenuBool("Spells.Enabled", "Enable Ability usage", true));
                feedingMenu.Add(new MenuBool("Summoners.Enabled", "Enable Summoner usage", true));
            }

            RootMenu.Add(feedingMenu);
            RootMenu.Attach();
        }
    }
}