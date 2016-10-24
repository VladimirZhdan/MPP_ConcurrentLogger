namespace MPP_ConcurrentLogger
{
    public interface IObjectConverter<T>
    {
        T BytesToObject(byte[] obj);
        byte[] ObjectToBytes(T obj);        
    }
}
