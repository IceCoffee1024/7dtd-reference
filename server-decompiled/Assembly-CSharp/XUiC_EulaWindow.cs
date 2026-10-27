using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E22 RID: 3618
[Preserve]
public class XUiC_EulaWindow : XUiController
{
	// Token: 0x17000C4E RID: 3150
	// (get) Token: 0x06006E20 RID: 28192 RVA: 0x002AF750 File Offset: 0x002AD950
	[XuiXmlBinding("pages")]
	public Dictionary<string, string> PagesDict
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.pagesDict;
		}
	}

	// Token: 0x17000C4F RID: 3151
	// (get) Token: 0x06006E21 RID: 28193 RVA: 0x002AF758 File Offset: 0x002AD958
	[XuiXmlBinding("pagecount")]
	public int PageCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.pagesDict.Count;
		}
	}

	// Token: 0x17000C50 RID: 3152
	// (get) Token: 0x06006E22 RID: 28194 RVA: 0x002AF765 File Offset: 0x002AD965
	[XuiXmlBinding("viewmode")]
	public bool ViewMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_EulaWindow.viewMode;
		}
	}

	// Token: 0x17000C51 RID: 3153
	// (get) Token: 0x06006E23 RID: 28195 RVA: 0x002AF76C File Offset: 0x002AD96C
	// (set) Token: 0x06006E24 RID: 28196 RVA: 0x002AF774 File Offset: 0x002AD974
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

	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x06006E25 RID: 28197 RVA: 0x002AF78E File Offset: 0x002AD98E
	// (set) Token: 0x06006E26 RID: 28198 RVA: 0x002AF796 File Offset: 0x002AD996
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

	// Token: 0x06006E27 RID: 28199 RVA: 0x002AF7B0 File Offset: 0x002AD9B0
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

	// Token: 0x06006E28 RID: 28200 RVA: 0x002AF80D File Offset: 0x002ADA0D
	public static void Open(XUi _xui, bool _viewMode = false)
	{
		XUiC_EulaWindow.viewMode = _viewMode;
		_xui.playerUI.windowManager.Open(XUiC_EulaWindow.id, true, true);
	}

	// Token: 0x06006E29 RID: 28201 RVA: 0x002AF82C File Offset: 0x002ADA2C
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

	// Token: 0x06006E2A RID: 28202 RVA: 0x002AF879 File Offset: 0x002ADA79
	public override void OnClose()
	{
		base.OnClose();
		this.scrollBar.ScrollPosition = 0f;
		this.scrollView.ResetPosition();
	}

	// Token: 0x06006E2B RID: 28203 RVA: 0x002AF89C File Offset: 0x002ADA9C
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

	// Token: 0x06006E2C RID: 28204 RVA: 0x002AF8F4 File Offset: 0x002ADAF4
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

	// Token: 0x06006E2D RID: 28205 RVA: 0x002AF9A0 File Offset: 0x002ADBA0
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

	// Token: 0x06006E2E RID: 28206 RVA: 0x002AFA88 File Offset: 0x002ADC88
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

	// Token: 0x06006E2F RID: 28207 RVA: 0x002AFAFE File Offset: 0x002ADCFE
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

	// Token: 0x06006E30 RID: 28208 RVA: 0x002AFB10 File Offset: 0x002ADD10
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

	// Token: 0x06006E31 RID: 28209 RVA: 0x002AFC1B File Offset: 0x002ADE1B
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

	// Token: 0x06006E32 RID: 28210 RVA: 0x002AFC54 File Offset: 0x002ADE54
	[XuiBindEvent("OnPress", "btnDecline")]
	[XuiBindEvent("OnPress", "btnDone")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x0400540A RID: 21514
	[PublicizedFrom(EAccessModifier.Private)]
	public const int AlphanumericPageCharacterLimit = 2000;

	// Token: 0x0400540B RID: 21515
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ScriptPageCharacterLimit = 1000;

	// Token: 0x0400540C RID: 21516
	public static string RetrievedEula;

	// Token: 0x0400540D RID: 21517
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x0400540E RID: 21518
	[XuiBindComponent("btnAccept", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnAccept;

	// Token: 0x0400540F RID: 21519
	[XuiBindComponent("btnDecline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDecline;

	// Token: 0x04005410 RID: 21520
	[XuiBindComponent("btnDone", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDone;

	// Token: 0x04005411 RID: 21521
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView scrollView;

	// Token: 0x04005412 RID: 21522
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollBar scrollBar;

	// Token: 0x04005413 RID: 21523
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Table textTable;

	// Token: 0x04005414 RID: 21524
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultEula;

	// Token: 0x04005415 RID: 21525
	[PublicizedFrom(EAccessModifier.Private)]
	public int defaultEulaVersion = -1;

	// Token: 0x04005416 RID: 21526
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, string> pagesDict = new Dictionary<string, string>();

	// Token: 0x04005417 RID: 21527
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageFormatted;

	// Token: 0x04005418 RID: 21528
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool viewMode;

	// Token: 0x04005419 RID: 21529
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowAccept;

	// Token: 0x0400541A RID: 21530
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uiVisible;
}
