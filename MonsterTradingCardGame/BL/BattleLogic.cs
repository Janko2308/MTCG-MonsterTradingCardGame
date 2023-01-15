using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    internal class BattleLogic
    {
        private DAL.BattleRepository battleRepository = new();
        private UserLogic userLogic = new();
        private SessionLogic session = new();
        private DeckLogic deckLogic = new();

        private static Mutex mutex = new Mutex();
        private static List<BattleUtils> battles = new();

        public BattleUtils StartBattle(Http.HttpRequest request, Http.HttpResponse response)
        {
            var authHeader = request.headers["Authorization"].ToString();
            authHeader = authHeader.Split(" ")[1];
            authHeader = authHeader.ToLower();
            string userName = userLogic.GetUserNameFromToken(authHeader);
            if (!session.CheckIfSession(userName, authHeader))
            {
                throw new AuthenticateTokenException("Access token is missing or invalid");
            }
            if (!deckLogic.CheckIfDeckExists(userName))
            {
                throw new InvalidDeckException("Deck is not valid");
            }
            mutex.WaitOne();
          
            if (battles.Count == 0)
            {
                BattleUtils battle = new BattleUtils();
                battle.Player1 = userName;
                battles.Add(battle);
                mutex.ReleaseMutex();
                return battle;
            }
            else
            {
                if (battles[0].Player1 == userName)
                {
                    mutex.ReleaseMutex();
                    throw new UserAlreadyInBattleException("You are already in a battle");
                }
                BattleUtils battle = battles[0];
                battle.Player2 = userName;
                battles.Remove(battle);
                mutex.ReleaseMutex();
                Battle(battle);
                battle.Mres.Set();
                return battle;
                
            }
        }
        public void Battle(BattleUtils battle)
        {
            List<Card> deck1 = battleRepository.GetDeck(battle.Player1);
            List<Card> deck2 = battleRepository.GetDeck(battle.Player2);
            int counter = 1;
            Random rnd = new Random();
            while (counter < 101)
            {
                if (deck1.Count == 0)
                {
                    battle.BattleLog += "\n" + battle.Player2 +" wins";
                    battleRepository.SetScore(battle.Player2, battle.Player1);
                    break;
                }
                if (deck2.Count == 0)
                {
                    battle.BattleLog += "\n" + battle.Player1 + " wins";
                    battleRepository.SetScore(battle.Player1, battle.Player2);
                    break;
                }
                int randomIndex1 = rnd.Next(deck1.Count);
                Card SelectedCard1 = deck1[randomIndex1];
                
                int randomIndex2 = rnd.Next(deck2.Count);
                Card SelectedCard2 = deck2[randomIndex2];

                if (SelectedCard1.Type == "Monster" && SelectedCard2.Type == "Monster")
                {
                    if (SelectedCard1.Name == "WaterGoblin" && SelectedCard2.Name == "Dragon")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                        SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                    }
                    else if (SelectedCard2.Name == "WaterGoblin" && SelectedCard1.Name == "Dragon")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                        SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                    }
                    else if (SelectedCard1.Name == "Wizard" && SelectedCard2.Name == "Ork")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                        SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                    }
                    else if (SelectedCard1.Name == "Ork" && SelectedCard2.Name == "Wizard")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                        SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                    }
                    else if (SelectedCard1.Name == "FireElf" && SelectedCard2.Name == "Dragon")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                        SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                    }
                    else if (SelectedCard1.Name == "Dragon" && SelectedCard2.Name == "FireElf")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                        SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                    }
                    else
                    {
                        if (SelectedCard1.Damage > SelectedCard2.Damage)
                        {
                            MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                            SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                        }
                        else if (SelectedCard1.Damage < SelectedCard2.Damage)
                        {
                            MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                            SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                        }
                    }
                }
                else if(SelectedCard1.Type == "Monster" && SelectedCard2.Type == "Spell")
                {
                    if(SelectedCard1.Name == "Kraken" && SelectedCard2.Type == "Spell")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                        SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                    }
                    else if(SelectedCard1.Name == "Knight" && SelectedCard2.Element == "Water")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                        SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                    }
                    double damage1 = SelectedCard1.Damage;
                    double damage2 = SelectedCard2.Damage;
                    damage1 *= CalculateAdvantage(SelectedCard1, SelectedCard2);
                    damage2 *= CalculateAdvantage(SelectedCard2, SelectedCard1);
                    if (damage1 > damage2)
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                        SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                    }

                    else if (damage1 < damage2)
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                        SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                    }
                    
                }
                else if (SelectedCard1.Type == "Spell" && SelectedCard2.Type == "Monster")
                {
                    if (SelectedCard1.Type == "Spell" && SelectedCard2.Name == "Kraken")
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                        SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                    }
                    else if (SelectedCard1.Element == "Water" && SelectedCard2.Name == "Knight" )
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                        SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                    }
                    double damage1 = SelectedCard1.Damage;
                    double damage2 = SelectedCard2.Damage;
                    damage1 *= CalculateAdvantage(SelectedCard1, SelectedCard2);
                    damage2 *= CalculateAdvantage(SelectedCard2, SelectedCard1);
                    if (damage1 > damage2)
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                        SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                    }

                    else if (damage1 < damage2)
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                        SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                    }
                }
                else if (SelectedCard1.Type == "Spell" && SelectedCard2.Type == "Spell")
                {
                    double spell1 = SelectedCard1.Damage;
                    double spell2 = SelectedCard2.Damage;
                    spell1 *= CalculateAdvantage(SelectedCard1,SelectedCard2);
                    spell2 *= CalculateAdvantage(SelectedCard2, SelectedCard1);
                    if(spell1 > spell2)
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 1);
                        SaveLog(battle, counter, 1, SelectedCard1, SelectedCard2);
                    }
                    else if (spell2 > spell1)
                    {
                        MoveCard(deck1, deck2, SelectedCard1, SelectedCard2, 2);
                        SaveLog(battle, counter, 2, SelectedCard1, SelectedCard2);
                    }
                }
                    counter++;               
            }
            if(counter == 101)
            {
                battle.BattleLog += "\n The battle resulted in a draw";
            }
        }
        public void MoveCard(List<Card> deck1, List<Card> deck2, Card SelectedCard1, Card SelectedCard2, int winner)
        {
            if (winner == 1)
            {
                Card tmpCard = SelectedCard2;
                deck2.Remove(SelectedCard2);
                deck1.Add(tmpCard);
            }
            else if (winner == 2)
            {
                Card tmpCard = SelectedCard1;
                deck1.Remove(SelectedCard1);
                deck2.Add(tmpCard);
            }
        }
        public void SaveLog(BattleUtils battle, int round, int winner, Card SelectedCard1, Card SelectedCard2)
        {
            if (winner == 1)
            {
                battle.BattleLog += "Round: " + round + ":" + battle.Player1 + " wins with " + SelectedCard1.Name + "(" + SelectedCard1.Damage + ")" + " against " + SelectedCard2.Name + "(" + SelectedCard2.Damage + ")\n";
            }
            else if (winner == 2)
            {
                battle.BattleLog += "Round: " + round + ":" + battle.Player2 + " wins with " + SelectedCard2.Name + "(" + SelectedCard2.Damage + ")" + " against " + SelectedCard1.Name + "(" + SelectedCard1.Damage + ")\n";
            }
        }

        public double CalculateAdvantage(Card card1, Card card2)
        {
            switch (card1.Element)
            {
                case "Fire":
                    if (card2.Element == "Water")
                    {
                        return 0.5;
                    }
                    else if (card2.Element == "Fire")
                    {
                        return 1;
                    }
                    else if (card2.Element == "Normal")
                    {
                        return 2;
                    }
                    break;
                case "Water":
                    if (card2.Element == "Water")
                    {
                        return 1;
                    }
                    else if (card2.Element == "Fire")
                    {
                        return 2;
                    }
                    else if (card2.Element == "Normal")
                    {
                        return 0.5;
                    }
                    break;
                case "Normal":
                    if (card2.Element == "Water")
                    {
                        return 2;
                    }
                    else if (card2.Element == "Fire")
                    {
                        return 0.5;
                    }
                    else if (card2.Element == "Normal")
                    {
                        return 1;
                    }
                    break;
            }
            return 1;
        }
    }
}
