using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Telemetry
{
    public class ActivitySourceProvider
    {
        public static ActivitySource Source = new ActivitySource("EduPortal.Application.Service.Source", "1.00");
    }
}
