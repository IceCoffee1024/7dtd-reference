using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000150 RID: 336
[Preserve]
public class BlockSpeakerTrader : Block
{
	// Token: 0x06000946 RID: 2374 RVA: 0x00040A88 File Offset: 0x0003EC88
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("OpenSound"))
		{
			this.openSound = base.Properties.Values["OpenSound"];
		}
		if (base.Properties.Values.ContainsKey("CloseSound"))
		{
			this.closeSound = base.Properties.Values["CloseSound"];
		}
		if (base.Properties.Values.ContainsKey("WarningSound"))
		{
			this.warningSound = base.Properties.Values["WarningSound"];
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00040B34 File Offset: 0x0003ED34
	public void PlayOpen(Vector3i _blockPos, EntityTrader _trader)
	{
		string text = this.openSound;
		if (string.IsNullOrEmpty(text))
		{
			text = ((_trader != null) ? (_trader.NPCInfo.VoiceSet + "_announce_open") : "");
		}
		if (text != "")
		{
			Manager.BroadcastPlay(_blockPos.ToVector3(), text, 0f);
		}
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00040B98 File Offset: 0x0003ED98
	public void PlayClose(Vector3i _blockPos, EntityTrader _trader)
	{
		string text = this.closeSound;
		if (string.IsNullOrEmpty(text))
		{
			text = ((_trader != null) ? (_trader.NPCInfo.VoiceSet + "_announce_closed") : "");
		}
		if (text != "")
		{
			Manager.BroadcastPlay(_blockPos.ToVector3(), text, 0f);
		}
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x00040BFC File Offset: 0x0003EDFC
	public void PlayWarning(Vector3i _blockPos, EntityTrader _trader)
	{
		string text = this.warningSound;
		if (string.IsNullOrEmpty(text))
		{
			text = ((_trader != null) ? (_trader.NPCInfo.VoiceSet + "_announce_closing") : "");
		}
		if (text != "")
		{
			Manager.BroadcastPlay(_blockPos.ToVector3(), text, 0f);
		}
	}

	// Token: 0x040009B7 RID: 2487
	[PublicizedFrom(EAccessModifier.Private)]
	public string openSound;

	// Token: 0x040009B8 RID: 2488
	[PublicizedFrom(EAccessModifier.Private)]
	public string closeSound;

	// Token: 0x040009B9 RID: 2489
	[PublicizedFrom(EAccessModifier.Private)]
	public string warningSound;
}
