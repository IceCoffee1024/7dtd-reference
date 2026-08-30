using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002C3 RID: 707
[Preserve]
public class ConsoleCmdXui : ConsoleCmdAbstract
{
	// Token: 0x06001467 RID: 5223 RVA: 0x0007BE2D File Offset: 0x0007A02D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "xui" };
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06001468 RID: 5224 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06001469 RID: 5225 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x0007BE3D File Offset: 0x0007A03D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Execute XUi operations";
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x0007BE44 File Offset: 0x0007A044
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n   xui open <window group name> [instance] [closeOthers]\n   xui close <window group name> [instance]\n   xui reload [window group name] [instance]\n   xui list <\"instances\" / \"windows\"> [instance]\n";
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x0007BE4C File Offset: 0x0007A04C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No sub command given.");
			return;
		}
		if (_params[0].EqualsCaseInsensitive("open"))
		{
			this.ExecuteOpen(_params);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("close"))
		{
			this.ExecuteClose(_params);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("reload"))
		{
			this.ExecuteReload(_params);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("list"))
		{
			this.ExecuteList(_params);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("limits"))
		{
			this.ExecuteLimits();
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid sub command \"" + _params[0] + "\".");
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x0007BF1C File Offset: 0x0007A11C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteOpen(List<string> _params)
	{
		if (_params.Count < 2 || _params.Count > 4)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 2 to 4, found " + _params.Count.ToString() + ".");
			return;
		}
		string text = _params[1];
		bool flag = true;
		int num;
		XUi xuiInstance = this.getXuiInstance(_params, 2, out num);
		if (xuiInstance == null)
		{
			return;
		}
		if (_params.Count > 3)
		{
			flag = ConsoleHelper.ParseParamBool(_params[3], false);
		}
		text = this.getXuiWindow(xuiInstance, text);
		if (text != null)
		{
			xuiInstance.playerUI.windowManager.Open(text, flag);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"" + text + "\" opened.");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"" + _params[1] + "\" does not exist.");
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x0007BFF4 File Offset: 0x0007A1F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteClose(List<string> _params)
	{
		if (_params.Count < 2 || _params.Count > 3)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 2 to 3, found " + _params.Count.ToString() + ".");
			return;
		}
		string text = _params[1];
		int num;
		XUi xuiInstance = this.getXuiInstance(_params, 2, out num);
		if (xuiInstance == null)
		{
			return;
		}
		text = this.getXuiWindow(xuiInstance, text);
		if (text != null)
		{
			xuiInstance.playerUI.windowManager.Close(text);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"" + text + "\" closed.");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"" + _params[1] + "\" does not exist.");
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x0007C0B0 File Offset: 0x0007A2B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteReload(List<string> _params)
	{
		if (_params.Count > 3)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 1 to 3, found " + _params.Count.ToString() + ".");
			return;
		}
		string text = "*";
		if (_params.Count > 1)
		{
			text = _params[1];
		}
		int num;
		XUi xuiInstance = this.getXuiInstance(_params, 2, out num);
		if (xuiInstance == null)
		{
			return;
		}
		if (text == "*")
		{
			XUi.Reload(xuiInstance.playerUI);
			xuiInstance.SetDataConnections();
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi windows reloaded.");
			return;
		}
		if (XUi.ReloadWindow(xuiInstance.playerUI, text))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"" + text + "\" reloaded.");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"" + text + "\" not found or failed to load.");
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x0007C18C File Offset: 0x0007A38C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteList(List<string> _params)
	{
		if (_params.Count < 2)
		{
			_params.Add("windows");
		}
		if (_params[1].EqualsCaseInsensitive("instances"))
		{
			XUi[] array = this.xuiInstances();
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Loaded XUi instances:");
			for (int i = 0; i < array.Length; i++)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("  " + i.ToString() + ". " + array[i].playerUI.windowManager.gameObject.name);
			}
			return;
		}
		if (!_params[1].EqualsCaseInsensitive("windows"))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument 2 has to be either \"instances\" or \"windows\".");
			return;
		}
		int num;
		XUi xuiInstance = this.getXuiInstance(_params, 2, out num);
		if (xuiInstance == null)
		{
			return;
		}
		List<string> list = new List<string>();
		for (int j = 0; j < xuiInstance.WindowGroups.Count; j++)
		{
			list.Add(xuiInstance.WindowGroups[j].Id);
		}
		list.Sort();
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
		{
			"Loaded XUi window groups in instance ",
			num.ToString(),
			" (\"",
			xuiInstance.playerUI.gameObject.name,
			"\"):"
		}));
		for (int k = 0; k < list.Count; k++)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("  " + list[k]);
		}
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x0007C318 File Offset: 0x0007A518
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteLimits()
	{
		XUi xui = this.xuiInstances()[0];
		string xuiWindow = this.getXuiWindow(xui, "uiLimitsTest");
		if (xuiWindow == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"uiLimitsTest\" does not exist.");
			return;
		}
		if (xui.playerUI.windowManager.IsWindowOpen(xuiWindow))
		{
			xui.playerUI.windowManager.Close(xuiWindow);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"" + xuiWindow + "\" closed.");
			return;
		}
		xui.playerUI.windowManager.Open(xuiWindow, false);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi window group \"" + xuiWindow + "\" opened.");
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x0007C3BC File Offset: 0x0007A5BC
	[PublicizedFrom(EAccessModifier.Private)]
	public XUi getXuiInstance(List<string> _params, int _index, out int _xuiInstanceId)
	{
		XUi[] array = this.xuiInstances();
		_xuiInstanceId = array.Length - 1;
		if (_params.Count > _index)
		{
			if (!StringParsers.TryParseSInt32(_params[_index], out _xuiInstanceId))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[_index] + "\" is not a valid integer.");
				return null;
			}
			if (_xuiInstanceId >= array.Length)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("XUi instance " + _xuiInstanceId.ToString() + " does not exist.");
				return null;
			}
		}
		return array[_xuiInstanceId];
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x0007C440 File Offset: 0x0007A640
	[PublicizedFrom(EAccessModifier.Private)]
	public string getXuiWindow(XUi _xuiInstance, string _name)
	{
		for (int i = 0; i < _xuiInstance.WindowGroups.Count; i++)
		{
			if (_xuiInstance.WindowGroups[i].Id.EqualsCaseInsensitive(_name))
			{
				return _xuiInstance.WindowGroups[i].Id;
			}
		}
		return null;
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x0007C48F File Offset: 0x0007A68F
	[PublicizedFrom(EAccessModifier.Private)]
	public XUi[] xuiInstances()
	{
		XUi[] array = global::UnityEngine.Object.FindObjectsByType<XUi>(FindObjectsSortMode.None);
		Array.Sort<XUi>(array, (XUi _x, XUi _y) => string.Compare(_x.playerUI.windowManager.gameObject.name, _y.playerUI.windowManager.gameObject.name, StringComparison.Ordinal));
		return array;
	}
}
