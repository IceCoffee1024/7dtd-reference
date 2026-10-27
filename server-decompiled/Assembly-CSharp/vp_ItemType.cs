using System;
using UnityEngine;

// Token: 0x020015EC RID: 5612
[Serializable]
public class vp_ItemType : ScriptableObject
{
	// Token: 0x1700146C RID: 5228
	// (get) Token: 0x0600AF07 RID: 44807 RVA: 0x0041444E File Offset: 0x0041264E
	public string DisplayNameFull
	{
		get
		{
			return this.IndefiniteArticle + " " + this.DisplayName;
		}
	}

	// Token: 0x04008249 RID: 33353
	public string IndefiniteArticle = "a";

	// Token: 0x0400824A RID: 33354
	public string DisplayName;

	// Token: 0x0400824B RID: 33355
	public string Description;

	// Token: 0x0400824C RID: 33356
	public Texture2D Icon;

	// Token: 0x0400824D RID: 33357
	public float Space;
}
