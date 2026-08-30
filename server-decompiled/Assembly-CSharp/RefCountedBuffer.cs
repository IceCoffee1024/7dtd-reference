using System;

// Token: 0x02000067 RID: 103
public sealed class RefCountedBuffer : IDisposable
{
	// Token: 0x06000202 RID: 514 RVA: 0x00011408 File Offset: 0x0000F608
	[PublicizedFrom(EAccessModifier.Private)]
	public RefCountedBuffer(RefCountedBuffer.SharedBuffer buffer)
	{
		if (buffer == null || buffer.IsDisposed)
		{
			this.Dispose();
			return;
		}
		this.m_buffer = buffer;
		this.m_buffer.Increment();
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00011434 File Offset: 0x0000F634
	[PublicizedFrom(EAccessModifier.Private)]
	public void Dispose(bool disposing)
	{
		if (!disposing)
		{
			Log.Error("RefCountedBuffer is being finalized. It should be disposed properly.");
			return;
		}
		if (this.m_buffer == null)
		{
			return;
		}
		this.m_buffer.Decrement();
		this.m_buffer = null;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0001145F File Offset: 0x0000F65F
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00011470 File Offset: 0x0000F670
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~RefCountedBuffer()
	{
		this.Dispose(false);
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000206 RID: 518 RVA: 0x000114A0 File Offset: 0x0000F6A0
	public bool IsDisposed
	{
		get
		{
			return this.m_buffer == null || this.m_buffer.IsDisposed;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000207 RID: 519 RVA: 0x000114B8 File Offset: 0x0000F6B8
	public Span<byte> Span
	{
		get
		{
			RefCountedBuffer.SharedBuffer buffer = this.m_buffer;
			if (((buffer != null) ? buffer.BufferRaw : null) != null)
			{
				return this.m_buffer.BufferRaw.AsSpan<byte>(this.m_buffer.Offset, this.m_buffer.Length);
			}
			return Span<byte>.Empty;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000208 RID: 520 RVA: 0x00011508 File Offset: 0x0000F708
	public Memory<byte> Memory
	{
		get
		{
			RefCountedBuffer.SharedBuffer buffer = this.m_buffer;
			Memory<byte>? memory;
			if (buffer == null)
			{
				memory = null;
			}
			else
			{
				byte[] bufferRaw = buffer.BufferRaw;
				memory = ((bufferRaw != null) ? new Memory<byte>?(bufferRaw.AsMemory<byte>(this.m_buffer.Offset, this.m_buffer.Length)) : null);
			}
			Memory<byte>? memory2 = memory;
			if (memory2 == null)
			{
				return Memory<byte>.Empty;
			}
			return memory2.GetValueOrDefault();
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000209 RID: 521 RVA: 0x00011575 File Offset: 0x0000F775
	public byte[] BufferRaw
	{
		get
		{
			RefCountedBuffer.SharedBuffer buffer = this.m_buffer;
			return ((buffer != null) ? buffer.BufferRaw : null) ?? Array.Empty<byte>();
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x0600020A RID: 522 RVA: 0x00011592 File Offset: 0x0000F792
	public int Offset
	{
		get
		{
			RefCountedBuffer.SharedBuffer buffer = this.m_buffer;
			if (buffer == null)
			{
				return 0;
			}
			return buffer.Offset;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x0600020B RID: 523 RVA: 0x000115A5 File Offset: 0x0000F7A5
	public int Length
	{
		get
		{
			RefCountedBuffer.SharedBuffer buffer = this.m_buffer;
			if (buffer == null)
			{
				return 0;
			}
			return buffer.Length;
		}
	}

	// Token: 0x0600020C RID: 524 RVA: 0x000115B8 File Offset: 0x0000F7B8
	public RefCountedBuffer CreateRef()
	{
		RefCountedBuffer.SharedBuffer buffer = this.m_buffer;
		if (buffer == null)
		{
			return null;
		}
		return buffer.CreateRef();
	}

	// Token: 0x0600020D RID: 525 RVA: 0x000115CB File Offset: 0x0000F7CB
	public static RefCountedBuffer CreateFromExisting(byte[] buffer)
	{
		return RefCountedBuffer.CreateFromExisting(buffer, 0, buffer.Length);
	}

	// Token: 0x0600020E RID: 526 RVA: 0x000115D7 File Offset: 0x0000F7D7
	public static RefCountedBuffer CreateFromExisting(byte[] buffer, int offset, int length)
	{
		return new RefCountedBuffer.SharedBuffer(buffer, offset, length, false).CreateRef();
	}

	// Token: 0x0600020F RID: 527 RVA: 0x000115E7 File Offset: 0x0000F7E7
	public static RefCountedBuffer CreatePooled(int length)
	{
		return new RefCountedBuffer.SharedBuffer(SaveBufferPool.Instance.Alloc(length), 0, length, true).CreateRef();
	}

	// Token: 0x040002AD RID: 685
	[PublicizedFrom(EAccessModifier.Private)]
	public RefCountedBuffer.SharedBuffer m_buffer;

	// Token: 0x02000068 RID: 104
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class SharedBuffer : IDisposable
	{
		// Token: 0x06000210 RID: 528 RVA: 0x00011601 File Offset: 0x0000F801
		public SharedBuffer(byte[] bufferRaw, int offset, int length, bool pooled)
		{
			this.m_bufferRaw = bufferRaw;
			this.m_offset = offset;
			this.m_length = length;
			this.m_pooled = pooled;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00011634 File Offset: 0x0000F834
		[PublicizedFrom(EAccessModifier.Private)]
		public void Dispose(bool disposing)
		{
			if (!disposing)
			{
				Log.Error("RefCountedBuffer.SharedBuffer is being finalized. It should be disposed of properly by making use of RefCountedBuffer.");
				return;
			}
			object lockObj = this.m_lockObj;
			lock (lockObj)
			{
				if (this.m_bufferRaw != null)
				{
					if (this.m_refs != 0)
					{
						Log.Error(string.Format("{0} expected {1} to be 0 when disposing but was {2}.", "RefCountedBuffer", "m_refs", this.m_refs));
					}
					if (this.m_pooled)
					{
						SaveBufferPool.Instance.Free(this.m_bufferRaw);
					}
					this.m_bufferRaw = null;
					this.m_offset = 0;
					this.m_length = 0;
				}
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000116E0 File Offset: 0x0000F8E0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000116F0 File Offset: 0x0000F8F0
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~SharedBuffer()
		{
			this.Dispose(false);
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00011720 File Offset: 0x0000F920
		public bool IsDisposed
		{
			get
			{
				return this.m_bufferRaw == null;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0001172C File Offset: 0x0000F92C
		public void Increment()
		{
			object lockObj = this.m_lockObj;
			lock (lockObj)
			{
				if (this.m_bufferRaw == null)
				{
					throw new ObjectDisposedException("Shared Buffer has already been disposed.");
				}
				this.m_refs++;
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00011788 File Offset: 0x0000F988
		public void Decrement()
		{
			object lockObj = this.m_lockObj;
			lock (lockObj)
			{
				if (this.m_bufferRaw == null)
				{
					throw new ObjectDisposedException("Shared Buffer has already been disposed.");
				}
				this.m_refs--;
				if (this.m_refs <= 0)
				{
					this.Dispose();
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000117F4 File Offset: 0x0000F9F4
		public byte[] BufferRaw
		{
			get
			{
				return this.m_bufferRaw;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000218 RID: 536 RVA: 0x000117FC File Offset: 0x0000F9FC
		public int Offset
		{
			get
			{
				return this.m_offset;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00011804 File Offset: 0x0000FA04
		public int Length
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0001180C File Offset: 0x0000FA0C
		public RefCountedBuffer CreateRef()
		{
			return new RefCountedBuffer(this);
		}

		// Token: 0x040002AE RID: 686
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_lockObj = new object();

		// Token: 0x040002AF RID: 687
		[PublicizedFrom(EAccessModifier.Private)]
		public byte[] m_bufferRaw;

		// Token: 0x040002B0 RID: 688
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_offset;

		// Token: 0x040002B1 RID: 689
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_length;

		// Token: 0x040002B2 RID: 690
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_pooled;

		// Token: 0x040002B3 RID: 691
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_refs;
	}
}
