using System;

namespace SistemaAcademico
{
    class Program
    {
        // --- CONSTANTES ---
        const int NotaMinimaAprobacion = 70;
        const int MinEstudiantes = 3;
        const int MaxEstudiantes = 10;
        const int MinEvaluaciones = 2;
        const int MaxEvaluaciones = 5;
        const int NotaMinima = 0;
        const int NotaMaxima = 100;

        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   Bienvenido al Sistema Académico      ");
            Console.WriteLine("========================================");

             // PRUEBAS DE VALIDACIÓN:
            string nombre = LeerTextoNoVacio("Ingrese nombre del estudiante: ");
            int estudiantes = LeerEnteroEnRango("Cantidad de estudiantes (3-10): ", MinEstudiantes, MaxEstudiantes);

            Console.WriteLine($"\nDatos válidos recibidos: {nombre}, {estudiantes} estudiantes.");
        }

        // --- MÉTODOS DE VALIDACIÓN ROBUSTA ---

        static int LeerEnteroEnRango(string mensaje, int minimo, int maximo)
        {
            int valor;
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();

                // Valida que sea número entero Y que esté dentro de los límites
                if (int.TryParse(entrada, out valor) && valor >= minimo && valor <= maximo)
                {
                    return valor;
                }

                Console.WriteLine($"Error: Ingrese un número válido entre {minimo} y {maximo}.");
            }
        }

        static string LeerTextoNoVacio(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();

                // Valida que no sea nulo, ni vacío, ni puros espacios en blanco
                if (!string.IsNullOrWhiteSpace(entrada))
                {
                    return entrada.Trim();
                }

                Console.WriteLine("Error: El texto no puede estar vacío ni contener solo espacios.");
            }
        }

    }
}