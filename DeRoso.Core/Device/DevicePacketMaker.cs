using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Device
{
    static public class DevicePacketMaker
    {
        /// <summary>
        /// Разделение буфера данных на пакеты заданной длины
        /// </summary>
        /// <param name="maxSize"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static public List<byte[]> Split(int size, byte[] data)
        {
            List<byte[]> packs = new List<byte[]>();

            //разделить данные на пакеты с максимальной длинной  TXPackSize
            
            int lastIndex = 0;

            while (lastIndex < data.Length)
            {
                int left = data.Length - lastIndex;
                int count = (left < size) ? left : size;

                byte[] buffer = new byte[size];

                //копируем данные во временный буфер
                Array.Copy(data, lastIndex, buffer, 0, count);

                //сохраняем пакет в буфер данных
                packs.Add(buffer);

                //смещаем позицию копирования данных
                lastIndex += count;

            }
            return packs;

        }
    }
}
