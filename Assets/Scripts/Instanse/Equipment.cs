public class Equipment
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int MP { get; set; }
    public int ATK { get; set; }
    public int DFS { get; set; }
    public string DSC { get; set; }
    public Equipment(string name, int health, int margic, int attack, int defense)
    {
        Name = name;
        HP = health;
        MP = margic;
        ATK = attack;
        DFS = defense;
    }
    public void Descrition(string descrition)
    {
        DSC = descrition;
    }
}
