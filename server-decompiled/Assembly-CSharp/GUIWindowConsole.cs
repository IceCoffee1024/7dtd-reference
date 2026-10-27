using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001247 RID: 4679
public class GUIWindowConsole : GUIWindowUGUI
{
	// Token: 0x170011E8 RID: 4584
	// (get) Token: 0x060094EE RID: 38126 RVA: 0x00386207 File Offset: 0x00384407
	public override string UIPrefabPath
	{
		get
		{
			return "GUI/Prefabs/ConsoleWindow";
		}
	}

	// Token: 0x060094EF RID: 38127 RVA: 0x00386210 File Offset: 0x00384410
	public GUIWindowConsole()
		: base("GUIWindowConsole")
	{
		Log.LogCallbacks += this.LogCallback;
		this.alwaysUsesMouseCursor = true;
		this.components = this.canvas.GetComponent<GUIWindowConsoleComponents>();
		this.scrollRect = this.components.scrollRect;
		this.contentRect = this.components.contentRect;
		this.commandField = this.components.commandField;
		this.commandField.onSubmit.AddListener(new UnityAction<string>(this.EnterCommand));
		this.commandField.shouldActivateOnSelect = !TouchScreenKeyboard.isSupported;
		this.components.closeButton.onClick.AddListener(new UnityAction(this.CloseConsole));
		this.components.openLogsButton.onClick.AddListener(delegate
		{
			GameIO.OpenExplorer(Application.consoleLogPath);
		});
		if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
		{
			this.components.openLogsButton.gameObject.SetActive(false);
		}
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.components.consoleLinePrefab);
			gameObject.SetActive(false);
			gameObject.transform.SetParent(this.contentRect, false);
			this.linePool.Push(gameObject.GetComponent<Text>());
		}
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.Input_OnLastInputStyleChanged;
	}

	// Token: 0x060094F0 RID: 38128 RVA: 0x003863C1 File Offset: 0x003845C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void Input_OnLastInputStyleChanged(PlayerInputManager.InputStyle _inputStyle)
	{
		if (_inputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			this.components.controllerPrompts.SetActive(false);
			return;
		}
		this.components.controllerPrompts.SetActive(true);
		this.components.RefreshButtonPrompts();
	}

	// Token: 0x060094F1 RID: 38129 RVA: 0x003863F8 File Offset: 0x003845F8
	[PublicizedFrom(EAccessModifier.Private)]
	public Text AllocText()
	{
		Text text;
		if (this.linePool.TryPop(out text))
		{
			text.gameObject.SetActive(true);
			return text;
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.components.consoleLinePrefab);
		gameObject.transform.SetParent(this.contentRect, false);
		return gameObject.GetComponent<Text>();
	}

	// Token: 0x060094F2 RID: 38130 RVA: 0x00386449 File Offset: 0x00384649
	[PublicizedFrom(EAccessModifier.Private)]
	public void FreeText(Text _text)
	{
		_text.gameObject.SetActive(false);
		this.linePool.Push(_text);
	}

	// Token: 0x060094F3 RID: 38131 RVA: 0x00386464 File Offset: 0x00384664
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddDisplayedLine(GUIWindowConsole.ConsoleLine _line)
	{
		foreach (StringSpan stringSpan in _line.text.GetSplitAnyEnumerator(GUIWindowConsole.lineSeparators, StringSplitOptions.RemoveEmptyEntries))
		{
			Text text;
			if (this.displayedLines.Count == 300)
			{
				text = this.displayedLines.Dequeue();
			}
			else
			{
				text = this.AllocText();
			}
			if (stringSpan.Length > 500)
			{
				text.text = SpanUtils.Concat(stringSpan.Slice(0, 500), "...");
			}
			else
			{
				text.text = stringSpan.ToString();
			}
			text.color = _line.GetLogColor();
			text.transform.SetAsLastSibling();
			this.displayedLines.Enqueue(text);
		}
	}

	// Token: 0x060094F4 RID: 38132 RVA: 0x00386544 File Offset: 0x00384744
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearDisplayedLines()
	{
		Text text;
		while (this.displayedLines.TryDequeue(out text))
		{
			this.FreeText(text);
		}
	}

	// Token: 0x060094F5 RID: 38133 RVA: 0x00386569 File Offset: 0x00384769
	[PublicizedFrom(EAccessModifier.Private)]
	public void shutdown()
	{
		Log.LogCallbacks -= this.LogCallback;
	}

	// Token: 0x060094F6 RID: 38134 RVA: 0x0038657C File Offset: 0x0038477C
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogCallback(string _msg, string _trace, LogType _type)
	{
		switch (_type)
		{
		case LogType.Assert:
			this.openConsole(_msg);
			break;
		case LogType.Exception:
			this.openConsole(_msg);
			break;
		}
		this.internalAddLine(new GUIWindowConsole.ConsoleLine(_msg, _trace, _type));
	}

	// Token: 0x060094F7 RID: 38135 RVA: 0x003865B8 File Offset: 0x003847B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void openConsole(string _logString)
	{
		if (Submission.Enabled)
		{
			return;
		}
		if (_logString.StartsWith("Can't send RPC"))
		{
			return;
		}
		if (_logString.StartsWith("You are trying to load data from"))
		{
			return;
		}
		this.windowManager.Open(this, false);
	}

	// Token: 0x060094F8 RID: 38136 RVA: 0x003865EC File Offset: 0x003847EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void addLines(List<string> _lines)
	{
		for (int i = 0; i < _lines.Count; i++)
		{
			this.addLine(_lines[i]);
		}
	}

	// Token: 0x060094F9 RID: 38137 RVA: 0x00386617 File Offset: 0x00384817
	[PublicizedFrom(EAccessModifier.Private)]
	public void addLine(string _line)
	{
		this.internalAddLine(new GUIWindowConsole.ConsoleLine(_line, string.Empty, LogType.Log));
	}

	// Token: 0x060094FA RID: 38138 RVA: 0x0038662C File Offset: 0x0038482C
	[PublicizedFrom(EAccessModifier.Private)]
	public void internalAddLine(GUIWindowConsole.ConsoleLine consoleLine)
	{
		Queue<GUIWindowConsole.ConsoleLine> queue = this.linesToAdd;
		lock (queue)
		{
			this.linesToAdd.Enqueue(consoleLine);
			while (this.linesToAdd.Count > 300)
			{
				this.linesToAdd.Dequeue();
			}
		}
	}

	// Token: 0x060094FB RID: 38139 RVA: 0x00386694 File Offset: 0x00384894
	public override void Update()
	{
		base.Update();
		this.scrolledToBottom = this.scrollRect.verticalNormalizedPosition < 0.1f;
		bool flag = false;
		Queue<GUIWindowConsole.ConsoleLine> queue = this.linesToAdd;
		lock (queue)
		{
			if (this.linesToAdd.Count > 0)
			{
				flag = true;
				foreach (GUIWindowConsole.ConsoleLine consoleLine in this.linesToAdd)
				{
					this.AddDisplayedLine(consoleLine);
				}
				this.linesToAdd.Clear();
			}
		}
		if (flag && this.scrolledToBottom)
		{
			Canvas.ForceUpdateCanvases();
			this.scrollRect.verticalNormalizedPosition = 0f;
		}
		if (this.bFirstTime)
		{
			if (!TouchScreenKeyboard.isSupported)
			{
				this.commandField.Select();
				this.commandField.ActivateInputField();
			}
			this.scrollRect.verticalNormalizedPosition = 0f;
			this.bFirstTime = false;
		}
		if (this.bUpdateCursor)
		{
			this.commandField.MoveTextEnd(false);
			this.bUpdateCursor = false;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.PreviousCommand();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.NextCommand();
		}
		else if (Input.GetKeyDown(KeyCode.PageUp))
		{
			float num = this.CalculateNormalizedPageSize();
			this.scrollRect.verticalNormalizedPosition = Math.Min(this.scrollRect.verticalNormalizedPosition + num, 1f);
		}
		else if (Input.GetKeyDown(KeyCode.PageDown))
		{
			float num2 = this.CalculateNormalizedPageSize();
			this.scrollRect.verticalNormalizedPosition = Math.Max(this.scrollRect.verticalNormalizedPosition - num2, 0f);
		}
		PlayerActionsLocal playerInput = this.playerUI.playerInput;
		PlayerActionsGUI playerActionsGUI = ((playerInput != null) ? playerInput.GUIActions : null);
		if (playerActionsGUI == null)
		{
			return;
		}
		if (playerActionsGUI.Submit.WasReleased)
		{
			this.EnterCommand(this.commandField.text);
		}
		else if (playerActionsGUI.DPad_Up.WasReleased && playerActionsGUI.DPad_Up.LastDeviceClass != InputDeviceClass.Keyboard)
		{
			this.PreviousCommand();
		}
		else if (playerActionsGUI.DPad_Down.WasReleased && playerActionsGUI.DPad_Down.LastDeviceClass != InputDeviceClass.Keyboard)
		{
			this.NextCommand();
		}
		else if (playerActionsGUI.DPad_Left.WasReleased && playerActionsGUI.DPad_Down.LastDeviceClass != InputDeviceClass.Keyboard)
		{
			IVirtualKeyboard virtualKeyboard = PlatformManager.NativePlatform.VirtualKeyboard;
			if (virtualKeyboard != null)
			{
				virtualKeyboard.Open("Enter Command", this.commandField.text, new Action<bool, string>(this.OnTextReceived), UIInput.InputType.Standard, false, 200U);
			}
		}
		else if (this.playerUI.playerInput.PermanentActions.Cancel.WasReleased || PlayerActionsGlobal.Instance.Console.WasPressed)
		{
			ThreadManager.StartCoroutine(this.closeLater());
		}
		float y = playerActionsGUI.Camera.Vector.y;
		if (y != 0f)
		{
			float num3 = this.CalculateNormalizedPageSize();
			this.scrollRect.verticalNormalizedPosition = Math.Max(this.scrollRect.verticalNormalizedPosition + num3 * y * 0.05f, 0f);
		}
	}

	// Token: 0x060094FC RID: 38140 RVA: 0x003869C8 File Offset: 0x00384BC8
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalculateNormalizedPageSize()
	{
		float height = this.scrollRect.viewport.rect.height;
		float num = this.scrollRect.content.rect.height - height;
		if (num > height)
		{
			return Math.Max(height / num, 0.01f);
		}
		return 1f;
	}

	// Token: 0x060094FD RID: 38141 RVA: 0x00386A20 File Offset: 0x00384C20
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTextReceived(bool _success, string _text)
	{
		if (_success)
		{
			this.commandField.text = _text;
		}
	}

	// Token: 0x060094FE RID: 38142 RVA: 0x00386A31 File Offset: 0x00384C31
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeLater()
	{
		yield return null;
		this.CloseConsole();
		yield break;
	}

	// Token: 0x060094FF RID: 38143 RVA: 0x00386A40 File Offset: 0x00384C40
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloseConsole()
	{
		this.windowManager.Close(this, false);
		this.commandField.text = string.Empty;
	}

	// Token: 0x06009500 RID: 38144 RVA: 0x00386A60 File Offset: 0x00384C60
	[PublicizedFrom(EAccessModifier.Private)]
	public void EnterCommand(string _command)
	{
		if (_command.Length > 0)
		{
			if (_command == "clear")
			{
				this.Clear();
			}
			else
			{
				this.scrollRect.verticalNormalizedPosition = 0f;
				this.internalAddLine(new GUIWindowConsole.ConsoleLine("> " + _command, string.Empty, LogType.Log));
				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
				{
					GUIWindowConsole.AddLines(SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(_command, null));
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(_command), false);
				}
			}
			if (this.lastCommands.Count == 0 || !this.lastCommands[this.lastCommands.Count - 1].Equals(_command))
			{
				if (this.lastCommands.Contains(_command))
				{
					this.lastCommands.Remove(_command);
				}
				this.lastCommands.Add(_command);
			}
			this.lastCommandsIdx = this.lastCommands.Count;
			this.commandField.text = "";
			if (!TouchScreenKeyboard.isSupported)
			{
				this.commandField.Select();
				this.commandField.ActivateInputField();
			}
		}
	}

	// Token: 0x06009501 RID: 38145 RVA: 0x00386B84 File Offset: 0x00384D84
	[PublicizedFrom(EAccessModifier.Private)]
	public void PreviousCommand()
	{
		if (this.lastCommands.Count > 0)
		{
			this.lastCommandsIdx = Mathf.Max(0, this.lastCommandsIdx - 1);
			this.commandField.text = this.lastCommands[this.lastCommandsIdx];
			if (!TouchScreenKeyboard.isSupported)
			{
				this.commandField.Select();
				this.commandField.ActivateInputField();
			}
			this.bUpdateCursor = true;
		}
	}

	// Token: 0x06009502 RID: 38146 RVA: 0x00386BF4 File Offset: 0x00384DF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void NextCommand()
	{
		if (this.lastCommands.Count > 0)
		{
			this.lastCommandsIdx = Mathf.Min(this.lastCommands.Count, this.lastCommandsIdx + 1);
			if (this.lastCommandsIdx < this.lastCommands.Count)
			{
				this.commandField.text = this.lastCommands[this.lastCommandsIdx];
				this.bUpdateCursor = true;
				if (!TouchScreenKeyboard.isSupported)
				{
					this.commandField.Select();
					this.commandField.ActivateInputField();
					return;
				}
			}
			else
			{
				this.commandField.text = string.Empty;
			}
		}
	}

	// Token: 0x06009503 RID: 38147 RVA: 0x00386C94 File Offset: 0x00384E94
	public void Clear()
	{
		this.ClearDisplayedLines();
	}

	// Token: 0x06009504 RID: 38148 RVA: 0x00386C9C File Offset: 0x00384E9C
	public override void OnOpen()
	{
		base.OnOpen();
		if (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) && this.windowManager.IsWindowOpen(XUiC_InGameDebugMenu.ID))
		{
			this.bShouldReopenGebugMenu = true;
			this.windowManager.Close(XUiC_InGameDebugMenu.ID);
		}
		else
		{
			this.bShouldReopenGebugMenu = false;
		}
		this.commandField.text = string.Empty;
		this.bFirstTime = true;
		this.isInputActive = true;
		if (UIInput.selection != null)
		{
			UIInput.selection.isSelected = false;
		}
	}

	// Token: 0x06009505 RID: 38149 RVA: 0x00386D20 File Offset: 0x00384F20
	public override void OnClose()
	{
		this.scrollRect.verticalNormalizedPosition = 0f;
		base.OnClose();
		if (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) && this.bShouldReopenGebugMenu)
		{
			this.windowManager.Open(XUiC_InGameDebugMenu.ID, false);
		}
		this.bShouldReopenGebugMenu = false;
		this.isInputActive = false;
	}

	// Token: 0x06009506 RID: 38150 RVA: 0x00386D73 File Offset: 0x00384F73
	public static GUIWindowConsole GetNewInstance()
	{
		GUIWindowConsole guiwindowConsole;
		if ((guiwindowConsole = GUIWindowConsole.instance) == null)
		{
			guiwindowConsole = (GUIWindowConsole.instance = new GUIWindowConsole());
		}
		return guiwindowConsole;
	}

	// Token: 0x06009507 RID: 38151 RVA: 0x00386D89 File Offset: 0x00384F89
	public static void Open()
	{
		LocalPlayerUI.primaryUI.windowManager.Open(GUIWindowConsole.instance, false);
	}

	// Token: 0x06009508 RID: 38152 RVA: 0x00386DA0 File Offset: 0x00384FA0
	public static void Close()
	{
		LocalPlayerUI.primaryUI.windowManager.Close(GUIWindowConsole.instance, false);
	}

	// Token: 0x06009509 RID: 38153 RVA: 0x00386DB7 File Offset: 0x00384FB7
	public static bool IsOpen()
	{
		return LocalPlayerUI.primaryUI.windowManager.IsWindowOpen(GUIWindowConsole.instance);
	}

	// Token: 0x0600950A RID: 38154 RVA: 0x00386DCD File Offset: 0x00384FCD
	public static void AddLines(List<string> _lines)
	{
		GUIWindowConsole guiwindowConsole = GUIWindowConsole.instance;
		if (guiwindowConsole == null)
		{
			return;
		}
		guiwindowConsole.addLines(_lines);
	}

	// Token: 0x0600950B RID: 38155 RVA: 0x00386DDF File Offset: 0x00384FDF
	public static void Shutdown()
	{
		GUIWindowConsole guiwindowConsole = GUIWindowConsole.instance;
		if (guiwindowConsole != null)
		{
			guiwindowConsole.shutdown();
		}
		GUIWindowConsole.instance = null;
	}

	// Token: 0x04006F9E RID: 28574
	[PublicizedFrom(EAccessModifier.Private)]
	public static GUIWindowConsole instance;

	// Token: 0x04006F9F RID: 28575
	[PublicizedFrom(EAccessModifier.Private)]
	public bool scrolledToBottom = true;

	// Token: 0x04006FA0 RID: 28576
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<GUIWindowConsole.ConsoleLine> linesToAdd = new Queue<GUIWindowConsole.ConsoleLine>(301);

	// Token: 0x04006FA1 RID: 28577
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Text> displayedLines = new Queue<Text>();

	// Token: 0x04006FA2 RID: 28578
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxConsoleLines = 300;

	// Token: 0x04006FA3 RID: 28579
	[PublicizedFrom(EAccessModifier.Private)]
	public Stack<Text> linePool = new Stack<Text>();

	// Token: 0x04006FA4 RID: 28580
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> lastCommands = new List<string>();

	// Token: 0x04006FA5 RID: 28581
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastCommandsIdx;

	// Token: 0x04006FA6 RID: 28582
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bFirstTime;

	// Token: 0x04006FA7 RID: 28583
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bUpdateCursor;

	// Token: 0x04006FA8 RID: 28584
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bShouldReopenGebugMenu;

	// Token: 0x04006FA9 RID: 28585
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIWindowConsoleComponents components;

	// Token: 0x04006FAA RID: 28586
	[PublicizedFrom(EAccessModifier.Private)]
	public ScrollRect scrollRect;

	// Token: 0x04006FAB RID: 28587
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform contentRect;

	// Token: 0x04006FAC RID: 28588
	[PublicizedFrom(EAccessModifier.Private)]
	public InputField commandField;

	// Token: 0x04006FAD RID: 28589
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] lineSeparators = new string[] { "\r\n", "\r", "\n" };

	// Token: 0x02001248 RID: 4680
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ConsoleLine
	{
		// Token: 0x0600950D RID: 38157 RVA: 0x00386E1C File Offset: 0x0038501C
		public ConsoleLine(string _text, string _stackTrace, LogType _type)
		{
			this.text = _text;
			this.stackTrace = _stackTrace;
			this.type = _type;
		}

		// Token: 0x0600950E RID: 38158 RVA: 0x00386E34 File Offset: 0x00385034
		public Color GetLogColor()
		{
			switch (this.type)
			{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				return Color.red;
			case LogType.Warning:
				return Color.yellow;
			case LogType.Log:
				return Color.white;
			default:
				return Color.white;
			}
		}

		// Token: 0x04006FAE RID: 28590
		public string text;

		// Token: 0x04006FAF RID: 28591
		public LogType type;

		// Token: 0x04006FB0 RID: 28592
		public string stackTrace;
	}
}
