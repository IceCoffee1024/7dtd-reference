using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020013C7 RID: 5063
public class MemoryBackedArray<[IsUnmanaged] T> : IBackedArray<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009F1D RID: 40733 RVA: 0x003C169D File Offset: 0x003BF89D
	public MemoryBackedArray(int length)
	{
		this.m_array = new T[length];
	}

	// Token: 0x06009F1E RID: 40734 RVA: 0x003C16B1 File Offset: 0x003BF8B1
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

	// Token: 0x06009F1F RID: 40735 RVA: 0x003C16C8 File Offset: 0x003BF8C8
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009F20 RID: 40736 RVA: 0x003C16D8 File Offset: 0x003BF8D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~MemoryBackedArray()
	{
		this.Dispose(false);
	}

	// Token: 0x06009F21 RID: 40737 RVA: 0x003C1708 File Offset: 0x003BF908
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

	// Token: 0x17001306 RID: 4870
	// (get) Token: 0x06009F22 RID: 40738 RVA: 0x003C171D File Offset: 0x003BF91D
	public int Length
	{
		get
		{
			return this.m_array.Length;
		}
	}

	// Token: 0x06009F23 RID: 40739 RVA: 0x003C1728 File Offset: 0x003BF928
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

	// Token: 0x06009F24 RID: 40740 RVA: 0x003C1770 File Offset: 0x003BF970
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle GetMemoryInternal(int start, int length, out Memory<T> memory, BackedArrayHandleMode mode)
	{
		memory = this.m_array.AsMemory<T>(start, length);
		return MemoryBackedArray<T>.GetStaticHandle(mode);
	}

	// Token: 0x06009F25 RID: 40741 RVA: 0x003C178C File Offset: 0x003BF98C
	public IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory)
	{
		return this.GetMemoryInternal(start, length, out memory, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009F26 RID: 40742 RVA: 0x003C1798 File Offset: 0x003BF998
	public IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory)
	{
		Memory<T> memory2;
		IBackedArrayHandle memoryInternal = this.GetMemoryInternal(start, length, out memory2, BackedArrayHandleMode.ReadOnly);
		memory = memory2;
		return memoryInternal;
	}

	// Token: 0x06009F27 RID: 40743 RVA: 0x003C17BC File Offset: 0x003BF9BC
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

	// Token: 0x06009F28 RID: 40744 RVA: 0x003C1850 File Offset: 0x003BFA50
	public unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		return this.GetMemoryUnsafeInternal(start, length, out arrayPtr, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009F29 RID: 40745 RVA: 0x003C185C File Offset: 0x003BFA5C
	public unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		return this.GetMemoryUnsafeInternal(start, length, out arrayPtr, BackedArrayHandleMode.ReadOnly);
	}

	// Token: 0x06009F2A RID: 40746 RVA: 0x003C1868 File Offset: 0x003BFA68
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle GetSpanInternal(int start, int length, out Span<T> span, BackedArrayHandleMode mode)
	{
		span = this.m_array.AsSpan<T>(start, length);
		return MemoryBackedArray<T>.GetStaticHandle(mode);
	}

	// Token: 0x06009F2B RID: 40747 RVA: 0x003C1884 File Offset: 0x003BFA84
	public IBackedArrayHandle GetSpan(int start, int length, out Span<T> span)
	{
		return this.GetSpanInternal(start, length, out span, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009F2C RID: 40748 RVA: 0x003C1890 File Offset: 0x003BFA90
	public IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span)
	{
		Span<T> span2;
		IBackedArrayHandle spanInternal = this.GetSpanInternal(start, length, out span2, BackedArrayHandleMode.ReadOnly);
		span = span2;
		return spanInternal;
	}

	// Token: 0x040078A7 RID: 30887
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly MemoryBackedArray<T>.MemoryBackedArrayHandle s_handleReadWrite = new MemoryBackedArray<T>.MemoryBackedArrayHandle(BackedArrayHandleMode.ReadWrite);

	// Token: 0x040078A8 RID: 30888
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly MemoryBackedArray<T>.MemoryBackedArrayHandle s_handleReadOnly = new MemoryBackedArray<T>.MemoryBackedArrayHandle(BackedArrayHandleMode.ReadOnly);

	// Token: 0x040078A9 RID: 30889
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] m_array;

	// Token: 0x020013C8 RID: 5064
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class MemoryBackedArrayHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009F2E RID: 40750 RVA: 0x003C18CC File Offset: 0x003BFACC
		public MemoryBackedArrayHandle(BackedArrayHandleMode mode)
		{
			this.m_mode = mode;
		}

		// Token: 0x06009F2F RID: 40751 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x06009F30 RID: 40752 RVA: 0x003C18DB File Offset: 0x003BFADB
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009F31 RID: 40753 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x040078AA RID: 30890
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}

	// Token: 0x020013C9 RID: 5065
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class MemoryBackedArrayUnsafeHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009F32 RID: 40754 RVA: 0x003C18E3 File Offset: 0x003BFAE3
		public MemoryBackedArrayUnsafeHandle(GCHandle gcHandle, BackedArrayHandleMode mode)
		{
			this.m_gcHandle = gcHandle;
			this.m_mode = mode;
		}

		// Token: 0x06009F33 RID: 40755 RVA: 0x003C18FC File Offset: 0x003BFAFC
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

		// Token: 0x06009F34 RID: 40756 RVA: 0x003C1944 File Offset: 0x003BFB44
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009F35 RID: 40757 RVA: 0x003C1954 File Offset: 0x003BFB54
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~MemoryBackedArrayUnsafeHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06009F36 RID: 40758 RVA: 0x003C1984 File Offset: 0x003BFB84
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

		// Token: 0x06009F37 RID: 40759 RVA: 0x003C199C File Offset: 0x003BFB9C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_gcHandle == default(GCHandle);
		}

		// Token: 0x06009F38 RID: 40760 RVA: 0x003C19BD File Offset: 0x003BFBBD
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

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x06009F39 RID: 40761 RVA: 0x003C19D7 File Offset: 0x003BFBD7
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009F3A RID: 40762 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x040078AB RID: 30891
		[PublicizedFrom(EAccessModifier.Private)]
		public GCHandle m_gcHandle;

		// Token: 0x040078AC RID: 30892
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}

	// Token: 0x020013CA RID: 5066
	public sealed class MemoryBackedArrayView : IBackedArrayView<T>, IDisposable
	{
		// Token: 0x06009F3B RID: 40763 RVA: 0x003C19DF File Offset: 0x003BFBDF
		public MemoryBackedArrayView(MemoryBackedArray<T> array, BackedArrayHandleMode mode)
		{
			this.m_array = array.m_array;
			this.m_mode = mode;
		}

		// Token: 0x06009F3C RID: 40764 RVA: 0x003C19FA File Offset: 0x003BFBFA
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

		// Token: 0x06009F3D RID: 40765 RVA: 0x003C1A11 File Offset: 0x003BFC11
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009F3E RID: 40766 RVA: 0x003C1A20 File Offset: 0x003BFC20
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~MemoryBackedArrayView()
		{
			this.Dispose(false);
		}

		// Token: 0x06009F3F RID: 40767 RVA: 0x003C1A50 File Offset: 0x003BFC50
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

		// Token: 0x06009F40 RID: 40768 RVA: 0x003C1A65 File Offset: 0x003BFC65
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_array == null;
		}

		// Token: 0x06009F41 RID: 40769 RVA: 0x003C1A70 File Offset: 0x003BFC70
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

		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06009F42 RID: 40770 RVA: 0x003C1A8A File Offset: 0x003BFC8A
		public int Length
		{
			get
			{
				return this.m_array.Length;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06009F43 RID: 40771 RVA: 0x003C1A94 File Offset: 0x003BFC94
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x1700130B RID: 4875
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

		// Token: 0x06009F46 RID: 40774 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x040078AD RID: 30893
		[PublicizedFrom(EAccessModifier.Private)]
		public T[] m_array;

		// Token: 0x040078AE RID: 30894
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}
}
