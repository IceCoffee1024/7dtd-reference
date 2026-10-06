using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012C0 RID: 4800
public class NGUIWindowManager : MonoBehaviour
{
	// Token: 0x1700123F RID: 4671
	// (get) Token: 0x0600980D RID: 38925 RVA: 0x00395B74 File Offset: 0x00393D74
	public GUIWindowManager WindowManager
	{
		get
		{
			if (!(this.playerUI != null))
			{
				return null;
			}
			return this.playerUI.windowManager;
		}
	}

	// Token: 0x17001240 RID: 4672
	// (get) Token: 0x0600980E RID: 38926 RVA: 0x00395B91 File Offset: 0x00393D91
	// (set) Token: 0x0600980F RID: 38927 RVA: 0x00395B99 File Offset: 0x00393D99
	public NGuiWdwInGameHUD InGameHUD
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009810 RID: 38928 RVA: 0x00395BA2 File Offset: 0x00393DA2
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.playerUI = base.GetComponent<LocalPlayerUI>();
		this.bGlobalShowFlag = true;
		this.ParseWindows();
	}

	// Token: 0x06009811 RID: 38929 RVA: 0x00395BC0 File Offset: 0x00393DC0
	public void ParseWindows()
	{
		if (this.parsedWindows)
		{
			return;
		}
		this.parsedWindows = true;
		foreach (Transform transform in this.Windows)
		{
			if (!(transform == null))
			{
				EnumNGUIWindow enumNGUIWindow = EnumUtils.Parse<EnumNGUIWindow>(transform.name.Substring(3), false);
				this.windowMap[enumNGUIWindow] = transform;
			}
		}
		Transform window = this.GetWindow(EnumNGUIWindow.InGameHUD);
		if (window != null)
		{
			this.InGameHUD = window.GetComponent<NGuiWdwInGameHUD>();
			return;
		}
		Log.Error("Window wdwInGameHUD not found!");
	}

	// Token: 0x06009812 RID: 38930 RVA: 0x00395C4C File Offset: 0x00393E4C
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform GetWindow(EnumNGUIWindow _wdw)
	{
		Transform transform;
		if (!this.windowMap.TryGetValue(_wdw, out transform))
		{
			Log.Error("NGUIWindowManager.GetWindow: Window " + _wdw.ToStringCached<EnumNGUIWindow>() + " not found!");
		}
		return transform;
	}

	// Token: 0x06009813 RID: 38931 RVA: 0x00395C84 File Offset: 0x00393E84
	public void ShowAll(bool _bShow)
	{
		this.bGlobalShowFlag = _bShow;
		if (!_bShow)
		{
			this.windowsVisibleBeforeHide.Clear();
			using (Dictionary<EnumNGUIWindow, Transform>.Enumerator enumerator = this.windowMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<EnumNGUIWindow, Transform> keyValuePair = enumerator.Current;
					EnumNGUIWindow enumNGUIWindow;
					Transform transform;
					keyValuePair.Deconstruct(out enumNGUIWindow, out transform);
					EnumNGUIWindow enumNGUIWindow2 = enumNGUIWindow;
					if (this.IsShowing(enumNGUIWindow2))
					{
						this.windowsVisibleBeforeHide.Add(enumNGUIWindow2);
						this.Show(enumNGUIWindow2, false);
					}
				}
				return;
			}
		}
		foreach (EnumNGUIWindow enumNGUIWindow3 in this.windowsVisibleBeforeHide)
		{
			this.Show(enumNGUIWindow3, true);
		}
	}

	// Token: 0x06009814 RID: 38932 RVA: 0x00395D58 File Offset: 0x00393F58
	public bool IsShowing(EnumNGUIWindow _eWindow)
	{
		Transform window = this.GetWindow(_eWindow);
		return window != null && window.gameObject.activeSelf;
	}

	// Token: 0x06009815 RID: 38933 RVA: 0x00395D84 File Offset: 0x00393F84
	public void Show(EnumNGUIWindow _eWindow, bool _bEnable)
	{
		Transform window = this.GetWindow(_eWindow);
		if (window == null)
		{
			return;
		}
		window.gameObject.SetActive(_bEnable && this.bGlobalShowFlag);
		if (_eWindow == EnumNGUIWindow.Loading && this.ellipsisAnimator != null)
		{
			this.ellipsisAnimator = null;
		}
	}

	// Token: 0x06009816 RID: 38934 RVA: 0x00395DD0 File Offset: 0x00393FD0
	public void SetLabelText(EnumNGUIWindow _eElement, string _text, bool _toUpper = true)
	{
		Transform window = this.GetWindow(_eElement);
		if (window == null)
		{
			return;
		}
		this.Show(_eElement, !string.IsNullOrEmpty(_text));
		if (string.IsNullOrEmpty(_text))
		{
			return;
		}
		UILabel component = window.GetComponent<UILabel>();
		if (!component)
		{
			return;
		}
		_text = ((!_toUpper) ? _text : _text.ToUpper());
		component.text = _text;
		if (_eElement == EnumNGUIWindow.Loading)
		{
			this.ellipsisAnimator = new TextEllipsisAnimator(_text, component);
		}
	}

	// Token: 0x06009817 RID: 38935 RVA: 0x00395E40 File Offset: 0x00394040
	public void SetLabel(EnumNGUIWindow _eElement, string _text, Color? _color = null, bool _toUpper = true)
	{
		Transform window = this.GetWindow(_eElement);
		if (window == null)
		{
			return;
		}
		this.Show(_eElement, !string.IsNullOrEmpty(_text));
		UILabel component = window.GetComponent<UILabel>();
		if (!component)
		{
			return;
		}
		_text = ((!_toUpper) ? _text : ((_text != null) ? _text.ToUpper() : null));
		component.text = _text ?? "";
		if (_color != null)
		{
			component.color = _color.Value;
		}
		if (_eElement == EnumNGUIWindow.Loading)
		{
			this.ellipsisAnimator = new TextEllipsisAnimator(_text + "...", component);
		}
	}

	// Token: 0x06009818 RID: 38936 RVA: 0x00395ED8 File Offset: 0x003940D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.playerUI == null)
		{
			this.playerUI = base.GetComponent<LocalPlayerUI>();
		}
		if (!this.playerUI.isPrimaryUI)
		{
			return;
		}
		this.updateVersionLabelDisplayState();
		TextEllipsisAnimator textEllipsisAnimator = this.ellipsisAnimator;
		if (textEllipsisAnimator == null)
		{
			return;
		}
		textEllipsisAnimator.GetNextAnimatedString(Time.unscaledDeltaTime);
	}

	// Token: 0x06009819 RID: 38937 RVA: 0x00395F28 File Offset: 0x00394128
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateVersionLabelDisplayState()
	{
		GUIWindowManager windowManager = this.WindowManager;
		bool flag = this.AlwaysShowVersionUi && LocalPlayerUI.GetUIForPrimaryPlayer() != null;
		if (!flag)
		{
			LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
			if (uiforPrimaryPlayer != null)
			{
				flag |= uiforPrimaryPlayer.windowManager.IsWindowOpen("ingameMenu");
			}
		}
		this.ShowVersionUi(flag);
	}

	// Token: 0x17001241 RID: 4673
	// (get) Token: 0x0600981A RID: 38938 RVA: 0x00395F7F File Offset: 0x0039417F
	// (set) Token: 0x0600981B RID: 38939 RVA: 0x00395F87 File Offset: 0x00394187
	public bool AlwaysShowVersionUi { get; set; } = true;

	// Token: 0x17001242 RID: 4674
	// (get) Token: 0x0600981C RID: 38940 RVA: 0x00395F90 File Offset: 0x00394190
	public bool VersionUiVisible
	{
		get
		{
			return this.GetWindow(EnumNGUIWindow.Version).gameObject.activeSelf;
		}
	}

	// Token: 0x0600981D RID: 38941 RVA: 0x00395FA3 File Offset: 0x003941A3
	public void ToggleVersionUi()
	{
		this.ShowVersionUi(!this.VersionUiVisible);
	}

	// Token: 0x0600981E RID: 38942 RVA: 0x00395FB4 File Offset: 0x003941B4
	public void ShowVersionUi(bool _show)
	{
		this.GetWindow(EnumNGUIWindow.Version).gameObject.SetActive(_show);
	}

	// Token: 0x0600981F RID: 38943 RVA: 0x00395FC8 File Offset: 0x003941C8
	public void SetBackgroundScale(float _uiScale)
	{
		this.GetWindow(EnumNGUIWindow.MainMenuBackground).localScale = new Vector3(_uiScale, _uiScale, _uiScale);
	}

	// Token: 0x040071F6 RID: 29174
	public Transform[] Windows;

	// Token: 0x040071F7 RID: 29175
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<EnumNGUIWindow, Transform> windowMap = new EnumDictionary<EnumNGUIWindow, Transform>();

	// Token: 0x040071F8 RID: 29176
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly HashSet<EnumNGUIWindow> windowsVisibleBeforeHide = new HashSet<EnumNGUIWindow>();

	// Token: 0x040071F9 RID: 29177
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bGlobalShowFlag;

	// Token: 0x040071FA RID: 29178
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;

	// Token: 0x040071FB RID: 29179
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool parsedWindows;

	// Token: 0x040071FD RID: 29181
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextEllipsisAnimator ellipsisAnimator;
}
