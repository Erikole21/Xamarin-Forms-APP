using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public enum ADEnumTipoCliente
    {
        /// <summary>
        /// Peatón - Convenio
        /// </summary>        
        PCO,
        /// <summary>
        /// Peatón - Peatón
        /// </summary>

        PPE,
        /// <summary>
        /// Convenio - Peatón
        /// </summary>

        CPE,
        /// <summary>
        /// Convenio - Convenio
        /// </summary>

        CCO,
        /// <summary>
        /// Interno
        /// </summary>

        INT,
        /// <summary>
        /// Credito
        /// </summary>

        CRE,
        /// <summary>
        /// Peaton
        /// </summary>

        PEA
    }

    public enum RAEnumEstados
    {        
        None = 0,
        
        Creada = 1,
        
        Respuesta = 2,
        
        Revisado = 3,
        
        Escalado = 4,
        
        Asignado = 5,
        
        Cerrado = 6,
        
        Rechazado = 7,
        
        Cancelado = 8,
        
        Vencido = 9,
        
        Reasignado = 10
    }
}
