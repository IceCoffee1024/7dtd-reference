using System;
using UnityEngine.Scripting;

// Token: 0x020002E7 RID: 743
[Preserve]
public class DialogActionAddBuff : BaseDialogAction
{
	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06001553 RID: 5459 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddBuff;
		}
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x00080600 File Offset: 0x0007E800
	public override void PerformAction(EntityPlayer player)
	{
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			EntityBuffs.BuffStatus buffStatus = primaryPlayer.Buffs.AddBuff(base.ID, -1, true, false, -1f);
			if (buffStatus != EntityBuffs.BuffStatus.Added)
			{
				switch (buffStatus)
				{
				case EntityBuffs.BuffStatus.FailedInvalidName:
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: buff \"" + base.ID + "\" unknown");
					return;
				case EntityBuffs.BuffStatus.FailedImmune:
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: entity is immune to \"" + base.ID);
					return;
				case EntityBuffs.BuffStatus.FailedFriendlyFire:
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: entity is friendly");
					return;
				case EntityBuffs.BuffStatus.FailedEditor:
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: buff " + base.ID + " not allowed in editor.");
					return;
				case EntityBuffs.BuffStatus.FailedGameStat:
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: missing required game stat.");
					break;
				default:
					return;
				}
			}
		}
	}

	// Token: 0x04000E4F RID: 3663
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
