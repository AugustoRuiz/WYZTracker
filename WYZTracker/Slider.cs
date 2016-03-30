using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public partial class Slider : Panel, INotifyPropertyChanged
    {
        private int _sliderPos;

        public Slider()
        {
            this.initialize();
            InitializeComponent();
        }

        public Slider(IContainer container)
        {
            this.initialize();
            container.Add(this);
            InitializeComponent();
        }

        private void initialize()
        {
            this._minimum = 0;
            this._maximum = 100;
            this._logarithmic = false;
        }

        private Orientation _orientation;
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                if (value != _orientation)
                {
                    _orientation = value;
                    this.OnPropertyChanged("Orientation");
                    this.Invalidate();
                }
            }
        }

        private int _minimum;
        private double _minLogValue;
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (value != _minimum)
                {
                    _minimum = value;
                    if (_minimum > 0)
                    {
                        _minLogValue = Math.Log(_minimum);
                    }
                    else
                    {
                        _minLogValue = Math.Log(0.0001);
                    }
                    this.clampValue();
                    this.OnPropertyChanged("Minimum");
                    this.Invalidate();
                }
            }
        }

        private int _maximum;
        private double _maxLogValue;
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (value != _maximum)
                {
                    _maximum = value;
                    _maxLogValue = Math.Log(_maximum);
                    this.clampValue();
                    this.OnPropertyChanged("Maximum");
                    this.Invalidate();
                }
            }
        }

        private bool _logarithmic;
        public bool Logarithmic
        {
            get { return _logarithmic; }
            set
            {
                if (value != _logarithmic)
                {
                    _logarithmic = value;
                    this.updateStepPositions();
                    this.OnPropertyChanged("Logarithmic");
                    this.Invalidate();
                }
            }
        }

        private int _value;
        private double _logValue;
        public int Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    _logValue = Math.Log(value);
                    this.clampValue();
                    this.OnPropertyChanged("Value");
                    this.Invalidate();
                }
            }
        }

        private double[] _steps;
        private double[] _stepPositions;
        public double[] Steps
        {
            get { return _steps; }
            set
            {
                if (value != _steps)
                {
                    if (value == null) { _steps = null; }
                    else
                    {
                        List<double> steps = new List<double>(value);
                        steps.Sort();
                        if (steps[0] != this._minimum) steps.Insert(0, this._minimum);
                        if (steps[steps.Count - 1] != this._maximum) steps.Add(this._maximum);
                        _steps = steps.ToArray();
                    }
                    this.updateStepPositions();
                    this.OnPropertyChanged("Steps");
                    this.Invalidate();

                }
            }
        }

        private Color _stepColor;
        public Color StepColor
        {
            get { return _stepColor; }
            set
            {
                if (value != _stepColor)
                {
                    _stepColor = value;
                    this.OnPropertyChanged("StepColor");
                    this.Invalidate();
                }
            }
        }

        private bool _evenSteps;

        public bool EvenSteps
        {
            get { return _evenSteps; }
            set
            {
                if (value != _evenSteps)
                {
                    _evenSteps = value;
                    this.OnPropertyChanged("EvenSteps");
                    this.Invalidate();
                }
            }
        }

        private bool _paintValue;
        public bool PaintValue
        {
            get { return _paintValue; }
            set
            {
                if (value != _paintValue)
                {
                    _paintValue = value;
                    this.OnPropertyChanged("PaintValue");
                    this.Invalidate();
                }
            }
        }

        private bool _hex;
        public bool Hex
        {
            get { return _hex; }
            set
            {
                if (value != _hex)
                {
                    _hex = value;
                    this.OnPropertyChanged("Hex");
                    this.Invalidate();
                }
            }
        }

        private bool _snapValue;

        public bool SnapValue
        {
            get { return _snapValue; }
            set
            {
                if (value != _snapValue)
                {
                    _snapValue = value;
                    this.OnPropertyChanged("SnapValue");
                    this.Invalidate();
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            setValueFromMouseEvent(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            setValueFromMouseEvent(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            setValueFromMouseEvent(e);
            base.OnMouseUp(e);
        }

        private void setValueFromMouseEvent(MouseEventArgs e)
        {
            if (!this.DesignMode)
            {
                if ((e.Button & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left)
                {
                    this.setValueFromPoint(e.Location);
                }
            }
        }

        private void clampValue()
        {
            if (this._value > this._maximum)
            {
                this._value = this._maximum;
                this._logValue = Math.Log(this._value);
            }
            if (this._value < this._minimum)
            {
                this._value = this._minimum;
                this._logValue = Math.Log(this._value);
            }
        }

        private double getScaleFactor()
        {
            double scaleFactor = 0;
            if (this.Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                scaleFactor = this.ClientRectangle.Width;
            }
            else
            {
                scaleFactor = this.ClientRectangle.Height;
            }
            return scaleFactor;
        }

        private void updateStepPositions()
        {
            if (this._steps != null)
            {
                double scaleFactor = getScaleFactor();
                this._stepPositions = new double[this._steps.Length];
                if (this.EvenSteps)
                {
                    double delta = scaleFactor / (this._steps.Length - 1);
                    double current = 0;
                    for (int i = 0, li = this._steps.Length; i < li; ++i)
                    {
                        this._stepPositions[i] = current;
                        current += delta;
                    }
                }
                else
                {
                    if (this.Logarithmic)
                    {
                        double logScale = (this._maxLogValue - this._minLogValue) / scaleFactor;
                        for (int i = 0, li = this._steps.Length; i < li; ++i)
                        {
                            if (this._steps[i] > 0)
                            {
                                this._stepPositions[i] = (Math.Log(this._steps[i]) - this._minLogValue) / logScale;
                            }
                            else
                            {
                                this._stepPositions[i] = this._minLogValue;
                            }
                        }
                    }
                    else
                    {
                        double linScale = (this._maximum - this._minimum) / scaleFactor;
                        for (int i = 0, li = this._steps.Length; i < li; ++i)
                        {
                            this._stepPositions[i] = (this._steps[i] - this._minimum) / linScale;
                        }
                    }
                }
            }
        }

        private void updateParameters()
        {
            double scaleFactor = getScaleFactor();
            this.updateStepPositions();
            if (this.EvenSteps && this.Steps != null)
            {
                if (this.SnapValue)
                {
                    int valIdx = Array.IndexOf(this._steps, this._value);
                    if (valIdx == -1)
                    {
                        this._sliderPos = getNearestPos(this._value);
                        this._value = getNearestValue(this._sliderPos);
                    }
                    valIdx = Array.IndexOf(this._steps, this._value);
                    this._sliderPos = (int)Math.Round(this._stepPositions[valIdx]);
                }
                else
                {
                    double scale = scaleFactor / (this._steps.Length + 2);
                    double minor = double.NaN, major = double.NaN, minorPos = double.NaN, majorPos = double.NaN;
                    for (int i = 0, li = this._steps.Length; i < li; ++i)
                    {
                        if (this._steps[i] >= this._value)
                        {
                            major = this._steps[i];
                            majorPos = this._stepPositions[i];
                            if (i == 0)
                            {
                                minor = _minimum; minorPos = 0;
                            }
                            else
                            {
                                minor = this._steps[i - 1];
                                minorPos = this._stepPositions[i - 1];
                            }
                            break;
                        }
                    }
                    this._sliderPos = (int)Math.Round(minorPos + ((this._value - minor) / (major - minor) * (majorPos - minorPos)));
                }
            }
            else
            {
                if (this.Logarithmic)
                {
                    double logScale = (this._maxLogValue - this._minLogValue) / scaleFactor;
                    this._sliderPos = (int)Math.Round((this._logValue - this._minLogValue) / logScale);
                }
                else
                {
                    double linScale = (this._maximum - this._minimum) / scaleFactor;
                    this._sliderPos = (int)Math.Round((this._value - this._minimum) / linScale);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.updateParameters();
            base.OnPaint(e);
            Brush brush = new SolidBrush(this.ForeColor);

            e.Graphics.FillRectangle(brush, this.getValueRectangle());

            if (this._steps != null)
            {
                Pen linePen = new Pen(this.StepColor, 1);
                if (this.Orientation == System.Windows.Forms.Orientation.Horizontal)
                {
                    float y1 = 0;
                    float y2 = this.ClientRectangle.Height;
                    foreach (double stepPos in this._stepPositions)
                    {
                        e.Graphics.DrawLine(linePen, (float)stepPos, y1, (float)stepPos, y2);
                    }
                }
                else
                {
                    float x1 = 0;
                    float x2 = this.ClientRectangle.Width;
                    foreach (double stepPos in this._stepPositions)
                    {
                        e.Graphics.DrawLine(linePen, x1, (float)stepPos, x2, (float)stepPos);
                    }
                }

                linePen.Dispose();
            }

            if (this.PaintValue)
            {
                string valString = this.Value.ToString(this.Hex ? "X2" : "G");
                Brush bgAlphaBrush = new SolidBrush(Color.FromArgb(128, this.BackColor));
                SizeF stringSize = e.Graphics.MeasureString(valString, this.Font);
                PointF textOrigin = this.getTextOrigin(stringSize);

                e.Graphics.FillRectangle(bgAlphaBrush, textOrigin.X - 2, textOrigin.Y - 2, stringSize.Width + 4, stringSize.Height + 4);
                e.Graphics.DrawString(valString, this.Font, brush, textOrigin);
                bgAlphaBrush.Dispose();
            }

            brush.Dispose();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSizeChanged(e);
        }

        private PointF getTextOrigin(SizeF contentSize)
        {
            PointF result = new PointF(
                this.ClientRectangle.X + (this.ClientRectangle.Width - contentSize.Width) / 2,
                this.ClientRectangle.Y + (this.ClientRectangle.Height - contentSize.Height) / 2
            );
            return result;
        }

        private void setValueFromPoint(Point pt)
        {
            int scaleFactor = 0;
            if (this.Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                this._sliderPos = pt.X;
                scaleFactor = this.ClientRectangle.Width;
            }
            else
            {
                this._sliderPos = this.ClientRectangle.Height - pt.Y;
                scaleFactor = this.ClientRectangle.Height;
            }

            if (this._sliderPos < 0) this._sliderPos = 0;
            if (this._sliderPos > scaleFactor) this._sliderPos = scaleFactor;

            if (this.EvenSteps && this.Steps != null)
            {
                if (this.SnapValue)
                {
                    this.Value = getNearestValue(this._sliderPos);
                }
                else
                {
                    double value = double.NaN, minVal = double.NaN,
                           maxVal = double.NaN, minPos = double.NaN,
                           maxPos = double.NaN;

                    for (int i = 0, li = this._stepPositions.Length; i < li; ++i)
                    {
                        if (this._stepPositions[i] >= this._sliderPos)
                        {
                            minPos = this._stepPositions[i - 1];
                            minVal = this._steps[i - 1];
                            maxPos = this._stepPositions[i];
                            maxVal = this._steps[i];
                            break;
                        }
                    }
                    if (this.Logarithmic)
                    {
                        if (minVal == 0) minVal = 0.00001;
                        double logScale = (Math.Log(maxVal) - Math.Log(minVal)) / (maxPos - minPos);
                        value = Math.Exp(Math.Log(minVal) + ((this._sliderPos - minPos) * logScale));
                    }
                    else
                    {
                        double linScale = (maxVal - minVal) / (maxPos - minPos);
                        value = minVal + ((this._sliderPos - minPos) * linScale);
                    }
                    this.Value = (int)Math.Round(value);
                }
            }
            else
            {
                if (SnapValue && this.Steps != null)
                {
                    this.Value = getNearestValue(this._sliderPos);
                }
                else
                {
                    double value;
                    if (this.Logarithmic)
                    {
                        double logScale = (this._maxLogValue - this._minLogValue) / (double)scaleFactor;
                        value = Math.Exp(this._minLogValue + (this._sliderPos * logScale));
                    }
                    else
                    {
                        double linScale = (this._maximum - this._minimum) / (double)scaleFactor;
                        value = this._minimum + (this._sliderPos * linScale);
                    }
                    this.Value = (int)Math.Round(value);
                }
            }
        }

        private int getNearestValue(double sliderPos)
        {
            double result = double.NaN;
            if (sliderPos < this._stepPositions[0])
            {
                result = this._minimum;
            }
            else if (sliderPos > this._stepPositions[this._stepPositions.Length - 1])
            {
                result = this._maximum;
            }
            else
            {
                for (int i = 1, li = this._steps.Length; i < li; ++i)
                {
                    if (this._stepPositions[i - 1] <= sliderPos && this._stepPositions[i] >= sliderPos)
                    {
                        double min = this._stepPositions[i - 1];
                        double max = this._stepPositions[i];
                        double deltaMin = sliderPos - min;
                        double deltaMax = max - sliderPos;
                        if (deltaMax > deltaMin)
                        {
                            result = this._steps[i - 1];
                        }
                        else
                        {
                            result = this._steps[i];
                        }
                        break;
                    }
                }
            }
            return (int)Math.Round(result);
        }

        private int getNearestPos(int value)
        {
            double result = double.NaN;
            int maxIdx = this._steps.Length - 1;
            if (value < this._steps[0])
            {
                result = this._stepPositions[0];
            }
            else if (value > this._steps[maxIdx])
            {
                result = this._stepPositions[maxIdx];
            }
            else
            {
                for (int i = 1, li = this._steps.Length; i < li; ++i)
                {
                    if (this._steps[i - 1] <= value && this._steps[i] >= value)
                    {
                        double min = this._steps[i - 1];
                        double max = this._steps[i];
                        double deltaMin = value - min;
                        double deltaMax = max - value;
                        if (deltaMax > deltaMin)
                        {
                            result = this._stepPositions[i - 1];
                        }
                        else
                        {
                            result = this._stepPositions[i];
                        }
                        break;
                    }
                }
            }
            return (int)Math.Round(result);
        }

        private Rectangle getValueRectangle()
        {
            Rectangle result = new Rectangle();
            result.X = 0;
            if (this.Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                result.Y = 0;
                result.Height = this.ClientRectangle.Height;
                result.Width = this._sliderPos;
            }
            else
            {
                result.Height = this._sliderPos;
                result.Y = this.ClientRectangle.Height - result.Height;
                result.Width = this.ClientRectangle.Width;
            }
            return result;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler tmp = this.PropertyChanged;
            if (tmp != null)
            {
                tmp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
