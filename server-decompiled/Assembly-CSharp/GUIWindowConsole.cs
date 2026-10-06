using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001235 RID: 4661
public class GUIWindowConsole : GUIWindowUGUI
{
	// Token: 0x170011DD RID: 4573
	// (get) Token: 0x06009497 RID: 38039 RVA: 0x00383E0B File Offset: 0x0038200B
	public override string UIPrefabPath
	{
		get
		{
			return "GUI/Prefabs/ConsoleWindow";
		}
	}

	// Token: 0x06009498 RID: 38040 RVA: 0x00383E14 File Offset: 0x00382014
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

	// Token: 0x06009499 RID: 38041 RVA: 0x00383FC5 File Offset: 0x003821C5
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

	// Token: 0x0600949A RID: 38042 RVA: 0x00383FFC File Offset: 0x003821FC
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

	// Token: 0x0600949B RID: 38043 RVA: 0x0038404D File Offset: 0x0038224D
	[PublicizedFrom(EAccessModifier.Private)]
	public void FreeText(Text _text)
	{
		_text.gameObject.SetActive(false);
		this.linePool.Push(_text);
	}

	// Token: 0x0600949C RID: 38044 RVA: 0x00384068 File Offset: 0x00382268
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

	// Token: 0x0600949D RID: 38045 RVA: 0x00384148 File Offset: 0x00382348
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearDisplayedLines()
	{
		Text text;
		while (this.displayedLines.TryDequeue(out text))
		{
			this.FreeText(text);
		}
	}

	// Token: 0x0600949E RID: 38046 RVA: 0x0038416D File Offset: 0x0038236D
	[PublicizedFrom(EAccessModifier.Private)]
	public void shutdown()
	{
		Log.LogCallbacks -= this.LogCallback;
	}

	// Token: 0x0600949F RID: 38047 RVA: 0x00384180 File Offset: 0x00382380
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

	// Token: 0x060094A0 RID: 38048 RVA: 0x003841BC File Offset: 0x003823BC
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

	// Token: 0x060094A1 RID: 38049 RVA: 0x003841F0 File Offset: 0x003823F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void addLines(List<string> _lines)
	{
		for (int i = 0; i < _lines.Count; i++)
		{
			this.addLine(_lines[i]);
		}
	}

	// Token: 0x060094A2 RID: 38050 RVA: 0x0038421B File Offset: 0x0038241B
	[PublicizedFrom(EAccessModifier.Private)]
	public void addLine(string _line)
	{
		this.internalAddLine(new GUIWindowConsole.ConsoleLine(_line, string.Empty, LogType.Log));
	}

	// Token: 0x060094A3 RID: 38051 RVA: 0x00384230 File Offset: 0x00382430
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

	// Token: 0x060094A4 RID: 38052 RVA: 0x00384298 File Offset: 0x00382498
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

	// Token: 0x060094A5 RID: 38053 RVA: 0x003845CC File Offset: 0x003827CC
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

	// Token: 0x060094A6 RID: 38054 RVA: 0x00384624 File Offset: 0x00382824
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTextReceived(bool _success, string _text)
	{
		if (_success)
		{
			this.commandField.text = _text;
		}
	}

	// Token: 0x060094A7 RID: 38055 RVA: 0x00384635 File Offset: 0x00382835
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator closeLater()
	{
		yield return null;
		this.CloseConsole();
		yield break;
	}

	// Token: 0x060094A8 RID: 38056 RVA: 0x00384644 File Offset: 0x00382844
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloseConsole()
	{
		this.windowManager.Close(this, false);
		this.commandField.text = string.Empty;
	}

	// Token: 0x060094A9 RID: 38057 RVA: 0x00384664 File Offset: 0x00382864
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

	// Token: 0x060094AA RID: 38058 RVA: 0x00384788 File Offset: 0x00382988
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

	// Token: 0x060094AB RID: 38059 RVA: 0x003847F8 File Offset: 0x003829F8
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

	// Token: 0x060094AC RID: 38060 RVA: 0x00384898 File Offset: 0x00382A98
	public void Clear()
	{
		this.ClearDisplayedLines();
	}

	// Token: 0x060094AD RID: 38061 RVA: 0x003848A0 File Offset: 0x00382AA0
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

	// Token: 0x060094AE RID: 38062 RVA: 0x00384924 File Offset: 0x00382B24
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

	// Token: 0x060094AF RID: 38063 RVA: 0x00384977 File Offset: 0x00382B77
	public static GUIWindowConsole GetNewInstance()
	{
		GUIWindowConsole guiwindowConsole;
		if ((guiwindowConsole = GUIWindowConsole.instance) == null)
		{
			guiwindowConsole = (GUIWindowConsole.instance = new GUIWindowConsole());
		}
		return guiwindowConsole;
	}

	// Token: 0x060094B0 RID: 38064 RVA: 0x0038498D File Offset: 0x00382B8D
	public static void Open()
	{
		LocalPlayerUI.primaryUI.windowManager.Open(GUIWindowConsole.instance, false);
	}

	// Token: 0x060094B1 RID: 38065 RVA: 0x003849A4 File Offset: 0x00382BA4
	public static void Close()
	{
		LocalPlayerUI.primaryUI.windowManager.Close(GUIWindowConsole.instance, false);
	}

	// Token: 0x060094B2 RID: 38066 RVA: 0x003849BB File Offset: 0x00382BBB
	public static bool IsOpen()
	{
		return LocalPlayerUI.primaryUI.windowManager.IsWindowOpen(GUIWindowConsole.instance);
	}

	// Token: 0x060094B3 RID: 38067 RVA: 0x003849D1 File Offset: 0x00382BD1
	public static void AddLines(List<string> _lines)
	{
		GUIWindowConsole guiwindowConsole = GUIWindowConsole.instance;
		if (guiwindowConsole == null)
		{
			return;
		}
		guiwindowConsole.addLines(_lines);
	}

	// Token: 0x060094B4 RID: 38068 RVA: 0x003849E3 File Offset: 0x00382BE3
	public static void Shutdown()
	{
		GUIWindowConsole guiwindowConsole = GUIWindowConsole.instance;
		if (guiwindowConsole != null)
		{
			guiwindowConsole.shutdown();
		}
		GUIWindowConsole.instance = null;
	}

	// Token: 0x04006F5D RID: 28509
	[PublicizedFrom(EAccessModifier.Private)]
	public static GUIWindowConsole instance;

	// Token: 0x04006F5E RID: 28510
	[PublicizedFrom(EAccessModifier.Private)]
	public bool scrolledToBottom = true;

	// Token: 0x04006F5F RID: 28511
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<GUIWindowConsole.ConsoleLine> linesToAdd = new Queue<GUIWindowConsole.ConsoleLine>(301);

	// Token: 0x04006F60 RID: 28512
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Text> displayedLines = new Queue<Text>();

	// Token: 0x04006F61 RID: 28513
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxConsoleLines = 300;

	// Token: 0x04006F62 RID: 28514
	[PublicizedFrom(EAccessModifier.Private)]
	public Stack<Text> linePool = new Stack<Text>();

	// Token: 0x04006F63 RID: 28515
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> lastCommands = new List<string>();

	// Token: 0x04006F64 RID: 28516
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastCommandsIdx;

	// Token: 0x04006F65 RID: 28517
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bFirstTime;

	// Token: 0x04006F66 RID: 28518
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bUpdateCursor;

	// Token: 0x04006F67 RID: 28519
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bShouldReopenGebugMenu;

	// Token: 0x04006F68 RID: 28520
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIWindowConsoleComponents components;

	// Token: 0x04006F69 RID: 28521
	[PublicizedFrom(EAccessModifier.Private)]
	public ScrollRect scrollRect;

	// Token: 0x04006F6A RID: 28522
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform contentRect;

	// Token: 0x04006F6B RID: 28523
	[PublicizedFrom(EAccessModifier.Private)]
	public InputField commandField;

	// Token: 0x04006F6C RID: 28524
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] lineSeparators = new string[] { "\r\n", "\r", "\n" };

	// Token: 0x02001236 RID: 4662
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ConsoleLine
	{
		// Token: 0x060094B6 RID: 38070 RVA: 0x00384A20 File Offset: 0x00382C20
		public ConsoleLine(string _text, string _stackTrace, LogType _type)
		{
			this.text = _text;
			this.stackTrace = _stackTrace;
			this.type = _type;
		}

		// Token: 0x060094B7 RID: 38071 RVA: 0x00384A38 File Offset: 0x00382C38
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

		// Token: 0x04006F6D RID: 28525
		public string text;

		// Token: 0x04006F6E RID: 28526
		public LogType type;

		// Token: 0x04006F6F RID: 28527
		public string stackTrace;
	}
}
