using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PruebaEmpleado
{
    class Program
    {
        static string cadenaDeConexion = string.Empty;
        static SqlConnection conexion = null;
        static SqlCommand mySqlCommand = null;
        static SqlDataReader mySqlDataReader = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenido al software de gestión de los empleados de la Empresa. Por favor, seleccione una opción para continuar: \n");
            ConectarASQLServer();
            IniciarPrograma();
            CerrarConexion();
            Console.WriteLine("\nPresione una tecla para cerrar esta ventana.");
            Console.ReadKey();
        }

        private static void ConectarASQLServer()
        {
            try
            {
                cadenaDeConexion = "Server=DESKTOP-I1VHTFL;Database=PruebaEmpleados;Trusted_Connection=True";
                conexion = new SqlConnection(cadenaDeConexion);
                conexion.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Conexión Errónea. Detalles: ");
                Console.WriteLine(e.Message);
            }
        }

        private static void IniciarPrograma()
        {
            Console.WriteLine("1- Mostrar los datos referidos a los empleados actuales. \n2- Agregar un nuevo empleado \n3- Actualizar el salario de un empleado actual. \n4- Eliminar un empleado actual.\n");
            int opcionSeleccionada = Convert.ToInt32(Console.ReadLine());

            switch (opcionSeleccionada)
            {
                case 1:
                    MostrarDatosDeEmpleado();
                    break;
                case 2:
                    AgregarNuevoEmpleado();
                    break;
                case 3:
                    ActualizarEmpleado();
                    break;
                case 4:
                    EliminarEmpleado();
                    break;
                default:
                    Console.WriteLine("\nError: Debe seleccionar una de las opciones mencionadas.\n");
                    IniciarPrograma();
                    break;
            }
        }

        private static void MostrarDatosDeEmpleado()
        {
            try
            {
                string sqlQuery = "SELECT * FROM Empleado";
                mySqlCommand = new SqlCommand(sqlQuery,conexion);
                mySqlDataReader = mySqlCommand.ExecuteReader();
                Console.WriteLine("EMPLEADOS ACTUALES");
                Console.WriteLine("\nNombre \t" + "Edad \t" + "Salario \t");

                while (mySqlDataReader.Read())
                {
                    Console.WriteLine(mySqlDataReader[1]+ "\t"+ mySqlDataReader[2] + "\t" + mySqlDataReader[3]);
                }

                mySqlDataReader.Close();

                DeseaSalir();
            }
            catch (Exception e)
            {
                Console.WriteLine("Conexión Errónea. Detalles: ");
                Console.WriteLine(e.Message);
            }
        }

        private static void AgregarNuevoEmpleado()
        {
            try
            {
                Console.WriteLine("Introduzca los datos del empleado a agregar: ");
                Console.WriteLine("NOMBRE: ");
                string nombreIntroducido = Console.ReadLine();
                Console.WriteLine("EDAD: ");
                int edadIntroducida = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("SALARIO: ");
                decimal salarioIntroducido = Convert.ToDecimal(Console.ReadLine());
                string sqlQuery = "INSERT into Empleado(Name, Age, Salary) values(@nombre, @edad, @salario)";
                mySqlCommand = new SqlCommand(sqlQuery, conexion);
                mySqlCommand.Parameters.AddWithValue("nombre", nombreIntroducido);
                mySqlCommand.Parameters.AddWithValue("edad", edadIntroducida);
                mySqlCommand.Parameters.AddWithValue("salario", salarioIntroducido);
                int resultado = mySqlCommand.ExecuteNonQuery();

                if (resultado == 1)
                    Console.WriteLine(resultado + " registro actualizado.");
                else
                    Console.WriteLine(resultado + " registros actualizados.");

                MostrarDatosDeEmpleado();
                DeseaSalir();
            }
            catch (Exception e)
            {
                Console.WriteLine("No se ha podido insertar el empleado solicitado. Detalles: ");
                Console.WriteLine(e.Message);
            }
        }

        private static void ActualizarEmpleado()
        {

            try
            {
                Console.WriteLine("\nIntroduzca el nombre del empleado cuyo salario se desea actualizar: ");
                string nombreIntroducido = Console.ReadLine();
                Console.WriteLine("Introduzca el monto del nuevo salario: ");
                decimal salarioIntroducido = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Se actualizará el salario de " + nombreIntroducido + " a "+ salarioIntroducido);
                string sqlQuery = "UPDATE Empleado SET Salary = @salario WHERE Name = @nombre";
                mySqlCommand = new SqlCommand(sqlQuery, conexion);
                mySqlCommand.Parameters.AddWithValue("salario", salarioIntroducido);
                mySqlCommand.Parameters.AddWithValue("nombre", nombreIntroducido);
                int resultado = mySqlCommand.ExecuteNonQuery();

                if (resultado == 1)
                    Console.WriteLine(resultado + " registro actualizado.");
                else
                    Console.WriteLine(resultado + " registros actualizados.");

                MostrarDatosDeEmpleado();
                DeseaSalir();
            }
            catch (Exception e)
            {
                Console.WriteLine("No se ha podido actualizar al empleado solicitado. Detalles: ");
                Console.WriteLine(e.Message);
            }

        }

        private static void EliminarEmpleado()
        {
            try
            {
                Console.WriteLine("\nIngrese el nombre del empleado a eliminar: ");
                string empleadoEliminar = Console.ReadLine();
                string sqlQuery = "DELETE FROM Empleado WHERE Name = @nombre";

                mySqlCommand = new SqlCommand(sqlQuery, conexion);
                mySqlCommand.Parameters.AddWithValue("nombre", empleadoEliminar);
                int resultado = mySqlCommand.ExecuteNonQuery();

                if (resultado == 1)
                    Console.WriteLine(resultado + " registro actualizado.");
                else
                    Console.WriteLine(resultado + " registros actualizados.");

                MostrarDatosDeEmpleado();
                
            }
            catch (Exception e)
            {
                Console.WriteLine("No se ha podido eliminar el empleado solicitado. Detalles: ");
                Console.WriteLine(e.Message);
            }

        }
        private static void CerrarConexion ()
        {
            try
            {
                conexion.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Problema al cerrar la conexión. Detalles: ");
                Console.WriteLine(e.Message);
            }
        }
        private static void DeseaSalir()
        {
            Console.WriteLine("\nSi desea volver al menú presione 1 o presione cualquier otro botón para salir.");
            int numeroPresionado = Convert.ToInt32(Console.ReadLine());
            if (numeroPresionado == 1)
                IniciarPrograma();
            else
                CerrarPrograma();
        }
        private static void CerrarPrograma()
        {
            Environment.Exit(0);
        }

    }
}
