using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MPP_ConcurrentLogger
{
    public class ByteConverter<T> : IObjectConverter<T>
    {       
        public T BytesToObject(byte[] bytesObj)
        {            
            if (bytesObj.Length == 0)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(bytesObj, 0, bytesObj.Length);
                memoryStream.Position = 0;
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        public byte[] ObjectToBytes(T obj)
        {
            if(obj == null)
            {
                return default(byte[]);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }         
        }

    }
}
