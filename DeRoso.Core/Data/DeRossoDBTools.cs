using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Data
{

    /// <summary>
    /// Синглтон для заполнения и трассировки базы данных
    /// </summary>
    public class DeRossoDBTools
    {
        private static readonly DeRossoDBTools instance = new DeRossoDBTools();

        public static DeRossoDBTools Instance => instance;

        private DeRossoDBTools()
        {

        }

        /// <summary>
        /// Заполнение базы данных из отдельных файлов
        /// </summary>
        /// <param name="db">Контекст базы данных</param>
        /// <param name="progXMLPath">Путь к файлу  данных для тестов</param>
        /// <param name="drugFilePath">Путь к файлу  данных для препаратов</param>
        public void InitDB(DeRosoContext db, string progXMLPath, string drugFilePath)
        {
            DevicePrograms progs = new DevicePrograms();
            progs.Load(progXMLPath);

            List<DeviceProgram> all = progs.Programs;

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
                int secId = db.Sections.Local.Select(s => s).Where(s => s.Title == pr).FirstOrDefault().Id;

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
                int secId = db.Sections.Local.Select(s => s).Where(s => s.Title == p.SectionTitle).FirstOrDefault().Id;
                int grId = db.Groups.Local.Select(g => g).Where(g => g.Title == p.SubsectionTitle).FirstOrDefault().Id;

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
            int lastAddress = 0;

            string[] nameDrugs = File.ReadAllLines(drugFilePath, Encoding.Default);

            for (int i = 0; i < nameDrugs.Length; i++)
            {
                string [] Element = nameDrugs[i].Split('\t');
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
                

                db.Drugs.Add(new HealthTestDrug()
                    {
                        
                        Id = i + 10000,
                        Title = drug.Name,
                        Address = drug.Address,
                        Cell = drug.Cell,
                        Description = drug.Description
                    }
                );
               
            }

           



            db.SaveChanges();

            Random rnd = new Random(DateTime.Now.Millisecond);
            int maxID = 10000 + db.Drugs.Count() - 1;

            foreach (HealthTestSection sec in db.Sections)
            {

                foreach (HealthTestGroup gr in sec.Groups)
                {

                    foreach (HealthTest t in gr.Tests)
                    {


                        if (t.Reciepts == null)
                            t.Reciepts = new ObservableCollection<HealthTestReciept>();
                        else
                            t.Reciepts.Clear();

                        int cnt = rnd.Next(1, 6);

                        for (int i = 0; i < cnt; i++ )
                        {                           

                            int di = rnd.Next(10000, maxID);
                            var drug = db.Drugs.Select(d => d).Where(d => d.Id == di).FirstOrDefault();

                            HealthTestReciept reciept = new HealthTestReciept()
                            {
                                HealthTestDrugId = t.Id,
                                Order = i + 1,
                                Drug = drug
                            };

                            
                            t.Reciepts.Add(reciept);                           

                        }
                    }
                }
            }

            db.SaveChanges();
            
        }

        public void TraceDB(DeRosoContext db, string outFilePath)
        {
            StreamWriter sw = new StreamWriter(outFilePath);

            foreach (HealthTestSection sec in db.Sections)
            {
                sw.WriteLine($"\t{sec.Id}|--{sec.Title} ({sec.Description})");
                foreach (HealthTestGroup gr in sec.Groups)
                {
                    sw.WriteLine($"\t\t{gr.Id}|--{gr.Title} ({gr.Description})");
                    foreach (HealthTest t in gr.Tests)
                    {
                        sw.WriteLine($"\t\t\t\t{t.Id}|--{t.Title} ({t.Description})");
                        foreach (HealthTestReciept d in t.Reciepts)
                        {
                            sw.WriteLine($"\t\t\t\t\t\t{d.Id}|-- ({d.Order} {d.Drug.Address} : {d.Drug.Cell} ) {d.Drug.Title} ({d.Drug.Description})");
                        }
                    }
                }
            }

            sw.Close();
        }
    }
}
