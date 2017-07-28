using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace NJ.WindowsService.WSExemplo
{
    public partial class Service : ServiceBase
    {
        #region Propriedades

        private Timer _timer;
        private bool _executando;
        const int IntervaloExecucaoMinutos = 5;

        #endregion

        #region Construtor

        public Service()
        {
            InitializeComponent();
        }

        #endregion

        #region Métodos

        private static void Main()
        {
#if (DEBUG)
            var service = new Service();
            service.Processar();
            Thread.Sleep(Timeout.Infinite);
#else
            var servicesToRun = new ServiceBase[] { new Service() };
            Run(servicesToRun);
#endif
        }

        private void timer_ElapsedWindowsService(object sender, ElapsedEventArgs args)
        {
            if (_executando) return;

            _executando = true;
            Processar();
            _executando = false;
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(IntervaloExecucaoMinutos * 60000) { AutoReset = true };
            _timer.Elapsed += timer_ElapsedWindowsService;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer = null;
            _executando = false;
            base.OnStop();
        }

        protected override void OnPause()
        {
            _timer.Enabled = false;
            _executando = false;
            base.OnPause();
        }

        protected override void OnContinue()
        {
            if (!_executando)
            {
                _executando = true;
                Processar();
                _executando = false;
            }

            _timer.Enabled = true;
            base.OnContinue();
        }

        protected override void OnShutdown()
        {
            _timer.Enabled = false;
            _executando = false;
            base.OnShutdown();
        }
        
        public void Processar()
        {
            // implementação da rotina a ser executada.
        }

        #endregion
    }

}
