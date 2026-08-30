using System;
using UnityEngine;

// Token: 0x02000616 RID: 1558
public class LocalPlayerManager
{
	// Token: 0x1400003A RID: 58
	// (add) Token: 0x060032B1 RID: 12977 RVA: 0x00149478 File Offset: 0x00147678
	// (remove) Token: 0x060032B2 RID: 12978 RVA: 0x001494AC File Offset: 0x001476AC
	public static event Action OnLocalPlayersChanged;

	// Token: 0x060032B3 RID: 12979 RVA: 0x001494DF File Offset: 0x001476DF
	public static void LocalPlayersChanged()
	{
		Action onLocalPlayersChanged = LocalPlayerManager.OnLocalPlayersChanged;
		if (onLocalPlayersChanged == null)
		{
			return;
		}
		onLocalPlayersChanged();
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x001494F0 File Offset: 0x001476F0
	public static void Init()
	{
		GameManager.Instance.OnLocalPlayerChanged += LocalPlayerManager.HandleLocalPlayerChanged;
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x00149508 File Offset: 0x00147708
	public static void Destroy()
	{
		GameManager.Instance.OnLocalPlayerChanged -= LocalPlayerManager.HandleLocalPlayerChanged;
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x00149520 File Offset: 0x00147720
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
