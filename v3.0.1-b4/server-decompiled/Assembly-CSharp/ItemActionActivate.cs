using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200058A RID: 1418
[Preserve]
public class ItemActionActivate : ItemAction
{
	// Token: 0x06002EB1 RID: 11953 RVA: 0x00127168 File Offset: 0x00125368
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

	// Token: 0x06002EB2 RID: 11954 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x06002EB3 RID: 11955 RVA: 0x0012725F File Offset: 0x0012545F
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.HasExecuted;
	}

	// Token: 0x06002EB4 RID: 11956 RVA: 0x00127267 File Offset: 0x00125467
	public override void CancelAction(ItemActionData _actionData)
	{
		base.CancelAction(_actionData);
		_actionData.HasExecuted = false;
		_actionData.lastUseTime = Time.time;
	}
}
