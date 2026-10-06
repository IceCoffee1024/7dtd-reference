using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013ED RID: 5101
public abstract class DebugWrapper
{
	// Token: 0x0600A003 RID: 40963 RVA: 0x003C5928 File Offset: 0x003C3B28
	[PublicizedFrom(EAccessModifier.Protected)]
	public DebugWrapper(DebugWrapper parent)
	{
		this.m_readCounter = ((parent != null) ? parent.m_readCounter : null) ?? new AtomicCounter();
		this.m_writeCounter = ((parent != null) ? parent.m_writeCounter : null) ?? new AtomicCounter();
		this.DebugNonMainThreadAccess = parent != null && parent.DebugNonMainThreadAccess;
		this.DebugConcurrentModifications = parent != null && parent.DebugConcurrentModifications;
	}

	// Token: 0x17001314 RID: 4884
	// (get) Token: 0x0600A004 RID: 40964 RVA: 0x003C5995 File Offset: 0x003C3B95
	// (set) Token: 0x0600A005 RID: 40965 RVA: 0x003C599D File Offset: 0x003C3B9D
	public bool DebugNonMainThreadAccess { get; set; }

	// Token: 0x17001315 RID: 4885
	// (get) Token: 0x0600A006 RID: 40966 RVA: 0x003C59A6 File Offset: 0x003C3BA6
	// (set) Token: 0x0600A007 RID: 40967 RVA: 0x003C59AE File Offset: 0x003C3BAE
	public bool DebugConcurrentModifications { get; set; }

	// Token: 0x17001316 RID: 4886
	// (get) Token: 0x0600A008 RID: 40968 RVA: 0x003C59B7 File Offset: 0x003C3BB7
	public bool NeedsScope
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.DebugConcurrentModifications;
		}
	}

	// Token: 0x0600A009 RID: 40969 RVA: 0x003C59BF File Offset: 0x003C3BBF
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

	// Token: 0x0600A00A RID: 40970 RVA: 0x003C59F3 File Offset: 0x003C3BF3
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

	// Token: 0x0600A00B RID: 40971 RVA: 0x003C5A27 File Offset: 0x003C3C27
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator<T> DebugEnumerator<T>(IEnumerator<T> enumerator)
	{
		return new DebugWrapper.Enumerator<T>(this.DebugReadScope(), enumerator);
	}

	// Token: 0x0600A00C RID: 40972 RVA: 0x003C5A35 File Offset: 0x003C3C35
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator DebugEnumerator(IEnumerator enumerator)
	{
		return new DebugWrapper.Enumerator(this.DebugReadScope(), enumerator);
	}

	// Token: 0x0400793F RID: 31039
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AtomicCounter m_readCounter;

	// Token: 0x04007940 RID: 31040
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AtomicCounter m_writeCounter;

	// Token: 0x020013EE RID: 5102
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DummyScope : IDisposable
	{
		// Token: 0x0600A00D RID: 40973 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public DummyScope()
		{
		}

		// Token: 0x0600A00E RID: 40974 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x04007943 RID: 31043
		public static readonly DebugWrapper.DummyScope Instance = new DebugWrapper.DummyScope();
	}

	// Token: 0x020013EF RID: 5103
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ReadScope : IDisposable
	{
		// Token: 0x0600A010 RID: 40976 RVA: 0x003C5A50 File Offset: 0x003C3C50
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

		// Token: 0x0600A011 RID: 40977 RVA: 0x003C5AB4 File Offset: 0x003C3CB4
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~ReadScope()
		{
			this.Dispose(false);
		}

		// Token: 0x0600A012 RID: 40978 RVA: 0x003C5AE4 File Offset: 0x003C3CE4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600A013 RID: 40979 RVA: 0x003C5AF4 File Offset: 0x003C3CF4
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

		// Token: 0x04007944 RID: 31044
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DebugWrapper m_wrapper;

		// Token: 0x04007945 RID: 31045
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_concurrentModificationNotified;

		// Token: 0x04007946 RID: 31046
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}

	// Token: 0x020013F0 RID: 5104
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ReadWriteScope : IDisposable
	{
		// Token: 0x0600A014 RID: 40980 RVA: 0x003C5B68 File Offset: 0x003C3D68
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

		// Token: 0x0600A015 RID: 40981 RVA: 0x003C5C14 File Offset: 0x003C3E14
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~ReadWriteScope()
		{
			this.Dispose(false);
		}

		// Token: 0x0600A016 RID: 40982 RVA: 0x003C5C44 File Offset: 0x003C3E44
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600A017 RID: 40983 RVA: 0x003C5C54 File Offset: 0x003C3E54
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

		// Token: 0x04007947 RID: 31047
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DebugWrapper m_wrapper;

		// Token: 0x04007948 RID: 31048
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_concurrentModificationNotified;

		// Token: 0x04007949 RID: 31049
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}

	// Token: 0x020013F1 RID: 5105
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Enumerator : IEnumerator, IDisposable
	{
		// Token: 0x0600A018 RID: 40984 RVA: 0x003C5CFA File Offset: 0x003C3EFA
		public Enumerator(IDisposable scope, IEnumerator enumerator)
		{
			this.m_scope = scope;
			this.m_enumerator = enumerator;
		}

		// Token: 0x0600A019 RID: 40985 RVA: 0x003C5D10 File Offset: 0x003C3F10
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

		// Token: 0x0600A01A RID: 40986 RVA: 0x003C5D32 File Offset: 0x003C3F32
		public bool MoveNext()
		{
			return this.m_enumerator.MoveNext();
		}

		// Token: 0x0600A01B RID: 40987 RVA: 0x003C5D3F File Offset: 0x003C3F3F
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x0600A01C RID: 40988 RVA: 0x003C5D4C File Offset: 0x003C3F4C
		public object Current
		{
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x0400794A RID: 31050
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IDisposable m_scope;

		// Token: 0x0400794B RID: 31051
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator m_enumerator;
	}

	// Token: 0x020013F2 RID: 5106
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Enumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
	{
		// Token: 0x0600A01D RID: 40989 RVA: 0x003C5D59 File Offset: 0x003C3F59
		public Enumerator(IDisposable scope, IEnumerator<T> enumerator)
		{
			this.m_scope = scope;
			this.m_enumerator = enumerator;
		}

		// Token: 0x0600A01E RID: 40990 RVA: 0x003C5D6F File Offset: 0x003C3F6F
		public void Dispose()
		{
			this.m_scope.Dispose();
			this.m_enumerator.Dispose();
		}

		// Token: 0x0600A01F RID: 40991 RVA: 0x003C5D87 File Offset: 0x003C3F87
		public bool MoveNext()
		{
			return this.m_enumerator.MoveNext();
		}

		// Token: 0x0600A020 RID: 40992 RVA: 0x003C5D94 File Offset: 0x003C3F94
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x0600A021 RID: 40993 RVA: 0x003C5DA1 File Offset: 0x003C3FA1
		public T Current
		{
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x0600A022 RID: 40994 RVA: 0x003C5DAE File Offset: 0x003C3FAE
		public object System.Collections.IEnumerator.Current
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x0400794C RID: 31052
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IDisposable m_scope;

		// Token: 0x0400794D RID: 31053
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator<T> m_enumerator;
	}
}
