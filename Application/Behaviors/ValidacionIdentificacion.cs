using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Behaviors
{
    public static class ValidacionIdentificacion
    {
        public static bool VerificaIdentificacion(string identificacion)
        {
            bool estado = false;
            char[] valced = new char[13];
            int provincia;
            if (identificacion.Length >= 10)
            {
                valced = identificacion.Trim().ToCharArray();
                provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
                if (provincia > 0 && provincia < 31) //Permitir cedulas emitidas en Consulados
                {
                    if (int.Parse(valced[2].ToString()) < 6)
                        estado = VerificaCedula(valced);
                    else if (int.Parse(valced[2].ToString()) == 6)
                    {
                        if (valced.Length == 13)
                        {
                            estado = VerificaRUCPersonaNatural(valced);
                        }
                        else
                            //Permitir cedulas emitidas en Consulados
                            estado = VerificaCedula(valced);
                    }
                    //Se agregó la validación del tercer dígito 8.
                    else if (int.Parse(valced[2].ToString()) == 8)
                    {
                        if (valced.Length == 13)
                            estado = VerificaSectorPublico(valced);
                        else
                            estado = false;
                    }
                    else if (int.Parse(valced[2].ToString()) == 9)
                    {
                        //Se agrega la validación de excluir de la validación de RUC, las identificaciones cuyo tercer dígito sea 6 o 9.
                        estado = true;
                    }
                }
            }
            return estado;
        }

        public static bool VerificaRUCPersonaNatural(char[] validarCedula)
        {
            try
            {
                const string establecimiento = "001";
                var establecimientoRUC = string.Concat(string.Concat(validarCedula[10], validarCedula[11], validarCedula[12]));
                return establecimientoRUC.Equals(establecimiento) && VerificaCedula(validarCedula);
            }
            catch
            {
                return false;
            }
        }

        private static bool VerificaCedula(char[] validarCedula)
        {
            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }

        private static bool VerificaSectorPublico(char[] validarCedula)
        {
            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[9].ToString()) + int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };

                for (int i = 0; i < 8; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }

                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[8].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
