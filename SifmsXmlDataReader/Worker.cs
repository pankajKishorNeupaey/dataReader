/*
 *Author:Pankaj Kishor Neupaney
 * Created date:07-jul-2020
 * Desc:windows servisces for xml
*/
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SifmsXmlDataReader.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XmlConversion;

namespace SifmsXmlDataReader
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IConfiguration _configuration;
        private int _runIntervallInHours;
        public string _filePath;
        public int mailPort;
        public string mailServer;
        public string senderName;
        public string sender;
        public string password;
        public string getreciverEmail;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
          
                _configuration = _serviceScopeFactory.CreateScope().
                                 ServiceProvider.GetRequiredService<IConfiguration>();
                //this variable hold the value for the service Runtime Intervals
                //which is set in the appsetting.json file in root of the project
                _runIntervallInHours = int.Parse(_configuration
                                                ["App.Configurations:RunIntervallInHours"]);
            //this variable hold the value for the File Path From where the  Xml is supposed to be read
            //which is set in the appsetting.json file in root of the project
            _filePath = (_configuration["App.Configurations:ConfigurationFilePath"]).ToString();
            //the variable below holds the value for the Mail setting and the reciver 
            //which is set in the appsetting.json file in root of the project
            /*------------------------------Start---------------------------------*/
            mailPort = int.Parse(_configuration
                                            ["EmailSettings:MailPort"]);
            mailServer = (_configuration
                                            ["EmailSettings:MailServer"]).ToString();
            senderName = (_configuration
                                           ["EmailSettings:SenderName"]).ToString();
            sender = (_configuration
                                        ["EmailSettings:Sender"]).ToString();
            password = (_configuration
                              ["EmailSettings:Password"]).ToString();
            getreciverEmail = (_configuration
                              ["ReciverEmail"]).ToString();
            /*------------------------------End---------------------------------*/

            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
             while (!stoppingToken.IsCancellationRequested)
                {


                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //this variable hold the value for the File Path From where the  Xml is supposed to be read
                //which is set in the appsetting.json file in root of the project
                _filePath = (_configuration["App.Configurations:ConfigurationFilePath"]).ToString();
                //the variable below holds the value for the Mail setting and the reciver 
                //which is set in the appsetting.json file in root of the project
                /*------------------------------Start---------------------------------*/
                mailPort = int.Parse(_configuration
                                                ["EmailSettings:MailPort"]);
                mailServer = (_configuration
                                                ["EmailSettings:MailServer"]).ToString();
                senderName = (_configuration
                                               ["EmailSettings:SenderName"]).ToString();
                sender = (_configuration
                                            ["EmailSettings:Sender"]).ToString();
                password = (_configuration
                                  ["EmailSettings:Password"]).ToString();
                getreciverEmail = (_configuration
                                  ["ReciverEmail"]).ToString();
                /*------------------------------End---------------------------------*/
                string[] reciverEmail = getreciverEmail.Split(",");
                    EmailSettings emailSettings = new EmailSettings()
                    {
                        MailPort = mailPort,
                        MailServer = mailServer,
                        SenderName = senderName,
                        Sender = sender,
                        Password = password
                    };
                    List<EmailAddress> emailAddresses = new List<EmailAddress>();
                    for (int i = 0; i < reciverEmail.Length; i++)
                        emailAddresses.Add(new EmailAddress
                        {
                            emailAddress = reciverEmail[i]

                        });

                    DateTime dt = DateTime.Now;
                    
                    string path = dt.AddDays(-3).ToString("ddMMyyy");
                    string xmlFilePath = _filePath + "/EXP" + path + ".xml";
                    XmlRecordTracker tracker = new XmlRecordTracker();

                    int insertedStatus = tracker.GetXmlInsertRecordForCurrentDate();
                    // Usage
                    if (insertedStatus == 0)
                    {
                        insertXmlData xml = new insertXmlData();
                        int StatusCode = xml.StoreExpenditureXmlToDB(xmlFilePath, emailSettings);

                        Tracker data = new Tracker
                        {
                            executionTime = dt.ToString("HH:mm:ss"),
                            executionDate = dt.ToString("yyyy-MM-dd"),
                            dataType = "expenditure",
                            status = Convert.ToBoolean(StatusCode),
                        };
                        tracker.InsertXmlInsertedRecords(data);
                    Console.WriteLine("{0} ({1})", (int)insertedStatus, insertedStatus);
                    await Task.Delay(TimeSpan.FromHours(_runIntervallInHours), stoppingToken);
                    }
                Console.WriteLine("{0} ({1})", (int)insertedStatus, insertedStatus);
                await Task.Delay(TimeSpan.FromHours(_runIntervallInHours), stoppingToken);
             }     
        }

    }

}
