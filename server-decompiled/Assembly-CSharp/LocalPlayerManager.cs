using System;
using UnityEngine;

// Token: 0x0200061F RID: 1567
public class LocalPlayerManager
{
	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06003302 RID: 13058 RVA: 0x0014AA10 File Offset: 0x00148C10
	// (remove) Token: 0x06003303 RID: 13059 RVA: 0x0014AA44 File Offset: 0x00148C44
	public static event Action OnLocalPlayersChanged;

	// Token: 0x06003304 RID: 13060 RVA: 0x0014AA77 File Offset: 0x00148C77
	public static void LocalPlayersChanged()
	{
		Action onLocalPlayersChanged = LocalPlayerManager.OnLocalPlayersChanged;
		if (onLocalPlayersChanged == null)
		{
			return;
		}
		onLocalPlayersChanged();
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x0014AA88 File Offset: 0x00148C88
	public static void Init()
	{
		GameManager.Instance.OnLocalPlayerChanged += LocalPlayerManager.HandleLocalPlayerChanged;
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x0014AAA0 File Offset: 0x00148CA0
	public static void Destroy()
	{
		GameManager.Instance.OnLocalPlayerChanged -= LocalPlayerManager.HandleLocalPlayerChanged;
	}

	// Token: 0x06003307 RID: 13063 RVA: 0x0014AAB8 File Offset: 0x00148CB8
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
