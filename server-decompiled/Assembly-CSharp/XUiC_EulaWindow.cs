using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E17 RID: 3607
[Preserve]
public class XUiC_EulaWindow : XUiController
{
	// Token: 0x17000C48 RID: 3144
	// (get) Token: 0x06006DFB RID: 28155 RVA: 0x002AE1E4 File Offset: 0x002AC3E4
	[XuiXmlBinding("pages")]
	public Dictionary<string, string> PagesDict
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.pagesDict;
		}
	}

	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x06006DFC RID: 28156 RVA: 0x002AE1EC File Offset: 0x002AC3EC
	[XuiXmlBinding("pagecount")]
	public int PageCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.pagesDict.Count;
		}
	}

	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x06006DFD RID: 28157 RVA: 0x002AE1F9 File Offset: 0x002AC3F9
	[XuiXmlBinding("viewmode")]
	public bool ViewMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_EulaWindow.viewMode;
		}
	}

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x06006DFE RID: 28158 RVA: 0x002AE200 File Offset: 0x002AC400
	// (set) Token: 0x06006DFF RID: 28159 RVA: 0x002AE208 File Offset: 0x002AC408
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

	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x06006E00 RID: 28160 RVA: 0x002AE222 File Offset: 0x002AC422
	// (set) Token: 0x06006E01 RID: 28161 RVA: 0x002AE22A File Offset: 0x002AC42A
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

	// Token: 0x06006E02 RID: 28162 RVA: 0x002AE244 File Offset: 0x002AC444
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

	// Token: 0x06006E03 RID: 28163 RVA: 0x002AE2A1 File Offset: 0x002AC4A1
	public static void Open(XUi _xui, bool _viewMode = false)
	{
		XUiC_EulaWindow.viewMode = _viewMode;
		_xui.playerUI.windowManager.Open(XUiC_EulaWindow.id, true, true);
	}

	// Token: 0x06006E04 RID: 28164 RVA: 0x002AE2C0 File Offset: 0x002AC4C0
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

	// Token: 0x06006E05 RID: 28165 RVA: 0x002AE30D File Offset: 0x002AC50D
	public override void OnClose()
	{
		base.OnClose();
		this.scrollBar.ScrollPosition = 0f;
		this.scrollView.ResetPosition();
	}

	// Token: 0x06006E06 RID: 28166 RVA: 0x002AE330 File Offset: 0x002AC530
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

	// Token: 0x06006E07 RID: 28167 RVA: 0x002AE388 File Offset: 0x002AC588
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

	// Token: 0x06006E08 RID: 28168 RVA: 0x002AE434 File Offset: 0x002AC634
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

	// Token: 0x06006E09 RID: 28169 RVA: 0x002AE51C File Offset: 0x002AC71C
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

	// Token: 0x06006E0A RID: 28170 RVA: 0x002AE592 File Offset: 0x002AC792
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

	// Token: 0x06006E0B RID: 28171 RVA: 0x002AE5A4 File Offset: 0x002AC7A4
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

	// Token: 0x06006E0C RID: 28172 RVA: 0x002AE6AF File Offset: 0x002AC8AF
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

	// Token: 0x06006E0D RID: 28173 RVA: 0x002AE6E8 File Offset: 0x002AC8E8
	[XuiBindEvent("OnPress", "btnDecline")]
	[XuiBindEvent("OnPress", "btnDone")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x040053E5 RID: 21477
	[PublicizedFrom(EAccessModifier.Private)]
	public const int AlphanumericPageCharacterLimit = 2000;

	// Token: 0x040053E6 RID: 21478
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ScriptPageCharacterLimit = 1000;

	// Token: 0x040053E7 RID: 21479
	public static string RetrievedEula;

	// Token: 0x040053E8 RID: 21480
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x040053E9 RID: 21481
	[XuiBindComponent("btnAccept", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnAccept;

	// Token: 0x040053EA RID: 21482
	[XuiBindComponent("btnDecline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDecline;

	// Token: 0x040053EB RID: 21483
	[XuiBindComponent("btnDone", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDone;

	// Token: 0x040053EC RID: 21484
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView scrollView;

	// Token: 0x040053ED RID: 21485
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollBar scrollBar;

	// Token: 0x040053EE RID: 21486
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Table textTable;

	// Token: 0x040053EF RID: 21487
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultEula;

	// Token: 0x040053F0 RID: 21488
	[PublicizedFrom(EAccessModifier.Private)]
	public int defaultEulaVersion = -1;

	// Token: 0x040053F1 RID: 21489
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, string> pagesDict = new Dictionary<string, string>();

	// Token: 0x040053F2 RID: 21490
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageFormatted;

	// Token: 0x040053F3 RID: 21491
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool viewMode;

	// Token: 0x040053F4 RID: 21492
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowAccept;

	// Token: 0x040053F5 RID: 21493
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uiVisible;
}
