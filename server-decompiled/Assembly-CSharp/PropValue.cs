using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000195 RID: 405
[Serializable]
public struct PropValue : IEquatable<PropValue>
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000C4D RID: 3149 RVA: 0x0004C997 File Offset: 0x0004AB97
	// (set) Token: 0x06000C4E RID: 3150 RVA: 0x0004C9A4 File Offset: 0x0004ABA4
	public Vector3 position
	{
		get
		{
			return this.transform.position;
		}
		set
		{
			this.transform.position = value;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0004C9B2 File Offset: 0x0004ABB2
	// (set) Token: 0x06000C50 RID: 3152 RVA: 0x0004C9BF File Offset: 0x0004ABBF
	public Quaternion rotation
	{
		get
		{
			return this.transform.rotation;
		}
		set
		{
			this.transform.rotation = value;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000C51 RID: 3153 RVA: 0x0004C9CD File Offset: 0x0004ABCD
	// (set) Token: 0x06000C52 RID: 3154 RVA: 0x0004C9DA File Offset: 0x0004ABDA
	public Vector3 scale
	{
		get
		{
			return this.transform.scale;
		}
		set
		{
			this.transform.scale = value;
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000C53 RID: 3155 RVA: 0x0004C9E8 File Offset: 0x0004ABE8
	public bool IsAir
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.blockValue.isair;
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x0004C9F8 File Offset: 0x0004ABF8
	public static PropValue Read(BinaryReader br)
	{
		return new PropValue
		{
			transform = PropTransform.Read(br),
			blockValue = BlockValue.Read(br)
		};
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x0004CA28 File Offset: 0x0004AC28
	public readonly void Write(BinaryWriter bw)
	{
		this.transform.Write(bw);
		this.blockValue.Write(bw);
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x0004CA42 File Offset: 0x0004AC42
	public override string ToString()
	{
		return string.Format("transform=[{0}], blockValue=[{1}]", this.transform, this.blockValue);
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x0004CA64 File Offset: 0x0004AC64
	public bool Equals(PropValue other)
	{
		return this.transform.Equals(other.transform) && this.blockValue.Equals(other.blockValue);
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0004CA8C File Offset: 0x0004AC8C
	public override bool Equals(object obj)
	{
		if (obj is PropValue)
		{
			PropValue propValue = (PropValue)obj;
			return this.Equals(propValue);
		}
		return false;
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0004CAB1 File Offset: 0x0004ACB1
	public override int GetHashCode()
	{
		return HashCode.Combine<PropTransform, BlockValue>(this.transform, this.blockValue);
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x0004CAC4 File Offset: 0x0004ACC4
	public static bool operator ==(PropValue left, PropValue right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0004CACE File Offset: 0x0004ACCE
	public static bool operator !=(PropValue left, PropValue right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B18 RID: 2840
	public static readonly PropValue AIR = new PropValue
	{
		transform = PropTransform.identity,
		blockValue = BlockValue.Air
	};

	// Token: 0x04000B19 RID: 2841
	public PropTransform transform;

	// Token: 0x04000B1A RID: 2842
	public BlockValue blockValue;
}
