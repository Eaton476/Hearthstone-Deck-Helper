namespace HearthstoneDeckTracker.Model
{

    public class CardCollection
    {
        public Basic[] Basic { get; set; }
        public Classic[] Classic { get; set; }
        public object[] Promo { get; set; }
        public HallOfFame[] HallofFame { get; set; }
        public Naxxrama[] Naxxramas { get; set; }
        public GoblinsVsGnome[] GoblinsvsGnomes { get; set; }
        public BlackrockMountain[] BlackrockMountain { get; set; }
        public TheGrandTournament[] TheGrandTournament { get; set; }
        public TheLeagueOfExplorer[] TheLeagueofExplorers { get; set; }
        public WhispersOfTheOldGod[] WhispersoftheOldGods { get; set; }
        public OneNightInKarazhan[] OneNightinKarazhan { get; set; }
        public MeanStreetsOfGadgetzan[] MeanStreetsofGadgetzan { get; set; }
        public JourneyToUngoro[] JourneytoUnGoro { get; set; }
        public KnightsOfTheFrozenThrone[] KnightsoftheFrozenThrone { get; set; }
        public TavernBrawl[] TavernBrawl { get; set; }
        public HeroSkin[] HeroSkins { get; set; }
        public Mission[] Missions { get; set; }
        public Credit[] Credits { get; set; }
        public object[] System { get; set; }
        public object[] Debug { get; set; }
    }

    public class Basic
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public Mechanic[] mechanics { get; set; }
        public string faction { get; set; }
        public string rarity { get; set; }
        public int health { get; set; }
        public bool collectible { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public int attack { get; set; }
        public string race { get; set; }
        public int cost { get; set; }
        public string flavor { get; set; }
        public string artist { get; set; }
        public string howToGet { get; set; }
        public string howToGetGold { get; set; }
        public int durability { get; set; }
    }

    public class Mechanic
    {
        public string name { get; set; }
    }

    public class Classic
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public Mechanic1[] mechanics { get; set; }
        public string faction { get; set; }
        public string artist { get; set; }
        public string rarity { get; set; }
        public int cost { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public string race { get; set; }
        public int durability { get; set; }
        public bool elite { get; set; }
    }

    public class Mechanic1
    {
        public string name { get; set; }
    }

    public class HallOfFame
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public string faction { get; set; }
        public Mechanic2[] mechanics { get; set; }
        public int cost { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public string race { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string rarity { get; set; }
        public string flavor { get; set; }
        public string artist { get; set; }
        public bool collectible { get; set; }
        public string howToGet { get; set; }
        public string howToGetGold { get; set; }
        public bool elite { get; set; }
    }

    public class Mechanic2
    {
        public string name { get; set; }
    }

    public class Naxxrama
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public int health { get; set; }
        public string playerClass { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string locale { get; set; }
        public string text { get; set; }
        public Mechanic3[] mechanics { get; set; }
        public int cost { get; set; }
        public string rarity { get; set; }
        public int attack { get; set; }
        public bool elite { get; set; }
        public string race { get; set; }
        public string flavor { get; set; }
        public string artist { get; set; }
        public bool collectible { get; set; }
        public string howToGet { get; set; }
        public string howToGetGold { get; set; }
        public int durability { get; set; }
    }

    public class Mechanic3
    {
        public string name { get; set; }
    }

    public class GoblinsVsGnome
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public Mechanic4[] mechanics { get; set; }
        public int cost { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string artist { get; set; }
        public string rarity { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public string race { get; set; }
        public int durability { get; set; }
        public bool elite { get; set; }
    }

    public class Mechanic4
    {
        public string name { get; set; }
    }

    public class BlackrockMountain
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public int health { get; set; }
        public string playerClass { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string locale { get; set; }
        public string text { get; set; }
        public Mechanic5[] mechanics { get; set; }
        public int cost { get; set; }
        public string rarity { get; set; }
        public int attack { get; set; }
        public bool elite { get; set; }
        public string race { get; set; }
        public string artist { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public string howToGet { get; set; }
        public string howToGetGold { get; set; }
        public int durability { get; set; }
        public string faction { get; set; }
    }

    public class Mechanic5
    {
        public string name { get; set; }
    }

    public class TheGrandTournament
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public Mechanic6[] mechanics { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public int cost { get; set; }
        public string rarity { get; set; }
        public string flavor { get; set; }
        public string artist { get; set; }
        public bool collectible { get; set; }
        public string race { get; set; }
        public int durability { get; set; }
        public bool elite { get; set; }
    }

    public class Mechanic6
    {
        public string name { get; set; }
    }

    public class TheLeagueOfExplorer
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public int health { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public Mechanic7[] mechanics { get; set; }
        public int attack { get; set; }
        public string race { get; set; }
        public int cost { get; set; }
        public string artist { get; set; }
        public string rarity { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public string howToGet { get; set; }
        public string howToGetGold { get; set; }
        public int durability { get; set; }
        public bool elite { get; set; }
    }

    public class Mechanic7
    {
        public string name { get; set; }
    }

    public class WhispersOfTheOldGod
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public Mechanic8[] mechanics { get; set; }
        public string rarity { get; set; }
        public int cost { get; set; }
        public string artist { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public string race { get; set; }
        public int durability { get; set; }
        public bool elite { get; set; }
        public string howToGet { get; set; }
        public string howToGetGold { get; set; }
    }

    public class Mechanic8
    {
        public string name { get; set; }
    }

    public class OneNightInKarazhan
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public Mechanic9[] mechanics { get; set; }
        public string rarity { get; set; }
        public int health { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string artist { get; set; }
        public int attack { get; set; }
        public string race { get; set; }
        public int cost { get; set; }
        public string faction { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public string howToGet { get; set; }
        public string howToGetGold { get; set; }
        public int durability { get; set; }
        public bool elite { get; set; }
    }

    public class Mechanic9
    {
        public string name { get; set; }
    }

    public class MeanStreetsOfGadgetzan
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string artist { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public Mechanic10[] mechanics { get; set; }
        public string rarity { get; set; }
        public int cost { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string faction { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public string race { get; set; }
        public bool elite { get; set; }
        public string multiClassGroup { get; set; }
        public string[] classes { get; set; }
        public int durability { get; set; }
    }

    public class Mechanic10
    {
        public string name { get; set; }
    }

    public class JourneyToUngoro
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string artist { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public Mechanic11[] mechanics { get; set; }
        public int cost { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string rarity { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public string faction { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public string race { get; set; }
        public bool elite { get; set; }
        public int durability { get; set; }
    }

    public class Mechanic11
    {
        public string name { get; set; }
    }

    public class KnightsOfTheFrozenThrone
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public int health { get; set; }
        public Mechanic12[] mechanics { get; set; }
        public string rarity { get; set; }
        public int cost { get; set; }
        public string artist { get; set; }
        public int attack { get; set; }
        public string flavor { get; set; }
        public bool collectible { get; set; }
        public string race { get; set; }
        public string faction { get; set; }
        public bool elite { get; set; }
        public int durability { get; set; }
        public string armor { get; set; }
    }

    public class Mechanic12
    {
        public string name { get; set; }
    }

    public class TavernBrawl
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string faction { get; set; }
        public string rarity { get; set; }
        public int health { get; set; }
        public string playerClass { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string locale { get; set; }
        public string text { get; set; }
        public int attack { get; set; }
        public Mechanic13[] mechanics { get; set; }
        public bool elite { get; set; }
        public int cost { get; set; }
        public int durability { get; set; }
        public string race { get; set; }
        public string flavor { get; set; }
    }

    public class Mechanic13
    {
        public string name { get; set; }
    }

    public class HeroSkin
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string rarity { get; set; }
        public int health { get; set; }
        public bool collectible { get; set; }
        public string playerClass { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string locale { get; set; }
        public string faction { get; set; }
        public int cost { get; set; }
        public int attack { get; set; }
        public int durability { get; set; }
        public string text { get; set; }
    }

    public class Mission
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string playerClass { get; set; }
        public string locale { get; set; }
        public string rarity { get; set; }
        public int health { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public Mechanic14[] mechanics { get; set; }
        public string faction { get; set; }
        public int cost { get; set; }
        public int attack { get; set; }
        public int durability { get; set; }
    }

    public class Mechanic14
    {
        public string name { get; set; }
    }

    public class Credit
    {
        public string cardId { get; set; }
        public string dbfId { get; set; }
        public string name { get; set; }
        public string cardSet { get; set; }
        public string type { get; set; }
        public string rarity { get; set; }
        public string text { get; set; }
        public bool elite { get; set; }
        public string playerClass { get; set; }
        public string img { get; set; }
        public string imgGold { get; set; }
        public string locale { get; set; }
        public int cost { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public string race { get; set; }
    }

}
