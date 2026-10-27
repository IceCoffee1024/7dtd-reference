using System;
using System.Collections.Generic;
using AssetManagement;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005BB RID: 1467
[Preserve]
public class ItemActionLauncher : ItemActionRanged
{
	// Token: 0x06002FF4 RID: 12276 RVA: 0x001321D7 File Offset: 0x001303D7
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionLauncher.ItemActionDataLauncher(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FF5 RID: 12277 RVA: 0x001321E0 File Offset: 0x001303E0
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
	}

	// Token: 0x06002FF6 RID: 12278 RVA: 0x001321EC File Offset: 0x001303EC
	public override void StartHolding(ItemActionData _actionData)
	{
		base.StartHolding(_actionData);
		this.DeleteProjectiles(_actionData);
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		if (_actionData.invData.itemValue.Meta != 0 && this.GetMaxAmmoCount(itemActionDataLauncher) != 0)
		{
			for (int i = 0; i < _actionData.invData.itemValue.Meta; i++)
			{
				itemActionDataLauncher.projectileTs.Add(this.instantiateProjectile(_actionData, default(Vector3)));
			}
		}
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x0013225F File Offset: 0x0013045F
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		this.DeleteProjectiles(_data);
	}

	// Token: 0x06002FF8 RID: 12280 RVA: 0x00132270 File Offset: 0x00130470
	public void DeleteProjectiles(ItemActionData _actionData)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		for (int i = 0; i < itemActionDataLauncher.projectileTs.Count; i++)
		{
			Transform transform = itemActionDataLauncher.projectileTs[i];
			if (transform)
			{
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		itemActionDataLauncher.projectileTs.Clear();
	}

	// Token: 0x06002FF9 RID: 12281 RVA: 0x001322C8 File Offset: 0x001304C8
	public override void ReloadGun(ItemActionData _actionData)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		if (itemActionDataLauncher != null)
		{
			itemActionDataLauncher.isReloadRequested = false;
			Manager.StopSequence(_actionData.invData.holdingEntity, ((ItemActionRanged.ItemActionDataRanged)_actionData).SoundStart);
			if (!_actionData.invData.holdingEntity.isEntityRemote)
			{
				_actionData.invData.holdingEntity.OnReloadStart();
			}
		}
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x00132324 File Offset: 0x00130524
	public override void CancelReload(ItemActionData _actionData, bool holsterWeapon)
	{
		base.CancelReload(_actionData, holsterWeapon);
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		this.ClampAmmoCount(itemActionDataLauncher);
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x00132348 File Offset: 0x00130548
	public override void SwapAmmoType(EntityAlive _entity, int _selectedIndex = -1)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_entity.inventory.holdingItemData.actionData[0];
		this.ClampAmmoCount(itemActionDataLauncher);
		base.SwapAmmoType(_entity, _selectedIndex);
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x00132380 File Offset: 0x00130580
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClampAmmoCount(ItemActionLauncher.ItemActionDataLauncher actionData)
	{
		int maxAmmoCount = this.GetMaxAmmoCount(actionData);
		int num = actionData.projectileTs.Count - maxAmmoCount;
		if (num > 0)
		{
			for (int i = maxAmmoCount; i < actionData.projectileTs.Count; i++)
			{
				if (actionData.projectileTs[i] != null)
				{
					global::UnityEngine.Object.Destroy(actionData.projectileTs[i].gameObject);
				}
			}
			actionData.projectileTs.RemoveRange(maxAmmoCount, num);
		}
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x001323F4 File Offset: 0x001305F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 fireShot(int _shotIdx, ItemActionRanged.ItemActionDataRanged _actionData, ref bool hitEntity)
	{
		hitEntity = true;
		return Vector3.zero;
	}

	// Token: 0x06002FFE RID: 12286 RVA: 0x00132400 File Offset: 0x00130600
	public Transform instantiateProjectile(ItemActionData _actionData, Vector3 _positionOffset = default(Vector3))
	{
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		ItemValue holdingItemItemValue = holdingEntity.inventory.holdingItemItemValue;
		ItemValue item = ItemClass.GetItem(this.MagazineItemNames[(int)holdingItemItemValue.SelectedAmmoTypeIndex], false);
		this.LastProjectileType = item;
		ItemClass forId = ItemClass.GetForId(item.type);
		if (forId == null)
		{
			return null;
		}
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		itemActionDataLauncher.lastAttackStrainPercent = itemActionDataLauncher.strainPercent;
		ItemValue itemValue = new ItemValue(forId.Id, false);
		InstanceHandle<GameObject> instanceHandle = forId.CloneModel(itemValue, null, BlockShape.MeshPurpose.World);
		Transform transform = instanceHandle.Instance.transform;
		Transform transform2 = itemActionDataLauncher.projectileJointT;
		if (!transform2)
		{
			transform2 = holdingEntity.emodel.GetRightHandTransform();
		}
		if (transform2)
		{
			transform.SetParent(transform2, false);
			transform.SetLocalPositionAndRotation(_positionOffset, Quaternion.identity);
			transform.localScale = Vector3.one;
		}
		else
		{
			transform.parent = null;
			transform.localScale = Vector3.one;
		}
		Utils.SetLayerRecursively(transform.gameObject, transform2 ? transform2.gameObject.layer : 0);
		ProjectileMoveScript projectileMoveScript = transform.gameObject.AddComponent<ProjectileMoveScript>();
		projectileMoveScript.itemProjectile = forId;
		projectileMoveScript.itemValueProjectile = itemValue;
		projectileMoveScript.itemValueLauncher = holdingItemItemValue;
		projectileMoveScript.itemActionProjectile = (ItemActionProjectile)((forId.Actions[0] is ItemActionProjectile) ? forId.Actions[0] : forId.Actions[1]);
		projectileMoveScript.ProjectileOwnerID = holdingEntity.entityId;
		projectileMoveScript.actionData = itemActionDataLauncher;
		projectileMoveScript.assetHandle = instanceHandle;
		transform.gameObject.SetActive(true);
		return transform;
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x0013258C File Offset: 0x0013078C
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		base.ItemActionEffects(_gameManager, _actionData, _firingState, _startPos, _direction, _userData);
		if (_firingState == 0)
		{
			return;
		}
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		int num = this.GetBurstCount(_actionData);
		if (num <= 0)
		{
			return;
		}
		for (int i = itemActionDataLauncher.projectileTs.Count - 1; i >= 0; i--)
		{
			Transform transform = itemActionDataLauncher.projectileTs[i];
			if (transform)
			{
				transform.GetComponent<ProjectileMoveScript>().Fire(_startPos, this.getDirectionOffset(itemActionDataLauncher, _direction, i), _actionData.invData.holdingEntity, this.hitmaskOverride, 0f, false);
			}
			itemActionDataLauncher.projectileTs.RemoveAt(i);
			if (--num <= 0)
			{
				break;
			}
		}
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x0013262F File Offset: 0x0013082F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ConsumeAmmo(ItemActionData _actionData)
	{
		_actionData.invData.itemValue.Meta -= this.GetBurstCount(_actionData);
	}

	// Token: 0x06003001 RID: 12289 RVA: 0x00132650 File Offset: 0x00130850
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int GetActionEffectsValues(ItemActionData _actionData, out Vector3 _startPos, out Vector3 _direction)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		Ray lookRay = itemActionDataLauncher.invData.holdingEntity.GetLookRay();
		_startPos = lookRay.origin;
		_direction = lookRay.direction;
		_direction = this.getDirectionOffset(itemActionDataLauncher, _direction, 0);
		return 0;
	}

	// Token: 0x04002501 RID: 9473
	public ItemValue LastProjectileType;

	// Token: 0x020005BC RID: 1468
	public class ItemActionDataLauncher : ItemActionRanged.ItemActionDataRanged
	{
		// Token: 0x06003003 RID: 12291 RVA: 0x001326AC File Offset: 0x001308AC
		public ItemActionDataLauncher(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
			this.projectileJointT = ((_invData.model != null) ? _invData.model.FindInChilds("ProjectileJoint", false) : null);
			this.projectileTs = new List<Transform>();
		}

		// Token: 0x04002502 RID: 9474
		public Transform projectileJointT;

		// Token: 0x04002503 RID: 9475
		public List<Transform> projectileTs;

		// Token: 0x04002504 RID: 9476
		public float strainPercent = 1f;

		// Token: 0x04002505 RID: 9477
		public float lastAttackStrainPercent;
	}
}
