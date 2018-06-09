#if (!NETCOREAPP1_0 && !NETCOREAPP1_1 && !NETCOREAPP2_0 && !NETCOREAPP2_1)

using System;
using System.Windows;

namespace Remnant.Core.Services
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	public class GraphicsService
	{
		public static bool PointInRegion(Point point, Rect rect)
		{
			if (point.X >= rect.Left &&
				 point.X <= rect.Left + rect.Width &&
				 point.Y >= rect.Top &&
				 point.Y <= rect.Top + rect.Height)
				return true;
			
			return false;
		}

		public static int CalcAngle(float x, float y)
		{
			if (x == 0 && y < 0)
				return 0;
			if (x == 0 && y > 0)
				return 180;
			if (x > 0 && y == 0)
				return 90;
			if (x < 0 && y == 0)
				return 270;
			if (x > 0 && y < 0)
				return System.Convert.ToInt32(90 - Math.Round(Math.Atan(Math.Abs(y) / x) * 180 / Math.PI));
			if (x > 0 && y > 0)
				return System.Convert.ToInt32(Math.Round(90 + Math.Atan(Math.Abs(y) / x) * 180 / Math.PI));
			if (x < 0 && y > 0)
				return System.Convert.ToInt32(180 + Math.Round(Math.Atan(Math.Abs(x) / Math.Abs(y)) * 180 / Math.PI));
			if (x < 0 && y < 0)
				return System.Convert.ToInt32(Math.Round(360 - Math.Atan(Math.Abs(x) / Math.Abs(y)) * 180 / Math.PI));
			return 0;
		}

		public static Point SubtractPoints(Point pVec1, Point pVec2)
		{
			return new Point(pVec1.X - pVec2.X, pVec1.Y - pVec2.Y);
		}

		public static bool LinesCross(Point pLineAP1, Point pLineAP2, Point pLineBP1, Point pLineBP2)
		{
			Point diffLA;
			Point diffLB;
			double CompareA = 0;
			double CompareB = 0;

			diffLA = SubtractPoints(pLineAP2, pLineAP1);
			diffLB = SubtractPoints(pLineBP2, pLineBP1);
			CompareA = diffLA.X * pLineAP1.Y - diffLA.Y * pLineAP1.X;
			CompareB = diffLB.X * pLineBP1.Y - diffLB.Y * pLineBP1.X;

			if (
					((diffLA.X * pLineBP1.Y - diffLA.Y * pLineBP1.X) < CompareA) ^
					((diffLA.X * pLineBP2.Y - diffLA.Y * pLineBP2.X) < CompareA) &&
					((diffLB.X * pLineAP1.Y - diffLB.Y * pLineAP1.X) < CompareB) ^
					((diffLB.X * pLineAP2.Y - diffLB.Y * pLineAP2.X) < CompareB)
				)
				return true;
			
			return false;
		}

		public static Point LinesIntersection(Point LineAP1, Point LineAP2, Point LineBP1, Point LineBP2)
		{
			Double LDetLineA;
			Double LDetLineB;
			Double LDetDivInv;
			Point LDiffLA;
			Point LDiffLB;

			LDetLineA = LineAP1.X * LineAP2.Y - LineAP1.Y * LineAP2.X;
			LDetLineB = LineBP1.X * LineBP2.Y - LineBP1.Y * LineBP2.X;

			LDiffLA = SubtractPoints(LineAP1, LineAP2);
			LDiffLB = SubtractPoints(LineBP1, LineBP2);

			// gives EZeroDivide if lines are parallel. Must check for this first
			try
			{
				LDetDivInv = 1 / ((LDiffLA.X * LDiffLB.Y) - (LDiffLA.Y * LDiffLB.X));
			}
			catch
			{
				return new Point(0, 0);
			}

			return new Point(Convert.ToInt32(Math.Round(((LDetLineA * LDiffLB.X) - (LDiffLA.X * LDetLineB)) * LDetDivInv)),
																		 Convert.ToInt32(Math.Round(((LDetLineA * LDiffLB.Y) - (LDiffLA.Y * LDetLineB)) * LDetDivInv)));
		}

		public static Point LinesIntersectRect(Point LineAP1, Point LineAP2, Rect rect)
		{
			Point BP1;
			Point BP2;
			Point result;

			BP1 = new Point(rect.Left, rect.Top);
			BP2 = new Point(rect.Left, rect.Bottom);
			result = LinesIntersect(LineAP1, LineAP2, BP1, BP2);
			if (result.X != 0 && result.Y != 0) return result;

			BP1 = new Point(rect.Left, rect.Top);
			BP2 = new Point(rect.Right, rect.Top);
			result = LinesIntersect(LineAP1, LineAP2, BP1, BP2);
			if (result.X != 0 && result.Y != 0) return result;

			BP1 = new Point(rect.Right, rect.Top);
			BP2 = new Point(rect.Right, rect.Bottom);
			result = LinesIntersect(LineAP1, LineAP2, BP1, BP2);
			if (result.X != 0 && result.Y != 0) return result;

			BP1 = new Point(rect.Right, rect.Bottom);
			BP2 = new Point(rect.Left, rect.Bottom);
			result = LinesIntersect(LineAP1, LineAP2, BP1, BP2);
			if (result.X != 0 && result.Y != 0) return result;

			return new Point(0, 0);
		}

		public static Point LinesIntersect(Point line1Start, Point line1End, Point line2Start, Point line2End)
		{
			double x1 = line1Start.X;
			double y1 = line1Start.Y;
			double x2 = line1End.X;
			double y2 = line1End.Y;
			double x3 = line2Start.X;
			double y3 = line2Start.Y;
			double x4 = line2End.X;
			double y4 = line2End.Y;

			double d = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
			if (d != 0)
			{
				double ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / d;
				if (ua >= 0.0 && ua <= 1.0)
				{
					double ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / d;
					if (ub >= 0.0 && ub <= 1.0)
					{
						double x = x1 + ua * (x2 - x1);
						double y = y1 + ua * (y2 - y1);
						return new Point(Convert.ToInt32(x), Convert.ToInt32(y));
					}
				}
			}
			return new Point(0, 0);
		}
	}
}
#endif