using System;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public struct ActionTarget : IEquatable<ActionTarget>
{
	// Token: 0x06002B54 RID: 11092 RVA: 0x00113041 File Offset: 0x00111241
	[PublicizedFrom(EAccessModifier.Private)]
	public ActionTarget(ActionTargetType type, Vector3 position, BlockValueRef blockValueReference)
	{
		this.Type = type;
		this.Position = position;
		this.BlockValueReference = blockValueReference;
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x00113058 File Offset: 0x00111258
	public ActionTarget(float x, float y, float z)
	{
		this = new ActionTarget(new Vector3(x, y, z));
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x00113068 File Offset: 0x00111268
	public ActionTarget(Vector3 pos)
	{
		this = new ActionTarget(ActionTargetType.Position, pos, BlockValueRef.None);
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x00113077 File Offset: 0x00111277
	public ActionTarget(int x, int y, int z)
	{
		this = new ActionTarget(new BlockValueRef(x, y, z));
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x00113087 File Offset: 0x00111287
	public ActionTarget(Vector3i pos)
	{
		this = new ActionTarget(new BlockValueRef(pos));
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x00113095 File Offset: 0x00111295
	public ActionTarget(BlockValueRef bvRef)
	{
		this = new ActionTarget(ActionTargetType.BlockValueRef, Vector3.zero, bvRef);
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x001130A4 File Offset: 0x001112A4
	public static ActionTarget Read(PooledBinaryReader br)
	{
		ActionTarget actionTarget;
		switch (br.ReadByte())
		{
		case 0:
			actionTarget = ActionTarget.None;
			break;
		case 1:
			actionTarget = new ActionTarget(StreamUtils.ReadVector3(br));
			break;
		case 2:
			actionTarget = new ActionTarget(BlockValueRef.Read(br));
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return actionTarget;
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x001130F8 File Offset: 0x001112F8
	public readonly void Write(PooledBinaryWriter bw)
	{
		bw.Write((byte)this.Type);
		switch (this.Type)
		{
		case ActionTargetType.None:
			return;
		case ActionTargetType.Position:
			StreamUtils.Write(bw, this.Position);
			return;
		case ActionTargetType.BlockValueRef:
			this.BlockValueReference.Write(bw);
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x0011314C File Offset: 0x0011134C
	public static implicit operator Vector3(ActionTarget target)
	{
		if (target.Type == ActionTargetType.Position)
		{
			return target.Position;
		}
		Vector3i vector3i;
		if (target.Type == ActionTargetType.BlockValueRef && target.BlockValueReference.TryGetBlockPos(out vector3i))
		{
			return vector3i;
		}
		Log.Warning(string.Format("[PROPS] ActionTarget implicitly converted to Vector3, but failed: {0}", target));
		return Vector3.zero;
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x001131A4 File Offset: 0x001113A4
	public static implicit operator Vector3i(ActionTarget target)
	{
		Vector3i vector3i;
		if (target.Type == ActionTargetType.BlockValueRef && target.BlockValueReference.TryGetBlockPos(out vector3i))
		{
			return vector3i;
		}
		Log.Warning(string.Format("[PROPS] ActionTarget implicitly converted to Vector3i, but failed: {0}", target));
		return Vector3i.zero;
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x001131E6 File Offset: 0x001113E6
	public static implicit operator BlockValueRef(ActionTarget target)
	{
		if (target.Type == ActionTargetType.BlockValueRef)
		{
			return target.BlockValueReference;
		}
		Log.Warning(string.Format("[PROPS] ActionTarget implicitly converted to BlockValueRef, but failed: {0}", target));
		return BlockValueRef.None;
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x00113212 File Offset: 0x00111412
	public static implicit operator ActionTarget(Vector3 pos)
	{
		return new ActionTarget(pos);
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x0011321A File Offset: 0x0011141A
	public static implicit operator ActionTarget(Vector3i pos)
	{
		return new ActionTarget(new BlockValueRef(pos));
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x00113227 File Offset: 0x00111427
	public static implicit operator ActionTarget(PropRef propRef)
	{
		return new ActionTarget(new BlockValueRef(propRef));
	}

	// Token: 0x06002B62 RID: 11106 RVA: 0x00113234 File Offset: 0x00111434
	public static implicit operator ActionTarget(BlockValueRef bvRef)
	{
		return new ActionTarget(bvRef);
	}

	// Token: 0x06002B63 RID: 11107 RVA: 0x0011323C File Offset: 0x0011143C
	public bool Equals(ActionTarget other)
	{
		return this.Type == other.Type && this.Position.Equals(other.Position) && this.BlockValueReference.Equals(other.BlockValueReference);
	}

	// Token: 0x06002B64 RID: 11108 RVA: 0x00113274 File Offset: 0x00111474
	public override bool Equals(object obj)
	{
		if (obj is ActionTarget)
		{
			ActionTarget actionTarget = (ActionTarget)obj;
			return this.Equals(actionTarget);
		}
		return false;
	}

	// Token: 0x06002B65 RID: 11109 RVA: 0x00113299 File Offset: 0x00111499
	public override int GetHashCode()
	{
		return HashCode.Combine<int, Vector3, BlockValueRef>((int)this.Type, this.Position, this.BlockValueReference);
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x001132B2 File Offset: 0x001114B2
	public static bool operator ==(ActionTarget left, ActionTarget right)
	{
		return left.Equals(right);
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x001132BC File Offset: 0x001114BC
	public static bool operator !=(ActionTarget left, ActionTarget right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04002144 RID: 8516
	public static readonly ActionTarget None = new ActionTarget(ActionTargetType.None, Vector3.zero, BlockValueRef.None);

	// Token: 0x04002145 RID: 8517
	public ActionTargetType Type;

	// Token: 0x04002146 RID: 8518
	public Vector3 Position;

	// Token: 0x04002147 RID: 8519
	public BlockValueRef BlockValueReference;
}
