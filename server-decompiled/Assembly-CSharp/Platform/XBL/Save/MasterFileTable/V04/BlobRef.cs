using System;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001C9B RID: 7323
	public sealed class BlobRef : IEquatable<BlobRef>
	{
		// Token: 0x0600D8F9 RID: 55545 RVA: 0x004DD45E File Offset: 0x004DB65E
		public BlobRef(ulong id, uint length)
			: this(id, length, BlobRef.s_emptyHash.AsMemory<byte>())
		{
		}

		// Token: 0x0600D8FA RID: 55546 RVA: 0x004DD478 File Offset: 0x004DB678
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

		// Token: 0x0600D8FB RID: 55547 RVA: 0x004DD4D0 File Offset: 0x004DB6D0
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

		// Token: 0x0600D8FC RID: 55548 RVA: 0x004DD520 File Offset: 0x004DB720
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

		// Token: 0x0600D8FD RID: 55549 RVA: 0x004DD5A0 File Offset: 0x004DB7A0
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

		// Token: 0x0600D8FE RID: 55550 RVA: 0x004DD660 File Offset: 0x004DB860
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

		// Token: 0x0600D8FF RID: 55551 RVA: 0x004DD6C0 File Offset: 0x004DB8C0
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

		// Token: 0x0600D900 RID: 55552 RVA: 0x004DD723 File Offset: 0x004DB923
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600D901 RID: 55553 RVA: 0x004DD730 File Offset: 0x004DB930
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600D902 RID: 55554 RVA: 0x004DD798 File Offset: 0x004DB998
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600D903 RID: 55555 RVA: 0x004DD7BE File Offset: 0x004DB9BE
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600D904 RID: 55556 RVA: 0x001DCBE8 File Offset: 0x001DADE8
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600D905 RID: 55557 RVA: 0x001DCBF1 File Offset: 0x001DADF1
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A4CC RID: 42188
		public const int HashSizeBytes = 16;

		// Token: 0x0400A4CD RID: 42189
		public const int SizeBytes = 28;

		// Token: 0x0400A4CE RID: 42190
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A4CF RID: 42191
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A4D0 RID: 42192
		public readonly ulong Id;

		// Token: 0x0400A4D1 RID: 42193
		public readonly uint Length;

		// Token: 0x0400A4D2 RID: 42194
		public readonly ReadOnlyMemory<byte> Hash;
	}
}
