using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Threading;

namespace SolarSystem
{
    public class Earth : SphereGeometry3D
    {
        public Earth()
        {
            Radius = 4;
            NumberOfSegments = 50;
        }
    }

    public class Moon : SphereGeometry3D
    {
        public Moon()
        {
            Radius = 1;
            NumberOfSegments = 50;
        }
    }

    public class Sun : SphereGeometry3D
    {
        public Sun()
        {
            Radius = 5;
            NumberOfSegments = 50;
        }
    }

    public class OrbitsCalculator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        const double EarthYear = 365.25;
        const double EarthDay = 1.0;
        const double SunDay = 25.0;
        const double MoonDay = 27.322;
        const double MoonYear = 27.322;

        private double _DaysPerSecond;
        private double _EarthOrbitRadius;
        private double _MoonOrbitRadius;
        private double _Days;
        private double _EarthRotationAngle;
        private double _SunRotationAngle;
        private double _MoonRotationAngle;
        private double _EarthOrbitPositionX;
        private double _EarthOrbitPositionY;
        private double _EarthOrbitPositionZ;
        private double _MoonOrbitPositionX;
        private double _MoonOrbitPositionY;
        private double _MoonOrbitPositionZ;
        private bool _Paused;
        private bool _ReversedTime;
        private DateTime _StartTime;
        private DispatcherTimer _Timer;

        public bool ReversedTime
        {
            get { return _ReversedTime; }
            set { _ReversedTime = value; }
        }

        public bool Paused
        {
            get { return _Paused; }
            set { _Paused = value; }
        }

        public double EarthOrbitPositionZ
        {
            get { return _EarthOrbitPositionZ; }
            set { _EarthOrbitPositionZ = value; }
        }

        public double EarthOrbitPositionY
        {
            get { return _EarthOrbitPositionY; }
            set { _EarthOrbitPositionY = value; }
        }

        public double EarthOrbitPositionX
        {
            get { return _EarthOrbitPositionX; }
            set { _EarthOrbitPositionX = value; }
        }

        public double MoonOrbitPositionZ
        {
            get { return _MoonOrbitPositionZ; }
            set { _MoonOrbitPositionZ = value; }
        }

        public double MoonOrbitPositionY
        {
            get { return _MoonOrbitPositionY; }
            set { _MoonOrbitPositionY = value; }
        }

        public double MoonOrbitPositionX
        {
            get { return _MoonOrbitPositionX; }
            set { _MoonOrbitPositionX = value; }
        }

        public double EarthRotationAngle
        {
            get { return _EarthRotationAngle; }
            set { _EarthRotationAngle = value; }
        }

        public double SunRotationAngle
        {
            get { return _SunRotationAngle; }
            set { _SunRotationAngle = value; }
        }

        public double MoonRotationAngle
        {
            get { return _MoonRotationAngle; }
            set { _MoonRotationAngle = value; }
        }

        public double Days
        {
            get { return _Days; }
            set { _Days = value; }
        }

        public double EarthOrbitRadius
        {
            get { return _EarthOrbitRadius; }
            set { _EarthOrbitRadius = value; }
        }

        public double MoonOrbitRadius
        {
            get { return _MoonOrbitRadius; }
            set { _MoonOrbitRadius = value; }
        }

        public double DaysPerSecond
        {
            get { return _DaysPerSecond; }
            set 
            { 
                _DaysPerSecond = value;
                Notify("DaysPerSecond");
            }
        }

        public OrbitsCalculator()
        {
            DaysPerSecond = 1;
            EarthOrbitRadius = 40;
            MoonOrbitRadius = 10;
            EarthOrbitPositionX = EarthOrbitRadius;
            MoonOrbitPositionX = EarthOrbitPositionX + MoonOrbitRadius;
        }

        public void StartTimer()
        {
            _StartTime = DateTime.Now;
            _Timer = new DispatcherTimer();
            _Timer.Interval = TimeSpan.FromMilliseconds(10);
            _Timer.Tick +=_Timer_Tick;
            _Timer.Start();
        }

        private void StopTimer()
        {
            _Timer.Stop();
            _Timer.Tick -= _Timer_Tick;
            _Timer = null;
        }

        public void ReverseTime()
        {
            ReversedTime = !ReversedTime;
        }

        public void PauseTimer()
        {
            if (!Paused)
            {
                StopTimer();
                Paused = true;
            }
            else
            {
                StartTimer();
                Paused = false;
            }
        }

        void _Timer_Tick(object sender, EventArgs e)
        {
 	        DateTime now = DateTime.Now;
            Days += (now-_StartTime).TotalMilliseconds * DaysPerSecond / 1000.0 * (ReversedTime ? -1 : 1);
            _StartTime = now;
            Notify("Days");
            OnTimeChanged();   
        }

        private void OnTimeChanged()
        {
            EarthPosition();
            MoonPosition();
            EarthRotation();
            SunRotation();
            MoonRotation();
        }

        private void EarthPosition()
        {
            double angle = 2 * Math.PI * Days / EarthYear;
            EarthOrbitPositionX = EarthOrbitRadius * Math.Cos(angle);
            EarthOrbitPositionY = EarthOrbitRadius * Math.Sin(angle);
            Notify("EarthOrbitPositionX");
            Notify("EarthOrbitPositionY");
        }

        private void MoonPosition()
        {
            double angle = 2 * Math.PI * Days / MoonYear;
            MoonOrbitPositionX = EarthOrbitPositionX + MoonOrbitRadius * Math.Cos(angle);
            MoonOrbitPositionY = EarthOrbitPositionY + MoonOrbitRadius * Math.Sin(angle);
            Notify("MoonOrbitPositionX");
            Notify("MoonOrbitPositionY");
        }

        private void EarthRotation()
        {
            EarthRotationAngle = 360 * Days / EarthDay;
            Notify("EarthRotationAngle");
        }

        private void SunRotation()
        {
            SunRotationAngle = 360 * Days / SunDay;
            Notify("SunRotationAngle");
        }

        private void MoonRotation()
        {
            MoonRotationAngle = 360 * Days / MoonDay;
            Notify("MoonRotationAngle");
        }

        private void Notify(String property)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
