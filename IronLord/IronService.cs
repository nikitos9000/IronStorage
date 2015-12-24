using System.ServiceProcess;

namespace IronLord
{
    public partial class IronService : ServiceBase
    {
        public IronService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}