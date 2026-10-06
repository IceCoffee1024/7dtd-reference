using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020013B5 RID: 5045
public class MemoryBackedArray<[IsUnmanaged] T> : IBackedArray<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009EC5 RID: 40645 RVA: 0x003BF299 File Offset: 0x003BD499
	public MemoryBackedArray(int length)
	{
		this.m_array = new T[length];
	}

	// Token: 0x06009EC6 RID: 40646 RVA: 0x003BF2AD File Offset: 0x003BD4AD
	[PublicizedFrom(EAccessModifier.Private)]
	public void Dispose(bool disposing)
	{
		if (!disposing)
		{
			Log.Error("MemoryBackedArray<T> is being finalized, it should be disposed properly.");
			return;
		}
		this.m_array = null;
	}

	// Token: 0x06009EC7 RID: 40647 RVA: 0x003BF2C4 File Offset: 0x003BD4C4
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009EC8 RID: 40648 RVA: 0x003BF2D4 File Offset: 0x003BD4D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~MemoryBackedArray()
	{
		this.Dispose(false);
	}

	// Token: 0x06009EC9 RID: 40649 RVA: 0x003BF304 File Offset: 0x003BD504
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ThrowIfDisposed()
	{
		if (this.m_array == null)
		{
			throw new ObjectDisposedException("MemoryBackedArray has already been disposed.");
		}
	}

	// Token: 0x170012FA RID: 4858
	// (get) Token: 0x06009ECA RID: 40650 RVA: 0x003BF319 File Offset: 0x003BD519
	public int Length
	{
		get
		{
			return this.m_array.Length;
		}
	}

	// Token: 0x06009ECB RID: 40651 RVA: 0x003BF324 File Offset: 0x003BD524
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryBackedArray<T>.MemoryBackedArrayHandle GetStaticHandle(BackedArrayHandleMode mode)
	{
		MemoryBackedArray<T>.MemoryBackedArrayHandle memoryBackedArrayHandle;
		if (mode != BackedArrayHandleMode.ReadOnly)
		{
			if (mode != BackedArrayHandleMode.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("mode", mode, string.Format("Unknown mode: {0}", mode));
			}
			memoryBackedArrayHandle = MemoryBackedArray<T>.s_handleReadWrite;
		}
		else
		{
			memoryBackedArrayHandle = MemoryBackedArray<T>.s_handleReadOnly;
		}
		return memoryBackedArrayHandle;
	}

	// Token: 0x06009ECC RID: 40652 RVA: 0x003BF36C File Offset: 0x003BD56C
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle GetMemoryInternal(int start, int length, out Memory<T> memory, BackedArrayHandleMode mode)
	{
		memory = this.m_array.AsMemory<T>(start, length);
		return MemoryBackedArray<T>.GetStaticHandle(mode);
	}

	// Token: 0x06009ECD RID: 40653 RVA: 0x003BF388 File Offset: 0x003BD588
	public IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory)
	{
		return this.GetMemoryInternal(start, length, out memory, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009ECE RID: 40654 RVA: 0x003BF394 File Offset: 0x003BD594
	public IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory)
	{
		Memory<T> memory2;
		IBackedArrayHandle memoryInternal = this.GetMemoryInternal(start, length, out memory2, BackedArrayHandleMode.ReadOnly);
		memory = memory2;
		return memoryInternal;
	}

	// Token: 0x06009ECF RID: 40655 RVA: 0x003BF3B8 File Offset: 0x003BD5B8
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe IBackedArrayHandle GetMemoryUnsafeInternal(int start, int length, out T* arrayPtr, BackedArrayHandleMode mode)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException(string.Format("Expected length to be non-negative but was {0}.", length));
		}
		if (start < 0 || start + length > this.m_array.Length)
		{
			throw new ArgumentOutOfRangeException(string.Format("Expected requested memory range [{0}, {1}) to be a subset of [0, {2}).", start, start + length, this.m_array.Length));
		}
		GCHandle gchandle = GCHandle.Alloc(this.m_array, GCHandleType.Pinned);
		arrayPtr = (void*)gchandle.AddrOfPinnedObject();
		arrayPtr += (IntPtr)start * (IntPtr)sizeof(T);
		return new MemoryBackedArray<T>.MemoryBackedArrayUnsafeHandle(gchandle, mode);
	}

	// Token: 0x06009ED0 RID: 40656 RVA: 0x003BF44C File Offset: 0x003BD64C
	public unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		return this.GetMemoryUnsafeInternal(start, length, out arrayPtr, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009ED1 RID: 40657 RVA: 0x003BF458 File Offset: 0x003BD658
	public unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		return this.GetMemoryUnsafeInternal(start, length, out arrayPtr, BackedArrayHandleMode.ReadOnly);
	}

	// Token: 0x06009ED2 RID: 40658 RVA: 0x003BF464 File Offset: 0x003BD664
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle GetSpanInternal(int start, int length, out Span<T> span, BackedArrayHandleMode mode)
	{
		span = this.m_array.AsSpan<T>(start, length);
		return MemoryBackedArray<T>.GetStaticHandle(mode);
	}

	// Token: 0x06009ED3 RID: 40659 RVA: 0x003BF480 File Offset: 0x003BD680
	public IBackedArrayHandle GetSpan(int start, int length, out Span<T> span)
	{
		return this.GetSpanInternal(start, length, out span, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009ED4 RID: 40660 RVA: 0x003BF48C File Offset: 0x003BD68C
	public IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span)
	{
		Span<T> span2;
		IBackedArrayHandle spanInternal = this.GetSpanInternal(start, length, out span2, BackedArrayHandleMode.ReadOnly);
		span = span2;
		return spanInternal;
	}

	// Token: 0x04007866 RID: 30822
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly MemoryBackedArray<T>.MemoryBackedArrayHandle s_handleReadWrite = new MemoryBackedArray<T>.MemoryBackedArrayHandle(BackedArrayHandleMode.ReadWrite);

	// Token: 0x04007867 RID: 30823
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly MemoryBackedArray<T>.MemoryBackedArrayHandle s_handleReadOnly = new MemoryBackedArray<T>.MemoryBackedArrayHandle(BackedArrayHandleMode.ReadOnly);

	// Token: 0x04007868 RID: 30824
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] m_array;

	// Token: 0x020013B6 RID: 5046
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class MemoryBackedArrayHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009ED6 RID: 40662 RVA: 0x003BF4C8 File Offset: 0x003BD6C8
		public MemoryBackedArrayHandle(BackedArrayHandleMode mode)
		{
			this.m_mode = mode;
		}

		// Token: 0x06009ED7 RID: 40663 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06009ED8 RID: 40664 RVA: 0x003BF4D7 File Offset: 0x003BD6D7
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009ED9 RID: 40665 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x04007869 RID: 30825
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}

	// Token: 0x020013B7 RID: 5047
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class MemoryBackedArrayUnsafeHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009EDA RID: 40666 RVA: 0x003BF4DF File Offset: 0x003BD6DF
		public MemoryBackedArrayUnsafeHandle(GCHandle gcHandle, BackedArrayHandleMode mode)
		{
			this.m_gcHandle = gcHandle;
			this.m_mode = mode;
		}

		// Token: 0x06009EDB RID: 40667 RVA: 0x003BF4F8 File Offset: 0x003BD6F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Dispose(bool disposing)
		{
			if (!disposing)
			{
				Log.Error("MemoryBackedArrayHandle is being finalized, it should be disposed properly.");
				return;
			}
			if (this.m_gcHandle != default(GCHandle))
			{
				this.m_gcHandle.Free();
				this.m_gcHandle = default(GCHandle);
			}
		}

		// Token: 0x06009EDC RID: 40668 RVA: 0x003BF540 File Offset: 0x003BD740
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009EDD RID: 40669 RVA: 0x003BF550 File Offset: 0x003BD750
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~MemoryBackedArrayUnsafeHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06009EDE RID: 40670 RVA: 0x003BF580 File Offset: 0x003BD780
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void ThrowIfDisposed()
		{
			if (this.IsDisposed())
			{
				throw new ObjectDisposedException("MemoryBackedArrayUnsafeHandle has already been disposed.");
			}
		}

		// Token: 0x06009EDF RID: 40671 RVA: 0x003BF598 File Offset: 0x003BD798
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_gcHandle == default(GCHandle);
		}

		// Token: 0x06009EE0 RID: 40672 RVA: 0x003BF5B9 File Offset: 0x003BD7B9
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void ThrowIfCannotWrite()
		{
			if (!this.m_mode.CanWrite())
			{
				throw new NotSupportedException("This MemoryBackedArrayUnsafeHandle is not writable.");
			}
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06009EE1 RID: 40673 RVA: 0x003BF5D3 File Offset: 0x003BD7D3
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009EE2 RID: 40674 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x0400786A RID: 30826
		[PublicizedFrom(EAccessModifier.Private)]
		public GCHandle m_gcHandle;

		// Token: 0x0400786B RID: 30827
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}

	// Token: 0x020013B8 RID: 5048
	public sealed class MemoryBackedArrayView : IBackedArrayView<T>, IDisposable
	{
		// Token: 0x06009EE3 RID: 40675 RVA: 0x003BF5DB File Offset: 0x003BD7DB
		public MemoryBackedArrayView(MemoryBackedArray<T> array, BackedArrayHandleMode mode)
		{
			this.m_array = array.m_array;
			this.m_mode = mode;
		}

		// Token: 0x06009EE4 RID: 40676 RVA: 0x003BF5F6 File Offset: 0x003BD7F6
		[PublicizedFrom(EAccessModifier.Private)]
		public void Dispose(bool disposing)
		{
			if (!disposing)
			{
				Log.Error("MemoryBackedArrayView is being finalized, it should be disposed properly.");
				return;
			}
			this.m_array = null;
		}

		// Token: 0x06009EE5 RID: 40677 RVA: 0x003BF60D File Offset: 0x003BD80D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009EE6 RID: 40678 RVA: 0x003BF61C File Offset: 0x003BD81C
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~MemoryBackedArrayView()
		{
			this.Dispose(false);
		}

		// Token: 0x06009EE7 RID: 40679 RVA: 0x003BF64C File Offset: 0x003BD84C
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void ThrowIfDisposed()
		{
			if (this.IsDisposed())
			{
				throw new ObjectDisposedException("MemoryBackedArrayView has already been disposed.");
			}
		}

		// Token: 0x06009EE8 RID: 40680 RVA: 0x003BF661 File Offset: 0x003BD861
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_array == null;
		}

		// Token: 0x06009EE9 RID: 40681 RVA: 0x003BF66C File Offset: 0x003BD86C
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void ThrowIfCannotWrite()
		{
			if (!this.m_mode.CanWrite())
			{
				throw new NotSupportedException("This MemoryBackedArrayView is not writable.");
			}
		}

		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06009EEA RID: 40682 RVA: 0x003BF686 File Offset: 0x003BD886
		public int Length
		{
			get
			{
				return this.m_array.Length;
			}
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06009EEB RID: 40683 RVA: 0x003BF690 File Offset: 0x003BD890
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x170012FF RID: 4863
		public T this[int i]
		{
			get
			{
				return this.m_array[i];
			}
			set
			{
				this.m_array[i] = value;
			}
		}

		// Token: 0x06009EEE RID: 40686 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x0400786C RID: 30828
		[PublicizedFrom(EAccessModifier.Private)]
		public T[] m_array;

		// Token: 0x0400786D RID: 30829
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}
}
