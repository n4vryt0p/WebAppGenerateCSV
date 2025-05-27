﻿using log4net;
using log4net.Config;

namespace MonitoringBatchService.Models
{
    public static class Log4NetExtensions
    {
        public static void AddLog4net(this IServiceCollection services)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        }
    }
}
