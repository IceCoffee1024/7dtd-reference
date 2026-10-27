using System;
using System.IO;
using System.Text;

namespace SharpEXR
{
	// Token: 0x02001717 RID: 5911
	public class EXRReader : IDisposable, IEXRReader
	{
		// Token: 0x0600B89C RID: 47260 RVA: 0x00444501 File Offset: 0x00442701
		public EXRReader(Stream stream, bool leaveOpen = false)
			: this(new BinaryReader(stream, Encoding.ASCII, leaveOpen))
		{
		}

		// Token: 0x0600B89D RID: 47261 RVA: 0x00444515 File Offset: 0x00442715
		public EXRReader(BinaryReader reader)
		{
			this.reader = reader;
		}

		// Token: 0x0600B89E RID: 47262 RVA: 0x00444524 File Offset: 0x00442724
		public byte ReadByte()
		{
			return this.reader.ReadByte();
		}

		// Token: 0x0600B89F RID: 47263 RVA: 0x00444531 File Offset: 0x00442731
		public int ReadInt32()
		{
			return this.reader.ReadInt32();
		}

		// Token: 0x0600B8A0 RID: 47264 RVA: 0x0044453E File Offset: 0x0044273E
		public uint ReadUInt32()
		{
			return this.reader.ReadUInt32();
		}

		// Token: 0x0600B8A1 RID: 47265 RVA: 0x0044454B File Offset: 0x0044274B
		public Half ReadHalf()
		{
			return Half.ToHalf(this.reader.ReadUInt16());
		}

		// Token: 0x0600B8A2 RID: 47266 RVA: 0x0044455D File Offset: 0x0044275D
		public float ReadSingle()
		{
			return this.reader.ReadSingle();
		}

		// Token: 0x0600B8A3 RID: 47267 RVA: 0x0044456A File Offset: 0x0044276A
		public double ReadDouble()
		{
			return this.reader.ReadDouble();
		}

		// Token: 0x0600B8A4 RID: 47268 RVA: 0x00444578 File Offset: 0x00442778
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

		// Token: 0x0600B8A5 RID: 47269 RVA: 0x004445F0 File Offset: 0x004427F0
		public string ReadString()
		{
			int num = this.ReadInt32();
			return this.ReadString(num);
		}

		// Token: 0x0600B8A6 RID: 47270 RVA: 0x0044460C File Offset: 0x0044280C
		public string ReadString(int length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				stringBuilder.Append((char)this.reader.ReadByte());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B8A7 RID: 47271 RVA: 0x00444643 File Offset: 0x00442843
		public byte[] ReadBytes(int count)
		{
			return this.reader.ReadBytes(count);
		}

		// Token: 0x0600B8A8 RID: 47272 RVA: 0x00444651 File Offset: 0x00442851
		public void CopyBytes(byte[] dest, int offset, int count)
		{
			if (this.reader.BaseStream.Read(dest, offset, count) != count)
			{
				throw new Exception("Less bytes read than expected");
			}
		}

		// Token: 0x0600B8A9 RID: 47273 RVA: 0x00444674 File Offset: 0x00442874
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600B8AA RID: 47274 RVA: 0x00444684 File Offset: 0x00442884
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

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x0600B8AB RID: 47275 RVA: 0x004446C4 File Offset: 0x004428C4
		// (set) Token: 0x0600B8AC RID: 47276 RVA: 0x004446D7 File Offset: 0x004428D7
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

		// Token: 0x040089E9 RID: 35305
		[PublicizedFrom(EAccessModifier.Private)]
		public BinaryReader reader;

		// Token: 0x040089EA RID: 35306
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;
	}
}
