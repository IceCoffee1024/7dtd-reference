using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200142F RID: 5167
public static class GCUtils
{
	// Token: 0x0600A208 RID: 41480 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PreUnload()
	{
	}

	// Token: 0x0600A209 RID: 41481 RVA: 0x003CD143 File Offset: 0x003CB343
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PostUnload()
	{
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	// Token: 0x0600A20A RID: 41482 RVA: 0x003CD14F File Offset: 0x003CB34F
	public static void UnloadAndCollectStart()
	{
		ThreadManager.StartCoroutine(GCUtils.UnloadAndCollectCo());
	}

	// Token: 0x0600A20B RID: 41483 RVA: 0x003CD15C File Offset: 0x003CB35C
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

	// Token: 0x0600A20C RID: 41484 RVA: 0x003CD164 File Offset: 0x003CB364
	public static IEnumerator WaitForIdle()
	{
		while (GCUtils.m_working > 0)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040079F4 RID: 31220
	[PublicizedFrom(EAccessModifier.Private)]
	public static int m_working;
}
