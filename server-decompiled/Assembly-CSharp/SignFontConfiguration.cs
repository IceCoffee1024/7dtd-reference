using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000AA1 RID: 2721
[CreateAssetMenu]
public class SignFontConfiguration : ScriptableObject
{
	// Token: 0x04003F35 RID: 16181
	public List<SignFontConfiguration.FontInfo> fontInfos = new List<SignFontConfiguration.FontInfo>();

	// Token: 0x02000AA2 RID: 2722
	[Serializable]
	public class FontInfo
	{
		// Token: 0x04003F36 RID: 16182
		public string name;

		// Token: 0x04003F37 RID: 16183
		public TMP_FontAsset fontAsset;
	}
}
