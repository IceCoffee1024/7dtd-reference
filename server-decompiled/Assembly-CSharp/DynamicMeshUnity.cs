using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x020003B4 RID: 948
public class DynamicMeshUnity
{
	// Token: 0x06001C27 RID: 7207 RVA: 0x000A7828 File Offset: 0x000A5A28
	public static int RoundChunk(int value)
	{
		if (value < 0)
		{
			value -= 15;
		}
		return value / 16 * 16;
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x000A783B File Offset: 0x000A5A3B
	public static int RoundChunk(float value)
	{
		return DynamicMeshUnity.RoundChunk((int)value);
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x000A7844 File Offset: 0x000A5A44
	public static string GetItemPath(long key)
	{
		return DynamicMeshFile.MeshLocation + key.ToString() + ".update";
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x000A783B File Offset: 0x000A5A3B
	public static int GetChunkPositionFromWorldPosition(float pos)
	{
		return DynamicMeshUnity.RoundChunk((int)pos);
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x000A785C File Offset: 0x000A5A5C
	public static int GetChunkPositionFromWorldPosition(int pos)
	{
		return DynamicMeshUnity.RoundChunk(pos);
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x000A7864 File Offset: 0x000A5A64
	public static long GetRegionKeyFromItemKey(long itemKey)
	{
		return DynamicMeshUnity.GetRegionKeyFromWorldPosition(DynamicMeshUnity.GetWorldPosFromKey(itemKey));
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x000A7871 File Offset: 0x000A5A71
	public static long GetRegionKeyFromWorldPosition(Vector3i pos)
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundRegion(pos.x)), World.toChunkXZ(DynamicMeshUnity.RoundRegion(pos.z)));
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x000A7898 File Offset: 0x000A5A98
	public static Vector2 GetXZFromKey(long key)
	{
		return new Vector2((float)(WorldChunkCache.extractX(key) * 16), (float)(WorldChunkCache.extractZ(key) * 16));
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x000A78B3 File Offset: 0x000A5AB3
	public static Vector3i GetWorldPosFromKey(long key)
	{
		return new Vector3i(WorldChunkCache.extractX(key) * 16, 0, WorldChunkCache.extractZ(key) * 16);
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x000A78CD File Offset: 0x000A5ACD
	public static string GetDebugPositionFromKey(long key)
	{
		return string.Format("{0},{1}", WorldChunkCache.extractX(key) * 16, WorldChunkCache.extractZ(key) * 16);
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x000A78CD File Offset: 0x000A5ACD
	public static string GetDebugPositionKey(long key)
	{
		return string.Format("{0},{1}", WorldChunkCache.extractX(key) * 16, WorldChunkCache.extractZ(key) * 16);
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000A78F5 File Offset: 0x000A5AF5
	public static int GetChunkSectionX(long key)
	{
		return WorldChunkCache.extractX(key);
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000A78FD File Offset: 0x000A5AFD
	public static int GetChunkSectionZ(long key)
	{
		return WorldChunkCache.extractZ(key);
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x000A7905 File Offset: 0x000A5B05
	public static int GetWorldXFromKey(long key)
	{
		return WorldChunkCache.extractX(key) * 16;
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x000A7910 File Offset: 0x000A5B10
	public static int GetWorldZFromKey(long key)
	{
		return WorldChunkCache.extractZ(key) * 16;
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000A791B File Offset: 0x000A5B1B
	public static long GetRegionKeyFromWorldPosition(int worldX, int worldZ)
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundRegion(worldX)), World.toChunkXZ(DynamicMeshUnity.RoundRegion(worldZ)));
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x000A7938 File Offset: 0x000A5B38
	public static Vector3i GetRegionPositionFromWorldPosition(int worldX, int worldZ)
	{
		return new Vector3i(DynamicMeshUnity.RoundRegion(worldX), 0, DynamicMeshUnity.RoundRegion(worldZ));
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x000A794C File Offset: 0x000A5B4C
	public static Vector3i GetRegionPositionFromWorldPosition(Vector3i worldPos)
	{
		return new Vector3i(DynamicMeshUnity.RoundRegion(worldPos.x), 0, DynamicMeshUnity.RoundRegion(worldPos.z));
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x000A796A File Offset: 0x000A5B6A
	public static int RoundRegion(int value)
	{
		if (value < 0)
		{
			value -= 159;
		}
		return value / 160 * 160;
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x000A7986 File Offset: 0x000A5B86
	public static int RoundRegion(float value)
	{
		return DynamicMeshUnity.RoundRegion((int)value);
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x000A798F File Offset: 0x000A5B8F
	public static Vector3i GetRegionPositionFromWorldPosition(Vector3 worldPos)
	{
		return new Vector3i(DynamicMeshUnity.RoundRegion(worldPos.x), 0, DynamicMeshUnity.RoundRegion(worldPos.z));
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x000A79AD File Offset: 0x000A5BAD
	public static long GetItemKey(int worldX, int worldZ)
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x000A79C0 File Offset: 0x000A5BC0
	public static int GetItemPosition(int pos)
	{
		return World.toChunkXZ(pos) * 16;
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x000A79CB File Offset: 0x000A5BCB
	public static float Distance(Vector3i a, Vector3i b)
	{
		return Mathf.Abs(Mathf.Sqrt(Mathf.Pow((float)(a.x - b.x), 2f) + Mathf.Pow((float)(a.z - b.z), 2f)));
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x000A7A08 File Offset: 0x000A5C08
	public static float Distance(Vector3i a, Vector3 b)
	{
		return Mathf.Abs(Mathf.Sqrt(Mathf.Pow((float)a.x - b.x, 2f) + Mathf.Pow((float)a.z - b.z, 2f)));
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x000A7A45 File Offset: 0x000A5C45
	public static float Distance(int x1, int y1, int x2, int y2)
	{
		return Mathf.Abs(Mathf.Sqrt(Mathf.Pow((float)(x1 - x2), 2f) + Mathf.Pow((float)(y1 - y2), 2f)));
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x000A7A6E File Offset: 0x000A5C6E
	public static void log(string msg)
	{
		if (DynamicMeshManager.ShowDebug && DynamicMeshManager.DoLog)
		{
			DynamicMeshManager.LogMsg(msg);
		}
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x000A33A3 File Offset: 0x000A15A3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogMsg(string msg)
	{
		if (DynamicMeshManager.DoLog)
		{
			DynamicMeshManager.LogMsg(msg);
		}
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x000A7A84 File Offset: 0x000A5C84
	public static Vector3i GetChunkPositionFromWorldPosition(Vector3i worldPosition)
	{
		return new Vector3i(DynamicMeshUnity.RoundChunk(worldPosition.x), worldPosition.y, DynamicMeshUnity.RoundChunk(worldPosition.z));
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x000A7AA8 File Offset: 0x000A5CA8
	public static bool IsInBuffer(float x, float z, int bufferSize, int xIndex, int zIndex)
	{
		int num = (int)(x / 160f);
		int num2 = (int)(z / 160f);
		if (x < 0f)
		{
			num--;
		}
		if (z < 0f)
		{
			num2--;
		}
		return zIndex >= num2 - bufferSize && zIndex <= num2 + bufferSize && xIndex >= num - bufferSize && xIndex <= num + bufferSize;
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x000A7B00 File Offset: 0x000A5D00
	public static void WaitCoroutine(IEnumerator func)
	{
		while (func.MoveNext())
		{
			if (func.Current != null)
			{
				IEnumerator enumerator;
				try
				{
					enumerator = (IEnumerator)func.Current;
				}
				catch (InvalidCastException)
				{
					if (func.Current.GetType() == typeof(WaitForSeconds))
					{
						Log.Warning("Skipped call to WaitForSeconds. Use WaitForSecondsRealtime instead.");
					}
					break;
				}
				DynamicMeshUnity.WaitCoroutine(enumerator);
			}
		}
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000A7B6C File Offset: 0x000A5D6C
	public static long GetMeshSize(GameObject go)
	{
		long num = 0L;
		MeshFilter component = go.GetComponent<MeshFilter>();
		if (component != null)
		{
			Mesh sharedMesh = component.sharedMesh;
			if (sharedMesh != null)
			{
				num += Profiler.GetRuntimeMemorySizeLong(sharedMesh);
			}
		}
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			num += DynamicMeshUnity.GetMeshSize(transform.gameObject);
		}
		return num;
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x000A7C00 File Offset: 0x000A5E00
	public static Bounds GetBoundsFromVertsJustY(ArrayListMP<Vector3> verts, Bounds bounds)
	{
		float num = verts[0].y;
		float num2 = verts[0].y;
		for (int i = 0; i < verts.Count; i++)
		{
			Vector3 vector = verts[i];
			num = Math.Min(num, vector.y);
			num2 = Math.Max(num2, vector.y);
		}
		Vector3 vector2 = new Vector3(0f, num, 0f);
		Vector3 vector3 = new Vector3(0f, num2, 0f);
		bounds.SetMinMax(vector2, vector3);
		return bounds;
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x000A7C90 File Offset: 0x000A5E90
	public static void DeleteDynamicMeshData(ICollection<long> chunks)
	{
		string text;
		HashSetLong hashSetLong;
		DynamicMeshUnity.GetOrCreateDynamicMeshChunksList(out text, out hashSetLong);
		if (!hashSetLong.Overlaps(chunks))
		{
			return;
		}
		if (DynamicMeshManager.Instance != null && GamePrefs.GetBool(EnumGamePrefs.DynamicMeshEnabled))
		{
			DynamicMeshUnity.tempRegions.Clear();
			foreach (long num in chunks)
			{
				if (hashSetLong.Contains(num))
				{
					DynamicMeshManager instance = DynamicMeshManager.Instance;
					DynamicMeshItem dynamicMeshItem = ((instance != null) ? instance.GetItemOrNull(num) : null);
					if (dynamicMeshItem == null)
					{
						Log.Error(string.Format("Failed to retrieve valid DynamicMeshItem for cached dynamic mesh chunk key: {0}.", num));
					}
					else
					{
						DynamicMeshManager.Instance.RemoveItem(dynamicMeshItem, true);
						string itemPath = DynamicMeshUnity.GetItemPath(num);
						if (SdFile.Exists(itemPath))
						{
							SdFile.Delete(itemPath);
						}
						DynamicMeshRegion region = dynamicMeshItem.GetRegion();
						DynamicMeshUnity.tempRegions.Add(region);
					}
				}
			}
			foreach (DynamicMeshRegion dynamicMeshRegion in DynamicMeshUnity.tempRegions)
			{
				dynamicMeshRegion.CleanUp();
				if (dynamicMeshRegion.LoadedItems.Count == 0 && dynamicMeshRegion.UnloadedItems.Count == 0)
				{
					string path = dynamicMeshRegion.Path;
					if (SdFile.Exists(path))
					{
						SdFile.Delete(path);
					}
				}
				else
				{
					DynamicMeshThread.AddRegionUpdateData(dynamicMeshRegion.WorldPosition.x, dynamicMeshRegion.WorldPosition.z, true);
				}
			}
			DynamicMeshUnity.tempRegions.Clear();
			return;
		}
		DynamicMeshUnity.tempRegionKeys.Clear();
		foreach (long num2 in chunks)
		{
			if (hashSetLong.Contains(num2))
			{
				string text2 = text + num2.ToString() + ".update";
				if (SdFile.Exists(text2))
				{
					SdFile.Delete(text2);
				}
				int num3 = WorldChunkCache.extractX(num2) * 16;
				int num4 = WorldChunkCache.extractZ(num2) * 16;
				long num5 = WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundRegion(num3)), World.toChunkXZ(DynamicMeshUnity.RoundRegion(num4)));
				DynamicMeshUnity.tempRegionKeys.Add(num5);
				hashSetLong.Remove(num2);
			}
		}
		foreach (long num6 in DynamicMeshUnity.tempRegionKeys)
		{
			string text3 = text + num6.ToString() + ".group";
			if (SdFile.Exists(text3))
			{
				SdFile.Delete(text3);
			}
		}
		DynamicMeshUnity.tempRegionKeys.Clear();
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x000A7F48 File Offset: 0x000A6148
	[PublicizedFrom(EAccessModifier.Private)]
	public static void GetOrCreateDynamicMeshChunksList(out string meshLocation, out HashSetLong keys)
	{
		string text = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? GameIO.GetSaveGameDir() : GameIO.GetSaveGameLocalDir());
		if (!DynamicMeshUnity.cachedDynamicMeshChunksList.Item1.StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
		{
			string text2 = text + "/DynamicMeshes/";
			DynamicMeshUnity.cachedDynamicMeshChunksList.Item1 = text2;
			DynamicMeshUnity.cachedDynamicMeshChunksList.Item2.Clear();
			SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(text2);
			if (sdDirectoryInfo.Exists)
			{
				using (IEnumerator<SdFileInfo> enumerator = sdDirectoryInfo.EnumerateFiles("*.update", SearchOption.TopDirectoryOnly).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						long num;
						if (long.TryParse(Path.GetFileNameWithoutExtension(enumerator.Current.Name), out num))
						{
							DynamicMeshUnity.cachedDynamicMeshChunksList.Item2.Add(num);
						}
					}
				}
			}
		}
		meshLocation = DynamicMeshUnity.cachedDynamicMeshChunksList.Item1;
		keys = DynamicMeshUnity.cachedDynamicMeshChunksList.Item2;
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000A8034 File Offset: 0x000A6234
	public static void AddDisabledImposterChunk(long key)
	{
		string text;
		HashSetLong hashSetLong;
		DynamicMeshUnity.GetOrCreateDynamicMeshChunksList(out text, out hashSetLong);
		hashSetLong.Add(key);
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x000A8054 File Offset: 0x000A6254
	public static void RemoveDisabledImposterChunk(long key)
	{
		string text;
		HashSetLong hashSetLong;
		DynamicMeshUnity.GetOrCreateDynamicMeshChunksList(out text, out hashSetLong);
		hashSetLong.Remove(key);
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x000A8072 File Offset: 0x000A6272
	public static void ClearCachedDynamicMeshChunksList()
	{
		DynamicMeshUnity.cachedDynamicMeshChunksList.Item1 = string.Empty;
		DynamicMeshUnity.cachedDynamicMeshChunksList.Item2.Clear();
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x000A8092 File Offset: 0x000A6292
	[Conditional("UNITY_STANDALONE")]
	public static void EnsureDMDirectoryExists()
	{
		if (!SdDirectory.Exists(DynamicMeshFile.MeshLocation))
		{
			SdDirectory.CreateDirectory(DynamicMeshFile.MeshLocation);
		}
	}

	// Token: 0x0400125C RID: 4700
	public const int RegionSize = 160;

	// Token: 0x0400125D RID: 4701
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<DynamicMeshRegion> tempRegions = new HashSet<DynamicMeshRegion>();

	// Token: 0x0400125E RID: 4702
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<long> tempRegionKeys = new HashSet<long>();

	// Token: 0x0400125F RID: 4703
	[TupleElementNames(new string[] { "path", "keys" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public static ValueTuple<string, HashSetLong> cachedDynamicMeshChunksList = new ValueTuple<string, HashSetLong>(string.Empty, new HashSetLong());
}
