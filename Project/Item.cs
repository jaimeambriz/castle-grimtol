using System.Collections.Generic;

namespace CastleGrimtol.Project
{
    public class Item : IItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Available {get; set;}
        public Room UseableRoom {get; set;}

        public Item(string name, string description, Room room, bool available){
            Name = name;
            Description = description;
            Available = available;
            UseableRoom = room;
        }
    }
}