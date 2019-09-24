using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Drawing;
using System.Data;

namespace PRSpline
{
    sealed class WindowsCharting
    {
        //method generates the chart
        #pragma warning disable 0628
        protected internal Chart GenerateChart(DataTable dtChartDataSource, int width,int height,string bgColor,int intType,string strTitle )
        {
            Chart chart = new Chart()
            {
                Width = width,
                Height = height
            };

            chart.Titles.Add("Title1");
            chart.Titles["Title1"].Text = strTitle;
            chart.Titles["Title1"].Font = new System.Drawing.Font("Trebuchet MS", 12F, FontStyle.Bold);
            
            chart.Legends.Add(new Legend(){Name = "Legend"});
            
            //chart.Legends[0].Docking = Docking.Bottom;
            string sChartAreaName = "ChartArea";
            ChartArea chartArea = new ChartArea() { Name = sChartAreaName };
            //Remove X-axis grid lines
            chartArea.AxisX.MajorGrid.LineWidth = 0;
            //Remove Y-axis grid lines
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            
            //Chart Area Back Color
            chartArea.BackColor = Color.FromName(bgColor);
            chart.ChartAreas.Add(chartArea);
            chart.Palette = ChartColorPalette.BrightPastel;
            string series = string.Empty;
            //create series and add data points to the series
            if (dtChartDataSource != null)
            {
                string[] sXValue = new string[dtChartDataSource.Rows.Count];
                int nCount = 0;
                decimal decYMax = 0;
                decimal decSYMax = 0;
                foreach (DataColumn dc in dtChartDataSource.Columns)
                {             
                    //a series to the chart
                    if (chart.Series.FindByName(dc.ColumnName) == null)
                    {
                        if (dc.ColumnName != "时间" && dc.ColumnName != "日期" && dc.ColumnName != "时段" && dc.ColumnName != "月份" && dc.ColumnName != "电费")
                        {
                            nCount = 0;

                            series = dc.ColumnName;
                            chart.Series.Add(series);
                            chart.Series[series].ChartType = (SeriesChartType)intType;
                            chart.Series[series].XValueType = ChartValueType.String;


                            if (intType == 17)
                            {
                                //Add data points to the series
                                foreach (DataRow dr in dtChartDataSource.Rows)
                                {
                                    double dataPoint = 0;
                                    double.TryParse(dr[dc.ColumnName].ToString(), out dataPoint);
                                    chart.Series[series].Points.AddXY(sXValue[nCount].ToString(), dataPoint);
                                    nCount++;    
                                }
                                chart.Series[series]["PieLabelStyle"] = "Outside";
                                chart.Series[series].Label = "#VALX\n(#VALY)\n#PERCENT{P1}";
                            }

                            if (intType == 10 || intType == 3)
                            {
                                //Add data points to the series
                                foreach (DataRow dr in dtChartDataSource.Rows)
                                {
                                    if (nCount < (dtChartDataSource.Rows.Count - 1))
                                    {
                                        double dataPoint = 0;
                                        double.TryParse(dr[dc.ColumnName].ToString(), out dataPoint);
                                        chart.Series[series].Points.AddXY(sXValue[nCount].ToString(), dataPoint);
                                        if (intType == 3)
                                            chart.Series[series].BorderWidth = 2;
                                        if (Convert.ToDecimal(dataPoint) > decSYMax)
                                        {
                                            decSYMax = Convert.ToDecimal(dataPoint);
                                        }

                                        nCount++;
                                    }
                                }
                                chart.Series[series].LegendText = "#SERIESNAME：\n最大值(#MAX)\n最小值(#MIN)";

                                if (decSYMax > decYMax)
                                {
                                    decYMax = decSYMax;
                                }
                                                                
                            }

                        }
                        else
                        {
                          //  nCount = dtChartDataSource.Rows.Count;
                            nCount = 0;
                            //Add data points to the series
                            foreach (DataRow dr in dtChartDataSource.Rows)
                            {
                                if (intType == 17)
                                {
                                    sXValue[nCount] = dr[dc.ColumnName].ToString();
                                    nCount++;
                                }

                                if (intType == 10 || intType == 3)
                                {
                                    if (nCount < (dtChartDataSource.Rows.Count - 1))
                                    {
                                        sXValue[nCount] = dr[dc.ColumnName].ToString();
                                        nCount++;
                                    }
                                }
                            }
                        }
                    }
                }
                chart.DataSource = dtChartDataSource;
                chart.ChartAreas[sChartAreaName].AxisX.Interval = 1;

                if (intType == 10 || intType == 3)
                {
                    chart.ChartAreas[sChartAreaName].AxisY.Maximum = Convert.ToDouble(decYMax) * 1.3;
                }//设定Y軸最大值
            }
            return chart;
        }
    }
}
