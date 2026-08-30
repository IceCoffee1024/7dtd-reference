using System;
using System.IO;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public struct ActionTarget : IEquatable<ActionTarget>
{
	// Token: 0x06002B2C RID: 11052 RVA: 0x00110C6D File Offset: 0x0010EE6D
	[PublicizedFrom(EAccessModifier.Private)]
	public ActionTarget(ActionTargetType type, Vector3 position, BlockValueRef blockValueReference)
	{
		this.Type = type;
		this.Position = position;
		this.BlockValueReference = blockValueReference;
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x00110C84 File Offset: 0x0010EE84
	public ActionTarget(float x, float y, float z)
	{
		this = new ActionTarget(new Vector3(x, y, z));
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x00110C94 File Offset: 0x0010EE94
	public ActionTarget(Vector3 pos)
	{
		this = new ActionTarget(ActionTargetType.Position, pos, BlockValueRef.None);
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x00110CA3 File Offset: 0x0010EEA3
	public ActionTarget(int x, int y, int z)
	{
		this = new ActionTarget(new BlockValueRef(x, y, z));
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x00110CB3 File Offset: 0x0010EEB3
	public ActionTarget(Vector3i pos)
	{
		this = new ActionTarget(new BlockValueRef(pos));
	}

	// Token: 0x06002B31 RID: 11057 RVA: 0x00110CC1 File Offset: 0x0010EEC1
	public ActionTarget(BlockValueRef bvRef)
	{
		this = new ActionTarget(ActionTargetType.BlockValueRef, Vector3.zero, bvRef);
	}

	// Token: 0x06002B32 RID: 11058 RVA: 0x00110CD0 File Offset: 0x0010EED0
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

	// Token: 0x06002B33 RID: 11059 RVA: 0x00110D24 File Offset: 0x0010EF24
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

	// Token: 0x06002B34 RID: 11060 RVA: 0x00110D78 File Offset: 0x0010EF78
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

	// Token: 0x06002B35 RID: 11061 RVA: 0x00110DD0 File Offset: 0x0010EFD0
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

	// Token: 0x06002B36 RID: 11062 RVA: 0x00110E12 File Offset: 0x0010F012
	public static implicit operator BlockValueRef(ActionTarget target)
	{
		if (target.Type == ActionTargetType.BlockValueRef)
		{
			return target.BlockValueReference;
		}
		Log.Warning(string.Format("[PROPS] ActionTarget implicitly converted to BlockValueRef, but failed: {0}", target));
		return BlockValueRef.None;
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x00110E3E File Offset: 0x0010F03E
	public static implicit operator ActionTarget(Vector3 pos)
	{
		return new ActionTarget(pos);
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x00110E46 File Offset: 0x0010F046
	public static implicit operator ActionTarget(Vector3i pos)
	{
		return new ActionTarget(new BlockValueRef(pos));
	}

	// Token: 0x06002B39 RID: 11065 RVA: 0x00110E53 File Offset: 0x0010F053
	public static implicit operator ActionTarget(PropRef propRef)
	{
		return new ActionTarget(new BlockValueRef(propRef));
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x00110E60 File Offset: 0x0010F060
	public static implicit operator ActionTarget(BlockValueRef bvRef)
	{
		return new ActionTarget(bvRef);
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x00110E68 File Offset: 0x0010F068
	public bool Equals(ActionTarget other)
	{
		return this.Type == other.Type && this.Position.Equals(other.Position) && this.BlockValueReference.Equals(other.BlockValueReference);
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x00110EA0 File Offset: 0x0010F0A0
	public override bool Equals(object obj)
	{
		if (obj is ActionTarget)
		{
			ActionTarget actionTarget = (ActionTarget)obj;
			return this.Equals(actionTarget);
		}
		return false;
	}

	// Token: 0x06002B3D RID: 11069 RVA: 0x00110EC5 File Offset: 0x0010F0C5
	public override int GetHashCode()
	{
		return HashCode.Combine<int, Vector3, BlockValueRef>((int)this.Type, this.Position, this.BlockValueReference);
	}

	// Token: 0x06002B3E RID: 11070 RVA: 0x00110EDE File Offset: 0x0010F0DE
	public static bool operator ==(ActionTarget left, ActionTarget right)
	{
		return left.Equals(right);
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x00110EE8 File Offset: 0x0010F0E8
	public static bool operator !=(ActionTarget left, ActionTarget right)
	{
		return !left.Equals(right);
	}

	// Token: 0x040020F6 RID: 8438
	public static readonly ActionTarget None = new ActionTarget(ActionTargetType.None, Vector3.zero, BlockValueRef.None);

	// Token: 0x040020F7 RID: 8439
	public ActionTargetType Type;

	// Token: 0x040020F8 RID: 8440
	public Vector3 Position;

	// Token: 0x040020F9 RID: 8441
	public BlockValueRef BlockValueReference;
}
