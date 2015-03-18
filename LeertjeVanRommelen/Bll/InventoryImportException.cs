using System;
using System.Runtime.Serialization;

namespace LeertjeVanRommelen.Bll
{
    [Serializable]
    internal class InventoryImportException : Exception
    {
        public InventoryImportException()
        { }

        public InventoryImportException(string message)
            : base(message)
        { }

        public InventoryImportException(string message, Exception inner)
            : base(message, inner)
        { }

        protected InventoryImportException(SerializationInfo info, StreamingContext context)
        { }
    }
}