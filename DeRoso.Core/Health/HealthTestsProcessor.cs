using DeRoso.Core.Data;
using DeRoso.Core.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DeRoso.Core.Health
{
    public class HealthTestsProcessor : ViewModelBase
    {

        public HealthTestsProcessor(DeviceProvider dev)
        {
            Device = dev;
            Device.DeviceTestStarted += OnDeviceTestEvent;
            Device.DeviceTestFailed += OnDeviceTestEvent;
            Device.DeviceTestComplete += OnDeviceTestEvent;
        }

        private void OnDeviceTestEvent(object sender, DeviceInitializationTestEventArgs args)
        {
            CurrentOperation = args.TestDescription;
        }

        public DeviceProvider Device
        {
            get;
            private set;
        }

        public HealthTestReport Report
        {
            get;
            private set;
        } = new HealthTestReport();


        public ObservableCollection<HealthTestResult> Results
        {
            get;
            private set;
        } = new ObservableCollection<HealthTestResult>();



        /// <summary>
        /// Использовать случайные знаения вместо реальных измерений
        /// </summary>
        public bool IsUsedMeassureAsRandom
        {
            get
            {
                return _isUsedMeassureAsRandom;
            }
            set
            {
                if (value == _isUsedMeassureAsRandom)
                    return;

                _isUsedMeassureAsRandom = value;
                OnPropertyChanged();
            }
        }
        private bool _isUsedMeassureAsRandom = false;

        
        /// <summary>
        /// Автосохранение результатовпосле окончвния тестирования
        /// </summary>
        public bool IsAutoSave
        {
            get { return _isAutoSave; }
            set
            {
                if (_isAutoSave == value)
                    return;

                _isAutoSave = value;
                OnPropertyChanged();
            }
        }
        private bool _isAutoSave = true;


        
        /// <summary>
        /// Показать результаты автосохранения
        /// </summary>
        public bool IsShowSaveResults
        {
            get { return _isShowSaveResults; }
            set
            {
                if (_isShowSaveResults == value)
                    return;

                _isShowSaveResults = value;
                OnPropertyChanged();
            }
        }
        private bool _isShowSaveResults = true;


        /// <summary>
        /// Приостановить процесс тестирования после выполнения текущего теста
        /// </summary>
        public void Pause()
        {
            IsPaused = !IsPaused;
            IsStoped = false;
        }

        /// <summary>
        /// Остановить процесс тестирования после выполнения текущего теста
        /// </summary>
        public void Stop()
        {
            IsStoped = true;
            IsPaused = false;
        }

        
        /// <summary>
        /// Флаг запуска тестирования
        /// </summary>
        public bool IsStarted
        {
            get { return _isStarted; }
            private set
            {
                if (_isStarted == value)
                    return;

                _isStarted = value;
                OnPropertyChanged();
            }
        }
        private bool _isStarted = false;
             

        /// <summary>
        /// Флаг приостановки процесса тестирования
        /// </summary>
        public bool IsPaused
        {
            get { return _isPaused; }
            private set
            {
                if (_isPaused == value)
                    return;

                _isPaused = value;
                OnPropertyChanged();
            }
        }
        private bool _isPaused = false;

         
        /// <summary>
        /// Флаг остановки процесса тастирования
        /// </summary>
        public bool IsStoped
        {
            get { return _isStoped; }
            private set
            {
                if (_isStoped == value)
                    return;

                _isStoped = value;
                OnPropertyChanged();
            }
        }
        private bool _isStoped = false;


        public bool IsUsedHV
        {
            get;
            private set;
        } = true;

        /// <summary>
        /// Результат измерений до пока не получим выдан HV импульс
        /// </summary>
        public float MeassureBeforeHV
        {
            get;
            private set;
        } = 0.0f;

        /// <summary>
        /// Время в секундах оставшееся до конца текущей операции
        /// </summary>
        public double TimeLeft
        {
            get
            {
                return _timeLeft;
            }
            private set
            {
                if (value == _timeLeft)
                    return;

                _timeLeft = value;
                OnPropertyChanged();
            }
        }
        private double _timeLeft = 0.0;

        /// <summary>
        /// Текстовое описание текущей операции
        /// </summary>
        public string CurrentOperation
        {
            get
            {
                return _currentOperation;
            }
            private set
            {
                if (value == _currentOperation)
                    return;

                _currentOperation = value;
                OnPropertyChanged();
            }
        }
        private string _currentOperation = "";


        /// <summary>
        /// Текущий тест
        /// </summary>
        public HealthTest CurrentTest
        {
            get
            {
                return _currentTest;
            }
            set
            {
                if (value == _currentTest)
                    return;

                _currentTest = value;
                OnPropertyChanged();              
            }
        }
        private HealthTest _currentTest = null;

        
        /// <summary>
        /// Выбранный пациент
        /// </summary>
        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set
            {
                if (_currentPatient == value)
                    return;

                _currentPatient = value;
                OnPropertyChanged();
            }
        }
        private Patient _currentPatient;


        /// <summary>
        /// Доступные пациенты
        /// </summary>
        public List<Patient> Patients
        {
            get
            {
                return _patients;
            }
            private set
            {
                if (value == _patients)
                    return;

                _patients = value;
                OnPropertyChanged();
            }
        }
        private List<Patient> _patients = DeRossoDataWorker.GetAllPatients(); 

        public void Update()
        {
            Patients =  DeRossoDataWorker.GetAllPatients();
        }
        /// <summary>
        /// Выбранные тесты 
        /// </summary>
        public List<HealthTest> Tests
        {
            get;
            private set;

        } = DeRossoDataWorker.GetLastSelectedTests(); 

        /// <summary>
        /// Делегат для событий текущих тестов
        /// </summary>
        /// <param name="sennder">Отправитель сообщения (устройство)</param>
        /// <param name="args">Аргументыы события</param>
        public delegate void HealthTestProcess(object sennder, HealthTestEventArgs args);

        /// <summary>
        /// Событие при нахождении теста в режиме ожидания перехода к следующему шагу
        /// </summary>
        public event HealthTestProcess HealthTestTick;

        /// <summary>
        /// Событие при запуске теста
        /// </summary>
        public event HealthTestProcess HealthTestStarted;

        /// <summary>
        /// Событие неудачного окончания  теста
        /// </summary>
        public event HealthTestProcess HealthTestComplete;

        /// <summary>
        /// Событие удачного окончания  теста
        /// </summary>
        public event HealthTestProcess HealthTestFailed;

        /// <summary>
        /// Обработчик отключения устройства от USB порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeviceRemoved(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Обработчик подключения устройства к USB порту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeviceArrived(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Обработчик события отключения конкретного устройства от USB порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecifiedDeviceRemoved(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Обработчик события подключения конкретного устройства к  USB порту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }       
        

        /// <summary>
        /// Функция ожидания заданного времени с выдачей сообщений с заданным интервалом
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="pausetick"></param>
        private void MakePause(TimeSpan interval, int pausetick, HealthTestItem testitem, EnumHealthTestStep step)
        {
            ///фиксируем время старта
            DateTime begin = DateTime.Now;

            ///текущее значение ожидания
            TimeSpan duration = TimeSpan.FromSeconds(0);

            //выставляем текущую операцию
            CurrentOperation = EnumHelper.GetDescription(step);

            //оставшееся время
            TimeSpan left = interval;
            TimeLeft = left.TotalSeconds;

            while (duration < interval)
            {
                //ждем один тик
                Thread.Sleep(pausetick);

                //получаем общую длительность
                duration = DateTime.Now - begin;

                //получаем ставшееся время
                left = interval - duration;
                TimeLeft = left.TotalSeconds;

                HealthTestTick?.Invoke(this, new HealthTestEventArgs(testitem, step, left));
            }

        }


        /// <summary>
        /// Выполнить список тестов
        /// </summary>
        /// <param name="tests">Список выполняемых тестов</param>
        /// <returns></returns>
        public void Do(IEnumerable<HealthTest> tests)
        {
            IsStoped  = false;
            IsPaused  = false;
            IsStarted = false;


            

            var dispatcher = Application.Current.Dispatcher;

            //очищаем список результатов
            dispatcher.BeginInvoke(new Action(
                () => Report.Clear())
            );           

            //сбрасываем прибор
            Device.Reset();

            //Тестирование устройства
            //if (!Device.IsReady)

            Device.Test();


            //запускаем тесты в цикле
            foreach (HealthTest test in tests)
            {
                HealthTestStarted?.Invoke(this, new HealthTestEventArgs(test, EnumHealthTestStep.Started, TimeSpan.FromSeconds(0)));
                CurrentOperation = EnumHelper.GetDescription(EnumHealthTestStep.Started);
                CurrentTest = test;

                Thread.Sleep(1000);

                ///выполняем тест и получаем результат 
                HealthTestResult res = Do(test);

                //если тест выполнен и получены результаты
                if (res != null)
                {
                    
                    HealthTestComplete?.Invoke(this, new HealthTestEventArgs(test, EnumHealthTestStep.Complete, TimeSpan.FromSeconds(0)));
                }
                else
                {
                    IsStoped = true;
                    HealthTestFailed?.Invoke(this, new HealthTestEventArgs(test, EnumHealthTestStep.Failed, TimeSpan.FromSeconds(0)));
                    CurrentOperation = "Ошибка при проведении теста...";
                    break;
                }
                
                ///если прервали тестирование
                if ( IsStoped )
                {
                    IsStarted = false;
                    IsPaused = false;
                    CurrentOperation = "Тестирование остановлено...";
                    return;
                }
                
                ///если тестирование поставили на паузу 
                while ( IsPaused )
                {
                    CurrentOperation = "Тестирование приостановлено...";
                    Thread.Sleep(500);
                }
            }

            

            IsStarted = false;
            IsPaused = false;
            IsStoped = true;

            CurrentOperation = "Тестирование завершено...";
            Thread.Sleep(2000);

            if (IsAutoSave)
            {
                CurrentOperation = "Сохранение результатов...";
                Report.Save(IsShowSaveResults);
            }
            
            Thread.Sleep(2000);

        }

        /// <summary>
        /// Выполнить конкретный тест
        /// </summary>
        /// <param name="test">Конкретный выполняемый тест</param>
        /// <returns></returns>
        public HealthTestResult Do(HealthTest test)
        {
            var dispatcher = Application.Current.Dispatcher;
            Random rnd = new Random(DateTime.Now.Millisecond);

            //готовим объект результата теста
            HealthTestResult resTest = new HealthTestResult();
            resTest.HealthTestId = test.Id;
            resTest.Test = test;


            //HealthTestTick?.Invoke(this, new HealthTestEventArgs(resTest, EnumHealthTestStep.MeassureBefore, TimeSpan.FromSeconds(0)));

            //добавляем новый результат теста
            dispatcher.BeginInvoke(new Action(
                () => Report.AddTestResult(resTest))
            );
            
            CurrentOperation = EnumHelper.GetDescription(EnumHealthTestStep.MeassureBefore);
            Thread.Sleep(1000);

            //если был использван HV импульс заново начинаем измерение ДО
            if (IsUsedHV)
                MeassureBeforeHV = IsUsedMeassureAsRandom ? (rnd.Next(20, 98) + (float)rnd.NextDouble()) : Device.Meassure(); 
                
            
            //устанавливаем значение измерения до для всего теста
            resTest.MeassurmentBefore = MeassureBeforeHV;


            /************************************************/
            /*              ВЫДАЧА ПРЕПАРАТОВ               */
            /************************************************/

            foreach (HealthTestReciept r in test.Reciepts)
            {
                //создаем результат для конкретного препарата и добавляем к результату теста
                HealthTestDrugResult res = new HealthTestDrugResult();
                HealthTestDrug drug = r.Drug;

                res.HealthTestDrugId = r.Id;
                res.HealthTestId = test.Id;
                res.Test = test;
                res.Drug = drug;

                //устанавливаем значение измерения до для текущего препарата
                res.MeassurmentBefore = MeassureBeforeHV;
                

                dispatcher.BeginInvoke(new Action(
                    () => resTest.Meassurments.Add(res))
                );

                HealthTestTick?.Invoke(this, new HealthTestEventArgs(res, EnumHealthTestStep.AddDrug, TimeSpan.FromSeconds(0)));


                /************* ОЖИДАНИЕ ПЕРЕД ВЫДАЧЕЙ *****************/
                MakePause(test.PauseBeforeDrug, 200, drug, EnumHealthTestStep.WaitBeforeDrugDespencing);

                /*************  ВЫДАЧА КОНКРЕТНОГО ПРЕПАРАТА **********/
                Device.AddDrug(drug.Address, drug.Cell);

                Device.SendCommand(DeviceCommands.SelectorOn);
                Device.SendCommand(DeviceCommands.OutDrugStart);

                MakePause(test.DrugDuration, 200, drug, EnumHealthTestStep.DrugDespencing);

                Device.SendCommand(DeviceCommands.OutDrugStop);
                Device.SendCommand(DeviceCommands.SelectorOff);


                /************* ОЖИДАНИЕ ПОСЛЕ ВЫДАЧИ ******************/

                MakePause(test.PauseAfterDrug, 200, drug, EnumHealthTestStep.WaitAfterDrugDespencing);

                /*************************************************/
                /*             ИЗМЕРЕНИЯ ПОСЛЕ  ВЫДАЧИ ПРЕПАРАТА */
                /*************************************************/                

                //схраняем результаты измерений после выдачи препарата
                res.MeassurmentAfter = IsUsedMeassureAsRandom ? (rnd.Next(20, 98) + (float)rnd.NextDouble()) : Device.Meassure();
            }


            resTest.SelectOptimalDrug();


            /************************************************/
            /*            ВЫДАЧА ИМПУЛЬСА HV                */
            /************************************************/

            if (test.UseHV)
            {
                /************* ОЖИДАНИЕ ПЕРЕД ИМПУЛЬСОМ  *****************/
                MakePause(test.PauseBeforeHV, 200, test, EnumHealthTestStep.WaitHV);

                /************* ВЫДАЧА ИМПУЛЬСА  *****************/
                Device.SendCommand(DeviceCommands.ImpulsOn);
                Device.SetFrequencyHV((int)(test.FrequencyHV * 100));

                CurrentOperation = EnumHelper.GetDescription(EnumHealthTestStep.ImpulseHV);

                IsUsedHV = true;
                Thread.Sleep(1000);
            }
            else
                IsUsedHV = false;

            Device.SendCommand(DeviceCommands.AllOff);

            return resTest;

        }

    }
}
