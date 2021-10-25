using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UsbLibrary;

namespace DeRoso.Core.Device
{

    /// <summary>
    /// Делегат для события конвертирования данных устройства
    /// </summary>
    /// <param name="data">Данные для конвертации</param>
    public delegate void ReciveDataArray(byte[] data);

    /// <summary>
    /// Делегат для событий проверочных тестов для устройства
    /// </summary>
    /// <param name="sender">Отправитель сообщения (устройство)</param>
    /// <param name="args">Аргументы события</param>
    public delegate void DeviceTest(object sender, DeviceInitializationTestEventArgs args);

    /// <summary>
    /// Делегат для событий текущих тестов
    /// </summary>
    /// <param name="sennder">Отправитель сообщения (устройство)</param>
    /// <param name="args">Аргументыы события</param>
    public delegate void HealthTestProcess(object sennder, HealthTestEventArgs args);



    public class DeviceProvider
    {

        public DeviceProvider()
        {
            this.usb = new UsbHidPort();

            this.usb.OnSpecifiedDeviceArrived += new System.EventHandler(this.OnSpecifiedDeviceArrived);
            this.usb.OnSpecifiedDeviceRemoved += new System.EventHandler(this.OnSpecifiedDeviceRemoved);
            this.usb.OnDeviceArrived += new System.EventHandler(this.OnDeviceArrived);
            this.usb.OnDeviceRemoved += new System.EventHandler(this.OnDeviceRemoved);
            this.usb.OnDataRecieved += new UsbLibrary.DataRecievedEventHandler(this.OnDataRecieved);
            this.usb.OnDataSend += new System.EventHandler(this.OnDataSend);

            DataConversion += OnDataConversion;

            Connect();

            Reset();
        }

        #region ДЕЛЕГАТЫ И СОБЫТИЯ        
        

        /// <summary>
        /// Событие для конвертации данных 
        /// </summary>
        public static event ReciveDataArray DataConversion;


        /// <summary>
        /// Событие начала теста устройства
        /// </summary>
        public  event DeviceTest DeviceTestStarted;

        /// <summary>
        /// Событие удачного окончания теста устройства
        /// </summary>
        public  event DeviceTest DeviceTestComplete;


        /// <summary>
        /// Событие  ошибочного  окончания теста устройства
        /// </summary>
        public  event DeviceTest DeviceTestFailed;


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
        /// Буфер данных от прибора
        /// </summary>
        public List<int> DataBuffer = new List<int>();

        //порог регистрации данных
        private int registrationBorder = 130;

        /// <summary>
        /// Обработчик события конвертации данных 
        /// </summary>
        /// <param name="data"></param>
        public void OnDataConversion(byte[] data)
        {   

            for (int i = 0; i < 8; i++)
            {
                /* Младший байт 7 бит данные старший бит 0 признак младшего   */
                /* Старший байт 5 бит данных старший бит 1 признак старшего байта  */

                int dataTmp = data[i * 2 + 1] | (data[i * 2 + 1 + 1]) << 7;

                int[] correct = { 0x4f56, 0x4f57, 0x4f58, 0x4f59 };

                //получаем данные от селектора или HV
                if ( correct.Contains( dataTmp ) )
                {
                    if (DataBuffer.Count == 0)
                        DataBuffer.Add(dataTmp);
                    else
                        DataBuffer[0] = dataTmp;

                    isDataRecived = true;
                    return;
                }
                
                //получаем данные об измерении 
                dataTmp = data[i * 2 + 1] | (data[i * 2 + 1 + 1] & 0x1f) << 7;

                if (DataBuffer.Count < 512)
                    DataBuffer.Add(dataTmp > registrationBorder ? dataTmp : 0);
                else
                    isDataRecived = true;

            }
        }

        /// <summary>
        /// Обработчик события отправки данных в устройство
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataSend(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void TraceRecivedData(byte[] data)
        {
            Debug.WriteLine(BitConverter.ToString(data));
        }

        /// <summary>
        /// Обработчик события получения данных от устройства
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataRecieved(object sender, DataRecievedEventArgs args)
        {
            //TraceRecivedData(args.data);

            for (int i = 0; i < 16; i++)        //проверка на наличие 0 в данных
            {
                if (args.data[i + 1] == 0)
                    return;
            }
            
            DataConversion?.Invoke(args.data);
        }

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
        #endregion

        //размер буфера данных устройства
        public readonly int TXPackSize = 64;

        private UsbHidPort usb;



        /// <summary>
        /// Сброс устройства в начальное состояние
        /// </summary>
        /// <returns></returns>
        public void Reset()
        {
            sendCommand(DeviceCommands.MeteringOff);
            sendCommand(DeviceCommands.AllOff);
            sendCommand(DeviceCommands.OutDrugStop);
        }

        /// <summary>
        /// Соединение с устройством 
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            this.usb.ProductId = 0x1c05;
            this.usb.VendorId = 0xc252;           
            this.usb.CheckDevicePresent();

            return usb.SpecifiedDevice != null;
        }


        private float Calculate(IEnumerable<int> data) 
        {
            float sum = 0.0f;
            int count = 0;
            float value = 0.0f;

            
            //вычисляем среднее не нулевых элементов
            for (int i = 0; i < data.Count(); i++)      
            {
                int elem = data.ElementAt(i);
                if (elem > 0)
                {
                    sum += elem;
                    count++;
                }
            }

            if (count > 0)
                value = sum / count;

            //получаем значение параметра
            value = (value * 100.0f) / kADC;
            
            return value;
        }

        /// <summary>
        /// Выполнить список тестов
        /// </summary>
        /// <param name="tests">Список выполняемых тестов</param>
        /// <returns></returns>
        public void Do(IEnumerable<HealthTest> tests)
        {
            //сбрасываем прибор
            Reset();

            //Тестирование устройства
            if (!Test())
                return;

            //запускаем тесты в цикле
            foreach(HealthTest test in tests)
            {
                HealthTestStarted?.Invoke(this, new HealthTestEventArgs(test, EnumHealthTestStep.Started, TimeSpan.FromSeconds(0) ));

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

            //оставшееся время
            TimeSpan left = interval;

            while (duration < interval)
            {
                //ждем один тик
                Thread.Sleep(pausetick);

                //получаем общую длительность
                duration = DateTime.Now - begin;

                //получаем ставшееся время
                left = interval - duration;

                HealthTestTick?.Invoke(this, new HealthTestEventArgs(testitem, step, left));
            }

        }

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

            

            HealthTestTick?.Invoke(this, new HealthTestEventArgs(resTest, EnumHealthTestStep.MeassureBefore, TimeSpan.FromSeconds(0)));

            //если был использван HV импульс начинаем измерение ДО
            if (IsUsedHV)
            {
                /************************************************/
                /*                  ИЗМЕРЕНИЕ ДО                */
                /************************************************/                

                //готовим буфер к приемму данных
                PrepareDataBuffer();

                sendCommand(DeviceCommands.DiagnosticOn);
                sendCommand(DeviceCommands.MeteringOn);

                //ожидаем получение данных
                Task<bool> getdata = new Task<bool>(() => reciveData(TimeSpan.FromSeconds(3.0)));
                getdata.Start();
                getdata.Wait();

                sendCommand(DeviceCommands.MeteringOff);
                sendCommand(DeviceCommands.AllOff);

                //если данные не получены
                if (!getdata.Result)
                    return null;

                //схраняем результаты измерений до выдачи препарата
                MeassureBeforeHV = Calculate(DataBuffer);
                MeassureBeforeHV = rnd.Next(20, 98);               

            }

            resTest.MeassurmentBefore = MeassureBeforeHV;

            /************************************************/
            /*              ВЫДАЧА ПРЕПАРАТОВ               */
            /************************************************/

            foreach (HealthTestDrug drug in test.Drugs)
            {
                HealthTestDrugResult res = new HealthTestDrugResult();
                res.HealthTestDrugId = drug.Id;
                res.HealthTestId = test.Id;
                res.Test = test;
                res.Drug = drug;

                res.MeassurmentBefore = MeassureBeforeHV;                

                HealthTestTick?.Invoke(this, new HealthTestEventArgs(res, EnumHealthTestStep.AddDrug, TimeSpan.FromSeconds(0)));                               


                /************* ОЖИДАНИЕ ПЕРЕД ВЫДАЧЕЙ *****************/
                MakePause(drug.PauseBefore, 200, drug, EnumHealthTestStep.WaitBeforeDrugDespencing);

                /*************  ВЫДАЧА КОНКРЕТНОГО ПРЕПАРАТА **********/
                addDrug(drug.Address, drug.Cell);

                sendCommand(DeviceCommands.SelectorOn);
                sendCommand(DeviceCommands.OutDrugStart);

                MakePause(drug.Duration, 200, drug, EnumHealthTestStep.DrugDespencing);

                sendCommand(DeviceCommands.OutDrugStop);
                sendCommand(DeviceCommands.SelectorOff);
                

                /************* ОЖИДАНИЕ ПОСЛЕ ВЫДАЧИ ******************/

                MakePause(drug.PauseAfter, 200, drug, EnumHealthTestStep.WaitAfterDrugDespencing);

                /*************************************************/
                /*             ИЗМЕРЕНИЯ ПОСЛЕ  ВЫДАЧИ ПРЕПАРАТА */
                /*************************************************/

                HealthTestTick?.Invoke(this, new HealthTestEventArgs(res, EnumHealthTestStep.MeassureAfter, TimeSpan.FromSeconds(0)));

                //готовим буфер к приемму данных
                PrepareDataBuffer();

                sendCommand(DeviceCommands.DiagnosticOn);
                sendCommand(DeviceCommands.MeteringOn);

                //ожидаем получение данных
                Task<bool>  getdata = new Task<bool>(() => reciveData(TimeSpan.FromSeconds(3.0)));
                getdata.Start();
                getdata.Wait();

                sendCommand(DeviceCommands.MeteringOff);
                sendCommand(DeviceCommands.AllOff);


                //если данные не получены
                if (!getdata.Result)
                {
                    //вызов обработчиков неудачного теста 
                    //TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                    //return false;
                }                

                //схраняем результаты измерений после выдачи препарата
                res.MeassurmentAfter = Calculate(DataBuffer);
                res.MeassurmentAfter = rnd.Next(20, 98);

                //meassureBefore = res.MeassurmentAfter;                
                //resTest.Meassurments.Add(res);
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
                sendCommand(DeviceCommands.ImpulsOn);
                setFrequencyHV((int)(test.FrequencyHV * 100));

                IsUsedHV = true;
                Thread.Sleep(1000);
            }
            else
                IsUsedHV = false;

            sendCommand(DeviceCommands.AllOff);

            return resTest;
            
        }

        

        #region ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ       

        /// <summary>
        /// Отправить команду в устройство
        /// </summary>
        /// <param name="cmd">Команда в виде перечисления</param>
        /// <param name="data">Данные характерные для команды</param>
        private void sendCommand(DeviceCommands cmd, byte [] data = null)
        {
            sendToUSB(DeviceCommand.CreateCommand(cmd, data));
            Thread.Sleep(50);
        }

        /// <summary>
        /// Запись  данных в устройство с учетом разбиения на пакеты заданной длины
        /// </summary>
        /// <param name="data">Данные</param>
        /// <returns></returns>
        private bool sendToUSB(byte[] data)
        {
            //TraceRecivedData(data);
            if (this.usb.SpecifiedDevice != null)
            {
                //делим данные на пакеты с максимальным размером  TXPackSize
                List<byte[]> packs = DevicePacketMaker.Split(TXPackSize, data);

                //отправляем каждый пакет данных
                foreach (byte[] pack in packs)
                {
                    usb.SpecifiedDevice.SendData(pack);
                    
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Задание частоты импульса HV
        /// </summary>
        /// <param name="freq">Частота импульса</param>
        private void setFrequencyHV(int freq)
        {
            byte[] data = { (byte)freq, (byte)(freq >> 8) };
            sendCommand(DeviceCommands.SetFrequency, data);
        }


        /// <summary>
        /// Добавить препарат
        /// </summary>
        /// <param name="drug">Адрес</param>
        /// <param name="cell">Ячейка</param>
        private void addDrug(int drug, int cell)
        {
            if (drug <= 3)
                drug += 4;
            else
            if (drug > 3 && drug <= 10)
                drug += 5;
            else
            if (drug > 10 && drug <= 25)
                drug += 6;
            else
            if (drug > 25 && drug <= 56)
                drug += 7;
            else
            if (drug > 56 && drug <= 119)
                drug += 8;
            else
            if (drug > 119 && drug <= 246)
                drug += 9;
            else
            if (drug > 246 && drug <= 501)
                drug += 10;
            else
            if (drug > 501 && drug <= 1012)
                drug += 11;
            else
            if (drug > 1012 && drug <= 2035)
                drug += 12;
            else
            if (drug > 2035)
                drug += 13;


            byte[] data = { (byte)drug, (byte)(drug >> 8), (byte)cell };
            sendCommand(DeviceCommands.AddDrug, data);

        }


        /// <summary>
        /// Флаг готовности буфера данных
        /// </summary>
        bool isDataRecived = false;

        private void PrepareDataBuffer()
        {
            //очищаем буфер данных
            DataBuffer.Clear();

            //сбрасываем флаг окончания получения данных
            isDataRecived = false;
        }

        /// <summary>
        /// Ожидание получения данных от прибора
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private bool reciveData(TimeSpan timeout)
        {
            //начало ожидания данных
            DateTime begin = DateTime.Now;
            TimeSpan duration = TimeSpan.FromMilliseconds(0);

            //ожидание данных
            while (!isDataRecived)
            {
                Thread.Sleep(10);
                duration = DateTime.Now - begin;

                if (duration > timeout)
                    return false;
            }

            return true;
            
        }

        /// <summary>
        /// Тестирование работоспособности прибора
        /// </summary>
        /// <returns></returns>
        public  bool Test()
        {
            if (!testHV())
            {
                Debug.WriteLine("Ошибка проверки HV");
                sendCommand(DeviceCommands.AllOff);
                return false;
            }

            Thread.Sleep(500);
            if (!testSelector())
            {
                Debug.WriteLine("Ошибка проверки селектора");
                sendCommand(DeviceCommands.AllOff);
                return false;
            }

            Thread.Sleep(500);
            if (!testCalibration())
            {
                Debug.WriteLine("Ошибка проверки калибровки");
                sendCommand(DeviceCommands.AllOff);
                return false;
            }

            Thread.Sleep(500);
            if (!testElectrodes())
            {
                Debug.WriteLine("Ошибка проверки электродов");
                sendCommand(DeviceCommands.AllOff);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Тестирование импульса HV 
        /// </summary>
        /// <returns></returns>
        private  bool testHV()
        {
            string testName = "Тестирование импульса HV";

            //вызов обработчиков начала тестирования устройства
            DeviceTestStarted?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

            sendCommand(DeviceCommands.MeteringOff);
            sendCommand(DeviceCommands.ReleSelectorOn);
            sendCommand(DeviceCommands.ReleDiagnosticOff);

            setFrequencyHV(10000);
            Thread.Sleep(600);

            //Подготовка буфера к приему данных
            PrepareDataBuffer();

            //выдаем тестовые импульсы
            sendCommand(DeviceCommands.ImpulsTest);
            sendCommand(DeviceCommands.ImpulsTest);

            //ожидаем получение данных
            Task<bool> getdata = new Task<bool>( () => reciveData(TimeSpan.FromSeconds(3.0)));
            getdata.Start();
            getdata.Wait();

            //сброс частоты
            setFrequencyHV(0);

            //если данные не получены
            if (!getdata.Result)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }
                

            //получаем данные 
            int dataHV = DataBuffer[0];

            //проверка ошибки HV
            if (dataHV == 0x4f57)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;

            }

            //вызов обработчиков окончания тестирования устройства
            DeviceTestComplete?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true; 
        }


        /// <summary>
        /// Проверка селектора
        /// </summary>
        /// <returns></returns>
        private bool testSelector()
        {
            string testName = "Тестирование селектора";//вызов обработчиков начала тестирования устройства
            DeviceTestStarted?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));


            sendCommand(DeviceCommands.SelectorTest);
            sendCommand(DeviceCommands.SelectorTest);

            //Подготовка буфера к приему данных
            PrepareDataBuffer();

            sendCommand(DeviceCommands.ReadSelectorTest);            

            //ожидаем получение данных
            Task<bool> getdata = new Task<bool>(() => reciveData(TimeSpan.FromSeconds(3.0)));
            getdata.Start();
            getdata.Wait();

            //если данные не получены
            if (!getdata.Result)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //получаем данные 
            int dataSelector = DataBuffer[0];

            //проверка ошибки селектора
            if (dataSelector == 0x4f59)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //вызов обработчиков окончания тестирования устройства
            DeviceTestComplete?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true;
           
        }


        private float kADC = 0.0f;

        private float minADC = 3000.0f;

        private float maxADC = 3850.0f;

        /// <summary>
        /// Проверка калибровки
        /// </summary>
        /// <returns></returns>
        private bool testCalibration()
        {
            string testName = "Тестирование калибровки";//вызов обработчиков начала тестирования устройства
            DeviceTestStarted?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

            sendCommand(DeviceCommands.ReleDiagnosticOn);
            sendCommand(DeviceCommands.MeteringOn);

            //Подготовка буфера к приему данных
            PrepareDataBuffer();

            //ожидаем получение данных
            Task<bool> getdata = new Task<bool>(() => reciveData(TimeSpan.FromSeconds(3.0)));
            getdata.Start();
            getdata.Wait();

            //если данные не получены
            if (!getdata.Result)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            sendCommand(DeviceCommands.AllOff);

            float average = (float)DataBuffer.Average();

            //
            if (average > minADC && average < maxADC)
            {
                kADC = average;
                //вызов обработчиков окончания тестирования устройства
                DeviceTestComplete?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
                return true;
            }


            //вызов обработчиков неудачного тестирования устройства
            DeviceTestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
            return false;
        }

        /// <summary>
        /// Проверка подключения электродов
        /// </summary>
        /// <returns></returns>
        private bool testElectrodes()
        {
            string testName = "Тестирование электродов";//вызов обработчиков начала тестирования устройства
            DeviceTestStarted?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

            sendCommand(DeviceCommands.ReleCalibrationOff);
            sendCommand(DeviceCommands.DiagnosticOn);
            sendCommand(DeviceCommands.MeteringOn);

            //Подготовка буфера к приему данных
            PrepareDataBuffer();

            //ожидаем получение данных
            Task<bool> getdata = new Task<bool>(() => reciveData(TimeSpan.FromSeconds(3.0)));
            getdata.Start();
            getdata.Wait();

            //если данные не получены
            if (!getdata.Result)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            sendCommand(DeviceCommands.AllOff);

            double average = DataBuffer.Average();

            if (average < registrationBorder)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //вызов обработчиков окончания тестирования устройства
            DeviceTestComplete?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true;
            
        }

        #endregion       
    }
}
