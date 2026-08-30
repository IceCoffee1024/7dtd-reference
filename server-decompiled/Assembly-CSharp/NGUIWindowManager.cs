using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012A8 RID: 4776
public class NGUIWindowManager : MonoBehaviour
{
	// Token: 0x170011FB RID: 4603
	// (get) Token: 0x06009780 RID: 38784 RVA: 0x0039130C File Offset: 0x0038F50C
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

	// Token: 0x170011FC RID: 4604
	// (get) Token: 0x06009781 RID: 38785 RVA: 0x00391329 File Offset: 0x0038F529
	// (set) Token: 0x06009782 RID: 38786 RVA: 0x00391331 File Offset: 0x0038F531
	public NGuiWdwInGameHUD InGameHUD
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009783 RID: 38787 RVA: 0x0039133A File Offset: 0x0038F53A
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.playerUI = base.GetComponent<LocalPlayerUI>();
		this.bGlobalShowFlag = true;
		this.ParseWindows();
	}

	// Token: 0x06009784 RID: 38788 RVA: 0x00391358 File Offset: 0x0038F558
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

	// Token: 0x06009785 RID: 38789 RVA: 0x003913E4 File Offset: 0x0038F5E4
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

	// Token: 0x06009786 RID: 38790 RVA: 0x0039141C File Offset: 0x0038F61C
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

	// Token: 0x06009787 RID: 38791 RVA: 0x003914F0 File Offset: 0x0038F6F0
	public bool IsShowing(EnumNGUIWindow _eWindow)
	{
		Transform window = this.GetWindow(_eWindow);
		return window != null && window.gameObject.activeSelf;
	}

	// Token: 0x06009788 RID: 38792 RVA: 0x0039151C File Offset: 0x0038F71C
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

	// Token: 0x06009789 RID: 38793 RVA: 0x00391568 File Offset: 0x0038F768
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

	// Token: 0x0600978A RID: 38794 RVA: 0x003915D8 File Offset: 0x0038F7D8
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

	// Token: 0x0600978B RID: 38795 RVA: 0x00391670 File Offset: 0x0038F870
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

	// Token: 0x0600978C RID: 38796 RVA: 0x003916C0 File Offset: 0x0038F8C0
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

	// Token: 0x170011FD RID: 4605
	// (get) Token: 0x0600978D RID: 38797 RVA: 0x00391717 File Offset: 0x0038F917
	// (set) Token: 0x0600978E RID: 38798 RVA: 0x0039171F File Offset: 0x0038F91F
	public bool AlwaysShowVersionUi { get; set; } = true;

	// Token: 0x170011FE RID: 4606
	// (get) Token: 0x0600978F RID: 38799 RVA: 0x00391728 File Offset: 0x0038F928
	public bool VersionUiVisible
	{
		get
		{
			return this.GetWindow(EnumNGUIWindow.Version).gameObject.activeSelf;
		}
	}

	// Token: 0x06009790 RID: 38800 RVA: 0x0039173B File Offset: 0x0038F93B
	public void ToggleVersionUi()
	{
		this.ShowVersionUi(!this.VersionUiVisible);
	}

	// Token: 0x06009791 RID: 38801 RVA: 0x0039174C File Offset: 0x0038F94C
	public void ShowVersionUi(bool _show)
	{
		this.GetWindow(EnumNGUIWindow.Version).gameObject.SetActive(_show);
	}

	// Token: 0x06009792 RID: 38802 RVA: 0x00391760 File Offset: 0x0038F960
	public void SetBackgroundScale(float _uiScale)
	{
		this.GetWindow(EnumNGUIWindow.MainMenuBackground).localScale = new Vector3(_uiScale, _uiScale, _uiScale);
	}

	// Token: 0x0400718C RID: 29068
	public Transform[] Windows;

	// Token: 0x0400718D RID: 29069
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<EnumNGUIWindow, Transform> windowMap = new EnumDictionary<EnumNGUIWindow, Transform>();

	// Token: 0x0400718E RID: 29070
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly HashSet<EnumNGUIWindow> windowsVisibleBeforeHide = new HashSet<EnumNGUIWindow>();

	// Token: 0x0400718F RID: 29071
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bGlobalShowFlag;

	// Token: 0x04007190 RID: 29072
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;

	// Token: 0x04007191 RID: 29073
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool parsedWindows;

	// Token: 0x04007193 RID: 29075
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextEllipsisAnimator ellipsisAnimator;
}
