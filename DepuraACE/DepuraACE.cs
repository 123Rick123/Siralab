using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using DepuraACE.Logic;

namespace DepuraACE
{
    public partial class DepuraACE : ServiceBase
    {

        private int _eventId = 1;
        private int _eventError = 1;
        private DepuracionColasBL _depuracionColasBl = new DepuracionColasBL();
        private System.Timers.Timer _timer = new System.Timers.Timer { Interval = 86_400_000 }; // 24 horas  
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);


        
        public DepuraACE()
        {
            InitializeComponent();
            eventLog1 = new EventLog();

            if (!EventLog.SourceExists("SIRALAB_Depuracion"))
            {
                EventLog.CreateEventSource("SIRALAB_Depuracion", "Siralab_Depuracion");
            }

            eventLog1.Source = "SIRALAB_Depuracion";
            eventLog1.Log = "Siralab_Depuracion";
        }

        protected override void OnStart(string[] args)
        {
            var serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_START_PENDING,
                dwWaitHint = 100000
            };

            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("Se inicia el servicio");
            _timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            _timer.Start();

            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Se detiene el servicio");


            var serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_STOP_PENDING,
                dwWaitHint = 100000
            };
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnPause()
        {
            var serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_PAUSE_PENDING,
                dwWaitHint = 100000
            };


            _timer.Stop();
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("Se pausa el servicio");
        }

        protected override void OnContinue()
        {
            var serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_CONTINUE_PENDING,
                dwWaitHint = 100000
            };

            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            _timer = new System.Timers.Timer { Interval = 86_400_000 };
            _timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            _timer.Start();
            
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("Finaliza la pausa del servicio");
        }

        protected override void OnShutdown()
        {
            eventLog1.WriteEntry("Se apaga el servicio");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            try
            {
                _depuracionColasBl.Depurar();
                eventLog1.WriteEntry("Colas depuradas", EventLogEntryType.Information, _eventId++);
            }
            catch (Exception e)
            {
                eventLog1.WriteEntry(e.InnerException?.ToString(), EventLogEntryType.Error, _eventError++);
            }

        }
    }

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
}
