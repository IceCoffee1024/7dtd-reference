using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197B RID: 6523
	[Preserve]
	public class ActionRemoveEntities : BaseAction
	{
		// Token: 0x0600C775 RID: 51061 RVA: 0x004964EC File Offset: 0x004946EC
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.targetGroup != "")
			{
				List<Entity> entityGroup = base.Owner.GetEntityGroup(this.targetGroup);
				if (entityGroup != null)
				{
					GameEventManager gameEventManager = GameEventManager.Current;
					for (int i = 0; i < entityGroup.Count; i++)
					{
						this.HandleRemoveData(gameEventManager, entityGroup[i]);
						GameManager.Instance.StartCoroutine(this.removeLater(entityGroup[i]));
					}
				}
				return BaseAction.ActionCompleteStates.Complete;
			}
			if (base.Owner.Target != null)
			{
				this.HandleRemoveData(GameEventManager.Current, base.Owner.Target);
				GameManager.Instance.StartCoroutine(this.removeLater(base.Owner.Target));
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C776 RID: 51062 RVA: 0x004965A5 File Offset: 0x004947A5
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRemoveData(GameEventManager gm, Entity ent)
		{
			gm.RemoveSpawnedEntry(ent);
		}

		// Token: 0x0600C777 RID: 51063 RVA: 0x004965AE File Offset: 0x004947AE
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator removeLater(Entity e)
		{
			yield return new WaitForSeconds(0.25f);
			EntityVehicle entityVehicle = e as EntityVehicle;
			if (entityVehicle != null)
			{
				entityVehicle.Kill();
			}
			if (e != null)
			{
				GameManager.Instance.World.RemoveEntity(e.entityId, EnumRemoveEntityReason.Killed);
			}
			yield break;
		}

		// Token: 0x0600C778 RID: 51064 RVA: 0x004965BD File Offset: 0x004947BD
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRemoveEntities.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600C779 RID: 51065 RVA: 0x004965D7 File Offset: 0x004947D7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveEntities
			{
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x04009721 RID: 38689
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009722 RID: 38690
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";
	}
}
