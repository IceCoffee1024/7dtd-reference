using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x020002CA RID: 714
	public abstract class PhraseRecognizer : IDisposable
	{
		// Token: 0x06001E75 RID: 7797
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		protected static extern IntPtr CreateFromKeywords(object self, [Unmarshalled] string[] keywords, ConfidenceLevel minimumConfidence);

		// Token: 0x06001E76 RID: 7798
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		protected static extern IntPtr CreateFromGrammarFile(object self, string grammarFilePath, ConfidenceLevel minimumConfidence);

		// Token: 0x06001E77 RID: 7799
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Start_Internal(IntPtr recognizer);

		// Token: 0x06001E78 RID: 7800
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Stop_Internal(IntPtr recognizer);

		// Token: 0x06001E79 RID: 7801
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsRunning_Internal(IntPtr recognizer);

		// Token: 0x06001E7A RID: 7802
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Destroy(IntPtr recognizer);

		// Token: 0x06001E7B RID: 7803
		[ThreadSafe]
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyThreaded(IntPtr recognizer);

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06001E7C RID: 7804 RVA: 0x000320A8 File Offset: 0x000302A8
		// (remove) Token: 0x06001E7D RID: 7805 RVA: 0x000320E0 File Offset: 0x000302E0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PhraseRecognizer.PhraseRecognizedDelegate OnPhraseRecognized;

		// Token: 0x06001E7E RID: 7806 RVA: 0x00009E2F File Offset: 0x0000802F
		internal PhraseRecognizer()
		{
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x00032118 File Offset: 0x00030318
		protected override void Finalize()
		{
			try
			{
				bool flag = this.m_Recognizer != IntPtr.Zero;
				if (flag)
				{
					PhraseRecognizer.DestroyThreaded(this.m_Recognizer);
					this.m_Recognizer = IntPtr.Zero;
					GC.SuppressFinalize(this);
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x00032178 File Offset: 0x00030378
		public void Start()
		{
			bool flag = this.m_Recognizer == IntPtr.Zero;
			if (!flag)
			{
				PhraseRecognizer.Start_Internal(this.m_Recognizer);
			}
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000321A8 File Offset: 0x000303A8
		public void Stop()
		{
			bool flag = this.m_Recognizer == IntPtr.Zero;
			if (!flag)
			{
				PhraseRecognizer.Stop_Internal(this.m_Recognizer);
			}
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000321D8 File Offset: 0x000303D8
		public void Dispose()
		{
			bool flag = this.m_Recognizer != IntPtr.Zero;
			if (flag)
			{
				PhraseRecognizer.Destroy(this.m_Recognizer);
				this.m_Recognizer = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001E83 RID: 7811 RVA: 0x0003221C File Offset: 0x0003041C
		public bool IsRunning
		{
			get
			{
				return this.m_Recognizer != IntPtr.Zero && PhraseRecognizer.IsRunning_Internal(this.m_Recognizer);
			}
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x00032250 File Offset: 0x00030450
		[RequiredByNativeCode]
		private void InvokePhraseRecognizedEvent(string text, ConfidenceLevel confidence, SemanticMeaning[] semanticMeanings, long phraseStartFileTime, long phraseDurationTicks)
		{
			PhraseRecognizer.PhraseRecognizedDelegate onPhraseRecognized = this.OnPhraseRecognized;
			bool flag = onPhraseRecognized != null;
			if (flag)
			{
				onPhraseRecognized(new PhraseRecognizedEventArgs(text, confidence, semanticMeanings, DateTime.FromFileTime(phraseStartFileTime), TimeSpan.FromTicks(phraseDurationTicks)));
			}
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0003228C File Offset: 0x0003048C
		[RequiredByNativeCode]
		private unsafe static SemanticMeaning[] MarshalSemanticMeaning(IntPtr keys, IntPtr values, IntPtr valueSizes, int valueCount)
		{
			SemanticMeaning[] array = new SemanticMeaning[valueCount];
			int num = 0;
			for (int i = 0; i < valueCount; i++)
			{
				uint num2 = *(uint*)((byte*)(void*)valueSizes + (IntPtr)i * 4);
				SemanticMeaning semanticMeaning = new SemanticMeaning
				{
					key = new string(*(IntPtr*)((byte*)(void*)keys + (IntPtr)i * (IntPtr)sizeof(char*))),
					values = new string[num2]
				};
				int num3 = 0;
				while ((long)num3 < (long)((ulong)num2))
				{
					semanticMeaning.values[num3] = new string(*(IntPtr*)((byte*)(void*)values + (IntPtr)(num + num3) * (IntPtr)sizeof(char*)));
					num3++;
				}
				array[i] = semanticMeaning;
				num += (int)num2;
			}
			return array;
		}

		// Token: 0x040009FE RID: 2558
		protected IntPtr m_Recognizer;

		// Token: 0x020002CB RID: 715
		// (Invoke) Token: 0x06001E87 RID: 7815
		public delegate void PhraseRecognizedDelegate(PhraseRecognizedEventArgs args);
	}
}
