using System;
using Unity.Profiling;

namespace UnityEngine
{
	// Token: 0x02000284 RID: 644
	public sealed class StaticBatchingUtility
	{
		// Token: 0x06001AD8 RID: 6872 RVA: 0x0002D594 File Offset: 0x0002B794
		public static void Combine(GameObject staticBatchRoot)
		{
			using (StaticBatchingUtility.s_CombineMarker.Auto())
			{
				StaticBatchingUtility.CombineRoot(staticBatchRoot);
			}
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x0002D5D8 File Offset: 0x0002B7D8
		public static void Combine(GameObject[] gos, GameObject staticBatchRoot)
		{
			using (StaticBatchingUtility.s_CombineMarker.Auto())
			{
				StaticBatchingHelper.CombineMeshes(gos, staticBatchRoot);
			}
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x0002D61C File Offset: 0x0002B81C
		private static void CombineRoot(GameObject staticBatchRoot)
		{
			bool flag = staticBatchRoot == null;
			MeshFilter[] array;
			if (flag)
			{
				array = (MeshFilter[])Object.FindObjectsOfType(typeof(MeshFilter));
			}
			else
			{
				array = staticBatchRoot.GetComponentsInChildren<MeshFilter>();
			}
			GameObject[] array2 = new GameObject[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].gameObject;
			}
			StaticBatchingHelper.CombineMeshes(array2, staticBatchRoot);
		}

		// Token: 0x04000933 RID: 2355
		internal static ProfilerMarker s_CombineMarker = new ProfilerMarker("StaticBatching.Combine");
	}
}
