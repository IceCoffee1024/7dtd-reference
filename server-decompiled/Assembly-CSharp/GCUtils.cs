using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200140F RID: 5135
public static class GCUtils
{
	// Token: 0x0600A15D RID: 41309 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PreUnload()
	{
	}

	// Token: 0x0600A15E RID: 41310 RVA: 0x003C85A7 File Offset: 0x003C67A7
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PostUnload()
	{
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	// Token: 0x0600A15F RID: 41311 RVA: 0x003C85B3 File Offset: 0x003C67B3
	public static void UnloadAndCollectStart()
	{
		ThreadManager.StartCoroutine(GCUtils.UnloadAndCollectCo());
	}

	// Token: 0x0600A160 RID: 41312 RVA: 0x003C85C0 File Offset: 0x003C67C0
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

	// Token: 0x0600A161 RID: 41313 RVA: 0x003C85C8 File Offset: 0x003C67C8
	public static IEnumerator WaitForIdle()
	{
		while (GCUtils.m_working > 0)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04007979 RID: 31097
	[PublicizedFrom(EAccessModifier.Private)]
	public static int m_working;
}
