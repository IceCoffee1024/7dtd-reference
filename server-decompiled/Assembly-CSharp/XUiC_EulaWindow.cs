using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E08 RID: 3592
[Preserve]
public class XUiC_EulaWindow : XUiController
{
	// Token: 0x17000C2D RID: 3117
	// (get) Token: 0x06006E03 RID: 28163 RVA: 0x002AAE3C File Offset: 0x002A903C
	[XuiXmlBinding("pages")]
	public Dictionary<string, string> PagesDict
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.pagesDict;
		}
	}

	// Token: 0x17000C2E RID: 3118
	// (get) Token: 0x06006E04 RID: 28164 RVA: 0x002AAE44 File Offset: 0x002A9044
	[XuiXmlBinding("pagecount")]
	public int PageCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.pagesDict.Count;
		}
	}

	// Token: 0x17000C2F RID: 3119
	// (get) Token: 0x06006E05 RID: 28165 RVA: 0x002AAE51 File Offset: 0x002A9051
	[XuiXmlBinding("viewmode")]
	public bool ViewMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_EulaWindow.viewMode;
		}
	}

	// Token: 0x17000C30 RID: 3120
	// (get) Token: 0x06006E06 RID: 28166 RVA: 0x002AAE58 File Offset: 0x002A9058
	// (set) Token: 0x06006E07 RID: 28167 RVA: 0x002AAE60 File Offset: 0x002A9060
	[XuiXmlBinding("allowaccept")]
	public bool AllowAccept
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.allowAccept;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.allowAccept == value)
			{
				return;
			}
			this.allowAccept = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C31 RID: 3121
	// (get) Token: 0x06006E08 RID: 28168 RVA: 0x002AAE7A File Offset: 0x002A907A
	// (set) Token: 0x06006E09 RID: 28169 RVA: 0x002AAE82 File Offset: 0x002A9082
	[XuiXmlBinding("uivisible")]
	public bool UIVisible
	{
		get
		{
			return this.uiVisible;
		}
		set
		{
			if (this.uiVisible == value)
			{
				return;
			}
			this.uiVisible = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006E0A RID: 28170 RVA: 0x002AAE9C File Offset: 0x002A909C
	public override void Init()
	{
		base.Init();
		XUiC_EulaWindow.id = base.WindowGroup.Id;
		TextAsset textAsset = Resources.Load<TextAsset>("Data/EULA/eula_" + Localization.ActiveLanguage.ToLower());
		if (textAsset != null)
		{
			this.loadDefaultXML(textAsset.bytes);
			return;
		}
		Log.Error("Could not load default EULA text asset");
	}

	// Token: 0x06006E0B RID: 28171 RVA: 0x002AAEF9 File Offset: 0x002A90F9
	public static void Open(XUi _xui, bool _viewMode = false)
	{
		XUiC_EulaWindow.viewMode = _viewMode;
		_xui.playerUI.windowManager.Open(XUiC_EulaWindow.id, true, true);
	}

	// Token: 0x06006E0C RID: 28172 RVA: 0x002AAF18 File Offset: 0x002A9118
	public override void OnOpen()
	{
		base.OnOpen();
		this.scrollBar.ScrollPosition = 0f;
		this.pageFormatted = false;
		this.UIVisible = false;
		this.AllowAccept = false;
		if (XUiC_EulaWindow.viewMode)
		{
			this.btnDone.SelectCursorElement(true, false);
		}
	}

	// Token: 0x06006E0D RID: 28173 RVA: 0x002AAF65 File Offset: 0x002A9165
	public override void OnClose()
	{
		base.OnClose();
		this.scrollBar.ScrollPosition = 0f;
		this.scrollView.ResetPosition();
	}

	// Token: 0x06006E0E RID: 28174 RVA: 0x002AAF88 File Offset: 0x002A9188
	[PublicizedFrom(EAccessModifier.Private)]
	public void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (XUiC_EulaWindow.viewMode)
		{
			this.xui.playerUI.windowManager.Open(XUiC_OptionsGeneral.ID, true);
			return;
		}
		XUiC_MainMenu.Open(this.xui);
	}

	// Token: 0x06006E0F RID: 28175 RVA: 0x002AAFE0 File Offset: 0x002A91E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void loadDefaultXML(byte[] _data)
	{
		XmlFile xmlFile;
		try
		{
			xmlFile = new XmlFile(_data, true);
		}
		catch (Exception ex)
		{
			Log.Error("Failed loading default EULA XML: {0}", new object[] { ex.Message });
			return;
		}
		XElement root = xmlFile.XmlDoc.Root;
		if (root == null)
		{
			return;
		}
		this.defaultEulaVersion = int.Parse(root.GetAttribute("version").Trim());
		this.defaultEula = root.Value;
		if (this.defaultEulaVersion > GamePrefs.GetInt(EnumGamePrefs.EulaLatestVersion))
		{
			GamePrefs.Set(EnumGamePrefs.EulaLatestVersion, this.defaultEulaVersion);
		}
		Log.Out("Loaded default EULA");
	}

	// Token: 0x06006E10 RID: 28176 RVA: 0x002AB08C File Offset: 0x002A928C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (!this.pageFormatted)
		{
			if (GameManager.UpdatingRemoteResources || !GameManager.RemoteResourcesLoaded)
			{
				return;
			}
			this.pageFormatted = true;
			this.displayEulaOrClose();
		}
		if (!XUiC_EulaWindow.viewMode && (double)this.scrollBar.ScrollPosition >= 0.98 && !this.AllowAccept)
		{
			this.xui.playerUI.CursorController.SetNavigationTarget(this.btnDecline.ViewComponent);
			this.AllowAccept = true;
		}
		if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			this.close();
		}
		if (!XUiC_EulaWindow.viewMode && this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.btnAccept_OnPressed(this, -1);
		}
	}

	// Token: 0x06006E11 RID: 28177 RVA: 0x002AB174 File Offset: 0x002A9374
	[PublicizedFrom(EAccessModifier.Private)]
	public void displayEulaOrClose()
	{
		bool flag = string.IsNullOrEmpty(XUiC_EulaWindow.RetrievedEula);
		bool flag2 = (flag ? (GamePrefs.GetInt(EnumGamePrefs.EulaVersionAccepted) >= this.defaultEulaVersion) : GameManager.HasAcceptedLatestEula());
		if (!XUiC_EulaWindow.viewMode && flag2)
		{
			this.close();
			return;
		}
		this.UIVisible = true;
		string text = (flag ? this.defaultEula : XUiC_EulaWindow.RetrievedEula);
		this.formatPages(text);
		ThreadManager.StartCoroutine(this.updateScrolling());
	}

	// Token: 0x06006E12 RID: 28178 RVA: 0x002AB1EA File Offset: 0x002A93EA
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator updateScrolling()
	{
		this.textTable.Reposition();
		yield return null;
		this.textTable.Reposition();
		this.scrollView.ResetPosition();
		int num;
		for (int i = 0; i < 5; i = num + 1)
		{
			yield return null;
			this.scrollView.UpdatePosition();
			num = i;
		}
		yield break;
	}

	// Token: 0x06006E13 RID: 28179 RVA: 0x002AB1FC File Offset: 0x002A93FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void formatPages(string _content)
	{
		_content = _content.Replace("\t", "  ");
		this.pagesDict.Clear();
		string[] array = _content.Split('\n', StringSplitOptions.None);
		string activeLanguage = Localization.ActiveLanguage;
		int num;
		if (activeLanguage == "japanese" || activeLanguage == "koreana" || activeLanguage == "schinese" || activeLanguage == "tchinese")
		{
			num = 1000;
		}
		else
		{
			num = 2000;
		}
		int num2 = num;
		StringBuilder stringBuilder = new StringBuilder();
		int i = 0;
		while (i < array.Length)
		{
			stringBuilder.AppendLine(array[i]);
			stringBuilder.AppendLine();
			i++;
			while (stringBuilder.Length < num2 && i < array.Length)
			{
				stringBuilder.AppendLine(array[i]);
				stringBuilder.AppendLine();
				i++;
			}
			stringBuilder.Length--;
			this.pagesDict.Add(this.pagesDict.Count.ToString(), stringBuilder.ToString());
			stringBuilder.Clear();
		}
	}

	// Token: 0x06006E14 RID: 28180 RVA: 0x002AB307 File Offset: 0x002A9507
	[XuiBindEvent("OnPress", "btnAccept")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnAccept_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.btnAccept.ViewComponent.Enabled)
		{
			return;
		}
		GamePrefs.Set(EnumGamePrefs.EulaVersionAccepted, GamePrefs.GetInt(EnumGamePrefs.EulaLatestVersion));
		GamePrefs.Instance.Save();
		this.close();
	}

	// Token: 0x06006E15 RID: 28181 RVA: 0x002AB340 File Offset: 0x002A9540
	[XuiBindEvent("OnPress", "btnDecline")]
	[XuiBindEvent("OnPress", "btnDone")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x040053AE RID: 21422
	[PublicizedFrom(EAccessModifier.Private)]
	public const int AlphanumericPageCharacterLimit = 2000;

	// Token: 0x040053AF RID: 21423
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ScriptPageCharacterLimit = 1000;

	// Token: 0x040053B0 RID: 21424
	public static string RetrievedEula;

	// Token: 0x040053B1 RID: 21425
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x040053B2 RID: 21426
	[XuiBindComponent("btnAccept", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnAccept;

	// Token: 0x040053B3 RID: 21427
	[XuiBindComponent("btnDecline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDecline;

	// Token: 0x040053B4 RID: 21428
	[XuiBindComponent("btnDone", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDone;

	// Token: 0x040053B5 RID: 21429
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView scrollView;

	// Token: 0x040053B6 RID: 21430
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollBar scrollBar;

	// Token: 0x040053B7 RID: 21431
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Table textTable;

	// Token: 0x040053B8 RID: 21432
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultEula;

	// Token: 0x040053B9 RID: 21433
	[PublicizedFrom(EAccessModifier.Private)]
	public int defaultEulaVersion = -1;

	// Token: 0x040053BA RID: 21434
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, string> pagesDict = new Dictionary<string, string>();

	// Token: 0x040053BB RID: 21435
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageFormatted;

	// Token: 0x040053BC RID: 21436
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool viewMode;

	// Token: 0x040053BD RID: 21437
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowAccept;

	// Token: 0x040053BE RID: 21438
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uiVisible;
}
