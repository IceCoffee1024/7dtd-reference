using System;
using System.Collections.Generic;

// Token: 0x0200044A RID: 1098
public class EAIItemTask : EAIBase
{
	// Token: 0x06002158 RID: 8536 RVA: 0x000C96E3 File Offset: 0x000C78E3
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x000C96EC File Offset: 0x000C78EC
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x000C96F5 File Offset: 0x000C78F5
	public override bool CanExecute()
	{
		return !string.IsNullOrEmpty(this.ItemKey);
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000C9705 File Offset: 0x000C7905
	public override bool Continue()
	{
		return base.Continue();
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000C970D File Offset: 0x000C790D
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x0600215D RID: 8541 RVA: 0x000C9715 File Offset: 0x000C7915
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x04001702 RID: 5890
	public string ItemKey;
}
