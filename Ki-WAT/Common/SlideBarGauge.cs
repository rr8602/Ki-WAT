using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
//using System.Windows.Media.Media3D;

namespace RollTester
{


    public class SlideBarGauge : Control
    {

        private float _minimum = -60;
        private float _maximum = 60;
        private float _value = 35;
        private float _barHeight = 50;
        private float _Target = 00;
        private float _Adjust = 10;
        private float _Evaluation = 20;


        public SlideBarGauge()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                   ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw |
                   ControlStyles.UserPaint, true);
            Size = new Size(300, 50);
        }

        [Category("Behavior")]
        public float Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                if (_maximum < _minimum) _maximum = _minimum;
                if (_value < _minimum) _value = _minimum;
                Invalidate();
            }
        }

        [Category("Behavior")]
        public float Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                if (_minimum > _maximum) _minimum = _maximum;
                if (_value > _maximum) _value = _maximum;
                Invalidate();
            }
        }
        [Category("Behavior")]
        public float BarHeight
        {
            get => _barHeight;
            set
            {
                _barHeight = value;
                Invalidate();
            }
        }
        [Category("Behavior")]
        public float Value
        {
            get => _value;
            set
            {
                float newVal = Math.Max(_minimum, Math.Min(_maximum, value));
                if (_value != newVal)
                {
                    _value = newVal;
                    Invalidate();
                }
            }
        }

        [Category("Appearance")] public Color BarBackColor { get; set; } = Color.DeepPink;
        [Category("Appearance")] public Color BarAdjustColor { get; set; } = Color.Green;
        [Category("Appearance")] public Color BarEvaluationColor { get; set; } = Color.Yellow;
        [Category("Appearance")] public Color ThumbColor { get; set; } = Color.White;
        [Category("Appearance")] public Color ThumbBorderColor { get; set; } = Color.Black;
        [Category("Appearance")] public Color TickColor { get; set; } = Color.Black;

        public int Tolerance { get; set; } = 10;
        public bool bTarget { get; set; } = true;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 바 영역
            //var barY = (Height - _barHeight) / 2;
            float barY = (Height - _barHeight) / 4;
            var barRect = new Rectangle(0 + 40, (int)barY, Width - 80, (int)_barHeight);

            // 배경 바
            using (var back = new SolidBrush(BarBackColor))
                g.FillRectangle(back, barRect);
            using (var barPen = new Pen(Color.Black, 1.5f))
                g.DrawRectangle(barPen, barRect);
            // 0점 위치
            float zeroT = (_maximum == _minimum) ? 0.5f :
               (0f - _minimum) / (float)(_maximum - _minimum);
            int zeroX = barRect.X + (int)(barRect.Width * zeroT);

            // 값 위치
            float valT = (_maximum == _minimum) ? 0.5f :
               (Value - _minimum) / (float)(_maximum - _minimum);
            int valX = barRect.X + (int)(barRect.Width * valT);

            // 0~값 채우기
            bool bTolerance = true;


            if (Tolerance < Math.Abs(Value)) bTolerance = false;



            DrawBar(g, barRect);

            if (bTarget)
            {
                DrawTargetBar(g, barRect);
            }

            // 눈금 표시
            DrawTicks(g, barRect, zeroX);

            // 값 표시 (Value → X 좌표 변환)
            int range = (int)Math.Abs(Maximum) + (int)Math.Abs(Minimum);
            float ratio = (float)(Value - Minimum) / range;
            valX = barRect.Left + (int)(barRect.Width * ratio);

            int thumbWidth = 5;
            int thumbHeight = barRect.Height + 0;
            int thumbTop = barRect.Top - 0;


            // 단순 박스 Thumb
            Rectangle thumbRect = new Rectangle(
               valX - thumbWidth / 2,
               thumbTop,
               thumbWidth,
               thumbHeight);
            DrawThumb(g, thumbRect);

        }

        private void DrawBar(Graphics g, Rectangle barRect)
        {

            // 배경 바 (SolidBrush → LinearGradientBrush)
            using (var gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                     barRect,
                     Color.FromArgb(180, Color.White),  // 위 (반투명)
                     Color.FromArgb(180, BarBackColor),  // 아래 (반투명)

                     System.Drawing.Drawing2D.LinearGradientMode.Vertical))
            {
                // 중앙 밝고 위/아래 진한 효과
                var blend = new System.Drawing.Drawing2D.Blend();
                blend.Positions = new float[] { 0f, 0.5f, 1f }; // 위, 중앙, 아래
                blend.Factors = new float[] { 1f, 0.3f, 1f };   // 위=진함, 중앙=밝음, 아래=진함

                gradBrush.Blend = blend;

                g.FillRectangle(gradBrush, barRect);
            }

            // 테두리
            //using (var barPen = new Pen(Color.Gray, 1f))
            //g.DrawRectangle(barPen, barRect);
        }
        private void DrawTargetBar(Graphics g, Rectangle barRect)
        {
            {
                // 값 표시 (Value → X 좌표 변환)
                float range = Math.Abs(Maximum) + Math.Abs(Minimum);
                float OneSize = barRect.Width / range;

                float ratio = ((int)_Target + (int)Math.Abs(Minimum)) * OneSize;


                int thumbWidth = (int)(Math.Abs(_Evaluation) * 2 * OneSize);


                int valX = barRect.Left + (int)(ratio) - (thumbWidth / 2);

                int thumbHeight = barRect.Height + 0;
                int thumbTop = barRect.Top - 0;


                Rectangle barEvaluation = new Rectangle(
                         valX,
                         thumbTop,
                         thumbWidth,
                         thumbHeight);


                // 배경 바 (SolidBrush → LinearGradientBrush)
                using (var gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                         barRect,
                         Color.FromArgb(180, Color.White),  // 위 (반투명)
                         Color.FromArgb(180, Color.Yellow),  // 아래 (반투명)

                         System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    // 중앙 밝고 위/아래 진한 효과
                    var blend = new System.Drawing.Drawing2D.Blend();
                    blend.Positions = new float[] { 0f, 0.5f, 1f }; // 위, 중앙, 아래
                    blend.Factors = new float[] { 1f, 0.3f, 1f };   // 위=진함, 중앙=밝음, 아래=진함

                    gradBrush.Blend = blend;

                    g.FillRectangle(gradBrush, barEvaluation);
                }

                // 테두리
                //using (var barPen = new Pen(Color.Black, 1.5f))
                //g.DrawRectangle(barPen, barEvaluation);
            }

            {
                // 값 표시 (Value → X 좌표 변환)
                float range = Math.Abs(Maximum) + Math.Abs(Minimum);
                float OneSize = barRect.Width / range;

                float ratio = ((int)_Target + (int)Math.Abs(Minimum)) * OneSize;


                int thumbWidth = (int)(Math.Abs(_Adjust) * 2 * OneSize);


                int valX = barRect.Left + (int)(ratio) - (thumbWidth / 2);

                int thumbHeight = barRect.Height + 0;
                int thumbTop = barRect.Top - 0;


                Rectangle barAdjust = new Rectangle(
                         valX,
                         thumbTop,
                         thumbWidth,
                         thumbHeight);


                // 배경 바 (SolidBrush → LinearGradientBrush)
                using (var gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                         barRect,
                         Color.FromArgb(180, Color.White),  // 위 (반투명)
                         Color.FromArgb(180, Color.Lime),  // 아래 (반투명)

                         System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    // 중앙 밝고 위/아래 진한 효과
                    var blend = new System.Drawing.Drawing2D.Blend();
                    blend.Positions = new float[] { 0f, 0.5f, 1f }; // 위, 중앙, 아래
                    blend.Factors = new float[] { 1f, 0.3f, 1f };   // 위=진함, 중앙=밝음, 아래=진함

                    gradBrush.Blend = blend;

                    g.FillRectangle(gradBrush, barAdjust);
                }

                // 테두리
                //using (var barPen = new Pen(Color.Black, 1.5f))
                //g.DrawRectangle(barPen, barAdjust);
            }






        }
        private void DrawThumb(Graphics g, Rectangle thumbRect)
        {
            using (Brush b = new SolidBrush(Color.White))
                g.FillRectangle(b, thumbRect);

            using (Pen p = new Pen(Color.Black, 2))
                g.DrawRectangle(p, thumbRect);
        }

        private void DrawTicks(Graphics g, Rectangle barRect, int zeroX)
        {
            int nDiv = (int)((Math.Abs(Maximum) + Math.Abs(Minimum)) / 10);
            int step = (int)((Maximum - Minimum) / nDiv); // 10등분
            if (step <= 0) step = 1;

            using (var pen = new Pen(TickColor, 1))
            {
                for (int v = (int)Minimum; v <= Maximum; v += step)
                {

                    float range = Math.Abs(Maximum) + Math.Abs(Minimum);
                    float OneSize = barRect.Width / range;

                    float ratio = ((int)_Target + (int)Math.Abs(Minimum)) * OneSize;

                    int Center = barRect.Left + (int)(ratio);



                    if (v == _Target) g.DrawLine(pen, Center, barRect.Top, Center, barRect.Bottom);

                    // 값 레이블

                    if (v == Maximum || v == Minimum || v == _Target)
                    {

                        int radius = (int)(Math.Min(this.Width, this.Height) * 0.10); // 컨트롤 크기 기준 반지름 설정

                        using (Font valueFont = new Font(this.Font.FontFamily, radius * 1f, FontStyle.Bold))
                        {
                            var str = v.ToString();
                            var sz = g.MeasureString(str, valueFont);

                            if (v == Minimum) g.DrawString(str, valueFont, Brushes.Black, barRect.Left - sz.Width / 2 - 2, barRect.Bottom + radius / 10);
                            if (v == Maximum) g.DrawString(str, valueFont, Brushes.Black, barRect.Right - sz.Width / 2 + 2, barRect.Bottom + radius / 10);
                            if (v == _Target) g.DrawString(str, valueFont, Brushes.Black, Center - sz.Width / 2, barRect.Bottom + radius / 10);

                        }

                    }
                }

            }
        }
        public void SetTargetAdjust(int target, int adjust, int evaluation)
        {
            if (_Target != target)
            {

                float oldTarget = _Target; // 이전 값 저장



                {
                    _Target = target;
                    float diff = _Target - oldTarget; // 변화량 계산
                    Minimum += diff;                // 변화량만큼 조정
                    Maximum += diff;
                    _Adjust = adjust;
                    _Evaluation = evaluation;

                    Invalidate();

                }
            }
        }

        public void SetTarget(float target, float range)
        {
            _Target = target;
            this.Minimum = target - range;
            this.Maximum = target + range;
            Invalidate();
        }
    }

}
