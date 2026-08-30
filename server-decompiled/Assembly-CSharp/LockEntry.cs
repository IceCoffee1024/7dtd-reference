using System;

// Token: 0x02000581 RID: 1409
public readonly struct LockEntry : IEquatable<LockEntry>
{
	// Token: 0x06002E19 RID: 11801 RVA: 0x00124C05 File Offset: 0x00122E05
	public LockEntry(ILockTarget target, ushort channel = 0)
	{
		this.Target = target;
		this.Channel = channel;
	}

	// Token: 0x06002E1A RID: 11802 RVA: 0x00124C15 File Offset: 0x00122E15
	public bool Equals(LockEntry other)
	{
		return this.Target == other.Target && this.Channel == other.Channel;
	}

	// Token: 0x06002E1B RID: 11803 RVA: 0x00124C35 File Offset: 0x00122E35
	public override int GetHashCode()
	{
		ILockTarget target = this.Target;
		return HashCode.Combine<int, ushort>((target != null) ? target.GetHashCode() : 0, this.Channel);
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x00124C54 File Offset: 0x00122E54
	public override bool Equals(object obj)
	{
		if (obj is LockEntry)
		{
			LockEntry lockEntry = (LockEntry)obj;
			return this.Equals(lockEntry);
		}
		return false;
	}

	// Token: 0x06002E1D RID: 11805 RVA: 0x00124C79 File Offset: 0x00122E79
	public static bool operator ==(LockEntry left, LockEntry right)
	{
		return left.Equals(right);
	}

	// Token: 0x06002E1E RID: 11806 RVA: 0x00124C83 File Offset: 0x00122E83
	public static bool operator !=(LockEntry left, LockEntry right)
	{
		return !left.Equals(right);
	}

	// Token: 0x040023BA RID: 9146
	public readonly ILockTarget Target;

	// Token: 0x040023BB RID: 9147
	public readonly ushort Channel;
}
