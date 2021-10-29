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



    public class DeviceProvider
    {

        public DeviceProvider()
        {
           
            this.USB.OnSpecifiedDeviceArrived += new System.EventHandler(this.OnSpecifiedDeviceArrived);
            this.USB.OnSpecifiedDeviceRemoved += new System.EventHandler(this.OnSpecifiedDeviceRemoved);
            this.USB.OnDeviceArrived += new System.EventHandler(this.OnDeviceArrived);
            this.USB.OnDeviceRemoved += new System.EventHandler(this.OnDeviceRemoved);
           

            this.USB.OnDataRecieved += new UsbLibrary.DataRecievedEventHandler(this.OnDataRecieved);
            this.USB.OnDataSend += new System.EventHandler(this.OnDataSend);

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

                    IsDataRecived = true;
                    return;
                }
                
                //получаем данные об измерении 
                dataTmp = data[i * 2 + 1] | (data[i * 2 + 1 + 1] & 0x1f) << 7;

                if (DataBuffer.Count < 512)
                    DataBuffer.Add(dataTmp > registrationBorder ? dataTmp : 0);
                else
                    IsDataRecived = true;

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
        #endregion

        //размер буфера данных устройства
        public readonly int TXPackSize = 64;

        public UsbHidPort USB
        {
            get;
            private set;
        } = new UsbHidPort();


        /// <summary>
        /// Прибор готов к работе
        /// </summary>
        public bool IsReady
        {
            get;
            private set;
        } = true;

        /// <summary>
        /// Сброс устройства в начальное состояние
        /// </summary>
        /// <returns></returns>
        public void Reset()
        {
            SendCommand(DeviceCommands.MeteringOff);
            SendCommand(DeviceCommands.AllOff);
            SendCommand(DeviceCommands.OutDrugStop);
        }

        /// <summary>
        /// Соединение с устройством 
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            this.USB.ProductId = 0x1c05;
            this.USB.VendorId = 0xc252;           
            this.USB.CheckDevicePresent();

            return USB.SpecifiedDevice != null;
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

        

        public float Meassure()
        {
            //готовим буфер к приемму данных
            PrepareDataBuffer();

            SendCommand(DeviceCommands.DiagnosticOn);
            SendCommand(DeviceCommands.MeteringOn);
            
            //ожидаем получение данных
            Task<bool> getdata = new Task<bool>(() => reciveData(TimeSpan.FromSeconds(3.0)));
            getdata.Start();
            getdata.Wait();

            SendCommand(DeviceCommands.MeteringOff);
            SendCommand(DeviceCommands.AllOff);

            //если данные не получены
            if (!getdata.Result)
                return float.NegativeInfinity;

            return Calculate(DataBuffer);
        }
        

        #region ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ       

        /// <summary>
        /// Отправить команду в устройство
        /// </summary>
        /// <param name="cmd">Команда в виде перечисления</param>
        /// <param name="data">Данные характерные для команды</param>
        public void SendCommand(DeviceCommands cmd, byte [] data = null)
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
            if (this.USB.SpecifiedDevice != null)
            {
                //делим данные на пакеты с максимальным размером  TXPackSize
                List<byte[]> packs = DevicePacketMaker.Split(TXPackSize, data);

                //отправляем каждый пакет данных
                foreach (byte[] pack in packs)
                {
                    USB.SpecifiedDevice.SendData(pack);
                    
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
        public void SetFrequencyHV(int freq)
        {
            byte[] data = { (byte)freq, (byte)(freq >> 8) };
            SendCommand(DeviceCommands.SetFrequency, data);
        }


        /// <summary>
        /// Добавить препарат
        /// </summary>
        /// <param name="drug">Адрес</param>
        /// <param name="cell">Ячейка</param>
        public void AddDrug(int drug, int cell)
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
            SendCommand(DeviceCommands.AddDrug, data);

        }


        /// <summary>
        /// Флаг готовности буфера данных
        /// </summary>
        private  bool IsDataRecived = false;

        private void PrepareDataBuffer()
        {
            //очищаем буфер данных
            DataBuffer.Clear();

            //сбрасываем флаг окончания получения данных
            IsDataRecived = false;
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
            while (!IsDataRecived)
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
            if (!TestHV())
            {
                Debug.WriteLine("Ошибка проверки HV");
                SendCommand(DeviceCommands.AllOff);
                return false;
            }

            Thread.Sleep(500);
            if (!TestSelector())
            {
                Debug.WriteLine("Ошибка проверки селектора");
                SendCommand(DeviceCommands.AllOff);
                return false;
            }

            Thread.Sleep(500);
            if (!TestCalibration())
            {
                Debug.WriteLine("Ошибка проверки калибровки");
                SendCommand(DeviceCommands.AllOff);
                return false;
            }

            Thread.Sleep(500);
            if (!TestElectrodes())
            {
                Debug.WriteLine("Ошибка проверки электродов");
                SendCommand(DeviceCommands.AllOff);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Тестирование импульса HV 
        /// </summary>
        /// <returns></returns>
        public  bool TestHV()
        {
            string testName = "Тестирование импульса HV";

            //вызов обработчиков начала тестирования устройства
            DeviceTestStarted?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

            SendCommand(DeviceCommands.MeteringOff);
            SendCommand(DeviceCommands.ReleSelectorOn);
            SendCommand(DeviceCommands.ReleDiagnosticOff);

            SetFrequencyHV(10000);
            Thread.Sleep(600);

            //Подготовка буфера к приему данных
            PrepareDataBuffer();

            //выдаем тестовые импульсы
            SendCommand(DeviceCommands.ImpulsTest);
            SendCommand(DeviceCommands.ImpulsTest);

            //ожидаем получение данных
            Task<bool> getdata = new Task<bool>( () => reciveData(TimeSpan.FromSeconds(3.0)));
            getdata.Start();
            getdata.Wait();

            //сброс частоты
            SetFrequencyHV(0);

            //если данные не получены
            if (!getdata.Result)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }                

            //получаем данные 
            int dataHV = DataBuffer[0];

            //проверка ошибки HV
            if (dataHV == 0x4f57)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;

            }

            //вызов обработчиков окончания тестирования устройства
            DeviceTestComplete?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true; 
        }


        /// <summary>
        /// Проверка селектора
        /// </summary>
        /// <returns></returns>
        public bool TestSelector()
        {
            string testName = "Тестирование селектора";//вызов обработчиков начала тестирования устройства
            DeviceTestStarted?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));


            SendCommand(DeviceCommands.SelectorTest);
            SendCommand(DeviceCommands.SelectorTest);

            //Подготовка буфера к приему данных
            PrepareDataBuffer();

            SendCommand(DeviceCommands.ReadSelectorTest);            

            //ожидаем получение данных
            Task<bool> getdata = new Task<bool>(() => reciveData(TimeSpan.FromSeconds(3.0)));
            getdata.Start();
            getdata.Wait();

            //если данные не получены
            if (!getdata.Result)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //получаем данные 
            int dataSelector = DataBuffer[0];

            //проверка ошибки селектора
            if (dataSelector == 0x4f59)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //вызов обработчиков окончания тестирования устройства
            DeviceTestComplete?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true;
           
        }


        private float kADC = 0.0f;

        private float minADC = 3000.0f;

        private float maxADC = 3850.0f;

        /// <summary>
        /// Проверка калибровки
        /// </summary>
        /// <returns></returns>
        public bool TestCalibration()
        {
            string testName = "Тестирование калибровки";//вызов обработчиков начала тестирования устройства
            DeviceTestStarted?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

            SendCommand(DeviceCommands.ReleDiagnosticOn);
            SendCommand(DeviceCommands.MeteringOn);

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
                DeviceTestFailed?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            SendCommand(DeviceCommands.AllOff);

            float average = (float)DataBuffer.Average();

            //
            if (average > minADC && average < maxADC)
            {
                kADC = average;
                //вызов обработчиков окончания тестирования устройства
                DeviceTestComplete?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
                return true;
            }


            //вызов обработчиков неудачного тестирования устройства
            DeviceTestFailed?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
            return false;
        }

        /// <summary>
        /// Проверка подключения электродов
        /// </summary>
        /// <returns></returns>
        public  bool TestElectrodes()
        {
            string testName = "Тестирование электродов";//вызов обработчиков начала тестирования устройства
            DeviceTestStarted?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

            SendCommand(DeviceCommands.ReleCalibrationOff);
            SendCommand(DeviceCommands.DiagnosticOn);
            SendCommand(DeviceCommands.MeteringOn);

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
                DeviceTestFailed?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            SendCommand(DeviceCommands.AllOff);

            double average = DataBuffer.Average();

            if (average < registrationBorder)
            {
                //вызов обработчиков неудачного тестирования устройства
                DeviceTestFailed?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //вызов обработчиков окончания тестирования устройства
            DeviceTestComplete?.Invoke(USB, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true;
            
        }

        #endregion       
    }
}
