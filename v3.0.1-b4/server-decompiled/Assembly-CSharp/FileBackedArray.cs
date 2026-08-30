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

// Token: 0x02001374 RID: 4980
public sealed class FileBackedArray<[IsUnmanaged] T> : IBackedArray<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009CE6 RID: 40166 RVA: 0x003B4B60 File Offset: 0x003B2D60
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

	// Token: 0x06009CE7 RID: 40167 RVA: 0x003B4C38 File Offset: 0x003B2E38
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

	// Token: 0x06009CE8 RID: 40168 RVA: 0x003B4D4C File Offset: 0x003B2F4C
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009CE9 RID: 40169 RVA: 0x003B4D5C File Offset: 0x003B2F5C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~FileBackedArray()
	{
		this.Dispose(false);
	}

	// Token: 0x06009CEA RID: 40170 RVA: 0x003B4D8C File Offset: 0x003B2F8C
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

	// Token: 0x06009CEB RID: 40171 RVA: 0x003B4DA4 File Offset: 0x003B2FA4
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

	// Token: 0x06009CEC RID: 40172 RVA: 0x003B4E04 File Offset: 0x003B3004
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

	// Token: 0x06009CED RID: 40173 RVA: 0x003B4E54 File Offset: 0x003B3054
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

	// Token: 0x17001274 RID: 4724
	// (get) Token: 0x06009CEE RID: 40174 RVA: 0x003B4E9C File Offset: 0x003B309C
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x06009CEF RID: 40175 RVA: 0x003B4EA4 File Offset: 0x003B30A4
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<T>.FileBackedArrayHandle GetHandle(int start, int length, BackedArrayHandleMode mode)
	{
		this.CheckBounds(start, length);
		return new FileBackedArray<T>.FileBackedArrayHandle(this, this.m_valueSize, start, length, mode);
	}

	// Token: 0x06009CF0 RID: 40176 RVA: 0x003B4EC0 File Offset: 0x003B30C0
	public IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		memory = handle.GetMemory();
		return handle;
	}

	// Token: 0x06009CF1 RID: 40177 RVA: 0x003B4EE4 File Offset: 0x003B30E4
	public IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		memory = handle.GetReadOnlyMemory();
		return handle;
	}

	// Token: 0x06009CF2 RID: 40178 RVA: 0x003B4F08 File Offset: 0x003B3108
	public unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		arrayPtr = handle.GetPtr();
		return handle;
	}

	// Token: 0x06009CF3 RID: 40179 RVA: 0x003B4F28 File Offset: 0x003B3128
	public unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		arrayPtr = handle.GetPtr();
		return handle;
	}

	// Token: 0x06009CF4 RID: 40180 RVA: 0x003B4F48 File Offset: 0x003B3148
	public IBackedArrayHandle GetSpan(int start, int length, out Span<T> span)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		span = handle.GetSpan();
		return handle;
	}

	// Token: 0x06009CF5 RID: 40181 RVA: 0x003B4F6C File Offset: 0x003B316C
	public IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		span = handle.GetReadOnlySpan();
		return handle;
	}

	// Token: 0x17001275 RID: 4725
	// (get) Token: 0x06009CF6 RID: 40182 RVA: 0x003B4F90 File Offset: 0x003B3190
	// (set) Token: 0x06009CF7 RID: 40183 RVA: 0x003B4F98 File Offset: 0x003B3198
	public FileBackedArray<T>.OnWrittenHandler OnWritten
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x04007700 RID: 30464
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_STREAM_BUFFER_SIZE = 4096;

	// Token: 0x04007701 RID: 30465
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_length;

	// Token: 0x04007702 RID: 30466
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_valueSize;

	// Token: 0x04007703 RID: 30467
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_filePath;

	// Token: 0x04007704 RID: 30468
	[PublicizedFrom(EAccessModifier.Private)]
	public FileStream m_fileStream;

	// Token: 0x04007705 RID: 30469
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_fileStreamsLock = new object();

	// Token: 0x04007706 RID: 30470
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<FileStream> m_fileStreams;

	// Token: 0x02001375 RID: 4981
	// (Invoke) Token: 0x06009CF9 RID: 40185
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate void OnWrittenHandler(int start, ReadOnlySpan<T> span);

	// Token: 0x02001376 RID: 4982
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class FileBackedArrayMemoryManager : MemoryManager<T>
	{
		// Token: 0x06009CFC RID: 40188 RVA: 0x003B4FA1 File Offset: 0x003B31A1
		public unsafe FileBackedArrayMemoryManager(T* ptr, int length, int valueSize)
		{
			this.m_ptr = ptr;
			this.m_length = length;
			this.m_valueSize = valueSize;
		}

		// Token: 0x06009CFD RID: 40189 RVA: 0x003B4FBE File Offset: 0x003B31BE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void Dispose(bool disposing)
		{
			this.m_ptr = null;
			this.m_length = 0;
			this.m_valueSize = 0;
		}

		// Token: 0x06009CFE RID: 40190 RVA: 0x003B4FD6 File Offset: 0x003B31D6
		public unsafe override Span<T> GetSpan()
		{
			return new Span<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x06009CFF RID: 40191 RVA: 0x003B4FEC File Offset: 0x003B31EC
		public unsafe override MemoryHandle Pin(int elementIndex = 0)
		{
			return new MemoryHandle((void*)(this.m_ptr + (IntPtr)(elementIndex * this.m_valueSize) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), default(GCHandle), null);
		}

		// Token: 0x06009D00 RID: 40192 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Unpin()
		{
		}

		// Token: 0x04007708 RID: 30472
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe T* m_ptr;

		// Token: 0x04007709 RID: 30473
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x0400770A RID: 30474
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_valueSize;
	}

	// Token: 0x02001377 RID: 4983
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class FileBackedArrayHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009D01 RID: 40193 RVA: 0x003B5020 File Offset: 0x003B3220
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

		// Token: 0x06009D02 RID: 40194 RVA: 0x003B5188 File Offset: 0x003B3388
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

		// Token: 0x06009D03 RID: 40195 RVA: 0x003B526C File Offset: 0x003B346C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009D04 RID: 40196 RVA: 0x003B527C File Offset: 0x003B347C
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~FileBackedArrayHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06009D05 RID: 40197 RVA: 0x003B52AC File Offset: 0x003B34AC
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

		// Token: 0x06009D06 RID: 40198 RVA: 0x003B52C1 File Offset: 0x003B34C1
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_array == null;
		}

		// Token: 0x06009D07 RID: 40199 RVA: 0x003B52CC File Offset: 0x003B34CC
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

		// Token: 0x06009D08 RID: 40200 RVA: 0x003B52E8 File Offset: 0x003B34E8
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

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06009D09 RID: 40201 RVA: 0x003B5352 File Offset: 0x003B3552
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009D0A RID: 40202 RVA: 0x003B535C File Offset: 0x003B355C
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

		// Token: 0x06009D0B RID: 40203 RVA: 0x003B53CA File Offset: 0x003B35CA
		public void Flush()
		{
			this.FlushInternal();
		}

		// Token: 0x06009D0C RID: 40204 RVA: 0x003B53D2 File Offset: 0x003B35D2
		public Memory<T> GetMemory()
		{
			if (this.m_memoryOwner == null)
			{
				this.m_memoryOwner = new FileBackedArray<T>.FileBackedArrayMemoryManager(this.m_ptr, this.m_length, this.m_valueSize);
			}
			return this.m_memoryOwner.Memory;
		}

		// Token: 0x06009D0D RID: 40205 RVA: 0x003B5404 File Offset: 0x003B3604
		public ReadOnlyMemory<T> GetReadOnlyMemory()
		{
			if (this.m_memoryOwner == null)
			{
				this.m_memoryOwner = new FileBackedArray<T>.FileBackedArrayMemoryManager(this.m_ptr, this.m_length, this.m_valueSize);
			}
			return this.m_memoryOwner.Memory;
		}

		// Token: 0x06009D0E RID: 40206 RVA: 0x003B543B File Offset: 0x003B363B
		public unsafe T* GetPtr()
		{
			return this.m_ptr;
		}

		// Token: 0x06009D0F RID: 40207 RVA: 0x003B5443 File Offset: 0x003B3643
		public unsafe Span<T> GetSpan()
		{
			return new Span<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x06009D10 RID: 40208 RVA: 0x003B5456 File Offset: 0x003B3656
		public unsafe ReadOnlySpan<T> GetReadOnlySpan()
		{
			return new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x0400770B RID: 30475
		[PublicizedFrom(EAccessModifier.Private)]
		public FileBackedArray<T> m_array;

		// Token: 0x0400770C RID: 30476
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int m_valueSize;

		// Token: 0x0400770D RID: 30477
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_start;

		// Token: 0x0400770E RID: 30478
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x0400770F RID: 30479
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;

		// Token: 0x04007710 RID: 30480
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeArray<T> m_buffer;

		// Token: 0x04007711 RID: 30481
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe T* m_ptr;

		// Token: 0x04007712 RID: 30482
		[PublicizedFrom(EAccessModifier.Private)]
		public IMemoryOwner<T> m_memoryOwner;
	}
}
