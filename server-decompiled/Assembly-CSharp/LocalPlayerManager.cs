using System;
using UnityEngine;

// Token: 0x02000618 RID: 1560
public class LocalPlayerManager
{
	// Token: 0x1400003B RID: 59
	// (add) Token: 0x060032F4 RID: 13044 RVA: 0x00149E1C File Offset: 0x0014801C
	// (remove) Token: 0x060032F5 RID: 13045 RVA: 0x00149E50 File Offset: 0x00148050
	public static event Action OnLocalPlayersChanged;

	// Token: 0x060032F6 RID: 13046 RVA: 0x00149E83 File Offset: 0x00148083
	public static void LocalPlayersChanged()
	{
		Action onLocalPlayersChanged = LocalPlayerManager.OnLocalPlayersChanged;
		if (onLocalPlayersChanged == null)
		{
			return;
		}
		onLocalPlayersChanged();
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x00149E94 File Offset: 0x00148094
	public static void Init()
	{
		GameManager.Instance.OnLocalPlayerChanged += LocalPlayerManager.HandleLocalPlayerChanged;
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x00149EAC File Offset: 0x001480AC
	public static void Destroy()
	{
		GameManager.Instance.OnLocalPlayerChanged -= LocalPlayerManager.HandleLocalPlayerChanged;
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x00149EC4 File Offset: 0x001480C4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void HandleLocalPlayerChanged(EntityPlayerLocal localPlayer)
	{
		if (localPlayer != null)
		{
			return;
		}
		foreach (LocalPlayerUI localPlayerUI in LocalPlayerUI.PlayerUIs)
		{
			if (!localPlayerUI.isPrimaryUI)
			{
				global::UnityEngine.Object.Destroy(localPlayerUI.gameObject);
			}
		}
	}
}
