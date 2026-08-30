using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F3 RID: 6643
	[Preserve]
	public class ActionPOIReset : BaseAction
	{
		// Token: 0x0600CA8E RID: 51854 RVA: 0x004A3FA2 File Offset: 0x004A21A2
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

		// Token: 0x0600CA8F RID: 51855 RVA: 0x004A3FD1 File Offset: 0x004A21D1
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

		// Token: 0x0600CA90 RID: 51856 RVA: 0x004A3FE0 File Offset: 0x004A21E0
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator UpdateBlocks(List<BlockChangeInfo> blockChanges)
		{
			yield return new WaitForSeconds(1f);
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
			yield break;
		}

		// Token: 0x0600CA91 RID: 51857 RVA: 0x004A3FEF File Offset: 0x004A21EF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPOIReset();
		}

		// Token: 0x0600CA92 RID: 51858 RVA: 0x004A3FF6 File Offset: 0x004A21F6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this._state = ActionPOIReset.State.Start;
		}

		// Token: 0x040099CD RID: 39373
		[PublicizedFrom(EAccessModifier.Private)]
		public BaseAction.ActionCompleteStates _retVal;

		// Token: 0x040099CE RID: 39374
		[PublicizedFrom(EAccessModifier.Private)]
		public ActionPOIReset.State _state;

		// Token: 0x020019F4 RID: 6644
		[PublicizedFrom(EAccessModifier.Private)]
		public enum State
		{
			// Token: 0x040099D0 RID: 39376
			Start,
			// Token: 0x040099D1 RID: 39377
			Wait
		}
	}
}
