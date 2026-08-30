using System;
using UnityEngine;

// Token: 0x0200159D RID: 5533
[Serializable]
public class vp_ItemType : ScriptableObject
{
	// Token: 0x170013E2 RID: 5090
	// (get) Token: 0x0600ACFF RID: 44287 RVA: 0x00407EB6 File Offset: 0x004060B6
	public string DisplayNameFull
	{
		get
		{
			return this.IndefiniteArticle + " " + this.DisplayName;
		}
	}

	// Token: 0x040080AC RID: 32940
	public string IndefiniteArticle = "a";

	// Token: 0x040080AD RID: 32941
	public string DisplayName;

	// Token: 0x040080AE RID: 32942
	public string Description;

	// Token: 0x040080AF RID: 32943
	public Texture2D Icon;

	// Token: 0x040080B0 RID: 32944
	public float Space;
}
