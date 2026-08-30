using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002BF RID: 703
[Preserve]
public class ConsoleCmdXui : ConsoleCmdAbstract
{
	// Token: 0x06001455 RID: 5205 RVA: 0x0007B7D6 File Offset: 0x000799D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "xui" };
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06001456 RID: 5206 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06001457 RID: 5207 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x0007B7E6 File Offset: 0x000799E6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Execute XUi operations";
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x0007B7ED File Offset: 0x000799ED
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n   xui open <window group name> [instance] [closeOthers]\n   xui close <window group name> [instance]\n   xui reload [window group name] [instance]\n   xui list <\"instances\" / \"windows\"> [instance]\n";
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x0007B7F4 File Offset: 0x000799F4
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

	// Token: 0x0600145B RID: 5211 RVA: 0x0007B8C4 File Offset: 0x00079AC4
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

	// Token: 0x0600145C RID: 5212 RVA: 0x0007B99C File Offset: 0x00079B9C
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

	// Token: 0x0600145D RID: 5213 RVA: 0x0007BA58 File Offset: 0x00079C58
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

	// Token: 0x0600145E RID: 5214 RVA: 0x0007BB34 File Offset: 0x00079D34
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

	// Token: 0x0600145F RID: 5215 RVA: 0x0007BCC0 File Offset: 0x00079EC0
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

	// Token: 0x06001460 RID: 5216 RVA: 0x0007BD64 File Offset: 0x00079F64
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

	// Token: 0x06001461 RID: 5217 RVA: 0x0007BDE8 File Offset: 0x00079FE8
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

	// Token: 0x06001462 RID: 5218 RVA: 0x0007BE37 File Offset: 0x0007A037
	[PublicizedFrom(EAccessModifier.Private)]
	public XUi[] xuiInstances()
	{
		XUi[] array = global::UnityEngine.Object.FindObjectsByType<XUi>(FindObjectsSortMode.None);
		Array.Sort<XUi>(array, (XUi _x, XUi _y) => string.Compare(_x.playerUI.windowManager.gameObject.name, _y.playerUI.windowManager.gameObject.name, StringComparison.Ordinal));
		return array;
	}
}
