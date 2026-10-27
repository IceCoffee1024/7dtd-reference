using System;
using UnityEngine;

// Token: 0x02000195 RID: 405
public struct PropTransform : IEquatable<PropTransform>
{
	// Token: 0x06000C3B RID: 3131 RVA: 0x0004C7F8 File Offset: 0x0004A9F8
	public static PropTransform Read(PooledBinaryReader br)
	{
		return new PropTransform
		{
			position = StreamUtils.ReadVector3(br),
			rotation = StreamUtils.ReadQuaterion(br),
			scale = StreamUtils.ReadVector3(br)
		};
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0004C835 File Offset: 0x0004AA35
	public readonly void Write(PooledBinaryWriter bw)
	{
		StreamUtils.Write(bw, this.position);
		StreamUtils.Write(bw, this.rotation);
		StreamUtils.Write(bw, this.scale);
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0004C85B File Offset: 0x0004AA5B
	public override string ToString()
	{
		return string.Format("position={0}, rotation={1}, scale={2}", this.position, this.rotation, this.scale);
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x0004C888 File Offset: 0x0004AA88
	public bool Equals(PropTransform other)
	{
		return this.position.Equals(other.position) && this.rotation.Equals(other.rotation) && this.scale.Equals(other.scale);
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0004C8C4 File Offset: 0x0004AAC4
	public override bool Equals(object obj)
	{
		if (obj is PropTransform)
		{
			PropTransform propTransform = (PropTransform)obj;
			return this.Equals(propTransform);
		}
		return false;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x0004C8E9 File Offset: 0x0004AAE9
	public override int GetHashCode()
	{
		return HashCode.Combine<Vector3, Quaternion, Vector3>(this.position, this.rotation, this.scale);
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x0004C902 File Offset: 0x0004AB02
	public static bool operator ==(PropTransform left, PropTransform right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x0004C90C File Offset: 0x0004AB0C
	public static bool operator !=(PropTransform left, PropTransform right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B0D RID: 2829
	public static readonly PropTransform identity = new PropTransform
	{
		position = Vector3.zero,
		rotation = Quaternion.identity,
		scale = Vector3.one
	};

	// Token: 0x04000B0E RID: 2830
	public Vector3 position;

	// Token: 0x04000B0F RID: 2831
	public Quaternion rotation;

	// Token: 0x04000B10 RID: 2832
	public Vector3 scale;
}
