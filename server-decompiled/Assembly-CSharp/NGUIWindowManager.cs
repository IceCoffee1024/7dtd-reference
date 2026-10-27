using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012D2 RID: 4818
public class NGUIWindowManager : MonoBehaviour
{
	// Token: 0x1700124B RID: 4683
	// (get) Token: 0x06009865 RID: 39013 RVA: 0x00397F7C File Offset: 0x0039617C
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

	// Token: 0x1700124C RID: 4684
	// (get) Token: 0x06009866 RID: 39014 RVA: 0x00397F99 File Offset: 0x00396199
	// (set) Token: 0x06009867 RID: 39015 RVA: 0x00397FA1 File Offset: 0x003961A1
	public NGuiWdwInGameHUD InGameHUD
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009868 RID: 39016 RVA: 0x00397FAA File Offset: 0x003961AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.playerUI = base.GetComponent<LocalPlayerUI>();
		this.bGlobalShowFlag = true;
		this.ParseWindows();
	}

	// Token: 0x06009869 RID: 39017 RVA: 0x00397FC8 File Offset: 0x003961C8
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

	// Token: 0x0600986A RID: 39018 RVA: 0x00398054 File Offset: 0x00396254
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

	// Token: 0x0600986B RID: 39019 RVA: 0x0039808C File Offset: 0x0039628C
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

	// Token: 0x0600986C RID: 39020 RVA: 0x00398160 File Offset: 0x00396360
	public bool IsShowing(EnumNGUIWindow _eWindow)
	{
		Transform window = this.GetWindow(_eWindow);
		return window != null && window.gameObject.activeSelf;
	}

	// Token: 0x0600986D RID: 39021 RVA: 0x0039818C File Offset: 0x0039638C
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

	// Token: 0x0600986E RID: 39022 RVA: 0x003981D8 File Offset: 0x003963D8
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

	// Token: 0x0600986F RID: 39023 RVA: 0x00398248 File Offset: 0x00396448
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

	// Token: 0x06009870 RID: 39024 RVA: 0x003982E0 File Offset: 0x003964E0
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

	// Token: 0x06009871 RID: 39025 RVA: 0x00398330 File Offset: 0x00396530
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

	// Token: 0x1700124D RID: 4685
	// (get) Token: 0x06009872 RID: 39026 RVA: 0x00398387 File Offset: 0x00396587
	// (set) Token: 0x06009873 RID: 39027 RVA: 0x0039838F File Offset: 0x0039658F
	public bool AlwaysShowVersionUi { get; set; } = true;

	// Token: 0x1700124E RID: 4686
	// (get) Token: 0x06009874 RID: 39028 RVA: 0x00398398 File Offset: 0x00396598
	public bool VersionUiVisible
	{
		get
		{
			return this.GetWindow(EnumNGUIWindow.Version).gameObject.activeSelf;
		}
	}

	// Token: 0x06009875 RID: 39029 RVA: 0x003983AB File Offset: 0x003965AB
	public void ToggleVersionUi()
	{
		this.ShowVersionUi(!this.VersionUiVisible);
	}

	// Token: 0x06009876 RID: 39030 RVA: 0x003983BC File Offset: 0x003965BC
	public void ShowVersionUi(bool _show)
	{
		this.GetWindow(EnumNGUIWindow.Version).gameObject.SetActive(_show);
	}

	// Token: 0x06009877 RID: 39031 RVA: 0x003983D0 File Offset: 0x003965D0
	public void SetBackgroundScale(float _uiScale)
	{
		this.GetWindow(EnumNGUIWindow.MainMenuBackground).localScale = new Vector3(_uiScale, _uiScale, _uiScale);
	}

	// Token: 0x04007237 RID: 29239
	public Transform[] Windows;

	// Token: 0x04007238 RID: 29240
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<EnumNGUIWindow, Transform> windowMap = new EnumDictionary<EnumNGUIWindow, Transform>();

	// Token: 0x04007239 RID: 29241
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly HashSet<EnumNGUIWindow> windowsVisibleBeforeHide = new HashSet<EnumNGUIWindow>();

	// Token: 0x0400723A RID: 29242
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bGlobalShowFlag;

	// Token: 0x0400723B RID: 29243
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;

	// Token: 0x0400723C RID: 29244
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool parsedWindows;

	// Token: 0x0400723E RID: 29246
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextEllipsisAnimator ellipsisAnimator;
}
