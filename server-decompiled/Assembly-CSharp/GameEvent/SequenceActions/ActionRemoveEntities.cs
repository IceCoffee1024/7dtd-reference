using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CD RID: 6605
	[Preserve]
	public class ActionRemoveEntities : BaseAction
	{
		// Token: 0x0600CA54 RID: 51796 RVA: 0x004A0CF4 File Offset: 0x0049EEF4
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

		// Token: 0x0600CA55 RID: 51797 RVA: 0x004A0DAD File Offset: 0x0049EFAD
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRemoveData(GameEventManager gm, Entity ent)
		{
			gm.RemoveSpawnedEntry(ent);
		}

		// Token: 0x0600CA56 RID: 51798 RVA: 0x004A0DB6 File Offset: 0x0049EFB6
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

		// Token: 0x0600CA57 RID: 51799 RVA: 0x004A0DC5 File Offset: 0x0049EFC5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRemoveEntities.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600CA58 RID: 51800 RVA: 0x004A0DDF File Offset: 0x0049EFDF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveEntities
			{
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x04009910 RID: 39184
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009911 RID: 39185
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";
	}
}
