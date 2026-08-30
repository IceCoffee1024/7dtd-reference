using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020013F4 RID: 5108
public static class GCUtils
{
	// Token: 0x0600A060 RID: 41056 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PreUnload()
	{
	}

	// Token: 0x0600A061 RID: 41057 RVA: 0x003C3263 File Offset: 0x003C1463
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PostUnload()
	{
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	// Token: 0x0600A062 RID: 41058 RVA: 0x003C326F File Offset: 0x003C146F
	public static void UnloadAndCollectStart()
	{
		ThreadManager.StartCoroutine(GCUtils.UnloadAndCollectCo());
	}

	// Token: 0x0600A063 RID: 41059 RVA: 0x003C327C File Offset: 0x003C147C
	public static IEnumerator UnloadAndCollectCo()
	{
		Interlocked.Increment(ref GCUtils.m_working);
		try
		{
			Task preUnload = Task.Run(new Action(GCUtils.PreUnload));
			yield return Resources.UnloadUnusedAssets();
			while (!preUnload.IsCompleted)
			{
				yield return null;
			}
			Task postUnload = Task.Run(new Action(GCUtils.PostUnload));
			while (!postUnload.IsCompleted)
			{
				yield return null;
			}
			preUnload = null;
			postUnload = null;
		}
		finally
		{
			Interlocked.Decrement(ref GCUtils.m_working);
		}
		yield break;
		yield break;
	}

	// Token: 0x0600A064 RID: 41060 RVA: 0x003C3284 File Offset: 0x003C1484
	public static IEnumerator WaitForIdle()
	{
		while (GCUtils.m_working > 0)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040078A0 RID: 30880
	[PublicizedFrom(EAccessModifier.Private)]
	public static int m_working;
}
