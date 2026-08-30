using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x0200137B RID: 4987
public class MemoryBackedArray<[IsUnmanaged] T> : IBackedArray<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009D1F RID: 40223 RVA: 0x003B5469 File Offset: 0x003B3669
	public MemoryBackedArray(int length)
	{
		this.m_array = new T[length];
	}

	// Token: 0x06009D20 RID: 40224 RVA: 0x003B547D File Offset: 0x003B367D
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

	// Token: 0x06009D21 RID: 40225 RVA: 0x003B5494 File Offset: 0x003B3694
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009D22 RID: 40226 RVA: 0x003B54A4 File Offset: 0x003B36A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~MemoryBackedArray()
	{
		this.Dispose(false);
	}

	// Token: 0x06009D23 RID: 40227 RVA: 0x003B54D4 File Offset: 0x003B36D4
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

	// Token: 0x1700127C RID: 4732
	// (get) Token: 0x06009D24 RID: 40228 RVA: 0x003B54E9 File Offset: 0x003B36E9
	public int Length
	{
		get
		{
			return this.m_array.Length;
		}
	}

	// Token: 0x06009D25 RID: 40229 RVA: 0x003B54F4 File Offset: 0x003B36F4
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

	// Token: 0x06009D26 RID: 40230 RVA: 0x003B553C File Offset: 0x003B373C
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle GetMemoryInternal(int start, int length, out Memory<T> memory, BackedArrayHandleMode mode)
	{
		memory = this.m_array.AsMemory<T>(start, length);
		return MemoryBackedArray<T>.GetStaticHandle(mode);
	}

	// Token: 0x06009D27 RID: 40231 RVA: 0x003B5558 File Offset: 0x003B3758
	public IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory)
	{
		return this.GetMemoryInternal(start, length, out memory, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009D28 RID: 40232 RVA: 0x003B5564 File Offset: 0x003B3764
	public IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory)
	{
		Memory<T> memory2;
		IBackedArrayHandle memoryInternal = this.GetMemoryInternal(start, length, out memory2, BackedArrayHandleMode.ReadOnly);
		memory = memory2;
		return memoryInternal;
	}

	// Token: 0x06009D29 RID: 40233 RVA: 0x003B5588 File Offset: 0x003B3788
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

	// Token: 0x06009D2A RID: 40234 RVA: 0x003B561C File Offset: 0x003B381C
	public unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		return this.GetMemoryUnsafeInternal(start, length, out arrayPtr, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009D2B RID: 40235 RVA: 0x003B5628 File Offset: 0x003B3828
	public unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		return this.GetMemoryUnsafeInternal(start, length, out arrayPtr, BackedArrayHandleMode.ReadOnly);
	}

	// Token: 0x06009D2C RID: 40236 RVA: 0x003B5634 File Offset: 0x003B3834
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle GetSpanInternal(int start, int length, out Span<T> span, BackedArrayHandleMode mode)
	{
		span = this.m_array.AsSpan<T>(start, length);
		return MemoryBackedArray<T>.GetStaticHandle(mode);
	}

	// Token: 0x06009D2D RID: 40237 RVA: 0x003B5650 File Offset: 0x003B3850
	public IBackedArrayHandle GetSpan(int start, int length, out Span<T> span)
	{
		return this.GetSpanInternal(start, length, out span, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009D2E RID: 40238 RVA: 0x003B565C File Offset: 0x003B385C
	public IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span)
	{
		Span<T> span2;
		IBackedArrayHandle spanInternal = this.GetSpanInternal(start, length, out span2, BackedArrayHandleMode.ReadOnly);
		span = span2;
		return spanInternal;
	}

	// Token: 0x04007713 RID: 30483
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly MemoryBackedArray<T>.MemoryBackedArrayHandle s_handleReadWrite = new MemoryBackedArray<T>.MemoryBackedArrayHandle(BackedArrayHandleMode.ReadWrite);

	// Token: 0x04007714 RID: 30484
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly MemoryBackedArray<T>.MemoryBackedArrayHandle s_handleReadOnly = new MemoryBackedArray<T>.MemoryBackedArrayHandle(BackedArrayHandleMode.ReadOnly);

	// Token: 0x04007715 RID: 30485
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] m_array;

	// Token: 0x0200137C RID: 4988
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class MemoryBackedArrayHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009D30 RID: 40240 RVA: 0x003B5698 File Offset: 0x003B3898
		public MemoryBackedArrayHandle(BackedArrayHandleMode mode)
		{
			this.m_mode = mode;
		}

		// Token: 0x06009D31 RID: 40241 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06009D32 RID: 40242 RVA: 0x003B56A7 File Offset: 0x003B38A7
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009D33 RID: 40243 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x04007716 RID: 30486
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}

	// Token: 0x0200137D RID: 4989
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class MemoryBackedArrayUnsafeHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009D34 RID: 40244 RVA: 0x003B56AF File Offset: 0x003B38AF
		public MemoryBackedArrayUnsafeHandle(GCHandle gcHandle, BackedArrayHandleMode mode)
		{
			this.m_gcHandle = gcHandle;
			this.m_mode = mode;
		}

		// Token: 0x06009D35 RID: 40245 RVA: 0x003B56C8 File Offset: 0x003B38C8
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

		// Token: 0x06009D36 RID: 40246 RVA: 0x003B5710 File Offset: 0x003B3910
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009D37 RID: 40247 RVA: 0x003B5720 File Offset: 0x003B3920
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~MemoryBackedArrayUnsafeHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06009D38 RID: 40248 RVA: 0x003B5750 File Offset: 0x003B3950
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

		// Token: 0x06009D39 RID: 40249 RVA: 0x003B5768 File Offset: 0x003B3968
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_gcHandle == default(GCHandle);
		}

		// Token: 0x06009D3A RID: 40250 RVA: 0x003B5789 File Offset: 0x003B3989
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

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06009D3B RID: 40251 RVA: 0x003B57A3 File Offset: 0x003B39A3
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009D3C RID: 40252 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x04007717 RID: 30487
		[PublicizedFrom(EAccessModifier.Private)]
		public GCHandle m_gcHandle;

		// Token: 0x04007718 RID: 30488
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}

	// Token: 0x0200137E RID: 4990
	public sealed class MemoryBackedArrayView : IBackedArrayView<T>, IDisposable
	{
		// Token: 0x06009D3D RID: 40253 RVA: 0x003B57AB File Offset: 0x003B39AB
		public MemoryBackedArrayView(MemoryBackedArray<T> array, BackedArrayHandleMode mode)
		{
			this.m_array = array.m_array;
			this.m_mode = mode;
		}

		// Token: 0x06009D3E RID: 40254 RVA: 0x003B57C6 File Offset: 0x003B39C6
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

		// Token: 0x06009D3F RID: 40255 RVA: 0x003B57DD File Offset: 0x003B39DD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009D40 RID: 40256 RVA: 0x003B57EC File Offset: 0x003B39EC
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~MemoryBackedArrayView()
		{
			this.Dispose(false);
		}

		// Token: 0x06009D41 RID: 40257 RVA: 0x003B581C File Offset: 0x003B3A1C
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

		// Token: 0x06009D42 RID: 40258 RVA: 0x003B5831 File Offset: 0x003B3A31
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_array == null;
		}

		// Token: 0x06009D43 RID: 40259 RVA: 0x003B583C File Offset: 0x003B3A3C
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

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06009D44 RID: 40260 RVA: 0x003B5856 File Offset: 0x003B3A56
		public int Length
		{
			get
			{
				return this.m_array.Length;
			}
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06009D45 RID: 40261 RVA: 0x003B5860 File Offset: 0x003B3A60
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x17001281 RID: 4737
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

		// Token: 0x06009D48 RID: 40264 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x04007719 RID: 30489
		[PublicizedFrom(EAccessModifier.Private)]
		public T[] m_array;

		// Token: 0x0400771A RID: 30490
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}
}
