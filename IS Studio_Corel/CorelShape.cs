using Corel.Interop.CorelDRAW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Studio_Corel
{
    public class CorelShape
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public double CurveLength { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }


        public CorelShape(int id, string name, double curveLength, int quantity, string color)
        {
            Id = id;
            Name = name;
            CurveLength = curveLength;
            Quantity = quantity;
            Color = color;
        }

        public CorelShape()
        {
        }

    }
}
