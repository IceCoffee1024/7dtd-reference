using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x020003AB RID: 939
public class DynamicMeshUnity
{
	// Token: 0x06001C0D RID: 7181 RVA: 0x000A6C98 File Offset: 0x000A4E98
	public static int RoundChunk(int value)
	{
		if (value < 0)
		{
			value -= 15;
		}
		return value / 16 * 16;
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x000A6CAB File Offset: 0x000A4EAB
	public static int RoundChunk(float value)
	{
		return DynamicMeshUnity.RoundChunk((int)value);
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x000A6CB4 File Offset: 0x000A4EB4
	public static string GetItemPath(long key)
	{
		return DynamicMeshFile.MeshLocation + key.ToString() + ".update";
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x000A6CAB File Offset: 0x000A4EAB
	public static int GetChunkPositionFromWorldPosition(float pos)
	{
		return DynamicMeshUnity.RoundChunk((int)pos);
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x000A6CCC File Offset: 0x000A4ECC
	public static int GetChunkPositionFromWorldPosition(int pos)
	{
		return DynamicMeshUnity.RoundChunk(pos);
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x000A6CD4 File Offset: 0x000A4ED4
	public static long GetRegionKeyFromItemKey(long itemKey)
	{
		return DynamicMeshUnity.GetRegionKeyFromWorldPosition(DynamicMeshUnity.GetWorldPosFromKey(itemKey));
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x000A6CE1 File Offset: 0x000A4EE1
	public static long GetRegionKeyFromWorldPosition(Vector3i pos)
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundRegion(pos.x)), World.toChunkXZ(DynamicMeshUnity.RoundRegion(pos.z)));
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x000A6D08 File Offset: 0x000A4F08
	public static Vector2 GetXZFromKey(long key)
	{
		return new Vector2((float)(WorldChunkCache.extractX(key) * 16), (float)(WorldChunkCache.extractZ(key) * 16));
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x000A6D23 File Offset: 0x000A4F23
	public static Vector3i GetWorldPosFromKey(long key)
	{
		return new Vector3i(WorldChunkCache.extractX(key) * 16, 0, WorldChunkCache.extractZ(key) * 16);
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x000A6D3D File Offset: 0x000A4F3D
	public static string GetDebugPositionFromKey(long key)
	{
		return string.Format("{0},{1}", WorldChunkCache.extractX(key) * 16, WorldChunkCache.extractZ(key) * 16);
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x000A6D3D File Offset: 0x000A4F3D
	public static string GetDebugPositionKey(long key)
	{
		return string.Format("{0},{1}", WorldChunkCache.extractX(key) * 16, WorldChunkCache.extractZ(key) * 16);
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x000A6D65 File Offset: 0x000A4F65
	public static int GetChunkSectionX(long key)
	{
		return WorldChunkCache.extractX(key);
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x000A6D6D File Offset: 0x000A4F6D
	public static int GetChunkSectionZ(long key)
	{
		return WorldChunkCache.extractZ(key);
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x000A6D75 File Offset: 0x000A4F75
	public static int GetWorldXFromKey(long key)
	{
		return WorldChunkCache.extractX(key) * 16;
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x000A6D80 File Offset: 0x000A4F80
	public static int GetWorldZFromKey(long key)
	{
		return WorldChunkCache.extractZ(key) * 16;
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x000A6D8B File Offset: 0x000A4F8B
	public static long GetRegionKeyFromWorldPosition(int worldX, int worldZ)
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundRegion(worldX)), World.toChunkXZ(DynamicMeshUnity.RoundRegion(worldZ)));
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x000A6DA8 File Offset: 0x000A4FA8
	public static Vector3i GetRegionPositionFromWorldPosition(int worldX, int worldZ)
	{
		return new Vector3i(DynamicMeshUnity.RoundRegion(worldX), 0, DynamicMeshUnity.RoundRegion(worldZ));
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x000A6DBC File Offset: 0x000A4FBC
	public static Vector3i GetRegionPositionFromWorldPosition(Vector3i worldPos)
	{
		return new Vector3i(DynamicMeshUnity.RoundRegion(worldPos.x), 0, DynamicMeshUnity.RoundRegion(worldPos.z));
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x000A6DDA File Offset: 0x000A4FDA
	public static int RoundRegion(int value)
	{
		if (value < 0)
		{
			value -= 159;
		}
		return value / 160 * 160;
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x000A6DF6 File Offset: 0x000A4FF6
	public static int RoundRegion(float value)
	{
		return DynamicMeshUnity.RoundRegion((int)value);
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x000A6DFF File Offset: 0x000A4FFF
	public static Vector3i GetRegionPositionFromWorldPosition(Vector3 worldPos)
	{
		return new Vector3i(DynamicMeshUnity.RoundRegion(worldPos.x), 0, DynamicMeshUnity.RoundRegion(worldPos.z));
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x000A6E1D File Offset: 0x000A501D
	public static long GetItemKey(int worldX, int worldZ)
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x000A6E30 File Offset: 0x000A5030
	public static int GetItemPosition(int pos)
	{
		return World.toChunkXZ(pos) * 16;
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x000A6E3B File Offset: 0x000A503B
	public static float Distance(Vector3i a, Vector3i b)
	{
		return Mathf.Abs(Mathf.Sqrt(Mathf.Pow((float)(a.x - b.x), 2f) + Mathf.Pow((float)(a.z - b.z), 2f)));
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x000A6E78 File Offset: 0x000A5078
	public static float Distance(Vector3i a, Vector3 b)
	{
		return Mathf.Abs(Mathf.Sqrt(Mathf.Pow((float)a.x - b.x, 2f) + Mathf.Pow((float)a.z - b.z, 2f)));
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x000A6EB5 File Offset: 0x000A50B5
	public static float Distance(int x1, int y1, int x2, int y2)
	{
		return Mathf.Abs(Mathf.Sqrt(Mathf.Pow((float)(x1 - x2), 2f) + Mathf.Pow((float)(y1 - y2), 2f)));
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x000A6EDE File Offset: 0x000A50DE
	public static void log(string msg)
	{
		if (DynamicMeshManager.ShowDebug && DynamicMeshManager.DoLog)
		{
			DynamicMeshManager.LogMsg(msg);
		}
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x000A2813 File Offset: 0x000A0A13
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogMsg(string msg)
	{
		if (DynamicMeshManager.DoLog)
		{
			DynamicMeshManager.LogMsg(msg);
		}
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x000A6EF4 File Offset: 0x000A50F4
	public static Vector3i GetChunkPositionFromWorldPosition(Vector3i worldPosition)
	{
		return new Vector3i(DynamicMeshUnity.RoundChunk(worldPosition.x), worldPosition.y, DynamicMeshUnity.RoundChunk(worldPosition.z));
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x000A6F18 File Offset: 0x000A5118
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

	// Token: 0x06001C2B RID: 7211 RVA: 0x000A6F70 File Offset: 0x000A5170
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

	// Token: 0x06001C2C RID: 7212 RVA: 0x000A6FDC File Offset: 0x000A51DC
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

	// Token: 0x06001C2D RID: 7213 RVA: 0x000A7070 File Offset: 0x000A5270
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

	// Token: 0x06001C2E RID: 7214 RVA: 0x000A7100 File Offset: 0x000A5300
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

	// Token: 0x06001C2F RID: 7215 RVA: 0x000A73B8 File Offset: 0x000A55B8
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

	// Token: 0x06001C30 RID: 7216 RVA: 0x000A74A4 File Offset: 0x000A56A4
	public static void AddDisabledImposterChunk(long key)
	{
		string text;
		HashSetLong hashSetLong;
		DynamicMeshUnity.GetOrCreateDynamicMeshChunksList(out text, out hashSetLong);
		hashSetLong.Add(key);
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x000A74C4 File Offset: 0x000A56C4
	public static void RemoveDisabledImposterChunk(long key)
	{
		string text;
		HashSetLong hashSetLong;
		DynamicMeshUnity.GetOrCreateDynamicMeshChunksList(out text, out hashSetLong);
		hashSetLong.Remove(key);
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000A74E2 File Offset: 0x000A56E2
	public static void ClearCachedDynamicMeshChunksList()
	{
		DynamicMeshUnity.cachedDynamicMeshChunksList.Item1 = string.Empty;
		DynamicMeshUnity.cachedDynamicMeshChunksList.Item2.Clear();
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000A7502 File Offset: 0x000A5702
	[Conditional("UNITY_STANDALONE")]
	public static void EnsureDMDirectoryExists()
	{
		if (!SdDirectory.Exists(DynamicMeshFile.MeshLocation))
		{
			SdDirectory.CreateDirectory(DynamicMeshFile.MeshLocation);
		}
	}

	// Token: 0x0400123E RID: 4670
	public const int RegionSize = 160;

	// Token: 0x0400123F RID: 4671
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<DynamicMeshRegion> tempRegions = new HashSet<DynamicMeshRegion>();

	// Token: 0x04001240 RID: 4672
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<long> tempRegionKeys = new HashSet<long>();

	// Token: 0x04001241 RID: 4673
	[TupleElementNames(new string[] { "path", "keys" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public static ValueTuple<string, HashSetLong> cachedDynamicMeshChunksList = new ValueTuple<string, HashSetLong>(string.Empty, new HashSetLong());
}
