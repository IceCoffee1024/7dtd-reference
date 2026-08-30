using System;
using System.IO;
using System.Text;

namespace SharpEXR
{
	// Token: 0x020016CC RID: 5836
	public class EXRReader : IDisposable, IEXRReader
	{
		// Token: 0x0600B671 RID: 46705 RVA: 0x0043D53D File Offset: 0x0043B73D
		public EXRReader(Stream stream, bool leaveOpen = false)
			: this(new BinaryReader(stream, Encoding.ASCII, leaveOpen))
		{
		}

		// Token: 0x0600B672 RID: 46706 RVA: 0x0043D551 File Offset: 0x0043B751
		public EXRReader(BinaryReader reader)
		{
			this.reader = reader;
		}

		// Token: 0x0600B673 RID: 46707 RVA: 0x0043D560 File Offset: 0x0043B760
		public byte ReadByte()
		{
			return this.reader.ReadByte();
		}

		// Token: 0x0600B674 RID: 46708 RVA: 0x0043D56D File Offset: 0x0043B76D
		public int ReadInt32()
		{
			return this.reader.ReadInt32();
		}

		// Token: 0x0600B675 RID: 46709 RVA: 0x0043D57A File Offset: 0x0043B77A
		public uint ReadUInt32()
		{
			return this.reader.ReadUInt32();
		}

		// Token: 0x0600B676 RID: 46710 RVA: 0x0043D587 File Offset: 0x0043B787
		public Half ReadHalf()
		{
			return Half.ToHalf(this.reader.ReadUInt16());
		}

		// Token: 0x0600B677 RID: 46711 RVA: 0x0043D599 File Offset: 0x0043B799
		public float ReadSingle()
		{
			return this.reader.ReadSingle();
		}

		// Token: 0x0600B678 RID: 46712 RVA: 0x0043D5A6 File Offset: 0x0043B7A6
		public double ReadDouble()
		{
			return this.reader.ReadDouble();
		}

		// Token: 0x0600B679 RID: 46713 RVA: 0x0043D5B4 File Offset: 0x0043B7B4
		public string ReadNullTerminatedString(int maxLength)
		{
			long position = this.reader.BaseStream.Position;
			StringBuilder stringBuilder = new StringBuilder();
			byte b;
			while ((b = this.reader.ReadByte()) != 0)
			{
				if (this.reader.BaseStream.Position - position > (long)maxLength)
				{
					throw new EXRFormatException("Null terminated string exceeded maximum length of " + maxLength.ToString() + " bytes.");
				}
				stringBuilder.Append((char)b);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B67A RID: 46714 RVA: 0x0043D62C File Offset: 0x0043B82C
		public string ReadString()
		{
			int num = this.ReadInt32();
			return this.ReadString(num);
		}

		// Token: 0x0600B67B RID: 46715 RVA: 0x0043D648 File Offset: 0x0043B848
		public string ReadString(int length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				stringBuilder.Append((char)this.reader.ReadByte());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B67C RID: 46716 RVA: 0x0043D67F File Offset: 0x0043B87F
		public byte[] ReadBytes(int count)
		{
			return this.reader.ReadBytes(count);
		}

		// Token: 0x0600B67D RID: 46717 RVA: 0x0043D68D File Offset: 0x0043B88D
		public void CopyBytes(byte[] dest, int offset, int count)
		{
			if (this.reader.BaseStream.Read(dest, offset, count) != count)
			{
				throw new Exception("Less bytes read than expected");
			}
		}

		// Token: 0x0600B67E RID: 46718 RVA: 0x0043D6B0 File Offset: 0x0043B8B0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600B67F RID: 46719 RVA: 0x0043D6C0 File Offset: 0x0043B8C0
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing)
			{
				try
				{
					this.reader.Dispose();
				}
				catch
				{
				}
			}
			this.disposed = true;
		}

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x0600B680 RID: 46720 RVA: 0x0043D700 File Offset: 0x0043B900
		// (set) Token: 0x0600B681 RID: 46721 RVA: 0x0043D713 File Offset: 0x0043B913
		public int Position
		{
			get
			{
				return (int)this.reader.BaseStream.Position;
			}
			set
			{
				this.reader.BaseStream.Seek((long)value, SeekOrigin.Begin);
			}
		}

		// Token: 0x04008871 RID: 34929
		[PublicizedFrom(EAccessModifier.Private)]
		public BinaryReader reader;

		// Token: 0x04008872 RID: 34930
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;
	}
}
