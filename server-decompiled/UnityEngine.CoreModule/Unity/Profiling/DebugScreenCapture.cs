using System;
using Unity.Collections;
using UnityEngine;

namespace Unity.Profiling
{
	// Token: 0x02000068 RID: 104
	public struct DebugScreenCapture
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000039A1 File Offset: 0x00001BA1
		// (set) Token: 0x06000193 RID: 403 RVA: 0x000039A9 File Offset: 0x00001BA9
		public NativeArray<byte> RawImageDataReference { readonly get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000039B2 File Offset: 0x00001BB2
		// (set) Token: 0x06000195 RID: 405 RVA: 0x000039BA File Offset: 0x00001BBA
		public TextureFormat ImageFormat { readonly get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000039C3 File Offset: 0x00001BC3
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000039CB File Offset: 0x00001BCB
		public int Width { readonly get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000039D4 File Offset: 0x00001BD4
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000039DC File Offset: 0x00001BDC
		public int Height { readonly get; set; }
	}
}
