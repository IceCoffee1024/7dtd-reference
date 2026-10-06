using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1F RID: 6687
	[Preserve]
	public class ActionPOIReset : BaseAction
	{
		// Token: 0x0600CBE6 RID: 52198 RVA: 0x004A846E File Offset: 0x004A666E
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this._state == ActionPOIReset.State.Start)
			{
				this._state = ActionPOIReset.State.Wait;
				this._retVal = BaseAction.ActionCompleteStates.InComplete;
				GameManager.Instance.StartCoroutine(this.onPerformAction());
			}
			return this._retVal;
		}

		// Token: 0x0600CBE7 RID: 52199 RVA: 0x004A849D File Offset: 0x004A669D
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator onPerformAction()
		{
			Vector3i poiposition = base.Owner.POIPosition;
			World world = GameManager.Instance.World;
			if (base.Owner.POIInstance == null)
			{
				this._retVal = BaseAction.ActionCompleteStates.InCompleteRefund;
				yield break;
			}
			List<PrefabInstance> prefabsIntersecting = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabsIntersecting(base.Owner.POIInstance);
			int num = -1;
			if (!GameManager.Instance.IsEditMode() && !GameUtils.IsPlaytesting())
			{
				num = ((base.Owner.Requester != null) ? base.Owner.Requester.entityId : (-1));
			}
			yield return world.ResetPOIS(prefabsIntersecting, QuestEventManager.manualResetTag, num, null, null);
			this._retVal = BaseAction.ActionCompleteStates.Complete;
			yield break;
		}

		// Token: 0x0600CBE8 RID: 52200 RVA: 0x004A84AC File Offset: 0x004A66AC
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
		{
			yield return new WaitForSeconds(1f);
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
			yield break;
		}

		// Token: 0x0600CBE9 RID: 52201 RVA: 0x004A84BB File Offset: 0x004A66BB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPOIReset();
		}

		// Token: 0x0600CBEA RID: 52202 RVA: 0x004A84C2 File Offset: 0x004A66C2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this._state = ActionPOIReset.State.Start;
		}

		// Token: 0x04009A93 RID: 39571
		[PublicizedFrom(EAccessModifier.Private)]
		public BaseAction.ActionCompleteStates _retVal;

		// Token: 0x04009A94 RID: 39572
		[PublicizedFrom(EAccessModifier.Private)]
		public ActionPOIReset.State _state;

		// Token: 0x02001A20 RID: 6688
		[PublicizedFrom(EAccessModifier.Private)]
		public enum State
		{
			// Token: 0x04009A96 RID: 39574
			Start,
			// Token: 0x04009A97 RID: 39575
			Wait
		}
	}
}
