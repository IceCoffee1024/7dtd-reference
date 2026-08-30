using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000AB2 RID: 2738
[CreateAssetMenu]
public class SignFontConfiguration : ScriptableObject
{
	// Token: 0x04003FC6 RID: 16326
	public List<SignFontConfiguration.FontInfo> fontInfos = new List<SignFontConfiguration.FontInfo>();

	// Token: 0x02000AB3 RID: 2739
	[Serializable]
	public class FontInfo
	{
		// Token: 0x04003FC7 RID: 16327
		public string name;

		// Token: 0x04003FC8 RID: 16328
		public TMP_FontAsset fontAsset;
	}
}
