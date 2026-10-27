using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A50 RID: 6736
	[Preserve]
	public class ActionPOIReset : BaseAction
	{
		// Token: 0x0600CCFA RID: 52474 RVA: 0x004ABF6A File Offset: 0x004AA16A
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

		// Token: 0x0600CCFB RID: 52475 RVA: 0x004ABF99 File Offset: 0x004AA199
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

		// Token: 0x0600CCFC RID: 52476 RVA: 0x004ABFA8 File Offset: 0x004AA1A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
		{
			yield return new WaitForSeconds(1f);
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
			yield break;
		}

		// Token: 0x0600CCFD RID: 52477 RVA: 0x004ABFB7 File Offset: 0x004AA1B7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPOIReset();
		}

		// Token: 0x0600CCFE RID: 52478 RVA: 0x004ABFBE File Offset: 0x004AA1BE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this._state = ActionPOIReset.State.Start;
		}

		// Token: 0x04009B7C RID: 39804
		[PublicizedFrom(EAccessModifier.Private)]
		public BaseAction.ActionCompleteStates _retVal;

		// Token: 0x04009B7D RID: 39805
		[PublicizedFrom(EAccessModifier.Private)]
		public ActionPOIReset.State _state;

		// Token: 0x02001A51 RID: 6737
		[PublicizedFrom(EAccessModifier.Private)]
		public enum State
		{
			// Token: 0x04009B7F RID: 39807
			Start,
			// Token: 0x04009B80 RID: 39808
			Wait
		}
	}
}
