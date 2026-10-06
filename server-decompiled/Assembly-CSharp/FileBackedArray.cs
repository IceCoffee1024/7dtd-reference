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

// Token: 0x020013AE RID: 5038
public sealed class FileBackedArray<[IsUnmanaged] T> : IBackedArray<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009E8C RID: 40588 RVA: 0x003BE990 File Offset: 0x003BCB90
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

	// Token: 0x06009E8D RID: 40589 RVA: 0x003BEA68 File Offset: 0x003BCC68
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

	// Token: 0x06009E8E RID: 40590 RVA: 0x003BEB7C File Offset: 0x003BCD7C
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009E8F RID: 40591 RVA: 0x003BEB8C File Offset: 0x003BCD8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~FileBackedArray()
	{
		this.Dispose(false);
	}

	// Token: 0x06009E90 RID: 40592 RVA: 0x003BEBBC File Offset: 0x003BCDBC
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

	// Token: 0x06009E91 RID: 40593 RVA: 0x003BEBD4 File Offset: 0x003BCDD4
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

	// Token: 0x06009E92 RID: 40594 RVA: 0x003BEC34 File Offset: 0x003BCE34
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

	// Token: 0x06009E93 RID: 40595 RVA: 0x003BEC84 File Offset: 0x003BCE84
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

	// Token: 0x170012F2 RID: 4850
	// (get) Token: 0x06009E94 RID: 40596 RVA: 0x003BECCC File Offset: 0x003BCECC
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x06009E95 RID: 40597 RVA: 0x003BECD4 File Offset: 0x003BCED4
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<T>.FileBackedArrayHandle GetHandle(int start, int length, BackedArrayHandleMode mode)
	{
		this.CheckBounds(start, length);
		return new FileBackedArray<T>.FileBackedArrayHandle(this, this.m_valueSize, start, length, mode);
	}

	// Token: 0x06009E96 RID: 40598 RVA: 0x003BECF0 File Offset: 0x003BCEF0
	public IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		memory = handle.GetMemory();
		return handle;
	}

	// Token: 0x06009E97 RID: 40599 RVA: 0x003BED14 File Offset: 0x003BCF14
	public IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		memory = handle.GetReadOnlyMemory();
		return handle;
	}

	// Token: 0x06009E98 RID: 40600 RVA: 0x003BED38 File Offset: 0x003BCF38
	public unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		arrayPtr = handle.GetPtr();
		return handle;
	}

	// Token: 0x06009E99 RID: 40601 RVA: 0x003BED58 File Offset: 0x003BCF58
	public unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		arrayPtr = handle.GetPtr();
		return handle;
	}

	// Token: 0x06009E9A RID: 40602 RVA: 0x003BED78 File Offset: 0x003BCF78
	public IBackedArrayHandle GetSpan(int start, int length, out Span<T> span)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		span = handle.GetSpan();
		return handle;
	}

	// Token: 0x06009E9B RID: 40603 RVA: 0x003BED9C File Offset: 0x003BCF9C
	public IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		span = handle.GetReadOnlySpan();
		return handle;
	}

	// Token: 0x170012F3 RID: 4851
	// (get) Token: 0x06009E9C RID: 40604 RVA: 0x003BEDC0 File Offset: 0x003BCFC0
	// (set) Token: 0x06009E9D RID: 40605 RVA: 0x003BEDC8 File Offset: 0x003BCFC8
	public FileBackedArray<T>.OnWrittenHandler OnWritten
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x04007853 RID: 30803
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_STREAM_BUFFER_SIZE = 4096;

	// Token: 0x04007854 RID: 30804
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_length;

	// Token: 0x04007855 RID: 30805
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_valueSize;

	// Token: 0x04007856 RID: 30806
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_filePath;

	// Token: 0x04007857 RID: 30807
	[PublicizedFrom(EAccessModifier.Private)]
	public FileStream m_fileStream;

	// Token: 0x04007858 RID: 30808
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_fileStreamsLock = new object();

	// Token: 0x04007859 RID: 30809
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<FileStream> m_fileStreams;

	// Token: 0x020013AF RID: 5039
	// (Invoke) Token: 0x06009E9F RID: 40607
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate void OnWrittenHandler(int start, ReadOnlySpan<T> span);

	// Token: 0x020013B0 RID: 5040
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class FileBackedArrayMemoryManager : MemoryManager<T>
	{
		// Token: 0x06009EA2 RID: 40610 RVA: 0x003BEDD1 File Offset: 0x003BCFD1
		public unsafe FileBackedArrayMemoryManager(T* ptr, int length, int valueSize)
		{
			this.m_ptr = ptr;
			this.m_length = length;
			this.m_valueSize = valueSize;
		}

		// Token: 0x06009EA3 RID: 40611 RVA: 0x003BEDEE File Offset: 0x003BCFEE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void Dispose(bool disposing)
		{
			this.m_ptr = null;
			this.m_length = 0;
			this.m_valueSize = 0;
		}

		// Token: 0x06009EA4 RID: 40612 RVA: 0x003BEE06 File Offset: 0x003BD006
		public unsafe override Span<T> GetSpan()
		{
			return new Span<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x06009EA5 RID: 40613 RVA: 0x003BEE1C File Offset: 0x003BD01C
		public unsafe override MemoryHandle Pin(int elementIndex = 0)
		{
			return new MemoryHandle((void*)(this.m_ptr + (IntPtr)(elementIndex * this.m_valueSize) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), default(GCHandle), null);
		}

		// Token: 0x06009EA6 RID: 40614 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Unpin()
		{
		}

		// Token: 0x0400785B RID: 30811
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe T* m_ptr;

		// Token: 0x0400785C RID: 30812
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x0400785D RID: 30813
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_valueSize;
	}

	// Token: 0x020013B1 RID: 5041
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class FileBackedArrayHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009EA7 RID: 40615 RVA: 0x003BEE50 File Offset: 0x003BD050
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

		// Token: 0x06009EA8 RID: 40616 RVA: 0x003BEFB8 File Offset: 0x003BD1B8
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

		// Token: 0x06009EA9 RID: 40617 RVA: 0x003BF09C File Offset: 0x003BD29C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009EAA RID: 40618 RVA: 0x003BF0AC File Offset: 0x003BD2AC
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~FileBackedArrayHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06009EAB RID: 40619 RVA: 0x003BF0DC File Offset: 0x003BD2DC
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

		// Token: 0x06009EAC RID: 40620 RVA: 0x003BF0F1 File Offset: 0x003BD2F1
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_array == null;
		}

		// Token: 0x06009EAD RID: 40621 RVA: 0x003BF0FC File Offset: 0x003BD2FC
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

		// Token: 0x06009EAE RID: 40622 RVA: 0x003BF118 File Offset: 0x003BD318
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

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06009EAF RID: 40623 RVA: 0x003BF182 File Offset: 0x003BD382
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009EB0 RID: 40624 RVA: 0x003BF18C File Offset: 0x003BD38C
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

		// Token: 0x06009EB1 RID: 40625 RVA: 0x003BF1FA File Offset: 0x003BD3FA
		public void Flush()
		{
			this.FlushInternal();
		}

		// Token: 0x06009EB2 RID: 40626 RVA: 0x003BF202 File Offset: 0x003BD402
		public Memory<T> GetMemory()
		{
			if (this.m_memoryOwner == null)
			{
				this.m_memoryOwner = new FileBackedArray<T>.FileBackedArrayMemoryManager(this.m_ptr, this.m_length, this.m_valueSize);
			}
			return this.m_memoryOwner.Memory;
		}

		// Token: 0x06009EB3 RID: 40627 RVA: 0x003BF234 File Offset: 0x003BD434
		public ReadOnlyMemory<T> GetReadOnlyMemory()
		{
			if (this.m_memoryOwner == null)
			{
				this.m_memoryOwner = new FileBackedArray<T>.FileBackedArrayMemoryManager(this.m_ptr, this.m_length, this.m_valueSize);
			}
			return this.m_memoryOwner.Memory;
		}

		// Token: 0x06009EB4 RID: 40628 RVA: 0x003BF26B File Offset: 0x003BD46B
		public unsafe T* GetPtr()
		{
			return this.m_ptr;
		}

		// Token: 0x06009EB5 RID: 40629 RVA: 0x003BF273 File Offset: 0x003BD473
		public unsafe Span<T> GetSpan()
		{
			return new Span<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x06009EB6 RID: 40630 RVA: 0x003BF286 File Offset: 0x003BD486
		public unsafe ReadOnlySpan<T> GetReadOnlySpan()
		{
			return new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x0400785E RID: 30814
		[PublicizedFrom(EAccessModifier.Private)]
		public FileBackedArray<T> m_array;

		// Token: 0x0400785F RID: 30815
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int m_valueSize;

		// Token: 0x04007860 RID: 30816
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_start;

		// Token: 0x04007861 RID: 30817
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x04007862 RID: 30818
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;

		// Token: 0x04007863 RID: 30819
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeArray<T> m_buffer;

		// Token: 0x04007864 RID: 30820
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe T* m_ptr;

		// Token: 0x04007865 RID: 30821
		[PublicizedFrom(EAccessModifier.Private)]
		public IMemoryOwner<T> m_memoryOwner;
	}
}
