using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Spire.Barcode.Forms;
using Spire.Barcode;
using System.IO;
using Logica;
using System.Security.Cryptography;
namespace CloverLabels
{
    public partial class Form1 : Form
    {
        string _lsUser;
        string _lsArea;
        string _lsPrinter;
        int _liTimeLapse;
        string _lsPath;
        string _lsRecPath;
        string _lsArtPath;
        string _lsCodBar2;
        public Form1()
        {
            InitializeComponent();
        }


       
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                int iP = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count;
                if (iP == 0)
                    Close();

                tssVersion.Text = "1.0.1.7";

                DataTable dtC = ConfigLogica.Consutlar();
                _lsPrinter = dtC.Rows[0]["printer_name"].ToString();
                _lsPath = dtC.Rows[0]["sys_direct"].ToString();
                _lsRecPath = dtC.Rows[0]["report_rec"].ToString();
                _lsArtPath = dtC.Rows[0]["report_rec2"].ToString();
                _lsCodBar2 = dtC.Rows[0]["gen_codbar2"].ToString();
                _liTimeLapse = int.Parse(dtC.Rows[0]["time_lap"].ToString());

                bool bPrinter = false;
                for (int i = 0; i < iP; i ++)
                {
                    string sPrint = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
                    if (sPrint.ToUpper() == _lsPrinter)
                    {
                        _lsPrinter = sPrint;
                        bPrinter = true;
                        break;
                    }
                }

                if (!bPrinter)
                    Close();

                _lsUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                if (_lsUser.IndexOf("\\") != -1)
                {
                    int iIdx = _lsUser.IndexOf("\\");
                    int iLen = _lsUser.Length;
                    iLen = iLen - iIdx;
                    _lsUser = _lsUser.Substring(iIdx + 1, iLen - 1);
                }

                UsuarioLogica user = new UsuarioLogica();
                user.Usuario = _lsUser;

                _lsArea = UsuarioLogica.GetArea(user);
                if (!string.IsNullOrEmpty(_lsArea))
                    Inicio();
                else
                    Close();
            }
            catch(FileNotFoundException ex)
            {
                this.Close();
            }catch(TypeInitializationException ex){
                this.Close();
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        private void Inicio()
        {
            if(_lsArea == "REC")
            {
                BuscarRecibo();
                timer1.Interval = _liTimeLapse;
                timer1.Start();
            }
            else
                BuscarArticulo();
            
        }
        private void BuscarArticulo()
        {
            try
            {
                this.Height = 385;
                lblInst.Text = "Presione ENTER en Cantidad para Imprimir";
                panel2.Visible = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
        private void BuscarRecibo()
        {
            try
            {
                this.Height = 192;
                ReciboLogica rec = new ReciboLogica();
                rec.Usuario = _lsUser;
                if (!ReciboLogica.ConsultaPendientesRec(rec))
                    return;

                DataTable datos = ReciboLogica.ListarPendientesRec(rec);
                for (int x = 0; x < datos.Rows.Count; x++)
                {
                    string sRecibo = datos.Rows[x][0].ToString();
                    int iCons = Convert.ToInt32(datos.Rows[x][1].ToString());
                    if (ImprimeEtiqueta(sRecibo, iCons))
                    {
                        rec.Recibo = sRecibo;
                        rec.Consec = iCons;
                        ReciboLogica.ReciboPrint(rec);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        private bool GeneraEtiqueta(string _asRecibo, string _asOrden, string _asFecha, string _asArt, string _asDescrip, double _adCant, string _asUM, string _asUsuario)
        {
            bool bPrint = false;
            try
            {
                
                dtsDataFields ds = new dtsDataFields();
                DataTable t = ds.Tables.Add("dtFields");
                t.Columns.Add("clave", Type.GetType("System.Byte[]"));
                t.Columns.Add("clave2", Type.GetType("System.Byte[]"));
                t.Columns.Add("clave3", Type.GetType("System.Byte[]"));
                t.Columns.Add("DataColumn1", Type.GetType("System.String"));
                t.Columns.Add("DataColumn2", Type.GetType("System.String"));
                t.Columns.Add("DataColumn3", Type.GetType("System.String"));
                t.Columns.Add("DataColumn4", Type.GetType("System.String"));
                t.Columns.Add("DataColumn5", Type.GetType("System.String"));
                t.Columns.Add("DataColumn6", Type.GetType("System.String"));
                t.Columns.Add("DataColumn7", Type.GetType("System.String"));
                t.Columns.Add("DataColumn8", Type.GetType("System.String"));
                DataRow r;

                r = t.NewRow();
               
                r["DataColumn1"] = _asRecibo;
                r["DataColumn2"] = _asOrden;
                r["DataColumn3"] = _asFecha;
                r["DataColumn4"] = _asArt.ToUpper();
                r["DataColumn5"] = _asDescrip;
                r["DataColumn6"] = _adCant.ToString("N0");
                r["DataColumn7"] = _asUM.ToString();
                r["DataColumn8"] = _asUsuario.ToString();

                BarCodeControl cont = new BarCodeControl();
                cont.Type = BarCodeType.Code128;
                cont.Height = 50;
                cont.ShowText =false;
                cont.ShowCheckSumChar = false;
                cont.Code128SetMode = Code128SetMode.Auto;
                cont.Data = _asRecibo;
                cont.ShowTopText = false;
                BarCodeGenerator bar = new BarCodeGenerator(cont);
                Image im = bar.GenerateImage();
                r["clave"] = imageToByteArray(im);

                cont.Data = _asOrden.ToString();
                Image im2 = bar.GenerateImage();
                r["clave2"] = imageToByteArray(im2);

                cont.Data = _asArt.ToString();
                Image im3 = bar.GenerateImage();
                r["clave3"] = imageToByteArray(im3);

                t.Rows.Add(r);

                ReportDocument rptAgenda = new ReportDocument();
                //throw new TypeInitializationException("Error",new Exception());
                //throw new FileNotFoundException("Error");


                string sDir = _lsPath;
                string sArchivo = sDir + @"\" + _lsRecPath;
                rptAgenda.Load(sArchivo);
                rptAgenda.SetDataSource(t);

                //PrintDialog dialog = new PrintDialog();
                //DialogResult result = dialog.ShowDialog();
                //if (result == DialogResult.OK)
                //{
                rptAgenda.PrintOptions.PrinterName = _lsPrinter;
                //rptAgenda.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                rptAgenda.PrintOptions.PaperSource = PaperSource.Auto;
                rptAgenda.PrintToPrinter(1, true,1,1);
                Cursor = Cursors.Arrow;
                bPrint = true;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Favor de notificar al administrador del sistema, se cerrará el programa: " + ex, "Error al cargar",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.Close();
            }
            catch (TypeInitializationException ex)
            {
                MessageBox.Show("Favor de notificar al administrador del sistema, se cerrará el programa: " + ex, "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bPrint = false;
            }
            return bPrint;
        }
        private bool GeneraEtiArticulo()
        {
            bool bPrint = false;
            try
            {

                int iCopy = 1;
                PupOp pCant = new PupOp();
                pCant.ShowDialog();
                iCopy = pCant._iCant;

                if (iCopy < 1)
                    return false;

                dtsDataFields ds = new dtsDataFields();
                DataTable t = ds.Tables.Add("dtFields");
                t.Columns.Add("clave3", Type.GetType("System.Byte[]"));
                t.Columns.Add("DataColumn4", Type.GetType("System.String"));
                t.Columns.Add("DataColumn5", Type.GetType("System.String"));
                t.Columns.Add("DataColumn6", Type.GetType("System.String"));
                t.Columns.Add("DataColumn7", Type.GetType("System.String"));

                DataRow r;

                r = t.NewRow();

                r["DataColumn4"] = cbbbArticulo.Text.ToString().ToUpper();
                r["DataColumn5"] = txtDesc.Text.ToString().ToUpper();
                r["DataColumn6"] = Double.Parse(txtCant.Text).ToString("N0");
                r["DataColumn7"] = txtUM.Text.ToString();

                if (_lsCodBar2 == "1")
                {
                    BarCodeControl cont = new BarCodeControl();
                    cont.Type = BarCodeType.Code128;
                    cont.Height = 80;
                    cont.ShowText = false;
                    cont.ShowCheckSumChar = false;
                    cont.Code128SetMode = Code128SetMode.Auto;
                    cont.Data = cbbbArticulo.Text.ToString().ToUpper();
                    cont.ShowTopText = false;
                    BarCodeGenerator bar = new BarCodeGenerator(cont);
                    Image im = bar.GenerateImage();
                    r["clave3"] = imageToByteArray(im);
                }
                t.Rows.Add(r);


                ReportDocument rptAgenda = new ReportDocument();

                string sArchivo = _lsPath + @"\" + _lsArtPath;
                rptAgenda.Load(sArchivo);
                rptAgenda.SetDataSource(t);
                rptAgenda.PrintOptions.PrinterName = _lsPrinter;
                //rptAgenda.PrintOptions.PaperOrientation= PaperOrientation.Landscape;
                rptAgenda.PrintToPrinter(iCopy, true, 0, 0);
                Cursor = Cursors.Arrow;
                bPrint = true;
                
                
                
                
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Favor de notificar al administrador del sistema, se cerrará el programa: " + ex, "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
            catch (TypeInitializationException ex)
            {
                MessageBox.Show("Favor de notificar al administrador del sistema, se cerrará el programa: " + ex, "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bPrint = false;
            }
            return bPrint;
        }
        private bool ImprimeEtiqueta(string _asRecibo,int _aiConsec)
        {
            this.Cursor = Cursors.WaitCursor;
            bool bPrint = false;
            try
            {
                ReciboLogica rec = new ReciboLogica();
                rec.Recibo = _asRecibo;
                rec.Consec = _aiConsec;

                DataTable datos = ReciboLogica.ConsultaReciboMov(rec);
                if(datos.Rows.Count > 0)
                {
                    string sFecha = datos.Rows[0]["f_carga"].ToString();
                    string sOrden = datos.Rows[0]["orden"].ToString();
                    string sArticulo = datos.Rows[0]["no_art"].ToString();
                    string sDescrip = datos.Rows[0]["desc_art"].ToString();
                    double dCant = Double.Parse(datos.Rows[0]["cantidad"].ToString());
                    string sUM = datos.Rows[0]["um"].ToString();
                    string sUsuario = datos.Rows[0]["recibio"].ToString();

                    bPrint = GeneraEtiqueta(_asRecibo, sOrden, sFecha, sArticulo, sDescrip, dCant, sUM, sUsuario);

                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bPrint = false;
                Close();
            }

            this.Cursor = Cursors.Arrow;
            return bPrint;
        }
        private bool ImprimeEtiquetaInnova(string _asRecibo, int _aiConsec)
        {
            this.Cursor = Cursors.WaitCursor;
            bool bPrint = false;
            try
            {
                InnovaLogica rec = new InnovaLogica();
                rec.Recibo = _asRecibo;
                rec.Consec = _aiConsec;

                DataTable datos = InnovaLogica.ConsultaReciboMov(rec);
                if (datos.Rows.Count > 0)
                {
                    string sFecha = datos.Rows[0]["fecha"].ToString();
                    string sOrden = datos.Rows[0]["orden"].ToString();
                    string sArticulo = datos.Rows[0]["no_art"].ToString();
                    string sDescrip = datos.Rows[0]["desc_art"].ToString();
                    double dCant = Double.Parse(datos.Rows[0]["cantidad"].ToString());
                    string sUM = datos.Rows[0]["um"].ToString();
                    string sAlmacen = datos.Rows[0]["almacen"].ToString();
                    string sUsuario = datos.Rows[0]["recibio"].ToString();

                    bPrint = GeneraEtiqueta(_asRecibo, sOrden, sFecha, sArticulo, sDescrip, dCant, sUM, sUsuario);

                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bPrint = false;
                Close();
            }

            this.Cursor = Cursors.Arrow;
            return bPrint;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            BuscarRecibo();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void txtRecibo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtRecibo.Text) || string.IsNullOrWhiteSpace(txtRecibo.Text))
                return;
            
            string sCve = txtRecibo.Text.ToString().Trim();
            string sTipo = string.Empty;
            if (sCve.IndexOf("REC") != -1)
                sTipo = "R";
            if (sCve.IndexOf("ORD") != -1)
                sTipo = "O";

            
            try
            {
                ReciboLogica rec = new ReciboLogica();
                rec.Recibo = sCve;
                DataTable dt = new DataTable();
                if (sTipo == "R")
                    dt = ReciboLogica.ConsultaRecibo(rec);
                else
                {
                    rec.Orden = sCve;
                    dt = ReciboLogica.ConsultaOrden(rec);
                }
                    

                if(dt.Rows.Count > 0)
                {
                    cbbbArticulo.DataSource = dt;
                    cbbbArticulo.ValueMember = "no_art";
                    cbbbArticulo.DisplayMember = "no_art";
                    cbbbArticulo.SelectedIndex = -1;
                    
                }
                else
                {
                    //buscar dentro de Innova
                    InnovaLogica inn = new InnovaLogica();
                    inn.Recibo = sCve;
                    if (sTipo == "R")
                        dt = InnovaLogica.ConsultaRecibo(inn);
                    else
                    {
                        inn.Orden = sCve;
                        dt = InnovaLogica.ConsultaOrden(inn);
                    }


                    if (dt.Rows.Count > 0)
                    {
                        cbbbArticulo.DataSource = dt;
                        cbbbArticulo.ValueMember = "no_art";
                        cbbbArticulo.DisplayMember = "no_art";
                        cbbbArticulo.SelectedIndex = -1;

                    }
                    else
                    {
                        MessageBox.Show("No se encontro el documento", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtRecibo.Clear();
                        txtRecibo.Focus();
                        return;
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtRecibo.Clear();
                txtRecibo.Focus();
                return;
            }


            cbbbArticulo.Focus();
        }

        private void cbbbArticulo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (_lsArea == "REC")
            {
                if (string.IsNullOrEmpty(txtRecibo.Text))
                    return;
            }
                

            if (string.IsNullOrEmpty(cbbbArticulo.Text))
                return;

            try
            {
                string sCve = txtRecibo.Text.ToString();
                string sTipo = string.Empty;
                if (sCve.IndexOf("REC") != -1)
                    sTipo = "R";
                if (sCve.IndexOf("ORD") != -1)
                    sTipo = "O";

                if (_lsArea == "REC" || _lsArea == "IT")
                {
                #region regRecibo
                    ReciboLogica rec = new ReciboLogica();
                    rec.Articulo = cbbbArticulo.Text.ToString().ToUpper().Trim();
                    DataTable dt = new DataTable();
                    if (sTipo == "R")
                    {
                        rec.Recibo = sCve;
                        dt = ReciboLogica.ConsultaReciboArt(rec);
                    }
                    else
                    {
                        rec.Orden = sCve;
                        dt = ReciboLogica.ConsultaOrdenArt(rec);

                    }

                    if (dt.Rows.Count > 0)
                    {
                        int iCons = Int32.Parse(dt.Rows[0]["consec"].ToString());
                        if (sTipo == "O")
                            sCve = dt.Rows[0]["recibo"].ToString();

                        rec.Recibo = sCve;
                        rec.Consec = iCons;

                        if (ImprimeEtiqueta(sCve, iCons))
                            ReciboLogica.ReciboPrint(rec);
                    }
                    else
                    {
                        //buscar dentro de Innova
                        InnovaLogica irec = new InnovaLogica();
                        irec.Articulo = rec.Articulo;

                        if (sTipo == "R")
                        {
                            irec.Recibo = sCve;
                            dt = InnovaLogica.ConsultaReciboArt(irec);
                        }
                        else
                        {
                            irec.Orden = sCve;
                            dt = InnovaLogica.ConsultaOrdenArt(irec);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            int iCons = Int32.Parse(dt.Rows[0]["csc_mov"].ToString());
                            if (sTipo == "O")
                                sCve = dt.Rows[0]["no_recibo"].ToString();

                            rec.Recibo = sCve;
                            rec.Consec = iCons;
                            ImprimeEtiquetaInnova(sCve, iCons);
                        }
                        else
                        {
                            MessageBox.Show("No se encontro el Artículo", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cbbbArticulo.ResetText();
                            return;
                        }
                    }
                #endregion
                }
                else
                {
                #region regArticulo
                    InnovaLogica irec = new InnovaLogica();
                    irec.Articulo = cbbbArticulo.Text.ToString().ToUpper().Trim();

                    DataTable dtA = InnovaLogica.ConsultaArticulo(irec);
                    if (dtA.Rows.Count > 0)
                    {
                        string sArtNombre = dtA.Rows[0]["desc_esp"].ToString();
                        string sUM = dtA.Rows[0]["um"].ToString();

                        txtDesc.Text = sArtNombre;
                        txtUM.Text = sUM;
                        txtCant.Focus();
                    }
                #endregion
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
        }

        private void txtCant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            
            if (string.IsNullOrEmpty(cbbbArticulo.Text))
                return;

            if (string.IsNullOrEmpty(txtDesc.Text.ToString()))
                return;
            if (string.IsNullOrEmpty(txtCant.Text.ToString()))
                return;
            if (string.IsNullOrEmpty(txtUM.Text.ToString()))
                return;

            try
            {

                GeneraEtiArticulo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void cbbbArticulo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbbArticulo.Text))
                return;

            try
            {
                if (_lsArea == "SUM" || _lsArea == "IT")
                {
                    InnovaLogica irec = new InnovaLogica();
                    irec.Articulo = cbbbArticulo.Text.ToString().ToUpper().Trim();

                    DataTable dtA = InnovaLogica.ConsultaArticulo(irec);
                    if (dtA.Rows.Count > 0)
                    {
                        string sArtNombre = dtA.Rows[0]["desc_esp"].ToString();
                        string sUM = dtA.Rows[0]["um"].ToString();

                        txtDesc.Text = sArtNombre;
                        txtUM.Text = sUM;
                        txtCant.Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error." + Environment.NewLine + ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

       
    }
}
