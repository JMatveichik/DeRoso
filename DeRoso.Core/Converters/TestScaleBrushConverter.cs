using System;
using System.Collections.Generic;


using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DeRoso.Core.Health;

namespace DeRoso.Core.Converters
{
    [ValueConversion(typeof(HealthTestResult), typeof(System.Windows.Media.Brush))]
    public class TestScaleBrushConverter : IMultiValueConverter
    {

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            
            HealthTestResult res = (HealthTestResult)value[0];

            if (res == null)
                return null;

            float val = (float)value[1];

            int size = res.Meassurments.Count;
            float percent = 0.0f;
            
            //однопрепаратный тест
            if (size == 1)
                percent = val / 100.0f;
            else
            {
                percent =  (size == 0) ? 0.0f : val / res.Meassurments.Count;
            }
                

            //Обратный расчет
            bool inverce = res.Test.IsReversedResult;

            System.Windows.Media.Color clr = GetScaleColor(percent, inverce);
            System.Windows.Media.Brush br = new System.Windows.Media.SolidColorBrush(clr);

            return br;
        }

        public object [] ConvertBack(object value, Type [] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private System.Windows.Media.Color GetScaleColor(float percent, bool inverce)
        {
            //Зелёный - 169, 208, 142
            //Жёлтый - 255, 217, 102
            //Красный - 236, 77, 60
            
            if (inverce)
            {
                //если обратный расчет
                //case [0-0.4]: цвет ячейки зелёный
                if (percent < 0.4f)
                    return System.Windows.Media.Color.FromArgb(255, 169, 208, 142);

                //case (0.4-0.7]: цвет ячейки жёлтый
                if (percent < 0.7f)
                    return System.Windows.Media.Color.FromArgb(255, 255, 217, 102);

                //case (0.7-1]: цвет ячейки красный
                return System.Windows.Media.Color.FromArgb(255, 236, 77, 60);
            }
            else
            {
                //прямой метод расчета
                
                //case (0.7-1]: цвет ячейки зелёный
                if (percent > 0.7f)
                    return System.Windows.Media.Color.FromArgb(255, 169, 208, 142);
                
                //case (0.4-0.7]: цвет ячейки жёлтый
                if (percent > 0.4)
                    return System.Windows.Media.Color.FromArgb(255, 255, 217, 102);

                //case [0-0.4]: цвет ячейки красный
                return System.Windows.Media.Color.FromArgb(255, 236, 77, 60);

             }
        }

       
    }
}


