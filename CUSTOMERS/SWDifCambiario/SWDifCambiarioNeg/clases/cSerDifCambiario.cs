using SW.DATOS.Entidad.Contabilidad;
using SW.ExceptionManage.General;
using System;
using System.Data;
using System.IO;

namespace SWDifCambiarioNeg.clases
{
    public class cSerDifCambiario
    {
        #region Initialize Constants

        /// <summary>
        /// Utilerias-Diferencial_Cambiario
        /// </summary>
        private const string v_SubModulo = "Utilerias-Diferencial_Cambiario";
        /// <summary>
        /// Negocio-cSerDifCambiario
        /// </summary>
        private const string v_Capa = "Negocio-cSerDifCambiario";
        /// <summary>
        /// Base de datos - SYSWEB_ACT
        /// </summary>
        private const string v_Catalogo = "SYSWEB_ACT";

        #endregion

        #region Initialize Objects, Classes

        /// <summary>
        /// Objeto que instancia a la clase cConContabilidad, que pertenece al proyecto: SWDATOS.
        /// </summary>
        private cConContabilidad c_ConContabilidad;
        /// <summary>
        /// Objeto tipo Estructura, utilizada para almacenar los valores obtenidos como argumento.
        /// </summary>
        private structCookie strCookie;
        /// <summary>
        /// Objeto tipo Estructura, utilizada para almacenar los valores obtenidos como argumento.
        /// </summary>
        private structData strData;
        /// <summary>
        /// Objeto tipo DataTable, utilizado para procesos del Mantenimiento de la Transacción.
        /// </summary>
        private DataTable v_DataTable;

        #endregion

        #region Initialize Variables

        /// <summary>
        /// Variable vector, utilizado para almacenar los valores enviados como argumento.
        /// </summary>
        private string[] v_Args;
        /// <summary>
        /// Variable tipo booleana, utilizada para validar el estado de los procesos del sistema.
        /// </summary>
        private bool v_Status = false;
        /// <summary>
        /// Variable tipo Entera, utilizada para validar procesos del sistema.
        /// </summary>
        private int v_Int = 0;

        #endregion

        #region Initialize Structures

        /// <summary>
        /// structCookie
        /// </summary>
        private struct structCookie
        {
            public int codEmpresa { get; set; }
            public int codUsuario { get; set; }
            public string empFolder { get; set; }
            public string cGEName { get; set; }

            /// <summary>
            /// Método encargado de limpiar las propiedades de la estructura.
            /// </summary>
            public void pb_CleanProperties()
            {
                this.codEmpresa = 0;
                this.codUsuario = 0;
                this.empFolder = string.Empty;
                this.cGEName = string.Empty;
            }
        }

        /// <summary>
        /// structData
        /// </summary>
        private struct structData
        {
            public string tipProceso { get; set; }
            public DateTime fechaMes { get; set; }
            public int accion { get; set; }
            public string concepto { get; set; }

            /// <summary>
            /// Método encargado de limpiar las propiedades de la estructura.
            /// </summary>
            public void pb_CleanProperties()
            {
                this.tipProceso = string.Empty;
                this.fechaMes = DateTime.Now;
                this.accion = 0;
                this.concepto = string.Empty;
            }
        }

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Constructor de la clase: cSerDifCambiario, encargado de inicializar los procesos de negocio.
        /// </summary>
        /// <param name="args"></param>
        public cSerDifCambiario(string[] p_Args)
        {
            //this.pv_LogProcess("Inició el Constructor. " + DateTime.Now.ToString());
            this.v_Args = p_Args;
            this.pv_ValidateArguments();
            //this.pv_LogProcess("Finalizó el constructor.");
        }

        /// <summary>
        /// Constructor de la clase: cSerDifCambiario, utilizado para eliminar de memoria todos los objetos creados.
        /// </summary>
        ~cSerDifCambiario()
        {
            //this.pv_LogProcess("Inició el Destructor. " + DateTime.Now.ToString());
            this.c_ConContabilidad = null;
            this.v_DataTable = null;
            this.v_Args = null;
        }

        #endregion

        #region Private Methods

        #region Validations

        /// <summary>
        /// Método encargado de validar que un DataTable contenga registros, de ser así, retorna true.
        /// </summary>
        /// <param name="p_Table"></param>
        /// <param name="msj"></param>
        /// <returns></returns>
        private bool pv_ValidateDataTable(DataTable p_Table)
        {
            try
            {
                if (p_Table != null) { if (p_Table.Rows.Count > 0) { return true; } }
                return false;
            }
            catch (Exception ex) { this.pv_ThrowNewException("pv_ValidateDataTable", ex); return false; }
        }

        /// <summary>
        /// Método encargado de validar que la variable tipo string no se encuentre vacía.
        /// </summary>
        /// <param name="p_txt"></param>
        /// <returns></returns>
        private bool pv_ValidateString(string p_txt)
        {
            try
            {
                if (string.IsNullOrEmpty(p_txt)) { return true; }
                return false;
            }
            catch (Exception ex) { this.pv_ThrowNewException("pv_ValidateString", ex); return false; }
        }

        /// <summary>
        /// Método encargado de validar que el vector obtenido contenga el formato correcto.
        /// </summary>
        /// <returns></returns>
        private bool pv_ValidateArguments()
        {
            try
            {
                //this.pv_LogProcess("Inició el proceso de validar los Argumentos.");

                if (this.v_Args != null)
                {
                    if (this.v_Args.Length == 2)
                    {
                        string[] v_VectCookie = this.v_Args[0].Split('~');
                        string[] v_VectData = this.v_Args[1].Split('~');

                        if (v_VectCookie.Length == 4)
                        {
                            for (int i = 0; i < v_VectCookie.Length; i++)
                            {
                                if (this.pv_ValidateString(v_VectCookie[i]))
                                {
                                    //this.pv_LogProcess("Validación incorrecta de Cookie."); 
                                    return false;
                                }
                            }
                        }

                        if (v_VectData.Length == 2)
                        {
                            for (int i = 0; i < v_VectData.Length; i++) 
                            { 
                                if (this.pv_ValidateString(v_VectData[i])) 
                                { 
                                    //this.pv_LogProcess("Validación incorrecta de Data.");
                                    return false; 
                                } 
                            }
                        }

                        if (this.pv_SetCookieFromArguments(v_VectCookie))
                        {
                            if (this.pv_SetDataFromArguments(v_VectData))
                            {
                                //this.pv_LogProcess("Finalizó el proceso de validar los Argumentos.");
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex) { this.pv_ThrowNewException("pv_ValidateArguments", ex); return false; }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Método encargado de inicializar la clase que genera el registro en bitácora de algún error en los procesos del sistema.
        /// </summary>
        /// <param name="p_MethodName">Nombre del método que está generando el llamado.</param>
        /// <param name="p_Exception">Excepcion generada desde el método.</param>
        private bool pv_ThrowNewException(string p_MethodName, Exception p_Exception)
        {
            if (!p_MethodName.Equals("pb_LogProcess")) 
            { 
                //this.pv_LogProcess("Se generó una excepción. Método: " + p_MethodName + ", Exception: " + p_Exception.Message); 
            }
            try { throw new cExceptionManage(this.strCookie.empFolder, this.strCookie.codEmpresa.ToString(), this.strCookie.codUsuario.ToString(), v_SubModulo, v_Capa, p_MethodName, p_Exception); }
            catch { return false; }
        }

        /// <summary>
        /// Método encargado de obtener la ruta de la carpeta bin.
        /// </summary>
        /// <returns></returns>
        private string pv_GetPathBin()
        {
            try
            {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("\\bin", "").Replace("file:\\", ""); //PROYECTO PUBLICADO.
                //return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("\\Release", "").Replace("file:\\", ""); //PROYECTO LOCAL.
            }
            catch (Exception ex) { this.pv_ThrowNewException("pv_GetPathBin", ex); return string.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_Text"></param>
        private void pv_LogProcess(string p_Text)
        {
            try
            {
                string v_File = "DIF_Cambiario_LOG.txt";
                string v_Folder = "DIF_Cambiario";
                string v_FullPath = this.pv_GetPathBin() + "\\SWDefault\\rec\\emp_0\\bit\\" + v_Folder + "\\";

                StreamWriter v_WriteLogFile = File.AppendText(v_FullPath + v_File);
                v_WriteLogFile.WriteLine(string.Empty);
                v_WriteLogFile.WriteLine(p_Text);
                v_WriteLogFile.WriteLine(string.Empty);
                v_WriteLogFile.Close();
            }
            catch (Exception ex) { this.pv_ThrowNewException("pv_LogProcess", ex); }
        }

        #endregion

        #region Set Data

        /// <summary>
        /// Método encargado de asignar valor a la estructura en base a los parámetros obtenidos.
        /// </summary>
        /// <param name="p_VectCookie"></param>
        private bool pv_SetCookieFromArguments(string[] p_VectCookie)
        {
            try
            {
                this.strCookie = new structCookie();
                this.strCookie.pb_CleanProperties();
                this.strCookie.cGEName = p_VectCookie[0];
                this.strCookie.empFolder = p_VectCookie[1];
                this.strCookie.codEmpresa = int.Parse(p_VectCookie[2]);
                this.strCookie.codUsuario = int.Parse(p_VectCookie[3]);

                //this.pv_LogProcess("Se asignó valor a la estructura cookie:");
                //this.pv_LogProcess("    cGEName = " + this.strCookie.cGEName);
                //this.pv_LogProcess("    empFolder = " + this.strCookie.empFolder);
                //this.pv_LogProcess("    codEmpresa = " + this.strCookie.codEmpresa);
                //this.pv_LogProcess("    codUsuario = " + this.strCookie.codUsuario);
                return true;
            }
            catch (Exception ex) { this.pv_ThrowNewException("pv_SetCookieFromArguments", ex); return false; }
        }

        /// <summary>
        /// Método encargado de asignar valor a la estructura en base a los parámetros obtenidos.
        /// </summary>
        /// <param name="p_VectCookie"></param>
        private bool pv_SetDataFromArguments(string[] p_VectData)
        {
            try
            {
                this.strData = new structData();
                this.strData.pb_CleanProperties();
                this.strData.tipProceso = p_VectData[0];
                this.strData.fechaMes = DateTime.Parse(p_VectData[1], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                
                //this.pv_LogProcess("Se asignó valor a la estructura data:");
                //this.pv_LogProcess("    tipProceso = " + this.strData.tipProceso);
                //this.pv_LogProcess("    fechaMes = " + this.strData.fechaMes);
                //this.pv_LogProcess("    accion = " + this.strData.accion);
                //this.pv_LogProcess("    concepto = " + this.strData.concepto);
                return true;
            }
            catch (Exception ex) { this.pv_ThrowNewException("pv_SetDataFromArguments", ex); return false; }
        }

        #endregion

        #endregion

        #region Public Methods

        #region Maintenance Methods

        public void pb_ProcesoDiferencialCambiario()
        {
            try
            {
                //System.Threading.Thread.Sleep(20000);
                bool? v_Status = false; string v_Msj = string.Empty;
                this.c_ConContabilidad = new cConContabilidad(this.strCookie.codUsuario, this.strCookie.cGEName, v_Catalogo, this.strCookie.empFolder, this.strCookie.codEmpresa.ToString());
                this.c_ConContabilidad.pb_InsDiferencialCambiario_Genera(this.strCookie.codEmpresa, this.strData.tipProceso, this.strData.fechaMes, ref v_Status, ref v_Msj);
                //this.pv_LogProcess("Se generó el llamado al proceso de Diferencial Cambiario.");
                if (!v_Status.Value) { this.pv_UpdateEstadoDifCambiario(); }
            }
            catch (Exception ex) { this.pv_UpdateEstadoDifCambiario(); this.pv_ThrowNewException("pb_ProcesoDiferencialCambiario", ex); }
        }

        /// <summary>
        /// Método encargado de Actualizar el Estado del Proceso en Finalizado.
        /// </summary>
        private void pv_UpdateEstadoDifCambiario()
        {
            try
            {
                bool? v_Status = false; string v_Msj = string.Empty;
                this.c_ConContabilidad.pb_UpdDiferencialCambiario_Estado(this.strCookie.codEmpresa, this.strData.tipProceso, this.strData.fechaMes, false, ref v_Status, ref v_Msj);
            }
            catch (Exception ex) { this.pv_ThrowNewException("pv_UpdateEstadoDifCambiario", ex); }
        }

        #endregion

        #endregion
    }
}