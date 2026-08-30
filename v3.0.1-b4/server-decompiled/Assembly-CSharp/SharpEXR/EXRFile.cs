using System;
using System.Collections.Generic;
using System.IO;
using SharpEXR.AttributeTypes;

namespace SharpEXR
{
	// Token: 0x0200169F RID: 5791
	public class EXRFile
	{
		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x0600B49F RID: 46239 RVA: 0x00435BCC File Offset: 0x00433DCC
		// (set) Token: 0x0600B4A0 RID: 46240 RVA: 0x00435BD4 File Offset: 0x00433DD4
		public EXRVersion Version
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x0600B4A1 RID: 46241 RVA: 0x00435BDD File Offset: 0x00433DDD
		// (set) Token: 0x0600B4A2 RID: 46242 RVA: 0x00435BE5 File Offset: 0x00433DE5
		public List<EXRHeader> Headers
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x0600B4A3 RID: 46243 RVA: 0x00435BEE File Offset: 0x00433DEE
		// (set) Token: 0x0600B4A4 RID: 46244 RVA: 0x00435BF6 File Offset: 0x00433DF6
		public List<OffsetTable> OffsetTables
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x0600B4A5 RID: 46245 RVA: 0x00435BFF File Offset: 0x00433DFF
		// (set) Token: 0x0600B4A6 RID: 46246 RVA: 0x00435C07 File Offset: 0x00433E07
		public List<EXRPart> Parts
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600B4A8 RID: 46248 RVA: 0x00435C10 File Offset: 0x00433E10
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

		// Token: 0x0600B4A9 RID: 46249 RVA: 0x00435D98 File Offset: 0x00433F98
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

		// Token: 0x0600B4AA RID: 46250 RVA: 0x00435DC0 File Offset: 0x00433FC0
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

		// Token: 0x0600B4AB RID: 46251 RVA: 0x00435E3C File Offset: 0x0043403C
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

		// Token: 0x0600B4AC RID: 46252 RVA: 0x00435EB0 File Offset: 0x004340B0
		public static EXRFile FromFile(string file)
		{
			EXRReader exrreader = new EXRReader(new FileStream(file, FileMode.Open, FileAccess.Read), false);
			EXRFile exrfile = EXRFile.FromReader(exrreader);
			exrreader.Dispose();
			return exrfile;
		}

		// Token: 0x0600B4AD RID: 46253 RVA: 0x00435ED8 File Offset: 0x004340D8
		public static EXRFile FromStream(Stream stream)
		{
			EXRReader exrreader = new EXRReader(new BinaryReader(stream));
			EXRFile exrfile = EXRFile.FromReader(exrreader);
			exrreader.Dispose();
			return exrfile;
		}

		// Token: 0x0600B4AE RID: 46254 RVA: 0x00435F00 File Offset: 0x00434100
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
