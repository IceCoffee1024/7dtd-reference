using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Platform;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x0200138E RID: 5006
public sealed class FileBackedArray<[IsUnmanaged] T> : IBackedArray<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009DE1 RID: 40417 RVA: 0x003B9E14 File Offset: 0x003B8014
	public FileBackedArray(int length)
	{
		if (length <= 0)
		{
			throw new ArgumentOutOfRangeException("length", length, "Length should be positive.");
		}
		this.m_length = length;
		this.m_valueSize = UnsafeUtility.SizeOf(typeof(T));
		this.m_filePath = PlatformManager.NativePlatform.Utils.GetTempFileName("fba", ".fba");
		this.m_fileStream = new FileStream(this.m_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, 4096, FileOptions.DeleteOnClose);
		this.m_fileStream.Seek((long)(length * this.m_valueSize - 1), SeekOrigin.Begin);
		this.m_fileStream.WriteByte(0);
		this.m_fileStream.Flush();
		this.m_fileStreams = new ThreadLocal<FileStream>(new Func<FileStream>(this.CreateFileStream), true);
	}

	// Token: 0x06009DE2 RID: 40418 RVA: 0x003B9EEC File Offset: 0x003B80EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Dispose(bool disposing)
	{
		if (!disposing)
		{
			Log.Error("FileBackedArray<T> is being finalized, it should be disposed properly.");
			return;
		}
		object fileStreamsLock = this.m_fileStreamsLock;
		lock (fileStreamsLock)
		{
			if (this.m_fileStreams != null)
			{
				foreach (FileStream fileStream in this.m_fileStreams.Values)
				{
					fileStream.Dispose();
				}
				this.m_fileStreams.Dispose();
				this.m_fileStreams = null;
			}
			if (this.m_fileStream != null)
			{
				this.m_fileStream.Dispose();
				this.m_fileStream = null;
			}
		}
		try
		{
			File.Delete(this.m_filePath);
		}
		catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
		{
			Log.Warning("FileBackedArray<T> Failed to delete: " + this.m_filePath);
		}
	}

	// Token: 0x06009DE3 RID: 40419 RVA: 0x003BA000 File Offset: 0x003B8200
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009DE4 RID: 40420 RVA: 0x003BA010 File Offset: 0x003B8210
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~FileBackedArray()
	{
		this.Dispose(false);
	}

	// Token: 0x06009DE5 RID: 40421 RVA: 0x003BA040 File Offset: 0x003B8240
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ThrowIfDisposed()
	{
		if (this.m_fileStream == null)
		{
			throw new ObjectDisposedException("FileBackedArray has already been disposed.");
		}
	}

	// Token: 0x06009DE6 RID: 40422 RVA: 0x003BA058 File Offset: 0x003B8258
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckBounds(int start, int length)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException(string.Format("Expected length to be non-negative but was {0}.", length));
		}
		if (start < 0 || start + length > this.m_length)
		{
			throw new ArgumentOutOfRangeException(string.Format("Expected requested range [{0}, {1}) to be a subset of [0, {2}).", start, start + length, this.m_length));
		}
	}

	// Token: 0x06009DE7 RID: 40423 RVA: 0x003BA0B8 File Offset: 0x003B82B8
	[PublicizedFrom(EAccessModifier.Private)]
	public FileStream CreateFileStream()
	{
		object fileStreamsLock = this.m_fileStreamsLock;
		FileStream fileStream;
		lock (fileStreamsLock)
		{
			fileStream = new FileStream(this.m_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read | FileShare.Write | FileShare.Delete, 4096);
		}
		return fileStream;
	}

	// Token: 0x06009DE8 RID: 40424 RVA: 0x003BA108 File Offset: 0x003B8308
	[PublicizedFrom(EAccessModifier.Private)]
	public FileStream GetFileStream()
	{
		object fileStreamsLock = this.m_fileStreamsLock;
		FileStream value;
		lock (fileStreamsLock)
		{
			value = this.m_fileStreams.Value;
		}
		return value;
	}

	// Token: 0x170012AD RID: 4781
	// (get) Token: 0x06009DE9 RID: 40425 RVA: 0x003BA150 File Offset: 0x003B8350
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x06009DEA RID: 40426 RVA: 0x003BA158 File Offset: 0x003B8358
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<T>.FileBackedArrayHandle GetHandle(int start, int length, BackedArrayHandleMode mode)
	{
		this.CheckBounds(start, length);
		return new FileBackedArray<T>.FileBackedArrayHandle(this, this.m_valueSize, start, length, mode);
	}

	// Token: 0x06009DEB RID: 40427 RVA: 0x003BA174 File Offset: 0x003B8374
	public IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		memory = handle.GetMemory();
		return handle;
	}

	// Token: 0x06009DEC RID: 40428 RVA: 0x003BA198 File Offset: 0x003B8398
	public IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		memory = handle.GetReadOnlyMemory();
		return handle;
	}

	// Token: 0x06009DED RID: 40429 RVA: 0x003BA1BC File Offset: 0x003B83BC
	public unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		arrayPtr = handle.GetPtr();
		return handle;
	}

	// Token: 0x06009DEE RID: 40430 RVA: 0x003BA1DC File Offset: 0x003B83DC
	public unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		arrayPtr = handle.GetPtr();
		return handle;
	}

	// Token: 0x06009DEF RID: 40431 RVA: 0x003BA1FC File Offset: 0x003B83FC
	public IBackedArrayHandle GetSpan(int start, int length, out Span<T> span)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		span = handle.GetSpan();
		return handle;
	}

	// Token: 0x06009DF0 RID: 40432 RVA: 0x003BA220 File Offset: 0x003B8420
	public IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		span = handle.GetReadOnlySpan();
		return handle;
	}

	// Token: 0x170012AE RID: 4782
	// (get) Token: 0x06009DF1 RID: 40433 RVA: 0x003BA244 File Offset: 0x003B8444
	// (set) Token: 0x06009DF2 RID: 40434 RVA: 0x003BA24C File Offset: 0x003B844C
	public FileBackedArray<T>.OnWrittenHandler OnWritten
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x040077D8 RID: 30680
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_STREAM_BUFFER_SIZE = 4096;

	// Token: 0x040077D9 RID: 30681
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_length;

	// Token: 0x040077DA RID: 30682
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_valueSize;

	// Token: 0x040077DB RID: 30683
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_filePath;

	// Token: 0x040077DC RID: 30684
	[PublicizedFrom(EAccessModifier.Private)]
	public FileStream m_fileStream;

	// Token: 0x040077DD RID: 30685
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_fileStreamsLock = new object();

	// Token: 0x040077DE RID: 30686
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<FileStream> m_fileStreams;

	// Token: 0x0200138F RID: 5007
	// (Invoke) Token: 0x06009DF4 RID: 40436
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate void OnWrittenHandler(int start, ReadOnlySpan<T> span);

	// Token: 0x02001390 RID: 5008
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class FileBackedArrayMemoryManager : MemoryManager<T>
	{
		// Token: 0x06009DF7 RID: 40439 RVA: 0x003BA255 File Offset: 0x003B8455
		public unsafe FileBackedArrayMemoryManager(T* ptr, int length, int valueSize)
		{
			this.m_ptr = ptr;
			this.m_length = length;
			this.m_valueSize = valueSize;
		}

		// Token: 0x06009DF8 RID: 40440 RVA: 0x003BA272 File Offset: 0x003B8472
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void Dispose(bool disposing)
		{
			this.m_ptr = null;
			this.m_length = 0;
			this.m_valueSize = 0;
		}

		// Token: 0x06009DF9 RID: 40441 RVA: 0x003BA28A File Offset: 0x003B848A
		public unsafe override Span<T> GetSpan()
		{
			return new Span<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x06009DFA RID: 40442 RVA: 0x003BA2A0 File Offset: 0x003B84A0
		public unsafe override MemoryHandle Pin(int elementIndex = 0)
		{
			return new MemoryHandle((void*)(this.m_ptr + (IntPtr)(elementIndex * this.m_valueSize) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), default(GCHandle), null);
		}

		// Token: 0x06009DFB RID: 40443 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Unpin()
		{
		}

		// Token: 0x040077E0 RID: 30688
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe T* m_ptr;

		// Token: 0x040077E1 RID: 30689
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x040077E2 RID: 30690
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_valueSize;
	}

	// Token: 0x02001391 RID: 5009
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class FileBackedArrayHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009DFC RID: 40444 RVA: 0x003BA2D4 File Offset: 0x003B84D4
		public unsafe FileBackedArrayHandle(FileBackedArray<T> array, int valueSize, int start, int length, BackedArrayHandleMode mode)
		{
			this.m_array = array;
			this.m_valueSize = valueSize;
			this.m_start = start;
			this.m_length = length;
			this.m_mode = mode;
			this.m_buffer = new NativeArray<T>(this.m_length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			BackedArrayHandleMode mode2 = this.m_mode;
			void* ptr;
			if (mode2 != BackedArrayHandleMode.ReadOnly)
			{
				if (mode2 != BackedArrayHandleMode.ReadWrite)
				{
					throw new ArgumentOutOfRangeException("mode", mode, string.Format("Unknown mode: {0}", mode));
				}
				ptr = this.m_buffer.GetUnsafePtr<T>();
			}
			else
			{
				ptr = this.m_buffer.GetUnsafeReadOnlyPtr<T>();
			}
			this.m_ptr = (T*)ptr;
			byte* ptr2 = (byte*)this.m_ptr;
			int num = this.m_length * this.m_valueSize;
			int i = 0;
			int num2 = this.m_start * this.m_valueSize;
			FileStream fileStream = this.m_array.GetFileStream();
			fileStream.Seek((long)num2, SeekOrigin.Begin);
			while (i < num)
			{
				int num3 = fileStream.Read(new Span<byte>((void*)(ptr2 + i), num - i));
				if (num3 <= 0)
				{
					this.m_buffer.Dispose();
					this.m_buffer = default(NativeArray<T>);
					throw new IOException(string.Format("Unexpected end of file (read {0} but expected {1} after offset {2}).", i, num, num2));
				}
				i += num3;
			}
			FileBackedArray<T> array2 = this.m_array;
			array2.OnWritten = (FileBackedArray<T>.OnWrittenHandler)Delegate.Combine(array2.OnWritten, new FileBackedArray<T>.OnWrittenHandler(this.OnWritten));
		}

		// Token: 0x06009DFD RID: 40445 RVA: 0x003BA43C File Offset: 0x003B863C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Dispose(bool disposing)
		{
			if (!disposing)
			{
				Log.Error("FileBackedArrayHandle is being finalized, it should be disposed properly.");
				return;
			}
			if (this.m_array != null)
			{
				FileBackedArray<T> array = this.m_array;
				array.OnWritten = (FileBackedArray<T>.OnWrittenHandler)Delegate.Remove(array.OnWritten, new FileBackedArray<T>.OnWrittenHandler(this.OnWritten));
				if (this.m_mode.CanWrite())
				{
					try
					{
						this.FlushInternal();
					}
					catch (Exception ex)
					{
						Log.Error("Failed to write potential changes back to the FileBackedArray file stream.");
						Log.Exception(ex);
					}
				}
			}
			if (this.m_memoryOwner != null)
			{
				this.m_memoryOwner.Dispose();
				this.m_memoryOwner = null;
			}
			if (this.m_buffer != default(NativeArray<T>))
			{
				this.m_buffer.Dispose();
				this.m_buffer = default(NativeArray<T>);
			}
			this.m_start = 0;
			this.m_length = 0;
			this.m_ptr = null;
			this.m_array = null;
		}

		// Token: 0x06009DFE RID: 40446 RVA: 0x003BA520 File Offset: 0x003B8720
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009DFF RID: 40447 RVA: 0x003BA530 File Offset: 0x003B8730
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~FileBackedArrayHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06009E00 RID: 40448 RVA: 0x003BA560 File Offset: 0x003B8760
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void ThrowIfDisposed()
		{
			if (this.IsDisposed())
			{
				throw new ObjectDisposedException("FileBackedArrayHandle has already been disposed.");
			}
		}

		// Token: 0x06009E01 RID: 40449 RVA: 0x003BA575 File Offset: 0x003B8775
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_array == null;
		}

		// Token: 0x06009E02 RID: 40450 RVA: 0x003BA580 File Offset: 0x003B8780
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void ThrowIfCannotWrite()
		{
			if (!this.m_mode.CanWrite())
			{
				throw new NotSupportedException("This FileBackedArrayHandle is not writable.");
			}
		}

		// Token: 0x06009E03 RID: 40451 RVA: 0x003BA59C File Offset: 0x003B879C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnWritten(int start, ReadOnlySpan<T> span)
		{
			int num = Math.Max(start, this.m_start);
			int num2 = Math.Min(start + span.Length, this.m_start + this.m_length) - num;
			if (num2 <= 0)
			{
				return;
			}
			ReadOnlySpan<T> readOnlySpan = span.Slice(num - start, num2);
			Span<T> span2 = this.GetSpan().Slice(num - this.m_start, num2);
			readOnlySpan.CopyTo(span2);
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06009E04 RID: 40452 RVA: 0x003BA606 File Offset: 0x003B8806
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009E05 RID: 40453 RVA: 0x003BA610 File Offset: 0x003B8810
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe void FlushInternal()
		{
			FileStream fileStream = this.m_array.GetFileStream();
			ReadOnlySpan<T> readOnlySpan = new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
			fileStream.Seek((long)(this.m_start * this.m_valueSize), SeekOrigin.Begin);
			fileStream.Write(MemoryMarshal.Cast<T, byte>(readOnlySpan));
			fileStream.Flush();
			FileBackedArray<T>.OnWrittenHandler onWritten = this.m_array.OnWritten;
			if (onWritten == null)
			{
				return;
			}
			onWritten(this.m_start, readOnlySpan);
		}

		// Token: 0x06009E06 RID: 40454 RVA: 0x003BA67E File Offset: 0x003B887E
		public void Flush()
		{
			this.FlushInternal();
		}

		// Token: 0x06009E07 RID: 40455 RVA: 0x003BA686 File Offset: 0x003B8886
		public Memory<T> GetMemory()
		{
			if (this.m_memoryOwner == null)
			{
				this.m_memoryOwner = new FileBackedArray<T>.FileBackedArrayMemoryManager(this.m_ptr, this.m_length, this.m_valueSize);
			}
			return this.m_memoryOwner.Memory;
		}

		// Token: 0x06009E08 RID: 40456 RVA: 0x003BA6B8 File Offset: 0x003B88B8
		public ReadOnlyMemory<T> GetReadOnlyMemory()
		{
			if (this.m_memoryOwner == null)
			{
				this.m_memoryOwner = new FileBackedArray<T>.FileBackedArrayMemoryManager(this.m_ptr, this.m_length, this.m_valueSize);
			}
			return this.m_memoryOwner.Memory;
		}

		// Token: 0x06009E09 RID: 40457 RVA: 0x003BA6EF File Offset: 0x003B88EF
		public unsafe T* GetPtr()
		{
			return this.m_ptr;
		}

		// Token: 0x06009E0A RID: 40458 RVA: 0x003BA6F7 File Offset: 0x003B88F7
		public unsafe Span<T> GetSpan()
		{
			return new Span<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x06009E0B RID: 40459 RVA: 0x003BA70A File Offset: 0x003B890A
		public unsafe ReadOnlySpan<T> GetReadOnlySpan()
		{
			return new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x040077E3 RID: 30691
		[PublicizedFrom(EAccessModifier.Private)]
		public FileBackedArray<T> m_array;

		// Token: 0x040077E4 RID: 30692
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int m_valueSize;

		// Token: 0x040077E5 RID: 30693
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_start;

		// Token: 0x040077E6 RID: 30694
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x040077E7 RID: 30695
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;

		// Token: 0x040077E8 RID: 30696
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeArray<T> m_buffer;

		// Token: 0x040077E9 RID: 30697
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe T* m_ptr;

		// Token: 0x040077EA RID: 30698
		[PublicizedFrom(EAccessModifier.Private)]
		public IMemoryOwner<T> m_memoryOwner;
	}
}
