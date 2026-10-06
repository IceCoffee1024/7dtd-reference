using System;
using System.IO;
using System.Text;

namespace SharpEXR
{
	// Token: 0x020016F8 RID: 5880
	public class EXRReader : IDisposable, IEXRReader
	{
		// Token: 0x0600B7C8 RID: 47048 RVA: 0x00441A5D File Offset: 0x0043FC5D
		public EXRReader(Stream stream, bool leaveOpen = false)
			: this(new BinaryReader(stream, Encoding.ASCII, leaveOpen))
		{
		}

		// Token: 0x0600B7C9 RID: 47049 RVA: 0x00441A71 File Offset: 0x0043FC71
		public EXRReader(BinaryReader reader)
		{
			this.reader = reader;
		}

		// Token: 0x0600B7CA RID: 47050 RVA: 0x00441A80 File Offset: 0x0043FC80
		public byte ReadByte()
		{
			return this.reader.ReadByte();
		}

		// Token: 0x0600B7CB RID: 47051 RVA: 0x00441A8D File Offset: 0x0043FC8D
		public int ReadInt32()
		{
			return this.reader.ReadInt32();
		}

		// Token: 0x0600B7CC RID: 47052 RVA: 0x00441A9A File Offset: 0x0043FC9A
		public uint ReadUInt32()
		{
			return this.reader.ReadUInt32();
		}

		// Token: 0x0600B7CD RID: 47053 RVA: 0x00441AA7 File Offset: 0x0043FCA7
		public Half ReadHalf()
		{
			return Half.ToHalf(this.reader.ReadUInt16());
		}

		// Token: 0x0600B7CE RID: 47054 RVA: 0x00441AB9 File Offset: 0x0043FCB9
		public float ReadSingle()
		{
			return this.reader.ReadSingle();
		}

		// Token: 0x0600B7CF RID: 47055 RVA: 0x00441AC6 File Offset: 0x0043FCC6
		public double ReadDouble()
		{
			return this.reader.ReadDouble();
		}

		// Token: 0x0600B7D0 RID: 47056 RVA: 0x00441AD4 File Offset: 0x0043FCD4
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

		// Token: 0x0600B7D1 RID: 47057 RVA: 0x00441B4C File Offset: 0x0043FD4C
		public string ReadString()
		{
			int num = this.ReadInt32();
			return this.ReadString(num);
		}

		// Token: 0x0600B7D2 RID: 47058 RVA: 0x00441B68 File Offset: 0x0043FD68
		public string ReadString(int length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				stringBuilder.Append((char)this.reader.ReadByte());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B7D3 RID: 47059 RVA: 0x00441B9F File Offset: 0x0043FD9F
		public byte[] ReadBytes(int count)
		{
			return this.reader.ReadBytes(count);
		}

		// Token: 0x0600B7D4 RID: 47060 RVA: 0x00441BAD File Offset: 0x0043FDAD
		public void CopyBytes(byte[] dest, int offset, int count)
		{
			if (this.reader.BaseStream.Read(dest, offset, count) != count)
			{
				throw new Exception("Less bytes read than expected");
			}
		}

		// Token: 0x0600B7D5 RID: 47061 RVA: 0x00441BD0 File Offset: 0x0043FDD0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600B7D6 RID: 47062 RVA: 0x00441BE0 File Offset: 0x0043FDE0
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

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x0600B7D7 RID: 47063 RVA: 0x00441C20 File Offset: 0x0043FE20
		// (set) Token: 0x0600B7D8 RID: 47064 RVA: 0x00441C33 File Offset: 0x0043FE33
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

		// Token: 0x04008936 RID: 35126
		[PublicizedFrom(EAccessModifier.Private)]
		public BinaryReader reader;

		// Token: 0x04008937 RID: 35127
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;
	}
}
