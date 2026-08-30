using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200128E RID: 4750
public class NGUIWindowManager : MonoBehaviour
{
	// Token: 0x170011C2 RID: 4546
	// (get) Token: 0x06009685 RID: 38533 RVA: 0x0038C1B0 File Offset: 0x0038A3B0
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

	// Token: 0x170011C3 RID: 4547
	// (get) Token: 0x06009686 RID: 38534 RVA: 0x0038C1CD File Offset: 0x0038A3CD
	// (set) Token: 0x06009687 RID: 38535 RVA: 0x0038C1D5 File Offset: 0x0038A3D5
	public NGuiWdwInGameHUD InGameHUD
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06009688 RID: 38536 RVA: 0x0038C1DE File Offset: 0x0038A3DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.playerUI = base.GetComponent<LocalPlayerUI>();
		this.bGlobalShowFlag = true;
		this.ParseWindows();
	}

	// Token: 0x06009689 RID: 38537 RVA: 0x0038C1FC File Offset: 0x0038A3FC
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

	// Token: 0x0600968A RID: 38538 RVA: 0x0038C288 File Offset: 0x0038A488
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

	// Token: 0x0600968B RID: 38539 RVA: 0x0038C2C0 File Offset: 0x0038A4C0
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

	// Token: 0x0600968C RID: 38540 RVA: 0x0038C394 File Offset: 0x0038A594
	public bool IsShowing(EnumNGUIWindow _eWindow)
	{
		Transform window = this.GetWindow(_eWindow);
		return window != null && window.gameObject.activeSelf;
	}

	// Token: 0x0600968D RID: 38541 RVA: 0x0038C3C0 File Offset: 0x0038A5C0
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

	// Token: 0x0600968E RID: 38542 RVA: 0x0038C40C File Offset: 0x0038A60C
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

	// Token: 0x0600968F RID: 38543 RVA: 0x0038C47C File Offset: 0x0038A67C
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

	// Token: 0x06009690 RID: 38544 RVA: 0x0038C514 File Offset: 0x0038A714
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

	// Token: 0x06009691 RID: 38545 RVA: 0x0038C564 File Offset: 0x0038A764
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

	// Token: 0x170011C4 RID: 4548
	// (get) Token: 0x06009692 RID: 38546 RVA: 0x0038C5BB File Offset: 0x0038A7BB
	// (set) Token: 0x06009693 RID: 38547 RVA: 0x0038C5C3 File Offset: 0x0038A7C3
	public bool AlwaysShowVersionUi { get; set; } = true;

	// Token: 0x170011C5 RID: 4549
	// (get) Token: 0x06009694 RID: 38548 RVA: 0x0038C5CC File Offset: 0x0038A7CC
	public bool VersionUiVisible
	{
		get
		{
			return this.GetWindow(EnumNGUIWindow.Version).gameObject.activeSelf;
		}
	}

	// Token: 0x06009695 RID: 38549 RVA: 0x0038C5DF File Offset: 0x0038A7DF
	public void ToggleVersionUi()
	{
		this.ShowVersionUi(!this.VersionUiVisible);
	}

	// Token: 0x06009696 RID: 38550 RVA: 0x0038C5F0 File Offset: 0x0038A7F0
	public void ShowVersionUi(bool _show)
	{
		this.GetWindow(EnumNGUIWindow.Version).gameObject.SetActive(_show);
	}

	// Token: 0x06009697 RID: 38551 RVA: 0x0038C604 File Offset: 0x0038A804
	public void SetBackgroundScale(float _uiScale)
	{
		this.GetWindow(EnumNGUIWindow.MainMenuBackground).localScale = new Vector3(_uiScale, _uiScale, _uiScale);
	}

	// Token: 0x040070B7 RID: 28855
	public Transform[] Windows;

	// Token: 0x040070B8 RID: 28856
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<EnumNGUIWindow, Transform> windowMap = new EnumDictionary<EnumNGUIWindow, Transform>();

	// Token: 0x040070B9 RID: 28857
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly HashSet<EnumNGUIWindow> windowsVisibleBeforeHide = new HashSet<EnumNGUIWindow>();

	// Token: 0x040070BA RID: 28858
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bGlobalShowFlag;

	// Token: 0x040070BB RID: 28859
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;

	// Token: 0x040070BC RID: 28860
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool parsedWindows;

	// Token: 0x040070BE RID: 28862
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextEllipsisAnimator ellipsisAnimator;
}
