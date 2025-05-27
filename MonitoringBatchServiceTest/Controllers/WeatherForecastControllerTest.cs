using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Controllers;

namespace MonitoringBatchServiceTest.Controllers
{
    public class WeatherForecastControllerTest
    {
        [Fact]
        public void Getshouldretruns()
        {
            var controller = new WeatherForecastController();
            var result = controller.Get().ToList();
            Assert.NotNull(result);
        }

     
    }
}
