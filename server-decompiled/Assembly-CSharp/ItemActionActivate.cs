using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000590 RID: 1424
[Preserve]
public class ItemActionActivate : ItemAction
{
	// Token: 0x06002F21 RID: 12065 RVA: 0x00128D78 File Offset: 0x00126F78
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

	// Token: 0x06002F22 RID: 12066 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x00128E6F File Offset: 0x0012706F
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.HasExecuted;
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x00128E77 File Offset: 0x00127077
	public override void CancelAction(ItemActionData _actionData)
	{
		base.CancelAction(_actionData);
		_actionData.HasExecuted = false;
		_actionData.lastUseTime = Time.time;
	}
}
