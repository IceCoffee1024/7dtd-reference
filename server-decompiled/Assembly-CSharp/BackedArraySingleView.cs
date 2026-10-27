using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x020013BE RID: 5054
public sealed class BackedArraySingleView<[IsUnmanaged] T> : IBackedArrayView<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009ECF RID: 40655 RVA: 0x003C0904 File Offset: 0x003BEB04
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

	// Token: 0x06009ED0 RID: 40656 RVA: 0x003C0950 File Offset: 0x003BEB50
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

	// Token: 0x06009ED1 RID: 40657 RVA: 0x003C09D0 File Offset: 0x003BEBD0
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

	// Token: 0x06009ED2 RID: 40658 RVA: 0x003C0A50 File Offset: 0x003BEC50
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009ED3 RID: 40659 RVA: 0x003C0A60 File Offset: 0x003BEC60
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~BackedArraySingleView()
	{
		this.Dispose(false);
	}

	// Token: 0x06009ED4 RID: 40660 RVA: 0x003C0A90 File Offset: 0x003BEC90
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

	// Token: 0x06009ED5 RID: 40661 RVA: 0x003C0AA5 File Offset: 0x003BECA5
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsDisposed()
	{
		return this.m_array == null;
	}

	// Token: 0x06009ED6 RID: 40662 RVA: 0x003C0AB0 File Offset: 0x003BECB0
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

	// Token: 0x170012FA RID: 4858
	// (get) Token: 0x06009ED7 RID: 40663 RVA: 0x003C0ACA File Offset: 0x003BECCA
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x170012FB RID: 4859
	// (get) Token: 0x06009ED8 RID: 40664 RVA: 0x003C0AD2 File Offset: 0x003BECD2
	public BackedArrayHandleMode Mode
	{
		get
		{
			return this.m_mode;
		}
	}

	// Token: 0x170012FC RID: 4860
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

	// Token: 0x06009EDB RID: 40667 RVA: 0x003C0B04 File Offset: 0x003BED04
	public void Flush()
	{
		foreach (BackedArraySingleView<T>.View view in this.m_views.Values)
		{
			view.m_handle.Flush();
		}
	}

	// Token: 0x06009EDC RID: 40668 RVA: 0x003C0B58 File Offset: 0x003BED58
	public static int GetDefaultViewLength(int length)
	{
		return Mathf.NextPowerOfTwo(4 * (int)Math.Sqrt((double)length));
	}

	// Token: 0x04007887 RID: 30855
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArray<T> m_array;

	// Token: 0x04007888 RID: 30856
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_length;

	// Token: 0x04007889 RID: 30857
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BackedArrayHandleMode m_mode;

	// Token: 0x0400788A RID: 30858
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_viewLength;

	// Token: 0x0400788B RID: 30859
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<BackedArraySingleView<T>.View> m_views;

	// Token: 0x0400788C RID: 30860
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_viewCount;

	// Token: 0x0400788D RID: 30861
	[PublicizedFrom(EAccessModifier.Private)]
	public const int VIEW_COUNT_WARNING_THRESHOLD = 16;

	// Token: 0x020013BF RID: 5055
	[PublicizedFrom(EAccessModifier.Private)]
	public class View : IDisposable
	{
		// Token: 0x06009EDD RID: 40669 RVA: 0x003C0B69 File Offset: 0x003BED69
		public View(BackedArraySingleView<T> backedArraySingleView)
		{
			this.m_backedArraySingleView = backedArraySingleView;
		}

		// Token: 0x06009EDE RID: 40670 RVA: 0x003C0B84 File Offset: 0x003BED84
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

		// Token: 0x170012FD RID: 4861
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

		// Token: 0x06009EE1 RID: 40673 RVA: 0x003C0CC8 File Offset: 0x003BEEC8
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~View()
		{
			this.Dispose(false);
		}

		// Token: 0x06009EE2 RID: 40674 RVA: 0x003C0CF8 File Offset: 0x003BEEF8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009EE3 RID: 40675 RVA: 0x003C0D08 File Offset: 0x003BEF08
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

		// Token: 0x0400788E RID: 30862
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArraySingleView<T> m_backedArraySingleView;

		// Token: 0x0400788F RID: 30863
		public IBackedArrayHandle m_handle;

		// Token: 0x04007890 RID: 30864
		public unsafe T* m_ptr;

		// Token: 0x04007891 RID: 30865
		public int m_start;

		// Token: 0x04007892 RID: 30866
		public int m_end;

		// Token: 0x04007893 RID: 30867
		[PublicizedFrom(EAccessModifier.Private)]
		public object m_disposeLock = new object();
	}
}
