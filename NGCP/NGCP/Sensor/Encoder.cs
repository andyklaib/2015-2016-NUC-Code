using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGCP.Sensor
{
    public class Encoder : Sensor<EncoderPackage>
    {
        public double scale;

        /// <summary>
        /// the total distance read by encoder
        /// </summary>
        public double displacement;
        /// <summary>
        /// velocity given by the first dirivative of the displacement
        /// </summary>
        public double velocity;
        /// <summary>
        /// acceleration given by the second dirivative of the displacment
        /// </summary>
        public double acceleration;


        public Encoder()
        {
            scale = 1;
            Clear();
        }

        public Encoder(double scale)
        {
            this.scale = scale;
            Clear();
        }
        public override void Update(EncoderPackage package)
        {
            displacement += package.displacement;

            double newVelocity = package.displacement / package.timelapce;

            double newAcceleration = (newVelocity - velocity) / package.timelapce;

            velocity = newVelocity;
            acceleration = newAcceleration;
        }

        /// <summary>
        /// Resets Encoder and clears all information
        /// </summary>
        public void Clear()
        {
            displacement = velocity = acceleration = 0;
        }
    }

    public class EncoderPackage : SensorPackage
    {
        public Int64 displacement; //ticks moved sence last update
        public double timelapce;  //time sence last update given in seconds
    }
}
