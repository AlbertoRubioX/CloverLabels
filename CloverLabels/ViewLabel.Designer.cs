namespace CloverLabels
{
    partial class ViewLabel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.dtsDataFields = new CloverLabels.dtsDataFields();
            this.dtFieldsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtsDataFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFieldsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.dtFieldsBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CloverLabels.Etiquetas.rptEtiqueta1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(12, 12);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(689, 343);
            this.reportViewer1.TabIndex = 0;
            // 
            // dtsDataFields
            // 
            this.dtsDataFields.DataSetName = "dtsDataFields";
            this.dtsDataFields.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dtFieldsBindingSource
            // 
            this.dtFieldsBindingSource.DataMember = "dtFields";
            this.dtFieldsBindingSource.DataSource = this.dtsDataFields;
            // 
            // ViewLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 367);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ViewLabel";
            this.Text = "ViewLabel";
            this.Load += new System.EventHandler(this.ViewLabel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtsDataFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFieldsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource dtFieldsBindingSource;
        private dtsDataFields dtsDataFields;
    }
}