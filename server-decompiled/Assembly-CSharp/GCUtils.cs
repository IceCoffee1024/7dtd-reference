using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02001441 RID: 5185
public static class GCUtils
{
	// Token: 0x0600A260 RID: 41568 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PreUnload()
	{
	}

	// Token: 0x0600A261 RID: 41569 RVA: 0x003CF547 File Offset: 0x003CD747
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PostUnload()
	{
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	// Token: 0x0600A262 RID: 41570 RVA: 0x003CF553 File Offset: 0x003CD753
	public static void UnloadAndCollectStart()
	{
		ThreadManager.StartCoroutine(GCUtils.UnloadAndCollectCo());
	}

	// Token: 0x0600A263 RID: 41571 RVA: 0x003CF560 File Offset: 0x003CD760
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

	// Token: 0x0600A264 RID: 41572 RVA: 0x003CF568 File Offset: 0x003CD768
	public static IEnumerator WaitForIdle()
	{
		while (GCUtils.m_working > 0)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04007A35 RID: 31285
	[PublicizedFrom(EAccessModifier.Private)]
	public static int m_working;
}
