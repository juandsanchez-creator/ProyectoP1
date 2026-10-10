using System;

namespace SistemaAcademico
{
    class Program
    {
        // --- CONSTANTES GLOBALES (SECCIÓN 4 Y 8 DEL PDF) ---
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
            Console.WriteLine("   SISTEMA DE ANÁLISIS ACADÉMICO        ");
            Console.WriteLine("========================================");

            // --- CONFIGURACIÓN INICIAL (DIMENSIONAMIENTO - SECCIÓN 6.1) ---
            int cantidadEstudiantes = LeerEnteroEnRango($"Ingrese la cantidad de estudiantes ({MinEstudiantes}-{MaxEstudiantes}): ", MinEstudiantes, MaxEstudiantes);
            int cantidadEvaluaciones = LeerEnteroEnRango($"Ingrese la cantidad de evaluaciones ({MinEvaluaciones}-{MaxEvaluaciones}): ", MinEvaluaciones, MaxEvaluaciones);

            string[] nombres = new string[cantidadEstudiantes];
            int[,] calificaciones = new int[cantidadEstudiantes, cantidadEvaluaciones];

            Console.WriteLine("\n[✓] Sistema dimensionado exitosamente.");

            // --- MENÚ PRINCIPAL INTERACTIVO CON do-while Y switch (SECCIÓN 5) ---
            int opcion;
            do
            {
                MostrarMenu();
                opcion = LeerEnteroEnRango("Opción: ", 0, 7);

                switch (opcion)
                {
                    case 1:
                        Opcion1_RegistrarOModificarNombres(nombres);
                        break;
                    case 2:
                        Opcion2_RegistrarOModificarCalificaciones(calificaciones, nombres);
                        break;
                    case 3:
                        Opcion3_MostrarTablaCompleta(calificaciones, nombres);
                        break;
                    case 4:
                        Opcion4_ReporteIndividual(calificaciones, nombres);
                        break;
                    case 5:
                        Opcion5_EstadisticasPorEvaluacion(calificaciones);
                        break;
                    case 6:
                        Opcion6_EstadisticasGenerales(calificaciones, nombres);
                        break;
                    case 7:
                        Opcion7_AprobadosYReprobados(calificaciones, nombres);
                        break;
                    case 0:
                        Console.WriteLine("\nSaliendo del sistema... ¡Hasta pronto!");
                        break;
                }

            } while (opcion != 0);
        }

        // =========================================================================
        // MÉTODOS DE VALIDACIÓN ROBUSTA (SECCIÓN 3 Y 8 DEL PDF)
        // =========================================================================

        static int LeerEnteroEnRango(string mensaje, int minimo, int maximo)
        {
            int valor;
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();

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

                if (!string.IsNullOrWhiteSpace(entrada))
                {
                    return entrada.Trim();
                }

                Console.WriteLine("Error: El texto no puede estar vacío ni contener solo espacios.");
            }
        }

        // =========================================================================
        // MÉTODOS DE APOYO MATEMÁTICO Y USO OBLIGATORIO DE ref / out (SECCIÓN 7.1)
        // =========================================================================

        // USO OBLIGATORIO DE ref (RÚBRICA): Actualiza valores existentes durante un recorrido
        static void ActualizarExtremos(int valor, ref int maximo, ref int minimo)
        {
            if (valor > maximo) maximo = valor;
            if (valor < minimo) minimo = valor;
        }

        // USO OBLIGATORIO DE out (RÚBRICA): Devuelve múltiples métricas calculadas
        static void CalcularEstadisticasEstudiante(int[,] notas, int fila, out double promedio, out int notaMayor, out int notaMenor, out int aprobadas, out int reprobadas)
        {
            double suma = 0;
            int totalEval = notas.GetLength(1);
            notaMayor = notas[fila, 0];
            notaMenor = notas[fila, 0];
            aprobadas = 0;
            reprobadas = 0;

            for (int j = 0; j < totalEval; j++)
            {
                int nota = notas[fila, j];
                suma += nota;

                ActualizarExtremos(nota, ref notaMayor, ref notaMenor);

                if (nota >= NotaMinimaAprobacion) aprobadas++;
                else reprobadas++;
            }

            promedio = totalEval > 0 ? suma / totalEval : 0;
        }

        static double PromedioFila(int[,] notas, int fila)
        {
            double suma = 0;
            int totalCol = notas.GetLength(1);
            for (int j = 0; j < totalCol; j++) suma += notas[fila, j];
            return totalCol > 0 ? suma / totalCol : 0;
        }

        static double PromedioColumna(int[,] notas, int columna)
        {
            double suma = 0;
            int totalFil = notas.GetLength(0);
            for (int i = 0; i < totalFil; i++) suma += notas[i, columna];
            return totalFil > 0 ? suma / totalFil : 0;
        }

        // =========================================================================
        // MÓDULOS DEL MENÚ PRINCIPAL (OPCIONES 1 A 7 DEL PDF)
        // =========================================================================

        static void MostrarMenu()
        {
            Console.WriteLine("\n============================================");
            Console.WriteLine("       SISTEMA DE ANÁLISIS ACADÉMICO        ");
            Console.WriteLine("============================================");
            Console.WriteLine("1. Registrar o modificar nombres");
            Console.WriteLine("2. Registrar o modificar calificaciones");
            Console.WriteLine("3. Mostrar tabla completa de calificaciones");
            Console.WriteLine("4. Consultar reporte de un estudiante");
            Console.WriteLine("5. Mostrar estadísticas por evaluación");
            Console.WriteLine("6. Mostrar estadísticas generales del curso");
            Console.WriteLine("7. Mostrar estudiantes aprobados y reprobados");
            Console.WriteLine("0. Salir");
            Console.WriteLine("============================================");
        }

        // Opción 1: Registrar o modificar nombres
        static void Opcion1_RegistrarOModificarNombres(string[] nombres)
        {
            Console.WriteLine("\n--- REGISTRO / MODIFICACIÓN DE NOMBRES ---");
            Console.WriteLine("1. Registrar todos los nombres");
            Console.WriteLine("2. Modificar un estudiante específico");
            int sub = LeerEnteroEnRango("Seleccione una opción: ", 1, 2);

            if (sub == 1)
            {
                for (int i = 0; i < nombres.Length; i++)
                {
                    nombres[i] = LeerTextoNoVacio($"Nombre del estudiante {i + 1}: ");
                }
                Console.WriteLine("\n[✓] Todos los nombres han sido registrados.");
            }
            else
            {
                int num = LeerEnteroEnRango($"Número de estudiante a modificar (1-{nombres.Length}): ", 1, nombres.Length);
                nombres[num - 1] = LeerTextoNoVacio($"Nuevo nombre para el estudiante {num}: ");
                Console.WriteLine("\n[✓] Nombre actualizado exitosamente.");
            }
        }

        // Opción 2: Registrar o modificar calificaciones
        static void Opcion2_RegistrarOModificarCalificaciones(int[,] calificaciones, string[] nombres)
        {
            Console.WriteLine("\n--- REGISTRO / MODIFICACIÓN DE CALIFICACIONES ---");
            Console.WriteLine("1. Carga completa de la matriz");
            Console.WriteLine("2. Modificar una calificación individual");
            int sub = LeerEnteroEnRango("Seleccione una opción: ", 1, 2);

            int totalEst = calificaciones.GetLength(0);
            int totalEval = calificaciones.GetLength(1);

            if (sub == 1)
            {
                for (int i = 0; i < totalEst; i++)
                {
                    string nom = string.IsNullOrWhiteSpace(nombres[i]) ? $"Estudiante {i + 1}" : nombres[i];
                    Console.WriteLine($"\nIngresando notas para: {nom}");
                    for (int j = 0; j < totalEval; j++)
                    {
                        calificaciones[i, j] = LeerEnteroEnRango($"  Evaluación {j + 1} ({NotaMinima}-{NotaMaxima}): ", NotaMinima, NotaMaxima);
                    }
                }
                Console.WriteLine("\n[✓] Calificaciones registradas exitosamente.");
            }
            else
            {
                int estNum = LeerEnteroEnRango($"Número del estudiante (1 a {totalEst}): ", 1, totalEst);
                int evalNum = LeerEnteroEnRango($"Número de la evaluación (1 a {totalEval}): ", 1, totalEval);

                int f = estNum - 1;
                int c = evalNum - 1;
                string nom = string.IsNullOrWhiteSpace(nombres[f]) ? $"Estudiante {estNum}" : nombres[f];

                Console.WriteLine($"-> {nom} | Evaluación {evalNum} | Nota actual: {calificaciones[f, c]}");
                calificaciones[f, c] = LeerEnteroEnRango($"Nueva calificación ({NotaMinima}-{NotaMaxima}): ", NotaMinima, NotaMaxima);
                Console.WriteLine("\n[✓] Calificación modificada correctamente.");
            }
        }

        // Opción 3: Mostrar tabla completa
        static void Opcion3_MostrarTablaCompleta(int[,] calificaciones, string[] nombres)
        {
            Console.WriteLine("\n======================= TABLA COMPLETA DE CALIFICACIONES =======================");
            int totalEst = calificaciones.GetLength(0);
            int totalEval = calificaciones.GetLength(1);

            for (int i = 0; i < totalEst; i++)
            {
                string nom = string.IsNullOrWhiteSpace(nombres[i]) ? $"Estudiante {i + 1}" : nombres[i];
                CalcularEstadisticasEstudiante(calificaciones, i, out double prom, out int mayor, out int menor, out _, out _);

                // USO OBLIGATORIO DE OPERADOR TERNARIO ?: (RÚBRICA SECCIÓN 6.4)
                string estado = prom >= NotaMinimaAprobacion ? "APROBADO" : "REPROBADO";

                Console.Write($"{nom,-15} | ");
                for (int j = 0; j < totalEval; j++)
                {
                    Console.Write($"Eval {j + 1}: {calificaciones[i, j],3} | ");
                }
                Console.WriteLine($"Prom: {prom,6:F2} | Max: {mayor,3} | Min: {menor,3} | {estado}");
            }
            Console.WriteLine("===============================================================================");
        }

        // Opción 4: Reporte individual
        static void Opcion4_ReporteIndividual(int[,] calificaciones, string[] nombres)
        {
            Console.WriteLine("\n--- CONSULTA DE REPORTE INDIVIDUAL ---");
            int totalEst = calificaciones.GetLength(0);
            int estNum = LeerEnteroEnRango($"Ingrese número de estudiante a consultar (1-{totalEst}): ", 1, totalEst);

            int f = estNum - 1;
            string nom = string.IsNullOrWhiteSpace(nombres[f]) ? $"Estudiante {estNum}" : nombres[f];
            CalcularEstadisticasEstudiante(calificaciones, f, out double prom, out int mayor, out int menor, out int apr, out int rep);

            string estado = prom >= NotaMinimaAprobacion ? "APROBADO" : "REPROBADO";

            Console.WriteLine($"\nEstudiante: {nom}");
            Console.Write("Calificaciones: ");
            for (int j = 0; j < calificaciones.GetLength(1); j++)
            {
                Console.Write($"[Eval {j + 1}: {calificaciones[f, j]}] ");
            }
            Console.WriteLine($"\nPromedio Final: {prom:F2}");
            Console.WriteLine($"Nota Mayor: {mayor} | Nota Menor: {menor}");
            Console.WriteLine($"Evaluaciones Aprobadas: {apr} | Reprobadas: {rep}");
            Console.WriteLine($"Estado Final: {estado}");
        }

        // Opción 5: Estadísticas por evaluación
        static void Opcion5_EstadisticasPorEvaluacion(int[,] calificaciones)
        {
            Console.WriteLine("\n--- ESTADÍSTICAS POR EVALUACIÓN ---");
            int totalEst = calificaciones.GetLength(0);
            int totalEval = calificaciones.GetLength(1);

            for (int j = 0; j < totalEval; j++)
            {
                double prom = PromedioColumna(calificaciones, j);
                int mayor = calificaciones[0, j];
                int menor = calificaciones[0, j];
                int aprobados = 0;
                int reprobados = 0;

                for (int i = 0; i < totalEst; i++)
                {
                    int nota = calificaciones[i, j];
                    ActualizarExtremos(nota, ref mayor, ref menor);

                    if (nota >= NotaMinimaAprobacion) aprobados++;
                    else reprobados++;
                }

                Console.WriteLine($"\nEvaluación {j + 1}:");
                Console.WriteLine($"  - Promedio: {prom:F2}");
                Console.WriteLine($"  - Nota Mayor: {mayor} | Nota Menor: {menor}");
                Console.WriteLine($"  - Estudiantes con nota >= {NotaMinimaAprobacion}: {aprobados}");
                Console.WriteLine($"  - Estudiantes con nota < {NotaMinimaAprobacion}: {reprobados}");
            }
        }

        // Opción 6: Estadísticas generales
        static void Opcion6_EstadisticasGenerales(int[,] calificaciones, string[] nombres)
        {
            Console.WriteLine("\n--- ESTADÍSTICAS GENERALES DEL CURSO ---");
            int totalEst = calificaciones.GetLength(0);
            int totalEval = calificaciones.GetLength(1);

            double sumaTotal = 0;
            int notaMasAlta = calificaciones[0, 0];
            int notaMasBaja = calificaciones[0, 0];

            double promMayor = PromedioFila(calificaciones, 0);
            double promMenor = promMayor;

            int aprobados = 0;
            int reprobados = 0;

            for (int i = 0; i < totalEst; i++)
            {
                double promEst = PromedioFila(calificaciones, i);
                if (promEst > promMayor) promMayor = promEst;
                if (promEst < promMenor) promMenor = promEst;

                if (promEst >= NotaMinimaAprobacion) aprobados++;
                else reprobados++;

                for (int j = 0; j < totalEval; j++)
                {
                    int nota = calificaciones[i, j];
                    sumaTotal += nota;
                    ActualizarExtremos(nota, ref notaMasAlta, ref notaMasBaja);
                }
            }

            double promGeneral = sumaTotal / (totalEst * totalEval);
            double pctAprobados = ((double)aprobados / totalEst) * 100;
            double pctReprobados = ((double)reprobados / totalEst) * 100;

            Console.WriteLine($"Promedio general del curso: {promGeneral:F2}");
            Console.WriteLine($"Nota más alta de toda la matriz: {notaMasAlta}");
            Console.WriteLine($"Nota más baja de toda la matriz: {notaMasBaja}");
            Console.WriteLine($"Promedio más alto entre estudiantes: {promMayor:F2}");
            Console.WriteLine($"Promedio más bajo entre estudiantes: {promMenor:F2}");
            Console.WriteLine($"Aprobados por promedio: {aprobados} ({pctAprobados:F2}%)");
            Console.WriteLine($"Reprobados por promedio: {reprobados} ({pctReprobados:F2}%)");
        }

        // Opción 7: Aprobados y reprobados con uso de continue y break
        static void Opcion7_AprobadosYReprobados(int[,] calificaciones, string[] nombres)
        {
            Console.WriteLine("\n--- LISTADO DE APROBADOS Y REPROBADOS ---");
            int totalEst = calificaciones.GetLength(0);

            // USO OBLIGATORIO DE continue (RÚBRICA SECCIÓN 6.8)
            Console.WriteLine("\n[ESTUDIANTES APROBADOS]");
            for (int i = 0; i < totalEst; i++)
            {
                double prom = PromedioFila(calificaciones, i);
                if (prom < NotaMinimaAprobacion) continue;

                string nom = string.IsNullOrWhiteSpace(nombres[i]) ? $"Estudiante {i + 1}" : nombres[i];
                Console.WriteLine($"✓ {nom} - Promedio: {prom:F2}");
            }

            Console.WriteLine("\n[ESTUDIANTES REPROBADOS]");
            for (int i = 0; i < totalEst; i++)
            {
                double prom = PromedioFila(calificaciones, i);
                if (prom >= NotaMinimaAprobacion) continue;

                string nom = string.IsNullOrWhiteSpace(nombres[i]) ? $"Estudiante {i + 1}" : nombres[i];
                Console.WriteLine($"✗ {nom} - Promedio: {prom:F2}");
            }

            // USO OBLIGATORIO DE break (RÚBRICA SECCIÓN 6.8)
            Console.WriteLine("\n[BÚSQUEDA DE CONTROL: Primer estudiante en riesgo crítico (< 50)]");
            bool hayCritico = false;
            for (int i = 0; i < totalEst; i++)
            {
                double prom = PromedioFila(calificaciones, i);
                if (prom < 50)
                {
                    string nom = string.IsNullOrWhiteSpace(nombres[i]) ? $"Estudiante {i + 1}" : nombres[i];
                    Console.WriteLine($"(!) Primer caso crítico detectado: {nom} con promedio {prom:F2}. Deteniendo búsqueda.");
                    hayCritico = true;
                    break;
                }
            }
            if (!hayCritico) Console.WriteLine("Ningún estudiante tiene promedio inferior a 50.");
        }
    }
}