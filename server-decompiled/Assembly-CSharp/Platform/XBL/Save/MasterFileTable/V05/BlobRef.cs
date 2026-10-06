using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.V05
{
	// Token: 0x02001C97 RID: 7319
	public sealed class BlobRef : IEquatable<BlobRef>, IMigratable
	{
		// Token: 0x17001B50 RID: 6992
		// (get) Token: 0x0600D8C3 RID: 55491 RVA: 0x004DC418 File Offset: 0x004DA618
		// (set) Token: 0x0600D8C4 RID: 55492 RVA: 0x004DC420 File Offset: 0x004DA620
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 5;

		// Token: 0x17001B51 RID: 6993
		// (get) Token: 0x0600D8C5 RID: 55493 RVA: 0x004DC429 File Offset: 0x004DA629
		// (set) Token: 0x0600D8C6 RID: 55494 RVA: 0x004DC431 File Offset: 0x004DA631
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

		// Token: 0x17001B52 RID: 6994
		// (get) Token: 0x0600D8C7 RID: 55495 RVA: 0x004DC45C File Offset: 0x004DA65C
		// (set) Token: 0x0600D8C8 RID: 55496 RVA: 0x004DC464 File Offset: 0x004DA664
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x0600D8C9 RID: 55497 RVA: 0x004DC470 File Offset: 0x004DA670
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

		// Token: 0x0600D8CA RID: 55498 RVA: 0x004DC5B8 File Offset: 0x004DA7B8
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

		// Token: 0x0600D8CB RID: 55499 RVA: 0x004DC6E4 File Offset: 0x004DA8E4
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

		// Token: 0x0600D8CC RID: 55500 RVA: 0x004DC747 File Offset: 0x004DA947
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600D8CD RID: 55501 RVA: 0x004DC754 File Offset: 0x004DA954
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

		// Token: 0x0600D8CE RID: 55502 RVA: 0x004DC7B4 File Offset: 0x004DA9B4
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600D8CF RID: 55503 RVA: 0x004DC824 File Offset: 0x004DAA24
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600D8D0 RID: 55504 RVA: 0x004DC84C File Offset: 0x004DAA4C
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600D8D1 RID: 55505 RVA: 0x001DCBE8 File Offset: 0x001DADE8
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600D8D2 RID: 55506 RVA: 0x001DCBF1 File Offset: 0x001DADF1
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A4AF RID: 42159
		public const ushort VERSION = 5;

		// Token: 0x0400A4B0 RID: 42160
		public const int HashSizeBytes = 16;

		// Token: 0x0400A4B1 RID: 42161
		public const int SizeBytes = 28;

		// Token: 0x0400A4B2 RID: 42162
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A4B3 RID: 42163
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A4B5 RID: 42165
		public ulong Id;

		// Token: 0x0400A4B6 RID: 42166
		public uint Length;

		// Token: 0x0400A4B7 RID: 42167
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlyMemory<byte> m_hash = BlobRef.s_emptyHash;
	}
}
