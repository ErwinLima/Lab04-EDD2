using System.Text;

namespace Lab04
{
    public class DES
    {

        //------------------P-BOX-------------
        private static int[] IP = { 58, 50, 42, 34, 26, 18, 10, 2,
                     60, 52, 44, 36, 28, 20, 12, 4,
                     62, 54, 46, 38, 30, 22, 14, 6,
                     64, 56, 48, 40, 32, 24, 16, 8,
                     57, 49, 41, 33, 25, 17, 9, 1,
                     59, 51, 43, 35, 27, 19, 11, 3,
                     61, 53, 45, 37, 29, 21, 13, 5,
                     63, 55, 47, 39, 31, 23, 15, 7};

        private static int[] INVP = { 40, 8, 48, 16, 56, 24, 64, 32,
                       39, 7, 47, 15, 55, 23, 63, 31,
                       38, 6, 46, 14, 54, 22, 62, 30,
                       37, 5, 45, 13, 53, 21, 61, 29,
                       36, 4, 44, 12, 52, 20, 60, 28,
                       35, 3, 43, 11, 51, 19, 59, 27,
                       34, 2, 42, 10, 50, 18, 58, 26,
                       33, 1, 41, 9, 49, 17, 57, 25};

        private static int[] PC1 = { 57, 49, 41, 33, 25, 17, 9,
                      1, 58, 50, 42, 34, 26, 18,
                      10, 2, 59, 51, 43, 35, 27,
                      19, 11, 3, 60, 52, 44, 36,
                      63, 55, 47, 39, 31, 23, 15,
                      7, 62, 54, 46, 38, 30, 22,
                      14, 6, 61, 53, 45, 37, 29,
                      21, 13, 5, 28, 20, 12, 4};

        private static int[] PC2 =
        {
                      14, 17, 11, 24, 1, 5,
                      3, 28, 15, 6, 21, 10,
                      23, 19, 12, 4, 26, 8,
                      16, 7, 27, 20, 13, 2,
                      41, 52, 31, 37, 47, 55,
                      30, 40, 51, 45, 33, 48,
                      44, 49, 39, 56, 34, 53,
                      46, 42, 50, 36, 29, 32
        };

        private static int[] E =
        {
                    32, 1, 2, 3, 4, 5,
                    4, 5, 6, 7, 8, 9,
                    8, 9, 10, 11, 12, 13,
                    12, 13, 14, 15, 16, 17,
                    16, 17, 18, 19, 20, 21,
                    20, 21, 22, 23, 24, 25,
                    24, 25, 26, 27, 28, 29,
                    28, 29, 30, 31, 32, 1
        };

        private static int[] P =
        {
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
        };

        //-----------------S-BOX--------------
        private static int[,] S1 = {
            {14, 4,	13,	1,	2,	15,	11,	8,	3,	10,	6,	12,	5,	9,	0,	7},
            {0, 15, 7,  4,  14, 2,  13, 1,  10, 6,  12, 11, 9,  5,  3,  8},
            {4, 1,  14, 8,  13, 6,  2,  11, 15, 12, 9,  7,  3,  10, 5,  0},
            {15, 12, 8,  2,  4,  9,  1,  7,  5, 11, 3, 14, 10, 0, 6, 13}
        };

        private static int[,] S2 =
        {
            {15,1,  8,  14, 6,  11, 3,  4,  9,  7,  2,  13, 12, 0,  5,  10},
            {3, 13, 4,   7,  15, 2,  8,  14, 12, 0,  1,  10, 6,  9,  11, 5},
            {0, 14, 7,  11, 10, 4,  13, 1,  5,  8,  12, 6,  9,  3,  2,  15},
            {13, 8,  10, 1,  3,  15, 4,  2,  11, 6,  7,  12, 0,  5,  14, 9}
        };

        private static int[,] S3 =
        {
            {10,    0,  9,  14, 6,  3,  15, 5,  1,  13, 12, 7,  11, 4,  2,  8},
            {13,    7,  0,  9,  3,  4,  6,  10, 2,  8,  5,  14, 12, 11, 15, 1},
            {13,    6,  4,  9,  8,  15, 3,  0,  11, 1,  2,  12, 5,  10, 14, 7},
            {1, 10, 13, 0,  6,  9,  8,  7,  4,  15, 14, 3,  11, 5,  2,  12}
        };

        private static int[,] S4 =
        {
            {7, 13, 14, 3,  0,  6,  9,  10, 1,  2,  8,  5,  11, 12, 4,  15},
            {13,    8,  11, 5,  6,  15, 0,  3,  4,  7,  2,  12, 1,  10, 14, 9},
            {10,    6,  9,  0,  12, 11, 7,  13, 15, 1,  3,  14, 5,  2,  8,  4},
            {3, 15, 0,  6,  10, 1,  13, 8,  9,  4,  5,  11, 12, 7,  2,  14}
        };

        private static int[,] S5 =
        {
            {2, 12, 4,  1,  7,  10, 11, 6,  8,  5,  3,  15, 13, 0,  14, 9},
            {14,    11, 2,  12, 4,  7,  13, 1,  5,  0,  15, 10, 3,  9,  8,  6},
            {4, 2,  1,  11, 10, 13, 7,  8,  15, 9,  12, 5,  6,  3,  0,  14},
            {11,    8,  12, 7,  1,  14, 2,  13, 6,  15, 0,  9,  10, 4,  5,  3}
        };

        private static int[,] S6 =
        {
            {12,    1,  10, 15, 9,  2,  6,  8,  0,  13, 3,  4,  14, 7,  5,  11},
            {10,    15, 4,  2,  7,  12, 9,  5,  6,  1,  13, 14, 0,  11, 3,  8},
            {9, 14, 15, 5,  2,  8,  12, 3,  7,  0,  4,  10, 1,  13, 11, 6},
            {4, 3,  2,  12, 9,  5,  15, 10, 11, 14, 1,  7,  6,  0,  8,  13}
        };

        private static int[,] S7 =
        {
            {4, 11, 2,  14, 15, 0,  8,  13, 3,  12, 9,  7,  5,  10, 6,  1},
            {13,0,  11, 7,  4,  9,  1,  10, 14, 3,  5,  12, 2,  15, 8,  6},
            {1, 4,  11, 13, 12, 3,  7,  14, 10, 15, 6,  8,  0,  5,  9,  2},
            {6, 11, 13, 8,  1,  4,  10, 7,  9,  5,  0,  15, 14, 2,  3,  12}
        };

        private static int[,] S8 =
        {
            {13, 2,  8,  4,  6,  15, 11, 1,  10, 9,  3,  14, 5,  0,  12, 7},
            {1, 15, 13, 8,  10, 3,  7,  4,  12, 5,  6,  11, 0,  14, 9,  2},
            {7, 11, 4,  1,  9,  12, 14, 2,  0,  6,  10, 13, 15, 3,  5,  8},
            {2, 1,  14, 7,  4,  10, 8,  13, 15, 12, 9,  0,  3,  5,  6,  11}
        };

        public static Func<string, string, string> encriptar = delegate (string mensaje, string llave)
        {
            while (mensaje.Length % 8 != 0)//Se llena de espacio en blanco si es necesario
            {
                mensaje = mensaje + " ";
            }
            StringBuilder msjEncriptado = new StringBuilder();            
            string bits = StringToBinary(mensaje);//Se pasa a bits el mensaje
            int posicion = 0;
            string[] keys = generateKeys(llave); //Se generan las llaves

            while (posicion < bits.Length)
            {
                string block = bits.Substring(posicion, 64);
                posicion = posicion + 64;
                string initialP = operacionesVectores(IP, block); //Permutación inicial                        
                string procesoDES = ProcesoDES(initialP, keys);
                string left = procesoDES.Substring(0, 32);
                string right = procesoDES.Substring(32, 32);
                string temp = right + left;
                string permINV = operacionesVectores(INVP, temp);
                msjEncriptado.Append(BinaryToString(permINV));
            }
            return msjEncriptado.ToString();
        };

        public static Func<string, string, string> desencriptar = delegate (string msjCifrado, string llave)
        {
            while (msjCifrado.Length % 8 != 0)//Se llena de espacio en blanco si es necesario
            {
                msjCifrado = msjCifrado + " ";
            }
            StringBuilder msjDescifrado = new StringBuilder();
            int posicion = 0;
            string bits = StringToBinary(msjCifrado);//Se pasa a bits el mensaje

            string[] keys = generateKeys(llave); //Se generan las llaves
            keys = invLlaves(keys);

            while (posicion < bits.Length)
            {
                string block = bits.Substring(posicion, 64);
                string initialP = operacionesVectores(IP, block); //Permutación inicial                                

                string procesoDES = ProcesoDES(initialP, keys);
                string left = procesoDES.Substring(0, 32);
                string right = procesoDES.Substring(32, 32);
                string temp = right + left;

                string permINV = operacionesVectores(INVP, temp);
                msjDescifrado.Append(BinaryToString(permINV));
                posicion = posicion + 64;
            }                        

            return msjDescifrado.ToString();
        };

        private static string[] generateKeys(String key)//Generador de llaves
        {
            string[] keys = new string[16];//Vector que guarda las llaves
            string keyBits = StringToBinary(key);//Se obtienen los bits de la llave
            string keySinP = operacionesVectores(PC1, keyBits);//Se quitan los bits de paridad
            //Se separa la llave
            string izq = keySinP.Substring(0, 28);
            string der = keySinP.Substring(28, 28);   
            for (int round = 1; round <= 16; round++)//Se realizan las 16 rondas para generar 16 llaves
            {
                if (round == 1 || round == 2 || round == 9 || round == 16)
                {
                    izq = shiftLeft(izq, 1);
                    der = shiftLeft(der, 1);
                }
                else
                {
                    izq = shiftLeft(izq, 2);
                    der = shiftLeft(der, 2);
                }
                string txt = izq + der;
                string ki = operacionesVectores(PC2, txt);
                keys[round - 1] = ki;
            }
            return keys;
        }

        private static string shiftLeft(string texto, int cantidad)
        {
            int contador = 0;
            StringBuilder resultado = new StringBuilder();
            char[] caracteres = texto.ToCharArray();
            while (contador < cantidad)
            {
                char primero = caracteres[0];
                for (int i = 1; i < caracteres.Length; i++)
                {
                    caracteres[i - 1] = caracteres[i];
                }
                caracteres[caracteres.Length - 1] = primero;
                contador++;
            }

            foreach (var c in caracteres)
            {
                resultado.Append(c);
            }

            return resultado.ToString();
        }

        private static string ProcesoDES(string message, string[] keys)
        {            
            string left = message.Substring(0, 32);
            string right = message.Substring(32, 32);
            for (int i = 0; i < 16; i++)
            {
                string temp = right;
                string resultF = funcCompleja(right, keys[i]);
                string resultFXOR = XOR(left, resultF);
                right = resultFXOR;
                left = temp;
            }
            return left + right;
        }

        private static string funcCompleja(string right, string key)
        {
            string resultado;
            string rightExp = operacionesVectores(E, right);
            string rightExpXOR = XOR(rightExp, key);
            string sbox = SBOX(rightExpXOR);            
            resultado = operacionesVectores(P, sbox);
            return resultado;
        }

        private static string SBOX(string data)
        {
            int posicion = 0; //Posicion en data
            int cont = 1; //Contador para saber que sbox usar 1-8
            int row = 0; //fila en la sbox
            int col = 0; //columna en la sbox
            StringBuilder sb = new StringBuilder();
            while (posicion < data.Length)
            {
                string temporal = data.Substring(posicion, 6);
                row = Convert.ToByte(temporal[0].ToString() + temporal[5], 2);
                col = Convert.ToByte(temporal.Substring(1, 4), 2);
                switch (cont)
                {
                    case 1: sb.Append(auxiliarFunc(Convert.ToString(S1[row, col], 2),4)); break;
                    case 2: sb.Append(auxiliarFunc(Convert.ToString(S2[row, col], 2), 4)); break;
                    case 3: sb.Append(auxiliarFunc(Convert.ToString(S3[row, col], 2), 4)); break;
                    case 4: sb.Append(auxiliarFunc(Convert.ToString(S4[row, col], 2), 4)); break;
                    case 5: sb.Append(auxiliarFunc(Convert.ToString(S5[row, col], 2), 4)); break;
                    case 6: sb.Append(auxiliarFunc(Convert.ToString(S6[row, col], 2), 4)); break;
                    case 7: sb.Append(auxiliarFunc(Convert.ToString(S7[row, col], 2), 4)); break;
                    case 8: sb.Append(auxiliarFunc(Convert.ToString(S8[row, col], 2), 4)); break;
                    default: break;
                }
                posicion += 6;
                cont++;
            }
            return sb.ToString();
        }

        private static string operacionesVectores(int[] vector, string cadenaBits)//Función para operar con vectores
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < vector.Length; i++)
            {
                char c = cadenaBits[vector[i] - 1];
                sb.Append(c);
            }
            return sb.ToString();
        }

        private static string XOR(string txt1, string txt2)//Función XOR
        {
            StringBuilder resultado = new StringBuilder();
            for (int i = 0; i < txt1.Length; i++)
            {
                if (txt1[i] == txt2[i])
                {
                    resultado.Append("0");
                }
                else
                {
                    resultado.Append("1");
                }
            }
            return resultado.ToString();
        }

        //public static string getBits(string m1)
        //{
        //    byte[] byteArray = Encoding.ASCII.GetBytes(m1);
        //    StringBuilder msjBits = new StringBuilder();
        //    foreach (var item in byteArray)
        //    {
        //        string bits = Convert.ToString(item, 2);
        //        bits = auxiliarFunc(bits, 8);
        //        msjBits.Append(bits);
        //    }
        //    return msjBits.ToString();
        //}

        //public static string getString(string bits)
        //{            
        //    byte[] bytes = new byte[bits.Length / 8];
        //    int j = 0;
        //    while (bits.Length > 0)
        //    {
        //        var algo = Convert.ToByte(bits.Substring(0,8),2);
        //        bytes[j++] = algo;
        //        if (bits.Length >= 8)
        //            bits = bits.Substring(8);
        //    }
        //    string resultString = Encoding.ASCII.GetString(bytes);

        //    return resultString;
        //}

        private static string StringToBinary(string data)//Pasa una cadena a binario
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }
        private static string BinaryToString(string data)//Pasa un binario a su representación en texto
        {
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("ISO-8859-1");
            return iso_8859_1.GetString(byteList.ToArray());
        }        

        private static string auxiliarFunc(string bits, int max)//Función para añadir 0 adicionales
        {            
            while (bits.Length < max)
            {
                bits = "0" + bits;
            }
            return bits;
        }

        private static string[] invLlaves(string[] keys)
        {
            string[] result = new string[keys.Length];
            int c = keys.Length - 1;
            for (int i = 0; i < keys.Length; i++)
            {
                result[c] = keys[i];
                c--;
            }
            return result;
        }
    }
}
