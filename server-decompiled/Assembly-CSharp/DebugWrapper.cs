using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013CD RID: 5069
public abstract class DebugWrapper
{
	// Token: 0x06009F58 RID: 40792 RVA: 0x003C0D8C File Offset: 0x003BEF8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DebugWrapper(DebugWrapper parent)
	{
		this.m_readCounter = ((parent != null) ? parent.m_readCounter : null) ?? new AtomicCounter();
		this.m_writeCounter = ((parent != null) ? parent.m_writeCounter : null) ?? new AtomicCounter();
		this.DebugNonMainThreadAccess = parent != null && parent.DebugNonMainThreadAccess;
		this.DebugConcurrentModifications = parent != null && parent.DebugConcurrentModifications;
	}

	// Token: 0x170012CF RID: 4815
	// (get) Token: 0x06009F59 RID: 40793 RVA: 0x003C0DF9 File Offset: 0x003BEFF9
	// (set) Token: 0x06009F5A RID: 40794 RVA: 0x003C0E01 File Offset: 0x003BF001
	public bool DebugNonMainThreadAccess { get; set; }

	// Token: 0x170012D0 RID: 4816
	// (get) Token: 0x06009F5B RID: 40795 RVA: 0x003C0E0A File Offset: 0x003BF00A
	// (set) Token: 0x06009F5C RID: 40796 RVA: 0x003C0E12 File Offset: 0x003BF012
	public bool DebugConcurrentModifications { get; set; }

	// Token: 0x170012D1 RID: 4817
	// (get) Token: 0x06009F5D RID: 40797 RVA: 0x003C0E1B File Offset: 0x003BF01B
	public bool NeedsScope
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.DebugConcurrentModifications;
		}
	}

	// Token: 0x06009F5E RID: 40798 RVA: 0x003C0E23 File Offset: 0x003BF023
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

	// Token: 0x06009F5F RID: 40799 RVA: 0x003C0E57 File Offset: 0x003BF057
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

	// Token: 0x06009F60 RID: 40800 RVA: 0x003C0E8B File Offset: 0x003BF08B
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator<T> DebugEnumerator<T>(IEnumerator<T> enumerator)
	{
		return new DebugWrapper.Enumerator<T>(this.DebugReadScope(), enumerator);
	}

	// Token: 0x06009F61 RID: 40801 RVA: 0x003C0E99 File Offset: 0x003BF099
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator DebugEnumerator(IEnumerator enumerator)
	{
		return new DebugWrapper.Enumerator(this.DebugReadScope(), enumerator);
	}

	// Token: 0x040078C4 RID: 30916
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AtomicCounter m_readCounter;

	// Token: 0x040078C5 RID: 30917
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AtomicCounter m_writeCounter;

	// Token: 0x020013CE RID: 5070
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DummyScope : IDisposable
	{
		// Token: 0x06009F62 RID: 40802 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public DummyScope()
		{
		}

		// Token: 0x06009F63 RID: 40803 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x040078C8 RID: 30920
		public static readonly DebugWrapper.DummyScope Instance = new DebugWrapper.DummyScope();
	}

	// Token: 0x020013CF RID: 5071
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ReadScope : IDisposable
	{
		// Token: 0x06009F65 RID: 40805 RVA: 0x003C0EB4 File Offset: 0x003BF0B4
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

		// Token: 0x06009F66 RID: 40806 RVA: 0x003C0F18 File Offset: 0x003BF118
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~ReadScope()
		{
			this.Dispose(false);
		}

		// Token: 0x06009F67 RID: 40807 RVA: 0x003C0F48 File Offset: 0x003BF148
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009F68 RID: 40808 RVA: 0x003C0F58 File Offset: 0x003BF158
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

		// Token: 0x040078C9 RID: 30921
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DebugWrapper m_wrapper;

		// Token: 0x040078CA RID: 30922
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_concurrentModificationNotified;

		// Token: 0x040078CB RID: 30923
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}

	// Token: 0x020013D0 RID: 5072
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class ReadWriteScope : IDisposable
	{
		// Token: 0x06009F69 RID: 40809 RVA: 0x003C0FCC File Offset: 0x003BF1CC
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

		// Token: 0x06009F6A RID: 40810 RVA: 0x003C1078 File Offset: 0x003BF278
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~ReadWriteScope()
		{
			this.Dispose(false);
		}

		// Token: 0x06009F6B RID: 40811 RVA: 0x003C10A8 File Offset: 0x003BF2A8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009F6C RID: 40812 RVA: 0x003C10B8 File Offset: 0x003BF2B8
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

		// Token: 0x040078CC RID: 30924
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DebugWrapper m_wrapper;

		// Token: 0x040078CD RID: 30925
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_concurrentModificationNotified;

		// Token: 0x040078CE RID: 30926
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}

	// Token: 0x020013D1 RID: 5073
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Enumerator : IEnumerator, IDisposable
	{
		// Token: 0x06009F6D RID: 40813 RVA: 0x003C115E File Offset: 0x003BF35E
		public Enumerator(IDisposable scope, IEnumerator enumerator)
		{
			this.m_scope = scope;
			this.m_enumerator = enumerator;
		}

		// Token: 0x06009F6E RID: 40814 RVA: 0x003C1174 File Offset: 0x003BF374
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

		// Token: 0x06009F6F RID: 40815 RVA: 0x003C1196 File Offset: 0x003BF396
		public bool MoveNext()
		{
			return this.m_enumerator.MoveNext();
		}

		// Token: 0x06009F70 RID: 40816 RVA: 0x003C11A3 File Offset: 0x003BF3A3
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06009F71 RID: 40817 RVA: 0x003C11B0 File Offset: 0x003BF3B0
		public object Current
		{
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x040078CF RID: 30927
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IDisposable m_scope;

		// Token: 0x040078D0 RID: 30928
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator m_enumerator;
	}

	// Token: 0x020013D2 RID: 5074
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class Enumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
	{
		// Token: 0x06009F72 RID: 40818 RVA: 0x003C11BD File Offset: 0x003BF3BD
		public Enumerator(IDisposable scope, IEnumerator<T> enumerator)
		{
			this.m_scope = scope;
			this.m_enumerator = enumerator;
		}

		// Token: 0x06009F73 RID: 40819 RVA: 0x003C11D3 File Offset: 0x003BF3D3
		public void Dispose()
		{
			this.m_scope.Dispose();
			this.m_enumerator.Dispose();
		}

		// Token: 0x06009F74 RID: 40820 RVA: 0x003C11EB File Offset: 0x003BF3EB
		public bool MoveNext()
		{
			return this.m_enumerator.MoveNext();
		}

		// Token: 0x06009F75 RID: 40821 RVA: 0x003C11F8 File Offset: 0x003BF3F8
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x06009F76 RID: 40822 RVA: 0x003C1205 File Offset: 0x003BF405
		public T Current
		{
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x06009F77 RID: 40823 RVA: 0x003C1212 File Offset: 0x003BF412
		public object System.Collections.IEnumerator.Current
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x040078D1 RID: 30929
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IDisposable m_scope;

		// Token: 0x040078D2 RID: 30930
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEnumerator<T> m_enumerator;
	}
}
