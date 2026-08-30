using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x02001372 RID: 4978
public sealed class BackedArraySingleView<[IsUnmanaged] T> : IBackedArrayView<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009CD1 RID: 40145 RVA: 0x003B46D0 File Offset: 0x003B28D0
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

	// Token: 0x06009CD2 RID: 40146 RVA: 0x003B471C File Offset: 0x003B291C
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

	// Token: 0x06009CD3 RID: 40147 RVA: 0x003B479C File Offset: 0x003B299C
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

	// Token: 0x06009CD4 RID: 40148 RVA: 0x003B481C File Offset: 0x003B2A1C
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009CD5 RID: 40149 RVA: 0x003B482C File Offset: 0x003B2A2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~BackedArraySingleView()
	{
		this.Dispose(false);
	}

	// Token: 0x06009CD6 RID: 40150 RVA: 0x003B485C File Offset: 0x003B2A5C
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

	// Token: 0x06009CD7 RID: 40151 RVA: 0x003B4871 File Offset: 0x003B2A71
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsDisposed()
	{
		return this.m_array == null;
	}

	// Token: 0x06009CD8 RID: 40152 RVA: 0x003B487C File Offset: 0x003B2A7C
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

	// Token: 0x17001270 RID: 4720
	// (get) Token: 0x06009CD9 RID: 40153 RVA: 0x003B4896 File Offset: 0x003B2A96
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x17001271 RID: 4721
	// (get) Token: 0x06009CDA RID: 40154 RVA: 0x003B489E File Offset: 0x003B2A9E
	public BackedArrayHandleMode Mode
	{
		get
		{
			return this.m_mode;
		}
	}

	// Token: 0x17001272 RID: 4722
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

	// Token: 0x06009CDD RID: 40157 RVA: 0x003B48D0 File Offset: 0x003B2AD0
	public void Flush()
	{
		foreach (BackedArraySingleView<T>.View view in this.m_views.Values)
		{
			view.m_handle.Flush();
		}
	}

	// Token: 0x06009CDE RID: 40158 RVA: 0x003B4924 File Offset: 0x003B2B24
	public static int GetDefaultViewLength(int length)
	{
		return Mathf.NextPowerOfTwo(4 * (int)Math.Sqrt((double)length));
	}

	// Token: 0x040076F3 RID: 30451
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArray<T> m_array;

	// Token: 0x040076F4 RID: 30452
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_length;

	// Token: 0x040076F5 RID: 30453
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BackedArrayHandleMode m_mode;

	// Token: 0x040076F6 RID: 30454
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_viewLength;

	// Token: 0x040076F7 RID: 30455
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<BackedArraySingleView<T>.View> m_views;

	// Token: 0x040076F8 RID: 30456
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_viewCount;

	// Token: 0x040076F9 RID: 30457
	[PublicizedFrom(EAccessModifier.Private)]
	public const int VIEW_COUNT_WARNING_THRESHOLD = 16;

	// Token: 0x02001373 RID: 4979
	[PublicizedFrom(EAccessModifier.Private)]
	public class View : IDisposable
	{
		// Token: 0x06009CDF RID: 40159 RVA: 0x003B4935 File Offset: 0x003B2B35
		public View(BackedArraySingleView<T> backedArraySingleView)
		{
			this.m_backedArraySingleView = backedArraySingleView;
		}

		// Token: 0x06009CE0 RID: 40160 RVA: 0x003B4950 File Offset: 0x003B2B50
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

		// Token: 0x17001273 RID: 4723
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

		// Token: 0x06009CE3 RID: 40163 RVA: 0x003B4A94 File Offset: 0x003B2C94
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~View()
		{
			this.Dispose(false);
		}

		// Token: 0x06009CE4 RID: 40164 RVA: 0x003B4AC4 File Offset: 0x003B2CC4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009CE5 RID: 40165 RVA: 0x003B4AD4 File Offset: 0x003B2CD4
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

		// Token: 0x040076FA RID: 30458
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArraySingleView<T> m_backedArraySingleView;

		// Token: 0x040076FB RID: 30459
		public IBackedArrayHandle m_handle;

		// Token: 0x040076FC RID: 30460
		public unsafe T* m_ptr;

		// Token: 0x040076FD RID: 30461
		public int m_start;

		// Token: 0x040076FE RID: 30462
		public int m_end;

		// Token: 0x040076FF RID: 30463
		[PublicizedFrom(EAccessModifier.Private)]
		public object m_disposeLock = new object();
	}
}
