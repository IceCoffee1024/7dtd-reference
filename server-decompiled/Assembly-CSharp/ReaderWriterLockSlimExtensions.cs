using System;
using System.Threading;

// Token: 0x020014B7 RID: 5303
public static class ReaderWriterLockSlimExtensions
{
	// Token: 0x0600A603 RID: 42499 RVA: 0x003DDAD5 File Offset: 0x003DBCD5
	public static ReaderWriterLockSlimExtensions.ReadScope ReadLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.ReadScope(lockSlim);
	}

	// Token: 0x0600A604 RID: 42500 RVA: 0x003DDADD File Offset: 0x003DBCDD
	public static ReaderWriterLockSlimExtensions.WriteScope WriteLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.WriteScope(lockSlim);
	}

	// Token: 0x0600A605 RID: 42501 RVA: 0x003DDAE5 File Offset: 0x003DBCE5
	public static ReaderWriterLockSlimExtensions.UpgradeableReadScope UpgradableReadLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.UpgradeableReadScope(lockSlim);
	}

	// Token: 0x020014B8 RID: 5304
	public readonly struct ReadScope : IDisposable
	{
		// Token: 0x0600A606 RID: 42502 RVA: 0x003DDAED File Offset: 0x003DBCED
		public ReadScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterReadLock();
		}

		// Token: 0x0600A607 RID: 42503 RVA: 0x003DDB01 File Offset: 0x003DBD01
		public void Dispose()
		{
			this.m_lockSlim.ExitReadLock();
		}

		// Token: 0x04007BC6 RID: 31686
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}

	// Token: 0x020014B9 RID: 5305
	public readonly struct WriteScope : IDisposable
	{
		// Token: 0x0600A608 RID: 42504 RVA: 0x003DDB0E File Offset: 0x003DBD0E
		public WriteScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterWriteLock();
		}

		// Token: 0x0600A609 RID: 42505 RVA: 0x003DDB22 File Offset: 0x003DBD22
		public void Dispose()
		{
			this.m_lockSlim.ExitWriteLock();
		}

		// Token: 0x04007BC7 RID: 31687
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}

	// Token: 0x020014BA RID: 5306
	public readonly struct UpgradeableReadScope : IDisposable
	{
		// Token: 0x0600A60A RID: 42506 RVA: 0x003DDB2F File Offset: 0x003DBD2F
		public UpgradeableReadScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterUpgradeableReadLock();
		}

		// Token: 0x0600A60B RID: 42507 RVA: 0x003DDB43 File Offset: 0x003DBD43
		public void Dispose()
		{
			this.m_lockSlim.ExitUpgradeableReadLock();
		}

		// Token: 0x04007BC8 RID: 31688
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}
}
