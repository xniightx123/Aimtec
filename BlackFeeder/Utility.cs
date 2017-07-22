using System;
using System.Linq;
using Aimtec;

namespace BlackFeeder
{
    internal class Utility
    {
        public enum FMode
        {
            MID = 0,
            BOT = 1,
            TOP = 2,
            FLW = 3,
            RND = 4
        }

        public enum FType
        {
            AllyF,
            EnemyF
        }

        public static SpellSlot GetSpellSlot(Obj_AI_Hero unit, string name)
        {
            foreach (
                var spell in
                unit.SpellBook.Spells.Where(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase))
            )
                return spell.Slot;

            return SpellSlot.Unknown;
        }
    }
}