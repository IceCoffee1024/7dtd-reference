using System;
using System.IO;
using UnityEngine;

// Token: 0x02000194 RID: 404
public struct PropTransform : IEquatable<PropTransform>
{
	// Token: 0x06000C44 RID: 3140 RVA: 0x0004C834 File Offset: 0x0004AA34
	public static PropTransform Read(BinaryReader br)
	{
		return new PropTransform
		{
			position = StreamUtils.ReadVector3(br),
			rotation = StreamUtils.ReadQuaterion(br),
			scale = StreamUtils.ReadVector3(br)
		};
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x0004C871 File Offset: 0x0004AA71
	public readonly void Write(BinaryWriter bw)
	{
		StreamUtils.Write(bw, this.position);
		StreamUtils.Write(bw, this.rotation);
		StreamUtils.Write(bw, this.scale);
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0004C897 File Offset: 0x0004AA97
	public override string ToString()
	{
		return string.Format("position={0}, rotation={1}, scale={2}", this.position, this.rotation, this.scale);
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0004C8C4 File Offset: 0x0004AAC4
	public bool Equals(PropTransform other)
	{
		return this.position.Equals(other.position) && this.rotation.Equals(other.rotation) && this.scale.Equals(other.scale);
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x0004C900 File Offset: 0x0004AB00
	public override bool Equals(object obj)
	{
		if (obj is PropTransform)
		{
			PropTransform propTransform = (PropTransform)obj;
			return this.Equals(propTransform);
		}
		return false;
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0004C925 File Offset: 0x0004AB25
	public override int GetHashCode()
	{
		return HashCode.Combine<Vector3, Quaternion, Vector3>(this.position, this.rotation, this.scale);
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x0004C93E File Offset: 0x0004AB3E
	public static bool operator ==(PropTransform left, PropTransform right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0004C948 File Offset: 0x0004AB48
	public static bool operator !=(PropTransform left, PropTransform right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B14 RID: 2836
	public static readonly PropTransform identity = new PropTransform
	{
		position = Vector3.zero,
		rotation = Quaternion.identity,
		scale = Vector3.one
	};

	// Token: 0x04000B15 RID: 2837
	public Vector3 position;

	// Token: 0x04000B16 RID: 2838
	public Quaternion rotation;

	// Token: 0x04000B17 RID: 2839
	public Vector3 scale;
}
