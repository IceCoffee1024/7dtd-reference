using System;
using System.Threading;

// Token: 0x020014A5 RID: 5285
public static class ReaderWriterLockSlimExtensions
{
	// Token: 0x0600A5AB RID: 42411 RVA: 0x003DB6D1 File Offset: 0x003D98D1
	public static ReaderWriterLockSlimExtensions.ReadScope ReadLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.ReadScope(lockSlim);
	}

	// Token: 0x0600A5AC RID: 42412 RVA: 0x003DB6D9 File Offset: 0x003D98D9
	public static ReaderWriterLockSlimExtensions.WriteScope WriteLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.WriteScope(lockSlim);
	}

	// Token: 0x0600A5AD RID: 42413 RVA: 0x003DB6E1 File Offset: 0x003D98E1
	public static ReaderWriterLockSlimExtensions.UpgradeableReadScope UpgradableReadLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.UpgradeableReadScope(lockSlim);
	}

	// Token: 0x020014A6 RID: 5286
	public readonly struct ReadScope : IDisposable
	{
		// Token: 0x0600A5AE RID: 42414 RVA: 0x003DB6E9 File Offset: 0x003D98E9
		public ReadScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterReadLock();
		}

		// Token: 0x0600A5AF RID: 42415 RVA: 0x003DB6FD File Offset: 0x003D98FD
		public void Dispose()
		{
			this.m_lockSlim.ExitReadLock();
		}

		// Token: 0x04007B85 RID: 31621
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}

	// Token: 0x020014A7 RID: 5287
	public readonly struct WriteScope : IDisposable
	{
		// Token: 0x0600A5B0 RID: 42416 RVA: 0x003DB70A File Offset: 0x003D990A
		public WriteScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterWriteLock();
		}

		// Token: 0x0600A5B1 RID: 42417 RVA: 0x003DB71E File Offset: 0x003D991E
		public void Dispose()
		{
			this.m_lockSlim.ExitWriteLock();
		}

		// Token: 0x04007B86 RID: 31622
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}

	// Token: 0x020014A8 RID: 5288
	public readonly struct UpgradeableReadScope : IDisposable
	{
		// Token: 0x0600A5B2 RID: 42418 RVA: 0x003DB72B File Offset: 0x003D992B
		public UpgradeableReadScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterUpgradeableReadLock();
		}

		// Token: 0x0600A5B3 RID: 42419 RVA: 0x003DB73F File Offset: 0x003D993F
		public void Dispose()
		{
			this.m_lockSlim.ExitUpgradeableReadLock();
		}

		// Token: 0x04007B87 RID: 31623
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}
}
