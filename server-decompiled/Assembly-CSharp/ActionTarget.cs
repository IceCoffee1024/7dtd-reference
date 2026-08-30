using System;
using System.IO;
using UnityEngine;

// Token: 0x02000525 RID: 1317
public struct ActionTarget : IEquatable<ActionTarget>
{
	// Token: 0x06002B56 RID: 11094 RVA: 0x00111CE5 File Offset: 0x0010FEE5
	[PublicizedFrom(EAccessModifier.Private)]
	public ActionTarget(ActionTargetType type, Vector3 position, BlockValueRef blockValueReference)
	{
		this.Type = type;
		this.Position = position;
		this.BlockValueReference = blockValueReference;
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x00111CFC File Offset: 0x0010FEFC
	public ActionTarget(float x, float y, float z)
	{
		this = new ActionTarget(new Vector3(x, y, z));
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x00111D0C File Offset: 0x0010FF0C
	public ActionTarget(Vector3 pos)
	{
		this = new ActionTarget(ActionTargetType.Position, pos, BlockValueRef.None);
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x00111D1B File Offset: 0x0010FF1B
	public ActionTarget(int x, int y, int z)
	{
		this = new ActionTarget(new BlockValueRef(x, y, z));
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x00111D2B File Offset: 0x0010FF2B
	public ActionTarget(Vector3i pos)
	{
		this = new ActionTarget(new BlockValueRef(pos));
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x00111D39 File Offset: 0x0010FF39
	public ActionTarget(BlockValueRef bvRef)
	{
		this = new ActionTarget(ActionTargetType.BlockValueRef, Vector3.zero, bvRef);
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x00111D48 File Offset: 0x0010FF48
	public static ActionTarget Read(BinaryReader br)
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

	// Token: 0x06002B5D RID: 11101 RVA: 0x00111D9C File Offset: 0x0010FF9C
	public readonly void Write(BinaryWriter bw)
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

	// Token: 0x06002B5E RID: 11102 RVA: 0x00111DF0 File Offset: 0x0010FFF0
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

	// Token: 0x06002B5F RID: 11103 RVA: 0x00111E48 File Offset: 0x00110048
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

	// Token: 0x06002B60 RID: 11104 RVA: 0x00111E8A File Offset: 0x0011008A
	public static implicit operator BlockValueRef(ActionTarget target)
	{
		if (target.Type == ActionTargetType.BlockValueRef)
		{
			return target.BlockValueReference;
		}
		Log.Warning(string.Format("[PROPS] ActionTarget implicitly converted to BlockValueRef, but failed: {0}", target));
		return BlockValueRef.None;
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x00111EB6 File Offset: 0x001100B6
	public static implicit operator ActionTarget(Vector3 pos)
	{
		return new ActionTarget(pos);
	}

	// Token: 0x06002B62 RID: 11106 RVA: 0x00111EBE File Offset: 0x001100BE
	public static implicit operator ActionTarget(Vector3i pos)
	{
		return new ActionTarget(new BlockValueRef(pos));
	}

	// Token: 0x06002B63 RID: 11107 RVA: 0x00111ECB File Offset: 0x001100CB
	public static implicit operator ActionTarget(PropRef propRef)
	{
		return new ActionTarget(new BlockValueRef(propRef));
	}

	// Token: 0x06002B64 RID: 11108 RVA: 0x00111ED8 File Offset: 0x001100D8
	public static implicit operator ActionTarget(BlockValueRef bvRef)
	{
		return new ActionTarget(bvRef);
	}

	// Token: 0x06002B65 RID: 11109 RVA: 0x00111EE0 File Offset: 0x001100E0
	public bool Equals(ActionTarget other)
	{
		return this.Type == other.Type && this.Position.Equals(other.Position) && this.BlockValueReference.Equals(other.BlockValueReference);
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x00111F18 File Offset: 0x00110118
	public override bool Equals(object obj)
	{
		if (obj is ActionTarget)
		{
			ActionTarget actionTarget = (ActionTarget)obj;
			return this.Equals(actionTarget);
		}
		return false;
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x00111F3D File Offset: 0x0011013D
	public override int GetHashCode()
	{
		return HashCode.Combine<int, Vector3, BlockValueRef>((int)this.Type, this.Position, this.BlockValueReference);
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x00111F56 File Offset: 0x00110156
	public static bool operator ==(ActionTarget left, ActionTarget right)
	{
		return left.Equals(right);
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x00111F60 File Offset: 0x00110160
	public static bool operator !=(ActionTarget left, ActionTarget right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04002125 RID: 8485
	public static readonly ActionTarget None = new ActionTarget(ActionTargetType.None, Vector3.zero, BlockValueRef.None);

	// Token: 0x04002126 RID: 8486
	public ActionTargetType Type;

	// Token: 0x04002127 RID: 8487
	public Vector3 Position;

	// Token: 0x04002128 RID: 8488
	public BlockValueRef BlockValueReference;
}
