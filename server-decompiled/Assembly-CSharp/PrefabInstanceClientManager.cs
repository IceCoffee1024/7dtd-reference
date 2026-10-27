using System;
using System.Collections.Generic;
using PrefabVolumes;

// Token: 0x02000961 RID: 2401
public class PrefabInstanceClientManager
{
	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x060045C6 RID: 17862 RVA: 0x001BBD80 File Offset: 0x001B9F80
	public static PrefabInstanceClientManager Instance
	{
		get
		{
			PrefabInstanceClientManager prefabInstanceClientManager;
			if ((prefabInstanceClientManager = PrefabInstanceClientManager.instance) == null)
			{
				prefabInstanceClientManager = (PrefabInstanceClientManager.instance = new PrefabInstanceClientManager());
			}
			return prefabInstanceClientManager;
		}
	}

	// Token: 0x060045C7 RID: 17863 RVA: 0x001BBD98 File Offset: 0x001B9F98
	public void Cleanup()
	{
		this.clientPrefabs.Clear();
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator != null)
		{
			dynamicPrefabDecorator.OnPrefabLoaded -= this.prefabLoadedServer;
			dynamicPrefabDecorator.OnPrefabChanged -= this.prefabChangedServer;
			dynamicPrefabDecorator.OnPrefabRemoved -= this.prefabRemovedServer;
		}
		PrefabEditModeManager.Instance.OnPrefabChanged -= this.prefabChangedServer;
		GameManager.Instance.OnClientSpawned -= this.sendAllPrefabs;
	}

	// Token: 0x060045C8 RID: 17864 RVA: 0x001BBE20 File Offset: 0x001BA020
	public void StartAsServer()
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		dynamicPrefabDecorator.OnPrefabLoaded += this.prefabLoadedServer;
		dynamicPrefabDecorator.OnPrefabChanged += this.prefabChangedServer;
		dynamicPrefabDecorator.OnPrefabRemoved += this.prefabRemovedServer;
		PrefabEditModeManager.Instance.OnPrefabChanged += this.prefabChangedServer;
		GameManager.Instance.OnClientSpawned += this.sendAllPrefabs;
	}

	// Token: 0x060045C9 RID: 17865 RVA: 0x001BBE98 File Offset: 0x001BA098
	public void StartAsClient()
	{
		this.clientPrefabs.Clear();
	}

	// Token: 0x060045CA RID: 17866 RVA: 0x001BBEA8 File Offset: 0x001BA0A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void sendAllPrefabs(ClientInfo _toClient)
	{
		if (_toClient != null)
		{
			this.dynamicPrefabBuffer.Clear();
			GameManager.Instance.GetDynamicPrefabDecorator().GetWorldPrefabs(this.dynamicPrefabBuffer);
			foreach (PrefabInstance prefabInstance in this.dynamicPrefabBuffer)
			{
				_toClient.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Added, prefabInstance));
				foreach (PrefabVolumeListAbs prefabVolumeListAbs in prefabInstance.prefab.AllVolumeLists)
				{
					prefabVolumeListAbs.SendAllVolumesToClient(_toClient, prefabInstance.id);
				}
			}
		}
	}

	// Token: 0x060045CB RID: 17867 RVA: 0x001BBF78 File Offset: 0x001BA178
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabLoadedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Added, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060045CC RID: 17868 RVA: 0x001BBFB0 File Offset: 0x001BA1B0
	public void PrefabLoadedClient(int _prefabInstanceId, Vector3i _boundingBoxPosition, Vector3i _boundingBoxSize, string _prefabInstanceName, Vector3i _prefabSize, string _prefabFilename, int _prefabLocalRotation, int _yOffset)
	{
		PathAbstractions.AbstractedLocation location = PathAbstractions.PrefabsSearchPaths.GetLocation(_prefabFilename, null, null);
		PrefabInstance prefabInstance = new PrefabInstance(_prefabInstanceId, location, _boundingBoxPosition, 0, null, 0)
		{
			boundingBoxSize = _boundingBoxSize,
			name = _prefabInstanceName,
			prefab = new Prefab
			{
				size = _prefabSize,
				location = location,
				yOffset = _yOffset
			}
		};
		prefabInstance.prefab.SetLocalRotation(_prefabLocalRotation);
		prefabInstance.CreateBoundingBox(false);
		this.clientPrefabs.Add(prefabInstance);
		if (this.clientPrefabs.Count == 1)
		{
			PrefabEditModeManager.Instance.SetGroundLevel(_yOffset);
		}
	}

	// Token: 0x060045CD RID: 17869 RVA: 0x001BC054 File Offset: 0x001BA254
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabChangedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Changed, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060045CE RID: 17870 RVA: 0x001BC08C File Offset: 0x001BA28C
	public void PrefabChangedClient(int _prefabInstanceId, Vector3i _boundingBoxPosition, Vector3i _boundingBoxSize, string _prefabInstanceName, Vector3i _prefabSize, string _prefabFilename, int _prefabLocalRotation, int _yOffset)
	{
		PrefabInstance prefabInstance = this.GetPrefabInstance(_prefabInstanceId);
		if (prefabInstance == null)
		{
			Log.Error("Prefab not found: " + _prefabInstanceId.ToString());
			return;
		}
		PathAbstractions.AbstractedLocation location = PathAbstractions.PrefabsSearchPaths.GetLocation(_prefabFilename, null, null);
		prefabInstance.boundingBoxPosition = _boundingBoxPosition;
		prefabInstance.boundingBoxSize = _boundingBoxSize;
		prefabInstance.name = _prefabInstanceName;
		prefabInstance.prefab.size = _prefabSize;
		prefabInstance.prefab.location = location;
		prefabInstance.prefab.SetLocalRotation(_prefabLocalRotation);
		prefabInstance.prefab.yOffset = _yOffset;
		prefabInstance.CreateBoundingBox(false);
		if (this.clientPrefabs.IndexOf(prefabInstance) == 0)
		{
			PrefabEditModeManager.Instance.SetGroundLevel(_yOffset);
		}
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x001BC144 File Offset: 0x001BA344
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabRemovedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Removed, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060045D0 RID: 17872 RVA: 0x001BC17C File Offset: 0x001BA37C
	public void PrefabRemovedClient(int _prefabInstanceId)
	{
		for (int i = 0; i < this.clientPrefabs.Count; i++)
		{
			PrefabInstance prefabInstance = this.clientPrefabs[i];
			if (prefabInstance.id == _prefabInstanceId)
			{
				this.clientPrefabs.RemoveAt(i);
				using (List<PrefabVolumeListAbs>.Enumerator enumerator = prefabInstance.prefab.AllVolumeLists.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PrefabVolumeListAbs prefabVolumeListAbs = enumerator.Current;
						prefabVolumeListAbs.RemoveVolumes(prefabInstance);
					}
					break;
				}
			}
		}
	}

	// Token: 0x060045D1 RID: 17873 RVA: 0x001BC20C File Offset: 0x001BA40C
	public PrefabInstance GetPrefabInstance(int _prefabId)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			foreach (PrefabInstance prefabInstance in this.clientPrefabs)
			{
				if (prefabInstance.id == _prefabId)
				{
					return prefabInstance;
				}
			}
			return null;
		}
		if (GameManager.Instance.GetDynamicPrefabDecorator() == null)
		{
			return null;
		}
		return GameManager.Instance.GetDynamicPrefabDecorator().GetPrefab(_prefabId);
	}

	// Token: 0x04003862 RID: 14434
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabInstanceClientManager instance;

	// Token: 0x04003863 RID: 14435
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PrefabInstance> clientPrefabs = new List<PrefabInstance>();

	// Token: 0x04003864 RID: 14436
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PrefabInstance> dynamicPrefabBuffer = new List<PrefabInstance>();
}
