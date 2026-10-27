using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005DC RID: 1500
[Preserve]
public class ItemActionSpawnVehicle : ItemAction
{
	// Token: 0x060030D9 RID: 12505 RVA: 0x0013AE50 File Offset: 0x00139050
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionSpawnVehicle.ItemActionDataSpawnVehicle(_invData, _indexInEntityOfAction);
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x0013AE5C File Offset: 0x0013905C
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Vehicle"))
		{
			this.entityToSpawn = _props.Values["Vehicle"];
		}
		this.vehicleSize = new Vector3(1f, 1.9f, 2f);
		if (_props.Values.ContainsKey("VehicleSize"))
		{
			this.vehicleSize = StringParsers.ParseVector3(_props.Values["VehicleSize"], 0, -1);
		}
		foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
		{
			if (keyValuePair.Value.entityClassName == this.entityToSpawn)
			{
				this.entityId = keyValuePair.Key;
				break;
			}
		}
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x0013AF4C File Offset: 0x0013914C
	public override void StartHolding(ItemActionData _actionData)
	{
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = (ItemActionSpawnVehicle.ItemActionDataSpawnVehicle)_actionData;
		EntityPlayerLocal entityPlayerLocal = itemActionDataSpawnVehicle.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
		{
			return;
		}
		if (itemActionDataSpawnVehicle.VehiclePreviewT)
		{
			global::UnityEngine.Object.DestroyImmediate(itemActionDataSpawnVehicle.VehiclePreviewT.gameObject);
		}
		GameObject gameObject = DataLoader.LoadAsset<GameObject>(entityPlayerLocal.inventory.holdingItem.MeshFile, false);
		itemActionDataSpawnVehicle.VehiclePreviewT = global::UnityEngine.Object.Instantiate<GameObject>(gameObject).transform;
		Vehicle.SetupPreview(itemActionDataSpawnVehicle.VehiclePreviewT);
		this.SetupPreview(itemActionDataSpawnVehicle);
		GameManager.Instance.StartCoroutine(this.UpdatePreview(itemActionDataSpawnVehicle));
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x0013AFE4 File Offset: 0x001391E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupPreview(ItemActionSpawnVehicle.ItemActionDataSpawnVehicle data)
	{
		data.ValidPosition = false;
		if (data.PreviewRenderers == null || data.PreviewRenderers.Length == 0 || data.PreviewRenderers[0] == null)
		{
			data.PreviewRenderers = data.VehiclePreviewT.GetComponentsInChildren<Renderer>();
		}
		for (int i = 0; i < data.PreviewRenderers.Length; i++)
		{
			data.PreviewRenderers[i].material.color = new Color(2f, 0.25f, 0.25f);
		}
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x0013B063 File Offset: 0x00139263
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator UpdatePreview(ItemActionSpawnVehicle.ItemActionDataSpawnVehicle data)
	{
		World world = data.invData.world;
		while (data.VehiclePreviewT)
		{
			bool flag = this.CalcSpawnPosition(data, ref data.Position) && world.CanPlaceBlockAt(new Vector3i(data.Position), world.GetGameManager().GetPersistentLocalPlayer(), false);
			if (data.ValidPosition != flag)
			{
				data.ValidPosition = flag;
				if (data.PreviewRenderers == null || data.PreviewRenderers.Length == 0 || data.PreviewRenderers[0] == null)
				{
					data.PreviewRenderers = data.VehiclePreviewT.GetComponentsInChildren<Renderer>();
				}
				Color color = (flag ? new Color(0.25f, 2f, 0.25f) : new Color(2f, 0.25f, 0.25f));
				for (int i = 0; i < data.PreviewRenderers.Length; i++)
				{
					data.PreviewRenderers[i].material.color = color;
				}
			}
			if (data.Position.y < 9999f)
			{
				Quaternion quaternion = Quaternion.Euler(0f, data.invData.holdingEntity.rotation.y + 90f, 0f);
				data.VehiclePreviewT.localRotation = quaternion;
				data.VehiclePreviewT.position = data.Position - Origin.position;
				data.VehiclePreviewT.gameObject.SetActive(true);
			}
			else
			{
				data.VehiclePreviewT.gameObject.SetActive(false);
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x0013B07C File Offset: 0x0013927C
	public override void CancelAction(ItemActionData _actionData)
	{
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = (ItemActionSpawnVehicle.ItemActionDataSpawnVehicle)_actionData;
		if (itemActionDataSpawnVehicle.VehiclePreviewT && itemActionDataSpawnVehicle.invData.holdingEntity is EntityPlayerLocal)
		{
			global::UnityEngine.Object.Destroy(itemActionDataSpawnVehicle.VehiclePreviewT.gameObject);
		}
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x0013B0C0 File Offset: 0x001392C0
	public override void StopHolding(ItemActionData _actionData)
	{
		base.StopHolding(_actionData);
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = (ItemActionSpawnVehicle.ItemActionDataSpawnVehicle)_actionData;
		if (itemActionDataSpawnVehicle.VehiclePreviewT && itemActionDataSpawnVehicle.invData.holdingEntity is EntityPlayerLocal)
		{
			global::UnityEngine.Object.Destroy(itemActionDataSpawnVehicle.VehiclePreviewT.gameObject);
		}
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x0013B10C File Offset: 0x0013930C
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
		{
			return;
		}
		float time = Time.time;
		if (time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		if (time - _actionData.lastUseTime < 2f)
		{
			return;
		}
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = (ItemActionSpawnVehicle.ItemActionDataSpawnVehicle)_actionData;
		if (!itemActionDataSpawnVehicle.ValidPosition)
		{
			return;
		}
		if (this.entityId < 0)
		{
			foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
			{
				if (keyValuePair.Value.entityClassName == this.entityToSpawn)
				{
					this.entityId = keyValuePair.Key;
					break;
				}
			}
			if (this.entityId == 0)
			{
				return;
			}
		}
		_actionData.lastUseTime = time;
		ItemValue holdingItemItemValue = entityPlayerLocal.inventory.holdingItemItemValue;
		if (VehicleManager.CanAddMoreVehicles())
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageVehicleSpawn>().Setup(this.entityId, itemActionDataSpawnVehicle.Position, new Vector3(0f, entityPlayerLocal.rotation.y + 90f, 0f), holdingItemItemValue.Clone(), entityPlayerLocal.entityId), true);
			}
			else
			{
				Entity entity = EntityFactory.CreateEntity(this.entityId, itemActionDataSpawnVehicle.Position + Vector3.up * 0.25f, new Vector3(0f, entityPlayerLocal.rotation.y + 90f, 0f));
				entity.SetSpawnerSource(EnumSpawnerSource.StaticSpawner);
				EntityVehicle entityVehicle = entity as EntityVehicle;
				if (entityVehicle != null)
				{
					entityVehicle.GetVehicle().SetItemValue(holdingItemItemValue.Clone());
					entityVehicle.SetOwner(PlatformManager.InternalLocalUserIdentifier);
				}
				else
				{
					EntityAlive entityAlive = entity as EntityAlive;
					if (entityAlive != null)
					{
						entityAlive.factionId = entityPlayerLocal.factionId;
						entityAlive.belongsPlayerId = entityPlayerLocal.entityId;
						entityAlive.factionRank = entityPlayerLocal.factionRank - 1;
					}
				}
				GameManager.Instance.World.SpawnEntityInWorld(entity);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageVehicleCount>().Setup(), false, -1, -1, -1, null, 192, false);
			}
			entityPlayerLocal.RightArmAnimationUse = true;
			entityPlayerLocal.DropTimeDelay = 0.5f;
			entityPlayerLocal.inventory.DecHoldingItem(1);
			entityPlayerLocal.PlayOneShot((this.soundStart != null) ? this.soundStart : "placeblock", false, false, false, null, 1f);
			this.ClearPreview(_actionData);
			return;
		}
		GameManager.ShowTooltip(entityPlayerLocal, "uiCannotAddVehicle", false, false, 0f);
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x0013B3B4 File Offset: 0x001395B4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CalcSpawnPosition(ItemActionSpawnVehicle.ItemActionDataSpawnVehicle _actionData, ref Vector3 position)
	{
		World world = _actionData.invData.world;
		Ray lookRay = _actionData.invData.holdingEntity.GetLookRay();
		if (Vector3.Dot(lookRay.direction, Vector3.up) == 1f)
		{
			return false;
		}
		position.y = float.MaxValue;
		float num = 4f + this.vehicleSize.x;
		if (Voxel.Raycast(world, lookRay, num + 1.5f, 8454144, 69, 0f))
		{
			if ((Voxel.voxelRayHitInfo.hit.pos - lookRay.origin).magnitude > num)
			{
				position = Voxel.voxelRayHitInfo.hit.pos;
				return false;
			}
			for (float num2 = 0.14f; num2 < 1.15f; num2 += 0.25f)
			{
				position = Voxel.voxelRayHitInfo.hit.pos;
				position.y += num2;
				Vector3 normalized = Vector3.Cross(lookRay.direction, Vector3.up).normalized;
				Vector3 vector = Vector3.Cross(normalized, Vector3.up);
				Vector3 vector2 = position - Origin.position;
				vector2.y += this.vehicleSize.y * 0.5f + 0.05f;
				if (this.CheckForSpace(vector2, normalized, this.vehicleSize.z, vector, this.vehicleSize.x, Vector3.up, this.vehicleSize.y) && this.CheckForSpace(vector2, vector, this.vehicleSize.x, normalized, this.vehicleSize.z, Vector3.up, this.vehicleSize.y) && this.CheckForSpace(vector2, Vector3.up, this.vehicleSize.y, normalized, this.vehicleSize.z, vector, this.vehicleSize.x))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x0013B5B0 File Offset: 0x001397B0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckForSpace(Vector3 localPos, Vector3 dirN, float length, Vector3 axis1N, float axis1Length, Vector3 axis2N, float axis2Length)
	{
		Vector3 vector = dirN * length * 0.5f;
		for (float num = -axis1Length * 0.5f; num <= axis1Length * 0.5f; num += 0.2499f)
		{
			Vector3 vector2 = localPos + axis1N * num;
			for (float num2 = -axis2Length * 0.5f; num2 <= axis2Length * 0.5f; num2 += 0.2499f)
			{
				Vector3 vector3 = vector2 + axis2N * num2;
				if (Physics.Raycast(vector3 - vector, dirN, length, 28901376))
				{
					return false;
				}
				if (Physics.Raycast(vector3 + vector, -dirN, length, 28901376))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x0013B668 File Offset: 0x00139868
	public void ClearPreview(ItemActionData _data)
	{
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = _data as ItemActionSpawnVehicle.ItemActionDataSpawnVehicle;
		this.ClearPreview(itemActionDataSpawnVehicle);
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x0013B683 File Offset: 0x00139883
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearPreview(ItemActionSpawnVehicle.ItemActionDataSpawnVehicle _actionData)
	{
		if (_actionData.VehiclePreviewT)
		{
			global::UnityEngine.Object.Destroy(_actionData.VehiclePreviewT.gameObject);
		}
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x0013B6A4 File Offset: 0x001398A4
	public override void Cleanup(ItemActionData _data)
	{
		base.Cleanup(_data);
		ItemActionSpawnVehicle.ItemActionDataSpawnVehicle itemActionDataSpawnVehicle = _data as ItemActionSpawnVehicle.ItemActionDataSpawnVehicle;
		if (itemActionDataSpawnVehicle != null && itemActionDataSpawnVehicle.invData != null && itemActionDataSpawnVehicle.invData.holdingEntity is EntityPlayerLocal)
		{
			this.ClearPreview(itemActionDataSpawnVehicle);
		}
	}

	// Token: 0x040025C9 RID: 9673
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cColliderMask = 28901376;

	// Token: 0x040025CA RID: 9674
	[PublicizedFrom(EAccessModifier.Private)]
	public string entityToSpawn;

	// Token: 0x040025CB RID: 9675
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId = -1;

	// Token: 0x040025CC RID: 9676
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 vehicleSize;

	// Token: 0x020005DD RID: 1501
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataSpawnVehicle : ItemActionAttackData
	{
		// Token: 0x060030E7 RID: 12519 RVA: 0x0012BE18 File Offset: 0x0012A018
		public ItemActionDataSpawnVehicle(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025CD RID: 9677
		public Transform VehiclePreviewT;

		// Token: 0x040025CE RID: 9678
		public Renderer[] PreviewRenderers;

		// Token: 0x040025CF RID: 9679
		public bool ValidPosition;

		// Token: 0x040025D0 RID: 9680
		public Vector3 Position;
	}
}
