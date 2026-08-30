using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000193 RID: 403
[Serializable]
public struct PropValue : IEquatable<PropValue>
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0004C57F File Offset: 0x0004A77F
	// (set) Token: 0x06000C43 RID: 3139 RVA: 0x0004C58C File Offset: 0x0004A78C
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
	// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0004C59A File Offset: 0x0004A79A
	// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0004C5A7 File Offset: 0x0004A7A7
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
	// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0004C5B5 File Offset: 0x0004A7B5
	// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0004C5C2 File Offset: 0x0004A7C2
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
	// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0004C5D0 File Offset: 0x0004A7D0
	public bool IsAir
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.blockValue.isair;
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0004C5E0 File Offset: 0x0004A7E0
	public static PropValue Read(BinaryReader br)
	{
		return new PropValue
		{
			transform = PropTransform.Read(br),
			blockValue = BlockValue.Read(br)
		};
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x0004C610 File Offset: 0x0004A810
	public readonly void Write(BinaryWriter bw)
	{
		this.transform.Write(bw);
		this.blockValue.Write(bw);
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0004C62A File Offset: 0x0004A82A
	public override string ToString()
	{
		return string.Format("transform=[{0}], blockValue=[{1}]", this.transform, this.blockValue);
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0004C64C File Offset: 0x0004A84C
	public bool Equals(PropValue other)
	{
		return this.transform.Equals(other.transform) && this.blockValue.Equals(other.blockValue);
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x0004C674 File Offset: 0x0004A874
	public override bool Equals(object obj)
	{
		if (obj is PropValue)
		{
			PropValue propValue = (PropValue)obj;
			return this.Equals(propValue);
		}
		return false;
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x0004C699 File Offset: 0x0004A899
	public override int GetHashCode()
	{
		return HashCode.Combine<PropTransform, BlockValue>(this.transform, this.blockValue);
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x0004C6AC File Offset: 0x0004A8AC
	public static bool operator ==(PropValue left, PropValue right)
	{
		return left.Equals(right);
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x0004C6B6 File Offset: 0x0004A8B6
	public static bool operator !=(PropValue left, PropValue right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04000B0B RID: 2827
	public static readonly PropValue AIR = new PropValue
	{
		transform = PropTransform.identity,
		blockValue = BlockValue.Air
	};

	// Token: 0x04000B0C RID: 2828
	public PropTransform transform;

	// Token: 0x04000B0D RID: 2829
	public BlockValue blockValue;
}
