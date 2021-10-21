﻿using DeRoso.Core.Health;
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
        public  event DeviceTest TestStarted;

        /// <summary>
        /// Событие удачного окончания теста устройства
        /// </summary>
        public  event DeviceTest TestComplete;


        /// <summary>
        /// Событие  ошибочного  окончания теста устройства
        /// </summary>
        public  event DeviceTest TestFailed;


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
            TraceRecivedData(args.data);

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


        /// <summary>
        /// Выполнить список тестов
        /// </summary>
        /// <param name="tests"></param>
        /// <returns></returns>
        public async Task<bool> Do(IEnumerable<HealthTest> tests)
        {
            foreach(HealthTest test in tests)
            {
                 await Do(test);

            }
            return false;
        }

        /// <summary>
        /// Выполнить конкретный тест
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public async Task<bool> Do(HealthTest test)
        {
            foreach(HealthTestDrug drug in test.Drugs)
            {

            }
            return false;
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
            TraceRecivedData(data);
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
            TestStarted?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

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
                TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }
                

            //получаем данные 
            int dataHV = DataBuffer[0];

            //проверка ошибки HV
            if (dataHV == 0x4f57)
            {
                //вызов обработчиков неудачного тестирования устройства
                TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;

            }

            //вызов обработчиков окончания тестирования устройства
            TestComplete?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true; 
        }


        /// <summary>
        /// Проверка селектора
        /// </summary>
        /// <returns></returns>
        private bool testSelector()
        {
            string testName = "Тестирование селектора";//вызов обработчиков начала тестирования устройства
            TestStarted?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));


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
                TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //получаем данные 
            int dataSelector = DataBuffer[0];

            //проверка ошибки селектора
            if (dataSelector == 0x4f59)
            {
                //вызов обработчиков неудачного тестирования устройства
                TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //вызов обработчиков окончания тестирования устройства
            TestComplete?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true;
           
        }


        private double kADC = 0.0;

        private double minADC = 3000;

        private double maxADC = 3850;

        /// <summary>
        /// Проверка калибровки
        /// </summary>
        /// <returns></returns>
        private bool testCalibration()
        {
            string testName = "Тестирование калибровки";//вызов обработчиков начала тестирования устройства
            TestStarted?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

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
                TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            sendCommand(DeviceCommands.AllOff);

            double average = DataBuffer.Average();

            //
            if (average > minADC && average < maxADC)
            {
                kADC = average;
                //вызов обработчиков окончания тестирования устройства
                TestComplete?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
                return true;
            }


            //вызов обработчиков неудачного тестирования устройства
            TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
            return false;
        }

        /// <summary>
        /// Проверка подключения электродов
        /// </summary>
        /// <returns></returns>
        private bool testElectrodes()
        {
            string testName = "Тестирование электродов";//вызов обработчиков начала тестирования устройства
            TestStarted?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Started));

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
                TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            sendCommand(DeviceCommands.AllOff);

            double average = DataBuffer.Average();

            if (average < registrationBorder)
            {
                //вызов обработчиков неудачного тестирования устройства
                TestFailed?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Failed));
                return false;
            }

            //вызов обработчиков окончания тестирования устройства
            TestComplete?.Invoke(usb, new DeviceInitializationTestEventArgs(testName, DeviceInitializationTestState.Complete));
            return true;
            
        }

        #endregion       
    }
}