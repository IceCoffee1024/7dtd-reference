using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x020002CC RID: 716
	public sealed class DictationRecognizer : IDisposable
	{
		// Token: 0x06001E8A RID: 7818
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(object self, ConfidenceLevel minimumConfidence, DictationTopicConstraint topicConstraint);

		// Token: 0x06001E8B RID: 7819
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Start(IntPtr self);

		// Token: 0x06001E8C RID: 7820
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Stop(IntPtr self);

		// Token: 0x06001E8D RID: 7821
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Destroy(IntPtr self);

		// Token: 0x06001E8E RID: 7822
		[ThreadSafe]
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyThreaded(IntPtr self);

		// Token: 0x06001E8F RID: 7823
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SpeechSystemStatus GetStatus(IntPtr self);

		// Token: 0x06001E90 RID: 7824
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetAutoSilenceTimeoutSeconds(IntPtr self);

		// Token: 0x06001E91 RID: 7825
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAutoSilenceTimeoutSeconds(IntPtr self, float value);

		// Token: 0x06001E92 RID: 7826
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetInitialSilenceTimeoutSeconds(IntPtr self);

		// Token: 0x06001E93 RID: 7827
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetInitialSilenceTimeoutSeconds(IntPtr self, float value);

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001E94 RID: 7828 RVA: 0x00032350 File Offset: 0x00030550
		// (remove) Token: 0x06001E95 RID: 7829 RVA: 0x00032388 File Offset: 0x00030588
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event DictationRecognizer.DictationHypothesisDelegate DictationHypothesis;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001E96 RID: 7830 RVA: 0x000323C0 File Offset: 0x000305C0
		// (remove) Token: 0x06001E97 RID: 7831 RVA: 0x000323F8 File Offset: 0x000305F8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event DictationRecognizer.DictationResultDelegate DictationResult;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001E98 RID: 7832 RVA: 0x00032430 File Offset: 0x00030630
		// (remove) Token: 0x06001E99 RID: 7833 RVA: 0x00032468 File Offset: 0x00030668
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event DictationRecognizer.DictationCompletedDelegate DictationComplete;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06001E9A RID: 7834 RVA: 0x000324A0 File Offset: 0x000306A0
		// (remove) Token: 0x06001E9B RID: 7835 RVA: 0x000324D8 File Offset: 0x000306D8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event DictationRecognizer.DictationErrorHandler DictationError;

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001E9C RID: 7836 RVA: 0x00032510 File Offset: 0x00030710
		public SpeechSystemStatus Status
		{
			get
			{
				return (this.m_Recognizer != IntPtr.Zero) ? DictationRecognizer.GetStatus(this.m_Recognizer) : SpeechSystemStatus.Stopped;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x00032544 File Offset: 0x00030744
		// (set) Token: 0x06001E9E RID: 7838 RVA: 0x00032580 File Offset: 0x00030780
		public float AutoSilenceTimeoutSeconds
		{
			get
			{
				bool flag = this.m_Recognizer == IntPtr.Zero;
				float num;
				if (flag)
				{
					num = 0f;
				}
				else
				{
					num = DictationRecognizer.GetAutoSilenceTimeoutSeconds(this.m_Recognizer);
				}
				return num;
			}
			set
			{
				bool flag = this.m_Recognizer == IntPtr.Zero;
				if (!flag)
				{
					DictationRecognizer.SetAutoSilenceTimeoutSeconds(this.m_Recognizer, value);
				}
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x000325B4 File Offset: 0x000307B4
		// (set) Token: 0x06001EA0 RID: 7840 RVA: 0x000325F0 File Offset: 0x000307F0
		public float InitialSilenceTimeoutSeconds
		{
			get
			{
				bool flag = this.m_Recognizer == IntPtr.Zero;
				float num;
				if (flag)
				{
					num = 0f;
				}
				else
				{
					num = DictationRecognizer.GetInitialSilenceTimeoutSeconds(this.m_Recognizer);
				}
				return num;
			}
			set
			{
				bool flag = this.m_Recognizer == IntPtr.Zero;
				if (!flag)
				{
					DictationRecognizer.SetInitialSilenceTimeoutSeconds(this.m_Recognizer, value);
				}
			}
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00032621 File Offset: 0x00030821
		public DictationRecognizer()
			: this(ConfidenceLevel.Medium, DictationTopicConstraint.Dictation)
		{
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0003262D File Offset: 0x0003082D
		public DictationRecognizer(ConfidenceLevel confidenceLevel)
			: this(confidenceLevel, DictationTopicConstraint.Dictation)
		{
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x00032639 File Offset: 0x00030839
		public DictationRecognizer(DictationTopicConstraint topic)
			: this(ConfidenceLevel.Medium, topic)
		{
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x00032645 File Offset: 0x00030845
		public DictationRecognizer(ConfidenceLevel minimumConfidence, DictationTopicConstraint topic)
		{
			this.m_Recognizer = DictationRecognizer.Create(this, minimumConfidence, topic);
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x00032660 File Offset: 0x00030860
		protected override void Finalize()
		{
			try
			{
				bool flag = this.m_Recognizer != IntPtr.Zero;
				if (flag)
				{
					DictationRecognizer.DestroyThreaded(this.m_Recognizer);
					this.m_Recognizer = IntPtr.Zero;
					GC.SuppressFinalize(this);
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000326C0 File Offset: 0x000308C0
		public void Start()
		{
			bool flag = this.m_Recognizer == IntPtr.Zero;
			if (!flag)
			{
				DictationRecognizer.Start(this.m_Recognizer);
			}
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000326F0 File Offset: 0x000308F0
		public void Stop()
		{
			bool flag = this.m_Recognizer == IntPtr.Zero;
			if (!flag)
			{
				DictationRecognizer.Stop(this.m_Recognizer);
			}
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00032720 File Offset: 0x00030920
		public void Dispose()
		{
			bool flag = this.m_Recognizer != IntPtr.Zero;
			if (flag)
			{
				DictationRecognizer.Destroy(this.m_Recognizer);
				this.m_Recognizer = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x00032764 File Offset: 0x00030964
		[RequiredByNativeCode]
		private void DictationRecognizer_InvokeHypothesisGeneratedEvent(string keyword)
		{
			DictationRecognizer.DictationHypothesisDelegate dictationHypothesis = this.DictationHypothesis;
			bool flag = dictationHypothesis != null;
			if (flag)
			{
				dictationHypothesis(keyword);
			}
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0003278C File Offset: 0x0003098C
		[RequiredByNativeCode]
		private void DictationRecognizer_InvokeResultGeneratedEvent(string keyword, ConfidenceLevel minimumConfidence)
		{
			DictationRecognizer.DictationResultDelegate dictationResult = this.DictationResult;
			bool flag = dictationResult != null;
			if (flag)
			{
				dictationResult(keyword, minimumConfidence);
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000327B4 File Offset: 0x000309B4
		[RequiredByNativeCode]
		private void DictationRecognizer_InvokeCompletedEvent(DictationCompletionCause cause)
		{
			DictationRecognizer.DictationCompletedDelegate dictationComplete = this.DictationComplete;
			bool flag = dictationComplete != null;
			if (flag)
			{
				dictationComplete(cause);
			}
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000327DC File Offset: 0x000309DC
		[RequiredByNativeCode]
		private void DictationRecognizer_InvokeErrorEvent(string error, int hresult)
		{
			DictationRecognizer.DictationErrorHandler dictationError = this.DictationError;
			bool flag = dictationError != null;
			if (flag)
			{
				dictationError(error, hresult);
			}
		}

		// Token: 0x04000A00 RID: 2560
		private IntPtr m_Recognizer;

		// Token: 0x020002CD RID: 717
		// (Invoke) Token: 0x06001EAE RID: 7854
		public delegate void DictationHypothesisDelegate(string text);

		// Token: 0x020002CE RID: 718
		// (Invoke) Token: 0x06001EB2 RID: 7858
		public delegate void DictationResultDelegate(string text, ConfidenceLevel confidence);

		// Token: 0x020002CF RID: 719
		// (Invoke) Token: 0x06001EB6 RID: 7862
		public delegate void DictationCompletedDelegate(DictationCompletionCause cause);

		// Token: 0x020002D0 RID: 720
		// (Invoke) Token: 0x06001EBA RID: 7866
		public delegate void DictationErrorHandler(string error, int hresult);
	}
}
