using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E5 RID: 6629
	[Preserve]
	public class ActionGetLandClaimPosition : BaseAction
	{
		// Token: 0x0600CB03 RID: 51971 RVA: 0x004A2B6C File Offset: 0x004A0D6C
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			World world = GameManager.Instance.World;
			List<Vector3i> lpblocks = world.GetGameManager().GetPersistentPlayerList().GetPlayerDataFromEntityID(base.Owner.Target.entityId)
				.LPBlocks;
			if (lpblocks == null || lpblocks.Count == 0)
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			int num = (GameStats.GetInt(EnumGameStats.LandClaimSize) - 1) / 2;
			int num2 = num * num;
			bool flag = false;
			for (int i = 0; i < lpblocks.Count; i++)
			{
				Vector3i vector3i = lpblocks[i];
				TEFeatureLandClaim selfOrFeature = world.GetTileEntity(vector3i).GetSelfOrFeature<TEFeatureLandClaim>();
				if (selfOrFeature != null && selfOrFeature.IsPrimary() && base.Owner.Target.GetDistanceSq(vector3i) < (float)num2)
				{
					flag = true;
					base.Owner.TargetPosition = vector3i;
				}
			}
			if (!flag)
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB04 RID: 51972 RVA: 0x004A2C37 File Offset: 0x004A0E37
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionGetLandClaimPosition();
		}
	}
}
