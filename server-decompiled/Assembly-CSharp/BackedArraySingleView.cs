using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x0200138C RID: 5004
public sealed class BackedArraySingleView<[IsUnmanaged] T> : IBackedArrayView<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009DCC RID: 40396 RVA: 0x003B9984 File Offset: 0x003B7B84
	[PublicizedFrom(EAccessModifier.Private)]
	public BackedArraySingleView<T>.View CreateView()
	{
		BackedArraySingleView<T>.View view = new BackedArraySingleView<T>.View(this);
		int num = Interlocked.Increment(ref this.m_viewCount);
		if (num > 16)
		{
			Log.Warning(string.Format("{0}<T> has opened a large amount of array views, this could indicate a memory issue. Count: {1}, View Length: {2}", "BackedArraySingleView", num, this.m_viewLength));
		}
		return view;
	}

	// Token: 0x06009DCD RID: 40397 RVA: 0x003B99D0 File Offset: 0x003B7BD0
	public BackedArraySingleView(IBackedArray<T> array, BackedArrayHandleMode mode, int viewLength = 0)
	{
		this.m_array = array;
		this.m_length = array.Length;
		this.m_mode = mode;
		if (!mode.CanRead())
		{
			throw new ArgumentException("Expected a readable mode.", "mode");
		}
		if (viewLength <= 0)
		{
			viewLength = BackedArraySingleView<T>.GetDefaultViewLength(array.Length);
		}
		this.m_viewLength = Math.Min(this.m_length, viewLength);
		this.m_views = new ThreadLocal<BackedArraySingleView<T>.View>(new Func<BackedArraySingleView<T>.View>(this.CreateView), true);
	}

	// Token: 0x06009DCE RID: 40398 RVA: 0x003B9A50 File Offset: 0x003B7C50
	[PublicizedFrom(EAccessModifier.Private)]
	public void Dispose(bool disposing)
	{
		if (!disposing)
		{
			Log.Error("BackedArraySingleView<T> is being finalized, it should be disposed properly.");
			return;
		}
		if (this.IsDisposed())
		{
			return;
		}
		foreach (BackedArraySingleView<T>.View view in this.m_views.Values)
		{
			view.Dispose();
		}
		this.m_views.Dispose();
		this.m_views = null;
		this.m_array = null;
	}

	// Token: 0x06009DCF RID: 40399 RVA: 0x003B9AD0 File Offset: 0x003B7CD0
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009DD0 RID: 40400 RVA: 0x003B9AE0 File Offset: 0x003B7CE0
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~BackedArraySingleView()
	{
		this.Dispose(false);
	}

	// Token: 0x06009DD1 RID: 40401 RVA: 0x003B9B10 File Offset: 0x003B7D10
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ThrowIfDisposed()
	{
		if (this.IsDisposed())
		{
			throw new ObjectDisposedException("BackedArraySingleView has already been disposed.");
		}
	}

	// Token: 0x06009DD2 RID: 40402 RVA: 0x003B9B25 File Offset: 0x003B7D25
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsDisposed()
	{
		return this.m_array == null;
	}

	// Token: 0x06009DD3 RID: 40403 RVA: 0x003B9B30 File Offset: 0x003B7D30
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ThrowIfCannotWrite()
	{
		if (!this.m_mode.CanWrite())
		{
			throw new NotSupportedException("This BackedArraySingleView is not writable.");
		}
	}

	// Token: 0x170012A9 RID: 4777
	// (get) Token: 0x06009DD4 RID: 40404 RVA: 0x003B9B4A File Offset: 0x003B7D4A
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x170012AA RID: 4778
	// (get) Token: 0x06009DD5 RID: 40405 RVA: 0x003B9B52 File Offset: 0x003B7D52
	public BackedArrayHandleMode Mode
	{
		get
		{
			return this.m_mode;
		}
	}

	// Token: 0x170012AB RID: 4779
	public T this[int i]
	{
		get
		{
			return this.m_views.Value[i];
		}
		set
		{
			this.m_views.Value[i] = value;
		}
	}

	// Token: 0x06009DD8 RID: 40408 RVA: 0x003B9B84 File Offset: 0x003B7D84
	public void Flush()
	{
		foreach (BackedArraySingleView<T>.View view in this.m_views.Values)
		{
			view.m_handle.Flush();
		}
	}

	// Token: 0x06009DD9 RID: 40409 RVA: 0x003B9BD8 File Offset: 0x003B7DD8
	public static int GetDefaultViewLength(int length)
	{
		return Mathf.NextPowerOfTwo(4 * (int)Math.Sqrt((double)length));
	}

	// Token: 0x040077CB RID: 30667
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArray<T> m_array;

	// Token: 0x040077CC RID: 30668
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_length;

	// Token: 0x040077CD RID: 30669
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BackedArrayHandleMode m_mode;

	// Token: 0x040077CE RID: 30670
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_viewLength;

	// Token: 0x040077CF RID: 30671
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<BackedArraySingleView<T>.View> m_views;

	// Token: 0x040077D0 RID: 30672
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_viewCount;

	// Token: 0x040077D1 RID: 30673
	[PublicizedFrom(EAccessModifier.Private)]
	public const int VIEW_COUNT_WARNING_THRESHOLD = 16;

	// Token: 0x0200138D RID: 5005
	[PublicizedFrom(EAccessModifier.Private)]
	public class View : IDisposable
	{
		// Token: 0x06009DDA RID: 40410 RVA: 0x003B9BE9 File Offset: 0x003B7DE9
		public View(BackedArraySingleView<T> backedArraySingleView)
		{
			this.m_backedArraySingleView = backedArraySingleView;
		}

		// Token: 0x06009DDB RID: 40411 RVA: 0x003B9C04 File Offset: 0x003B7E04
		public void Cache(int offset)
		{
			if (this.m_start <= offset && offset < this.m_end)
			{
				return;
			}
			IBackedArray<T> array = this.m_backedArraySingleView.m_array;
			int length = this.m_backedArraySingleView.m_length;
			int viewLength = this.m_backedArraySingleView.m_viewLength;
			BackedArrayHandleMode mode = this.m_backedArraySingleView.m_mode;
			if (offset < 0 || offset >= length)
			{
				throw new IndexOutOfRangeException(string.Format("Expected index {0} to be in the range [0, {1}).", offset, length));
			}
			if (offset + viewLength > length)
			{
				offset = length - viewLength;
			}
			IBackedArrayHandle handle = this.m_handle;
			if (handle != null)
			{
				handle.Dispose();
			}
			IBackedArrayHandle backedArrayHandle;
			if (mode != BackedArrayHandleMode.ReadOnly)
			{
				if (mode != BackedArrayHandleMode.ReadWrite)
				{
					throw new ArgumentOutOfRangeException("mode", mode, string.Format("Unknown mode: {0}", mode));
				}
				backedArrayHandle = array.GetMemoryUnsafe(offset, viewLength, out this.m_ptr);
			}
			else
			{
				backedArrayHandle = array.GetReadOnlyMemoryUnsafe(offset, viewLength, out this.m_ptr);
			}
			this.m_handle = backedArrayHandle;
			this.m_start = offset;
			this.m_end = offset + viewLength;
		}

		// Token: 0x170012AC RID: 4780
		public unsafe T this[int i]
		{
			get
			{
				this.Cache(i);
				return this.m_ptr[(IntPtr)(i - this.m_start) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
			set
			{
				this.Cache(i);
				this.m_ptr[(IntPtr)(i - this.m_start) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
			}
		}

		// Token: 0x06009DDE RID: 40414 RVA: 0x003B9D48 File Offset: 0x003B7F48
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~View()
		{
			this.Dispose(false);
		}

		// Token: 0x06009DDF RID: 40415 RVA: 0x003B9D78 File Offset: 0x003B7F78
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009DE0 RID: 40416 RVA: 0x003B9D88 File Offset: 0x003B7F88
		[PublicizedFrom(EAccessModifier.Private)]
		public void Dispose(bool isDisposing)
		{
			object disposeLock = this.m_disposeLock;
			lock (disposeLock)
			{
				if (!isDisposing)
				{
					Log.Warning("View<T> is being finalized, it should be disposed properly.");
				}
				if (this.m_handle != null)
				{
					this.m_handle.Dispose();
					this.m_handle = null;
					this.m_ptr = null;
					this.m_start = 0;
					this.m_end = 0;
					Interlocked.Decrement(ref this.m_backedArraySingleView.m_viewCount);
				}
			}
		}

		// Token: 0x040077D2 RID: 30674
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArraySingleView<T> m_backedArraySingleView;

		// Token: 0x040077D3 RID: 30675
		public IBackedArrayHandle m_handle;

		// Token: 0x040077D4 RID: 30676
		public unsafe T* m_ptr;

		// Token: 0x040077D5 RID: 30677
		public int m_start;

		// Token: 0x040077D6 RID: 30678
		public int m_end;

		// Token: 0x040077D7 RID: 30679
		[PublicizedFrom(EAccessModifier.Private)]
		public object m_disposeLock = new object();
	}
}
