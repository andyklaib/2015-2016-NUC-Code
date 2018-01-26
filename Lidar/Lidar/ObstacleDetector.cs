using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObstacleDetection
{
    class ObstacleDetector
    {
        private List<double> stepDistances;
        private List<Vector2d> vectors;
        private int startStep;

        //Objects that are farther than the MAX_DISTANCE
        //Will not be considered when forming an obstacle 
        private double maxDistance;

        public ObstacleDetector(int startingStep, List<double> distanceList, int maxDistance)
        {
            this.maxDistance = maxDistance;
            stepDistances = distanceList;
            startStep = startingStep;
            vectors = new List<Vector2d>();
        }

        /**
        * Takes a list of distance vectors and creates
        * calculates the vectorSum 
        **/
        public VectorSum getVectorSum()
        {
            Vector2d sum = new Vector2d(0, 0);
            int count = 0;
            int step;
            for (int i = 0; i < stepDistances.Count; i++)
            {
                step = i + startStep;

                //Ignore Data that is greater than maxDistance or is infinity
                if (stepDistances[i] * 0.001 <= maxDistance && stepDistances[i] != 1)
                {
                    double magnitude = stepDistances[i] * .001;
                    double angle = stepToRadians(step);
                    Vector2d newVector = new Vector2d(1 / magnitude, angle);

                    sum += newVector;
                    count++;
                }

            }
            if (count > 0)
            {
                sum.angle /= count;
                sum.magnitude /= count;
            }
            return new VectorSum(count, sum.getAngle(), sum.getMagnitude());
        }

        //Set the max distance in milimieters
        public void setMaxDistance(double newMax)
        {
            maxDistance = newMax;
        }

        /**
        * Converts a step from the lidar into a degree.
        * Note that each step on the the Hokuyo UTM-30LX lidar
        * is separated by .25 degrees. The lidar has a 270 degree
        * range of detection. Step 0 occurs at angle 225 degrees.  
        **/
        private double stepToRadians(int step)
        {
            double startAngle = 225;
            double angle = startAngle + step * .25f;
            if (angle >= 360)
            {
                angle = angle - 360;
            }
            return angle * Math.PI / 180;
        }
    }
}
