using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013B2 RID: 5042
public abstract class DebugWrapper
{
	// Token: 0x06009E5B RID: 40539 RVA: 0x003BBA48 File Offset: 0x003B9C48
	[PublicizedFrom(EAccessModifier.Protected)]
	public DebugWrapper(DebugWrapper parent)
	{
		this.m_readCounter = ((parent != null) ? parent.m_readCounter : null) ?? new AtomicCounter();
		this.m_writeCounter = ((parent != null) ? parent.m_writeCounter : null) ?? new AtomicCounter();
		this.DebugNonMainThreadAccess = parent != null && parent.DebugNonMainThreadAccess;
		this.DebugConcurrentModifications = parent != null && parent.DebugConcurrentModifications;
	}

	// Token: 0x17001296 RID: 4758
	// (get) Token: 0x06009E5C RID: 40540 RVA: 0x003BBAB5 File Offset: 0x003B9CB5
	// (set) Token: 0x06009E5D RID: 40541 RVA: 0x003BBABD File Offset: 0x003B9CBD
	public bool DebugNonMainThreadAccess { get; set; }

	// Token: 0x17001297 RID: 4759
	// (get) Token: 0x06009E5E RID: 40542 RVA: 0x003BBAC6 File Offset: 0x003B9CC6
	// (set) Token: 0x06009E5F RID: 40543 RVA: 0x003BBACE File Offset: 0x003B9CCE
	public bool DebugConcurrentModifications { get; set; }

	// Token: 0x17001298 RID: 4760
	// (get) Token: 0x06009E60 RID: 40544 RVA: 0x003BBAD7 File Offset: 0x003B9CD7
	public bool NeedsScope
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.DebugConcurrentModifications;
		}
	}

	// Token: 0x06009E61 RID: 40545 RVA: 0x003BBADF File Offset: 0x003B9CDF
	[PublicizedFrom(EAccessModifier.Protected)]
	public IDisposable DebugReadScope()
	{
		if (this.DebugNonMainThreadAccess && !ThreadManager.IsMainThread())
		{
			Log.Exception(new DebugWrapperException("A read is being issued outside of the main thread."));
		}
		if (!this.NeedsScope)
		{
			return DebugWrapper.DummyScope.Instance;
		}
		return new DebugWrapper.ReadScope(this);
	}

	// Token: 0x06009E62 RID: 40546 RVA: 0x003BBB13 File Offset: 0x003B9D13
	[PublicizedFrom(EAccessModifier.Protected)]
	public IDisposable DebugReadWriteScope()
	{
		if (this.DebugNonMainThreadAccess && !ThreadManager.IsMainThread())
		{
			Log.Exception(new DebugWrapperException("A read/write is being issued outside of the main thread."));
		}
		if (!this.NeedsScope)
		{
			return DebugWrapper.DummyScope.Instance;
		}
		return new DebugWrapper.ReadWriteScope(this);
	}

	// Token: 0x06009E63 RID: 40547 RVA: 0x003BBB47 File Offset: 0x003B9D47
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator<T> DebugEnumerator<T>(IEnumerator<T> enumerator)
	{
		return new DebugWrapper.Enumerator<T>(this.DebugReadScope(), enumerator);
	}

	// Token: 0x06009E64 RID: 40548 RVA: 0x003BBB55 File Offset: 0x003B9D55
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator DebugEnumerator(IEnumerator enumerator)
	{
		return new DebugWrapper.Enumerator(this.DebugReadScope(), enumerator);
	}

	// Token: 0x040077EB RID: 30699
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AtomicCounter m_readCounter;

	// Token: 0x040077EC RID: 30700
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AtomicCounter m_writeCounter;

	// Token: 0x020013B3 RID: 5043
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DummyScope : IDisposable
	{
		// Token: 0x06009E65 RID: 40549 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public DummyScope()
		{
		}

		// Token: 0x06009E66 RID: 40550 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x040077EF RID: 30703
		public static readonly DebugWrapper.DummyScope Instance = new DebugWrapper.DummyScope();
	}

	// Token: 0x020013B4 RID: 5044
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ReadScope : IDisposable
	{
		// Token: 0x06009E68 RID: 40552 RVA: 0x003BBB70 File Offset: 0x003B9D70
		public ReadScope(DebugWrapper wrapper)
		{
			this.m_wrapper = wrapper;
			if (this.m_wrapper.DebugConcurrentModifications && this.m_wrapper.m_writeCounter.Value > 0)
			{
				Log.Exception(new DebugWrapperException("A read is being issued while there are active writers."));
				this.m_concurrentModificationNotified = true;
			}
			this.m_wrapper.m_readCounter.Increment();
		}

		// Token: 0x06009E69 RID: 40553 RVA: 0x003BBBD4 File Offset: 0x003B9DD4
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~ReadScope()
		{
			this.Dispose(false);
		}

		// Token: 0x06009E6A RID: 40554 RVA: 0x003BBC04 File Offset: 0x003B9E04
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009E6B RID: 40555 RVA: 0x003BBC14 File Offset: 0x003B9E14
		[PublicizedFrom(EAccessModifier.Private)]
		public void Dispose(bool disposing)
		{
			if (this.m_disposed)
			{
				return;
			}
			this.m_disposed = true;
			if (!disposing)
			{
				Log.Error("DebugWrapper.ReadScope was not disposed of correctly.");
			}
			this.m_wrapper.m_readCounter.Decrement();
			if (!this.m_concurrentModificationNotified && this.m_wrapper.DebugConcurrentModifications && this.m_wrapper.m_writeCounter.Value > 0)
			{
				Log.Exception(new DebugWrapperException("A read was issued while there were active writers."));
			}
		}

		// Token: 0x040077F0 RID: 30704
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DebugWrapper m_wrapper;

		// Token: 0x040077F1 RID: 30705
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_concurrentModificationNotified;

		// Token: 0x040077F2 RID: 30706
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}

	// Token: 0x020013B5 RID: 5045
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ReadWriteScope : IDisposable
	{
		// Token: 0x06009E6C RID: 40556 RVA: 0x003BBC88 File Offset: 0x003B9E88
		public ReadWriteScope(DebugWrapper wrapper)
		{
			this.m_wrapper = wrapper;
			if (this.m_wrapper.DebugConcurrentModifications && this.m_wrapper.m_writeCounter.Value > 0)
			{
				if (this.m_wrapper.m_writeCounter.Value > 0)
				{
					Log.Exception(new DebugWrapperException("A read/write is being issued while there are active writers."));
				}
				else if (this.m_wrapper.m_readCounter.Value > 0)
				{
					Log.Exception(new DebugWrapperException("A read/write is being issued while there are active readers."));
				}
				this.m_concurrentModificationNotified = true;
			}
			this.m_wrapper.m_readCounter.Increment();
			this.m_wrapper.m_writeCounter.Increment();
		}

		// Token: 0x06009E6D RID: 40557 RVA: 0x003BBD34 File Offset: 0x003B9F34
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~ReadWriteScope()
		{
			this.Dispose(false);
		}

		// Token: 0x06009E6E RID: 40558 RVA: 0x003BBD64 File Offset: 0x003B9F64
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009E6F RID: 40559 RVA: 0x003BBD74 File Offset: 0x003B9F74
		[PublicizedFrom(EAccessModifier.Private)]
		public void Dispose(bool disposing)
		{
			if (this.m_disposed)
			{
				return;
			}
			this.m_disposed = true;
			if (!disposing)
			{
				Log.Error("DebugWrapper.ReadWriteScope was not disposed of correctly.");
			}
			this.m_wrapper.m_writeCounter.Decrement();
			this.m_wrapper.m_readCounter.Decrement();
			if (!this.m_concurrentModificationNotified && this.m_wrapper.DebugConcurrentModifications)
			{
				if (this.m_wrapper.m_writeCounter.Value > 0)
				{
					Log.Exception(new DebugWrapperException("A read/write was issued while there were active writers."));
					return;
				}
				if (this.m_wrapper.m_readCounter.Value > 0)
				{
					Log.Exception(new DebugWrapperException("A read/write was issued while there are active readers."));
				}
			}
		}

		// Token: 0x040077F3 RID: 30707
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DebugWrapper m_wrapper;

		// Token: 0x040077F4 RID: 30708
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_concurrentModificationNotified;

		// Token: 0x040077F5 RID: 30709
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}

	// Token: 0x020013B6 RID: 5046
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Enumerator : IEnumerator, IDisposable
	{
		// Token: 0x06009E70 RID: 40560 RVA: 0x003BBE1A File Offset: 0x003BA01A
		public Enumerator(IDisposable scope, IEnumerator enumerator)
		{
			this.m_scope = scope;
			this.m_enumerator = enumerator;
		}

		// Token: 0x06009E71 RID: 40561 RVA: 0x003BBE30 File Offset: 0x003BA030
		public void Dispose()
		{
			this.m_scope.Dispose();
			IDisposable disposable = this.m_enumerator as IDisposable;
			if (disposable == null)
			{
				return;
			}
			disposable.Dispose();
		}

		// Token: 0x06009E72 RID: 40562 RVA: 0x003BBE52 File Offset: 0x003BA052
		public bool MoveNext()
		{
			return this.m_enumerator.MoveNext();
		}

		// Token: 0x06009E73 RID: 40563 RVA: 0x003BBE5F File Offset: 0x003BA05F
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06009E74 RID: 40564 RVA: 0x003BBE6C File Offset: 0x003BA06C
		public object Current
		{
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x040077F6 RID: 30710
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IDisposable m_scope;

		// Token: 0x040077F7 RID: 30711
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator m_enumerator;
	}

	// Token: 0x020013B7 RID: 5047
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Enumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
	{
		// Token: 0x06009E75 RID: 40565 RVA: 0x003BBE79 File Offset: 0x003BA079
		public Enumerator(IDisposable scope, IEnumerator<T> enumerator)
		{
			this.m_scope = scope;
			this.m_enumerator = enumerator;
		}

		// Token: 0x06009E76 RID: 40566 RVA: 0x003BBE8F File Offset: 0x003BA08F
		public void Dispose()
		{
			this.m_scope.Dispose();
			this.m_enumerator.Dispose();
		}

		// Token: 0x06009E77 RID: 40567 RVA: 0x003BBEA7 File Offset: 0x003BA0A7
		public bool MoveNext()
		{
			return this.m_enumerator.MoveNext();
		}

		// Token: 0x06009E78 RID: 40568 RVA: 0x003BBEB4 File Offset: 0x003BA0B4
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06009E79 RID: 40569 RVA: 0x003BBEC1 File Offset: 0x003BA0C1
		public T Current
		{
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06009E7A RID: 40570 RVA: 0x003BBECE File Offset: 0x003BA0CE
		public object System.Collections.IEnumerator.Current
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x040077F8 RID: 30712
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IDisposable m_scope;

		// Token: 0x040077F9 RID: 30713
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator<T> m_enumerator;
	}
}
