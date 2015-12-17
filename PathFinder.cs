using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RobotKinematics
{
    public class PathFinder
    {
        public IList<Point> path { get; set; }

        private ColliderContainer colliderContainer;
        private ConfigurationSpace configurationSpace;
        private Robot robot;
        private LinearInterpolator linearInterpolator;
        private DispatcherTimer timer;

        public PathFinder(ColliderContainer colliderContainer, ConfigurationSpace configurationSpace, Robot robot, LinearInterpolator linearInterpolator, DispatcherTimer timer)
        {
            this.colliderContainer = colliderContainer;
            this.configurationSpace = configurationSpace;
            this.robot = robot;
            this.linearInterpolator = linearInterpolator;
            this.timer = timer;
        }

        public void MoveRobot(Point start, Point end)
        {
            //ShowSelectedPosition(start);
            //ShowSelectedPosition(end);


            Point startAngles = robot.CalculateInverseKinematicsFirst(start.X, start.Y);
            Point endAngles = robot.CalculateInverseKinematicsSecond(end.X, end.Y);

            //Convert from radians to euler
            startAngles.X *= 180.0f / Math.PI;
            startAngles.Y *= 180.0f / Math.PI;
            endAngles.X *= 180.0f / Math.PI;
            endAngles.Y *= 180.0f / Math.PI;

            //transform angles to span from 0 to 360
            startAngles.X = (int)(startAngles.X + 360) % 360;
            startAngles.Y = (int)(startAngles.Y + 360) % 360;
            endAngles.X = (int)(endAngles.X + 360) % 360;
            endAngles.Y = (int)(endAngles.Y + 360) % 360;

            configurationSpace.Reset();
            configurationSpace.MarkUnreachableCells(colliderContainer.anglesArray);

            bool success = configurationSpace.FloodFill(startAngles, endAngles);
            if (success)
            {
                configurationSpace.ColorFill();
                path = configurationSpace.TraverseBack(startAngles, endAngles);
                configurationSpace.Update();
                linearInterpolator.Setup(path);
                linearInterpolator.startTime = DateTime.Now;
                timer.Start();
            }
            else
            {
                MessageBox.Show("Chosen points cannot be reached by robot.");
            }
        }
    }
}
