using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotKinematics
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int animationTime;
        public int AnimationTime
        {
            get { return animationTime; }
            set
            {
                if (value != animationTime)
                {
                    animationTime = value;
                    OnPropertyChanged("AnimationTime");
                }
            }
        }

        private double l1;
        public double L1
        {
            get { return l1; }
            set
            {
                if (value != l1)
                {
                    l1 = value;
                    robot.first.l = l1;
                    OnPropertyChanged("L1");
                }
            }
        }

        private double l2;
        public double L2
        {
            get { return l2; }
            set
            {
                if (value != l2)
                {
                    l2 = value;
                    robot.second.l = l2;
                    OnPropertyChanged("L2");
                }
            }
        }

        private bool editorMode;
        public bool EditorMode
        {
            get { return editorMode; }
            set
            {
                if (value != editorMode)
                {
                    editorMode = value;
                    OnPropertyChanged("EditorMode");
                }
            }
        }
    }
}
