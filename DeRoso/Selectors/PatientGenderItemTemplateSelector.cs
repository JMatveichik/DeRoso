using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DeRoso.Selectors
{
   
    public class PatientGenderItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MalePatientDataTemplate { get; set; }
        public DataTemplate FemalePatientDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Patient patient = (Patient)item;
            if (patient.Gender == EnumPatientGender.Male)
                return MalePatientDataTemplate;

            return FemalePatientDataTemplate;
        }
    }
}
