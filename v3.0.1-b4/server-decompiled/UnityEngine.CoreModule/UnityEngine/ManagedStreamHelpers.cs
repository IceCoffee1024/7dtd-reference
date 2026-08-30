using System;
using System.IO;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000250 RID: 592
	internal static class ManagedStreamHelpers
	{
		// Token: 0x06001937 RID: 6455 RVA: 0x0002A228 File Offset: 0x00028428
		internal static void ValidateLoadFromStream(Stream stream)
		{
			bool flag = stream == null;
			if (flag)
			{
				throw new ArgumentNullException("ManagedStream object must be non-null", "stream");
			}
			bool flag2 = !stream.CanRead;
			if (flag2)
			{
				throw new ArgumentException("ManagedStream object must be readable (stream.CanRead must return true)", "stream");
			}
			bool flag3 = !stream.CanSeek;
			if (flag3)
			{
				throw new ArgumentException("ManagedStream object must be seekable (stream.CanSeek must return true)", "stream");
			}
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0002A288 File Offset: 0x00028488
		[RequiredByNativeCode]
		internal unsafe static void ManagedStreamRead(byte[] buffer, int offset, int count, Stream stream, IntPtr returnValueAddress)
		{
			bool flag = returnValueAddress == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
			}
			ManagedStreamHelpers.ValidateLoadFromStream(stream);
			*(int*)(void*)returnValueAddress = stream.Read(buffer, offset, count);
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0002A2D0 File Offset: 0x000284D0
		[RequiredByNativeCode]
		internal unsafe static void ManagedStreamSeek(long offset, uint origin, Stream stream, IntPtr returnValueAddress)
		{
			bool flag = returnValueAddress == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
			}
			ManagedStreamHelpers.ValidateLoadFromStream(stream);
			*(long*)(void*)returnValueAddress = stream.Seek(offset, (SeekOrigin)origin);
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0002A314 File Offset: 0x00028514
		[RequiredByNativeCode]
		internal unsafe static void ManagedStreamLength(Stream stream, IntPtr returnValueAddress)
		{
			bool flag = returnValueAddress == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
			}
			ManagedStreamHelpers.ValidateLoadFromStream(stream);
			*(long*)(void*)returnValueAddress = stream.Length;
		}
	}
}
