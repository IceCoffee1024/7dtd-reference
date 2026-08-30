using System;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001C42 RID: 7234
	public sealed class BlobRef : IEquatable<BlobRef>
	{
		// Token: 0x0600D5F0 RID: 54768 RVA: 0x004D24FE File Offset: 0x004D06FE
		public BlobRef(ulong id, uint length)
			: this(id, length, BlobRef.s_emptyHash.AsMemory<byte>())
		{
		}

		// Token: 0x0600D5F1 RID: 54769 RVA: 0x004D2518 File Offset: 0x004D0718
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

		// Token: 0x0600D5F2 RID: 54770 RVA: 0x004D2570 File Offset: 0x004D0770
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

		// Token: 0x0600D5F3 RID: 54771 RVA: 0x004D25C0 File Offset: 0x004D07C0
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

		// Token: 0x0600D5F4 RID: 54772 RVA: 0x004D2640 File Offset: 0x004D0840
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

		// Token: 0x0600D5F5 RID: 54773 RVA: 0x004D2700 File Offset: 0x004D0900
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

		// Token: 0x0600D5F6 RID: 54774 RVA: 0x004D2760 File Offset: 0x004D0960
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

		// Token: 0x0600D5F7 RID: 54775 RVA: 0x004D27C3 File Offset: 0x004D09C3
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600D5F8 RID: 54776 RVA: 0x004D27D0 File Offset: 0x004D09D0
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600D5F9 RID: 54777 RVA: 0x004D2838 File Offset: 0x004D0A38
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600D5FA RID: 54778 RVA: 0x004D285E File Offset: 0x004D0A5E
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600D5FB RID: 54779 RVA: 0x001D8EA0 File Offset: 0x001D70A0
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600D5FC RID: 54780 RVA: 0x001D8EA9 File Offset: 0x001D70A9
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A2C6 RID: 41670
		public const int HashSizeBytes = 16;

		// Token: 0x0400A2C7 RID: 41671
		public const int SizeBytes = 28;

		// Token: 0x0400A2C8 RID: 41672
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A2C9 RID: 41673
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A2CA RID: 41674
		public readonly ulong Id;

		// Token: 0x0400A2CB RID: 41675
		public readonly uint Length;

		// Token: 0x0400A2CC RID: 41676
		public readonly ReadOnlyMemory<byte> Hash;
	}
}
