namespace LeertjeVanRommelen
{
    interface ILog
    {
        void Info(string message, params object[] args);
        void Error(string message, params object[] args);
    }
}