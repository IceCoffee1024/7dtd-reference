using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02001395 RID: 5013
public class MemoryBackedArray<[IsUnmanaged] T> : IBackedArray<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009E1A RID: 40474 RVA: 0x003BA71D File Offset: 0x003B891D
	public MemoryBackedArray(int length)
	{
		this.m_array = new T[length];
	}

	// Token: 0x06009E1B RID: 40475 RVA: 0x003BA731 File Offset: 0x003B8931
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

	// Token: 0x06009E1C RID: 40476 RVA: 0x003BA748 File Offset: 0x003B8948
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009E1D RID: 40477 RVA: 0x003BA758 File Offset: 0x003B8958
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~MemoryBackedArray()
	{
		this.Dispose(false);
	}

	// Token: 0x06009E1E RID: 40478 RVA: 0x003BA788 File Offset: 0x003B8988
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

	// Token: 0x170012B5 RID: 4789
	// (get) Token: 0x06009E1F RID: 40479 RVA: 0x003BA79D File Offset: 0x003B899D
	public int Length
	{
		get
		{
			return this.m_array.Length;
		}
	}

	// Token: 0x06009E20 RID: 40480 RVA: 0x003BA7A8 File Offset: 0x003B89A8
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

	// Token: 0x06009E21 RID: 40481 RVA: 0x003BA7F0 File Offset: 0x003B89F0
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle GetMemoryInternal(int start, int length, out Memory<T> memory, BackedArrayHandleMode mode)
	{
		memory = this.m_array.AsMemory<T>(start, length);
		return MemoryBackedArray<T>.GetStaticHandle(mode);
	}

	// Token: 0x06009E22 RID: 40482 RVA: 0x003BA80C File Offset: 0x003B8A0C
	public IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory)
	{
		return this.GetMemoryInternal(start, length, out memory, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009E23 RID: 40483 RVA: 0x003BA818 File Offset: 0x003B8A18
	public IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory)
	{
		Memory<T> memory2;
		IBackedArrayHandle memoryInternal = this.GetMemoryInternal(start, length, out memory2, BackedArrayHandleMode.ReadOnly);
		memory = memory2;
		return memoryInternal;
	}

	// Token: 0x06009E24 RID: 40484 RVA: 0x003BA83C File Offset: 0x003B8A3C
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

	// Token: 0x06009E25 RID: 40485 RVA: 0x003BA8D0 File Offset: 0x003B8AD0
	public unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		return this.GetMemoryUnsafeInternal(start, length, out arrayPtr, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009E26 RID: 40486 RVA: 0x003BA8DC File Offset: 0x003B8ADC
	public unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		return this.GetMemoryUnsafeInternal(start, length, out arrayPtr, BackedArrayHandleMode.ReadOnly);
	}

	// Token: 0x06009E27 RID: 40487 RVA: 0x003BA8E8 File Offset: 0x003B8AE8
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle GetSpanInternal(int start, int length, out Span<T> span, BackedArrayHandleMode mode)
	{
		span = this.m_array.AsSpan<T>(start, length);
		return MemoryBackedArray<T>.GetStaticHandle(mode);
	}

	// Token: 0x06009E28 RID: 40488 RVA: 0x003BA904 File Offset: 0x003B8B04
	public IBackedArrayHandle GetSpan(int start, int length, out Span<T> span)
	{
		return this.GetSpanInternal(start, length, out span, BackedArrayHandleMode.ReadWrite);
	}

	// Token: 0x06009E29 RID: 40489 RVA: 0x003BA910 File Offset: 0x003B8B10
	public IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span)
	{
		Span<T> span2;
		IBackedArrayHandle spanInternal = this.GetSpanInternal(start, length, out span2, BackedArrayHandleMode.ReadOnly);
		span = span2;
		return spanInternal;
	}

	// Token: 0x040077EB RID: 30699
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly MemoryBackedArray<T>.MemoryBackedArrayHandle s_handleReadWrite = new MemoryBackedArray<T>.MemoryBackedArrayHandle(BackedArrayHandleMode.ReadWrite);

	// Token: 0x040077EC RID: 30700
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly MemoryBackedArray<T>.MemoryBackedArrayHandle s_handleReadOnly = new MemoryBackedArray<T>.MemoryBackedArrayHandle(BackedArrayHandleMode.ReadOnly);

	// Token: 0x040077ED RID: 30701
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] m_array;

	// Token: 0x02001396 RID: 5014
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class MemoryBackedArrayHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009E2B RID: 40491 RVA: 0x003BA94C File Offset: 0x003B8B4C
		public MemoryBackedArrayHandle(BackedArrayHandleMode mode)
		{
			this.m_mode = mode;
		}

		// Token: 0x06009E2C RID: 40492 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06009E2D RID: 40493 RVA: 0x003BA95B File Offset: 0x003B8B5B
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009E2E RID: 40494 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x040077EE RID: 30702
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}

	// Token: 0x02001397 RID: 5015
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class MemoryBackedArrayUnsafeHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009E2F RID: 40495 RVA: 0x003BA963 File Offset: 0x003B8B63
		public MemoryBackedArrayUnsafeHandle(GCHandle gcHandle, BackedArrayHandleMode mode)
		{
			this.m_gcHandle = gcHandle;
			this.m_mode = mode;
		}

		// Token: 0x06009E30 RID: 40496 RVA: 0x003BA97C File Offset: 0x003B8B7C
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

		// Token: 0x06009E31 RID: 40497 RVA: 0x003BA9C4 File Offset: 0x003B8BC4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009E32 RID: 40498 RVA: 0x003BA9D4 File Offset: 0x003B8BD4
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~MemoryBackedArrayUnsafeHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06009E33 RID: 40499 RVA: 0x003BAA04 File Offset: 0x003B8C04
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

		// Token: 0x06009E34 RID: 40500 RVA: 0x003BAA1C File Offset: 0x003B8C1C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_gcHandle == default(GCHandle);
		}

		// Token: 0x06009E35 RID: 40501 RVA: 0x003BAA3D File Offset: 0x003B8C3D
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

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06009E36 RID: 40502 RVA: 0x003BAA57 File Offset: 0x003B8C57
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009E37 RID: 40503 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x040077EF RID: 30703
		[PublicizedFrom(EAccessModifier.Private)]
		public GCHandle m_gcHandle;

		// Token: 0x040077F0 RID: 30704
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}

	// Token: 0x02001398 RID: 5016
	public sealed class MemoryBackedArrayView : IBackedArrayView<T>, IDisposable
	{
		// Token: 0x06009E38 RID: 40504 RVA: 0x003BAA5F File Offset: 0x003B8C5F
		public MemoryBackedArrayView(MemoryBackedArray<T> array, BackedArrayHandleMode mode)
		{
			this.m_array = array.m_array;
			this.m_mode = mode;
		}

		// Token: 0x06009E39 RID: 40505 RVA: 0x003BAA7A File Offset: 0x003B8C7A
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

		// Token: 0x06009E3A RID: 40506 RVA: 0x003BAA91 File Offset: 0x003B8C91
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009E3B RID: 40507 RVA: 0x003BAAA0 File Offset: 0x003B8CA0
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~MemoryBackedArrayView()
		{
			this.Dispose(false);
		}

		// Token: 0x06009E3C RID: 40508 RVA: 0x003BAAD0 File Offset: 0x003B8CD0
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

		// Token: 0x06009E3D RID: 40509 RVA: 0x003BAAE5 File Offset: 0x003B8CE5
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_array == null;
		}

		// Token: 0x06009E3E RID: 40510 RVA: 0x003BAAF0 File Offset: 0x003B8CF0
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

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06009E3F RID: 40511 RVA: 0x003BAB0A File Offset: 0x003B8D0A
		public int Length
		{
			get
			{
				return this.m_array.Length;
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06009E40 RID: 40512 RVA: 0x003BAB14 File Offset: 0x003B8D14
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x170012BA RID: 4794
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

		// Token: 0x06009E43 RID: 40515 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush()
		{
		}

		// Token: 0x040077F1 RID: 30705
		[PublicizedFrom(EAccessModifier.Private)]
		public T[] m_array;

		// Token: 0x040077F2 RID: 30706
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;
	}
}
