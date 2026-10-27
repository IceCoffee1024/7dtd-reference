using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000ACD RID: 2765
[CreateAssetMenu]
public class SignFontConfiguration : ScriptableObject
{
	// Token: 0x04004022 RID: 16418
	public List<SignFontConfiguration.FontInfo> fontInfos = new List<SignFontConfiguration.FontInfo>();

	// Token: 0x02000ACE RID: 2766
	[Serializable]
	public class FontInfo
	{
		// Token: 0x04004023 RID: 16419
		public string name;

		// Token: 0x04004024 RID: 16420
		public TMP_FontAsset fontAsset;
	}
}
