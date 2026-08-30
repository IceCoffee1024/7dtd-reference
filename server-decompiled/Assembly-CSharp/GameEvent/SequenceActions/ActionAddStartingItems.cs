using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001964 RID: 6500
	[Preserve]
	public class ActionAddStartingItems : ActionBaseClientAction
	{
		// Token: 0x0600C7FC RID: 51196 RVA: 0x00495540 File Offset: 0x00493740
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.SetupStartingItems();
			}
		}

		// Token: 0x0600C7FD RID: 51197 RVA: 0x0049555D File Offset: 0x0049375D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddStartingItems
			{
				targetGroup = this.targetGroup
			};
		}
	}
}
