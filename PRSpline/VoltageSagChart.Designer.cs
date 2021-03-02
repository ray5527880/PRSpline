namespace PRSpline
{
    partial class VoltageSagChart
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.PolygonAnnotation polygonAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.PolygonAnnotation();
            System.Windows.Forms.DataVisualization.Charting.AnnotationPathPoint annotationPathPoint1 = new System.Windows.Forms.DataVisualization.Charting.AnnotationPathPoint();
            System.Windows.Forms.DataVisualization.Charting.AnnotationPathPoint annotationPathPoint2 = new System.Windows.Forms.DataVisualization.Charting.AnnotationPathPoint();
            System.Windows.Forms.DataVisualization.Charting.AnnotationPathPoint annotationPathPoint3 = new System.Windows.Forms.DataVisualization.Charting.AnnotationPathPoint();
            System.Windows.Forms.DataVisualization.Charting.AnnotationPathPoint annotationPathPoint4 = new System.Windows.Forms.DataVisualization.Charting.AnnotationPathPoint();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0.05D, 0.8D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, 0.8D);
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0.05D, 0.5D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, 0.5D);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            polygonAnnotation1.AxisXName = "ChartArea1\\rX";
            polygonAnnotation1.BackColor = System.Drawing.Color.Aqua;
            annotationPathPoint1.PointType = ((byte)(0));
            annotationPathPoint1.X = 0.05F;
            annotationPathPoint2.X = 1F;
            annotationPathPoint3.X = 1F;
            annotationPathPoint3.Y = 0.8F;
            annotationPathPoint4.X = 0.05F;
            annotationPathPoint4.Y = 0.8F;
            polygonAnnotation1.GraphicsPathPoints.Add(annotationPathPoint1);
            polygonAnnotation1.GraphicsPathPoints.Add(annotationPathPoint2);
            polygonAnnotation1.GraphicsPathPoints.Add(annotationPathPoint3);
            polygonAnnotation1.GraphicsPathPoints.Add(annotationPathPoint4);
            polygonAnnotation1.Name = "PolygonAnnotation1";
            polygonAnnotation1.YAxisName = "ChartArea1\\rY";
            this.chart1.Annotations.Add(polygonAnnotation1);
            chartArea1.AlignWithChartArea = "ChartArea1";
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.Maximum = 10D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Interval = 0.1D;
            chartArea1.AxisY.Maximum = 1D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.DarkHorizontal;
            chartArea1.Name = "ChartArea2";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 45.5F;
            chartArea1.Position.Width = 50F;
            chartArea1.Position.X = 30.5F;
            chartArea1.Position.Y = 5F;
            chartArea2.BackColor = System.Drawing.Color.Transparent;
            chartArea2.BackImageTransparentColor = System.Drawing.Color.Turquoise;
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ChartAreas.Add(chartArea2);
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(19, 23);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea2";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.BackImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            series2.ChartArea = "ChartArea2";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            dataPoint1.BackSecondaryColor = System.Drawing.Color.Empty;
            series2.Points.Add(dataPoint1);
            series2.Points.Add(dataPoint2);
            series3.BackImageTransparentColor = System.Drawing.Color.Transparent;
            series3.ChartArea = "ChartArea2";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            series3.Legend = "Legend1";
            series3.Name = "Series3";
            series3.Points.Add(dataPoint3);
            series3.Points.Add(dataPoint4);
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1099, 684);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.SizeChanged += new System.EventHandler(this.chart1_SizeChanged);
            // 
            // VoltageSagChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add(this.chart1);
            this.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.Name = "VoltageSagChart";
            this.Size = new System.Drawing.Size(1142, 738);
            this.Load += new System.EventHandler(this.VoltageSagChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}
