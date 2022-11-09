using SWDifCambiarioNeg.clases;

namespace SWDifCambiario
{
    public class cHandler
    {
        #region Initialize Objects, Classes

        /// <summary>
        /// Objeto que instancia a la clase cSerDifCambiario, encargada de procesar los métodos de negocio.
        /// </summary>
        private cSerDifCambiario c_SerDifCambiario;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Constructor de la clase: cHandler, encargado de inicializar los servicios a utilizar.
        /// </summary>
        /// <param name="args"></param>
        public cHandler(string[] args)
        {
            try
            {
                //Proyecto Local. Se utiliza para debuguear el proyecto, ya que los valores se obtienen desde el proyecto que hace el llamado al exe..
                //args = new string[2]; args[0] = "Monterroso~Administrador~1~1"; args[1] = "CON~01/31/2012";
                this.c_SerDifCambiario = new cSerDifCambiario(args);
                this.c_SerDifCambiario.pb_ProcesoDiferencialCambiario();
            }
            catch { throw; }
        }

        /// <summary>
        /// Constructor de la clase: cHandler, utilizado para eliminar de memoria todos los objetos creados.
        /// </summary>
        ~cHandler()
        {
            this.c_SerDifCambiario = null;
        }

        #endregion
    }
}