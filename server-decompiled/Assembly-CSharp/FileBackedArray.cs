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

// Token: 0x020013C0 RID: 5056
public sealed class FileBackedArray<[IsUnmanaged] T> : IBackedArray<T>, IDisposable where T : struct, ValueType
{
	// Token: 0x06009EE4 RID: 40676 RVA: 0x003C0D94 File Offset: 0x003BEF94
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

	// Token: 0x06009EE5 RID: 40677 RVA: 0x003C0E6C File Offset: 0x003BF06C
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

	// Token: 0x06009EE6 RID: 40678 RVA: 0x003C0F80 File Offset: 0x003BF180
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06009EE7 RID: 40679 RVA: 0x003C0F90 File Offset: 0x003BF190
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~FileBackedArray()
	{
		this.Dispose(false);
	}

	// Token: 0x06009EE8 RID: 40680 RVA: 0x003C0FC0 File Offset: 0x003BF1C0
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

	// Token: 0x06009EE9 RID: 40681 RVA: 0x003C0FD8 File Offset: 0x003BF1D8
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

	// Token: 0x06009EEA RID: 40682 RVA: 0x003C1038 File Offset: 0x003BF238
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

	// Token: 0x06009EEB RID: 40683 RVA: 0x003C1088 File Offset: 0x003BF288
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

	// Token: 0x170012FE RID: 4862
	// (get) Token: 0x06009EEC RID: 40684 RVA: 0x003C10D0 File Offset: 0x003BF2D0
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x06009EED RID: 40685 RVA: 0x003C10D8 File Offset: 0x003BF2D8
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<T>.FileBackedArrayHandle GetHandle(int start, int length, BackedArrayHandleMode mode)
	{
		this.CheckBounds(start, length);
		return new FileBackedArray<T>.FileBackedArrayHandle(this, this.m_valueSize, start, length, mode);
	}

	// Token: 0x06009EEE RID: 40686 RVA: 0x003C10F4 File Offset: 0x003BF2F4
	public IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		memory = handle.GetMemory();
		return handle;
	}

	// Token: 0x06009EEF RID: 40687 RVA: 0x003C1118 File Offset: 0x003BF318
	public IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		memory = handle.GetReadOnlyMemory();
		return handle;
	}

	// Token: 0x06009EF0 RID: 40688 RVA: 0x003C113C File Offset: 0x003BF33C
	public unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		arrayPtr = handle.GetPtr();
		return handle;
	}

	// Token: 0x06009EF1 RID: 40689 RVA: 0x003C115C File Offset: 0x003BF35C
	public unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		arrayPtr = handle.GetPtr();
		return handle;
	}

	// Token: 0x06009EF2 RID: 40690 RVA: 0x003C117C File Offset: 0x003BF37C
	public IBackedArrayHandle GetSpan(int start, int length, out Span<T> span)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadWrite);
		span = handle.GetSpan();
		return handle;
	}

	// Token: 0x06009EF3 RID: 40691 RVA: 0x003C11A0 File Offset: 0x003BF3A0
	public IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span)
	{
		FileBackedArray<T>.FileBackedArrayHandle handle = this.GetHandle(start, length, BackedArrayHandleMode.ReadOnly);
		span = handle.GetReadOnlySpan();
		return handle;
	}

	// Token: 0x170012FF RID: 4863
	// (get) Token: 0x06009EF4 RID: 40692 RVA: 0x003C11C4 File Offset: 0x003BF3C4
	// (set) Token: 0x06009EF5 RID: 40693 RVA: 0x003C11CC File Offset: 0x003BF3CC
	public FileBackedArray<T>.OnWrittenHandler OnWritten
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x04007894 RID: 30868
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_STREAM_BUFFER_SIZE = 4096;

	// Token: 0x04007895 RID: 30869
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_length;

	// Token: 0x04007896 RID: 30870
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_valueSize;

	// Token: 0x04007897 RID: 30871
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_filePath;

	// Token: 0x04007898 RID: 30872
	[PublicizedFrom(EAccessModifier.Private)]
	public FileStream m_fileStream;

	// Token: 0x04007899 RID: 30873
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_fileStreamsLock = new object();

	// Token: 0x0400789A RID: 30874
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<FileStream> m_fileStreams;

	// Token: 0x020013C1 RID: 5057
	// (Invoke) Token: 0x06009EF7 RID: 40695
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate void OnWrittenHandler(int start, ReadOnlySpan<T> span);

	// Token: 0x020013C2 RID: 5058
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class FileBackedArrayMemoryManager : MemoryManager<T>
	{
		// Token: 0x06009EFA RID: 40698 RVA: 0x003C11D5 File Offset: 0x003BF3D5
		public unsafe FileBackedArrayMemoryManager(T* ptr, int length, int valueSize)
		{
			this.m_ptr = ptr;
			this.m_length = length;
			this.m_valueSize = valueSize;
		}

		// Token: 0x06009EFB RID: 40699 RVA: 0x003C11F2 File Offset: 0x003BF3F2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void Dispose(bool disposing)
		{
			this.m_ptr = null;
			this.m_length = 0;
			this.m_valueSize = 0;
		}

		// Token: 0x06009EFC RID: 40700 RVA: 0x003C120A File Offset: 0x003BF40A
		public unsafe override Span<T> GetSpan()
		{
			return new Span<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x06009EFD RID: 40701 RVA: 0x003C1220 File Offset: 0x003BF420
		public unsafe override MemoryHandle Pin(int elementIndex = 0)
		{
			return new MemoryHandle((void*)(this.m_ptr + (IntPtr)(elementIndex * this.m_valueSize) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), default(GCHandle), null);
		}

		// Token: 0x06009EFE RID: 40702 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Unpin()
		{
		}

		// Token: 0x0400789C RID: 30876
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe T* m_ptr;

		// Token: 0x0400789D RID: 30877
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x0400789E RID: 30878
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_valueSize;
	}

	// Token: 0x020013C3 RID: 5059
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class FileBackedArrayHandle : IBackedArrayHandle, IDisposable
	{
		// Token: 0x06009EFF RID: 40703 RVA: 0x003C1254 File Offset: 0x003BF454
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

		// Token: 0x06009F00 RID: 40704 RVA: 0x003C13BC File Offset: 0x003BF5BC
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

		// Token: 0x06009F01 RID: 40705 RVA: 0x003C14A0 File Offset: 0x003BF6A0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06009F02 RID: 40706 RVA: 0x003C14B0 File Offset: 0x003BF6B0
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~FileBackedArrayHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06009F03 RID: 40707 RVA: 0x003C14E0 File Offset: 0x003BF6E0
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

		// Token: 0x06009F04 RID: 40708 RVA: 0x003C14F5 File Offset: 0x003BF6F5
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsDisposed()
		{
			return this.m_array == null;
		}

		// Token: 0x06009F05 RID: 40709 RVA: 0x003C1500 File Offset: 0x003BF700
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

		// Token: 0x06009F06 RID: 40710 RVA: 0x003C151C File Offset: 0x003BF71C
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

		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06009F07 RID: 40711 RVA: 0x003C1586 File Offset: 0x003BF786
		public BackedArrayHandleMode Mode
		{
			get
			{
				return this.m_mode;
			}
		}

		// Token: 0x06009F08 RID: 40712 RVA: 0x003C1590 File Offset: 0x003BF790
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

		// Token: 0x06009F09 RID: 40713 RVA: 0x003C15FE File Offset: 0x003BF7FE
		public void Flush()
		{
			this.FlushInternal();
		}

		// Token: 0x06009F0A RID: 40714 RVA: 0x003C1606 File Offset: 0x003BF806
		public Memory<T> GetMemory()
		{
			if (this.m_memoryOwner == null)
			{
				this.m_memoryOwner = new FileBackedArray<T>.FileBackedArrayMemoryManager(this.m_ptr, this.m_length, this.m_valueSize);
			}
			return this.m_memoryOwner.Memory;
		}

		// Token: 0x06009F0B RID: 40715 RVA: 0x003C1638 File Offset: 0x003BF838
		public ReadOnlyMemory<T> GetReadOnlyMemory()
		{
			if (this.m_memoryOwner == null)
			{
				this.m_memoryOwner = new FileBackedArray<T>.FileBackedArrayMemoryManager(this.m_ptr, this.m_length, this.m_valueSize);
			}
			return this.m_memoryOwner.Memory;
		}

		// Token: 0x06009F0C RID: 40716 RVA: 0x003C166F File Offset: 0x003BF86F
		public unsafe T* GetPtr()
		{
			return this.m_ptr;
		}

		// Token: 0x06009F0D RID: 40717 RVA: 0x003C1677 File Offset: 0x003BF877
		public unsafe Span<T> GetSpan()
		{
			return new Span<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x06009F0E RID: 40718 RVA: 0x003C168A File Offset: 0x003BF88A
		public unsafe ReadOnlySpan<T> GetReadOnlySpan()
		{
			return new ReadOnlySpan<T>((void*)this.m_ptr, this.m_length);
		}

		// Token: 0x0400789F RID: 30879
		[PublicizedFrom(EAccessModifier.Private)]
		public FileBackedArray<T> m_array;

		// Token: 0x040078A0 RID: 30880
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int m_valueSize;

		// Token: 0x040078A1 RID: 30881
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_start;

		// Token: 0x040078A2 RID: 30882
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x040078A3 RID: 30883
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BackedArrayHandleMode m_mode;

		// Token: 0x040078A4 RID: 30884
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeArray<T> m_buffer;

		// Token: 0x040078A5 RID: 30885
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe T* m_ptr;

		// Token: 0x040078A6 RID: 30886
		[PublicizedFrom(EAccessModifier.Private)]
		public IMemoryOwner<T> m_memoryOwner;
	}
}
