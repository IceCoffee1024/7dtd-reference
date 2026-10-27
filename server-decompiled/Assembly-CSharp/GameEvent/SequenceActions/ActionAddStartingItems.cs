using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C1 RID: 6593
	[Preserve]
	public class ActionAddStartingItems : ActionBaseClientAction
	{
		// Token: 0x0600CA68 RID: 51816 RVA: 0x0049D530 File Offset: 0x0049B730
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.SetupStartingItems();
			}
		}

		// Token: 0x0600CA69 RID: 51817 RVA: 0x0049D54D File Offset: 0x0049B74D
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
