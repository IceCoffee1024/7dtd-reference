using System;

// Token: 0x02000193 RID: 403
public struct PropRef : IEquatable<PropRef>
{
	// Token: 0x06000C2C RID: 3116 RVA: 0x0004C628 File Offset: 0x0004A828
	public static PropRef Read(PooledBinaryReader br)
	{
		return new PropRef
		{
			ChunkPos = StreamUtils.ReadVector2i(br),
			PropId = br.ReadInt32()
		};
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x0004C658 File Offset: 0x0004A858
	public readonly void Write(PooledBinaryWriter bw)
	{
		StreamUtils.Write(bw, this.ChunkPos);
		bw.Write(this.PropId);
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x0004C672 File Offset: 0x0004A872
	public override string ToString()
	{
		return string.Format("ChunkPos=({0}), PropId={1}", this.ChunkPos, this.PropId);
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x0004C694 File Offset: 0x0004A894
	public bool Equals(PropRef other)
	{
		return this.ChunkPos.Equals(other.ChunkPos) && this.PropId == other.PropId;
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x0004C6BC File Offset: 0x0004A8BC
	public override bool Equals(object obj)
	{
		if (obj is PropRef)
		{
			PropRef propRef = (PropRef)obj;
			return this.Equals(propRef);
		}
		return false;
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0004C6E1 File Offset: 0x0004A8E1
	public override int GetHashCode()
	{
		return HashCode.Combine<Vector2i, int>(this.ChunkPos, this.PropId);
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0004C6F4 File Offset: 0x0004A8F4
	public static bool operator ==(PropRef left, PropRef right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x0004C6FE File Offset: 0x0004A8FE
	public static bool operator !=(PropRef left, PropRef right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B07 RID: 2823
	public static readonly PropRef Default = new PropRef
	{
		ChunkPos = Vector2i.zero,
		PropId = 0
	};

	// Token: 0x04000B08 RID: 2824
	public Vector2i ChunkPos;

	// Token: 0x04000B09 RID: 2825
	public int PropId;
}
