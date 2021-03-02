using System;
using System.Collections.Generic;
using System.Text;
using GSF.COMTRADE;
using System.IO;
using BF_FW.data;
using FFTWSharp;
using System.Numerics;


namespace BF_FW
{
    public class FFTCal
    {
        private Parser mParser;
        private int[] fftIndex;
        
        public FFTCal(int[] FFTIndex,Parser parser)
        {
            mParser = parser;
            fftIndex = FFTIndex;
        }
        
        public FFTData GetFFTData(List<double[]> data)
        {
            FFTData mFFTData = new FFTData();
            var fftdata = new List<FFTData.fftData>();
            for (int i = 0; i < mParser.Schema.SampleRates[0].EndSample; i++)
            {
                FFTData.fftData _data = new FFTData.fftData();
                _data.Value = new double[fftIndex.Length];
                _data.rad = new double[fftIndex.Length];
                for (int ii = 0; ii < fftIndex.Length; ii++)
                {
                    Complex mComplex = new Complex();

                    mComplex = FFTW(i, ii, data);

                    _data.Value[ii] = Math.Sqrt(Math.Pow(mComplex.Real, 2) + Math.Pow(mComplex.Imaginary, 2)) / (mParser.Schema.SampleRates[0].Rate / mParser.Schema.NominalFrequency / 2) / Math.Sqrt(2);
                    _data.rad[ii] = Math.Atan2(mComplex.Imaginary, mComplex.Real);
                }
                fftdata.Add(_data);
            }
            mFFTData.arrFFTData = fftdata.ToArray();

            return mFFTData;
        }

        private Complex FFTW(int index_Entry, int index_Value, List<double[]> datData)
        {
            int SIZE = Convert.ToInt32(mParser.Schema.SampleRates[0].Rate / mParser.Schema.NominalFrequency);

            double[] data = new double[SIZE];

            Complex[] cdata = new Complex[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                if ((index_Entry < SIZE / 2 && (i < SIZE / 2 - index_Entry)) || (i + index_Entry - (SIZE / 2 - 1)) > mParser.Schema.SampleRates[0].EndSample)
                {
                    cdata[i] = new Complex(0, 0);
                }
                else
                {
                    cdata[i] = new Complex(datData[i + index_Entry - SIZE / 2][fftIndex[index_Value] + 2], 0);
                }
            }
            fftw_complexarray input = new fftw_complexarray(SIZE);
            fftw_complexarray ReData = new fftw_complexarray(SIZE);

            input.SetData(cdata);

            fftw_plan pf = fftw_plan.dft_1d(SIZE, input, ReData, fftw_direction.Forward, fftw_flags.Estimate);

            pf.Execute();
            var data_Complex = ReData.GetData_Complex();

            return data_Complex[1];
        }
    }
}
