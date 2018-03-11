namespace RestFul.Loggers
{
    class NullLogger : IRestfulLogger
    {
        public void Debug(string format, params object[] args) { }

        public void Information(string format, params object[] args) { }

        public void Warning(string format, params object[] args) { }

        public void Error(string format, params object[] args) { }
    }
}
