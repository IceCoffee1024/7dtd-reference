using System;
using System.IO;

// Token: 0x02000190 RID: 400
public struct PropRef : IEquatable<PropRef>
{
	// Token: 0x06000C2A RID: 3114 RVA: 0x0004C24C File Offset: 0x0004A44C
	public static PropRef Read(BinaryReader br)
	{
		return new PropRef
		{
			ChunkPos = StreamUtils.ReadVector2i(br),
			PropId = br.ReadInt32()
		};
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0004C27C File Offset: 0x0004A47C
	public readonly void Write(BinaryWriter bw)
	{
		StreamUtils.Write(bw, this.ChunkPos);
		bw.Write(this.PropId);
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x0004C296 File Offset: 0x0004A496
	public override string ToString()
	{
		return string.Format("ChunkPos=({0}), PropId={1}", this.ChunkPos, this.PropId);
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x0004C2B8 File Offset: 0x0004A4B8
	public bool Equals(PropRef other)
	{
		return this.ChunkPos.Equals(other.ChunkPos) && this.PropId == other.PropId;
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x0004C2E0 File Offset: 0x0004A4E0
	public override bool Equals(object obj)
	{
		if (obj is PropRef)
		{
			PropRef propRef = (PropRef)obj;
			return this.Equals(propRef);
		}
		return false;
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x0004C305 File Offset: 0x0004A505
	public override int GetHashCode()
	{
		return HashCode.Combine<Vector2i, int>(this.ChunkPos, this.PropId);
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x0004C318 File Offset: 0x0004A518
	public static bool operator ==(PropRef left, PropRef right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0004C322 File Offset: 0x0004A522
	public static bool operator !=(PropRef left, PropRef right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B01 RID: 2817
	public static readonly PropRef Default = new PropRef
	{
		ChunkPos = Vector2i.zero,
		PropId = 0
	};

	// Token: 0x04000B02 RID: 2818
	public Vector2i ChunkPos;

	// Token: 0x04000B03 RID: 2819
	public int PropId;
}
