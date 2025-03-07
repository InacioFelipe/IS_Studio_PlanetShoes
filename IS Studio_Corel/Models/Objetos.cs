using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IS_Studio_Corel.Models
{
    public class Objetos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double posX { get; set; }
        public double posY { get; set; }

        public Objetos(int id, string name, double posx, double posy) {
            Id = id;
            Name = name;
            posX = posx;
            posY = posy;
        }

    }
}
