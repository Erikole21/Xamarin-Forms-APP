using System;
using System.Collections.Generic;
using System.Text;

namespace ClientePeatonXamarin.Modelos
{
    public enum EnumLoginEstados : short
    {

        UsuarioOPasswordInvalido = 0,
        UsuarioBloqueado = 1,
        UsuarioPasswordValidos = 2,
        UsuarioNoExiste = 3,
        PasswordVencida = 4,
        EmpleadoNoExisteOEmpleadoInactivo = 5,
        UsuarioSinPermisos = 6,
        UsuarioSinUbicacion = 7,
        FalloCreacionSesion = 8,
        UsuarioNuevo = 9,
        AplicacionNoExiste = 10,
        UsuarioValido = 11,
        NumeroIdentificacionNoEsNumerico = 12
    }

    public enum RGEnumTipoRecogidaDC
    {
        NoDefinida = 0,
        FijaCliente = 1,
        Esporadica = 2,
        FijaCentroServicio = 3
    }
}
