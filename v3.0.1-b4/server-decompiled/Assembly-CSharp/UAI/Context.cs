using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001762 RID: 5986
	[Preserve]
	public class Context
	{
		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x0600B90F RID: 47375 RVA: 0x0045152C File Offset: 0x0044F72C
		public List<string> AIPackages
		{
			get
			{
				return this.Self.AIPackages;
			}
		}

		// Token: 0x0600B910 RID: 47376 RVA: 0x00451539 File Offset: 0x0044F739
		public Context(EntityAlive _self)
		{
			this.Self = _self;
			this.World = GameManager.Instance.World;
			this.ConsiderationData = new ConsiderationData();
			this.ActionData = default(ActionData);
		}

		// Token: 0x04008AB2 RID: 35506
		public EntityAlive Self;

		// Token: 0x04008AB3 RID: 35507
		public World World;

		// Token: 0x04008AB4 RID: 35508
		public ConsiderationData ConsiderationData;

		// Token: 0x04008AB5 RID: 35509
		public ActionData ActionData;

		// Token: 0x04008AB6 RID: 35510
		public float updateTimer;
	}
}
