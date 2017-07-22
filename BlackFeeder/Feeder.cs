using System;
using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace BlackFeeder
{
    internal static class Feeder
    {
        private static Obj_AI_Hero _player;

        private static int _globalRnd;

        private static readonly Vector3 TopVector3 = new Vector3(2122, 53, 12558);
        private static readonly Vector3 BotVector3 = new Vector3(12608, 52, 2380);
        private static readonly Vector3 PurpleSpawn = new Vector3(14286f, 172f, 14382f);
        private static readonly Vector3 BlueSpawn = new Vector3(416f, 182f, 468f);

        private static bool _topVectorReached;
        private static bool _botVectorReached;

        public static void Load()
        {
            _player = ObjectManager.GetLocalPlayer();

            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (InitializeMenu.RootMenu["Feeding.Enabled"].Enabled)
            {
                EnableFeed();
                FeedingChecks();
                Console.WriteLine(_globalRnd);
            }
        }

        private static void EnableFeed()
        {
            var feedingMode = (Utility.FMode) InitializeMenu.RootMenu["Feeding.Mode"].Value;

            var fmode = feedingMode;

            if (InitializeMenu.RootMenu["Feeding.Options"]["Spells.Enabled"].Enabled)
                HandleSpells();

            if (InitializeMenu.RootMenu["Feeding.Options"]["Summoners.Enabled"].Enabled)
                HandleSummonerSpells();

            if (feedingMode == Utility.FMode.RND && _globalRnd == -1)
            {
                var rnd = new Random();
                _globalRnd = rnd.Next(0, 4);
            }

            if ((feedingMode != Utility.FMode.RND) | _player.IsDead)
                _globalRnd = -1;

            if (_globalRnd != -1)
                fmode = (Utility.FMode) _globalRnd;

            switch (fmode)
            {
                case Utility.FMode.MID:
                {
                    _player.IssueOrder(OrderType.MoveTo, _player.Team == GameObjectTeam.Order ? PurpleSpawn : BlueSpawn);
                }
                    break;
                case Utility.FMode.BOT:
                {
                    if (_player.Team == GameObjectTeam.Order)
                    {
                        if (!_botVectorReached)
                            _player.IssueOrder(OrderType.MoveTo, BotVector3);
                        else if (_botVectorReached)
                            _player.IssueOrder(OrderType.MoveTo, PurpleSpawn);
                    }
                    else
                    {
                        if (!_botVectorReached)
                            _player.IssueOrder(OrderType.MoveTo, BotVector3);
                        else if (_botVectorReached)
                            _player.IssueOrder(OrderType.MoveTo, BlueSpawn);
                    }
                }
                    break;
                case Utility.FMode.TOP:
                {
                    if (_player.Team == GameObjectTeam.Order)
                    {
                        if (!_topVectorReached)
                            _player.IssueOrder(OrderType.MoveTo, TopVector3);
                        else if (_topVectorReached)
                            _player.IssueOrder(OrderType.MoveTo, PurpleSpawn);
                    }
                    else
                    {
                        if (!_topVectorReached)
                            _player.IssueOrder(OrderType.MoveTo, TopVector3);
                        else if (_topVectorReached)
                            _player.IssueOrder(OrderType.MoveTo, BlueSpawn);
                    }
                }
                    break;
                case Utility.FMode.FLW:
                {
                    var enemy =
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(x => x.IsValidTarget() && !x.IsDead)
                            .OrderBy(y => y.Distance(_player.Position))
                            .FirstOrDefault();
                    if (enemy != null)
                    {
                        var nearestChamp = enemy.Position;

                        _player.IssueOrder(OrderType.MoveTo, nearestChamp);
                    }
                }
                    break;
            }
        }

        private static void HandleSpells()
        {
            var selfCast = new Dictionary<string, IEnumerable<SpellSlot>>
            {
                {"Blitzcrank", new[] {SpellSlot.W}},
                {"DrMundo", new[] {SpellSlot.R}},
                {"Draven", new[] {SpellSlot.W}},
                {"Evelynn", new[] {SpellSlot.W}},
                {"Garen", new[] {SpellSlot.Q}},
                {"Hecarim", new[] {SpellSlot.E}},
                {"Janna", new[] {SpellSlot.W}},
                {"Karma", new[] {SpellSlot.E}},
                {"Kayle", new[] {SpellSlot.W}},
                {"Kennen", new[] {SpellSlot.E}},
                {"Lulu", new[] {SpellSlot.W}},
                {"MasterYi", new[] {SpellSlot.R}},
                {"MissFortune", new[] {SpellSlot.W}},
                {"Nunu", new[] {SpellSlot.W}},
                {"Orianna", new[] {SpellSlot.W}},
                {"Poppy", new[] {SpellSlot.W}},
                {"Rakan", new[] {SpellSlot.R}},
                {"Rammus", new[] {SpellSlot.Q}},
                {"Rengar", new[] {SpellSlot.R}},
                {"Shyvana", new[] {SpellSlot.W}},
                {"Singed", new[] {SpellSlot.R}},
                {"Sivir", new[] {SpellSlot.R}},
                {"Skarner", new[] {SpellSlot.W}},
                {"Sona", new[] {SpellSlot.E}},
                {"Talon", new[] {SpellSlot.R}},
                {"Teemo", new[] {SpellSlot.W}},
                {"Trundle", new[] {SpellSlot.W}},
                {"Twitch", new[] {SpellSlot.Q}},
                {"Udyr", new[] {SpellSlot.E}},
                {"Vayne", new[] {SpellSlot.R}},
                {"Volibear", new[] {SpellSlot.Q}},
                {"Zilean", new[] {SpellSlot.E}}
            };

            var positionCast = new Dictionary<string, IEnumerable<SpellSlot>>
            {
                {"Aatrox", new[] {SpellSlot.Q}},
                {"Ahri", new[] {SpellSlot.R}},
                {"AurelionSol", new[] {SpellSlot.E}},
                {"Corki", new[] {SpellSlot.W}},
                {"Ekko", new[] {SpellSlot.E}},
                {"Ezreal", new[] {SpellSlot.E}},
                {"Fiora", new[] {SpellSlot.Q}},
                {"Gragas", new[] {SpellSlot.E}},
                {"Graves", new[] {SpellSlot.E}},
                {"Kassadin", new[] {SpellSlot.R}},
                {"Kayn", new[] {SpellSlot.Q}},
                {"KhaZix", new[] {SpellSlot.E}},
                {"Kindred", new[] {SpellSlot.Q}},
                {"Kled", new[] {SpellSlot.R}},
                {"Lucian", new[] {SpellSlot.E}},
                {"Nocturne", new[] {SpellSlot.Q}},
                {"Pantheon", new[] {SpellSlot.R}},
                {"Rakan", new[] {SpellSlot.W}},
                {"Renekton", new[] {SpellSlot.E}},
                {"Riven", new[] {SpellSlot.Q, SpellSlot.E}},
                {"Ryze", new[] {SpellSlot.R}},
                {"Sejuani", new[] {SpellSlot.Q}},
                {"Shaco", new[] {SpellSlot.Q}},
                {"Shen", new[] {SpellSlot.E}},
                {"Shyvana", new[] {SpellSlot.R}},
                {"TahmKench", new[] {SpellSlot.R}},
                {"Taliyah", new[] {SpellSlot.R}},
                {"Tristana", new[] {SpellSlot.W}},
                {"Tryndamere", new[] {SpellSlot.E}},
                {"Vayne", new[] {SpellSlot.Q}}
            };

            if ((_player.Distance(PurpleSpawn) < 600) | (_player.Distance(BlueSpawn) < 600))
                return;

            var selfCastName = selfCast.FirstOrDefault(x => x.Key == _player.ChampionName);
            var positionCastName = positionCast.FirstOrDefault(x => x.Key == _player.ChampionName);

            if (selfCastName.Equals(null) && positionCastName.Equals(null))
                return;

            var selfCastSpells = selfCastName.Value;

            foreach (var spell in selfCastSpells.OrEmptyIfNull())
                if (_player.SpellBook.GetSpell(spell).State.HasFlag(SpellState.NotLearned))
                    _player.SpellBook.LevelSpell(spell);
                else if (_player.SpellBook.CanUseSpell(spell))
                    _player.SpellBook.CastSpell(spell, _player);

            var feedingMode = (Utility.FMode) InitializeMenu.RootMenu["Feeding.Mode"].Value;
            var fmode = feedingMode;

            if (_globalRnd != -1)
                fmode = (Utility.FMode) _globalRnd;

            var positionCastSpells = positionCastName.Value;

            foreach (var spell in positionCastSpells.OrEmptyIfNull())
                if (_player.SpellBook.GetSpell(spell).State.HasFlag(SpellState.NotLearned))
                    _player.SpellBook.LevelSpell(spell);
                else if (_player.SpellBook.CanUseSpell(spell))
                    switch (fmode)
                    {
                        case Utility.FMode.MID:
                        {
                            _player.SpellBook.CastSpell(spell,
                                _player.Team == GameObjectTeam.Order ? PurpleSpawn : BlueSpawn);
                        }
                            break;
                        case Utility.FMode.BOT:
                        {
                            if (_player.Team == GameObjectTeam.Order)
                            {
                                if (!_botVectorReached)
                                    _player.SpellBook.CastSpell(spell, BotVector3);
                                else if (_botVectorReached)
                                    _player.SpellBook.CastSpell(spell, PurpleSpawn);
                            }
                            else
                            {
                                if (!_botVectorReached)
                                    _player.SpellBook.CastSpell(spell, BotVector3);
                                else if (_botVectorReached)
                                    _player.SpellBook.CastSpell(spell, BlueSpawn);
                            }
                        }
                            break;
                        case Utility.FMode.TOP:
                        {
                            if (_player.Team == GameObjectTeam.Order)
                            {
                                if (!_topVectorReached)
                                    _player.SpellBook.CastSpell(spell, TopVector3);
                                else if (_topVectorReached)
                                    _player.SpellBook.CastSpell(spell, PurpleSpawn);
                            }
                            else
                            {
                                if (!_topVectorReached)
                                    _player.SpellBook.CastSpell(spell, TopVector3);
                                else if (_topVectorReached)
                                    _player.SpellBook.CastSpell(spell, BlueSpawn);
                            }
                        }
                            break;
                        case Utility.FMode.FLW:
                        {
                            var enemy =
                                ObjectManager.Get<Obj_AI_Hero>()
                                    .Where(x => x.IsValidTarget() && !x.IsDead)
                                    .OrderBy(y => y.Distance(_player.Position))
                                    .FirstOrDefault();
                            if (enemy != null)
                            {
                                var nearestChamp = enemy.Position;

                                _player.SpellBook.CastSpell(spell, nearestChamp);
                            }
                        }
                            break;
                    }
        }

        private static void HandleSummonerSpells()
        {
            var flashSlot = ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Summoner1).Name ==
                            "SummonerFlash"
                ? SpellSlot.Summoner1
                : ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Summoner2).Name == "SummonerFlash"
                    ? SpellSlot.Summoner2
                    : SpellSlot.Unknown;

            var ghostSlot = ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Summoner1).Name ==
                            "SummonerGhost"
                ? SpellSlot.Summoner1
                : ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Summoner2).Name == "SummonerGhost"
                    ? SpellSlot.Summoner2
                    : SpellSlot.Unknown;

            var healSlot = ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Summoner1).Name ==
                           "SummonerHeal"
                ? SpellSlot.Summoner1
                : ObjectManager.GetLocalPlayer().SpellBook.GetSpell(SpellSlot.Summoner2).Name == "SummonerHeal"
                    ? SpellSlot.Summoner2
                    : SpellSlot.Unknown;

            var feedingMode = (Utility.FMode) InitializeMenu.RootMenu["Feeding.Mode"].Value;
            var fmode = feedingMode;

            if (_globalRnd != -1)
                fmode = (Utility.FMode) _globalRnd;

            switch (fmode)
            {
                case Utility.FMode.MID:
                {
                    if (_player.Team == GameObjectTeam.Order)
                    {
                        if (flashSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(flashSlot, PurpleSpawn);

                        if (ghostSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(ghostSlot, _player);

                        if (healSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(healSlot, _player);
                    }
                    else
                    {
                        if (flashSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(flashSlot, BlueSpawn);

                        if (ghostSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(ghostSlot, _player);

                        if (healSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(healSlot, _player);
                    }
                }
                    break;
                case Utility.FMode.BOT:
                {
                    if (_player.Team == GameObjectTeam.Order)
                    {
                        if (!_botVectorReached)
                        {
                            if (flashSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(flashSlot, BotVector3);

                            if (ghostSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(ghostSlot, _player);

                            if (healSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(healSlot, _player);
                        }
                        else if (_botVectorReached)
                        {
                            if (flashSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(flashSlot, PurpleSpawn);

                            if (ghostSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(ghostSlot, _player);

                            if (healSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(healSlot, _player);
                        }
                    }
                    else
                    {
                        if (!_botVectorReached)
                        {
                            if (flashSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(flashSlot, BotVector3);

                            if (ghostSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(ghostSlot, _player);

                            if (healSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(healSlot, _player);
                        }
                        else if (_botVectorReached)
                        {
                            if (flashSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(flashSlot, BlueSpawn);

                            if (ghostSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(ghostSlot, _player);

                            if (healSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(healSlot, _player);
                        }
                    }
                }
                    break;
                case Utility.FMode.TOP:
                {
                    if (_player.Team == GameObjectTeam.Order)
                    {
                        if (!_topVectorReached)
                        {
                            if (flashSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(flashSlot, TopVector3);

                            if (ghostSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(ghostSlot, _player);

                            if (healSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(healSlot, _player);
                        }
                        else if (_topVectorReached)
                        {
                            if (flashSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(flashSlot, PurpleSpawn);

                            if (ghostSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(ghostSlot, _player);

                            if (healSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(healSlot, _player);
                        }
                    }
                    else
                    {
                        if (!_topVectorReached)
                        {
                            if (flashSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(flashSlot, TopVector3);

                            if (ghostSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(ghostSlot, _player);

                            if (healSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(healSlot, _player);
                        }
                        else if (_topVectorReached)
                        {
                            if (flashSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(flashSlot, BlueSpawn);

                            if (ghostSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(ghostSlot, _player);

                            if (healSlot != SpellSlot.Unknown)
                                _player.SpellBook.CastSpell(healSlot, _player);
                        }
                    }
                }
                    break;
                case Utility.FMode.FLW:
                {
                    var enemy =
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(x => x.IsValidTarget() && !x.IsDead)
                            .OrderBy(y => y.Distance(_player.Position))
                            .FirstOrDefault();
                    if (enemy != null)
                    {
                        var nearestChamp = enemy.Position;

                        if (flashSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(flashSlot, nearestChamp);

                        if (ghostSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(ghostSlot, _player);

                        if (healSlot != SpellSlot.Unknown)
                            _player.SpellBook.CastSpell(healSlot, _player);
                    }
                }
                    break;
            }
        }

        private static void FeedingChecks()
        {
            if (_player.IsDead)
            {
                _topVectorReached = false;
                _botVectorReached = false;
            }
            else
            {
                if (_player.Distance(BotVector3) <= 300)
                    _botVectorReached = true;

                if (_player.Distance(TopVector3) <= 300)
                    _topVectorReached = true;
            }
        }

        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}