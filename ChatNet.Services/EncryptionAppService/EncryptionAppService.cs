using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNet.Services.EncryptionAppService
{
    public class EncryptionAppService : IEncryptionAppService
    {
        const int _intLONGITUD = 5;
        const string _strKEY = "ChatNet@Saion";

        public string DecryptKey(string clave)
        {
            string vl_strSalida = "";
            try
            {
                //Cadena enmascarada
                string vl_strCadenaEnmascarada;

                //Quitamos los signos de separacion
                string vl_strCadenaLimpia = clave.Replace("-", "");

                /*****************************************************************************************/
                /*Convertimos la cadena de hexadecimal a bytes y de alli lo pasamos a cadena enmascarada */
                /*****************************************************************************************/

                //TextoOfuscado en byte
                List<byte> vl_bytTextoOfuscado = new List<byte>();

                //limpiamos la cadena de caracteres no validos colocados al completar la cadena de ofuscacion
                int vl_intResiduo = vl_strCadenaLimpia.Length % 32;

                //retiramos la cadena 
                vl_strCadenaLimpia = vl_strCadenaLimpia.Remove(vl_strCadenaLimpia.Length - vl_intResiduo, vl_intResiduo);

                //Hasta donde se recorrera la cadena, ya que al enmascarar una cadena se completa con nulos
                //para mantener grupos iguales, solo se recorren los elementos con datos
                int hasta = vl_strCadenaLimpia.Length - vl_strCadenaLimpia.Length % 2;

                //Recorremos la cadena y la agrupanos en duetos para poder convertirlos en byte[] para
                //poder manipularlo con el algoritmo de enmascaramiento
                for (int vl_intAux = 0; vl_intAux < hasta; vl_intAux = vl_intAux + 2)
                {
                    //Obtenemos el numero entero del decimal
                    int vl_intCaracter = int.Parse(vl_strCadenaLimpia.Substring(vl_intAux, 2), System.Globalization.NumberStyles.HexNumber);

                    //Agregamos el entero como byte[]
                    vl_bytTextoOfuscado.Add(byte.Parse(vl_intCaracter.ToString()));
                }

                //Texto enmascarado
                vl_strCadenaEnmascarada = Convert.ToBase64String(vl_bytTextoOfuscado.ToArray());

                //Algoritmo de enmascaramiento
                System.Security.Cryptography.RijndaelManaged vl_rigAlgoritmo = new System.Security.Cryptography.RijndaelManaged();

                //Data Enmascarada
                byte[] vl_bytDataEnmascarada = Convert.FromBase64String(vl_strCadenaEnmascarada);

                //Numero de saltos que dara el algoritmo
                byte[] vl_bytSaltos = Encoding.ASCII.GetBytes(_strKEY.Length.ToString());

                //Creador de Clave
                System.Security.Cryptography.PasswordDeriveBytes vl_passSecreto = new System.Security.Cryptography.PasswordDeriveBytes(_strKEY, vl_bytSaltos);

                //Desenmascarador
                System.Security.Cryptography.ICryptoTransform vl_cryDesenmascarador = vl_rigAlgoritmo.CreateDecryptor(vl_passSecreto.GetBytes(32), vl_passSecreto.GetBytes(16));

                //Para el trabajo en memoria
                MemoryStream vl_menMemoryStream = new MemoryStream(vl_bytDataEnmascarada);

                //Para desenmascarar en memoria
                System.Security.Cryptography.CryptoStream vl_cryCryptoStream = new System.Security.Cryptography.CryptoStream(vl_menMemoryStream, vl_cryDesenmascarador, System.Security.Cryptography.CryptoStreamMode.Read);

                //Creamos el contenedor del texto desenmascarado
                byte[] PlainText = new byte[vl_bytDataEnmascarada.Length];

                //Cantidad de elementos a leer
                int vl_intLongitud = vl_cryCryptoStream.Read(PlainText, 0, PlainText.Length);

                //devolvemos el texto desenmascarado
                vl_strSalida = Encoding.Unicode.GetString(PlainText, 0, vl_intLongitud);
            }
            catch (Exception vl_BLMensajeError)
            {
                throw vl_BLMensajeError;
            }

            return vl_strSalida;

        }

        public string EncryptKey(string cadena)
        {
            string vl_strSalidaFormato = "";
            try
            {
                //Permite manejo del algoritmo de enmascaramiento
                System.Security.Cryptography.RijndaelManaged vl_rijAlgoritmo = new System.Security.Cryptography.RijndaelManaged();

                //Texto plano
                byte[] vl_bytTextoPlano = System.Text.Encoding.Unicode.GetBytes(cadena);

                //Saltos del algoritmo
                byte[] vl_bytSaltos = Encoding.ASCII.GetBytes(_strKEY.Length.ToString());

                //Clave derivada de la cantidad de saltos y el reporte
                System.Security.Cryptography.PasswordDeriveBytes vl_serKey = new System.Security.Cryptography.PasswordDeriveBytes(_strKEY, vl_bytSaltos);

                //Permite enmascarar la cadena
                System.Security.Cryptography.ICryptoTransform vl_enmascarador = vl_rijAlgoritmo.CreateEncryptor(vl_serKey.GetBytes(32), vl_serKey.GetBytes(16));

                //Para realizar las operaciones en memoria
                using (MemoryStream vl_menMemoria = new MemoryStream())
                {
                    //Para realizar la enmascaracion
                    using (System.Security.Cryptography.CryptoStream vl_criStream = new System.Security.Cryptography.CryptoStream(vl_menMemoria, vl_enmascarador, System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        //Escribimos la memoria en texto plano
                        vl_criStream.Write(vl_bytTextoPlano, 0, vl_bytTextoPlano.Length);

                        //actualizamos el buffer al stream
                        vl_criStream.FlushFinalBlock();

                        //Para la cadena enmascarada
                        byte[] vl_bytCadenaEnmascarada = vl_menMemoria.ToArray();

                        //String de salida sin formato
                        string vl_strSalida = "";

                        //Por cada letra enmascarada se procede a transformarla en Hexadecimal
                        foreach (byte vl_bytLetra in vl_bytCadenaEnmascarada)
                        {
                            //Para que cada dueto hexadecimal sea siempre de 2 caracteres, la maxima combinacion hexadecimal es FF
                            vl_strSalida += vl_bytLetra.ToString("X").PadLeft(2, '0');
                        }

                        //Para la salida con formato


                        //Se recorre la cadena enmascarada para agruparla 
                        for (int vl_intAux = 0; vl_intAux < vl_strSalida.Length; vl_intAux = vl_intAux + _intLONGITUD)
                        {
                            //Si la longitud de la cadena a agrupar es mayor al de la 
                            //cadena restante
                            if (vl_intAux + _intLONGITUD > vl_strSalida.Length)
                            {
                                //PAra ver si se pone o no guion de sepracion
                                if (vl_strSalidaFormato.Length > 0)
                                {
                                    vl_strSalidaFormato += "-" + vl_strSalida.Substring(vl_intAux).PadRight(_intLONGITUD, '0');
                                }
                                else
                                {
                                    vl_strSalidaFormato += vl_strSalida.Substring(vl_intAux).PadRight(_intLONGITUD, '0');
                                }
                            }
                            else
                            {
                                //Para ver si se pone o no guion de separacion
                                if (vl_strSalidaFormato.Length > 0)
                                {
                                    //concatenamos grupo a cadena de salida
                                    vl_strSalidaFormato += "-" + vl_strSalida.Substring(vl_intAux, _intLONGITUD);
                                }
                                else
                                {
                                    //concatenamos grupo a cadena de salida
                                    vl_strSalidaFormato += vl_strSalida.Substring(vl_intAux, _intLONGITUD);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vl_strSalidaFormato;

        }
    }
}
