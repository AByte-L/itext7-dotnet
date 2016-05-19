/*
$Id$

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.Collections.Generic;

namespace iTextSharp.Kernel.Geom
{
	/// <summary>Paths define shapes, trajectories, and regions of all sorts.</summary>
	/// <remarks>
	/// Paths define shapes, trajectories, and regions of all sorts. They shall be used
	/// to draw lines, define the shapes of filled areas, and specify boundaries for clipping
	/// other graphics. A path shall be composed of straight and curved line segments, which
	/// may connect to one another or may be disconnected.
	/// </remarks>
	public class Path
	{
		private const String START_PATH_ERR_MSG = "Path shall start with \"re\" or \"m\" operator";

		private IList<Subpath> subpaths = new List<Subpath>();

		private Point currentPoint;

		public Path()
		{
		}

		public Path(IList<Subpath> subpaths)
		{
			AddSubpaths(subpaths);
		}

		public Path(iTextSharp.Kernel.Geom.Path path)
		{
			AddSubpaths(path.GetSubpaths());
		}

		/// <returns>
		/// A
		/// <see cref="System.Collections.IList{E}"/>
		/// of subpaths forming this path.
		/// </returns>
		public virtual IList<Subpath> GetSubpaths()
		{
			return subpaths;
		}

		/// <summary>Adds the subpath to this path.</summary>
		/// <param name="subpath">The subpath to be added to this path.</param>
		public virtual void AddSubpath(Subpath subpath)
		{
			subpaths.Add(subpath);
			currentPoint = subpath.GetLastPoint();
		}

		/// <summary>Adds the subpaths to this path.</summary>
		/// <param name="subpaths">
		/// 
		/// <see cref="System.Collections.IList{E}"/>
		/// of subpaths to be added to this path.
		/// </param>
		public virtual void AddSubpaths<_T0>(IList<_T0> subpaths)
			where _T0 : Subpath
		{
			if (subpaths.Count > 0)
			{
				foreach (Subpath subpath in subpaths)
				{
					this.subpaths.Add(new Subpath(subpath));
				}
				currentPoint = this.subpaths[subpaths.Count - 1].GetLastPoint();
			}
		}

		/// <summary>The current point is the trailing endpoint of the segment most recently added to the current path.
		/// 	</summary>
		/// <returns>The current point.</returns>
		public virtual Point GetCurrentPoint()
		{
			return currentPoint;
		}

		/// <summary>Begins a new subpath by moving the current point to coordinates <CODE>(x, y)</CODE>.
		/// 	</summary>
		public virtual void MoveTo(float x, float y)
		{
			currentPoint = new Point(x, y);
			Subpath lastSubpath = subpaths.Count > 0 ? subpaths[subpaths.Count - 1] : null;
			if (lastSubpath != null && lastSubpath.IsSinglePointOpen())
			{
				lastSubpath.SetStartPoint(currentPoint);
			}
			else
			{
				subpaths.Add(new Subpath(currentPoint));
			}
		}

		/// <summary>Appends a straight line segment from the current point to the point <CODE>(x, y)</CODE>.
		/// 	</summary>
		public virtual void LineTo(float x, float y)
		{
			if (currentPoint == null)
			{
				throw new Exception(START_PATH_ERR_MSG);
			}
			Point targetPoint = new Point(x, y);
			GetLastSubpath().AddSegment(new Line(currentPoint, targetPoint));
			currentPoint = targetPoint;
		}

		/// <summary>Appends a cubic Bezier curve to the current path.</summary>
		/// <remarks>
		/// Appends a cubic Bezier curve to the current path. The curve shall extend from
		/// the current point to the point <CODE>(x3, y3)</CODE>.
		/// </remarks>
		public virtual void CurveTo(float x1, float y1, float x2, float y2, float x3, float
			 y3)
		{
			if (currentPoint == null)
			{
				throw new Exception(START_PATH_ERR_MSG);
			}
			// Numbered in natural order
			Point secondPoint = new Point(x1, y1);
			Point thirdPoint = new Point(x2, y2);
			Point fourthPoint = new Point(x3, y3);
			IList<Point> controlPoints = new List<Point>(iTextSharp.IO.Util.JavaUtil.ArraysAsList
				(currentPoint, secondPoint, thirdPoint, fourthPoint));
			GetLastSubpath().AddSegment(new BezierCurve(controlPoints));
			currentPoint = fourthPoint;
		}

		/// <summary>Appends a cubic Bezier curve to the current path.</summary>
		/// <remarks>
		/// Appends a cubic Bezier curve to the current path. The curve shall extend from
		/// the current point to the point <CODE>(x3, y3)</CODE> with the note that the current
		/// point represents two control points.
		/// </remarks>
		public virtual void CurveTo(float x2, float y2, float x3, float y3)
		{
			if (currentPoint == null)
			{
				throw new Exception(START_PATH_ERR_MSG);
			}
			CurveTo((float)currentPoint.GetX(), (float)currentPoint.GetY(), x2, y2, x3, y3);
		}

		/// <summary>Appends a cubic Bezier curve to the current path.</summary>
		/// <remarks>
		/// Appends a cubic Bezier curve to the current path. The curve shall extend from
		/// the current point to the point <CODE>(x3, y3)</CODE> with the note that the (x3, y3)
		/// point represents two control points.
		/// </remarks>
		public virtual void CurveFromTo(float x1, float y1, float x3, float y3)
		{
			if (currentPoint == null)
			{
				throw new Exception(START_PATH_ERR_MSG);
			}
			CurveTo(x1, y1, x3, y3, x3, y3);
		}

		/// <summary>Appends a rectangle to the current path as a complete subpath.</summary>
		public virtual void Rectangle(iTextSharp.Kernel.Geom.Rectangle rect)
		{
			Rectangle(rect.GetX(), rect.GetY(), rect.GetWidth(), rect.GetHeight());
		}

		/// <summary>Appends a rectangle to the current path as a complete subpath.</summary>
		public virtual void Rectangle(float x, float y, float w, float h)
		{
			MoveTo(x, y);
			LineTo(x + w, y);
			LineTo(x + w, y + h);
			LineTo(x, y + h);
			CloseSubpath();
		}

		/// <summary>Closes the current subpath.</summary>
		public virtual void CloseSubpath()
		{
			Subpath lastSubpath = GetLastSubpath();
			lastSubpath.SetClosed(true);
			Point startPoint = lastSubpath.GetStartPoint();
			MoveTo((float)startPoint.GetX(), (float)startPoint.GetY());
		}

		/// <summary>Closes all subpathes contained in this path.</summary>
		public virtual void CloseAllSubpaths()
		{
			foreach (Subpath subpath in subpaths)
			{
				subpath.SetClosed(true);
			}
		}

		/// <summary>Adds additional line to each closed subpath and makes the subpath unclosed.
		/// 	</summary>
		/// <remarks>
		/// Adds additional line to each closed subpath and makes the subpath unclosed.
		/// The line connects the last and the first points of the subpaths.
		/// </remarks>
		/// <returns>Indices of modified subpaths.</returns>
		public virtual IList<int?> ReplaceCloseWithLine()
		{
			IList<int?> modifiedSubpathsIndices = new List<int?>();
			int i = 0;
			/* It could be replaced with "for" cycle, because IList in C# provides effective
			* access by index. In Java List interface has at least one implementation (LinkedList)
			* which is "bad" for access elements by index.
			*/
			foreach (Subpath subpath in subpaths)
			{
				if (subpath.IsClosed())
				{
					subpath.SetClosed(false);
					subpath.AddSegment(new Line(subpath.GetLastPoint(), subpath.GetStartPoint()));
					modifiedSubpathsIndices.Add(i);
				}
				++i;
			}
			return modifiedSubpathsIndices;
		}

		/// <summary>Path is empty if it contains no subpaths.</summary>
		public virtual bool IsEmpty()
		{
			return subpaths.Count == 0;
		}

		private Subpath GetLastSubpath()
		{
			System.Diagnostics.Debug.Assert(subpaths.Count > 0);
			return subpaths[subpaths.Count - 1];
		}
	}
}
