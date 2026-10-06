using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199C RID: 6556
	[Preserve]
	public class ActionBaseTeleport : ActionBaseTargetAction
	{
		// Token: 0x0600C98C RID: 51596 RVA: 0x0049B4F8 File Offset: 0x004996F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public void TeleportEntity(Entity entity, Vector3 position)
		{
			float floatValue = GameEventManager.GetFloatValue(entity as EntityAlive, this.teleportDelayText, 0.1f);
			GameManager.Instance.StartCoroutine(base.TeleportEntity(entity, position, floatValue));
		}

		// Token: 0x0600C98D RID: 51597 RVA: 0x0049B530 File Offset: 0x00499730
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBaseTeleport.PropTeleportDelay, ref this.teleportDelayText);
		}

		// Token: 0x0600C98E RID: 51598 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x040097E5 RID: 38885
		[PublicizedFrom(EAccessModifier.Protected)]
		public string teleportDelayText = "";

		// Token: 0x040097E6 RID: 38886
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportDelay = "teleport_delay";
	}
}
