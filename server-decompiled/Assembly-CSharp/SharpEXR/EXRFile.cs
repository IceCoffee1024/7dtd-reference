using System;
using System.Collections.Generic;
using System.IO;
using SharpEXR.AttributeTypes;

namespace SharpEXR
{
	// Token: 0x020016C5 RID: 5829
	public class EXRFile
	{
		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x0600B622 RID: 46626 RVA: 0x0043B944 File Offset: 0x00439B44
		// (set) Token: 0x0600B623 RID: 46627 RVA: 0x0043B94C File Offset: 0x00439B4C
		public EXRVersion Version
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x0600B624 RID: 46628 RVA: 0x0043B955 File Offset: 0x00439B55
		// (set) Token: 0x0600B625 RID: 46629 RVA: 0x0043B95D File Offset: 0x00439B5D
		public List<EXRHeader> Headers
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x0600B626 RID: 46630 RVA: 0x0043B966 File Offset: 0x00439B66
		// (set) Token: 0x0600B627 RID: 46631 RVA: 0x0043B96E File Offset: 0x00439B6E
		public List<OffsetTable> OffsetTables
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x0600B628 RID: 46632 RVA: 0x0043B977 File Offset: 0x00439B77
		// (set) Token: 0x0600B629 RID: 46633 RVA: 0x0043B97F File Offset: 0x00439B7F
		public List<EXRPart> Parts
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600B62B RID: 46635 RVA: 0x0043B988 File Offset: 0x00439B88
		public void Read(IEXRReader reader)
		{
			if (reader.ReadInt32() != 20000630)
			{
				throw new EXRFormatException("Invalid or corrupt EXR layout: First four bytes were not 20000630.");
			}
			int num = reader.ReadInt32();
			this.Version = new EXRVersion(num);
			this.Headers = new List<EXRHeader>();
			if (this.Version.IsMultiPart)
			{
				for (;;)
				{
					EXRHeader exrheader = new EXRHeader();
					exrheader.Read(this, reader);
					if (exrheader.IsEmpty)
					{
						break;
					}
					this.Headers.Add(exrheader);
				}
				throw new NotImplementedException("Multi part EXR files are not currently supported");
			}
			if (this.Version.IsSinglePartTiled)
			{
				throw new NotImplementedException("Tiled EXR files are not currently supported");
			}
			EXRHeader exrheader2 = new EXRHeader();
			exrheader2.Read(this, reader);
			this.Headers.Add(exrheader2);
			this.OffsetTables = new List<OffsetTable>();
			foreach (EXRHeader exrheader3 in this.Headers)
			{
				int num2;
				if (this.Version.IsMultiPart)
				{
					num2 = exrheader3.ChunkCount;
				}
				else if (this.Version.IsSinglePartTiled)
				{
					num2 = 0;
				}
				else
				{
					EXRCompression compression = exrheader3.Compression;
					Box2I dataWindow = exrheader3.DataWindow;
					int scanLinesPerBlock = EXRFile.GetScanLinesPerBlock(compression);
					num2 = (int)Math.Ceiling((double)dataWindow.Height / (double)scanLinesPerBlock);
				}
				OffsetTable offsetTable = new OffsetTable(num2);
				offsetTable.Read(reader, num2);
				this.OffsetTables.Add(offsetTable);
			}
		}

		// Token: 0x0600B62C RID: 46636 RVA: 0x0043BB10 File Offset: 0x00439D10
		public static int GetScanLinesPerBlock(EXRCompression compression)
		{
			switch (compression)
			{
			case EXRCompression.ZIP:
			case EXRCompression.PXR24:
				return 16;
			case EXRCompression.PIZ:
			case EXRCompression.B44:
			case EXRCompression.B44A:
				return 32;
			default:
				return 1;
			}
		}

		// Token: 0x0600B62D RID: 46637 RVA: 0x0043BB38 File Offset: 0x00439D38
		public static int GetBytesPerPixel(ImageDestFormat format)
		{
			switch (format)
			{
			case ImageDestFormat.RGB8:
			case ImageDestFormat.BGR8:
				return 3;
			case ImageDestFormat.RGBA8:
			case ImageDestFormat.PremultipliedRGBA8:
			case ImageDestFormat.BGRA8:
			case ImageDestFormat.PremultipliedBGRA8:
				return 4;
			case ImageDestFormat.RGB16:
			case ImageDestFormat.BGR16:
				return 6;
			case ImageDestFormat.RGBA16:
			case ImageDestFormat.PremultipliedRGBA16:
			case ImageDestFormat.BGRA16:
			case ImageDestFormat.PremultipliedBGRA16:
				return 8;
			case ImageDestFormat.RGB32:
			case ImageDestFormat.BGR32:
				return 12;
			case ImageDestFormat.RGBA32:
			case ImageDestFormat.PremultipliedRGBA32:
			case ImageDestFormat.BGRA32:
			case ImageDestFormat.PremultipliedBGRA32:
				return 16;
			default:
				throw new ArgumentException("Unrecognized destination format", "format");
			}
		}

		// Token: 0x0600B62E RID: 46638 RVA: 0x0043BBB4 File Offset: 0x00439DB4
		public static int GetBitsPerPixel(ImageDestFormat format)
		{
			switch (format)
			{
			case ImageDestFormat.RGB8:
			case ImageDestFormat.RGBA8:
			case ImageDestFormat.PremultipliedRGBA8:
			case ImageDestFormat.BGR8:
			case ImageDestFormat.BGRA8:
			case ImageDestFormat.PremultipliedBGRA8:
				return 8;
			case ImageDestFormat.RGB16:
			case ImageDestFormat.RGBA16:
			case ImageDestFormat.PremultipliedRGBA16:
			case ImageDestFormat.BGR16:
			case ImageDestFormat.BGRA16:
			case ImageDestFormat.PremultipliedBGRA16:
				return 16;
			case ImageDestFormat.RGB32:
			case ImageDestFormat.RGBA32:
			case ImageDestFormat.PremultipliedRGBA32:
			case ImageDestFormat.BGR32:
			case ImageDestFormat.BGRA32:
			case ImageDestFormat.PremultipliedBGRA32:
				return 32;
			default:
				throw new ArgumentException("Unrecognized destination format", "format");
			}
		}

		// Token: 0x0600B62F RID: 46639 RVA: 0x0043BC28 File Offset: 0x00439E28
		public static EXRFile FromFile(string file)
		{
			EXRReader exrreader = new EXRReader(new FileStream(file, FileMode.Open, FileAccess.Read), false);
			EXRFile exrfile = EXRFile.FromReader(exrreader);
			exrreader.Dispose();
			return exrfile;
		}

		// Token: 0x0600B630 RID: 46640 RVA: 0x0043BC50 File Offset: 0x00439E50
		public static EXRFile FromStream(Stream stream)
		{
			EXRReader exrreader = new EXRReader(new BinaryReader(stream));
			EXRFile exrfile = EXRFile.FromReader(exrreader);
			exrreader.Dispose();
			return exrfile;
		}

		// Token: 0x0600B631 RID: 46641 RVA: 0x0043BC78 File Offset: 0x00439E78
		public static EXRFile FromReader(IEXRReader reader)
		{
			EXRFile exrfile = new EXRFile();
			exrfile.Read(reader);
			exrfile.Parts = new List<EXRPart>();
			for (int i = 0; i < exrfile.Headers.Count; i++)
			{
				EXRPart exrpart = new EXRPart(exrfile.Version, exrfile.Headers[i], exrfile.OffsetTables[i]);
				exrfile.Parts.Add(exrpart);
			}
			return exrfile;
		}
	}
}
