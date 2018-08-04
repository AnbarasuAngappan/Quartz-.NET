using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz.Impl;

namespace Quartz.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            JobSchedular jobScheduler = new JobSchedular();
            jobScheduler.Start();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Scheduler Started..");
            Console.ResetColor();
            Console.ReadLine();         
        }
    }

    public class JobSchedular
    {
        public async void Start()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();       
            IJobDetail job = JobBuilder.Create<Jobclass>().Build();

            #region
            //ITrigger trigger = TriggerBuilder.Create()
            //.WithIdentity("trigger1", "group1")
            //.StartNow()
            //.WithSimpleSchedule(x => x
            //.WithIntervalInSeconds(10)
            //.RepeatForever())
            //.Build();
            #endregion

            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(a => a.WithIntervalInSeconds(2).WithRepeatCount(2))
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();

            //Thread.Sleep(TimeSpan.FromSeconds(10));
            //await scheduler.Shutdown();

        }
    }

    public class Jobclass: IJob
    {
       
        Task IJob.Execute(IJobExecutionContext context)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(String.Format("{0,-10} | {1,-10} | {2,5}", Environment.UserName, "Welcome To the Ibot family", Environment.UserDomainName));
            //Console.WriteLine(string.Format("{0,10}",Environment.UserName, "Welcome To the Ibot family"));// Environment.UserName + "Welcome To the Ibot family");
            if(context == null)
            {
                throw new NotImplementedException();
            }
            return null;            
        }
    }

    public class JobclassMail : IJob
    {      

        Task IJob.Execute(IJobExecutionContext context)
        {
            using (var message = new MailMessage("kanbuarasu27@gmail.com", "sailendravolety@gmail.com"))
            {
                message.Subject = "Quartz Schedular";
                message.Body = "Started Time" + DateTime.Now;
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("kanbuarasu27@gmail.com", "tomjerrysai27")
                })
                {
                    client.Send(message);
                }
            }

            if (context == null)
            {
                throw new NotImplementedException();
            }
            return null;
        }
    }
}
