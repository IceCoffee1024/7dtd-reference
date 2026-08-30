using System;
using System.IO;

// Token: 0x02000192 RID: 402
public struct PropRef : IEquatable<PropRef>
{
	// Token: 0x06000C35 RID: 3125 RVA: 0x0004C664 File Offset: 0x0004A864
	public static PropRef Read(BinaryReader br)
	{
		return new PropRef
		{
			ChunkPos = StreamUtils.ReadVector2i(br),
			PropId = br.ReadInt32()
		};
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x0004C694 File Offset: 0x0004A894
	public readonly void Write(BinaryWriter bw)
	{
		StreamUtils.Write(bw, this.ChunkPos);
		bw.Write(this.PropId);
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x0004C6AE File Offset: 0x0004A8AE
	public override string ToString()
	{
		return string.Format("ChunkPos=({0}), PropId={1}", this.ChunkPos, this.PropId);
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0004C6D0 File Offset: 0x0004A8D0
	public bool Equals(PropRef other)
	{
		return this.ChunkPos.Equals(other.ChunkPos) && this.PropId == other.PropId;
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x0004C6F8 File Offset: 0x0004A8F8
	public override bool Equals(object obj)
	{
		if (obj is PropRef)
		{
			PropRef propRef = (PropRef)obj;
			return this.Equals(propRef);
		}
		return false;
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0004C71D File Offset: 0x0004A91D
	public override int GetHashCode()
	{
		return HashCode.Combine<Vector2i, int>(this.ChunkPos, this.PropId);
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x0004C730 File Offset: 0x0004A930
	public static bool operator ==(PropRef left, PropRef right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0004C73A File Offset: 0x0004A93A
	public static bool operator !=(PropRef left, PropRef right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B0E RID: 2830
	public static readonly PropRef Default = new PropRef
	{
		ChunkPos = Vector2i.zero,
		PropId = 0
	};

	// Token: 0x04000B0F RID: 2831
	public Vector2i ChunkPos;

	// Token: 0x04000B10 RID: 2832
	public int PropId;
}
