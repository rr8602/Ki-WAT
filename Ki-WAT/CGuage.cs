using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RollTester
{
	internal class CGuage : Control
	{
		public float currentValue { get; set; } = 0;

		private float sweepAngle = 230f;
		private float _target;

		public bool SpeedMode { get; set; } = true;
		public float maxValue { get; set; } = 160f;
		public float minValue { get; set; } = 0;
		public float TickInterval { get; set; } = 10f;

		public Color BgColor { get; set; } = Color.CornflowerBlue;
		public Color RegionColor { get; set; } = Color.LightGreen;
		public Color NiddleColor { get; set; } = Color.Magenta;
		public Color ValueColor { get; set; } = Color.LightSteelBlue;
		public Color TitleColor { get; set; } = Color.Black;
		public String Title { get; set; } = "";

		private float Target = 0f;
		private float TargetAdjust  = 10f;
		private float TargetEvaluation = 20f;


		private int nLowSection = 0;
		private int nHighSection = 0;

		public void SetSection(int nLow, int nHigh)
		{
			this.nLowSection = nLow;		
			this.nHighSection = nHigh;
		}
		public void SetTargetAdjust(float target, float adjust, float evaluation)
		{
			if (Target != target)
			{

				float oldTarget = Target; // 이전 값 저장
				

				if (SpeedMode == false)
				{
					Target = target;
					float diff = target - oldTarget; // 변화량 계산
					minValue += diff;                // 변화량만큼 조정
					maxValue += diff;
					TargetAdjust = adjust;
					TargetEvaluation = evaluation;

					Invalidate();

				}
			}
		}
		public void SetRange(float min, float max)
		{
			this.minValue = min;
			this.maxValue = max;
			Target = (max + min)/2;
			Invalidate();
		}
		public void SetTarget(float target, float range)
		{
			Target = target;
			this.minValue = target - range;
			this.maxValue = target + range;
			Invalidate ();
		}


		public CGuage()
		{
			this.DoubleBuffered = true;
			this.ResizeRedraw = true;

		}
		public void SetValue(double dSpeed)
		{
			int nSpeed = (int)dSpeed;
			if (nSpeed < 0) return;
			if(nSpeed > maxValue) return;
			this.currentValue = (float) dSpeed;	
			this.Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint(e);
			var g = e.Graphics;
			DrawGauge(g);
		}

		private void DrawGauge(Graphics g)
		{






			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			int centerX = this.Width / 2;
			int centerY = (int)(this.Height * 0.65); // 컨트롤 높이에 따라 위치 자동 조정

			int radius = (int)(Math.Min(this.Width, this.Height) * 0.53); // 컨트롤 크기 기준 반지름 설정
			float startAngle = 155f;

			Rectangle rect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);
			float baseLineRadius = 0;
			if (SpeedMode == true)
			{
				Pen basePen = new Pen(this.BgColor, radius * 0.08f);
				g.DrawArc(basePen, rect, startAngle, sweepAngle);

				//float safeStartVal = Math.Max(minValue, currentValue - 10);
				//float safeEndVal = Math.Min(maxValue, currentValue + 10);

				float safeStartVal = Math.Max(minValue, nLowSection);
				float safeEndVal = Math.Min(maxValue, nHighSection);

				float safeStartAngle = sweepAngle * (safeStartVal - minValue) / (maxValue - minValue);
				float safeSweep = sweepAngle * (safeEndVal - safeStartVal) / (maxValue - minValue);




				using (Pen safePen = new Pen(this.RegionColor, radius * 0.08f))
					g.DrawArc(safePen, rect, startAngle + safeStartAngle, safeSweep);
				baseLineRadius = radius - basePen.Width / 2;
				basePen.Dispose();
			}
			else
			{
				float nTarget = Target;
				float nAdjustLow = (nTarget - TargetAdjust);
				float nAdjustHigh = (nTarget + TargetAdjust);

				float nEvaluationLow = (nTarget - TargetEvaluation);
				float nEvaluationHigh = (nTarget + TargetEvaluation);

				float nMin = minValue;
				float nMax = maxValue;	


				List<(float start, float end, Color color)> zones = new List<(float, float, Color)>
				{
					(nMin, nEvaluationLow, Color.Red),
					(nEvaluationLow, nAdjustLow, Color.Yellow),
					(nAdjustLow, nAdjustHigh, Color.LightGreen),
					(nAdjustHigh, nEvaluationHigh, Color.Yellow),
					(nEvaluationHigh, nMax, Color.Red)
				};
				float thickness = radius * 0.08f;
				foreach (var zone in zones)
				{
					// 값 → 각도로 변환
					float zoneStartAngle = sweepAngle * (zone.start - minValue) / (maxValue - minValue);
					float zoneSweepAngle = sweepAngle * (zone.end - zone.start) / (maxValue - minValue);

					Pen zonePen = new Pen(zone.color, thickness);
					baseLineRadius = radius - zonePen.Width / 2;
					{
						g.DrawArc(zonePen, rect, startAngle + zoneStartAngle, zoneSweepAngle);
					}
					zonePen.Dispose();
				}
				
			}




				int gaugeThickness = (int)(radius * 0.08f);

			

			for (float i = minValue; i <= maxValue; i += TickInterval)
			{
				float angle = startAngle + sweepAngle * (i - minValue) / (maxValue - minValue);
				double rad = angle * Math.PI / 180;

				// 눈금 길이
				//float tickLength = radius * 0.08f;
				float tickLength = radius * 0.05f;

				float x1 = centerX + (float)(Math.Cos(rad) * (baseLineRadius - 0));
				float y1 = centerY + (float)(Math.Sin(rad) * (baseLineRadius - 0));
				float x2 = centerX + (float)(Math.Cos(rad) * (baseLineRadius + tickLength));
				float y2 = centerY + (float)(Math.Sin(rad) * (baseLineRadius + tickLength));
				g.DrawLine(Pens.Black, x1, y1, x2, y2);

				string label = ((int)i).ToString();
				using (Font tickFont = new Font(this.Font.FontFamily, radius * 0.06f))
				{
					SizeF textSize = g.MeasureString(label, tickFont);
					float xt = centerX + (float)(Math.Cos(rad) * (baseLineRadius - tickLength * 2.5f)) - textSize.Width / 2;
					float yt = centerY + (float)(Math.Sin(rad) * (baseLineRadius - tickLength * 2.5f)) - textSize.Height / 2;
					g.DrawString(label, tickFont, Brushes.Black, xt, yt);
				}
			}

			// Needle
			{
				float needleAngle = startAngle + sweepAngle * (currentValue - minValue) / (maxValue - minValue);
				double rad = needleAngle * Math.PI / 180;

				int nIncreaseNiddle = -50;
				if(this.Width < 350) nIncreaseNiddle = -20;

				float needleLength = radius + nIncreaseNiddle;
				float needleWidth = radius * 0.03f;

				PointF p1 = new PointF(centerX, centerY);
				PointF p2 = new PointF(
					centerX + (float)(Math.Cos(rad) * needleLength),
					centerY + (float)(Math.Sin(rad) * needleLength)
				);

				double leftRad = rad + Math.PI / 2;
				double rightRad = rad - Math.PI / 2;

				PointF p3 = new PointF(
					centerX + (float)(Math.Cos(leftRad) * (needleWidth / 2)),
					centerY + (float)(Math.Sin(leftRad) * (needleWidth / 2))
				);
				PointF p4 = new PointF(
					centerX + (float)(Math.Cos(rightRad) * (needleWidth / 2)),
					centerY + (float)(Math.Sin(rightRad) * (needleWidth / 2))
				);

				PointF[] needleTriangle = new PointF[] { p2, p3, p4 };
				Brush niddleBrush = new SolidBrush(NiddleColor);

				g.FillPolygon(niddleBrush, needleTriangle);
				niddleBrush.Dispose();
			}

			// 값 출력
			string valueStr = "";

			if (currentValue % 1 == 0)      // 소수점 이하가 0이면
				valueStr = ((int)currentValue).ToString();
			else
				valueStr = currentValue.ToString("F1");

			Brush valueBrush = new SolidBrush(ValueColor);
			Brush titleBrush = new SolidBrush(TitleColor);

			using (Font valueFont = new Font(this.Font.FontFamily, radius * 0.2f, FontStyle.Bold))
			{
				SizeF valueSize = g.MeasureString(valueStr, valueFont);
				g.DrawString(valueStr, valueFont, valueBrush,
					this.Width / 2 - valueSize.Width / 2,
					centerY - radius / 2 - valueSize.Height / 2 + (radius * 0.8f) + valueSize.Height / 2 + (valueSize.Height / 5));

			}
			using (Font valueFont = new Font(this.Font.FontFamily, radius * 0.15f, FontStyle.Bold))
			{
	
				if (Title != "")
				{
					SizeF valueSize = g.MeasureString(Title, valueFont);
					g.DrawString(Title, valueFont, titleBrush,
						this.Width / 2 - valueSize.Width / 2,
						centerY - radius / 2 - valueSize.Height / 2 + (radius * 0.8f));

				}

			}
			valueBrush.Dispose();
			titleBrush.Dispose();

		}
	}
}
