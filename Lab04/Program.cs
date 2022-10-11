using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace Lab04;

public class Program
{
    public static void Main()
    {
        try
        {
            AVLTree<Persona> arbolPersonas = new AVLTree<Persona>(); 
            arbolPersonas = LLenarArbol();
            Console.WriteLine("Ingrese el dpi de la persona que quiere buscar: ");
            string? dpi = Console.ReadLine();
            Persona persona1 = new Persona();
            persona1.dpi = dpi!;
            Nodo<Persona> temporal = arbolPersonas.Search(persona1, Delegates.DPIComparison);
            if (temporal == null)
            {
                Console.WriteLine("No se encontraron resultados que coincidan con el DPI");
                return;
            }
            Persona p1 = temporal.Value;
            string path = @"C:\Users\AndresLima\Desktop\crypted";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string convoPath = @"C:\Users\AndresLima\Desktop\crypted\" + p1.dpi;
            if (!Directory.Exists(convoPath))
            {
                Directory.CreateDirectory(convoPath);
            }
            int i = 1;

            foreach (var item in p1.convos)
            {
                try
                {
                    string content = File.ReadAllText(item);
                    List<int> compressed = LZW.encode(content);
                    char delimeter = ',';
                    string text = string.Join(delimeter, compressed);
                    string crypted = DES.encriptar(text, "arbustos");
                    string fileName = "crypted-CONVO-" + p1.dpi + "-" + i.ToString() + ".txt";
                    fileName = convoPath + "\\" + fileName;
                    File.WriteAllText(fileName, crypted);                    
                }
                catch (Exception e)
                {

                    throw new Exception("Sucedio un error inesperado");
                }
                i++;
            }

            Console.WriteLine("Se encontraron " + i.ToString() + " coversaciones para la persona con el dpi: " + p1.dpi);
            Console.WriteLine("Ingrese el número de la conversación que quiere descifrar");
            int convoOpt = Convert.ToInt32(Console.ReadLine());
            if (convoOpt <= 0 && convoOpt > i)
            {
                Console.WriteLine("No existe la conversación");
                return;
            }            
            Console.WriteLine("Ingrese la llave para descifrar la conversación");
            string llave = Console.ReadLine()!;
            if (llave.Length != 8)
            {
                Console.WriteLine("Llave de longitud incorrecta");
                return;
            }
            string file = "crypted-CONVO-" + p1.dpi + "-" + convoOpt + ".txt";
            string[] convo = Directory.GetFiles(@"C:\Users\AndresLima\Desktop\crypted\" + p1.dpi, file);
            string contenido = File.ReadAllText(convo[0]);
            string descifrado = DES.desencriptar(contenido, llave);
            string[] info = descifrado.Split(",");
            List<int> lista = new List<int>();
            foreach (var item in info)
            {
                lista.Add(Convert.ToInt32(item));
            }
            string decompressed = LZW.Decompress(lista);
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(decompressed);
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine("Surgió un error inesperado: " + e.Message);                        
        }
    }

    public static AVLTree<Persona> LLenarArbol()
    {
        AVLTree<Persona> arbolPersonas = new AVLTree<Persona>(); //Árbol AVL para almacenar las personas
        string route = @"C:\Users\AndresLima\Desktop\input.csv"; //Ruta del archivo a leer

        if (File.Exists(route))
        {
            string[] FileData = File.ReadAllLines(route);
            foreach (var item in FileData)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] fila = item.Split(";");
                    Persona? persona = JsonSerializer.Deserialize<Persona>(fila[1]);
                    string ruta = @"C:\Users\AndresLima\Desktop\inputs\";
                    string patron = "CONV-" + persona!.dpi + "*";
                    persona.convos = Directory.GetFiles(ruta, patron);
                    if (fila[0] == "INSERT")
                    {
                        arbolPersonas.Add(persona!, Delegates.DPIComparison);
                    }
                    else if (fila[0] == "DELETE")
                    {
                        arbolPersonas.Delete(persona!, Delegates.DPIComparison);
                    }
                    else if (fila[0] == "PATCH")
                    {
                        arbolPersonas.Patch(persona!, Delegates.DPIComparison);
                    }
                }
            }            
        }
        return arbolPersonas;
    }
}
