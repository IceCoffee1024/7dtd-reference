using System;
using System.Threading;

// Token: 0x02001485 RID: 5253
public static class ReaderWriterLockSlimExtensions
{
	// Token: 0x0600A4FD RID: 42237 RVA: 0x003D6A45 File Offset: 0x003D4C45
	public static ReaderWriterLockSlimExtensions.ReadScope ReadLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.ReadScope(lockSlim);
	}

	// Token: 0x0600A4FE RID: 42238 RVA: 0x003D6A4D File Offset: 0x003D4C4D
	public static ReaderWriterLockSlimExtensions.WriteScope WriteLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.WriteScope(lockSlim);
	}

	// Token: 0x0600A4FF RID: 42239 RVA: 0x003D6A55 File Offset: 0x003D4C55
	public static ReaderWriterLockSlimExtensions.UpgradeableReadScope UpgradableReadLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.UpgradeableReadScope(lockSlim);
	}

	// Token: 0x02001486 RID: 5254
	public readonly struct ReadScope : IDisposable
	{
		// Token: 0x0600A500 RID: 42240 RVA: 0x003D6A5D File Offset: 0x003D4C5D
		public ReadScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterReadLock();
		}

		// Token: 0x0600A501 RID: 42241 RVA: 0x003D6A71 File Offset: 0x003D4C71
		public void Dispose()
		{
			this.m_lockSlim.ExitReadLock();
		}

		// Token: 0x04007B0A RID: 31498
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}

	// Token: 0x02001487 RID: 5255
	public readonly struct WriteScope : IDisposable
	{
		// Token: 0x0600A502 RID: 42242 RVA: 0x003D6A7E File Offset: 0x003D4C7E
		public WriteScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterWriteLock();
		}

		// Token: 0x0600A503 RID: 42243 RVA: 0x003D6A92 File Offset: 0x003D4C92
		public void Dispose()
		{
			this.m_lockSlim.ExitWriteLock();
		}

		// Token: 0x04007B0B RID: 31499
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}

	// Token: 0x02001488 RID: 5256
	public readonly struct UpgradeableReadScope : IDisposable
	{
		// Token: 0x0600A504 RID: 42244 RVA: 0x003D6A9F File Offset: 0x003D4C9F
		public UpgradeableReadScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterUpgradeableReadLock();
		}

		// Token: 0x0600A505 RID: 42245 RVA: 0x003D6AB3 File Offset: 0x003D4CB3
		public void Dispose()
		{
			this.m_lockSlim.ExitUpgradeableReadLock();
		}

		// Token: 0x04007B0C RID: 31500
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}
}
