using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfOffset.Models
{
    public class Steel
    {
        public static string currentDiametr = "8";
        public static Dictionary<int, float> diametrInTonas = new Dictionary<int, float>();
        
        static Steel()
        {
            diametrInTonas.Add(6, 0.222f);
            diametrInTonas.Add(8, 0.395f);
            diametrInTonas.Add(10, 0.617f);
            diametrInTonas.Add(12, 0.888f);
            diametrInTonas.Add(14, 1.21f);
            diametrInTonas.Add(16, 1.58f);
            diametrInTonas.Add(18, 2f);
            diametrInTonas.Add(20, 2.47f);
            diametrInTonas.Add(22, 2.98f);
            diametrInTonas.Add(25, 3.85f);
            diametrInTonas.Add(28, 4.83f);
        }

        public static double[] CalcTonas(int d, int l, int a) // diametr, length, amount
        {
            double w = diametrInTonas.Values.ToArray()[d];
            double kilogramCount = (l / 1000d) * w * a;//кг
            double tonasCount = kilogramCount / 1000d;//тонны

            double[] result = new double[2] { kilogramCount, tonasCount };
            return result;
        }
    }
}
