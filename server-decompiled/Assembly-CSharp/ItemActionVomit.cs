using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005ED RID: 1517
[Preserve]
public class ItemActionVomit : ItemActionLauncher
{
	// Token: 0x06003166 RID: 12646 RVA: 0x0014001E File Offset: 0x0013E21E
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionVomit.ItemActionDataVomit(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x00140028 File Offset: 0x0013E228
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		_props.ParseInt("AnimType", ref this.animType);
		this.warningDelay = 1.2f;
		_props.ParseFloat("WarningDelay", ref this.warningDelay);
		this.warningMax = 3;
		_props.ParseInt("WarningMax", ref this.warningMax);
		_props.ParseString("Sound_warning", ref this.soundWarning);
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x00140092 File Offset: 0x0013E292
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetAttack(ItemActionVomit.ItemActionDataVomit _actionData)
	{
		_actionData.numWarningsPlayed = 0;
		_actionData.warningTime = 0f;
		_actionData.bAttackStarted = false;
		_actionData.isActive = false;
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x001400B4 File Offset: 0x0013E2B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int GetActionEffectsValues(ItemActionData _actionData, out Vector3 _startPos, out Vector3 _direction)
	{
		ItemActionVomit.ItemActionDataVomit itemActionDataVomit = (ItemActionVomit.ItemActionDataVomit)_actionData;
		Ray lookRay = itemActionDataVomit.invData.holdingEntity.GetLookRay();
		_startPos = lookRay.origin;
		_direction = lookRay.direction;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		ItemValue holdingItemItemValue = holdingEntity.inventory.holdingItemItemValue;
		ItemClass forId = ItemClass.GetForId(ItemClass.GetItem(this.MagazineItemNames[(int)holdingItemItemValue.SelectedAmmoTypeIndex], false).type);
		if (((ItemActionProjectile)((forId.Actions[0] is ItemActionProjectile) ? forId.Actions[0] : forId.Actions[1])).FlyTime < 0f)
		{
			Transform transform = itemActionDataVomit.projectileJointT;
			if (!transform)
			{
				transform = holdingEntity.emodel.GetRightHandTransform();
			}
			_startPos = transform.position + Origin.position;
			EntityAlive attackTarget = holdingEntity.GetAttackTarget();
			if (attackTarget)
			{
				Vector3 chestPosition = attackTarget.getChestPosition();
				if (holdingEntity.IsInFrontOfMe(chestPosition))
				{
					_direction = chestPosition;
					return 1;
				}
			}
			return 0;
		}
		_direction = this.getDirectionOffset(itemActionDataVomit, _direction, 0);
		return 0;
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x001401DC File Offset: 0x0013E3DC
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		ItemActionVomit.ItemActionDataVomit itemActionDataVomit = (ItemActionVomit.ItemActionDataVomit)_actionData;
		if (itemActionDataVomit.muzzle == null)
		{
			itemActionDataVomit.muzzle = _actionData.invData.holdingEntity.emodel.GetRightHandTransform();
		}
		if (_firingState != 0)
		{
			itemActionDataVomit.numVomits++;
			bool flag = _userData > 0;
			Vector3 vector = _direction;
			Vector3 direction = itemActionDataVomit.invData.holdingEntity.GetLookRay().direction;
			int burstCount = this.GetBurstCount(_actionData);
			for (int i = 0; i < burstCount; i++)
			{
				if (!flag)
				{
					vector = this.getDirectionRandomOffset(itemActionDataVomit, direction);
				}
				base.instantiateProjectile(_actionData, default(Vector3)).GetComponent<ProjectileMoveScript>().Fire(_startPos, vector, _actionData.invData.holdingEntity, this.hitmaskOverride, -1f, flag);
			}
		}
		base.ItemActionEffects(_gameManager, _actionData, _firingState, _startPos, _direction, _userData);
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x001402BC File Offset: 0x0013E4BC
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionVomit.ItemActionDataVomit itemActionDataVomit = (ItemActionVomit.ItemActionDataVomit)_actionData;
		if (_bReleased)
		{
			base.ExecuteAction(_actionData, _bReleased);
			this.resetAttack(itemActionDataVomit);
			return;
		}
		itemActionDataVomit.isActive = true;
		float time = Time.time;
		if (time - itemActionDataVomit.m_LastShotTime < this.Delay)
		{
			return;
		}
		if (itemActionDataVomit.warningTime > 0f && time < itemActionDataVomit.warningTime)
		{
			return;
		}
		if (!itemActionDataVomit.bAttackStarted)
		{
			EntityAlive holdingEntity = _actionData.invData.holdingEntity;
			if (itemActionDataVomit.numWarningsPlayed < this.warningMax - 1 && holdingEntity.rand.RandomFloat < 0.5f)
			{
				itemActionDataVomit.numWarningsPlayed++;
				itemActionDataVomit.warningTime = time + this.warningDelay;
				holdingEntity.PlayOneShot(this.soundWarning, false, false, false, null, 1f);
				holdingEntity.Raging = true;
				return;
			}
			itemActionDataVomit.bAttackStarted = true;
			itemActionDataVomit.numVomits = 0;
			holdingEntity.StartAnimAction(this.animType + 3000);
			if (this.warningMax > 0)
			{
				holdingEntity.PlayOneShot(this.soundWarning, false, false, false, null, 1f);
				itemActionDataVomit.warningTime = time + this.warningDelay;
				return;
			}
		}
		if (itemActionDataVomit.numVomits >= this.GetMaxAmmoCount(itemActionDataVomit))
		{
			itemActionDataVomit.isActive = false;
			return;
		}
		itemActionDataVomit.curBurstCount = 0;
		base.ExecuteAction(_actionData, _bReleased);
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x001403FD File Offset: 0x0013E5FD
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionVomit.ItemActionDataVomit)_actionData).isActive;
	}

	// Token: 0x0400262B RID: 9771
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRadius = -1f;

	// Token: 0x0400262C RID: 9772
	[PublicizedFrom(EAccessModifier.Private)]
	public int animType;

	// Token: 0x0400262D RID: 9773
	[PublicizedFrom(EAccessModifier.Private)]
	public float warningDelay;

	// Token: 0x0400262E RID: 9774
	[PublicizedFrom(EAccessModifier.Private)]
	public int warningMax;

	// Token: 0x0400262F RID: 9775
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundWarning;

	// Token: 0x020005EE RID: 1518
	public class ItemActionDataVomit : ItemActionLauncher.ItemActionDataLauncher
	{
		// Token: 0x0600316E RID: 12654 RVA: 0x0012C46D File Offset: 0x0012A66D
		public ItemActionDataVomit(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002630 RID: 9776
		public float warningTime;

		// Token: 0x04002631 RID: 9777
		public int numWarningsPlayed;

		// Token: 0x04002632 RID: 9778
		public int numVomits;

		// Token: 0x04002633 RID: 9779
		public bool bAttackStarted;

		// Token: 0x04002634 RID: 9780
		public bool isActive;
	}
}
