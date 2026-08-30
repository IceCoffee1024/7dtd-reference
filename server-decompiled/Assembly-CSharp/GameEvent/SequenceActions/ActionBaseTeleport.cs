using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001970 RID: 6512
	[Preserve]
	public class ActionBaseTeleport : ActionBaseTargetAction
	{
		// Token: 0x0600C834 RID: 51252 RVA: 0x0049701C File Offset: 0x0049521C
		[PublicizedFrom(EAccessModifier.Protected)]
		public void TeleportEntity(Entity entity, Vector3 position)
		{
			float floatValue = GameEventManager.GetFloatValue(entity as EntityAlive, this.teleportDelayText, 0.1f);
			GameManager.Instance.StartCoroutine(base.TeleportEntity(entity, position, floatValue));
		}

		// Token: 0x0600C835 RID: 51253 RVA: 0x00497054 File Offset: 0x00495254
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBaseTeleport.PropTeleportDelay, ref this.teleportDelayText);
		}

		// Token: 0x0600C836 RID: 51254 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x0400971F RID: 38687
		[PublicizedFrom(EAccessModifier.Protected)]
		public string teleportDelayText = "";

		// Token: 0x04009720 RID: 38688
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportDelay = "teleport_delay";
	}
}
