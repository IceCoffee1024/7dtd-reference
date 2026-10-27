using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013FF RID: 5119
public abstract class DebugWrapper
{
	// Token: 0x0600A05B RID: 41051 RVA: 0x003C7D2C File Offset: 0x003C5F2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DebugWrapper(DebugWrapper parent)
	{
		this.m_readCounter = ((parent != null) ? parent.m_readCounter : null) ?? new AtomicCounter();
		this.m_writeCounter = ((parent != null) ? parent.m_writeCounter : null) ?? new AtomicCounter();
		this.DebugNonMainThreadAccess = parent != null && parent.DebugNonMainThreadAccess;
		this.DebugConcurrentModifications = parent != null && parent.DebugConcurrentModifications;
	}

	// Token: 0x17001320 RID: 4896
	// (get) Token: 0x0600A05C RID: 41052 RVA: 0x003C7D99 File Offset: 0x003C5F99
	// (set) Token: 0x0600A05D RID: 41053 RVA: 0x003C7DA1 File Offset: 0x003C5FA1
	public bool DebugNonMainThreadAccess { get; set; }

	// Token: 0x17001321 RID: 4897
	// (get) Token: 0x0600A05E RID: 41054 RVA: 0x003C7DAA File Offset: 0x003C5FAA
	// (set) Token: 0x0600A05F RID: 41055 RVA: 0x003C7DB2 File Offset: 0x003C5FB2
	public bool DebugConcurrentModifications { get; set; }

	// Token: 0x17001322 RID: 4898
	// (get) Token: 0x0600A060 RID: 41056 RVA: 0x003C7DBB File Offset: 0x003C5FBB
	public bool NeedsScope
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.DebugConcurrentModifications;
		}
	}

	// Token: 0x0600A061 RID: 41057 RVA: 0x003C7DC3 File Offset: 0x003C5FC3
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

	// Token: 0x0600A062 RID: 41058 RVA: 0x003C7DF7 File Offset: 0x003C5FF7
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

	// Token: 0x0600A063 RID: 41059 RVA: 0x003C7E2B File Offset: 0x003C602B
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator<T> DebugEnumerator<T>(IEnumerator<T> enumerator)
	{
		return new DebugWrapper.Enumerator<T>(this.DebugReadScope(), enumerator);
	}

	// Token: 0x0600A064 RID: 41060 RVA: 0x003C7E39 File Offset: 0x003C6039
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator DebugEnumerator(IEnumerator enumerator)
	{
		return new DebugWrapper.Enumerator(this.DebugReadScope(), enumerator);
	}

	// Token: 0x04007980 RID: 31104
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AtomicCounter m_readCounter;

	// Token: 0x04007981 RID: 31105
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AtomicCounter m_writeCounter;

	// Token: 0x02001400 RID: 5120
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DummyScope : IDisposable
	{
		// Token: 0x0600A065 RID: 41061 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public DummyScope()
		{
		}

		// Token: 0x0600A066 RID: 41062 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x04007984 RID: 31108
		public static readonly DebugWrapper.DummyScope Instance = new DebugWrapper.DummyScope();
	}

	// Token: 0x02001401 RID: 5121
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ReadScope : IDisposable
	{
		// Token: 0x0600A068 RID: 41064 RVA: 0x003C7E54 File Offset: 0x003C6054
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

		// Token: 0x0600A069 RID: 41065 RVA: 0x003C7EB8 File Offset: 0x003C60B8
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~ReadScope()
		{
			this.Dispose(false);
		}

		// Token: 0x0600A06A RID: 41066 RVA: 0x003C7EE8 File Offset: 0x003C60E8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600A06B RID: 41067 RVA: 0x003C7EF8 File Offset: 0x003C60F8
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

		// Token: 0x04007985 RID: 31109
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DebugWrapper m_wrapper;

		// Token: 0x04007986 RID: 31110
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_concurrentModificationNotified;

		// Token: 0x04007987 RID: 31111
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}

	// Token: 0x02001402 RID: 5122
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ReadWriteScope : IDisposable
	{
		// Token: 0x0600A06C RID: 41068 RVA: 0x003C7F6C File Offset: 0x003C616C
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

		// Token: 0x0600A06D RID: 41069 RVA: 0x003C8018 File Offset: 0x003C6218
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~ReadWriteScope()
		{
			this.Dispose(false);
		}

		// Token: 0x0600A06E RID: 41070 RVA: 0x003C8048 File Offset: 0x003C6248
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600A06F RID: 41071 RVA: 0x003C8058 File Offset: 0x003C6258
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

		// Token: 0x04007988 RID: 31112
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DebugWrapper m_wrapper;

		// Token: 0x04007989 RID: 31113
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_concurrentModificationNotified;

		// Token: 0x0400798A RID: 31114
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}

	// Token: 0x02001403 RID: 5123
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Enumerator : IEnumerator, IDisposable
	{
		// Token: 0x0600A070 RID: 41072 RVA: 0x003C80FE File Offset: 0x003C62FE
		public Enumerator(IDisposable scope, IEnumerator enumerator)
		{
			this.m_scope = scope;
			this.m_enumerator = enumerator;
		}

		// Token: 0x0600A071 RID: 41073 RVA: 0x003C8114 File Offset: 0x003C6314
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

		// Token: 0x0600A072 RID: 41074 RVA: 0x003C8136 File Offset: 0x003C6336
		public bool MoveNext()
		{
			return this.m_enumerator.MoveNext();
		}

		// Token: 0x0600A073 RID: 41075 RVA: 0x003C8143 File Offset: 0x003C6343
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x0600A074 RID: 41076 RVA: 0x003C8150 File Offset: 0x003C6350
		public object Current
		{
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x0400798B RID: 31115
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IDisposable m_scope;

		// Token: 0x0400798C RID: 31116
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator m_enumerator;
	}

	// Token: 0x02001404 RID: 5124
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Enumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
	{
		// Token: 0x0600A075 RID: 41077 RVA: 0x003C815D File Offset: 0x003C635D
		public Enumerator(IDisposable scope, IEnumerator<T> enumerator)
		{
			this.m_scope = scope;
			this.m_enumerator = enumerator;
		}

		// Token: 0x0600A076 RID: 41078 RVA: 0x003C8173 File Offset: 0x003C6373
		public void Dispose()
		{
			this.m_scope.Dispose();
			this.m_enumerator.Dispose();
		}

		// Token: 0x0600A077 RID: 41079 RVA: 0x003C818B File Offset: 0x003C638B
		public bool MoveNext()
		{
			return this.m_enumerator.MoveNext();
		}

		// Token: 0x0600A078 RID: 41080 RVA: 0x003C8198 File Offset: 0x003C6398
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x0600A079 RID: 41081 RVA: 0x003C81A5 File Offset: 0x003C63A5
		public T Current
		{
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x0600A07A RID: 41082 RVA: 0x003C81B2 File Offset: 0x003C63B2
		public object System.Collections.IEnumerator.Current
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x0400798D RID: 31117
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IDisposable m_scope;

		// Token: 0x0400798E RID: 31118
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator<T> m_enumerator;
	}
}
