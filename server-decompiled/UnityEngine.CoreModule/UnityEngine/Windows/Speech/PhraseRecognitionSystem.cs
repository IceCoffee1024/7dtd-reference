using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x020002C7 RID: 711
	public static class PhraseRecognitionSystem
	{
		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001E63 RID: 7779
		public static extern bool isSupported
		{
			[ThreadSafe]
			[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001E64 RID: 7780
		public static extern SpeechSystemStatus Status
		{
			[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001E65 RID: 7781
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Restart();

		// Token: 0x06001E66 RID: 7782
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Shutdown();

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001E67 RID: 7783 RVA: 0x00031F90 File Offset: 0x00030190
		// (remove) Token: 0x06001E68 RID: 7784 RVA: 0x00031FC4 File Offset: 0x000301C4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event PhraseRecognitionSystem.ErrorDelegate OnError;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06001E69 RID: 7785 RVA: 0x00031FF8 File Offset: 0x000301F8
		// (remove) Token: 0x06001E6A RID: 7786 RVA: 0x0003202C File Offset: 0x0003022C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event PhraseRecognitionSystem.StatusDelegate OnStatusChanged;

		// Token: 0x06001E6B RID: 7787 RVA: 0x00032060 File Offset: 0x00030260
		[RequiredByNativeCode]
		private static void PhraseRecognitionSystem_InvokeErrorEvent(SpeechError errorCode)
		{
			PhraseRecognitionSystem.ErrorDelegate onError = PhraseRecognitionSystem.OnError;
			bool flag = onError != null;
			if (flag)
			{
				onError(errorCode);
			}
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x00032084 File Offset: 0x00030284
		[RequiredByNativeCode]
		private static void PhraseRecognitionSystem_InvokeStatusChangedEvent(SpeechSystemStatus status)
		{
			PhraseRecognitionSystem.StatusDelegate onStatusChanged = PhraseRecognitionSystem.OnStatusChanged;
			bool flag = onStatusChanged != null;
			if (flag)
			{
				onStatusChanged(status);
			}
		}

		// Token: 0x020002C8 RID: 712
		// (Invoke) Token: 0x06001E6E RID: 7790
		public delegate void ErrorDelegate(SpeechError errorCode);

		// Token: 0x020002C9 RID: 713
		// (Invoke) Token: 0x06001E72 RID: 7794
		public delegate void StatusDelegate(SpeechSystemStatus status);
	}
}
