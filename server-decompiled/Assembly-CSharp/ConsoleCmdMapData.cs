using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000244 RID: 580
[Preserve]
public class ConsoleCmdMapData : ConsoleCmdAbstract
{
	// Token: 0x06001157 RID: 4439 RVA: 0x0006E0AA File Offset: 0x0006C2AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "mapdata" };
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x0006E0BA File Offset: 0x0006C2BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\nclear - reset player map data\nprefab - save prefabs to mapdata.png\nstart - save start points to mapdata.png";
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x0006E0C1 File Offset: 0x0006C2C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Writes some map data to an image";
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x0006E0C8 File Offset: 0x0006C2C8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.getHelp());
			return;
		}
		string text = _params[0].ToLower();
		if (text == "clear")
		{
			GameManager.Instance.World.GetPrimaryPlayer().ChunkObserver.mapDatabase.Clear();
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Map cleared");
			return;
		}
		ConsoleCmdMapData.EMode emode = ConsoleCmdMapData.EMode.Prefabs;
		if (text == "start")
		{
			emode = ConsoleCmdMapData.EMode.StartPoints;
		}
		IChunkProvider chunkProvider = GameManager.Instance.World.ChunkCache.ChunkProvider;
		Vector2i worldSize = chunkProvider.GetWorldSize();
		Texture2D texture2D = new Texture2D(worldSize.x, worldSize.y);
		Color[] pixels = texture2D.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = new Color(0f, 0f, 0f, 0f);
		}
		if (emode == ConsoleCmdMapData.EMode.Prefabs)
		{
			DynamicPrefabDecorator dynamicPrefabDecorator = chunkProvider.GetDynamicPrefabDecorator();
			if (dynamicPrefabDecorator == null)
			{
				return;
			}
			List<PrefabInstance> list = new List<PrefabInstance>();
			dynamicPrefabDecorator.GetAllPrefabs(list);
			if (list == null)
			{
				return;
			}
			foreach (PrefabInstance prefabInstance in list)
			{
				this.setRect(worldSize, prefabInstance.boundingBoxPosition, prefabInstance.boundingBoxSize, pixels, ConsoleCmdMapData.EColorChannel.Green);
			}
		}
		if (emode == ConsoleCmdMapData.EMode.StartPoints)
		{
			SpawnPointList spawnPointList = chunkProvider.GetSpawnPointList();
			if (spawnPointList == null)
			{
				return;
			}
			foreach (SpawnPoint spawnPoint in spawnPointList)
			{
				this.setRect(worldSize, new Vector3i(spawnPoint.spawnPosition.position) - new Vector3i(3, 0, 3), new Vector3i(7, 0, 7), pixels, ConsoleCmdMapData.EColorChannel.Blue);
			}
		}
		texture2D.SetPixels(pixels);
		texture2D.Apply();
		TextureUtils.SaveTexture(texture2D, "mapdata.png");
		global::UnityEngine.Object.Destroy(texture2D);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Saved mapdata.png and put " + emode.ToStringCached<ConsoleCmdMapData.EMode>() + " into it");
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x0006E2EC File Offset: 0x0006C4EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setRect(Vector2i _worldSize, Vector3i _pos, Vector3i _size, Color[] _cols, ConsoleCmdMapData.EColorChannel _channel)
	{
		int num = _worldSize.x / 2 + _pos.x;
		int num2 = _worldSize.y / 2 + _pos.z;
		for (int i = 0; i < _size.x; i++)
		{
			for (int j = 0; j < _size.z; j++)
			{
				int num3 = i + num + (j + num2) * _worldSize.x;
				if (num3 >= 0 && num3 < _cols.Length)
				{
					Color color = _cols[num3];
					switch (_channel)
					{
					case ConsoleCmdMapData.EColorChannel.Red:
						color.r = 1f;
						color.a = 1f;
						break;
					case ConsoleCmdMapData.EColorChannel.Green:
						color.g = 1f;
						color.a = 1f;
						break;
					case ConsoleCmdMapData.EColorChannel.Blue:
						color.b = 1f;
						color.a = 1f;
						break;
					}
					_cols[num3] = color;
				}
			}
		}
	}

	// Token: 0x02000245 RID: 581
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EMode
	{
		// Token: 0x04000CD3 RID: 3283
		Prefabs,
		// Token: 0x04000CD4 RID: 3284
		StartPoints
	}

	// Token: 0x02000246 RID: 582
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EColorChannel
	{
		// Token: 0x04000CD6 RID: 3286
		Red,
		// Token: 0x04000CD7 RID: 3287
		Green,
		// Token: 0x04000CD8 RID: 3288
		Blue
	}
}
