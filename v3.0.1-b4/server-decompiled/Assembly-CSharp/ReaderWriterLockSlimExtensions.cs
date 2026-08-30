using System;
using System.Threading;

// Token: 0x0200146A RID: 5226
public static class ReaderWriterLockSlimExtensions
{
	// Token: 0x0600A400 RID: 41984 RVA: 0x003D1701 File Offset: 0x003CF901
	public static ReaderWriterLockSlimExtensions.ReadScope ReadLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.ReadScope(lockSlim);
	}

	// Token: 0x0600A401 RID: 41985 RVA: 0x003D1709 File Offset: 0x003CF909
	public static ReaderWriterLockSlimExtensions.WriteScope WriteLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.WriteScope(lockSlim);
	}

	// Token: 0x0600A402 RID: 41986 RVA: 0x003D1711 File Offset: 0x003CF911
	public static ReaderWriterLockSlimExtensions.UpgradeableReadScope UpgradableReadLockScope(this ReaderWriterLockSlim lockSlim)
	{
		return new ReaderWriterLockSlimExtensions.UpgradeableReadScope(lockSlim);
	}

	// Token: 0x0200146B RID: 5227
	public readonly struct ReadScope : IDisposable
	{
		// Token: 0x0600A403 RID: 41987 RVA: 0x003D1719 File Offset: 0x003CF919
		public ReadScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterReadLock();
		}

		// Token: 0x0600A404 RID: 41988 RVA: 0x003D172D File Offset: 0x003CF92D
		public void Dispose()
		{
			this.m_lockSlim.ExitReadLock();
		}

		// Token: 0x04007A31 RID: 31281
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}

	// Token: 0x0200146C RID: 5228
	public readonly struct WriteScope : IDisposable
	{
		// Token: 0x0600A405 RID: 41989 RVA: 0x003D173A File Offset: 0x003CF93A
		public WriteScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterWriteLock();
		}

		// Token: 0x0600A406 RID: 41990 RVA: 0x003D174E File Offset: 0x003CF94E
		public void Dispose()
		{
			this.m_lockSlim.ExitWriteLock();
		}

		// Token: 0x04007A32 RID: 31282
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}

	// Token: 0x0200146D RID: 5229
	public readonly struct UpgradeableReadScope : IDisposable
	{
		// Token: 0x0600A407 RID: 41991 RVA: 0x003D175B File Offset: 0x003CF95B
		public UpgradeableReadScope(ReaderWriterLockSlim lockSlim)
		{
			this.m_lockSlim = lockSlim;
			this.m_lockSlim.EnterUpgradeableReadLock();
		}

		// Token: 0x0600A408 RID: 41992 RVA: 0x003D176F File Offset: 0x003CF96F
		public void Dispose()
		{
			this.m_lockSlim.ExitUpgradeableReadLock();
		}

		// Token: 0x04007A33 RID: 31283
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReaderWriterLockSlim m_lockSlim;
	}
}
