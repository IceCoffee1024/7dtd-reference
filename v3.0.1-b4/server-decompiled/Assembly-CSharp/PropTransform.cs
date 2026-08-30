using System;
using System.IO;
using UnityEngine;

// Token: 0x02000192 RID: 402
public struct PropTransform : IEquatable<PropTransform>
{
	// Token: 0x06000C39 RID: 3129 RVA: 0x0004C41C File Offset: 0x0004A61C
	public static PropTransform Read(BinaryReader br)
	{
		return new PropTransform
		{
			position = StreamUtils.ReadVector3(br),
			rotation = StreamUtils.ReadQuaterion(br),
			scale = StreamUtils.ReadVector3(br)
		};
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0004C459 File Offset: 0x0004A659
	public readonly void Write(BinaryWriter bw)
	{
		StreamUtils.Write(bw, this.position);
		StreamUtils.Write(bw, this.rotation);
		StreamUtils.Write(bw, this.scale);
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x0004C47F File Offset: 0x0004A67F
	public override string ToString()
	{
		return string.Format("position={0}, rotation={1}, scale={2}", this.position, this.rotation, this.scale);
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0004C4AC File Offset: 0x0004A6AC
	public bool Equals(PropTransform other)
	{
		return this.position.Equals(other.position) && this.rotation.Equals(other.rotation) && this.scale.Equals(other.scale);
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0004C4E8 File Offset: 0x0004A6E8
	public override bool Equals(object obj)
	{
		if (obj is PropTransform)
		{
			PropTransform propTransform = (PropTransform)obj;
			return this.Equals(propTransform);
		}
		return false;
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x0004C50D File Offset: 0x0004A70D
	public override int GetHashCode()
	{
		return HashCode.Combine<Vector3, Quaternion, Vector3>(this.position, this.rotation, this.scale);
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0004C526 File Offset: 0x0004A726
	public static bool operator ==(PropTransform left, PropTransform right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x0004C530 File Offset: 0x0004A730
	public static bool operator !=(PropTransform left, PropTransform right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B07 RID: 2823
	public static readonly PropTransform identity = new PropTransform
	{
		position = Vector3.zero,
		rotation = Quaternion.identity,
		scale = Vector3.one
	};

	// Token: 0x04000B08 RID: 2824
	public Vector3 position;

	// Token: 0x04000B09 RID: 2825
	public Quaternion rotation;

	// Token: 0x04000B0A RID: 2826
	public Vector3 scale;
}
