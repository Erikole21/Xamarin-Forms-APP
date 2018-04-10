using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public class ADMotivoGuiaDC
    {
        public ADMotivoGuiaDC()
        {

        }

        public short IdMotivoGuia { get; set; }

        public string Descripcion { get; set; }

        
        public string DescripcionFinalMotivo
        {
            get
            {
                if (IdMotivoGuia == 114 || IdMotivoGuia == 116)
                    return "Reasignación Zona Postal";

                if (IdMotivoGuia == 15 || IdMotivoGuia == 122)
                    return "Residente Ausente";

                if (IdMotivoGuia == 184 || IdMotivoGuia == 156 || IdMotivoGuia == 132)
                    return "Siniestro";

                return Descripcion;
            }
        }
    }
}
