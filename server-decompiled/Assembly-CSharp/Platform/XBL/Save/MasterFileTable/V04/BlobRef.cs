using System;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001C69 RID: 7273
	public sealed class BlobRef : IEquatable<BlobRef>
	{
		// Token: 0x0600D780 RID: 55168 RVA: 0x004D8922 File Offset: 0x004D6B22
		public BlobRef(ulong id, uint length)
			: this(id, length, BlobRef.s_emptyHash.AsMemory<byte>())
		{
		}

		// Token: 0x0600D781 RID: 55169 RVA: 0x004D893C File Offset: 0x004D6B3C
		public BlobRef(ulong id, uint length, ReadOnlySpan<byte> hash)
		{
			this.Id = id;
			this.Length = length;
			if (16 != hash.Length)
			{
				throw new InvalidOperationException("Expected hash to be of length " + 16.ToString());
			}
			this.Hash = hash.ToArray();
		}

		// Token: 0x0600D782 RID: 55170 RVA: 0x004D8994 File Offset: 0x004D6B94
		public BlobRef(ulong id, uint length, ReadOnlyMemory<byte> hash)
		{
			this.Id = id;
			this.Length = length;
			if (16 != hash.Length)
			{
				throw new InvalidOperationException("Expected hash to be of length " + 16.ToString());
			}
			this.Hash = hash;
		}

		// Token: 0x0600D783 RID: 55171 RVA: 0x004D89E4 File Offset: 0x004D6BE4
		public BlobRef(ulong id, RefCountedBuffer buffer)
		{
			this.Id = id;
			this.Length = (uint)buffer.Length;
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
			this.Hash = array;
		}

		// Token: 0x0600D784 RID: 55172 RVA: 0x004D8A64 File Offset: 0x004D6C64
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(this.Id);
			writer.Write(this.Length);
			if (16 == this.Hash.Length)
			{
				writer.Write(this.Hash.Span);
				return;
			}
			Log.Error("[BlobRef] Expected a hash size of exactly " + 16.ToString() + ", padding or truncating as needed.");
			if (this.Hash.Length < 16)
			{
				writer.Write(this.Hash.Span);
				for (int i = this.Hash.Length; i < 16; i++)
				{
					writer.Write(0);
				}
				return;
			}
			writer.Write(this.Hash.Span.Slice(0, 16));
		}

		// Token: 0x0600D785 RID: 55173 RVA: 0x004D8B24 File Offset: 0x004D6D24
		public static BlobRef Read(PooledBinaryReader reader)
		{
			ulong num = reader.ReadUInt64();
			uint num2 = reader.ReadUInt32();
			byte[] array = new byte[16];
			int num3;
			if (!reader.TryReadAllBytes(array, out num3))
			{
				Log.Error(string.Format("[BlobRef] Expected {0} for the hash bytes, but reached the end of stream after reading {1} bytes?", 16, num3));
			}
			return new BlobRef(num, num2, array.AsMemory<byte>());
		}

		// Token: 0x0600D786 RID: 55174 RVA: 0x004D8B84 File Offset: 0x004D6D84
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

		// Token: 0x0600D787 RID: 55175 RVA: 0x004D8BE7 File Offset: 0x004D6DE7
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600D788 RID: 55176 RVA: 0x004D8BF4 File Offset: 0x004D6DF4
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600D789 RID: 55177 RVA: 0x004D8C5C File Offset: 0x004D6E5C
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600D78A RID: 55178 RVA: 0x004D8C82 File Offset: 0x004D6E82
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600D78B RID: 55179 RVA: 0x001DB938 File Offset: 0x001D9B38
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600D78C RID: 55180 RVA: 0x001DB941 File Offset: 0x001D9B41
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A3F7 RID: 41975
		public const int HashSizeBytes = 16;

		// Token: 0x0400A3F8 RID: 41976
		public const int SizeBytes = 28;

		// Token: 0x0400A3F9 RID: 41977
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A3FA RID: 41978
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A3FB RID: 41979
		public readonly ulong Id;

		// Token: 0x0400A3FC RID: 41980
		public readonly uint Length;

		// Token: 0x0400A3FD RID: 41981
		public readonly ReadOnlyMemory<byte> Hash;
	}
}
