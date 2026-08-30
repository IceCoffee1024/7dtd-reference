using System;
using System.Collections.Generic;
using PrefabVolumes;

// Token: 0x02000940 RID: 2368
public class PrefabInstanceClientManager
{
	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x0600457C RID: 17788 RVA: 0x001B8000 File Offset: 0x001B6200
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

	// Token: 0x0600457D RID: 17789 RVA: 0x001B8018 File Offset: 0x001B6218
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

	// Token: 0x0600457E RID: 17790 RVA: 0x001B80A0 File Offset: 0x001B62A0
	public void StartAsServer()
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		dynamicPrefabDecorator.OnPrefabLoaded += this.prefabLoadedServer;
		dynamicPrefabDecorator.OnPrefabChanged += this.prefabChangedServer;
		dynamicPrefabDecorator.OnPrefabRemoved += this.prefabRemovedServer;
		PrefabEditModeManager.Instance.OnPrefabChanged += this.prefabChangedServer;
		GameManager.Instance.OnClientSpawned += this.sendAllPrefabs;
	}

	// Token: 0x0600457F RID: 17791 RVA: 0x001B8118 File Offset: 0x001B6318
	public void StartAsClient()
	{
		this.clientPrefabs.Clear();
	}

	// Token: 0x06004580 RID: 17792 RVA: 0x001B8128 File Offset: 0x001B6328
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

	// Token: 0x06004581 RID: 17793 RVA: 0x001B81F8 File Offset: 0x001B63F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabLoadedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Added, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06004582 RID: 17794 RVA: 0x001B8230 File Offset: 0x001B6430
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

	// Token: 0x06004583 RID: 17795 RVA: 0x001B82D4 File Offset: 0x001B64D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabChangedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Changed, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06004584 RID: 17796 RVA: 0x001B830C File Offset: 0x001B650C
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

	// Token: 0x06004585 RID: 17797 RVA: 0x001B83C4 File Offset: 0x001B65C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabRemovedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Removed, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06004586 RID: 17798 RVA: 0x001B83FC File Offset: 0x001B65FC
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

	// Token: 0x06004587 RID: 17799 RVA: 0x001B848C File Offset: 0x001B668C
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

	// Token: 0x040037B5 RID: 14261
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabInstanceClientManager instance;

	// Token: 0x040037B6 RID: 14262
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PrefabInstance> clientPrefabs = new List<PrefabInstance>();

	// Token: 0x040037B7 RID: 14263
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PrefabInstance> dynamicPrefabBuffer = new List<PrefabInstance>();
}
