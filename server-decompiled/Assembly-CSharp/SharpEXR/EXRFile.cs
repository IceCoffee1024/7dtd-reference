using System;
using System.Collections.Generic;
using System.IO;
using SharpEXR.AttributeTypes;

namespace SharpEXR
{
	// Token: 0x020016F1 RID: 5873
	public class EXRFile
	{
		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x0600B779 RID: 46969 RVA: 0x0043FE64 File Offset: 0x0043E064
		// (set) Token: 0x0600B77A RID: 46970 RVA: 0x0043FE6C File Offset: 0x0043E06C
		public EXRVersion Version
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x0600B77B RID: 46971 RVA: 0x0043FE75 File Offset: 0x0043E075
		// (set) Token: 0x0600B77C RID: 46972 RVA: 0x0043FE7D File Offset: 0x0043E07D
		public List<EXRHeader> Headers
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x0600B77D RID: 46973 RVA: 0x0043FE86 File Offset: 0x0043E086
		// (set) Token: 0x0600B77E RID: 46974 RVA: 0x0043FE8E File Offset: 0x0043E08E
		public List<OffsetTable> OffsetTables
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x0600B77F RID: 46975 RVA: 0x0043FE97 File Offset: 0x0043E097
		// (set) Token: 0x0600B780 RID: 46976 RVA: 0x0043FE9F File Offset: 0x0043E09F
		public List<EXRPart> Parts
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600B782 RID: 46978 RVA: 0x0043FEA8 File Offset: 0x0043E0A8
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

		// Token: 0x0600B783 RID: 46979 RVA: 0x00440030 File Offset: 0x0043E230
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

		// Token: 0x0600B784 RID: 46980 RVA: 0x00440058 File Offset: 0x0043E258
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

		// Token: 0x0600B785 RID: 46981 RVA: 0x004400D4 File Offset: 0x0043E2D4
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

		// Token: 0x0600B786 RID: 46982 RVA: 0x00440148 File Offset: 0x0043E348
		public static EXRFile FromFile(string file)
		{
			EXRReader exrreader = new EXRReader(new FileStream(file, FileMode.Open, FileAccess.Read), false);
			EXRFile exrfile = EXRFile.FromReader(exrreader);
			exrreader.Dispose();
			return exrfile;
		}

		// Token: 0x0600B787 RID: 46983 RVA: 0x00440170 File Offset: 0x0043E370
		public static EXRFile FromStream(Stream stream)
		{
			EXRReader exrreader = new EXRReader(new BinaryReader(stream));
			EXRFile exrfile = EXRFile.FromReader(exrreader);
			exrreader.Dispose();
			return exrfile;
		}

		// Token: 0x0600B788 RID: 46984 RVA: 0x00440198 File Offset: 0x0043E398
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
