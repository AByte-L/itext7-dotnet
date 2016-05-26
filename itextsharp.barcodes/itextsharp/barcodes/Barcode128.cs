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
using System.Collections.Generic;
using System.Text;
using iTextSharp.IO.Font;
using iTextSharp.Kernel;
using iTextSharp.Kernel.Font;
using iTextSharp.Kernel.Geom;
using iTextSharp.Kernel.Pdf;
using iTextSharp.Kernel.Pdf.Canvas;

namespace iTextSharp.Barcodes
{
	public class Barcode128 : Barcode1D
	{
		/// <summary>A type of barcode</summary>
		public const int CODE128 = 1;

		/// <summary>A type of barcode</summary>
		public const int CODE128_UCC = 2;

		/// <summary>A type of barcode</summary>
		public const int CODE128_RAW = 3;

		/// <summary>The bars to generate the code.</summary>
		private static readonly byte[][] BARS = new byte[][] { new byte[] { 2, 1, 2, 2, 2
			, 2 }, new byte[] { 2, 2, 2, 1, 2, 2 }, new byte[] { 2, 2, 2, 2, 2, 1 }, new byte
			[] { 1, 2, 1, 2, 2, 3 }, new byte[] { 1, 2, 1, 3, 2, 2 }, new byte[] { 1, 3, 1, 
			2, 2, 2 }, new byte[] { 1, 2, 2, 2, 1, 3 }, new byte[] { 1, 2, 2, 3, 1, 2 }, new 
			byte[] { 1, 3, 2, 2, 1, 2 }, new byte[] { 2, 2, 1, 2, 1, 3 }, new byte[] { 2, 2, 
			1, 3, 1, 2 }, new byte[] { 2, 3, 1, 2, 1, 2 }, new byte[] { 1, 1, 2, 2, 3, 2 }, 
			new byte[] { 1, 2, 2, 1, 3, 2 }, new byte[] { 1, 2, 2, 2, 3, 1 }, new byte[] { 1
			, 1, 3, 2, 2, 2 }, new byte[] { 1, 2, 3, 1, 2, 2 }, new byte[] { 1, 2, 3, 2, 2, 
			1 }, new byte[] { 2, 2, 3, 2, 1, 1 }, new byte[] { 2, 2, 1, 1, 3, 2 }, new byte[
			] { 2, 2, 1, 2, 3, 1 }, new byte[] { 2, 1, 3, 2, 1, 2 }, new byte[] { 2, 2, 3, 1
			, 1, 2 }, new byte[] { 3, 1, 2, 1, 3, 1 }, new byte[] { 3, 1, 1, 2, 2, 2 }, new 
			byte[] { 3, 2, 1, 1, 2, 2 }, new byte[] { 3, 2, 1, 2, 2, 1 }, new byte[] { 3, 1, 
			2, 2, 1, 2 }, new byte[] { 3, 2, 2, 1, 1, 2 }, new byte[] { 3, 2, 2, 2, 1, 1 }, 
			new byte[] { 2, 1, 2, 1, 2, 3 }, new byte[] { 2, 1, 2, 3, 2, 1 }, new byte[] { 2
			, 3, 2, 1, 2, 1 }, new byte[] { 1, 1, 1, 3, 2, 3 }, new byte[] { 1, 3, 1, 1, 2, 
			3 }, new byte[] { 1, 3, 1, 3, 2, 1 }, new byte[] { 1, 1, 2, 3, 1, 3 }, new byte[
			] { 1, 3, 2, 1, 1, 3 }, new byte[] { 1, 3, 2, 3, 1, 1 }, new byte[] { 2, 1, 1, 3
			, 1, 3 }, new byte[] { 2, 3, 1, 1, 1, 3 }, new byte[] { 2, 3, 1, 3, 1, 1 }, new 
			byte[] { 1, 1, 2, 1, 3, 3 }, new byte[] { 1, 1, 2, 3, 3, 1 }, new byte[] { 1, 3, 
			2, 1, 3, 1 }, new byte[] { 1, 1, 3, 1, 2, 3 }, new byte[] { 1, 1, 3, 3, 2, 1 }, 
			new byte[] { 1, 3, 3, 1, 2, 1 }, new byte[] { 3, 1, 3, 1, 2, 1 }, new byte[] { 2
			, 1, 1, 3, 3, 1 }, new byte[] { 2, 3, 1, 1, 3, 1 }, new byte[] { 2, 1, 3, 1, 1, 
			3 }, new byte[] { 2, 1, 3, 3, 1, 1 }, new byte[] { 2, 1, 3, 1, 3, 1 }, new byte[
			] { 3, 1, 1, 1, 2, 3 }, new byte[] { 3, 1, 1, 3, 2, 1 }, new byte[] { 3, 3, 1, 1
			, 2, 1 }, new byte[] { 3, 1, 2, 1, 1, 3 }, new byte[] { 3, 1, 2, 3, 1, 1 }, new 
			byte[] { 3, 3, 2, 1, 1, 1 }, new byte[] { 3, 1, 4, 1, 1, 1 }, new byte[] { 2, 2, 
			1, 4, 1, 1 }, new byte[] { 4, 3, 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 2, 2, 4 }, 
			new byte[] { 1, 1, 1, 4, 2, 2 }, new byte[] { 1, 2, 1, 1, 2, 4 }, new byte[] { 1
			, 2, 1, 4, 2, 1 }, new byte[] { 1, 4, 1, 1, 2, 2 }, new byte[] { 1, 4, 1, 2, 2, 
			1 }, new byte[] { 1, 1, 2, 2, 1, 4 }, new byte[] { 1, 1, 2, 4, 1, 2 }, new byte[
			] { 1, 2, 2, 1, 1, 4 }, new byte[] { 1, 2, 2, 4, 1, 1 }, new byte[] { 1, 4, 2, 1
			, 1, 2 }, new byte[] { 1, 4, 2, 2, 1, 1 }, new byte[] { 2, 4, 1, 2, 1, 1 }, new 
			byte[] { 2, 2, 1, 1, 1, 4 }, new byte[] { 4, 1, 3, 1, 1, 1 }, new byte[] { 2, 4, 
			1, 1, 1, 2 }, new byte[] { 1, 3, 4, 1, 1, 1 }, new byte[] { 1, 1, 1, 2, 4, 2 }, 
			new byte[] { 1, 2, 1, 1, 4, 2 }, new byte[] { 1, 2, 1, 2, 4, 1 }, new byte[] { 1
			, 1, 4, 2, 1, 2 }, new byte[] { 1, 2, 4, 1, 1, 2 }, new byte[] { 1, 2, 4, 2, 1, 
			1 }, new byte[] { 4, 1, 1, 2, 1, 2 }, new byte[] { 4, 2, 1, 1, 1, 2 }, new byte[
			] { 4, 2, 1, 2, 1, 1 }, new byte[] { 2, 1, 2, 1, 4, 1 }, new byte[] { 2, 1, 4, 1
			, 2, 1 }, new byte[] { 4, 1, 2, 1, 2, 1 }, new byte[] { 1, 1, 1, 1, 4, 3 }, new 
			byte[] { 1, 1, 1, 3, 4, 1 }, new byte[] { 1, 3, 1, 1, 4, 1 }, new byte[] { 1, 1, 
			4, 1, 1, 3 }, new byte[] { 1, 1, 4, 3, 1, 1 }, new byte[] { 4, 1, 1, 1, 1, 3 }, 
			new byte[] { 4, 1, 1, 3, 1, 1 }, new byte[] { 1, 1, 3, 1, 4, 1 }, new byte[] { 1
			, 1, 4, 1, 3, 1 }, new byte[] { 3, 1, 1, 1, 4, 1 }, new byte[] { 4, 1, 1, 1, 3, 
			1 }, new byte[] { 2, 1, 1, 4, 1, 2 }, new byte[] { 2, 1, 1, 2, 1, 4 }, new byte[
			] { 2, 1, 1, 2, 3, 2 } };

		/// <summary>The stop bars.</summary>
		private static readonly byte[] BARS_STOP = new byte[] { 2, 3, 3, 1, 1, 1, 2 };

		/// <summary>The charset code change.</summary>
		public const char CODE_AB_TO_C = (char)99;

		/// <summary>The charset code change.</summary>
		public const char CODE_AC_TO_B = (char)100;

		/// <summary>The charset code change.</summary>
		public const char CODE_BC_TO_A = (char)101;

		/// <summary>The code for UCC/EAN-128.</summary>
		public const char FNC1_INDEX = (char)102;

		/// <summary>The start code.</summary>
		public const char START_A = (char)103;

		/// <summary>The start code.</summary>
		public const char START_B = (char)104;

		/// <summary>The start code.</summary>
		public const char START_C = (char)105;

		public const char FNC1 = '\u00ca';

		public const char DEL = '\u00c3';

		public const char FNC3 = '\u00c4';

		public const char FNC2 = '\u00c5';

		public const char SHIFT = '\u00c6';

		public const char CODE_C = '\u00c7';

		public const char CODE_A = '\u00c8';

		public const char FNC4 = '\u00c8';

		public const char STARTA = '\u00cb';

		public const char STARTB = '\u00cc';

		public const char STARTC = '\u00cd';

		private static IDictionary<int, int?> ais = new Dictionary<int, int?>();

		/// <summary>Creates new Barcode128</summary>
		public Barcode128(PdfDocument document)
			: base(document)
		{
			try
			{
				x = 0.8f;
				font = PdfFontFactory.CreateFont(FontConstants.HELVETICA, PdfEncodings.WINANSI);
				size = 8;
				baseline = size;
				barHeight = size * 3;
				textAlignment = ALIGN_CENTER;
				codeType = CODE128;
			}
			catch (System.IO.IOException e)
			{
				throw new Exception("Cannot create font", e);
			}
		}

		public enum Barcode128CodeSet
		{
			A,
			B,
			C,
			AUTO
		}

		public virtual void SetCodeSet(Barcode128.Barcode128CodeSet codeSet)
		{
			this.codeSet = codeSet;
		}

		public virtual Barcode128.Barcode128CodeSet GetCodeSet()
		{
			return this.codeSet;
		}

		private Barcode128.Barcode128CodeSet codeSet = Barcode128.Barcode128CodeSet.AUTO;

		/// <summary>Removes the FNC1 codes in the text.</summary>
		/// <param name="code">the text to clean</param>
		/// <returns>the cleaned text</returns>
		public static String RemoveFNC1(String code)
		{
			int len = code.Length;
			StringBuilder buf = new StringBuilder(len);
			for (int k = 0; k < len; ++k)
			{
				char c = code[k];
				if (c >= 32 && c <= 126)
				{
					buf.Append(c);
				}
			}
			return buf.ToString();
		}

		/// <summary>Gets the human readable text of a sequence of AI.</summary>
		/// <param name="code">the text</param>
		/// <returns>the human readable text</returns>
		public static String GetHumanReadableUCCEAN(String code)
		{
			StringBuilder buf = new StringBuilder();
			String fnc1 = new String(new char[] { FNC1 });
			while (true)
			{
				if (code.StartsWith(fnc1))
				{
					code = code.Substring(1);
					continue;
				}
				int n = 0;
				int idlen = 0;
				for (int k = 2; k < 5; ++k)
				{
					if (code.Length < k)
					{
						break;
					}
					int subcode = System.Convert.ToInt32(code.JSubstring(0, k));
					n = ais.ContainsKey(subcode) ? (int)ais[subcode] : 0;
					if (n != 0)
					{
						idlen = k;
						break;
					}
				}
				if (idlen == 0)
				{
					break;
				}
				buf.Append('(').Append(code.JSubstring(0, idlen)).Append(')');
				code = code.Substring(idlen);
				if (n > 0)
				{
					n -= idlen;
					if (code.Length <= n)
					{
						break;
					}
					buf.Append(RemoveFNC1(code.JSubstring(0, n)));
					code = code.Substring(n);
				}
				else
				{
					int idx = code.IndexOf(FNC1);
					if (idx < 0)
					{
						break;
					}
					buf.Append(code.JSubstring(0, idx));
					code = code.Substring(idx + 1);
				}
			}
			buf.Append(RemoveFNC1(code));
			return buf.ToString();
		}

		/// <summary>
		/// Converts the human readable text to the characters needed to
		/// create a barcode using the specified code set.
		/// </summary>
		/// <param name="text">the text to convert</param>
		/// <param name="ucc">
		/// <CODE>true</CODE> if it is an UCC/EAN-128. In this case
		/// the character FNC1 is added
		/// </param>
		/// <param name="codeSet">forced code set, or AUTO for optimized barcode.</param>
		/// <returns>the code ready to be fed to getBarsCode128Raw()</returns>
		public static String GetRawText(String text, bool ucc, Barcode128.Barcode128CodeSet
			 codeSet)
		{
			String @out = "";
			int tLen = text.Length;
			if (tLen == 0)
			{
				@out += GetStartSymbol(codeSet);
				if (ucc)
				{
					@out += FNC1_INDEX;
				}
				return @out;
			}
			int c;
			for (int k = 0; k < tLen; ++k)
			{
				c = text[k];
				if (c > 127 && c != FNC1)
				{
					throw new PdfException(PdfException.ThereAreIllegalCharactersForBarcode128In1);
				}
			}
			c = text[0];
			char currentCode = START_B;
			int index = 0;
			if ((codeSet == Barcode128.Barcode128CodeSet.AUTO || codeSet == Barcode128.Barcode128CodeSet
				.C) && IsNextDigits(text, index, 2))
			{
				currentCode = START_C;
				@out += currentCode;
				if (ucc)
				{
					@out += FNC1_INDEX;
				}
				String out2 = GetPackedRawDigits(text, index, 2);
				index += out2[0];
				@out += out2.Substring(1);
			}
			else
			{
				if (c < ' ')
				{
					currentCode = START_A;
					@out += currentCode;
					if (ucc)
					{
						@out += FNC1_INDEX;
					}
					@out += (char)(c + 64);
					++index;
				}
				else
				{
					@out += currentCode;
					if (ucc)
					{
						@out += FNC1_INDEX;
					}
					if (c == FNC1)
					{
						@out += FNC1_INDEX;
					}
					else
					{
						@out += (char)(c - ' ');
					}
					++index;
				}
			}
			if (codeSet != Barcode128.Barcode128CodeSet.AUTO && currentCode != GetStartSymbol
				(codeSet))
			{
				throw new PdfException(PdfException.ThereAreIllegalCharactersForBarcode128In1);
			}
			while (index < tLen)
			{
				switch (currentCode)
				{
					case START_A:
					{
						if (codeSet == Barcode128.Barcode128CodeSet.AUTO && IsNextDigits(text, index, 4))
						{
							currentCode = START_C;
							@out += CODE_AB_TO_C;
							String out2 = GetPackedRawDigits(text, index, 4);
							index += out2[0];
							@out += out2.Substring(1);
						}
						else
						{
							c = text[index++];
							if (c == FNC1)
							{
								@out += FNC1_INDEX;
							}
							else
							{
								if (c > '_')
								{
									currentCode = START_B;
									@out += CODE_AC_TO_B;
									@out += (char)(c - ' ');
								}
								else
								{
									if (c < ' ')
									{
										@out += (char)(c + 64);
									}
									else
									{
										@out += (char)(c - ' ');
									}
								}
							}
						}
						break;
					}

					case START_B:
					{
						if (codeSet == Barcode128.Barcode128CodeSet.AUTO && IsNextDigits(text, index, 4))
						{
							currentCode = START_C;
							@out += CODE_AB_TO_C;
							String out2 = GetPackedRawDigits(text, index, 4);
							index += out2[0];
							@out += out2.Substring(1);
						}
						else
						{
							c = text[index++];
							if (c == FNC1)
							{
								@out += FNC1_INDEX;
							}
							else
							{
								if (c < ' ')
								{
									currentCode = START_A;
									@out += CODE_BC_TO_A;
									@out += (char)(c + 64);
								}
								else
								{
									@out += (char)(c - ' ');
								}
							}
						}
						break;
					}

					case START_C:
					{
						if (IsNextDigits(text, index, 2))
						{
							String out2 = GetPackedRawDigits(text, index, 2);
							index += out2[0];
							@out += out2.Substring(1);
						}
						else
						{
							c = text[index++];
							if (c == FNC1)
							{
								@out += FNC1_INDEX;
							}
							else
							{
								if (c < ' ')
								{
									currentCode = START_A;
									@out += CODE_BC_TO_A;
									@out += (char)(c + 64);
								}
								else
								{
									currentCode = START_B;
									@out += CODE_AC_TO_B;
									@out += (char)(c - ' ');
								}
							}
						}
						break;
					}
				}
				if (codeSet != Barcode128.Barcode128CodeSet.AUTO && currentCode != GetStartSymbol
					(codeSet))
				{
					throw new PdfException(PdfException.ThereAreIllegalCharactersForBarcode128In1);
				}
			}
			return @out;
		}

		/// <summary>
		/// Converts the human readable text to the characters needed to
		/// create a barcode.
		/// </summary>
		/// <remarks>
		/// Converts the human readable text to the characters needed to
		/// create a barcode. Some optimization is done to get the shortest code.
		/// </remarks>
		/// <param name="text">the text to convert</param>
		/// <param name="ucc">
		/// <CODE>true</CODE> if it is an UCC/EAN-128. In this case
		/// the character FNC1 is added
		/// </param>
		/// <returns>the code ready to be fed to getBarsCode128Raw()</returns>
		public static String GetRawText(String text, bool ucc)
		{
			return GetRawText(text, ucc, Barcode128.Barcode128CodeSet.AUTO);
		}

		/// <summary>Generates the bars.</summary>
		/// <remarks>
		/// Generates the bars. The input has the actual barcodes, not
		/// the human readable text.
		/// </remarks>
		/// <param name="text">the barcode</param>
		/// <returns>the bars</returns>
		public static byte[] GetBarsCode128Raw(String text)
		{
			int idx = text.IndexOf('\uffff');
			if (idx >= 0)
			{
				text = text.JSubstring(0, idx);
			}
			int chk = text[0];
			for (int k = 1; k < text.Length; ++k)
			{
				chk += k * text[k];
			}
			chk = chk % 103;
			text += (char)chk;
			byte[] bars = new byte[(text.Length + 1) * 6 + 7];
			int k_1;
			for (k_1 = 0; k_1 < text.Length; ++k_1)
			{
				System.Array.Copy(BARS[text[k_1]], 0, bars, k_1 * 6, 6);
			}
			System.Array.Copy(BARS_STOP, 0, bars, k_1 * 6, 7);
			return bars;
		}

		/// <summary>
		/// Gets the maximum area that the barcode and the text, if
		/// any, will occupy.
		/// </summary>
		/// <remarks>
		/// Gets the maximum area that the barcode and the text, if
		/// any, will occupy. The lower left corner is always (0, 0).
		/// </remarks>
		/// <returns>the size the barcode occupies.</returns>
		public override Rectangle GetBarcodeSize()
		{
			float fontX = 0;
			float fontY = 0;
			String fullCode;
			if (font != null)
			{
				if (baseline > 0)
				{
					fontY = baseline - GetDescender();
				}
				else
				{
					fontY = -baseline + size;
				}
				if (codeType == CODE128_RAW)
				{
					int idx = code.IndexOf('\uffff');
					if (idx < 0)
					{
						fullCode = "";
					}
					else
					{
						fullCode = code.Substring(idx + 1);
					}
				}
				else
				{
					if (codeType == CODE128_UCC)
					{
						fullCode = GetHumanReadableUCCEAN(code);
					}
					else
					{
						fullCode = RemoveFNC1(code);
					}
				}
				fontX = font.GetWidth(altText != null ? altText : fullCode, size);
			}
			if (codeType == CODE128_RAW)
			{
				int idx = code.IndexOf('\uffff');
				if (idx >= 0)
				{
					fullCode = code.JSubstring(0, idx);
				}
				else
				{
					fullCode = code;
				}
			}
			else
			{
				fullCode = GetRawText(code, codeType == CODE128_UCC, codeSet);
			}
			int len = fullCode.Length;
			float fullWidth = (len + 2) * 11 * x + 2 * x;
			fullWidth = Math.Max(fullWidth, fontX);
			float fullHeight = barHeight + fontY;
			return new Rectangle(fullWidth, fullHeight);
		}

		/// <summary>Places the barcode in a <CODE>PdfCanvas</CODE>.</summary>
		/// <remarks>
		/// Places the barcode in a <CODE>PdfCanvas</CODE>. The
		/// barcode is always placed at coordinates (0, 0). Use the
		/// translation matrix to move it elsewhere.<p>
		/// The bars and text are written in the following colors:<p>
		/// <P><TABLE BORDER=1>
		/// <TR>
		/// <TH><P><CODE>barColor</CODE></TH>
		/// <TH><P><CODE>textColor</CODE></TH>
		/// <TH><P>Result</TH>
		/// </TR>
		/// <TR>
		/// <TD><P><CODE>null</CODE></TD>
		/// <TD><P><CODE>null</CODE></TD>
		/// <TD><P>bars and text painted with current fill color</TD>
		/// </TR>
		/// <TR>
		/// <TD><P><CODE>barColor</CODE></TD>
		/// <TD><P><CODE>null</CODE></TD>
		/// <TD><P>bars and text painted with <CODE>barColor</CODE></TD>
		/// </TR>
		/// <TR>
		/// <TD><P><CODE>null</CODE></TD>
		/// <TD><P><CODE>textColor</CODE></TD>
		/// <TD><P>bars painted with current color<br />text painted with <CODE>textColor</CODE></TD>
		/// </TR>
		/// <TR>
		/// <TD><P><CODE>barColor</CODE></TD>
		/// <TD><P><CODE>textColor</CODE></TD>
		/// <TD><P>bars painted with <CODE>barColor</CODE><br />text painted with <CODE>textColor</CODE></TD>
		/// </TR>
		/// </TABLE>
		/// </remarks>
		/// <param name="canvas">the <CODE>PdfCanvas</CODE> where the barcode will be placed</param>
		/// <param name="barColor">the color of the bars. It can be <CODE>null</CODE></param>
		/// <param name="textColor">the color of the text. It can be <CODE>null</CODE></param>
		/// <returns>the dimensions the barcode occupies</returns>
		public override Rectangle PlaceBarcode(PdfCanvas canvas, iTextSharp.Kernel.Color.Color
			 barColor, iTextSharp.Kernel.Color.Color textColor)
		{
			String fullCode;
			if (codeType == CODE128_RAW)
			{
				int idx = code.IndexOf('\uffff');
				if (idx < 0)
				{
					fullCode = "";
				}
				else
				{
					fullCode = code.Substring(idx + 1);
				}
			}
			else
			{
				if (codeType == CODE128_UCC)
				{
					fullCode = GetHumanReadableUCCEAN(code);
				}
				else
				{
					fullCode = RemoveFNC1(code);
				}
			}
			float fontX = 0;
			if (font != null)
			{
				fontX = font.GetWidth(fullCode = altText != null ? altText : fullCode, size);
			}
			String bCode;
			if (codeType == CODE128_RAW)
			{
				int idx = code.IndexOf('\uffff');
				if (idx >= 0)
				{
					bCode = code.JSubstring(0, idx);
				}
				else
				{
					bCode = code;
				}
			}
			else
			{
				bCode = GetRawText(code, codeType == CODE128_UCC, codeSet);
			}
			int len = bCode.Length;
			float fullWidth = (len + 2) * 11 * x + 2 * x;
			float barStartX = 0;
			float textStartX = 0;
			switch (textAlignment)
			{
				case ALIGN_LEFT:
				{
					break;
				}

				case ALIGN_RIGHT:
				{
					if (fontX > fullWidth)
					{
						barStartX = fontX - fullWidth;
					}
					else
					{
						textStartX = fullWidth - fontX;
					}
					break;
				}

				default:
				{
					if (fontX > fullWidth)
					{
						barStartX = (fontX - fullWidth) / 2;
					}
					else
					{
						textStartX = (fullWidth - fontX) / 2;
					}
					break;
				}
			}
			float barStartY = 0;
			float textStartY = 0;
			if (font != null)
			{
				if (baseline <= 0)
				{
					textStartY = barHeight - baseline;
				}
				else
				{
					textStartY = -GetDescender();
					barStartY = textStartY + baseline;
				}
			}
			byte[] bars = GetBarsCode128Raw(bCode);
			bool print = true;
			if (barColor != null)
			{
				canvas.SetFillColor(barColor);
			}
			for (int k = 0; k < bars.Length; ++k)
			{
				float w = bars[k] * x;
				if (print)
				{
					canvas.Rectangle(barStartX, barStartY, w - inkSpreading, barHeight);
				}
				print = !print;
				barStartX += w;
			}
			canvas.Fill();
			if (font != null)
			{
				if (textColor != null)
				{
					canvas.SetFillColor(textColor);
				}
				canvas.BeginText();
				canvas.SetFontAndSize(font, size);
				canvas.SetTextMatrix(textStartX, textStartY);
				canvas.ShowText(fullCode);
				canvas.EndText();
			}
			return GetBarcodeSize();
		}

		/// <summary>Sets the code to generate.</summary>
		/// <remarks>
		/// Sets the code to generate. If it's an UCC code and starts with '(' it will
		/// be split by the AI. This code in UCC mode is valid:
		/// <p/>
		/// <code>(01)00000090311314(10)ABC123(15)060916</code>
		/// </remarks>
		/// <param name="code">the code to generate</param>
		public override void SetCode(String code)
		{
			if (GetCodeType() == iTextSharp.Barcodes.Barcode128.CODE128_UCC && code.StartsWith
				("("))
			{
				int idx = 0;
				StringBuilder ret = new StringBuilder("");
				while (idx >= 0)
				{
					int end = code.IndexOf(')', idx);
					if (end < 0)
					{
						throw new ArgumentException("Badly formed ucc string");
					}
					String sai = code.JSubstring(idx + 1, end);
					if (sai.Length < 2)
					{
						throw new ArgumentException("AI is too short");
					}
					int ai = System.Convert.ToInt32(sai);
					int len = (int)ais[ai];
					if (len == 0)
					{
						throw new ArgumentException("AI not found");
					}
					sai = System.Convert.ToInt32(ai).ToString();
					if (sai.Length == 1)
					{
						sai = "0" + sai;
					}
					idx = code.IndexOf('(', end);
					int next = (idx < 0 ? code.Length : idx);
					ret.Append(sai).Append(code.JSubstring(end + 1, next));
					if (len < 0)
					{
						if (idx >= 0)
						{
							ret.Append(FNC1);
						}
					}
					else
					{
						if (next - end - 1 + sai.Length != len)
						{
							throw new ArgumentException("Invalid AI length");
						}
					}
				}
				base.SetCode(ret.ToString());
			}
			else
			{
				base.SetCode(code);
			}
		}

		private static char GetStartSymbol(Barcode128.Barcode128CodeSet codeSet)
		{
			switch (codeSet)
			{
				case Barcode128.Barcode128CodeSet.A:
				{
					return START_A;
				}

				case Barcode128.Barcode128CodeSet.B:
				{
					return START_B;
				}

				case Barcode128.Barcode128CodeSet.C:
				{
					return START_C;
				}

				default:
				{
					return START_B;
				}
			}
		}

		static Barcode128()
		{
			ais[0] = 20;
			ais[1] = 16;
			ais[2] = 16;
			ais[10] = -1;
			ais[11] = 9;
			ais[12] = 8;
			ais[13] = 8;
			ais[15] = 8;
			ais[17] = 8;
			ais[20] = 4;
			ais[21] = -1;
			ais[22] = -1;
			ais[23] = -1;
			ais[240] = -1;
			ais[241] = -1;
			ais[250] = -1;
			ais[251] = -1;
			ais[252] = -1;
			ais[30] = -1;
			for (int k = 3100; k < 3700; ++k)
			{
				ais[k] = 10;
			}
			ais[37] = -1;
			for (int k_1 = 3900; k_1 < 3940; ++k_1)
			{
				ais[k_1] = -1;
			}
			ais[400] = -1;
			ais[401] = -1;
			ais[402] = 20;
			ais[403] = -1;
			for (int k_2 = 410; k_2 < 416; ++k_2)
			{
				ais[k_2] = 16;
			}
			ais[420] = -1;
			ais[421] = -1;
			ais[422] = 6;
			ais[423] = -1;
			ais[424] = 6;
			ais[425] = 6;
			ais[426] = 6;
			ais[7001] = 17;
			ais[7002] = -1;
			for (int k_3 = 7030; k_3 < 7040; ++k_3)
			{
				ais[k_3] = -1;
			}
			ais[8001] = 18;
			ais[8002] = -1;
			ais[8003] = -1;
			ais[8004] = -1;
			ais[8005] = 10;
			ais[8006] = 22;
			ais[8007] = -1;
			ais[8008] = -1;
			ais[8018] = 22;
			ais[8020] = -1;
			ais[8100] = 10;
			ais[8101] = 14;
			ais[8102] = 6;
			for (int k_4 = 90; k_4 < 100; ++k_4)
			{
				ais[k_4] = -1;
			}
		}

		/// <summary>
		/// Returns <CODE>true</CODE> if the next <CODE>numDigits</CODE>
		/// starting from index <CODE>textIndex</CODE> are numeric skipping any FNC1.
		/// </summary>
		/// <param name="text">the text to check</param>
		/// <param name="textIndex">where to check from</param>
		/// <param name="numDigits">the number of digits to check</param>
		/// <returns>the check result</returns>
		internal static bool IsNextDigits(String text, int textIndex, int numDigits)
		{
			int len = text.Length;
			while (textIndex < len && numDigits > 0)
			{
				if (text[textIndex] == FNC1)
				{
					++textIndex;
					continue;
				}
				int n = Math.Min(2, numDigits);
				if (textIndex + n > len)
				{
					return false;
				}
				while (n-- > 0)
				{
					char c = text[textIndex++];
					if (c < '0' || c > '9')
					{
						return false;
					}
					--numDigits;
				}
			}
			return numDigits == 0;
		}

		/// <summary>Packs the digits for charset C also considering FNC1.</summary>
		/// <remarks>
		/// Packs the digits for charset C also considering FNC1. It assumes that all the parameters
		/// are valid.
		/// </remarks>
		/// <param name="text">the text to pack</param>
		/// <param name="textIndex">where to pack from</param>
		/// <param name="numDigits">the number of digits to pack. It is always an even number
		/// 	</param>
		/// <returns>the packed digits, two digits per character</returns>
		internal static String GetPackedRawDigits(String text, int textIndex, int numDigits
			)
		{
			StringBuilder @out = new StringBuilder("");
			int start = textIndex;
			while (numDigits > 0)
			{
				if (text[textIndex] == FNC1)
				{
					@out.Append(FNC1_INDEX);
					++textIndex;
					continue;
				}
				numDigits -= 2;
				int c1 = text[textIndex++] - '0';
				int c2 = text[textIndex++] - '0';
				@out.Append((char)(c1 * 10 + c2));
			}
			return (char)(textIndex - start) + @out.ToString();
		}
	}
}
