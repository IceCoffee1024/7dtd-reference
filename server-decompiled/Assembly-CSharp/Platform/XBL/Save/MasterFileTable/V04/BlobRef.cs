using System;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001CCC RID: 7372
	public sealed class BlobRef : IEquatable<BlobRef>
	{
		// Token: 0x0600DA0D RID: 55821 RVA: 0x004E0FAE File Offset: 0x004DF1AE
		public BlobRef(ulong id, uint length)
			: this(id, length, BlobRef.s_emptyHash.AsMemory<byte>())
		{
		}

		// Token: 0x0600DA0E RID: 55822 RVA: 0x004E0FC8 File Offset: 0x004DF1C8
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

		// Token: 0x0600DA0F RID: 55823 RVA: 0x004E1020 File Offset: 0x004DF220
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

		// Token: 0x0600DA10 RID: 55824 RVA: 0x004E1070 File Offset: 0x004DF270
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

		// Token: 0x0600DA11 RID: 55825 RVA: 0x004E10F0 File Offset: 0x004DF2F0
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

		// Token: 0x0600DA12 RID: 55826 RVA: 0x004E11B0 File Offset: 0x004DF3B0
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

		// Token: 0x0600DA13 RID: 55827 RVA: 0x004E1210 File Offset: 0x004DF410
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

		// Token: 0x0600DA14 RID: 55828 RVA: 0x004E1273 File Offset: 0x004DF473
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600DA15 RID: 55829 RVA: 0x004E1280 File Offset: 0x004DF480
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600DA16 RID: 55830 RVA: 0x004E12E8 File Offset: 0x004DF4E8
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600DA17 RID: 55831 RVA: 0x004E130E File Offset: 0x004DF50E
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600DA18 RID: 55832 RVA: 0x001DDCD0 File Offset: 0x001DBED0
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600DA19 RID: 55833 RVA: 0x001DDCD9 File Offset: 0x001DBED9
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A5B5 RID: 42421
		public const int HashSizeBytes = 16;

		// Token: 0x0400A5B6 RID: 42422
		public const int SizeBytes = 28;

		// Token: 0x0400A5B7 RID: 42423
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A5B8 RID: 42424
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A5B9 RID: 42425
		public readonly ulong Id;

		// Token: 0x0400A5BA RID: 42426
		public readonly uint Length;

		// Token: 0x0400A5BB RID: 42427
		public readonly ReadOnlyMemory<byte> Hash;
	}
}
