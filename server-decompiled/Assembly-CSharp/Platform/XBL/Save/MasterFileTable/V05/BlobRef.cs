using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.V05
{
	// Token: 0x02001C65 RID: 7269
	public sealed class BlobRef : IEquatable<BlobRef>, IMigratable
	{
		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x0600D74A RID: 55114 RVA: 0x004D78DC File Offset: 0x004D5ADC
		// (set) Token: 0x0600D74B RID: 55115 RVA: 0x004D78E4 File Offset: 0x004D5AE4
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 5;

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x0600D74C RID: 55116 RVA: 0x004D78ED File Offset: 0x004D5AED
		// (set) Token: 0x0600D74D RID: 55117 RVA: 0x004D78F5 File Offset: 0x004D5AF5
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

		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x0600D74E RID: 55118 RVA: 0x004D7920 File Offset: 0x004D5B20
		// (set) Token: 0x0600D74F RID: 55119 RVA: 0x004D7928 File Offset: 0x004D5B28
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x0600D750 RID: 55120 RVA: 0x004D7934 File Offset: 0x004D5B34
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

		// Token: 0x0600D751 RID: 55121 RVA: 0x004D7A7C File Offset: 0x004D5C7C
		public void Read(PooledBinaryReader reader)
		{
			if (this.Version != 5 || this.Id != 0UL || this.Length != 0U || !this.m_hash.Span.SequenceEqual<byte>(BlobRef.s_emptyHash) || this.FutureData.Length != 0)
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

		// Token: 0x0600D752 RID: 55122 RVA: 0x004D7BA8 File Offset: 0x004D5DA8
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

		// Token: 0x0600D753 RID: 55123 RVA: 0x004D7C0B File Offset: 0x004D5E0B
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600D754 RID: 55124 RVA: 0x004D7C18 File Offset: 0x004D5E18
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

		// Token: 0x0600D755 RID: 55125 RVA: 0x004D7C78 File Offset: 0x004D5E78
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600D756 RID: 55126 RVA: 0x004D7CE8 File Offset: 0x004D5EE8
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600D757 RID: 55127 RVA: 0x004D7D10 File Offset: 0x004D5F10
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600D758 RID: 55128 RVA: 0x001DB938 File Offset: 0x001D9B38
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600D759 RID: 55129 RVA: 0x001DB941 File Offset: 0x001D9B41
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A3DA RID: 41946
		public const ushort VERSION = 5;

		// Token: 0x0400A3DB RID: 41947
		public const int HashSizeBytes = 16;

		// Token: 0x0400A3DC RID: 41948
		public const int SizeBytes = 28;

		// Token: 0x0400A3DD RID: 41949
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A3DE RID: 41950
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A3E0 RID: 41952
		public ulong Id;

		// Token: 0x0400A3E1 RID: 41953
		public uint Length;

		// Token: 0x0400A3E2 RID: 41954
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlyMemory<byte> m_hash = BlobRef.s_emptyHash;
	}
}
