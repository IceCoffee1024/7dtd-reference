using System;
using System.Collections.Generic;
using PrefabVolumes;

// Token: 0x02000957 RID: 2391
public class PrefabInstanceClientManager
{
	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x060045A3 RID: 17827 RVA: 0x001BACFC File Offset: 0x001B8EFC
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

	// Token: 0x060045A4 RID: 17828 RVA: 0x001BAD14 File Offset: 0x001B8F14
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

	// Token: 0x060045A5 RID: 17829 RVA: 0x001BAD9C File Offset: 0x001B8F9C
	public void StartAsServer()
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		dynamicPrefabDecorator.OnPrefabLoaded += this.prefabLoadedServer;
		dynamicPrefabDecorator.OnPrefabChanged += this.prefabChangedServer;
		dynamicPrefabDecorator.OnPrefabRemoved += this.prefabRemovedServer;
		PrefabEditModeManager.Instance.OnPrefabChanged += this.prefabChangedServer;
		GameManager.Instance.OnClientSpawned += this.sendAllPrefabs;
	}

	// Token: 0x060045A6 RID: 17830 RVA: 0x001BAE14 File Offset: 0x001B9014
	public void StartAsClient()
	{
		this.clientPrefabs.Clear();
	}

	// Token: 0x060045A7 RID: 17831 RVA: 0x001BAE24 File Offset: 0x001B9024
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

	// Token: 0x060045A8 RID: 17832 RVA: 0x001BAEF4 File Offset: 0x001B90F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabLoadedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Added, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060045A9 RID: 17833 RVA: 0x001BAF2C File Offset: 0x001B912C
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

	// Token: 0x060045AA RID: 17834 RVA: 0x001BAFD0 File Offset: 0x001B91D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabChangedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Changed, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060045AB RID: 17835 RVA: 0x001BB008 File Offset: 0x001B9208
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

	// Token: 0x060045AC RID: 17836 RVA: 0x001BB0C0 File Offset: 0x001B92C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void prefabRemovedServer(PrefabInstance _prefabInstance)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorPrefabInstance>().Setup(NetPackageEditorPrefabInstance.EChangeType.Removed, _prefabInstance), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060045AD RID: 17837 RVA: 0x001BB0F8 File Offset: 0x001B92F8
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

	// Token: 0x060045AE RID: 17838 RVA: 0x001BB188 File Offset: 0x001B9388
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

	// Token: 0x04003842 RID: 14402
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabInstanceClientManager instance;

	// Token: 0x04003843 RID: 14403
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PrefabInstance> clientPrefabs = new List<PrefabInstance>();

	// Token: 0x04003844 RID: 14404
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PrefabInstance> dynamicPrefabBuffer = new List<PrefabInstance>();
}
