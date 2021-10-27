using DeRoso.Core.Data;
using DeRoso.Core.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    public class HealthTestsProcessor : ViewModelBase
    {

        public HealthTestsProcessor(DeviceProvider dev)
        {
            Device = dev;
        }

        public DeviceProvider Device
        {
            get;
            private set;
        }

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
        ///  Сохранить результаты тестирования
        /// </summary>
        /// <param name="sp"></param>
        public void Save(IResultsSaver sp)
        {
            sp.Save(Results);
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
            //очищаем список результатов
            Results.Clear();

            //сбрасываем прибор
            Device.Reset();

            //Тестирование устройства
            if (!Device.IsReady)
                Device.Test();


            //запускаем тесты в цикле
            foreach (HealthTest test in tests)
            {
                HealthTestStarted?.Invoke(this, new HealthTestEventArgs(test, EnumHealthTestStep.Started, TimeSpan.FromSeconds(0)));
                CurrentOperation = EnumHelper.GetDescription(EnumHealthTestStep.Started);
                Thread.Sleep(1000);

                HealthTestResult res = Do(test);

                if (res != null)
                {
                    HealthTestComplete?.Invoke(this, new HealthTestEventArgs(test, EnumHealthTestStep.Complete, TimeSpan.FromSeconds(0)));
                }
                else
                {
                    HealthTestFailed?.Invoke(this, new HealthTestEventArgs(test, EnumHealthTestStep.Failed, TimeSpan.FromSeconds(0)));
                    break;
                }
            }
        }

        /// <summary>
        /// Выполнить конкретный тест
        /// </summary>
        /// <param name="test">Конкретный выполняемый тест</param>
        /// <returns></returns>
        public HealthTestResult Do(HealthTest test)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            //готовим объект результата теста
            HealthTestResult resTest = new HealthTestResult();
            resTest.HealthTestId = test.Id;
            resTest.Test = test;


            //HealthTestTick?.Invoke(this, new HealthTestEventArgs(resTest, EnumHealthTestStep.MeassureBefore, TimeSpan.FromSeconds(0)));

            //добавляем новый результат теста
            Results.Add(resTest);
            CurrentOperation = EnumHelper.GetDescription(EnumHealthTestStep.MeassureBefore);
            Thread.Sleep(1000);

            //если был использван HV импульс заново начинаем измерение ДО
            if (IsUsedHV)
                MeassureBeforeHV = IsUsedMeassureAsRandom ? rnd.Next(20, 98) : Device.Meassure(); 
                
            
            //устанавливаем значение измерения до для всего теста
            resTest.MeassurmentBefore = MeassureBeforeHV;


            /************************************************/
            /*              ВЫДАЧА ПРЕПАРАТОВ               */
            /************************************************/

            foreach (HealthTestDrug drug in test.Drugs)
            {
                //создаем результат для конкретного препарата и добавляем к результату теста
                HealthTestDrugResult res = new HealthTestDrugResult();
                res.HealthTestDrugId = drug.Id;
                res.HealthTestId = test.Id;
                res.Test = test;
                res.Drug = drug;

                //устанавливаем значение измерения до для текущего препарата
                res.MeassurmentBefore = MeassureBeforeHV;
                resTest.Meassurments.Add(res);

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
                res.MeassurmentAfter = IsUsedMeassureAsRandom ? rnd.Next(20, 98) : Device.Meassure();                
            }


            resTest.SelectOptimalResult();


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
