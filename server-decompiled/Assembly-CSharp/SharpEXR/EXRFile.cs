using System;
using System.Collections.Generic;
using System.IO;
using SharpEXR.AttributeTypes;

namespace SharpEXR
{
	// Token: 0x02001710 RID: 5904
	public class EXRFile
	{
		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x0600B84D RID: 47181 RVA: 0x00442908 File Offset: 0x00440B08
		// (set) Token: 0x0600B84E RID: 47182 RVA: 0x00442910 File Offset: 0x00440B10
		public EXRVersion Version
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x0600B84F RID: 47183 RVA: 0x00442919 File Offset: 0x00440B19
		// (set) Token: 0x0600B850 RID: 47184 RVA: 0x00442921 File Offset: 0x00440B21
		public List<EXRHeader> Headers
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x0600B851 RID: 47185 RVA: 0x0044292A File Offset: 0x00440B2A
		// (set) Token: 0x0600B852 RID: 47186 RVA: 0x00442932 File Offset: 0x00440B32
		public List<OffsetTable> OffsetTables
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x0600B853 RID: 47187 RVA: 0x0044293B File Offset: 0x00440B3B
		// (set) Token: 0x0600B854 RID: 47188 RVA: 0x00442943 File Offset: 0x00440B43
		public List<EXRPart> Parts
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600B856 RID: 47190 RVA: 0x0044294C File Offset: 0x00440B4C
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

		// Token: 0x0600B857 RID: 47191 RVA: 0x00442AD4 File Offset: 0x00440CD4
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

		// Token: 0x0600B858 RID: 47192 RVA: 0x00442AFC File Offset: 0x00440CFC
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

		// Token: 0x0600B859 RID: 47193 RVA: 0x00442B78 File Offset: 0x00440D78
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

		// Token: 0x0600B85A RID: 47194 RVA: 0x00442BEC File Offset: 0x00440DEC
		public static EXRFile FromFile(string file)
		{
			EXRReader exrreader = new EXRReader(new FileStream(file, FileMode.Open, FileAccess.Read), false);
			EXRFile exrfile = EXRFile.FromReader(exrreader);
			exrreader.Dispose();
			return exrfile;
		}

		// Token: 0x0600B85B RID: 47195 RVA: 0x00442C14 File Offset: 0x00440E14
		public static EXRFile FromStream(Stream stream)
		{
			EXRReader exrreader = new EXRReader(new BinaryReader(stream));
			EXRFile exrfile = EXRFile.FromReader(exrreader);
			exrreader.Dispose();
			return exrfile;
		}

		// Token: 0x0600B85C RID: 47196 RVA: 0x00442C3C File Offset: 0x00440E3C
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
