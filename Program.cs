using System;
using System.Globalization;

namespace SistemaAnalisisAcademico
{
    class Program
    {
        // ---------- Constantes (evitan "numeros magicos") ----------
        const int NotaMinimaAprobacion = 70;
        const int MinEstudiantes = 3;
        const int MaxEstudiantes = 10;
        const int MinEvaluaciones = 2;
        const int MaxEvaluaciones = 5;
        const int NotaMinima = 0;
        const int NotaMaxima = 100;
        const int AnchoNombre = 15;
        const int AnchoLinea = 70;

        static void Main()
        {
            // Cultura invariable: los decimales siempre se muestran con punto (85.00)
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine("CONFIGURACION INICIAL");
            int cantidadEstudiantes = LeerEnteroEnRango(
                $"Cantidad de estudiantes ({MinEstudiantes}-{MaxEstudiantes}): ", MinEstudiantes, MaxEstudiantes);
            int cantidadEvaluaciones = LeerEnteroEnRango(
                $"Cantidad de evaluaciones ({MinEvaluaciones}-{MaxEvaluaciones}): ", MinEvaluaciones, MaxEvaluaciones);

            string[] nombres = new string[cantidadEstudiantes];
            for (int i = 0; i < nombres.Length; i++)
            {
                nombres[i] = "Estudiante " + (i + 1); // nombre provisional hasta usar la opcion 1
            }

            int[,] calificaciones = new int[cantidadEstudiantes, cantidadEvaluaciones];
            bool notasCargadas = false; // evita reportes sobre una matriz vacia

            int opcion;
            do
            {
                MostrarMenu();
                opcion = LeerEnteroEnRango("Opcion: ", 0, 8);
                Console.WriteLine();

                switch (opcion)
                {
                    case 1:
                        RegistrarNombres(nombres);
                        break;
                    case 2:
                        notasCargadas = RegistrarCalificaciones(calificaciones, nombres, notasCargadas);
                        break;
                    case 3:
                        if (HayNotas(notasCargadas)) ImprimirMatriz(calificaciones, nombres);
                        break;
                    case 4:
                        if (HayNotas(notasCargadas)) ReporteEstudiante(calificaciones, nombres);
                        break;
                    case 5:
                        if (HayNotas(notasCargadas)) EstadisticasPorEvaluacion(calificaciones);
                        break;
                    case 6:
                        if (HayNotas(notasCargadas)) EstadisticasGenerales(calificaciones, nombres);
                        break;
                    case 7:
                        if (HayNotas(notasCargadas)) MostrarAprobadosYReprobados(calificaciones, nombres);
                        break;
                    case 8:
                        // ref: el metodo reemplaza los arreglos por otros de distinto tamano
                        CargarCasoPrueba(ref nombres, ref calificaciones);
                        notasCargadas = true;
                        break;
                    case 0:
                        Console.WriteLine("Programa finalizado. Hasta luego.");
                        break;
                    default:
                        Console.WriteLine("Opcion no valida."); // no deberia ocurrir
                        break;
                }

                if (opcion != 0)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }
            } while (opcion != 0);
        }

        // =====================================================
        //  MENU Y LECTURA DE DATOS
        // =====================================================

        static void MostrarMenu()
        {
            string linea = new string('=', 44);
            Console.WriteLine(linea);
            Console.WriteLine("SISTEMA DE ANALISIS ACADEMICO");
            Console.WriteLine(linea);
            Console.WriteLine("1. Registrar o modificar nombres");
            Console.WriteLine("2. Registrar o modificar calificaciones");
            Console.WriteLine("3. Mostrar tabla completa de calificaciones");
            Console.WriteLine("4. Consultar reporte de un estudiante");
            Console.WriteLine("5. Mostrar estadisticas por evaluacion");
            Console.WriteLine("6. Mostrar estadisticas generales del curso");
            Console.WriteLine("7. Mostrar estudiantes aprobados y reprobados");
            Console.WriteLine("8. Cargar caso de prueba (Ana, Bruno, Carla, Diego)");
            Console.WriteLine("0. Salir");
            Console.WriteLine(linea);
        }

        // Repite la lectura (while) hasta recibir un entero dentro del rango.
        static int LeerEnteroEnRango(string mensaje, int minimo, int maximo)
        {
            int valor = 0;
            bool valido = false;
            while (!valido)
            {
                Console.Write(mensaje);
                string? texto = Console.ReadLine();
                if (int.TryParse(texto, out valor) && valor >= minimo && valor <= maximo)
                {
                    valido = true;
                }
                else
                {
                    Console.WriteLine($"  Entrada invalida. Ingrese un numero entero entre {minimo} y {maximo}.");
                }
            }
            return valor;
        }

        // Una sola lectura: devuelve false si el texto no es un entero (sin repetir).
        // El resultado numerico sale por "out".
        static bool TryLeerEntero(string mensaje, out int valor)
        {
            Console.Write(mensaje);
            string? texto = Console.ReadLine();
            return int.TryParse(texto, out valor);
        }

        static string LeerTextoNoVacio(string mensaje)
        {
            string texto = "";
            while (string.IsNullOrWhiteSpace(texto))
            {
                Console.Write(mensaje);
                texto = (Console.ReadLine() ?? "").Trim();
                if (texto.Length == 0)
                {
                    Console.WriteLine("  El texto no puede estar vacio.");
                }
            }
            return texto;
        }

        static bool HayNotas(bool notasCargadas)
        {
            if (!notasCargadas)
            {
                Console.WriteLine("Primero debe registrar las calificaciones (opcion 2) o cargar el caso de prueba (opcion 8).");
            }
            return notasCargadas;
        }

        // =====================================================
        //  REGISTRO DE DATOS
        // =====================================================

        static void MostrarListaEstudiantes(string[] nombres)
        {
            int numero = 1;
            foreach (string nombre in nombres)
            {
                Console.WriteLine($"  {numero}. {nombre}");
                numero++;
            }
        }

        static void RegistrarNombres(string[] nombres)
        {
            Console.WriteLine("1. Registrar todos los nombres");
            Console.WriteLine("2. Modificar el nombre de un estudiante");
            int modo = LeerEnteroEnRango("Opcion: ", 1, 2);

            if (modo == 1)
            {
                for (int i = 0; i < nombres.Length; i++)
                {
                    nombres[i] = LeerTextoNoVacio($"Nombre del estudiante {i + 1}: ");
                }
                Console.WriteLine("Nombres registrados correctamente.");
            }
            else
            {
                MostrarListaEstudiantes(nombres);
                int numero = LeerEnteroEnRango($"Numero de registro (1-{nombres.Length}): ", 1, nombres.Length);
                string anterior = nombres[numero - 1];
                nombres[numero - 1] = LeerTextoNoVacio("Nuevo nombre: ");
                Console.WriteLine($"Nombre actualizado: '{anterior}' -> '{nombres[numero - 1]}'.");
            }
        }

        // Devuelve true si la matriz ya contiene calificaciones validas.
        static bool RegistrarCalificaciones(int[,] notas, string[] nombres, bool yaCargadas)
        {
            Console.WriteLine("1. Carga completa de calificaciones");
            Console.WriteLine("2. Modificar una calificacion");
            int modo = LeerEnteroEnRango("Opcion: ", 1, 2);

            if (modo == 1)
            {
                // fila = estudiante, columna = evaluacion
                for (int i = 0; i < notas.GetLength(0); i++)
                {
                    for (int j = 0; j < notas.GetLength(1); j++)
                    {
                        notas[i, j] = LeerEnteroEnRango(
                            $"{nombres[i]} - Evaluacion {j + 1} ({NotaMinima}-{NotaMaxima}): ", NotaMinima, NotaMaxima);
                    }
                }
                Console.WriteLine("Calificaciones registradas correctamente.");
                return true;
            }

            if (!yaCargadas)
            {
                Console.WriteLine("Aun no hay calificaciones. Use primero la carga completa.");
                return false;
            }

            MostrarListaEstudiantes(nombres);
            int estudiante = LeerEnteroEnRango($"Numero de estudiante (1-{notas.GetLength(0)}): ", 1, notas.GetLength(0));
            int evaluacion = LeerEnteroEnRango($"Numero de evaluacion (1-{notas.GetLength(1)}): ", 1, notas.GetLength(1));

            Console.WriteLine($"Estudiante: {nombres[estudiante - 1]} | Evaluacion {evaluacion} | Nota actual: {notas[estudiante - 1, evaluacion - 1]}");
            notas[estudiante - 1, evaluacion - 1] = LeerEnteroEnRango(
                $"Nueva nota ({NotaMinima}-{NotaMaxima}): ", NotaMinima, NotaMaxima);
            Console.WriteLine("Calificacion modificada correctamente.");
            return true;
        }

        // Unico lugar con datos "quemados": el caso de prueba obligatorio.
        static void CargarCasoPrueba(ref string[] nombres, ref int[,] notas)
        {
            nombres = new string[] { "Ana", "Bruno", "Carla", "Diego" };
            notas = new int[,]
            {
                { 85, 78, 92 },
                { 60, 70, 65 },
                { 100, 95, 90 },
                { 69, 72, 75 }
            };
            Console.WriteLine("Caso de prueba cargado (4 estudiantes, 3 evaluaciones).");
            Console.WriteLine("Nota: los datos anteriores fueron reemplazados.");
        }

        // =====================================================
        //  CALCULOS REUTILIZABLES
        // =====================================================

        // Cast a double ANTES de dividir para evitar division entera.
        static double PromedioFila(int[,] notas, int fila)
        {
            int suma = 0;
            for (int j = 0; j < notas.GetLength(1); j++)
            {
                suma += notas[fila, j];
            }
            return (double)suma / notas.GetLength(1);
        }

        static double PromedioColumna(int[,] notas, int columna)
        {
            int suma = 0;
            for (int i = 0; i < notas.GetLength(0); i++)
            {
                suma += notas[i, columna];
            }
            return (double)suma / notas.GetLength(0);
        }

        // ref: actualiza valores que YA existen en el metodo que llama.
        static void ActualizarExtremos(int valor, ref int mayor, ref int menor)
        {
            if (valor > mayor) mayor = valor;
            if (valor < menor) menor = valor;
        }

        // out: un solo metodo entrega TRES resultados (promedio, mayor y menor).
        static void CalcularEstadisticasFila(int[,] notas, int fila, out double promedio, out int mayor, out int menor)
        {
            promedio = PromedioFila(notas, fila);
            mayor = notas[fila, 0];
            menor = notas[fila, 0];
            for (int j = 1; j < notas.GetLength(1); j++)
            {
                ActualizarExtremos(notas[fila, j], ref mayor, ref menor);
            }
        }

        static void ExtremosColumna(int[,] notas, int columna, out int mayor, out int menor)
        {
            mayor = notas[0, columna];
            menor = notas[0, columna];
            for (int i = 1; i < notas.GetLength(0); i++)
            {
                ActualizarExtremos(notas[i, columna], ref mayor, ref menor);
            }
        }

        static string ObtenerEstado(double promedio)
        {
            // Operador ternario
            string estado = promedio >= NotaMinimaAprobacion ? "APROBADO" : "REPROBADO";
            return estado;
        }

        static string Truncar(string texto, int ancho)
        {
            return texto.Length > ancho ? texto.Substring(0, ancho) : texto;
        }

        // =====================================================
        //  REPORTES
        // =====================================================

        // Opcion 3
        static void ImprimirMatriz(int[,] notas, string[] nombres)
        {
            string linea = new string('-', AnchoLinea);
            Console.WriteLine("TABLA DE CALIFICACIONES");
            Console.WriteLine(linea);

            Console.Write($"{"No.",-4}{"Estudiante",-16}");
            for (int j = 0; j < notas.GetLength(1); j++)
            {
                Console.Write($"{"Eval " + (j + 1),7}");
            }
            Console.WriteLine($"{"Promedio",10}{"Mayor",7}{"Menor",7}  Estado");
            Console.WriteLine(linea);

            for (int i = 0; i < notas.GetLength(0); i++)
            {
                Console.Write($"{i + 1,-4}{Truncar(nombres[i], AnchoNombre),-16}");
                for (int j = 0; j < notas.GetLength(1); j++)
                {
                    Console.Write($"{notas[i, j],7}");
                }
                CalcularEstadisticasFila(notas, i, out double promedio, out int mayor, out int menor);
                Console.WriteLine($"{promedio,10:F2}{mayor,7}{menor,7}  {ObtenerEstado(promedio)}");
            }
            Console.WriteLine(linea);
        }

        // Opcion 4
        static void ReporteEstudiante(int[,] notas, string[] nombres)
        {
            MostrarListaEstudiantes(nombres);
            if (!TryLeerEntero($"Numero de estudiante (1-{notas.GetLength(0)}): ", out int numero)
                || numero < 1 || numero > notas.GetLength(0))
            {
                Console.WriteLine("El estudiante indicado no existe.");
                return;
            }

            int fila = numero - 1;
            CalcularEstadisticasFila(notas, fila, out double promedio, out int mayor, out int menor);

            int aprobadas = 0;
            int reprobadas = 0;
            Console.WriteLine($"\nREPORTE DE: {nombres[fila]}");
            for (int j = 0; j < notas.GetLength(1); j++)
            {
                string resultado = notas[fila, j] >= NotaMinimaAprobacion ? "aprobada" : "reprobada";
                Console.WriteLine($"  Evaluacion {j + 1}: {notas[fila, j],3}  ({resultado})");
                if (notas[fila, j] >= NotaMinimaAprobacion) aprobadas++;
                else reprobadas++;
            }
            Console.WriteLine($"Promedio: {promedio:F2}");
            Console.WriteLine($"Nota mayor: {mayor}");
            Console.WriteLine($"Nota menor: {menor}");
            Console.WriteLine($"Evaluaciones aprobadas: {aprobadas}");
            Console.WriteLine($"Evaluaciones reprobadas: {reprobadas}");
            Console.WriteLine($"Estado final: {ObtenerEstado(promedio)}");
        }

        // Opcion 5
        static void EstadisticasPorEvaluacion(int[,] notas)
        {
            string linea = new string('-', AnchoLinea);
            Console.WriteLine("ESTADISTICAS POR EVALUACION");
            Console.WriteLine(linea);
            Console.WriteLine($"{"Evaluacion",-12}{"Promedio",10}{"Mayor",8}{"Menor",8}{">= " + NotaMinimaAprobacion,10}{"< " + NotaMinimaAprobacion,10}");
            Console.WriteLine(linea);

            for (int j = 0; j < notas.GetLength(1); j++)
            {
                ExtremosColumna(notas, j, out int mayor, out int menor);

                int conNotaAprobatoria = 0;
                int conNotaInferior = 0;
                for (int i = 0; i < notas.GetLength(0); i++)
                {
                    if (notas[i, j] >= NotaMinimaAprobacion) conNotaAprobatoria++;
                    else conNotaInferior++;
                }

                Console.WriteLine($"{"Eval " + (j + 1),-12}{PromedioColumna(notas, j),10:F2}{mayor,8}{menor,8}{conNotaAprobatoria,10}{conNotaInferior,10}");
            }
            Console.WriteLine(linea);
        }

        // Opcion 6
        static void EstadisticasGenerales(int[,] notas, string[] nombres)
        {
            int filas = notas.GetLength(0);
            int columnas = notas.GetLength(1);

            int sumaTotal = 0;
            int notaAlta = notas[0, 0];
            int notaBaja = notas[0, 0];
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    sumaTotal += notas[i, j];
                    ActualizarExtremos(notas[i, j], ref notaAlta, ref notaBaja);
                }
            }
            double promedioGeneral = (double)sumaTotal / (filas * columnas);

            double promedioAlto = PromedioFila(notas, 0);
            double promedioBajo = promedioAlto;
            int mejor = 0;
            int peor = 0;
            int aprobados = 0;
            for (int i = 0; i < filas; i++)
            {
                double promedio = PromedioFila(notas, i);
                if (promedio > promedioAlto) { promedioAlto = promedio; mejor = i; }
                if (promedio < promedioBajo) { promedioBajo = promedio; peor = i; }
                if (promedio >= NotaMinimaAprobacion) aprobados++;
            }
            int reprobados = filas - aprobados;
            double porcentajeAprobados = (double)aprobados / filas * 100;
            double porcentajeReprobados = (double)reprobados / filas * 100;

            Console.WriteLine("ESTADISTICAS GENERALES DEL CURSO");
            Console.WriteLine($"Promedio general:           {promedioGeneral:F2}");
            Console.WriteLine($"Nota mas alta:              {notaAlta}");
            Console.WriteLine($"Nota mas baja:              {notaBaja}");
            Console.WriteLine($"Promedio mas alto:          {promedioAlto:F2} ({nombres[mejor]})");
            Console.WriteLine($"Promedio mas bajo:          {promedioBajo:F2} ({nombres[peor]})");
            Console.WriteLine($"Aprobados por promedio:     {aprobados} ({porcentajeAprobados:F2}%)");
            Console.WriteLine($"Reprobados por promedio:    {reprobados} ({porcentajeReprobados:F2}%)");
        }

        // Opcion 7
        static void MostrarAprobadosYReprobados(int[,] notas, string[] nombres)
        {
            ListarPorEstado(notas, nombres, true);
            Console.WriteLine();
            ListarPorEstado(notas, nombres, false);

            int primero = BuscarPrimerReprobado(notas);
            Console.WriteLine();
            if (primero >= 0)
            {
                Console.WriteLine($"Primer estudiante reprobado encontrado: {nombres[primero]}");
            }
            else
            {
                Console.WriteLine("No hay estudiantes reprobados.");
            }
        }

        // Un solo metodo sirve para ambos listados (evita duplicar codigo).
        static void ListarPorEstado(int[,] notas, string[] nombres, bool listarAprobados)
        {
            Console.WriteLine(listarAprobados ? "--- ESTUDIANTES APROBADOS ---" : "--- ESTUDIANTES REPROBADOS ---");
            int contador = 0;
            for (int i = 0; i < notas.GetLength(0); i++)
            {
                double promedio = PromedioFila(notas, i);
                bool esAprobado = promedio >= NotaMinimaAprobacion;
                if (esAprobado != listarAprobados)
                {
                    continue; // salta a los estudiantes del otro grupo
                }
                Console.WriteLine($"  {nombres[i],-16} Promedio: {promedio:F2}");
                contador++;
            }
            if (contador == 0)
            {
                Console.WriteLine("  (ninguno)");
            }
        }

        // Busqueda con break: se detiene al encontrar el primero. Devuelve -1 si no hay.
        static int BuscarPrimerReprobado(int[,] notas)
        {
            int indice = -1;
            for (int i = 0; i < notas.GetLength(0); i++)
            {
                if (PromedioFila(notas, i) < NotaMinimaAprobacion)
                {
                    indice = i;
                    break;
                }
            }
            return indice;
        }
    }
}
