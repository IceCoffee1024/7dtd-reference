using System;

// Token: 0x02000065 RID: 101
public class LastAccessHelper
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060001FB RID: 507 RVA: 0x000112B8 File Offset: 0x0000F4B8
	public DateTime Time
	{
		get
		{
			object @lock = this.m_lock;
			DateTime dateTime;
			lock (@lock)
			{
				dateTime = ((this.m_numActive > 0) ? DateTime.Now : this.m_time);
			}
			return dateTime;
		}
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0001130C File Offset: 0x0000F50C
	public LastAccessHelper.Scope CreateScope()
	{
		return new LastAccessHelper.Scope(this);
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00011314 File Offset: 0x0000F514
	[PublicizedFrom(EAccessModifier.Private)]
	public void Increment()
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			this.m_numActive++;
		}
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0001135C File Offset: 0x0000F55C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Decrement()
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			this.m_numActive--;
			if (this.m_numActive == 0)
			{
				this.m_time = DateTime.Now;
			}
		}
	}

	// Token: 0x040002A9 RID: 681
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_lock = new object();

	// Token: 0x040002AA RID: 682
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_numActive;

	// Token: 0x040002AB RID: 683
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime m_time = DateTime.Now;

	// Token: 0x02000066 RID: 102
	public struct Scope : IDisposable
	{
		// Token: 0x06000200 RID: 512 RVA: 0x000113DA File Offset: 0x0000F5DA
		public Scope(LastAccessHelper parent)
		{
			this.m_parent = parent;
			this.m_parent.Increment();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000113EE File Offset: 0x0000F5EE
		public void Dispose()
		{
			LastAccessHelper parent = this.m_parent;
			if (parent != null)
			{
				parent.Decrement();
			}
			this.m_parent = null;
		}

		// Token: 0x040002AC RID: 684
		[PublicizedFrom(EAccessModifier.Private)]
		public LastAccessHelper m_parent;
	}
}
