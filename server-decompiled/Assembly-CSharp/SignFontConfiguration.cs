using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
[CreateAssetMenu]
public class SignFontConfiguration : ScriptableObject
{
	// Token: 0x04004001 RID: 16385
	public List<SignFontConfiguration.FontInfo> fontInfos = new List<SignFontConfiguration.FontInfo>();

	// Token: 0x02000AC4 RID: 2756
	[Serializable]
	public class FontInfo
	{
		// Token: 0x04004002 RID: 16386
		public string name;

		// Token: 0x04004003 RID: 16387
		public TMP_FontAsset fontAsset;
	}
}
