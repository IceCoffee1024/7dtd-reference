using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class MicroSplatKeywords : ScriptableObject
{
	// Token: 0x06000113 RID: 275 RVA: 0x0000C797 File Offset: 0x0000A997
	public bool IsKeywordEnabled(string k)
	{
		return this.keywords.Contains(k);
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000C7A5 File Offset: 0x0000A9A5
	public void EnableKeyword(string k)
	{
		if (!this.IsKeywordEnabled(k))
		{
			this.keywords.Add(k);
		}
	}

	// Token: 0x06000115 RID: 277 RVA: 0x0000C7BC File Offset: 0x0000A9BC
	public void DisableKeyword(string k)
	{
		if (this.IsKeywordEnabled(k))
		{
			this.keywords.Remove(k);
		}
	}

	// Token: 0x04000146 RID: 326
	public List<string> keywords = new List<string>();

	// Token: 0x04000147 RID: 327
	public int drawOrder = 100;
}
