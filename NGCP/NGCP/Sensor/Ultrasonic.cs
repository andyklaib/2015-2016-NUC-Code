using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGCP.Sensor
{
    public class Ultrasonic : Sensor<UltrasonicPackage>
    {
        /// <summary>
        /// distance of ultrasoni
        /// </summary>
        public double Distance;

        /// <summary>
        /// Update Ultrasonic data
        /// </summary>
        /// <param name="package"></param>
        public override void Update(UltrasonicPackage package)
        {
            Distance = package.DistanceData;
        }
    }

    public class UltrasonicPackage : SensorPackage
    {
        public double DistanceData;
    }
}
