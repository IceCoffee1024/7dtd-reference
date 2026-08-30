using System;
using System.Collections.Generic;

// Token: 0x02000446 RID: 1094
public class EAIItemTask : EAIBase
{
	// Token: 0x06002164 RID: 8548 RVA: 0x000C9283 File Offset: 0x000C7483
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x000C928C File Offset: 0x000C748C
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x000C9295 File Offset: 0x000C7495
	public override bool CanExecute()
	{
		return !string.IsNullOrEmpty(this.ItemKey);
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x000C92A5 File Offset: 0x000C74A5
	public override bool Continue()
	{
		return base.Continue();
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000C92AD File Offset: 0x000C74AD
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x000C92B5 File Offset: 0x000C74B5
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x040016FB RID: 5883
	public string ItemKey;
}
