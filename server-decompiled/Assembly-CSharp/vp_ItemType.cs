using System;
using UnityEngine;

// Token: 0x020015B8 RID: 5560
[Serializable]
public class vp_ItemType : ScriptableObject
{
	// Token: 0x1700141B RID: 5147
	// (get) Token: 0x0600ADFC RID: 44540 RVA: 0x0040D1F6 File Offset: 0x0040B3F6
	public string DisplayNameFull
	{
		get
		{
			return this.IndefiniteArticle + " " + this.DisplayName;
		}
	}

	// Token: 0x04008185 RID: 33157
	public string IndefiniteArticle = "a";

	// Token: 0x04008186 RID: 33158
	public string DisplayName;

	// Token: 0x04008187 RID: 33159
	public string Description;

	// Token: 0x04008188 RID: 33160
	public Texture2D Icon;

	// Token: 0x04008189 RID: 33161
	public float Space;
}
