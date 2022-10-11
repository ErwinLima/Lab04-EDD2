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
