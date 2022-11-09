namespace SWDifCambiario
{
    class Program
    {
        #region Main

        /// <summary>
        /// Método principal, encargado de instanciar la librería necesaria para establecer conexión con el negocio del proyecto.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            cHandler c_Handler = new cHandler(args);
        }

        #endregion
    }
}