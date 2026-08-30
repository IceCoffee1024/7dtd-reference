using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001C4D RID: 7245
	public sealed class BlobRef : IEquatable<BlobRef>, IMigratable
	{
		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x0600D65F RID: 54879 RVA: 0x004D4497 File Offset: 0x004D2697
		// (set) Token: 0x0600D660 RID: 54880 RVA: 0x004D449F File Offset: 0x004D269F
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001A54 RID: 6740
		// (get) Token: 0x0600D661 RID: 54881 RVA: 0x004D44A8 File Offset: 0x004D26A8
		// (set) Token: 0x0600D662 RID: 54882 RVA: 0x004D44B0 File Offset: 0x004D26B0
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

		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x0600D663 RID: 54883 RVA: 0x004D44DB File Offset: 0x004D26DB
		// (set) Token: 0x0600D664 RID: 54884 RVA: 0x004D44E3 File Offset: 0x004D26E3
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x0600D665 RID: 54885 RVA: 0x004D44EC File Offset: 0x004D26EC
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

		// Token: 0x0600D666 RID: 54886 RVA: 0x004D4634 File Offset: 0x004D2834
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

		// Token: 0x0600D667 RID: 54887 RVA: 0x004D4760 File Offset: 0x004D2960
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

		// Token: 0x0600D668 RID: 54888 RVA: 0x004D47C3 File Offset: 0x004D29C3
		public static HashAlgorithm GetHashAlgorithm()
		{
			return BlobRef.s_md5.Value;
		}

		// Token: 0x0600D669 RID: 54889 RVA: 0x004D47D0 File Offset: 0x004D29D0
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

		// Token: 0x0600D66A RID: 54890 RVA: 0x004D4830 File Offset: 0x004D2A30
		public bool Equals(BlobRef other)
		{
			return other != null && (this == other || (this.Id == other.Id && this.Length == other.Length && (this.Hash.Equals(other.Hash) || this.Hash.Span.SequenceEqual<byte>(other.Hash.Span))));
		}

		// Token: 0x0600D66B RID: 54891 RVA: 0x004D48A0 File Offset: 0x004D2AA0
		public override bool Equals(object obj)
		{
			if (this != obj)
			{
				BlobRef blobRef = obj as BlobRef;
				return blobRef != null && this.Equals(blobRef);
			}
			return true;
		}

		// Token: 0x0600D66C RID: 54892 RVA: 0x004D48C8 File Offset: 0x004D2AC8
		public override int GetHashCode()
		{
			return HashCode.Combine<ulong, uint, int>(this.Id, this.Length, SpanUtils.GetHashCode<byte>(this.Hash.Span));
		}

		// Token: 0x0600D66D RID: 54893 RVA: 0x001D8EA0 File Offset: 0x001D70A0
		public static bool operator ==(BlobRef left, BlobRef right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x0600D66E RID: 54894 RVA: 0x001D8EA9 File Offset: 0x001D70A9
		public static bool operator !=(BlobRef left, BlobRef right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400A2F8 RID: 41720
		public const ushort VERSION = 6;

		// Token: 0x0400A2F9 RID: 41721
		public const int HashSizeBytes = 16;

		// Token: 0x0400A2FA RID: 41722
		public const int SizeBytes = 28;

		// Token: 0x0400A2FB RID: 41723
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ThreadLocal<MD5> s_md5 = new ThreadLocal<MD5>(new Func<MD5>(MD5.Create));

		// Token: 0x0400A2FC RID: 41724
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] s_emptyHash = new byte[16];

		// Token: 0x0400A2FE RID: 41726
		public ulong Id;

		// Token: 0x0400A2FF RID: 41727
		public uint Length;

		// Token: 0x0400A300 RID: 41728
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlyMemory<byte> m_hash = BlobRef.s_emptyHash;
	}
}
