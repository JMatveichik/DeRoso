using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using DeRoso.Core;
using DeRoso.Core.Data;
using DeRoso.Core.Health;
using DeRoso.Views;

namespace DeRoso
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ContentManager.Instance.RegisterCreator("HOME", new CreatorViewHome());
            ContentManager.Instance.RegisterCreator("PROGRAMMS", new CreatorViewProgramms());
            ContentManager.Instance.RegisterCreator("HELP", new CreatorViewHelp());
            ContentManager.Instance.RegisterCreator("ARCHIVE", new CreatorViewArchive());
            ContentManager.Instance.RegisterCreator("TESTING", new CreatorViewTesting());


            DeRosoContext db = new DeRosoContext();
           
            
             Programs.Load("db.xml");

             List<DeviceProgram> all = Programs.Programs;

             var sections = all.Select(p => p.SectionTitle)
                             .Distinct()
                             .ToList();

             var groups = all.Select(p => p.SubsectionTitle)
                             .Distinct()
                             .ToList();


             int id = 10;
             foreach (string s in sections)
             {
                 db.Sections.Add(new HealthTestSection()
                 {
                     Id = id++,
                     Title = s,
                     Description = "Краткое описание раздела"
                 });                
             }

             db.SaveChanges();

             id = 100;
             foreach (string gn in groups)
             {
                 var pr = all.Select(p => p).Where(p => p.SubsectionTitle == gn).FirstOrDefault().SectionTitle;
                 int secId = db.Sections.Select(s => s).Where(s => s.Title == pr).FirstOrDefault().Id;

                 db.Groups.Add(new HealthTestGroup()
                 {
                     HealthTestSectionId = secId,
                     Id = id++,
                     Title = gn,
                     Description = "Краткое описание группы"
                 });                
             }

             db.SaveChanges();




             id = 1000;
             foreach (DeviceProgram p in all)
             {
                 int secId = db.Sections.Select(s => s).Where(s => s.Title == p.SectionTitle).FirstOrDefault().Id;
                 int grId = db.Groups.Select(g => g).Where(g => g.Title == p.SubsectionTitle).FirstOrDefault().Id;

                 db.Tests.Add(new HealthTest()
                 {
                     Id = id++,
                     //HealthTestSectionId = secId,
                     HealthTestGroupId = grId,
                     Title = p.Title,
                     Description = "Краткое описание теста",
                     PauseBeforeHV = p.PauseBeforeHV,
                     FrequencyHV = p.FrequencyHV,
                     UseHV = p.UseHV,
                     HighLimit = 100,
                     LowLimit = 0
                 });
             }

             db.SaveChanges();


             ///-----------------------------------------------------------------------------------
             int cell = 0;

             string[] nameDrugs = File.ReadAllLines("ProfLeng.txt", Encoding.Default);
             string[] prog_txt = File.ReadAllLines("tmpTxt.txt", Encoding.Default);

             int lastAddress = 0;
             List<string> outStrings = new List<string>();


             for (int i = 0; i < prog_txt.Length; i++)
             {
                 string [] Element = prog_txt[i].Split('\t');
                 if (Element.Length < 3)
                     continue;

                 int address = int.Parse(Element[0]);

                 if (address != lastAddress)
                 {
                     lastAddress = address;
                     cell = 1;
                 }
                 else
                 {
                     cell++;
                 }                

                 var drug = new { Name = Element[1], Cell = cell, Address = address,  Description = "Краткое описание препарата : " + Element[1] };
                 outStrings.Add (string.Format ("{0}\t{1}\t{2}\t{3}", drug.Address, drug.Cell, drug.Name, drug.Description));

                 db.Drugs.Add(new HealthTestDrug()
                 {
                     HealthTestId = null,
                     Id = i + 10000,
                     Title = drug.Name,
                     Address = drug.Address,
                     Cell = drug.Cell,
                     Description = drug.Description
                 }

                 );
                 //Debug.WriteLine(outStrings.Last());
             }

             File.WriteAllLines("out.txt", outStrings);



             db.SaveChanges();

             Random rnd = new Random(DateTime.Now.Millisecond);
             int maxID = 10000 + db.Drugs.Count() - 1;

             foreach (HealthTestSection sec in db.Sections)
             {

                 foreach (HealthTestGroup gr in sec.Groups)
                 {

                     foreach (HealthTest t in gr.Tests)
                     {


                         if (t.Drugs == null)
                             t.Drugs = new List<HealthTestDrug>();
                         else
                             t.Drugs.Clear();

                         int cnt = rnd.Next(1, 4);

                         for (int i = 0; i < cnt; i++ )
                         {                           

                             int di = rnd.Next(10000, maxID);
                             var drug = db.Drugs.Select(d => d).Where(d => d.Id == di).FirstOrDefault();

                             drug.HealthTestId = t.Id;
                             t.Drugs.Add(drug);                           

                         }
                     }
                 }
             }

             db.SaveChanges();
            
           /* var sections = db.Sections;
            var groups = db.Groups;
            var tests = db.Tests;
            var drurs = db.Drugs;*/

            StreamWriter sw = new StreamWriter("outtree.txt");

            foreach (HealthTestSection sec in db.Sections)
            {
                sw.WriteLine($"\t{sec.Id}|--{sec.Title} ({sec.Description})");
                foreach (HealthTestGroup gr in sec.Groups)
                {
                    sw.WriteLine($"\t\t{gr.Id}|--{gr.Title} ({gr.Description})");
                    foreach (HealthTest t in gr.Tests)
                    {
                        sw.WriteLine($"\t\t\t\t{t.Id}|--{t.Title} ({t.Description})");
                        foreach (HealthTestDrug d in t.Drugs)
                        {
                            sw.WriteLine($"\t\t\t\t\t\t{d.Id}|-- ({d.Address} : {d.Cell} ) {d.Title} ({d.Description})");
                        }                        
                    }
                }
            }

            sw.Close();
            ///////////////////////////////////////////////////////////////////////////////////////////

        }

        public DevicePrograms Programs = new DevicePrograms();

        
    }
}
