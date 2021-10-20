using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UsbLibrary;

namespace DeRoso.Core.Device
{
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

            testHV();

            DataConversion += OnDataConversion;
        }

        #region ДЕЛЕГАТЫ И СОБЫТИЯ

        
        /// <summary>
        /// Делегат для события конвертирования данных устройства
        /// </summary>
        /// <param name="data">Данные для конвертации</param>
        public delegate void ReciveDataArray(byte[] data);

        /// <summary>
        /// Событие для конвертации данных 
        /// </summary>
        public static event ReciveDataArray DataConversion;


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

                if ( correct.Contains( dataTmp ) )
                {
                    return;
                }

                /*
                Data_SELECTOR = Data_HV = 0;
                if (dataTmp == 0x4f56 || dataTmp == 0x4f57 || dataTmp == 0x4f58 || dataTmp == 0x4f59)
                {
                    Data_SELECTOR = Data_HV = dataTmp;
                    return;
                }

                Data_out = data[i * 2 + 1] | (data[i * 2 + 1 + 1] & 0x1f) << 7;

                if (Data_out > 130)  //порог регистрации 0.1V
                    dataFoleBuf[indexBufFole] = Data_out;     //512 значений
                else
                    dataFoleBuf[indexBufFole] = 0;

                dataReciveComplete++;
                indexBufFole++; // 0 - 512 (текущий объем данных)
                indexBufFole &= 0x1ff; // сброс индекса (оганичение по размеру 512 ) 
                */

            }
        }

        /// <summary>
        /// Обработчик события отправки данных в устройство
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataSend(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обработчик события получения данных от устройства
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataRecieved(object sender, DataRecievedEventArgs args)
        {
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обработчик подключения устройства к USB порту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeviceArrived(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обработчик события отключения конкретного устройства от USB порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecifiedDeviceRemoved(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обработчик события подключения конкретного устройства к  USB порту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        //размер буфера данных устройства
        public readonly int TXPackSize = 64;

        private UsbHidPort usb;

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
        public async Task<bool> Do(List<HealthTest> tests)
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

        public async Task<bool> Meassure(HealthTestDrug drug)
        {
            
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
        }

        /// <summary>
        /// Запись  данных в устройство с учетом разбиения на пакеты заданной длины
        /// </summary>
        /// <param name="data">Данные</param>
        /// <returns></returns>
        private bool sendToUSB(byte[] data)
        {
            
            if (this.usb.SpecifiedDevice != null)
            {
                //делим данные на пакеты с максимальным размером  TXPackSize
                List<byte[]> packs = DevicePacketMaker.Split(TXPackSize, data);

                //отправляем каждый пакет данных
                foreach (byte[] pack in packs)
                {
                    usb.SpecifiedDevice.SendData(pack);
                    Thread.Sleep(20);
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
        /// Тестирование импульса HV 
        /// </summary>
        /// <returns></returns>
        private bool testHV()
        {
            sendCommand(DeviceCommands.MeteringOff);
            sendCommand(DeviceCommands.ReleSelectorOn);
            sendCommand(DeviceCommands.ReleDiagnosticOff);

            setFrequencyHV(10000);
            Thread.Sleep(600);

            sendCommand(DeviceCommands.ImpulsTest);
            sendCommand(DeviceCommands.ImpulsTest);

            return false; 
        }

        #endregion

        /*
        
        private bool test()
        {
            if (stepTest == 0)                      //проверка работы HV
            {
                label_Wrning.Text = "";
                CmdOut((byte)MyComm.Metering_OFF);
                CmdOut((byte)MyComm.RELE_SELECTOR_ON);
                CmdOut((byte)MyComm.RELE_DIAGNOST_OFF);

                Set_Freq(10000);   //Set_Freq Установка частоты HV                

                Thread.Sleep(600);
                Data_HV = 0;

                CmdOut((byte)MyComm.Impuls_TEST);
                CmdOut((byte)MyComm.Impuls_TEST);

                stepTest++;
                return;
            }

            if (stepTest == 1)
            {
                if (Data_HV == 0)
                    return;

                Set_Freq(0);               //Set_Freq Установка частоты HV  

                if (Data_HV == 0x4f57)
                {
                    CmdOut((byte)MyComm.All_Off);
                    this.BackColor = Color.Coral;
                    stepTest = 0;
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    label_Wrning.Text = "Ошибка HV";
                    return;
                }

                stepTest++;
                return;
            }

            if (stepTest == 2)          //проверка работы Selector
            {
                CmdOut((byte)MyComm.SELECTOR_TEST); //RELE_CALIBR_ON, CP_RELE_SELECTOR_OFF, CP_RELE_DIAGNOST_ON 	
                CmdOut((byte)MyComm.SELECTOR_TEST);
                CmdOut((byte)MyComm.READ_SELECTOR_TEST);
                stepTest++;
                return;
            }


            if (stepTest == 3)
            {
                if (Data_SELECTOR == 0)
                    return;

                if (Data_SELECTOR == 0x4f59)
                {
                    CmdOut((byte)MyComm.All_Off);
                    this.BackColor = Color.Coral;
                    stepTest = 0;
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    label_Wrning.Text = "Ошибка Селектора";
                    return;
                }
                stepTest++;
                return;
            }


            if (stepTest == 4)      //КАЛИБРОВКА
            {
                CmdOut((byte)MyComm.RELE_DIAGNOST_ON);
                CmdOut((byte)MyComm.Metering_ON);
                dataReciveComplete = 0;
                startCalibr = "Start";
                stepTest++;
                return;
            }

            if (stepTest == 5)
            {
                testEndCalibr();
                if (startCalibr == "")
                    stepTest++;
                return;
            }

            if (stepTest == 6)                              //Проверка на подключение электродов
            {
                CmdOut((byte)MyComm.Rele_Calibr_OFF);
                CmdOut((byte)MyComm.Diagnostic_ON);
                CmdOut((byte)MyComm.Metering_ON);
                dataReciveComplete = 0;
                TimeControl_OUT = 0;
                TimeControl_IN = 0;

                stepTest++;
                return;
            }

            if (stepTest == 7)                              //Проверка на подключение электродов
            {
                tmp = Calculate();
                if (tmp == 0xffff)                          //преобразование не закончено
                    return;

                if ((int)tmp < 130)                         //меньше 100 мВ
                {
                    TimeControl_IN = 0;
                    this.BackColor = Color.Coral;
                    label_Wrning.Text = "Подключите электроды";

                    TimeControl_OUT++;
                    if (TimeControl_OUT > 11)
                    {
                        CmdOut((byte)MyComm.All_Off);
                        stepTest = 0;
                        timer1.Enabled = false;
                        timer2.Enabled = false;
                        label_Wrning.Text = "Стоп! Нет подключения электродов";
                        TimeControl_OUT = 0;
                        return;
                    }
                }
                else
                {
                    TimeControl_OUT = 0;

                    TimeControl_IN++;
                    if (TimeControl_IN > 2)
                    {
                        this.BackColor = SystemColors.Control;
                        label_Wrning.Text = "";
                        timer1.Enabled = true;
                        timer2.Enabled = false;
                        stepTest = 0;
                        TimeControl_IN = 0;
                    }
                }
                dataReciveComplete = 0;
            }
        }

        */
    }
}
