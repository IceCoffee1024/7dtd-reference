using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x020013AC RID: 5036
public sealed class BackedArraySingleView<[IsUnmanaged] T> : IBackedArrayView<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009E77 RID: 40567 RVA: 0x003BE500 File Offset: 0x003BC700
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

	// Token: 0x06009E78 RID: 40568 RVA: 0x003BE54C File Offset: 0x003BC74C
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

	// Token: 0x06009E79 RID: 40569 RVA: 0x003BE5CC File Offset: 0x003BC7CC
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

	// Token: 0x06009E7A RID: 40570 RVA: 0x003BE64C File Offset: 0x003BC84C
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009E7B RID: 40571 RVA: 0x003BE65C File Offset: 0x003BC85C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~BackedArraySingleView()
	{
		this.Dispose(false);
	}

	// Token: 0x06009E7C RID: 40572 RVA: 0x003BE68C File Offset: 0x003BC88C
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

	// Token: 0x06009E7D RID: 40573 RVA: 0x003BE6A1 File Offset: 0x003BC8A1
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsDisposed()
	{
		return this.m_array == null;
	}

	// Token: 0x06009E7E RID: 40574 RVA: 0x003BE6AC File Offset: 0x003BC8AC
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

	// Token: 0x170012EE RID: 4846
	// (get) Token: 0x06009E7F RID: 40575 RVA: 0x003BE6C6 File Offset: 0x003BC8C6
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x170012EF RID: 4847
	// (get) Token: 0x06009E80 RID: 40576 RVA: 0x003BE6CE File Offset: 0x003BC8CE
	public BackedArrayHandleMode Mode
	{
		get
		{
			return this.m_mode;
		}
	}

	// Token: 0x170012F0 RID: 4848
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

	// Token: 0x06009E83 RID: 40579 RVA: 0x003BE700 File Offset: 0x003BC900
	public void Flush()
	{
		foreach (BackedArraySingleView<T>.View view in this.m_views.Values)
		{
			view.m_handle.Flush();
		}
	}

	// Token: 0x06009E84 RID: 40580 RVA: 0x003BE754 File Offset: 0x003BC954
	public static int GetDefaultViewLength(int length)
	{
		return Mathf.NextPowerOfTwo(4 * (int)Math.Sqrt((double)length));
	}

	// Token: 0x04007846 RID: 30790
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArray<T> m_array;

	// Token: 0x04007847 RID: 30791
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_length;

	// Token: 0x04007848 RID: 30792
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BackedArrayHandleMode m_mode;

	// Token: 0x04007849 RID: 30793
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_viewLength;

	// Token: 0x0400784A RID: 30794
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<BackedArraySingleView<T>.View> m_views;

	// Token: 0x0400784B RID: 30795
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_viewCount;

	// Token: 0x0400784C RID: 30796
	[PublicizedFrom(EAccessModifier.Private)]
	public const int VIEW_COUNT_WARNING_THRESHOLD = 16;

	// Token: 0x020013AD RID: 5037
	[PublicizedFrom(EAccessModifier.Private)]
	public class View : IDisposable
	{
		// Token: 0x06009E85 RID: 40581 RVA: 0x003BE765 File Offset: 0x003BC965
		public View(BackedArraySingleView<T> backedArraySingleView)
		{
			this.m_backedArraySingleView = backedArraySingleView;
		}

		// Token: 0x06009E86 RID: 40582 RVA: 0x003BE780 File Offset: 0x003BC980
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

		// Token: 0x170012F1 RID: 4849
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

		// Token: 0x06009E89 RID: 40585 RVA: 0x003BE8C4 File Offset: 0x003BCAC4
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~View()
		{
			this.Dispose(false);
		}

		// Token: 0x06009E8A RID: 40586 RVA: 0x003BE8F4 File Offset: 0x003BCAF4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009E8B RID: 40587 RVA: 0x003BE904 File Offset: 0x003BCB04
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

		// Token: 0x0400784D RID: 30797
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArraySingleView<T> m_backedArraySingleView;

		// Token: 0x0400784E RID: 30798
		public IBackedArrayHandle m_handle;

		// Token: 0x0400784F RID: 30799
		public unsafe T* m_ptr;

		// Token: 0x04007850 RID: 30800
		public int m_start;

		// Token: 0x04007851 RID: 30801
		public int m_end;

		// Token: 0x04007852 RID: 30802
		[PublicizedFrom(EAccessModifier.Private)]
		public object m_disposeLock = new object();
	}
}
