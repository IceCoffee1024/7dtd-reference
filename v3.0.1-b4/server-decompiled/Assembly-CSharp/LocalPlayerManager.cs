using System;
using UnityEngine;

// Token: 0x0200060D RID: 1549
public class LocalPlayerManager
{
	// Token: 0x1400003A RID: 58
	// (add) Token: 0x0600325E RID: 12894 RVA: 0x00147354 File Offset: 0x00145554
	// (remove) Token: 0x0600325F RID: 12895 RVA: 0x00147388 File Offset: 0x00145588
	public static event Action OnLocalPlayersChanged;

	// Token: 0x06003260 RID: 12896 RVA: 0x001473BB File Offset: 0x001455BB
	public static void LocalPlayersChanged()
	{
		Action onLocalPlayersChanged = LocalPlayerManager.OnLocalPlayersChanged;
		if (onLocalPlayersChanged == null)
		{
			return;
		}
		onLocalPlayersChanged();
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x001473CC File Offset: 0x001455CC
	public static void Init()
	{
		GameManager.Instance.OnLocalPlayerChanged += LocalPlayerManager.HandleLocalPlayerChanged;
	}

	// Token: 0x06003262 RID: 12898 RVA: 0x001473E4 File Offset: 0x001455E4
	public static void Destroy()
	{
		GameManager.Instance.OnLocalPlayerChanged -= LocalPlayerManager.HandleLocalPlayerChanged;
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x001473FC File Offset: 0x001455FC
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
