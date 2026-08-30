using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B3 RID: 1459
[Preserve]
public class ItemActionLauncher : ItemActionRanged
{
	// Token: 0x06002FA3 RID: 12195 RVA: 0x00130B47 File Offset: 0x0012ED47
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionLauncher.ItemActionDataLauncher(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x00130B50 File Offset: 0x0012ED50
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x00130B5C File Offset: 0x0012ED5C
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

	// Token: 0x06002FA6 RID: 12198 RVA: 0x00130BCF File Offset: 0x0012EDCF
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		this.DeleteProjectiles(_data);
	}

	// Token: 0x06002FA7 RID: 12199 RVA: 0x00130BE0 File Offset: 0x0012EDE0
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

	// Token: 0x06002FA8 RID: 12200 RVA: 0x00130C38 File Offset: 0x0012EE38
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

	// Token: 0x06002FA9 RID: 12201 RVA: 0x00130C94 File Offset: 0x0012EE94
	public override void CancelReload(ItemActionData _actionData, bool holsterWeapon)
	{
		base.CancelReload(_actionData, holsterWeapon);
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_actionData;
		this.ClampAmmoCount(itemActionDataLauncher);
	}

	// Token: 0x06002FAA RID: 12202 RVA: 0x00130CB8 File Offset: 0x0012EEB8
	public override void SwapAmmoType(EntityAlive _entity, int _selectedIndex = -1)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = (ItemActionLauncher.ItemActionDataLauncher)_entity.inventory.holdingItemData.actionData[0];
		this.ClampAmmoCount(itemActionDataLauncher);
		base.SwapAmmoType(_entity, _selectedIndex);
	}

	// Token: 0x06002FAB RID: 12203 RVA: 0x00130CF0 File Offset: 0x0012EEF0
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

	// Token: 0x06002FAC RID: 12204 RVA: 0x00130D64 File Offset: 0x0012EF64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 fireShot(int _shotIdx, ItemActionRanged.ItemActionDataRanged _actionData, ref bool hitEntity)
	{
		hitEntity = true;
		return Vector3.zero;
	}

	// Token: 0x06002FAD RID: 12205 RVA: 0x00130D70 File Offset: 0x0012EF70
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
		Transform transform = forId.CloneModel(_actionData.invData.world, itemValue, Vector3.zero, null, BlockShape.MeshPurpose.World, default(TextureFullArray));
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
		transform.gameObject.SetActive(true);
		return transform;
	}

	// Token: 0x06002FAE RID: 12206 RVA: 0x00130F00 File Offset: 0x0012F100
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

	// Token: 0x06002FAF RID: 12207 RVA: 0x00130FA3 File Offset: 0x0012F1A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ConsumeAmmo(ItemActionData _actionData)
	{
		_actionData.invData.itemValue.Meta -= this.GetBurstCount(_actionData);
	}

	// Token: 0x06002FB0 RID: 12208 RVA: 0x00130FC4 File Offset: 0x0012F1C4
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

	// Token: 0x040024ED RID: 9453
	public ItemValue LastProjectileType;

	// Token: 0x020005B4 RID: 1460
	public class ItemActionDataLauncher : ItemActionRanged.ItemActionDataRanged
	{
		// Token: 0x06002FB2 RID: 12210 RVA: 0x00131020 File Offset: 0x0012F220
		public ItemActionDataLauncher(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
			this.projectileJointT = ((_invData.model != null) ? _invData.model.FindInChilds("ProjectileJoint", false) : null);
			this.projectileTs = new List<Transform>();
		}

		// Token: 0x040024EE RID: 9454
		public Transform projectileJointT;

		// Token: 0x040024EF RID: 9455
		public List<Transform> projectileTs;

		// Token: 0x040024F0 RID: 9456
		public float strainPercent = 1f;

		// Token: 0x040024F1 RID: 9457
		public float lastAttackStrainPercent;
	}
}
