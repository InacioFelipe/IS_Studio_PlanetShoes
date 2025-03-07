using IS_Studio_PlanetShoes.Models;
using IS_Studio_PlanetShoes.Views;
using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using IS_Studio_PlanetShoes.Services;

namespace IS_Studio_PlanetShoes.ViewModels
{
    public class CatalogoViewModel
    {
        //private Corel.Interop.VGCore.Application corelApp;
        private CorelService _corelService = new CorelService();
        public CatalogoViewModel()
        {
            //this.corelApp = new Corel.Interop.VGCore.Application();
        }

        
    }
}
