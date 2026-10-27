using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000196 RID: 406
[Serializable]
public struct PropValue : IEquatable<PropValue>
{
	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0004C95B File Offset: 0x0004AB5B
	// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0004C968 File Offset: 0x0004AB68
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

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0004C976 File Offset: 0x0004AB76
	// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0004C983 File Offset: 0x0004AB83
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

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0004C991 File Offset: 0x0004AB91
	// (set) Token: 0x06000C49 RID: 3145 RVA: 0x0004C99E File Offset: 0x0004AB9E
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

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0004C9AC File Offset: 0x0004ABAC
	public bool IsAir
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.blockValue.isair;
		}
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0004C9BC File Offset: 0x0004ABBC
	public static PropValue Read(PooledBinaryReader br)
	{
		return new PropValue
		{
			transform = PropTransform.Read(br),
			blockValue = BlockValue.Read(br)
		};
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0004C9EC File Offset: 0x0004ABEC
	public readonly void Write(PooledBinaryWriter bw)
	{
		this.transform.Write(bw);
		this.blockValue.Write(bw);
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x0004CA06 File Offset: 0x0004AC06
	public override string ToString()
	{
		return string.Format("transform=[{0}], blockValue=[{1}]", this.transform, this.blockValue);
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x0004CA28 File Offset: 0x0004AC28
	public bool Equals(PropValue other)
	{
		return this.transform.Equals(other.transform) && this.blockValue.Equals(other.blockValue);
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x0004CA50 File Offset: 0x0004AC50
	public override bool Equals(object obj)
	{
		if (obj is PropValue)
		{
			PropValue propValue = (PropValue)obj;
			return this.Equals(propValue);
		}
		return false;
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x0004CA75 File Offset: 0x0004AC75
	public override int GetHashCode()
	{
		return HashCode.Combine<PropTransform, BlockValue>(this.transform, this.blockValue);
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x0004CA88 File Offset: 0x0004AC88
	public static bool operator ==(PropValue left, PropValue right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x0004CA92 File Offset: 0x0004AC92
	public static bool operator !=(PropValue left, PropValue right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B11 RID: 2833
	public static readonly PropValue AIR = new PropValue
	{
		transform = PropTransform.identity,
		blockValue = BlockValue.Air
	};

	// Token: 0x04000B12 RID: 2834
	public PropTransform transform;

	// Token: 0x04000B13 RID: 2835
	public BlockValue blockValue;
}
