using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017DC RID: 6108
	[Preserve]
	public class UAITaskBase
	{
		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x0600BCCF RID: 48335 RVA: 0x0045E770 File Offset: 0x0045C970
		// (set) Token: 0x0600BCD0 RID: 48336 RVA: 0x0045E778 File Offset: 0x0045C978
		public string Name { get; set; }

		// Token: 0x0600BCD1 RID: 48337 RVA: 0x0045E781 File Offset: 0x0045C981
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

		// Token: 0x0600BCD2 RID: 48338 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void initializeParameters()
		{
		}

		// Token: 0x0600BCD3 RID: 48339 RVA: 0x0045E7BC File Offset: 0x0045C9BC
		public virtual void Start(Context _context)
		{
			_context.ActionData.Started = true;
			_context.ActionData.Executing = true;
		}

		// Token: 0x0600BCD4 RID: 48340 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Update(Context _context)
		{
		}

		// Token: 0x0600BCD5 RID: 48341 RVA: 0x0045E7D6 File Offset: 0x0045C9D6
		public virtual void Stop(Context _context)
		{
			_context.ActionData.Executing = false;
		}

		// Token: 0x0600BCD6 RID: 48342 RVA: 0x0045E7E4 File Offset: 0x0045C9E4
		public virtual void Reset(Context _context)
		{
			_context.ActionData.ClearData();
		}

		// Token: 0x04008D6B RID: 36203
		public Dictionary<string, string> Parameters = new Dictionary<string, string>();

		// Token: 0x04008D6C RID: 36204
		[PublicizedFrom(EAccessModifier.Private)]
		public bool parmsInitialized;
	}
}
