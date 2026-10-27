using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CD RID: 6605
	[Preserve]
	public class ActionBaseTeleport : ActionBaseTargetAction
	{
		// Token: 0x0600CAA0 RID: 51872 RVA: 0x0049EFF8 File Offset: 0x0049D1F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public void TeleportEntity(Entity entity, Vector3 position)
		{
			float floatValue = GameEventManager.GetFloatValue(entity as EntityAlive, this.teleportDelayText, 0.1f);
			GameManager.Instance.StartCoroutine(base.TeleportEntity(entity, position, floatValue));
		}

		// Token: 0x0600CAA1 RID: 51873 RVA: 0x0049F030 File Offset: 0x0049D230
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBaseTeleport.PropTeleportDelay, ref this.teleportDelayText);
		}

		// Token: 0x0600CAA2 RID: 51874 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x040098CE RID: 39118
		[PublicizedFrom(EAccessModifier.Protected)]
		public string teleportDelayText = "";

		// Token: 0x040098CF RID: 39119
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportDelay = "teleport_delay";
	}
}
