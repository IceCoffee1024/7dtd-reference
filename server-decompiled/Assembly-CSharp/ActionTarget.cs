using System;
using UnityEngine;

// Token: 0x0200052B RID: 1323
public struct ActionTarget : IEquatable<ActionTarget>
{
	// Token: 0x06002B4C RID: 11084 RVA: 0x00112AB5 File Offset: 0x00110CB5
	[PublicizedFrom(EAccessModifier.Private)]
	public ActionTarget(ActionTargetType type, Vector3 position, BlockValueRef blockValueReference)
	{
		this.Type = type;
		this.Position = position;
		this.BlockValueReference = blockValueReference;
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x00112ACC File Offset: 0x00110CCC
	public ActionTarget(float x, float y, float z)
	{
		this = new ActionTarget(new Vector3(x, y, z));
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x00112ADC File Offset: 0x00110CDC
	public ActionTarget(Vector3 pos)
	{
		this = new ActionTarget(ActionTargetType.Position, pos, BlockValueRef.None);
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x00112AEB File Offset: 0x00110CEB
	public ActionTarget(int x, int y, int z)
	{
		this = new ActionTarget(new BlockValueRef(x, y, z));
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x00112AFB File Offset: 0x00110CFB
	public ActionTarget(Vector3i pos)
	{
		this = new ActionTarget(new BlockValueRef(pos));
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x00112B09 File Offset: 0x00110D09
	public ActionTarget(BlockValueRef bvRef)
	{
		this = new ActionTarget(ActionTargetType.BlockValueRef, Vector3.zero, bvRef);
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x00112B18 File Offset: 0x00110D18
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

	// Token: 0x06002B53 RID: 11091 RVA: 0x00112B6C File Offset: 0x00110D6C
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

	// Token: 0x06002B54 RID: 11092 RVA: 0x00112BC0 File Offset: 0x00110DC0
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

	// Token: 0x06002B55 RID: 11093 RVA: 0x00112C18 File Offset: 0x00110E18
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

	// Token: 0x06002B56 RID: 11094 RVA: 0x00112C5A File Offset: 0x00110E5A
	public static implicit operator BlockValueRef(ActionTarget target)
	{
		if (target.Type == ActionTargetType.BlockValueRef)
		{
			return target.BlockValueReference;
		}
		Log.Warning(string.Format("[PROPS] ActionTarget implicitly converted to BlockValueRef, but failed: {0}", target));
		return BlockValueRef.None;
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x00112C86 File Offset: 0x00110E86
	public static implicit operator ActionTarget(Vector3 pos)
	{
		return new ActionTarget(pos);
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x00112C8E File Offset: 0x00110E8E
	public static implicit operator ActionTarget(Vector3i pos)
	{
		return new ActionTarget(new BlockValueRef(pos));
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x00112C9B File Offset: 0x00110E9B
	public static implicit operator ActionTarget(PropRef propRef)
	{
		return new ActionTarget(new BlockValueRef(propRef));
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x00112CA8 File Offset: 0x00110EA8
	public static implicit operator ActionTarget(BlockValueRef bvRef)
	{
		return new ActionTarget(bvRef);
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x00112CB0 File Offset: 0x00110EB0
	public bool Equals(ActionTarget other)
	{
		return this.Type == other.Type && this.Position.Equals(other.Position) && this.BlockValueReference.Equals(other.BlockValueReference);
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x00112CE8 File Offset: 0x00110EE8
	public override bool Equals(object obj)
	{
		if (obj is ActionTarget)
		{
			ActionTarget actionTarget = (ActionTarget)obj;
			return this.Equals(actionTarget);
		}
		return false;
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x00112D0D File Offset: 0x00110F0D
	public override int GetHashCode()
	{
		return HashCode.Combine<int, Vector3, BlockValueRef>((int)this.Type, this.Position, this.BlockValueReference);
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x00112D26 File Offset: 0x00110F26
	public static bool operator ==(ActionTarget left, ActionTarget right)
	{
		return left.Equals(right);
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x00112D30 File Offset: 0x00110F30
	public static bool operator !=(ActionTarget left, ActionTarget right)
	{
		return !left.Equals(right);
	}

	// Token: 0x0400212D RID: 8493
	public static readonly ActionTarget None = new ActionTarget(ActionTargetType.None, Vector3.zero, BlockValueRef.None);

	// Token: 0x0400212E RID: 8494
	public ActionTargetType Type;

	// Token: 0x0400212F RID: 8495
	public Vector3 Position;

	// Token: 0x04002130 RID: 8496
	public BlockValueRef BlockValueReference;
}
