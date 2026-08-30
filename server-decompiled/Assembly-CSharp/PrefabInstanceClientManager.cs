using System;
using System.Collections.Generic;
using PrefabVolumes;

// Token: 0x02000951 RID: 2385
public class PrefabInstanceClientManager
{
	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x06004602 RID: 17922 RVA: 0x001BA7F4 File Offset: 0x001B89F4
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

	// Token: 0x06004603 RID: 17923 RVA: 0x001BA80C File Offset: 0x001B8A0C
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

	// Token: 0x06004604 RID: 17924 RVA: 0x001BA894 File Offset: 0x001B8A94
	public void StartAsServer()
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		dynamicPrefabDecorator.OnPrefabLoaded += this.prefabLoadedServer;
		dynamicPrefabDecorator.OnPrefabChanged += this.prefabChangedServer;
		dynamicPrefabDecorator.OnPrefabRemoved += this.prefabRemovedServer;
		PrefabEditModeManager.Instance.OnPrefabChanged += this.prefabChangedServer;
		GameManager.Instance.OnClientSpawned += this.sendAllPrefabs;
	}

	// Token: 0x06004605 RID: 17925 RVA: 0x001BA90C File Offset: 0x001B8B0C
	public void StartAsClient()
	{
		this.clientPrefabs.Clear();
	}

	// Token: 0x06004606 RID: 17926 RVA: 0x001BA91C File Offset: 0x001B8B1C
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

	// Token: 0x06004607 RID: 17927 RVA: 0x001BA9EC File Offset: 0x001B8BEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabLoadedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Added, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06004608 RID: 17928 RVA: 0x001BAA24 File Offset: 0x001B8C24
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

	// Token: 0x06004609 RID: 17929 RVA: 0x001BAAC8 File Offset: 0x001B8CC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabChangedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Changed, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x0600460A RID: 17930 RVA: 0x001BAB00 File Offset: 0x001B8D00
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

	// Token: 0x0600460B RID: 17931 RVA: 0x001BABB8 File Offset: 0x001B8DB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabRemovedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Removed, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x0600460C RID: 17932 RVA: 0x001BABF0 File Offset: 0x001B8DF0
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

	// Token: 0x0600460D RID: 17933 RVA: 0x001BAC80 File Offset: 0x001B8E80
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

	// Token: 0x04003846 RID: 14406
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabInstanceClientManager instance;

	// Token: 0x04003847 RID: 14407
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PrefabInstance> clientPrefabs = new List<PrefabInstance>();

	// Token: 0x04003848 RID: 14408
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PrefabInstance> dynamicPrefabBuffer = new List<PrefabInstance>();
}
