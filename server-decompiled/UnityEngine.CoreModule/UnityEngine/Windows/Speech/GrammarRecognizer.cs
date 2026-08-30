using System;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x020002D9 RID: 729
	public sealed class GrammarRecognizer : PhraseRecognizer
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x000328D9 File Offset: 0x00030AD9
		// (set) Token: 0x06001EC3 RID: 7875 RVA: 0x000328E1 File Offset: 0x00030AE1
		public string GrammarFilePath { get; private set; }

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000328EA File Offset: 0x00030AEA
		public GrammarRecognizer(string grammarFilePath)
			: this(grammarFilePath, ConfidenceLevel.Medium)
		{
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000328F8 File Offset: 0x00030AF8
		public GrammarRecognizer(string grammarFilePath, ConfidenceLevel minimumConfidence)
		{
			bool flag = grammarFilePath == null;
			if (flag)
			{
				throw new ArgumentNullException("grammarFilePath");
			}
			bool flag2 = grammarFilePath.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("Grammar file path cannot be empty.");
			}
			this.GrammarFilePath = grammarFilePath;
			this.m_Recognizer = PhraseRecognizer.CreateFromGrammarFile(this, grammarFilePath, minimumConfidence);
		}
	}
}
