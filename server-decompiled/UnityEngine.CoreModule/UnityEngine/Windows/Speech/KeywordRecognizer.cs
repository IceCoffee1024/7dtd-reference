using System;
using System.Collections.Generic;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x020002D8 RID: 728
	public sealed class KeywordRecognizer : PhraseRecognizer
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x0003282A File Offset: 0x00030A2A
		// (set) Token: 0x06001EBF RID: 7871 RVA: 0x00032832 File Offset: 0x00030A32
		public IEnumerable<string> Keywords { get; private set; }

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0003283B File Offset: 0x00030A3B
		public KeywordRecognizer(string[] keywords)
			: this(keywords, ConfidenceLevel.Medium)
		{
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x00032848 File Offset: 0x00030A48
		public KeywordRecognizer(string[] keywords, ConfidenceLevel minimumConfidence)
		{
			bool flag = keywords == null;
			if (flag)
			{
				throw new ArgumentNullException("keywords");
			}
			bool flag2 = keywords.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("At least one keyword must be specified.", "keywords");
			}
			int num = keywords.Length;
			for (int i = 0; i < num; i++)
			{
				bool flag3 = keywords[i] == null;
				if (flag3)
				{
					throw new ArgumentNullException(string.Format("Keyword at index {0} is null.", i));
				}
			}
			this.Keywords = keywords;
			this.m_Recognizer = PhraseRecognizer.CreateFromKeywords(this, keywords, minimumConfidence);
		}
	}
}
