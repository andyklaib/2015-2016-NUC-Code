using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace NGCP.Sensor
{
    /*
    * This class pulls data off of the encoders through an arduino and calculates the 
    * degree at which the vehicle is turning and the distance traveled in the past 
    * 1/1000 of a second in mm, since mm/millisecond is the same as m/s  this value also
    * provides the current speed of the vehicle
    */
    public class Encoders : Sensor<EncodersPackage>
    {
        /// <summary>
        /// the speed measure by the encoders in m/s
        /// </summary>
        public double speed { get; private set; }
        /// <summary>
        /// The degree of turn measured by the encoders in radians left of forward
        /// </summary>
        public double turn { get; private set; }

        private double rightDistance;
        private double leftDistance;
        private const double CIRCUMFRANCE = 68 * 2 * Math.PI;//mm
        private const double WIDTH = 365;//mm


        public override void Update(EncodersPackage package)
        {
            leftDistance = package.left;
            rightDistance = package.right;

            //returns the average distance the vehicle has travelled in past 1/1000 of a second in mm
            speed = (rightDistance + leftDistance) * CIRCUMFRANCE / 2;


            //returns the angle the vehicle is turning at for the past 1/1000 of a second in rad
            //the value is positive when the vehicle is turning left and negitive when turning right
            turn = (rightDistance - leftDistance) / (WIDTH * 2);
        }
    }

    public class EncodersPackage : SensorPackage
    {
        public double left, right;
    }
}