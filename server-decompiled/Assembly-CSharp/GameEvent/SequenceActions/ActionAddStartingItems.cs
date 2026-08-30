using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200193E RID: 6462
	[Preserve]
	public class ActionAddStartingItems : ActionBaseClientAction
	{
		// Token: 0x0600C675 RID: 50805 RVA: 0x0048F1F4 File Offset: 0x0048D3F4
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.SetupStartingItems();
			}
		}

		// Token: 0x0600C676 RID: 50806 RVA: 0x0048F211 File Offset: 0x0048D411
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
