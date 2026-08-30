using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CC RID: 6604
	[Preserve]
	public class ActionPOIReset : BaseAction
	{
		// Token: 0x0600C905 RID: 51461 RVA: 0x0049DBAE File Offset: 0x0049BDAE
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

		// Token: 0x0600C906 RID: 51462 RVA: 0x0049DBDD File Offset: 0x0049BDDD
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

		// Token: 0x0600C907 RID: 51463 RVA: 0x0049DBEC File Offset: 0x0049BDEC
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
		{
			yield return new WaitForSeconds(1f);
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
			yield break;
		}

		// Token: 0x0600C908 RID: 51464 RVA: 0x0049DBFB File Offset: 0x0049BDFB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPOIReset();
		}

		// Token: 0x0600C909 RID: 51465 RVA: 0x0049DC02 File Offset: 0x0049BE02
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this._state = ActionPOIReset.State.Start;
		}

		// Token: 0x0400989C RID: 39068
		[PublicizedFrom(EAccessModifier.Private)]
		public BaseAction.ActionCompleteStates _retVal;

		// Token: 0x0400989D RID: 39069
		[PublicizedFrom(EAccessModifier.Private)]
		public ActionPOIReset.State _state;

		// Token: 0x020019CD RID: 6605
		[PublicizedFrom(EAccessModifier.Private)]
		public enum State
		{
			// Token: 0x0400989F RID: 39071
			Start,
			// Token: 0x040098A0 RID: 39072
			Wait
		}
	}
}
