using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017AB RID: 6059
	[Preserve]
	public class UAITaskBase
	{
		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x0600BBBB RID: 48059 RVA: 0x0045ADC4 File Offset: 0x00458FC4
		// (set) Token: 0x0600BBBC RID: 48060 RVA: 0x0045ADCC File Offset: 0x00458FCC
		public string Name { get; set; }

		// Token: 0x0600BBBD RID: 48061 RVA: 0x0045ADD5 File Offset: 0x00458FD5
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

		// Token: 0x0600BBBE RID: 48062 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void initializeParameters()
		{
		}

		// Token: 0x0600BBBF RID: 48063 RVA: 0x0045AE10 File Offset: 0x00459010
		public virtual void Start(Context _context)
		{
			_context.ActionData.Started = true;
			_context.ActionData.Executing = true;
		}

		// Token: 0x0600BBC0 RID: 48064 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Update(Context _context)
		{
		}

		// Token: 0x0600BBC1 RID: 48065 RVA: 0x0045AE2A File Offset: 0x0045902A
		public virtual void Stop(Context _context)
		{
			_context.ActionData.Executing = false;
		}

		// Token: 0x0600BBC2 RID: 48066 RVA: 0x0045AE38 File Offset: 0x00459038
		public virtual void Reset(Context _context)
		{
			_context.ActionData.ClearData();
		}

		// Token: 0x04008C82 RID: 35970
		public Dictionary<string, string> Parameters = new Dictionary<string, string>();

		// Token: 0x04008C83 RID: 35971
		[PublicizedFrom(EAccessModifier.Private)]
		public bool parmsInitialized;
	}
}
