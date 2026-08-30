using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Apple
{
	// Token: 0x020004FA RID: 1274
	[NativeHeader("Runtime/Export/Apple/FrameCaptureMetalScriptBindings.h")]
	[NativeConditional("PLATFORM_APPLE")]
	public class FrameCapture
	{
		// Token: 0x06002C6D RID: 11373 RVA: 0x00009E2F File Offset: 0x0000802F
		private FrameCapture()
		{
		}

		// Token: 0x06002C6E RID: 11374
		[FreeFunction("FrameCaptureMetalScripting::IsDestinationSupported")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDestinationSupportedImpl(FrameCaptureDestination dest);

		// Token: 0x06002C6F RID: 11375
		[FreeFunction("FrameCaptureMetalScripting::BeginCapture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void BeginCaptureImpl(FrameCaptureDestination dest, string path);

		// Token: 0x06002C70 RID: 11376
		[FreeFunction("FrameCaptureMetalScripting::EndCapture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EndCaptureImpl();

		// Token: 0x06002C71 RID: 11377
		[FreeFunction("FrameCaptureMetalScripting::CaptureNextFrame")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CaptureNextFrameImpl(FrameCaptureDestination dest, string path);

		// Token: 0x06002C72 RID: 11378 RVA: 0x0004A974 File Offset: 0x00048B74
		public static bool IsDestinationSupported(FrameCaptureDestination dest)
		{
			bool flag = dest != FrameCaptureDestination.DevTools && dest != FrameCaptureDestination.GPUTraceDocument;
			if (flag)
			{
				throw new ArgumentException("dest", "Argument dest has bad value (not one of FrameCaptureDestination enum values)");
			}
			return FrameCapture.IsDestinationSupportedImpl(dest);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x0004A9B0 File Offset: 0x00048BB0
		public static void BeginCaptureToXcode()
		{
			bool flag = !FrameCapture.IsDestinationSupported(FrameCaptureDestination.DevTools);
			if (flag)
			{
				throw new InvalidOperationException("Frame Capture with DevTools is not supported.");
			}
			FrameCapture.BeginCaptureImpl(FrameCaptureDestination.DevTools, null);
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x0004A9E0 File Offset: 0x00048BE0
		public static void BeginCaptureToFile(string path)
		{
			bool flag = !FrameCapture.IsDestinationSupported(FrameCaptureDestination.GPUTraceDocument);
			if (flag)
			{
				throw new InvalidOperationException("Frame Capture to file is not supported.");
			}
			bool flag2 = string.IsNullOrEmpty(path);
			if (flag2)
			{
				throw new ArgumentException("path", "Path must be supplied when capture destination is GPUTraceDocument.");
			}
			bool flag3 = Path.GetExtension(path) != ".gputrace";
			if (flag3)
			{
				throw new ArgumentException("path", "Destination file should have .gputrace extension.");
			}
			FrameCapture.BeginCaptureImpl(FrameCaptureDestination.GPUTraceDocument, new Uri(path).AbsoluteUri);
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x0004AA56 File Offset: 0x00048C56
		public static void EndCapture()
		{
			FrameCapture.EndCaptureImpl();
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x0004AA60 File Offset: 0x00048C60
		public static void CaptureNextFrameToXcode()
		{
			bool flag = !FrameCapture.IsDestinationSupported(FrameCaptureDestination.DevTools);
			if (flag)
			{
				throw new InvalidOperationException("Frame Capture with DevTools is not supported.");
			}
			FrameCapture.CaptureNextFrameImpl(FrameCaptureDestination.DevTools, null);
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x0004AA90 File Offset: 0x00048C90
		public static void CaptureNextFrameToFile(string path)
		{
			bool flag = !FrameCapture.IsDestinationSupported(FrameCaptureDestination.GPUTraceDocument);
			if (flag)
			{
				throw new InvalidOperationException("Frame Capture to file is not supported.");
			}
			bool flag2 = string.IsNullOrEmpty(path);
			if (flag2)
			{
				throw new ArgumentException("path", "Path must be supplied when capture destination is GPUTraceDocument.");
			}
			bool flag3 = Path.GetExtension(path) != ".gputrace";
			if (flag3)
			{
				throw new ArgumentException("path", "Destination file should have .gputrace extension.");
			}
			FrameCapture.CaptureNextFrameImpl(FrameCaptureDestination.GPUTraceDocument, new Uri(path).AbsoluteUri);
		}
	}
}
