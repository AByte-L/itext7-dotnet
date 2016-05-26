/*

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

namespace iTextSharp.Kernel.Geom
{
	public class AffineTransform
	{
		public const int TYPE_IDENTITY = 0;

		public const int TYPE_TRANSLATION = 1;

		public const int TYPE_UNIFORM_SCALE = 2;

		public const int TYPE_GENERAL_SCALE = 4;

		public const int TYPE_QUADRANT_ROTATION = 8;

		public const int TYPE_GENERAL_ROTATION = 16;

		public const int TYPE_GENERAL_TRANSFORM = 32;

		public const int TYPE_FLIP = 64;

		public const int TYPE_MASK_SCALE = TYPE_UNIFORM_SCALE | TYPE_GENERAL_SCALE;

		public const int TYPE_MASK_ROTATION = TYPE_QUADRANT_ROTATION | TYPE_GENERAL_ROTATION;

		/// <summary>The <code>TYPE_UNKNOWN</code> is an initial type value</summary>
		internal const int TYPE_UNKNOWN = -1;

		/// <summary>The min value equivalent to zero.</summary>
		/// <remarks>The min value equivalent to zero. If absolute value less then ZERO it considered as zero.
		/// 	</remarks>
		internal const double ZERO = 1E-10;

		/// <summary>The values of transformation matrix</summary>
		internal double m00;

		internal double m10;

		internal double m01;

		internal double m11;

		internal double m02;

		internal double m12;

		/// <summary>The transformation <code>type</code></summary>
		[System.NonSerialized]
		internal int type;

		public AffineTransform()
		{
			type = TYPE_IDENTITY;
			m00 = m11 = 1;
			m10 = m01 = m02 = m12 = 0;
		}

		public AffineTransform(iTextSharp.Kernel.Geom.AffineTransform t)
		{
			this.type = t.type;
			this.m00 = t.m00;
			this.m10 = t.m10;
			this.m01 = t.m01;
			this.m11 = t.m11;
			this.m02 = t.m02;
			this.m12 = t.m12;
		}

		public AffineTransform(double m00, double m10, double m01, double m11, double m02
			, double m12)
		{
			this.type = TYPE_UNKNOWN;
			this.m00 = m00;
			this.m10 = m10;
			this.m01 = m01;
			this.m11 = m11;
			this.m02 = m02;
			this.m12 = m12;
		}

		public AffineTransform(float[] matrix)
		{
			this.type = TYPE_UNKNOWN;
			m00 = matrix[0];
			m10 = matrix[1];
			m01 = matrix[2];
			m11 = matrix[3];
			if (matrix.Length > 4)
			{
				m02 = matrix[4];
				m12 = matrix[5];
			}
		}

		public AffineTransform(double[] matrix)
		{
			this.type = TYPE_UNKNOWN;
			m00 = matrix[0];
			m10 = matrix[1];
			m01 = matrix[2];
			m11 = matrix[3];
			if (matrix.Length > 4)
			{
				m02 = matrix[4];
				m12 = matrix[5];
			}
		}

		/*
		* Method returns type of affine transformation.
		*
		* Transform matrix is
		*   m00 m01 m02
		*   m10 m11 m12
		*
		* According analytic geometry new basis vectors are (m00, m01) and (m10, m11),
		* translation vector is (m02, m12). Original basis vectors are (1, 0) and (0, 1).
		* Type transformations classification:
		*   TYPE_IDENTITY - new basis equals original one and zero translation
		*   TYPE_TRANSLATION - translation vector isn't zero
		*   TYPE_UNIFORM_SCALE - vectors length of new basis equals
		*   TYPE_GENERAL_SCALE - vectors length of new basis doesn't equal
		*   TYPE_FLIP - new basis vector orientation differ from original one
		*   TYPE_QUADRANT_ROTATION - new basis is rotated by 90, 180, 270, or 360 degrees
		*   TYPE_GENERAL_ROTATION - new basis is rotated by arbitrary angle
		*   TYPE_GENERAL_TRANSFORM - transformation can't be inversed
		*/
		public virtual int GetType()
		{
			if (this.type != TYPE_UNKNOWN)
			{
				return this.type;
			}
			int type = 0;
			if (m00 * m01 + m10 * m11 != 0.0)
			{
				type |= TYPE_GENERAL_TRANSFORM;
				return type;
			}
			if (m02 != 0.0 || m12 != 0.0)
			{
				type |= TYPE_TRANSLATION;
			}
			else
			{
				if (m00 == 1.0 && m11 == 1.0 && m01 == 0.0 && m10 == 0.0)
				{
					type = TYPE_IDENTITY;
					return type;
				}
			}
			if (m00 * m11 - m01 * m10 < 0.0)
			{
				type |= TYPE_FLIP;
			}
			double dx = m00 * m00 + m10 * m10;
			double dy = m01 * m01 + m11 * m11;
			if (dx != dy)
			{
				type |= TYPE_GENERAL_SCALE;
			}
			else
			{
				if (dx != 1.0)
				{
					type |= TYPE_UNIFORM_SCALE;
				}
			}
			if ((m00 == 0.0 && m11 == 0.0) || (m10 == 0.0 && m01 == 0.0 && (m00 < 0.0 || m11 
				< 0.0)))
			{
				type |= TYPE_QUADRANT_ROTATION;
			}
			else
			{
				if (m01 != 0.0 || m10 != 0.0)
				{
					type |= TYPE_GENERAL_ROTATION;
				}
			}
			return type;
		}

		public virtual double GetScaleX()
		{
			return m00;
		}

		public virtual double GetScaleY()
		{
			return m11;
		}

		public virtual double GetShearX()
		{
			return m01;
		}

		public virtual double GetShearY()
		{
			return m10;
		}

		public virtual double GetTranslateX()
		{
			return m02;
		}

		public virtual double GetTranslateY()
		{
			return m12;
		}

		public virtual bool IsIdentity()
		{
			return GetType() == TYPE_IDENTITY;
		}

		public virtual void GetMatrix(float[] matrix)
		{
			matrix[0] = (float)m00;
			matrix[1] = (float)m10;
			matrix[2] = (float)m01;
			matrix[3] = (float)m11;
			if (matrix.Length > 4)
			{
				matrix[4] = (float)m02;
				matrix[5] = (float)m12;
			}
		}

		public virtual void GetMatrix(double[] matrix)
		{
			matrix[0] = m00;
			matrix[1] = m10;
			matrix[2] = m01;
			matrix[3] = m11;
			if (matrix.Length > 4)
			{
				matrix[4] = m02;
				matrix[5] = m12;
			}
		}

		public virtual double GetDeterminant()
		{
			return m00 * m11 - m01 * m10;
		}

		public virtual void SetTransform(float m00, float m10, float m01, float m11, float
			 m02, float m12)
		{
			this.type = TYPE_UNKNOWN;
			this.m00 = m00;
			this.m10 = m10;
			this.m01 = m01;
			this.m11 = m11;
			this.m02 = m02;
			this.m12 = m12;
		}

		public virtual void SetTransform(double m00, double m10, double m01, double m11, 
			double m02, double m12)
		{
			this.type = TYPE_UNKNOWN;
			this.m00 = m00;
			this.m10 = m10;
			this.m01 = m01;
			this.m11 = m11;
			this.m02 = m02;
			this.m12 = m12;
		}

		public virtual void SetTransform(iTextSharp.Kernel.Geom.AffineTransform t)
		{
			type = t.type;
			SetTransform(t.m00, t.m10, t.m01, t.m11, t.m02, t.m12);
		}

		public virtual void SetToIdentity()
		{
			type = TYPE_IDENTITY;
			m00 = m11 = 1;
			m10 = m01 = m02 = m12 = 0;
		}

		public virtual void SetToTranslation(double mx, double my)
		{
			m00 = m11 = 1;
			m01 = m10 = 0;
			m02 = mx;
			m12 = my;
			if (mx == 0 && my == 0)
			{
				type = TYPE_IDENTITY;
			}
			else
			{
				type = TYPE_TRANSLATION;
			}
		}

		public virtual void SetToScale(double scx, double scy)
		{
			m00 = scx;
			m11 = scy;
			m10 = m01 = m02 = m12 = 0;
			if (scx != 1.0 || scy != 1)
			{
				type = TYPE_UNKNOWN;
			}
			else
			{
				type = TYPE_IDENTITY;
			}
		}

		public virtual void SetToShear(double shx, double shy)
		{
			m00 = m11 = 1;
			m02 = m12 = 0;
			m01 = shx;
			m10 = shy;
			if (shx != 0.0 || shy != 0.0)
			{
				type = TYPE_UNKNOWN;
			}
			else
			{
				type = TYPE_IDENTITY;
			}
		}

		public virtual void SetToRotation(double angle)
		{
			double sin = Math.Sin(angle);
			double cos = Math.Cos(angle);
			if (Math.Abs(cos) < ZERO)
			{
				cos = 0.0;
				sin = sin > 0.0 ? 1.0 : -1.0;
			}
			else
			{
				if (Math.Abs(sin) < ZERO)
				{
					sin = 0.0;
					cos = cos > 0.0 ? 1.0 : -1.0;
				}
			}
			m00 = m11 = (float)cos;
			m01 = (float)-sin;
			m10 = (float)sin;
			m02 = m12 = 0;
			type = TYPE_UNKNOWN;
		}

		public virtual void SetToRotation(double angle, double px, double py)
		{
			SetToRotation(angle);
			m02 = px * (1 - m00) + py * m10;
			m12 = py * (1 - m00) - px * m10;
			type = TYPE_UNKNOWN;
		}

		public static iTextSharp.Kernel.Geom.AffineTransform GetTranslateInstance(double 
			mx, double my)
		{
			iTextSharp.Kernel.Geom.AffineTransform t = new iTextSharp.Kernel.Geom.AffineTransform
				();
			t.SetToTranslation(mx, my);
			return t;
		}

		public static iTextSharp.Kernel.Geom.AffineTransform GetScaleInstance(double scx, 
			double scY)
		{
			iTextSharp.Kernel.Geom.AffineTransform t = new iTextSharp.Kernel.Geom.AffineTransform
				();
			t.SetToScale(scx, scY);
			return t;
		}

		public static iTextSharp.Kernel.Geom.AffineTransform GetShearInstance(double shx, 
			double shy)
		{
			iTextSharp.Kernel.Geom.AffineTransform m = new iTextSharp.Kernel.Geom.AffineTransform
				();
			m.SetToShear(shx, shy);
			return m;
		}

		public static iTextSharp.Kernel.Geom.AffineTransform GetRotateInstance(double angle
			)
		{
			iTextSharp.Kernel.Geom.AffineTransform t = new iTextSharp.Kernel.Geom.AffineTransform
				();
			t.SetToRotation(angle);
			return t;
		}

		public static iTextSharp.Kernel.Geom.AffineTransform GetRotateInstance(double angle
			, double x, double y)
		{
			iTextSharp.Kernel.Geom.AffineTransform t = new iTextSharp.Kernel.Geom.AffineTransform
				();
			t.SetToRotation(angle, x, y);
			return t;
		}

		public virtual void Translate(double mx, double my)
		{
			Concatenate(iTextSharp.Kernel.Geom.AffineTransform.GetTranslateInstance(mx, my));
		}

		public virtual void Scale(double scx, double scy)
		{
			Concatenate(iTextSharp.Kernel.Geom.AffineTransform.GetScaleInstance(scx, scy));
		}

		public virtual void Shear(double shx, double shy)
		{
			Concatenate(iTextSharp.Kernel.Geom.AffineTransform.GetShearInstance(shx, shy));
		}

		public virtual void Rotate(double angle)
		{
			Concatenate(iTextSharp.Kernel.Geom.AffineTransform.GetRotateInstance(angle));
		}

		public virtual void Rotate(double angle, double px, double py)
		{
			Concatenate(iTextSharp.Kernel.Geom.AffineTransform.GetRotateInstance(angle, px, py
				));
		}

		/// <summary>Multiply matrix of two AffineTransform objects</summary>
		/// <param name="t1">- the AffineTransform object is a multiplicand</param>
		/// <param name="t2">- the AffineTransform object is a multiplier</param>
		/// <returns>an AffineTransform object that is a result of t1 multiplied by matrix t2.
		/// 	</returns>
		internal virtual iTextSharp.Kernel.Geom.AffineTransform Multiply(iTextSharp.Kernel.Geom.AffineTransform
			 t1, iTextSharp.Kernel.Geom.AffineTransform t2)
		{
			return new iTextSharp.Kernel.Geom.AffineTransform(t1.m00 * t2.m00 + t1.m10 * t2.m01
				, t1.m00 * t2.m10 + t1.m10 * t2.m11, t1.m01 * t2.m00 + t1.m11 * t2.m01, t1.m01 *
				 t2.m10 + t1.m11 * t2.m11, t1.m02 * t2.m00 + t1.m12 * t2.m01 + t2.m02, t1.m02 * 
				t2.m10 + t1.m12 * t2.m11 + t2.m12);
		}

		// m00
		// m01
		// m10
		// m11
		// m02
		// m12
		public virtual void Concatenate(iTextSharp.Kernel.Geom.AffineTransform t)
		{
			SetTransform(Multiply(t, this));
		}

		public virtual void PreConcatenate(iTextSharp.Kernel.Geom.AffineTransform t)
		{
			SetTransform(Multiply(this, t));
		}

		/// <exception cref="iTextSharp.Kernel.Geom.NoninvertibleTransformException"/>
		public virtual iTextSharp.Kernel.Geom.AffineTransform CreateInverse()
		{
			double det = GetDeterminant();
			if (Math.Abs(det) < ZERO)
			{
				// awt.204=Determinant is zero
				throw new NoninvertibleTransformException("Determinant is zero. Cannot invert transformation"
					);
			}
			//$NON-NLS-1$
			return new iTextSharp.Kernel.Geom.AffineTransform(m11 / det, -m10 / det, -m01 / det
				, m00 / det, (m01 * m12 - m11 * m02) / det, (m10 * m02 - m00 * m12) / det);
		}

		// m00
		// m10
		// m01
		// m11
		// m02
		// m12
		public virtual Point Transform(Point src, Point dst)
		{
			if (dst == null)
			{
				dst = new Point();
			}
			double x = src.GetX();
			double y = src.GetY();
			dst.SetLocation(x * m00 + y * m01 + m02, x * m10 + y * m11 + m12);
			return dst;
		}

		public virtual void Transform(Point[] src, int srcOff, Point[] dst, int dstOff, int
			 length)
		{
			while (--length >= 0)
			{
				Point srcPoint = src[srcOff++];
				double x = srcPoint.GetX();
				double y = srcPoint.GetY();
				Point dstPoint = dst[dstOff];
				if (dstPoint == null)
				{
					dstPoint = new Point();
				}
				dstPoint.SetLocation(x * m00 + y * m01 + m02, x * m10 + y * m11 + m12);
				dst[dstOff++] = dstPoint;
			}
		}

		public virtual void Transform(double[] src, int srcOff, double[] dst, int dstOff, 
			int length)
		{
			int step = 2;
			if (src == dst && srcOff < dstOff && dstOff < srcOff + length * 2)
			{
				srcOff = srcOff + length * 2 - 2;
				dstOff = dstOff + length * 2 - 2;
				step = -2;
			}
			while (--length >= 0)
			{
				double x = src[srcOff + 0];
				double y = src[srcOff + 1];
				dst[dstOff + 0] = x * m00 + y * m01 + m02;
				dst[dstOff + 1] = x * m10 + y * m11 + m12;
				srcOff += step;
				dstOff += step;
			}
		}

		public virtual void Transform(float[] src, int srcOff, float[] dst, int dstOff, int
			 length)
		{
			int step = 2;
			if (src == dst && srcOff < dstOff && dstOff < srcOff + length * 2)
			{
				srcOff = srcOff + length * 2 - 2;
				dstOff = dstOff + length * 2 - 2;
				step = -2;
			}
			while (--length >= 0)
			{
				float x = src[srcOff + 0];
				float y = src[srcOff + 1];
				dst[dstOff + 0] = (float)(x * m00 + y * m01 + m02);
				dst[dstOff + 1] = (float)(x * m10 + y * m11 + m12);
				srcOff += step;
				dstOff += step;
			}
		}

		public virtual void Transform(float[] src, int srcOff, double[] dst, int dstOff, 
			int length)
		{
			while (--length >= 0)
			{
				float x = src[srcOff++];
				float y = src[srcOff++];
				dst[dstOff++] = x * m00 + y * m01 + m02;
				dst[dstOff++] = x * m10 + y * m11 + m12;
			}
		}

		public virtual void Transform(double[] src, int srcOff, float[] dst, int dstOff, 
			int length)
		{
			while (--length >= 0)
			{
				double x = src[srcOff++];
				double y = src[srcOff++];
				dst[dstOff++] = (float)(x * m00 + y * m01 + m02);
				dst[dstOff++] = (float)(x * m10 + y * m11 + m12);
			}
		}

		public virtual Point DeltaTransform(Point src, Point dst)
		{
			if (dst == null)
			{
				dst = new Point();
			}
			double x = src.GetX();
			double y = src.GetY();
			dst.SetLocation(x * m00 + y * m01, x * m10 + y * m11);
			return dst;
		}

		public virtual void DeltaTransform(double[] src, int srcOff, double[] dst, int dstOff
			, int length)
		{
			while (--length >= 0)
			{
				double x = src[srcOff++];
				double y = src[srcOff++];
				dst[dstOff++] = x * m00 + y * m01;
				dst[dstOff++] = x * m10 + y * m11;
			}
		}

		/// <exception cref="iTextSharp.Kernel.Geom.NoninvertibleTransformException"/>
		public virtual Point InverseTransform(Point src, Point dst)
		{
			double det = GetDeterminant();
			if (Math.Abs(det) < ZERO)
			{
				// awt.204=Determinant is zero
				throw new NoninvertibleTransformException("Determinant is zero. Cannot invert transformation"
					);
			}
			//$NON-NLS-1$
			if (dst == null)
			{
				dst = new Point();
			}
			double x = src.GetX() - m02;
			double y = src.GetY() - m12;
			dst.SetLocation((x * m11 - y * m01) / det, (y * m00 - x * m10) / det);
			return dst;
		}

		/// <exception cref="iTextSharp.Kernel.Geom.NoninvertibleTransformException"/>
		public virtual void InverseTransform(double[] src, int srcOff, double[] dst, int 
			dstOff, int length)
		{
			double det = GetDeterminant();
			if (Math.Abs(det) < ZERO)
			{
				// awt.204=Determinant is zero
				throw new NoninvertibleTransformException("Determinant is zero. Cannot invert transformation"
					);
			}
			//$NON-NLS-1$
			while (--length >= 0)
			{
				double x = src[srcOff++] - m02;
				double y = src[srcOff++] - m12;
				dst[dstOff++] = (x * m11 - y * m01) / det;
				dst[dstOff++] = (y * m00 - x * m10) / det;
			}
		}

		/// <exception cref="iTextSharp.Kernel.Geom.NoninvertibleTransformException"/>
		public virtual void InverseTransform(float[] src, int srcOff, float[] dst, int dstOff
			, int length)
		{
			float det = (float)GetDeterminant();
			if (Math.Abs(det) < ZERO)
			{
				// awt.204=Determinant is zero
				throw new NoninvertibleTransformException("Determinant is zero. Cannot invert transformation"
					);
			}
			//$NON-NLS-1$
			while (--length >= 0)
			{
				float x = (float)(src[srcOff++] - m02);
				float y = (float)(src[srcOff++] - m12);
				dst[dstOff++] = (float)((x * m11 - y * m01) / det);
				dst[dstOff++] = (float)((y * m00 - x * m10) / det);
			}
		}

		/// <exception cref="Java.Lang.CloneNotSupportedException"/>
		public virtual iTextSharp.Kernel.Geom.AffineTransform Clone()
		{
			return new iTextSharp.Kernel.Geom.AffineTransform(this);
		}
	}
}
