namespace StudentCrudAppWithEFCoreCodeFirst.Services
{
    public class AppLogger:IAppLogger //Singleton Service
    {
        private readonly List<string> _logs = new List<string>();
        public void Log(string message) 
        {
            _logs.Add(message);
            Console.WriteLine($"Log : {message}");
        }
    }
}
