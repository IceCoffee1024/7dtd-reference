using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200177F RID: 6015
	[Preserve]
	public class UAITaskBase
	{
		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x0600BA64 RID: 47716 RVA: 0x004568A4 File Offset: 0x00454AA4
		// (set) Token: 0x0600BA65 RID: 47717 RVA: 0x004568AC File Offset: 0x00454AAC
		public string Name { get; set; }

		// Token: 0x0600BA66 RID: 47718 RVA: 0x004568B5 File Offset: 0x00454AB5
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

		// Token: 0x0600BA67 RID: 47719 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void initializeParameters()
		{
		}

		// Token: 0x0600BA68 RID: 47720 RVA: 0x004568F0 File Offset: 0x00454AF0
		public virtual void Start(Context _context)
		{
			_context.ActionData.Started = true;
			_context.ActionData.Executing = true;
		}

		// Token: 0x0600BA69 RID: 47721 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Update(Context _context)
		{
		}

		// Token: 0x0600BA6A RID: 47722 RVA: 0x0045690A File Offset: 0x00454B0A
		public virtual void Stop(Context _context)
		{
			_context.ActionData.Executing = false;
		}

		// Token: 0x0600BA6B RID: 47723 RVA: 0x00456918 File Offset: 0x00454B18
		public virtual void Reset(Context _context)
		{
			_context.ActionData.ClearData();
		}

		// Token: 0x04008BBD RID: 35773
		public Dictionary<string, string> Parameters = new Dictionary<string, string>();

		// Token: 0x04008BBE RID: 35774
		[PublicizedFrom(EAccessModifier.Private)]
		public bool parmsInitialized;
	}
}
