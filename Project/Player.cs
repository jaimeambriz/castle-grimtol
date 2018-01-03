using System.Collections.Generic;

namespace CastleGrimtol.Project
{
    public class Player : IPlayer
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<Item> Inventory { get; set; }
        public int Health { get; set; }
        public bool Camouflage {get; set;}
        public Player(string name)
        {
            Name = name;
            Camouflage = false;
            Inventory = new List<Item>();
            Score = 0;
        }


    }

}