using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001788 RID: 6024
	[Preserve]
	public class Context
	{
		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x0600BA92 RID: 47762 RVA: 0x004572A4 File Offset: 0x004554A4
		public List<string> AIPackages
		{
			get
			{
				return this.Self.AIPackages;
			}
		}

		// Token: 0x0600BA93 RID: 47763 RVA: 0x004572B1 File Offset: 0x004554B1
		public Context(EntityAlive _self)
		{
			this.Self = _self;
			this.World = GameManager.Instance.World;
			this.ConsiderationData = new ConsiderationData();
			this.ActionData = default(ActionData);
		}

		// Token: 0x04008BCE RID: 35790
		public EntityAlive Self;

		// Token: 0x04008BCF RID: 35791
		public World World;

		// Token: 0x04008BD0 RID: 35792
		public ConsiderationData ConsiderationData;

		// Token: 0x04008BD1 RID: 35793
		public ActionData ActionData;

		// Token: 0x04008BD2 RID: 35794
		public float updateTimer;
	}
}
