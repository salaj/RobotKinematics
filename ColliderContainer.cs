using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace RobotKinematics
{
    public class ColliderContainer
    {
        public IList<ExtendedRectangle> Colliders;
        public bool[,] anglesArray { get; set; }
        private int N = 3;
        int maxAngle = 360;
        private Random rand;
        public ColliderContainer()
        {
            Colliders = new List<ExtendedRectangle>();
            rand = new Random();
            anglesArray = new bool[maxAngle, maxAngle];
        }
        public void GenerateColliders(Canvas canvas)
        {
            double left, right, top, bottom, width, height;
            for (int i = 0; i < N; i++)
            {
                width = rand.Next(200);
                height = rand.Next(200);
                right = bottom = 0;
                left = rand.Next(300);
                top = rand.Next(300);
                var extendedRectangle = new ExtendedRectangle(width, height, left, top, right, bottom);
                Colliders.Add(extendedRectangle);
                canvas.Children.Add(extendedRectangle.rectangle);
            }
        }

        public void GenerateAnglesArray(Robot robot, Canvas Canvas, SegmentsIntersector segmentsIntersector)
        {
            int positiveCounter = 0;
            for (int i = 0; i < maxAngle; i++)
                for (int j = 0; j < maxAngle; j++)
                {
                    robot.UpdateAngles(i, j);
                    robot.Reset(Canvas);
                    Line firstLine = robot.GetLines()[0];
                    Line secondLine = robot.GetLines()[1];
                    bool sentinel = false;
                    foreach (var extendedRectangle in Colliders)
                    {
                        foreach (var line in extendedRectangle.GetLines())
                        {
                            bool intersectionWithFirst = segmentsIntersector.Intersect(firstLine, line);
                            bool intersectionWithSecond = segmentsIntersector.Intersect(secondLine, line);

                            if (intersectionWithFirst || intersectionWithSecond)
                            {
                                sentinel = true;
                                break;
                            }
                        }
                        if (sentinel)
                        {
                            break;
                        }
                    }
                    if (sentinel)
                    {
                        anglesArray[i, j] = false;
                    }
                    else
                    {
                        anglesArray[i, j] = true;
                        positiveCounter++;
                        //Canvas.Children.Add(firstLine);
                        //Canvas.Children.Add(secondLine);
                    }
                }
            int d = 10;
        }
    }
}
