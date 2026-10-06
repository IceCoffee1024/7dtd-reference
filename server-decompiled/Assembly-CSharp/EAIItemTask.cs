using System;
using System.Collections.Generic;

// Token: 0x02000448 RID: 1096
public class EAIItemTask : EAIBase
{
	// Token: 0x06002153 RID: 8531 RVA: 0x000C9337 File Offset: 0x000C7537
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x000C9340 File Offset: 0x000C7540
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x000C9349 File Offset: 0x000C7549
	public override bool CanExecute()
	{
		return !string.IsNullOrEmpty(this.ItemKey);
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x000C9359 File Offset: 0x000C7559
	public override bool Continue()
	{
		return base.Continue();
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000C9361 File Offset: 0x000C7561
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x000C9369 File Offset: 0x000C7569
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x040016F2 RID: 5874
	public string ItemKey;
}
