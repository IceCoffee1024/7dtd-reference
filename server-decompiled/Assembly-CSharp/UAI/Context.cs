using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017B4 RID: 6068
	[Preserve]
	public class Context
	{
		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x0600BBE9 RID: 48105 RVA: 0x0045B7C4 File Offset: 0x004599C4
		public List<string> AIPackages
		{
			get
			{
				return this.Self.AIPackages;
			}
		}

		// Token: 0x0600BBEA RID: 48106 RVA: 0x0045B7D1 File Offset: 0x004599D1
		public Context(EntityAlive _self)
		{
			this.Self = _self;
			this.World = GameManager.Instance.World;
			this.ConsiderationData = new ConsiderationData();
			this.ActionData = default(ActionData);
		}

		// Token: 0x04008C93 RID: 35987
		public EntityAlive Self;

		// Token: 0x04008C94 RID: 35988
		public World World;

		// Token: 0x04008C95 RID: 35989
		public ConsiderationData ConsiderationData;

		// Token: 0x04008C96 RID: 35990
		public ActionData ActionData;

		// Token: 0x04008C97 RID: 35991
		public float updateTimer;
	}
}
