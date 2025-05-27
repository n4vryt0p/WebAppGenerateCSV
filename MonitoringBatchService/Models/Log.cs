using log4net;
using System.Reflection;

namespace MonitoringBatchService.Models
{
    public interface IlogInternal
    {
        void Debug(string msg);
        void Info(string msg);
        void Error(string msg, Exception? ex = null);
    }
    public class Logger : IlogInternal
    {

        private readonly ILog _logger;
        public Logger(ILog logger)
        {
     
            _logger = logger;
            
        }
        public void Debug(string msg)
        {
            _logger.Debug(msg);
        }
        public void Info(string msg)
        {
            _logger.Info(msg);
        }
        public void Error(string msg, Exception? ex = null)
        {
            _logger?.Error(msg, ex);
        }
    }
}
