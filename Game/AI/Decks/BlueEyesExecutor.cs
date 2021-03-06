﻿using OCGWrapper.Enums;
using System.Collections.Generic;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace MycardBot.Game.AI.Decks
{
    [Deck("Blue-Eyes", "AI_BlueEyes")]
    class BlueEyesExecutor : DefaultExecutor
    {
        public enum CardId
        {
            青眼白龙 = 89631139,
            青眼亚白龙 = 38517737,
            白色灵龙 = 45467446,
            增殖的G = 23434538,
            太古的白石 = 71039903,
            传说的白石 = 79814787,
            青色眼睛的贤士 = 8240199,
            效果遮蒙者 = 97268402,
            银河旋风 = 5133471,
            鹰身女妖的羽毛扫 = 18144506,
            复活之福音 = 6853254,
            强欲而贪欲之壶 = 35261759,
            抵价购物 = 38120068,
            调和的宝札 = 39701395,
            龙之灵庙 = 41620959,
            龙觉醒旋律 = 48800175,
            灵魂补充 = 54447022,
            死者苏生 = 83764718,
            银龙的轰咆 = 87025064,

            鬼岩城 = 63422098,
            苍眼银龙 = 40908371,
            青眼精灵龙 = 59822133,
            月华龙黑蔷薇 = 33698022,
            银河眼暗物质龙 = 58820923,
            银河眼重铠光子龙 = 39030163,
            银河眼光子龙皇 = 31801517,
            银河眼光波龙 = 18963306,
            希望魁龙银河巨神 = 63767246,
            森罗的姬牙宫 = 33909817
        }

        private List<ClientCard> 使用过的青眼亚白龙 = new List<ClientCard>();

        public BlueEyesExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // 有坑先清
            AddExecutor(ExecutorType.Activate, (int)CardId.银河旋风, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, (int)CardId.鹰身女妖的羽毛扫);

            // 灵庙
            AddExecutor(ExecutorType.Activate, (int)CardId.龙之灵庙, 龙之灵庙效果);

            // 拿亚白
            AddExecutor(ExecutorType.Activate, (int)CardId.龙觉醒旋律, 龙觉醒旋律效果);

            // 调和
            AddExecutor(ExecutorType.Activate, (int)CardId.调和的宝札, 调和的宝札效果);

            // 八抽
            AddExecutor(ExecutorType.Activate, (int)CardId.抵价购物, 抵价购物效果);

            // 吸一口
            AddExecutor(ExecutorType.Activate, (int)CardId.强欲而贪欲之壶);

            // 有亚白就跳
            AddExecutor(ExecutorType.SpSummon, (int)CardId.青眼亚白龙);

            // 苏生
            AddExecutor(ExecutorType.Activate, (int)CardId.复活之福音, 死者苏生效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.银龙的轰咆, 死者苏生效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.死者苏生, 死者苏生效果);

            // 通招
            AddExecutor(ExecutorType.Summon, (int)CardId.青色眼睛的贤士);
            AddExecutor(ExecutorType.Summon, (int)CardId.太古的白石);
            AddExecutor(ExecutorType.Summon, (int)CardId.传说的白石);

            // 效果
            AddExecutor(ExecutorType.Activate, (int)CardId.青眼亚白龙, 青眼亚白龙效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.青色眼睛的贤士, 青色眼睛的贤士效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.太古的白石, 太古的白石效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.白色灵龙, 白色灵龙效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.青眼精灵龙, 青眼精灵龙效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.希望魁龙银河巨神, 希望魁龙银河巨神效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.苍眼银龙, 苍眼银龙效果);
            AddExecutor(ExecutorType.Activate, (int)CardId.森罗的姬牙宫);

            // 出大怪
            AddExecutor(ExecutorType.SpSummon, (int)CardId.青眼精灵龙, 青眼精灵龙同调召唤);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.希望魁龙银河巨神, 希望魁龙银河巨神超量召唤);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.森罗的姬牙宫);

            // 改变攻守表示
            AddExecutor(ExecutorType.Repos, 改变攻守表示);

            // Set traps
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);

        }

        public override bool OnSelectHand()
        {
            // 先攻
            return false;
        }

        public override void OnNewTurn()
        {
            // 回合开始时重置亚白龙状况
            使用过的青眼亚白龙.Clear();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, bool cancelable)
        {
            //Logger.WriteLine("OnSelectCard.");
            if (max==2)
            {
                //Logger.WriteLine("龙觉醒检索.");
                IList<ClientCard> result = new List<ClientCard>();
                if (!Duel.Fields[0].HasInHand((int)CardId.青眼白龙))
                {
                    //Logger.WriteLine("手里没有本体，拿一张.");
                    foreach (ClientCard card in cards)
                    { 
                        if (card.Id == (int)CardId.青眼白龙)
                        {
                            result.Add(card);
                            //Logger.WriteLine("拿到了.");
                            break;
                        }
                    }
                }
                foreach (ClientCard card in cards)
                {
                    //Logger.WriteLine("拿亚白龙.");
                    if (card.Id == (int)CardId.青眼亚白龙)
                    {
                        result.Add(card);
                    }
                }
                if (result.Count < min)
                {
                    foreach (ClientCard card in cards)
                    {
                        //Logger.WriteLine("亚白龙不够了.");
                        if (!result.Contains(card))
                            result.Add(card);
                        if (result.Count >= min)
                            break;
                    }
                }
                while (result.Count > max)
                {
                    //Logger.WriteLine("拿多了.");
                    result.RemoveAt(result.Count - 1);
                }
                return result;
            }
            //Logger.WriteLine("Use default.");
            return null;
        }

        public override IList<ClientCard> OnSelectSum(IList<ClientCard> cards, int sum, int min, int max)
        {
            Logger.WriteLine(cards.Count + " sync " + sum);
            IList<ClientCard> selected = new List<ClientCard>();
            int trysum = 0;
            if (使用过的青眼亚白龙.Count > 0 && cards.IndexOf(使用过的青眼亚白龙[0])>0)
            {
                Logger.WriteLine("优先用使用过的亚白龙同调.");
                ClientCard card = 使用过的青眼亚白龙[0];
                使用过的青眼亚白龙.Remove(card);
                cards.Remove(card);
                selected.Add(card);
                trysum = card.Level;
                if (trysum == sum)
                {
                    Logger.WriteLine(trysum + " dselected " + sum);
                    return selected;
                }
            }
            foreach (ClientCard card in cards)
            {
                // try level equal
                if (card.Level == sum)
                {
                    return new[] { card };
                }
                // try level add
                if (trysum + card.Level > sum)
                {
                    continue;
                }
                selected.Add(card);
                trysum += card.Level;
                Logger.WriteLine(card.Id + "");
                Logger.WriteLine(trysum + " selected " + sum);
                if (trysum == sum)
                {
                    return selected;
                }
            }
            IList<ClientCard> selected2 = new List<ClientCard>();
            foreach (ClientCard card in selected)
            {
                // clone
                selected2.Add(card);
            }
            foreach (ClientCard card in selected)
            {
                // try level sub
                selected2.Remove(card);
                trysum -= card.Level;
                //Logger.WriteLine(card.Id + "");
                Logger.WriteLine(trysum + " selected2 " + sum);
                if (trysum == sum)
                {
                    return selected2;
                }
            }
            // try all
            return cards;
        }

        private bool 龙之灵庙效果()
        {
            Logger.WriteLine("龙之灵庙.");
            AI.SelectCard(new[]
                {
                    (int)CardId.白色灵龙,
                    (int)CardId.青眼白龙,
                    (int)CardId.太古的白石,
                    (int)CardId.传说的白石
                });
            if (!Duel.Fields[0].HasInHand((int)CardId.青眼白龙))
            {
                Logger.WriteLine("手里没有本体，堆白石.");
                AI.SelectNextCard((int)CardId.传说的白石);
            }
            else
            {
                Logger.WriteLine("堆太古或灵龙或白石.");
                AI.SelectNextCard(new[]
                {
                    (int)CardId.太古的白石,
                    (int)CardId.白色灵龙,
                    (int)CardId.传说的白石
                });
            }
            return true;
        }

        private bool 龙觉醒旋律效果()
        {
            Logger.WriteLine("龙觉醒选要丢的卡.");
            AI.SelectCard(new[]
                {
                    (int)CardId.太古的白石,
                    (int)CardId.白色灵龙,
                    (int)CardId.传说的白石,
                    (int)CardId.银河旋风,
                    (int)CardId.效果遮蒙者,
                    (int)CardId.抵价购物,
                    (int)CardId.青色眼睛的贤士
                });
            return true;
        }

        private bool 调和的宝札效果()
        {
            Logger.WriteLine("调和选要丢的卡.");
            if (!Duel.Fields[0].HasInHand((int)CardId.青眼白龙))
            {
                Logger.WriteLine("手里没有本体，丢白石.");
                AI.SelectCard((int)CardId.传说的白石);
            }
            else if (Duel.Fields[0].HasInHand((int)CardId.抵价购物))
            {
                Logger.WriteLine("手里有本体，再拿一个喂八抽.");
                AI.SelectCard((int)CardId.传说的白石);
            }
            else
            {
                Logger.WriteLine("手里有本体，优先丢太古.");
                AI.SelectCard((int)CardId.太古的白石);
            }
            return true;
        }

        private bool 抵价购物效果()
        {
            Logger.WriteLine("抵价购物发动.");
            if (Duel.Fields[0].HasInHand((int)CardId.白色灵龙))
            {
                Logger.WriteLine("手里有白灵龙，优先丢掉.");
                AI.SelectCard((int)CardId.白色灵龙);
                return true;
            }
            else if (手里有2个((int)CardId.青眼白龙))
            {
                Logger.WriteLine("手里有2个青眼白龙，丢1个.");
                AI.SelectCard((int)CardId.青眼白龙);
                return true;
            }
            else if (手里有2个((int)CardId.青眼亚白龙))
            {
                Logger.WriteLine("手里有2个青眼亚白龙，丢1个.");
                AI.SelectCard((int)CardId.青眼亚白龙);
                return true;
            }
            else if (!Duel.Fields[0].HasInHand((int)CardId.青眼白龙) || !Duel.Fields[0].HasInHand((int)CardId.青眼亚白龙))
            {
                Logger.WriteLine("手里没有成对的青眼和亚白，丢1个.");
                AI.SelectCard(new[]
                {
                    (int)CardId.青眼白龙,
                    (int)CardId.青眼亚白龙
                });
                return true;
            }
            else
            {
                Logger.WriteLine("手里只有一对，不能乱丢.");
                return false;
            }
        }
        
        private bool 青眼亚白龙效果()
        {
            Logger.WriteLine("亚白龙效果.");
            ClientCard card = Duel.Fields[1].MonsterZone.GetInvincibleMonster();
            if (card != null)
            {
                Logger.WriteLine("炸打不死的怪.");
                AI.SelectCard(card);
                使用过的青眼亚白龙.Add(Card);
                return true;
            }
            card = Duel.Fields[1].MonsterZone.GetDangerousMonster();
            if (card != null)
            {
                Logger.WriteLine("炸厉害的怪.");
                AI.SelectCard(card);
                使用过的青眼亚白龙.Add(Card);
                return true;
            }
            card = AI.Utils.GetOneEnnemyBetterThanValue(Card.GetDefensePower(), false);
            if (card != null)
            {
                Logger.WriteLine("炸比自己强的怪.");
                AI.SelectCard(card);
                使用过的青眼亚白龙.Add(Card);
                return true;
            }
            if (能处理青眼亚白龙())
            {
                
                使用过的青眼亚白龙.Add(Card);
                return true;
            }
            Logger.WriteLine("不炸.");
            return false;
        }

        private bool 死者苏生效果()
        {
            if (Card.Location == CardLocation.Hand && CurrentChain.Count > 0)
            {
                Logger.WriteLine("轰咆避免卡时点.");
                return false;
            }
            List<int> targets = new List<int> {
                    (int)CardId.希望魁龙银河巨神,
                    (int)CardId.银河眼暗物质龙,
                    (int)CardId.青眼亚白龙,
                    (int)CardId.苍眼银龙,
                    (int)CardId.青眼精灵龙,
                    (int)CardId.青眼白龙,
                    (int)CardId.白色灵龙
                };
            if (!Duel.Fields[0].HasInGraveyard(targets))
            {
                return false;
            }
            List<ClientCard> spells = Duel.Fields[1].GetSpells();
            ClientCard selected = null;
            foreach (ClientCard card in spells)
            {
                if (card.IsSpellNegateAttack())
                {
                    selected = card;
                    break;
                }
            }
            if (selected != null && Duel.Fields[0].HasInGraveyard((int)CardId.白色灵龙))
            {
                AI.SelectCard((int)CardId.白色灵龙);
            }
            else
            {
                AI.SelectCard(targets);
            }
            return true;
        }

        private bool 苍眼银龙效果()
        {
            Logger.WriteLine("苍眼银龙效果.");
            if (Duel.Fields[1].GetSpellCount()>0)
            {
                AI.SelectCard((int)CardId.白色灵龙);
            }
            else
            {
                AI.SelectCard((int)CardId.青眼白龙);
            }
            return true;
        }

        private bool 青色眼睛的贤士效果()
        {
            if (Card.Location == CardLocation.Hand)
            {
                Logger.WriteLine("贤士手卡效果.");
                return false;
            }
            AI.SelectCard(new[]
                {
                    (int)CardId.太古的白石,
                    (int)CardId.效果遮蒙者,
                    (int)CardId.传说的白石
                });
            return true;
        }

        private bool 白色灵龙效果()
        {
            //Logger.WriteLine("白色灵龙"+ActivateDescription);
            if (ActivateDescription == -1) // AI.Utils.GetStringId((int)CardId.白色灵龙, 0))
            {
                Logger.WriteLine("白色灵龙拆后场.");
                return true;
            }
            /*else if(Duel.Phase==DuelPhase.BattleStart)
            {
                Logger.WriteLine("白色灵龙战阶变身.");
                return true;
            }*/
            else
            {
                //Logger.WriteLine("白色灵龙特招手卡. 对象数量"+Duel.ChainTargets.Count);
                foreach (ClientCard card in Duel.ChainTargets)
                {
                    // Logger.WriteLine("对象" + card.Id);
                    if (Card.Equals(card))
                    {
                        Logger.WriteLine("白色灵龙被取对象，是否变身.");
                        return 手里有2个((int)CardId.青眼白龙) || (
                            Duel.Fields[0].HasInGraveyard((int)CardId.青眼白龙)
                            && Duel.Fields[0].HasInGraveyard((int)CardId.太古的白石)
                            );
                    }
                }
                return false;
            }
        }

        private bool 青眼精灵龙效果()
        {
            //Logger.WriteLine("青眼精灵龙" + ActivateDescription);
            if (ActivateDescription == -1) // AI.Utils.GetStringId((int)CardId.白色灵龙, 0))
            {
                Logger.WriteLine("青眼精灵龙无效墓地.");
                return LastChainPlayer == 1;
            }
            else if(Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.End))
            {
                Logger.WriteLine("青眼精灵龙主动变身.");
                AI.SelectCard((int)CardId.苍眼银龙);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            else
            {
                //Logger.WriteLine("青眼精灵龙变身. 对象数量" + Duel.ChainTargets.Count);
                foreach (ClientCard card in Duel.ChainTargets)
                {
                    // Logger.WriteLine("对象" + card.Id);
                    if (Card.Equals(card))
                    {
                        Logger.WriteLine("青眼精灵龙被取对象，变身.");
                        AI.SelectCard((int)CardId.苍眼银龙);
                        return true;
                    }
                }
                return false;
            }
        }


        private bool 希望魁龙银河巨神效果()
        {
            Logger.WriteLine("希望魁龙银河巨神" + ActivateDescription);
            if (ActivateDescription == -1) // AI.Utils.GetStringId((int)CardId.白色灵龙, 0))
            {
                Logger.WriteLine("希望魁龙银河巨神无效魔法.");
                return LastChainPlayer == 1;
            }
            return LastChainPlayer == 1;
        }

        private bool 太古的白石效果()
        {
            if (ActivateDescription == AI.Utils.GetStringId((int)CardId.太古的白石, 0))
                {
                Logger.WriteLine("太古白石回收效果.");
                if (Duel.Fields[0].HasInHand((int)CardId.青眼白龙)
                    && !Duel.Fields[0].HasInHand((int)CardId.青眼亚白龙)
                    && Duel.Fields[0].HasInGraveyard((int)CardId.青眼亚白龙))
                {
                    Logger.WriteLine("缺亚白龙，回收.");
                    AI.SelectCard((int)CardId.青眼亚白龙);
                    return true;
                }
                if (Duel.Fields[0].HasInHand((int)CardId.青眼亚白龙)
                    && !Duel.Fields[0].HasInHand((int)CardId.青眼白龙)
                    && Duel.Fields[0].HasInGraveyard((int)CardId.青眼白龙))
                {
                    Logger.WriteLine("有亚白龙缺本体，回收.");
                    AI.SelectCard((int)CardId.青眼白龙);
                    return true;
                }
                if (Duel.Fields[0].HasInHand((int)CardId.抵价购物)
                    && !Duel.Fields[0].HasInHand((int)CardId.青眼白龙)
                    && !Duel.Fields[0].HasInHand((int)CardId.青眼亚白龙))
                {
                    Logger.WriteLine("回收喂八抽.");
                    AI.SelectCard((int)CardId.青眼白龙);
                    return true;
                }
                Logger.WriteLine("并没有应该回收的.");
                return false;
            }
            else
            {
                Logger.WriteLine("太古白石特招效果.");
                List<ClientCard> spells = Duel.Fields[1].GetSpells();
                if (spells.Count == 0)
                {
                    Logger.WriteLine("对面没坑，跳个本体.");
                    //AI.SelectCard((int)CardId.青眼白龙);
                    AI.SelectCard((int)CardId.白色灵龙);
                }
                else
                {
                    Logger.WriteLine("对面有坑，拆.");
                    AI.SelectCard((int)CardId.白色灵龙);
                }
                return true;
            }
        }

        private bool 青眼精灵龙同调召唤()
        {
            if (Duel.Phase == DuelPhase.Main1)
            {
                Logger.WriteLine("主阶段1同调精灵龙.");
                if (使用过的青眼亚白龙.Count>0)
                {
                    Logger.WriteLine("有用过的亚白需要同调.");
                    return true;
                }
                if (Duel.Turn==1)
                {
                    Logger.WriteLine("先攻同调.");
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    return true;
                }
            }
            if (Duel.Phase == DuelPhase.Main2)
            {
                Logger.WriteLine("主阶段2同调精灵龙.");
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool 希望魁龙银河巨神超量召唤()
        {
            if (Duel.Phase == DuelPhase.Main1)
            {
                Logger.WriteLine("主阶段1超量银河巨神.");
                if (使用过的青眼亚白龙.Count > 0)
                {
                    Logger.WriteLine("有用过的亚白可以叠.");
                    return true;
                }
                if (Duel.Turn == 1)
                {
                    Logger.WriteLine("先攻超量银河巨神.");
                    return true;
                }
            }
            if (Duel.Phase == DuelPhase.Main2)
            {
                Logger.WriteLine("主阶段2超量银河巨神.");
                return true;
            }
            return false;
        }

        private bool BreakthroughSkill()
        {
            return (CurrentChain.Count > 0 && DefaultTrap());
        }

        private bool 改变攻守表示()
        {
            bool ennemyBetter = AI.Utils.IsEnnemyBetter(true, true);

            if (Card.IsAttack() && ennemyBetter)
                return true;
            if (Card.IsDefense() && !ennemyBetter && Card.Attack >= Card.Defense)
                return true;
            if (Card.IsDefense() && (Card.Id == (int)CardId.青眼精灵龙
                || Card.Id == (int)CardId.苍眼银龙
                ))
                return true;
            if (Card.IsAttack() && (Card.Id == (int)CardId.青色眼睛的贤士
                || Card.Id == (int)CardId.太古的白石
                || Card.Id == (int)CardId.传说的白石
                ))
                return true;
            return false;
        }

        private bool 盖卡()
        {
            return (Card.IsTrap() || (Card.Id==(int)CardId.银龙的轰咆)) && Duel.Fields[0].GetSpellCountWithoutField() < 4;
        }


        private bool 手里有2个(int id)
        {
            int num = 0;
            foreach (ClientCard card in Duel.Fields[0].Hand)
            {
                if (card != null && card.Id == id)
                    num++;
            }
            return num >= 2;
        }

        private bool 能处理青眼亚白龙()
        {
            return Duel.Fields[0].HasInMonstersZone(new List<int>
                {
                    (int)CardId.青色眼睛的贤士,
                    (int)CardId.太古的白石,
                    (int)CardId.传说的白石,
                    (int)CardId.青眼白龙,
                    (int)CardId.白色灵龙
                }) || Duel.Fields[0].GetCountCardInZone(Duel.Fields[0].MonsterZone, (int)CardId.青眼亚白龙)>=2 ;
        }
    }
}
