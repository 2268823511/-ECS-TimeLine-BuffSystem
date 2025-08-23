namespace SingleTonSpace
{
    public class SingleTon<T> where T : class, new()
    {
        private static T instance = new T();

        public static T getInstance()
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }
}