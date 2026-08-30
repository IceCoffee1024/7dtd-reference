using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200024A RID: 586
[Preserve]
public class ConsoleCmdOcclusion : ConsoleCmdAbstract
{
	// Token: 0x0600118B RID: 4491 RVA: 0x0006E9DE File Offset: 0x0006CBDE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "occlusion" };
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x0600118C RID: 4492 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x0600118D RID: 4493 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x0006E9EE File Offset: 0x0006CBEE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Control OcclusionManager";
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x0006E9F8 File Offset: 0x0006CBF8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("occlusion off, partial, full, toggleVisible, togglePrints");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" off (set in main menu, not a map)");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   turns off gpu occlusion ");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" full (set in main menu, not a map)");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   hides occluded objects from both the camera and the sun");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" toggleVisible");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   toggles between forcing all meshes visible and normal gpu culling");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" view");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   toggles view of occlusion texture");
			return;
		}
		OcclusionManager instance = OcclusionManager.Instance;
		if (instance == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No occlusion manager!");
			return;
		}
		if (_params.Count == 1)
		{
			if (_params[0].EqualsCaseInsensitive("off"))
			{
				instance.EnableCulling(false);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Occlusion disabled");
				return;
			}
			if (_params[0].EqualsCaseInsensitive("full"))
			{
				instance.EnableCulling(true);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Occlusion enabled full");
				return;
			}
			if (_params[0].EqualsCaseInsensitive("toggleVisible"))
			{
				instance.forceAllVisible = !instance.forceAllVisible;
				if (instance.forceAllVisible)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("All meshes are forced to visible");
					return;
				}
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Normal GPU occlusion");
				return;
			}
			else if (_params[0].EqualsCaseInsensitive("view"))
			{
				instance.ToggleDebugView();
			}
		}
	}
}
