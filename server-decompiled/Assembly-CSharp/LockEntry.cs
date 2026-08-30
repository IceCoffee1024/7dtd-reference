using System;

// Token: 0x02000586 RID: 1414
public readonly struct LockEntry : IEquatable<LockEntry>
{
	// Token: 0x06002E47 RID: 11847 RVA: 0x00125CF5 File Offset: 0x00123EF5
	public LockEntry(ILockTarget target, ushort channel = 0)
	{
		this.Target = target;
		this.Channel = channel;
	}

	// Token: 0x06002E48 RID: 11848 RVA: 0x00125D05 File Offset: 0x00123F05
	public bool Equals(LockEntry other)
	{
		return this.Target == other.Target && this.Channel == other.Channel;
	}

	// Token: 0x06002E49 RID: 11849 RVA: 0x00125D25 File Offset: 0x00123F25
	public override int GetHashCode()
	{
		ILockTarget target = this.Target;
		return HashCode.Combine<int, ushort>((target != null) ? target.GetHashCode() : 0, this.Channel);
	}

	// Token: 0x06002E4A RID: 11850 RVA: 0x00125D44 File Offset: 0x00123F44
	public override bool Equals(object obj)
	{
		if (obj is LockEntry)
		{
			LockEntry lockEntry = (LockEntry)obj;
			return this.Equals(lockEntry);
		}
		return false;
	}

	// Token: 0x06002E4B RID: 11851 RVA: 0x00125D69 File Offset: 0x00123F69
	public static bool operator ==(LockEntry left, LockEntry right)
	{
		return left.Equals(right);
	}

	// Token: 0x06002E4C RID: 11852 RVA: 0x00125D73 File Offset: 0x00123F73
	public static bool operator !=(LockEntry left, LockEntry right)
	{
		return !left.Equals(right);
	}

	// Token: 0x040023EE RID: 9198
	public readonly ILockTarget Target;

	// Token: 0x040023EF RID: 9199
	public readonly ushort Channel;
}
