namespace MPP_ConcurrentLogger
{
    public class ObjectConverter<T>
    {
        private static IObjectConverter<T> byteConverter = new ByteConverter<T>();

        public static IObjectConverter<T> ByteConverter
        {
            get
            {
                return byteConverter;
            }
        }
    }
}
