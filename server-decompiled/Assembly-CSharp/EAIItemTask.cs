using System;
using System.Collections.Generic;

// Token: 0x02000442 RID: 1090
public class EAIItemTask : EAIBase
{
	// Token: 0x06002147 RID: 8519 RVA: 0x000C8A77 File Offset: 0x000C6C77
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x000C8A80 File Offset: 0x000C6C80
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x06002149 RID: 8521 RVA: 0x000C8A89 File Offset: 0x000C6C89
	public override bool CanExecute()
	{
		return !string.IsNullOrEmpty(this.ItemKey);
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x000C8A99 File Offset: 0x000C6C99
	public override bool Continue()
	{
		return base.Continue();
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x000C8AA1 File Offset: 0x000C6CA1
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000C8AA9 File Offset: 0x000C6CA9
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x040016E6 RID: 5862
	public string ItemKey;
}
