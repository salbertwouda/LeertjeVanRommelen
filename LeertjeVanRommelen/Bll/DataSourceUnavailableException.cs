using System;
using System.Runtime.Serialization;

namespace LeertjeVanRommelen.Bll
{
    [Serializable]
    internal class DataSourceUnavailableException : Exception
    {
        public DataSourceUnavailableException()
        {}

        public DataSourceUnavailableException(string message)
            :base(message)
        {}

        public DataSourceUnavailableException(string message, Exception inner)
            :base(message, inner)
        {}

        protected DataSourceUnavailableException(SerializationInfo info, StreamingContext context)
        {}

        
        public DataSourceUnavailableException(Exception inner, string name)
            :base(string.Format("DataSource {0} is unavailable", name), inner)
        {}
    }
}