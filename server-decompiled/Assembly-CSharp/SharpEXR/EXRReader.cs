using System;
using System.IO;
using System.Text;

namespace SharpEXR
{
	// Token: 0x020016A6 RID: 5798
	public class EXRReader : IDisposable, IEXRReader
	{
		// Token: 0x0600B4EE RID: 46318 RVA: 0x004377C5 File Offset: 0x004359C5
		public EXRReader(Stream stream, bool leaveOpen = false)
			: this(new BinaryReader(stream, Encoding.ASCII, leaveOpen))
		{
		}

		// Token: 0x0600B4EF RID: 46319 RVA: 0x004377D9 File Offset: 0x004359D9
		public EXRReader(BinaryReader reader)
		{
			this.reader = reader;
		}

		// Token: 0x0600B4F0 RID: 46320 RVA: 0x004377E8 File Offset: 0x004359E8
		public byte ReadByte()
		{
			return this.reader.ReadByte();
		}

		// Token: 0x0600B4F1 RID: 46321 RVA: 0x004377F5 File Offset: 0x004359F5
		public int ReadInt32()
		{
			return this.reader.ReadInt32();
		}

		// Token: 0x0600B4F2 RID: 46322 RVA: 0x00437802 File Offset: 0x00435A02
		public uint ReadUInt32()
		{
			return this.reader.ReadUInt32();
		}

		// Token: 0x0600B4F3 RID: 46323 RVA: 0x0043780F File Offset: 0x00435A0F
		public Half ReadHalf()
		{
			return Half.ToHalf(this.reader.ReadUInt16());
		}

		// Token: 0x0600B4F4 RID: 46324 RVA: 0x00437821 File Offset: 0x00435A21
		public float ReadSingle()
		{
			return this.reader.ReadSingle();
		}

		// Token: 0x0600B4F5 RID: 46325 RVA: 0x0043782E File Offset: 0x00435A2E
		public double ReadDouble()
		{
			return this.reader.ReadDouble();
		}

		// Token: 0x0600B4F6 RID: 46326 RVA: 0x0043783C File Offset: 0x00435A3C
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

		// Token: 0x0600B4F7 RID: 46327 RVA: 0x004378B4 File Offset: 0x00435AB4
		public string ReadString()
		{
			int num = this.ReadInt32();
			return this.ReadString(num);
		}

		// Token: 0x0600B4F8 RID: 46328 RVA: 0x004378D0 File Offset: 0x00435AD0
		public string ReadString(int length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				stringBuilder.Append((char)this.reader.ReadByte());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B4F9 RID: 46329 RVA: 0x00437907 File Offset: 0x00435B07
		public byte[] ReadBytes(int count)
		{
			return this.reader.ReadBytes(count);
		}

		// Token: 0x0600B4FA RID: 46330 RVA: 0x00437915 File Offset: 0x00435B15
		public void CopyBytes(byte[] dest, int offset, int count)
		{
			if (this.reader.BaseStream.Read(dest, offset, count) != count)
			{
				throw new Exception("Less bytes read than expected");
			}
		}

		// Token: 0x0600B4FB RID: 46331 RVA: 0x00437938 File Offset: 0x00435B38
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600B4FC RID: 46332 RVA: 0x00437948 File Offset: 0x00435B48
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

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x0600B4FD RID: 46333 RVA: 0x00437988 File Offset: 0x00435B88
		// (set) Token: 0x0600B4FE RID: 46334 RVA: 0x0043799B File Offset: 0x00435B9B
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

		// Token: 0x04008755 RID: 34645
		[PublicizedFrom(EAccessModifier.Private)]
		public BinaryReader reader;

		// Token: 0x04008756 RID: 34646
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;
	}
}
