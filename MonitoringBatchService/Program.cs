using AF.DAL;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.AspNetCore.DiagnosticListener;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.EntityFrameworkCore;
using Elastic.Apm.NetCoreAll;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using MonitoringBatchService;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;
using MonitoringBatchService.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Listen(IPAddress.Loopback, 4323);
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
//builder.Configuration.AddXmlFile("App.config", optional: true, reloadOnChange: true);
var cfg = builder.Configuration;
builder.Services.AddSingleton<SharedUtils>();
_ = new SharedUtils(cfg);

builder.Services.AddDbContextFactory<GfccstgDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();

builder.Services.AddElasticApm(new HttpDiagnosticsSubscriber(), new EfCoreDiagnosticsSubscriber(), new AspNetCoreDiagnosticSubscriber());

//var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetExecutingAssembly());
//XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
builder.Services.AddLog4net();
//CONECTION
builder.Services.AddDbContextFactory<GfccWebDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectiongfcct")));
builder.Services.AddSingleton<DatabaseService>();


//Proses 1
builder.Services.AddScoped<IAutoGenerateJson,AutoGenerateJson>();
//Proses 2
builder.Services.AddScoped<ICopyFileService,CopyFileService>();
//Proses 3
//outbound
builder.Services.AddScoped<IServiceOutbound, ServiceOutbound>();
//Proses 4
builder.Services.AddScoped<IFileWatcherService, FileWatcherService>();
//Proses 5
builder.Services.AddScoped<IValidatejson, Validatejson>();
builder.Services.AddScoped<IChecking, ServiceinsertDrf>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddScoped<IProcessService,ProcessService>();
//builder.Services.AddHostedService<DatabaseMonitorService>();
//builder.Services.AddScoped<IDatabaseMonitorService, DatabaseMonitorService>();
builder.Services.AddHostedService<DatabaseMonitorService>();
// Add services to the container.
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo($"{builder.Environment.ContentRootPath}\\Log4Net.config"));
builder.Services.AddControllers();

var app = builder.Build();
// Konfigurasi Elastic APM
//app.UseElasticApm(builder.Configuration);
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
