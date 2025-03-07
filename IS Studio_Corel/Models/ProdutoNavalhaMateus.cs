using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Studio_Corel
{
    public class ProdutoNavalhaMateus
    {
        public string Name{get;set;}
        public string Description { get;set;}
        public string Unit { get;set;}
        public double Quantity { get;set;}
        public double Value { get;set;}


        public ProdutoNavalhaMateus(string name,string descrition, string unit, double quantity ,double value)
        {
            Name= name;
            Description= descrition;
            Unit= unit;
            Quantity = quantity;
            Value= value;
        }
    }
}
