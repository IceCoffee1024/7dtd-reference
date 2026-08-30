using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001C74 RID: 7284
	public sealed class BlobRef : IEquatable<BlobRef>, IMigratable
	{
		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x0600D7EF RID: 55279 RVA: 0x004DA8BB File Offset: 0x004D8ABB
		// (set) Token: 0x0600D7F0 RID: 55280 RVA: 0x004DA8C3 File Offset: 0x004D8AC3
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x0600D7F1 RID: 55281 RVA: 0x004DA8CC File Offset: 0x004D8ACC
		// (set) Token: 0x0600D7F2 RID: 55282 RVA: 0x004DA8D4 File Offset: 0x004D8AD4
		public ReadOnlyMemory<byte> Hash
		{
			get
			{
				return this.m_hash;
			}
			set
			{
				if (16 != value.Length)
				{
					throw new InvalidOperationException(string.Format("Expected hash to be of length {0}", 16));
				}
				this.m_hash = value;
			}
		}

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x0600D7F3 RID: 55283 RVA: 0x004DA8FF File Offset: 0x004D8AFF
		// (set) Token: 0x0600D7F4 RID: 55284 RVA: 0x004DA907 File Offset: 0x004D8B07
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x0600D7F5 RID: 55285 RVA: 0x004DA910 File Offset: 0x004D8B10
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(this.Version);
			int num = 0;
			long position = writer.BaseStream.Position;
			writer.Write(num);
			long position2 = writer.BaseStream.Position;
			writer.Write(this.Id);
			writer.Write(this.Length);
			if (16 != this.Hash.Length)
			{
				Log.Error(string.Format("[{0}] Expected a hash size of exactly {1}, padding or truncating as needed.", "BlobRef", 16));
				if (this.Hash.Length < 16)
				{
					writer.Write(this.Hash.Span);
					for (int i = this.Hash.Length; i < 16; i++)
					{
						writer.Write(0);
					}
				}
				else
				{
					writer.Write(this.Hash.Span.Slice(0, 16));
				}
			}
			else
			{
				writer.Write(this.Hash.Span);
			}
			writer.Write(this.FutureData);
			long position3 = writer.BaseStream.Position;
			num = (int)(position3 - position2);
			writer.BaseStream.Position = position;
			writer.Write(num);
			writer.BaseStream.Position = position3;
		}

		// Token: 0x0600D7F6 RID: 55286 RVA: 0x004DAA58 File Offset: 0x004D8C58
		public void Read(PooledBinaryReader reader)
		{
			if (this.Version != 6 || this.Id != 0UL || this.Length != 0U || !this.m_hash.Span.SequenceEqual<byte>(BlobRef.s_emptyHash) || this.FutureData.Length != 0)
			{
				throw new InvalidOperationException("Read should only be called on a new BlobRef.");
			}
			this.Version = reader.ReadUInt16();
			int num = reader.ReadInt32();
			long position = reader.BaseStream.Position;
			this.Id = reader.ReadUInt64();
			this.Length = reader.ReadUInt32();
			byte[] array = new byte[16];
			int num2;
			if (!reader.TryReadAllBytes(array, out num2))
			{
				Log.Error(string.Format("[{0}] Expected {1} for the hash bytes, but reached the end of stream after reading {2} bytes?", "BlobRef", 16, num2));
			}
			this.Hash = array;
			long position2 = reader.BaseStream.Position;
			this.FutureData = new byte[(int)((long)num - (position2 - position))];
			int num3;
			if (!reader.TryReadAllBytes(this.FutureData, out num3))
			{
				throw new IOException(string.Format("Expected {0} bytes to be read for future data but only got {1} bytes.", this.FutureData.Length, num3));
			}
		}

		// Token: 0x0600D7F7 RID: 55287 RVA: 0x004DAB84 File Offset: 0x004D8D84
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"BlobRef[Id=",
				SaveContainer.IdToString(this.Id),
				", Length=",
				this.Length.FormatSize(false),
				", Hash=",
				this.Hash.ToHexString(),
				"]"
			});
		}

		// Token: 0x0600D7F8 RID: 55288 RVA: 0x004DABE7 File Offset: 0x004D8DE7
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600D7F9 RID: 55289 RVA: 0x004DABF4 File Offset: 0x004D8DF4
		public static ReadOnlyMemory<byte> CalculateHash(RefCountedBuffer buffer)
		{
			HashAlgorithm hashAlgorithm = BlobRef.GetHashAlgorithm();
			if (128 != hashAlgorithm.HashSize)
			{
				throw new InvalidOperationException("Unexpected hash algorithm hash size.");
			}
			byte[] array = new byte[16];
			int num;
			if (!hashAlgorithm.TryComputeHash(buffer.Span, array, out num))
			{
				throw new InvalidOperationException("Expected to be able to compute the hash.");
			}
			return array;
		}

		// Token: 0x0600D7FA RID: 55290 RVA: 0x004DAC54 File Offset: 0x004D8E54
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600D7FB RID: 55291 RVA: 0x004DACC4 File Offset: 0x004D8EC4
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600D7FC RID: 55292 RVA: 0x004DACEC File Offset: 0x004D8EEC
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600D7FD RID: 55293 RVA: 0x001DB938 File Offset: 0x001D9B38
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600D7FE RID: 55294 RVA: 0x001DB941 File Offset: 0x001D9B41
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A429 RID: 42025
		public const ushort VERSION = 6;

		// Token: 0x0400A42A RID: 42026
		public const int HashSizeBytes = 16;

		// Token: 0x0400A42B RID: 42027
		public const int SizeBytes = 28;

		// Token: 0x0400A42C RID: 42028
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A42D RID: 42029
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A42F RID: 42031
		public ulong Id;

		// Token: 0x0400A430 RID: 42032
		public uint Length;

		// Token: 0x0400A431 RID: 42033
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlyMemory<byte> m_hash = BlobRef.s_emptyHash;
	}
}
