using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A1 RID: 6561
	[Preserve]
	public class ActionRemoveEntities : BaseAction
	{
		// Token: 0x0600C8FC RID: 51452 RVA: 0x0049C830 File Offset: 0x0049AA30
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

		// Token: 0x0600C8FD RID: 51453 RVA: 0x0049C8E9 File Offset: 0x0049AAE9
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRemoveData(GameEventManager gm, Entity ent)
		{
			gm.RemoveSpawnedEntry(ent);
		}

		// Token: 0x0600C8FE RID: 51454 RVA: 0x0049C8F2 File Offset: 0x0049AAF2
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

		// Token: 0x0600C8FF RID: 51455 RVA: 0x0049C901 File Offset: 0x0049AB01
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRemoveEntities.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600C900 RID: 51456 RVA: 0x0049C91B File Offset: 0x0049AB1B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveEntities
			{
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x0400984A RID: 38986
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x0400984B RID: 38987
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";
	}
}
