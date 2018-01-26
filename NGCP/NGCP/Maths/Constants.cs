using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalman_Filter
{
    static public class Constants
    {
        public const int STATE_ROW = 6;
        public const int STATE_COLUMN = 1;

        public const int OP_ROW = 6;
        public const int OP_COLUMN = 6;

        public const double PI = 3.1415926535898;

        public const double GRAVITY = -9.807;
        public const double GPS_NOISE_COVARIANCE = 2.55;        //still need to figure out

        public const double RADIANS_TO_DEGREES = 180.0 / PI;
        public const double DEGREES_TO_RADIANS = PI / 180.0;

    }
}
