using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B7 RID: 1463
[Preserve]
public class ItemActionExchangeBlock : ItemAction
{
	// Token: 0x06002FE1 RID: 12257 RVA: 0x001318E8 File Offset: 0x0012FAE8
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (!_props.Values.ContainsKey("Sourceblock"))
		{
			throw new Exception("Missing attribute 'sourceblock' in use_action 'ExchangeBlock'");
		}
		string text = _props.Values["Sourceblock"];
		this.sourceblock = ItemClass.GetItem(text, false).ToBlockValue(false);
		if (this.sourceblock.Equals(BlockValue.Air))
		{
			throw new Exception("Unknown block name '" + text + "' in use_action!");
		}
		if (!_props.Values.ContainsKey("Targetblock"))
		{
			throw new Exception("Missing attribute 'targetblock' in use_action 'ExchangeBlock'");
		}
		text = _props.Values["Targetblock"];
		this.targetBlock = ItemClass.GetItem(text, false).ToBlockValue(false);
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x001319A8 File Offset: 0x0012FBA8
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime > Constants.cBuildIntervall)
		{
			ItemInventoryData invData = _actionData.invData;
			WorldRayHitInfo hitInfo = ((EntityPlayerLocal)invData.holdingEntity).HitInfo;
			if (hitInfo.bHitValid && GameUtils.IsBlockOrTerrain(hitInfo.tag))
			{
				Vector3i blockPos = hitInfo.hit.blockPos;
				BlockValue block = invData.world.GetBlock(blockPos);
				if (block.Equals(this.sourceblock))
				{
					_actionData.lastUseTime = Time.time;
					if (block.type != this.targetBlock.type)
					{
						invData.world.SetBlockRPC(blockPos, this.targetBlock);
						invData.holdingEntity.PlayOneShot((this.soundStart != null) ? this.soundStart : "placeblock", false, false, false, null, 1f);
					}
				}
				invData.holdingEntity.RightArmAnimationAttack = true;
			}
		}
	}

	// Token: 0x040024F5 RID: 9461
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue sourceblock;

	// Token: 0x040024F6 RID: 9462
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue targetBlock;
}
