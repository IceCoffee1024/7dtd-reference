using System;
using UnityEngine;

// Token: 0x020015DA RID: 5594
[Serializable]
public class vp_ItemType : ScriptableObject
{
	// Token: 0x17001460 RID: 5216
	// (get) Token: 0x0600AEAF RID: 44719 RVA: 0x0041204A File Offset: 0x0041024A
	public string DisplayNameFull
	{
		get
		{
			return this.IndefiniteArticle + " " + this.DisplayName;
		}
	}

	// Token: 0x04008208 RID: 33288
	public string IndefiniteArticle = "a";

	// Token: 0x04008209 RID: 33289
	public string DisplayName;

	// Token: 0x0400820A RID: 33290
	public string Description;

	// Token: 0x0400820B RID: 33291
	public Texture2D Icon;

	// Token: 0x0400820C RID: 33292
	public float Space;
}
