using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001759 RID: 5977
	[Preserve]
	public class UAITaskBase
	{
		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x0600B8E1 RID: 47329 RVA: 0x00450B2C File Offset: 0x0044ED2C
		// (set) Token: 0x0600B8E2 RID: 47330 RVA: 0x00450B34 File Offset: 0x0044ED34
		public string Name { get; set; }

		// Token: 0x0600B8E3 RID: 47331 RVA: 0x00450B3D File Offset: 0x0044ED3D
		public virtual void Init(Context _context)
		{
			_context.ActionData.Initialized = true;
			_context.ActionData.Started = false;
			_context.ActionData.Executing = false;
			if (!this.parmsInitialized)
			{
				this.initializeParameters();
				this.parmsInitialized = true;
			}
		}

		// Token: 0x0600B8E4 RID: 47332 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void initializeParameters()
		{
		}

		// Token: 0x0600B8E5 RID: 47333 RVA: 0x00450B78 File Offset: 0x0044ED78
		public virtual void Start(Context _context)
		{
			_context.ActionData.Started = true;
			_context.ActionData.Executing = true;
		}

		// Token: 0x0600B8E6 RID: 47334 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Update(Context _context)
		{
		}

		// Token: 0x0600B8E7 RID: 47335 RVA: 0x00450B92 File Offset: 0x0044ED92
		public virtual void Stop(Context _context)
		{
			_context.ActionData.Executing = false;
		}

		// Token: 0x0600B8E8 RID: 47336 RVA: 0x00450BA0 File Offset: 0x0044EDA0
		public virtual void Reset(Context _context)
		{
			_context.ActionData.ClearData();
		}

		// Token: 0x04008AA1 RID: 35489
		public Dictionary<string, string> Parameters = new Dictionary<string, string>();

		// Token: 0x04008AA2 RID: 35490
		[PublicizedFrom(EAccessModifier.Private)]
		public bool parmsInitialized;
	}
}
