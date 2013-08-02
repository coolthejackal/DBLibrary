using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using My_Library.Core.Domain;
using My_Library.Core.Services;
using My_Library.Data;
using My_Library.Services;

namespace My_Library.WinService
{
    partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }

        private Thread mainThread;

        protected override void OnStart(string[] args)
        {
            mainThread = new Thread(fnStart);
            mainThread.Start();
        }

        private void fnStart()
        {
            bool Action = true;

            WriteEventLog(EventLogEntryType.Warning, "App Start");

            while (true)
            {
                try
                {
                    if (ServiceRunning())
                    {
                        if (Action)
                        {
                            Departments departments = new Departments();
                            Department d = new Department { Name = "asd" };

                            departments.AddDepartment(d);
                            Common.IUnitOfWork.Commit();

                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                catch (Exception Ex)
                {
                    WriteEventLog(EventLogEntryType.Error, "1" + Ex.Message);
                }
                Thread.Sleep(5000);
            }

            WriteEventLog(EventLogEntryType.Warning, "Work End");
        }

        protected override void OnStop()
        {
            WriteEventLog(EventLogEntryType.Warning, "App Stop");
        }

        public class Departments
        {
            private readonly IDepartmentService _departmentService;

            public Departments()
            {
                if (_departmentService == null)
                    _departmentService = new DepartmentService(new EfRepository<Department>(Common.IUnitOfWork));
            }

            public void AddDepartment(Department department)
            {
                _departmentService.Add(department);
            }
        }

        public static bool ServiceRunning()
        {
            bool Status = true;
            try
            {
                var Service = new ServiceController("MyService");
                if (Service.Status == ServiceControllerStatus.StopPending ||
                    Service.Status == ServiceControllerStatus.Stopped)
                {
                    Status = false;
                }
            }
            catch (Exception Ex)
            {
                WriteEventLog(EventLogEntryType.Error, "2" + Ex.Message);
            }
            return Status;
        }

        public static void WriteEventLog(EventLogEntryType LogType, string Log)
        {
            try
            {
                string LogSource = "MyService";
                string LogName = "MyService";
                if (!EventLog.SourceExists(LogSource))
                {
                    EventLog.CreateEventSource(LogSource, LogName);
                }

                using (var eLog = new EventLog())
                {
                    eLog.Source = LogSource;
                    eLog.WriteEntry(Log, LogType);
                }
            }
            catch (Exception e)
            {
                //Recursive Call
                WriteEventLog(EventLogEntryType.Error, e.ToString());
            }
        }
    }
}
