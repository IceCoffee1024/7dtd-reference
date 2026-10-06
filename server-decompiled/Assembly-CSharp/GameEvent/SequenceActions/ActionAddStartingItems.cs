using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001990 RID: 6544
	[Preserve]
	public class ActionAddStartingItems : ActionBaseClientAction
	{
		// Token: 0x0600C954 RID: 51540 RVA: 0x00499A30 File Offset: 0x00497C30
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.SetupStartingItems();
			}
		}

		// Token: 0x0600C955 RID: 51541 RVA: 0x00499A4D File Offset: 0x00497C4D
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
