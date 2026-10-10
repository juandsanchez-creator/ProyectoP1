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
            // --- PARTE 4: MÓDULO DE CONFIGURACIÓN INICIAL (DIMENSIONAMIENTO) ---
            Console.WriteLine("\n--- CONFIGURACIÓN INICIAL DEL CURSO ---");
            
            // 1. Solicitar y validar dimensiones usando las constantes ya definidas
            int cantidadEstudiantes = LeerEnteroEnRango($"Ingrese la cantidad de estudiantes ({MinEstudiantes}-{MaxEstudiantes}): ", MinEstudiantes, MaxEstudiantes);
            int cantidadEvaluaciones = LeerEnteroEnRango($"Ingrese la cantidad de evaluaciones ({MinEvaluaciones}-{MaxEvaluaciones}): ", MinEvaluaciones, MaxEvaluaciones);

            // 2. Instanciar las estructuras fijas con los tamaños validados
            string[] nombres = new string[cantidadEstudiantes];
            int[,] calificaciones = new int[cantidadEstudiantes, cantidadEvaluaciones];

            Console.WriteLine("\n[✓] Sistema dimensionado correctamente:");
            Console.WriteLine($"    - Estudiantes registrados para el curso: {nombres.Length}");
            Console.WriteLine($"    - Evaluaciones por estudiante: {calificaciones.GetLength(1)}");


            // --- PARTE 5: MENÚ PRINCIPAL INTERACTIVO ---
            int opcion;
            do
            {
                MostrarMenu();
                opcion = LeerEnteroEnRango("Opción: ", 0, 7);

                switch (opcion)
                {
                    case 1:
                        RegistrarNombres(nombres);
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        Console.WriteLine("\n[!] En construcción...");
                        break;
                    case 0:
                        Console.WriteLine("\nSaliendo del sistema... ¡Hasta pronto!");
                        break;
                }

            } while (opcion != 0);


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

        // --- MÉTODO DEL MENÚ --- parte 5
        static void MostrarMenu()
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("            MENÚ PRINCIPAL              ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Registrar nombres de estudiantes");
            Console.WriteLine("2. Ingresar calificaciones");
            Console.WriteLine("3. Ver reporte general de calificaciones");
            Console.WriteLine("4. Ver promedio por estudiante");
            Console.WriteLine("5. Ver promedio por evaluación");
            Console.WriteLine("6. Ver estudiante con mayor promedio");
            Console.WriteLine("7. Ver estudiante con menor promedio");
            Console.WriteLine("0. Salir");
            Console.WriteLine("========================================");
        }

    }
}
static void RegistrarNombres(string[] nombres)
{
    Console.WriteLine("\n--- REGISTRO DE NOMBRES DE ESTUDIANTES ---");
    for (int i = 0; i < nombres.Length; i++)
    {
        string prompt = string.IsNullOrWhiteSpace(nombres[i])
            ? $"Ingrese el nombre del estudiante {i + 1}: "
            : $"Modificar nombre del estudiante {i + 1} [{nombres[i]}]: ";
        
        nombres[i] = LeerTextoNoVacio(prompt);
    }
    Console.WriteLine("\n[✓] ¡Registro de nombres completado con éxito!");
}