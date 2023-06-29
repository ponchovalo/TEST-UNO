using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_UNO
{
    public class Impresora
    {
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string Ip { get; set; }  
        public string Mac { get; set; }
        public string Edificio { get; set; }
        public string Ubicacion { get; set; }


        public Impresora(string Nombre, string Modelo, string Serie, string Ip, string Mac, string Edificio, string Ubicacion) 
        {
            this.Nombre = Nombre;
            this.Modelo = Modelo;
            this.Serie = Serie;
            this.Ip = Ip;
            this.Mac = Mac;
            this.Edificio = Edificio;
            this.Ubicacion = Ubicacion;
        }
    }
}
