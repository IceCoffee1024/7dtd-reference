using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200058F RID: 1423
[Preserve]
public class ItemActionActivate : ItemAction
{
	// Token: 0x06002EDF RID: 11999 RVA: 0x00128258 File Offset: 0x00126458
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		_actionData.lastUseTime = Time.time;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		if (EffectManager.GetValue(PassiveEffects.DisableItem, holdingEntity.inventory.holdingItemItemValue, 0f, holdingEntity, null, _actionData.invData.item.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			_actionData.lastUseTime = Time.time + 1f;
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			return;
		}
		if (this.soundStart != null)
		{
			_actionData.invData.holdingEntity.PlayOneShot(this.soundStart, this.Sound_in_head, false, false, null, 1f);
		}
		_actionData.invData.holdingEntity.inventory.holdingItem.OnHoldingItemActivated(_actionData.invData.holdingEntity.inventory.holdingItemData);
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x0012834F File Offset: 0x0012654F
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.HasExecuted;
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x00128357 File Offset: 0x00126557
	public override void CancelAction(ItemActionData _actionData)
	{
		base.CancelAction(_actionData);
		_actionData.HasExecuted = false;
		_actionData.lastUseTime = Time.time;
	}
}
