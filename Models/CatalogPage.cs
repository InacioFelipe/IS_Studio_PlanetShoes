using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Studio_PlanetShoes.Models
{
    public class CatalogPage
    {
        public CatalogPageSize Tamanho { get; set; }
        public CatalogPageLabel Label { get; set; }
        public CatalogPageHeader Cabecalho { get; set; }
        public List<CatalogPageCard> Cards { get; set; }
        public CatalogPageFoot Rodape { get; set; }
    }
}
