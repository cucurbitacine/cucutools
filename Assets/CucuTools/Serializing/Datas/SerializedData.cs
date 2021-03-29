using System;
using System.Linq;
using UnityEngine;

namespace CucuTools.Serializing.Datas
{
    [Serializable]
    public abstract class SerializedData
    {
        public abstract int SizeOf();

        public abstract byte[] Serialize();

        public abstract void Deserialize(byte[] bytes);

        #region Common

        public static byte[] SerializeData(params SerializedData[] data)
        {
            var length = data.Sum(d => d.SizeOf()) + data.Length * sizeof(int);
            var bytes = new byte[length];
            
            var point = 0;
            foreach (var d in data)
                point = AddData(bytes, point, d);

            return bytes;
        }

        public static void DeserializeData(byte[] bytes, params SerializedData[] data)
        {
            var point = 0;
            for (var i = 0; i < data.Length; i++)
                point = GetData(bytes, point, ref data[i]);
        }
        
        public static int Copy(byte[] source, byte[] destination, int destinationIndex)
        {
            Array.Copy(source, 0, destination, destinationIndex, source.Length);

            return destinationIndex + source.Length;
        }

        public static byte[] GetArray(byte[] array, int index, int length)
        {
            var result = new byte[length];
            Array.Copy(array, index, result, 0, length);
            return result;
        }

        #endregion
        
        #region Serialize

        public static int AddString(byte[] array, int pointer, string value)
        {
            pointer = AddValue(array, pointer, value.Length);
            
            for (int i = 0; i < value.Length; i++)
                pointer = AddValue(array, pointer, value[i]);
            
            return pointer;
        }

        public static int AddData<T>(byte[] array, int pointer, T value) where T : SerializedData
        {
            pointer = AddValue(array, pointer, value.SizeOf());
            pointer = AddValue(array, pointer, value.Serialize());
            return pointer;
        }
        
        public static int AddValue(byte[] array, int pointer, byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
                pointer = AddValue(array, pointer, value[i]);

            return pointer;
        }
        
        public static int AddValue(byte[] array, int pointer, byte value)
        {
            return Copy(new[] {value}, array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, string value)
        {
            for (int i = 0; i < value.Length; i++)
                pointer = AddValue(array, pointer, value[i]);
            
            return pointer;
        }

        public static int AddValue(byte[] array, int pointer, Vector3 value)
        {
            pointer = AddValue(array, pointer, value.x);
            pointer = AddValue(array, pointer, value.y);
            pointer = AddValue(array, pointer, value.z);

            return pointer;
        }
        
        public static int AddValue(byte[] array, int pointer, Quaternion value)
        {
            pointer = AddValue(array, pointer, value.x);
            pointer = AddValue(array, pointer, value.y);
            pointer = AddValue(array, pointer, value.z);
            pointer = AddValue(array, pointer, value.w);

            return pointer;
        }

        public static int AddValue(byte[] array, int pointer, bool value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }

        public static int AddValue(byte[] array, int pointer, char value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, ushort value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, uint value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, ulong value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, short value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, int value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, long value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, float value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }
        
        public static int AddValue(byte[] array, int pointer, double value)
        {
            return Copy(BitConverter.GetBytes(value), array, pointer);
        }

        #endregion

        #region Deserialize

        public static int GetValue(byte[] array, int pointer, out string value, int length)
        {
            var stringArray = GetArray(array, pointer, length * sizeof(char));

            value = string.Empty;

            for (int i = 0; i < stringArray.Length; i++)
            {
                if (i % 2 == 1)
                {
                    value += BitConverter.ToChar(new[] {stringArray[i - 1], stringArray[i]}, 0);
                }
            }
            
            return pointer + sizeof(char) * value.Length;
        }
        
        public static int GetData<T>(byte[] array, int pointer, ref T value) where T : SerializedData
        {
            pointer = GetValue(array, pointer, out int length);
            value.Deserialize(GetArray(array, pointer, length));
            
            return pointer + length;
        }
        
        public static int GetValue(byte[] array, int pointer, out byte value)
        {
            value = array[pointer];
            return pointer + sizeof(byte);
        }

        public static int GetString(byte[] array, int pointer, out string value)
        {
            pointer = GetValue(array, pointer, out int length);
            return GetValue(array, pointer, out value, length);
        }

        public static int GetValue(byte[] array, int pointer, out Vector3 value)
        {
            value = Vector3.zero;

            pointer = GetValue(array, pointer, out value.x);
            pointer = GetValue(array, pointer, out value.y);
            pointer = GetValue(array, pointer, out value.z);

            return pointer;
        }
        
        public static int GetValue(byte[] array, int pointer, out Quaternion value)
        {
            value = Quaternion.identity;

            pointer = GetValue(array, pointer, out value.x);
            pointer = GetValue(array, pointer, out value.y);
            pointer = GetValue(array, pointer, out value.z);
            pointer = GetValue(array, pointer, out value.w);

            return pointer;
        }

        public static int GetValue(byte[] array, int pointer, out bool value)
        {
            value = BitConverter.ToBoolean(array, pointer);
            return pointer + sizeof(bool);
        }

        public static int GetValue(byte[] array, int pointer, out ushort value)
        {
            value = BitConverter.ToUInt16(array, pointer);
            return pointer + sizeof(ushort);
        }
        
        public static int GetValue(byte[] array, int pointer, out uint value)
        {
            value = BitConverter.ToUInt32(array, pointer);
            return pointer + sizeof(uint);
        }
        
        public static int GetValue(byte[] array, int pointer, out ulong value)
        {
            value = BitConverter.ToUInt64(array, pointer);
            return pointer + sizeof(ulong);
        }
        
        public static int GetValue(byte[] array, int pointer, out short value)
        {
            value = BitConverter.ToInt16(array, pointer);
            return pointer + sizeof(short);
        }
        
        public static int GetValue(byte[] array, int pointer, out int value)
        {
            value = BitConverter.ToInt32(array, pointer);
            return pointer + sizeof(int);
        }
        
        public static int GetValue(byte[] array, int pointer, out long value)
        {
            value = BitConverter.ToInt64(array, pointer);
            return pointer + sizeof(long);
        }
        
        public static int GetValue(byte[] array, int pointer, out float value)
        {
            value = BitConverter.ToSingle(array, pointer);
            return pointer + sizeof(float);
        }
        
        public static int GetValue(byte[] array, int pointer, out double value)
        {
            value = BitConverter.ToDouble(array, pointer);
            return pointer + sizeof(double);
        }

        #endregion
    }
}