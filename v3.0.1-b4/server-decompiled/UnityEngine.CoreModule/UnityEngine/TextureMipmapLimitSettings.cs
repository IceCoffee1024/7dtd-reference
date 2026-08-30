using System;

namespace UnityEngine
{
	// Token: 0x020001BC RID: 444
	public struct TextureMipmapLimitSettings
	{
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x00015D6F File Offset: 0x00013F6F
		// (set) Token: 0x06001011 RID: 4113 RVA: 0x00015D77 File Offset: 0x00013F77
		public TextureMipmapLimitBiasMode limitBiasMode { readonly get; set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x00015D80 File Offset: 0x00013F80
		// (set) Token: 0x06001013 RID: 4115 RVA: 0x00015D88 File Offset: 0x00013F88
		public int limitBias { readonly get; set; }
	}
}
