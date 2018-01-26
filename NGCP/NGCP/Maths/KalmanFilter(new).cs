using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalman_Filter
{
    public class KalmanFilter
    {
        // Inputs
       Matrix u; // Control vector
       Matrix z; // Measurement vector

        // Outputs
       Matrix x; // New state
       Matrix P; // New error

       Matrix y; // State comparison

       Matrix Xpred; // State prediction
       Matrix Ppred; // Error prediction

       Matrix A; // State transition matrix
       Matrix B; // Control matrix
       Matrix S; // Error comparison
       Matrix I; // Identity matrix
       Matrix H; // Observation matrix
       Matrix Q; // Process error
       Matrix R; // Measurement error

       Matrix K; // Kalman gain

       Matrix dT; // Delta Time matrix

       // Takes an angle in degrees and returns them in radians
       double getRadians(double degrees)
       {
           return degrees * Constants.DEGREES_TO_RADIANS;
       }
       // Takes an angle in radians and returns them in degrees
       double getDegrees(double radians)
       {
           return radians * Constants.RADIANS_TO_DEGREES;
       }

       // Takes in the three components of a vector and returns the magnitude
       double getVectorSize(double x, double y, double z)
       {
           return (Math.Sqrt(x * x + y * y + z * z));
       }
       /** Initialize all our matrices
* @param stateVector - Vehicle's position and velocity in the world
*/
       void initializeKalmanFilter(/*stateVector class*/)
       {
           u = new Matrix(Constants.STATE_ROW, Constants.STATE_COLUMN);
           z = new Matrix(Constants.STATE_ROW, Constants.STATE_COLUMN);

           x = new Matrix(Constants.STATE_ROW, Constants.STATE_COLUMN);
           P = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           y = new Matrix(Constants.STATE_ROW, Constants.STATE_COLUMN);

           Xpred = new Matrix(Constants.STATE_ROW, Constants.STATE_COLUMN);
           Ppred = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           A = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);
           B = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);
           S = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           I = new Matrix(Constants.OP_ROW, Constants.OP_ROW);
           I = I.identity();

           H = new Matrix(Constants.OP_ROW, Constants.OP_ROW);
           H = H.identity();

           Q = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);
           R = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           K = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           dT = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           initializeState(stateVector);
           initializeProcessErrorMatrix();
           initializeMeasurementErrorMatrix();
       }
        /** Initializes the state with the current position and velocity
        * @param stateVector - Vehicle's position and velocity in the world
        */
       void initializeState(/*stateVector class*/)
       {
           x.Set(0,0,stateVector.X);
           x.Set(1,0,stateVector.Y);
           x.Set(2,0,stateVector.Z);

           x.Set(3,0,stateVector.X_DOT);
           x.Set(4,0,stateVector.Y_DOT);
           x.Set(5,0,stateVector.Z_DOT);
       }
       /** Initialize the process error matrix Q */
       void initializeProcessErrorMatrix()
       {
           // Going to calculate this while we run the filter
       }

       /** Initialize the measurement error R */
       void initializeMeasurementErrorMatrix()
       {
           for (int i = 0; i < Constants.OP_ROW; i++)
               R.Set(i,i,Constants.GPS_NOISE_COVARIANCE * Constants.GPS_NOISE_COVARIANCE);
       }
       /** Sets the change in time since the last predict / update phase. Used for
        * converting the predicted velocity and acceleration into a predicted
        * position and velocity
        * @param deltaTime - Time in seconds since the last predict / update call
        */
       void setDeltaTime(double deltaTime)
       {
           for (int i = 0; i < Constants.OP_ROW; i++)
               dT.Set(i,i, deltaTime);
       }

       /** Updates the values of the state transition matrix A
        * @param attitude - Vehicle's rotation and rotational rate
        */
       void calcStateTransitionValues(/*attitude class*/, double deltaTime)
       {
           for (int i = 0; i < Constants.OP_ROW; i++)
               A.Set(i,i,1.0);

           A.Set(0, 3, deltaTime);
           A.Set(1, 4, deltaTime);
           A.Set(2, 5, deltaTime);

       }
       /** Updates the values of the control matrix
        * @param attitude - Vehicle's rotation and rotational rate
        */
       void calcControlValues(/*attitude class*/, double deltaTime)
       {
           B.Set(0, 0, (deltaTime * deltaTime) / 2.0);
           B.Set(1, 1, (deltaTime * deltaTime) / 2.0);
           B.Set(2, 2, (deltaTime * deltaTime) / 2.0);

           B.Set(3, 3, deltaTime);
           B.Set(4, 4, deltaTime);
           B.Set(5, 5, deltaTime);

       }

       /** Updates the control vector matrix with fresh values from the IMU
        * @param stateVector - Vehicle's position and velocity in the world
        */
       void setControlVectorValues(/*stateVector class*/)
       {
           u.Set(0, 0, stateVector.X_DDOT);
           u.Set(1, 0, stateVector.Y_DDOT);
           u.Set(2, 0, stateVector.Z_DDOT);

           u.Set(3, 0, stateVector.X_DDOT);
           u.Set(4, 0, stateVector.Y_DDOT);
           u.Set(5, 0, stateVector.Z_DDOT);

       }
       /** Updates the covariance matrix Q with the new process error */
       void calcEstimatedProcessError(/*stateVector*/)
       {
           double acceleration = getVectorSize(stateVector.X_DDOT, stateVector.Y_DDOT, stateVector.Z_DDOT);
           Matrix BT = B.transpose();

           Q = B*BT;
           Q = Q * acceleration * acceleration;
       }

       /** Updates the covariance matrix R with the new measurement error */
       void calcEstimatedMeasurementError()
       {
           // TODO: Implement better R matrix here as opposed to a identity matrix
       }
       /** Updates the measurement vector with new values from the GPS and IMU
        * @param stateVector - Vehicle's position and velocity in the world
        */
       void setMeasurementVectorValues(/*stateVector class*/)
       {

           // Set the X, Y, and Z positions from the GPS
           z.Set(0, 0, stateVector.X);
           z.Set(1, 0, stateVector.Y);
           z.Set(2, 0, stateVector.Z);


           // Set the X, Y, and Z translational position from the IMU
           z.Set(3, 0, stateVector.X_DOT);
           z.Set(4, 0, stateVector.Y_DOT);
           z.Set(5, 0, stateVector.Z_DOT);

       }
       /** Perform the prediction phase using
        * @param stateVector - Vehicle's position and velocity in the world
        * @param attitude - Vehicle's rotation and rotational rate
        * @param deltaTime - Time in seconds since the last predict / update phase
        */
       void predict(/*stateVector*/, /*attitude*/, double deltaTime)
       {

           // Calculate values for A, B, u, Q, and dT
           calcStateTransitionValues(attitude, deltaTime);
           calcControlValues(attitude, deltaTime);
           setControlVectorValues(stateVector);
           calcEstimatedProcessError(stateVector);

           // State prediction
           Matrix Ax = new Matrix(Constants.OP_ROW, Constants.STATE_COLUMN);
           Matrix Bu = new Matrix(Constants.OP_ROW, Constants.STATE_COLUMN);

           Ax = A * x;
           Bu = B * u;
           
           Xpred = Ax + Bu;

           // Error prediction
           Matrix AT = A.transpose();
           Matrix AP = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);
           Matrix APAT = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           AP = A * P;
           APAT = AP * AT;
           Ppred = APAT + Q;
       }
       /** Performs the update phase
        * @param stateVector - Vehicle's position and velocity in the world
        */
       void update(/*stateVector*/)
       {

           // Fetch and Calculate values for z and R
           setMeasurementVectorValues(stateVector);
           //calcEstimatedMeasurementError();

           // Comparison of state
           Matrix HXpred = new Matrix(Constants.OP_ROW, Constants.STATE_COLUMN);

           HXpred = H * Xpred;
           y = z - HXpred;

           // Comparison of error
           Matrix HT = H.transpose();
           Matrix HPpred = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);
           Matrix HPpredHT = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           HPpred = H * Ppred;
           HPpredHT = HPpred * HT;
           S = HPpredHT + R;

           // Kalman gain
           Matrix SInverse = S.inverse();
           Matrix PpredHT = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           PpredHT = Ppred * HT;
           K = PpredHT * SInverse;
           
           // State update
           Matrix Ky = new Matrix(Constants.OP_ROW, Constants.STATE_COLUMN);

           Ky = K * y;
           x = Xpred + Ky;

           // Error update
           Matrix KH = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);
           Matrix I_minus_KH = new Matrix(Constants.OP_ROW, Constants.OP_COLUMN);

           KH = K * H;
           I_minus_KH = I - KH;
           P = I_minus_KH * Ppred;
       }
    }
}
