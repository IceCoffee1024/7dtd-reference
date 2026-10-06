using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000153 RID: 339
[Preserve]
public class BlockSpeakerTrader : Block
{
	// Token: 0x06000956 RID: 2390 RVA: 0x00040DC8 File Offset: 0x0003EFC8
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

	// Token: 0x06000957 RID: 2391 RVA: 0x00040E74 File Offset: 0x0003F074
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

	// Token: 0x06000958 RID: 2392 RVA: 0x00040ED8 File Offset: 0x0003F0D8
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

	// Token: 0x06000959 RID: 2393 RVA: 0x00040F3C File Offset: 0x0003F13C
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

	// Token: 0x040009BC RID: 2492
	[PublicizedFrom(EAccessModifier.Private)]
	public string openSound;

	// Token: 0x040009BD RID: 2493
	[PublicizedFrom(EAccessModifier.Private)]
	public string closeSound;

	// Token: 0x040009BE RID: 2494
	[PublicizedFrom(EAccessModifier.Private)]
	public string warningSound;
}
