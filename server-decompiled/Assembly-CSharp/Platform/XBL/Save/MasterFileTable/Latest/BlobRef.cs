using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001CA6 RID: 7334
	public sealed class BlobRef : IEquatable<BlobRef>, IMigratable
	{
		// Token: 0x17001B6B RID: 7019
		// (get) Token: 0x0600D968 RID: 55656 RVA: 0x004DF3F7 File Offset: 0x004DD5F7
		// (set) Token: 0x0600D969 RID: 55657 RVA: 0x004DF3FF File Offset: 0x004DD5FF
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x0600D96A RID: 55658 RVA: 0x004DF408 File Offset: 0x004DD608
		// (set) Token: 0x0600D96B RID: 55659 RVA: 0x004DF410 File Offset: 0x004DD610
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

		// Token: 0x17001B6D RID: 7021
		// (get) Token: 0x0600D96C RID: 55660 RVA: 0x004DF43B File Offset: 0x004DD63B
		// (set) Token: 0x0600D96D RID: 55661 RVA: 0x004DF443 File Offset: 0x004DD643
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x0600D96E RID: 55662 RVA: 0x004DF44C File Offset: 0x004DD64C
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

		// Token: 0x0600D96F RID: 55663 RVA: 0x004DF594 File Offset: 0x004DD794
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

		// Token: 0x0600D970 RID: 55664 RVA: 0x004DF6C0 File Offset: 0x004DD8C0
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

		// Token: 0x0600D971 RID: 55665 RVA: 0x004DF723 File Offset: 0x004DD923
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600D972 RID: 55666 RVA: 0x004DF730 File Offset: 0x004DD930
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

		// Token: 0x0600D973 RID: 55667 RVA: 0x004DF790 File Offset: 0x004DD990
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600D974 RID: 55668 RVA: 0x004DF800 File Offset: 0x004DDA00
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600D975 RID: 55669 RVA: 0x004DF828 File Offset: 0x004DDA28
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600D976 RID: 55670 RVA: 0x001DCBE8 File Offset: 0x001DADE8
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600D977 RID: 55671 RVA: 0x001DCBF1 File Offset: 0x001DADF1
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A4FE RID: 42238
		public const ushort VERSION = 6;

		// Token: 0x0400A4FF RID: 42239
		public const int HashSizeBytes = 16;

		// Token: 0x0400A500 RID: 42240
		public const int SizeBytes = 28;

		// Token: 0x0400A501 RID: 42241
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A502 RID: 42242
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A504 RID: 42244
		public ulong Id;

		// Token: 0x0400A505 RID: 42245
		public uint Length;

		// Token: 0x0400A506 RID: 42246
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlyMemory<byte> m_hash = BlobRef.s_emptyHash;
	}
}
