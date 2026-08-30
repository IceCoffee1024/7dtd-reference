using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DF3 RID: 3571
[Preserve]
public class XUiC_EulaWindow : XUiController
{
	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x06006D63 RID: 28003 RVA: 0x002A7928 File Offset: 0x002A5B28
	[XuiXmlBinding("pages")]
	public Dictionary<string, string> PagesDict
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.pagesDict;
		}
	}

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x06006D64 RID: 28004 RVA: 0x002A7930 File Offset: 0x002A5B30
	[XuiXmlBinding("pagecount")]
	public int PageCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.pagesDict.Count;
		}
	}

	// Token: 0x17000C16 RID: 3094
	// (get) Token: 0x06006D65 RID: 28005 RVA: 0x002A793D File Offset: 0x002A5B3D
	[XuiXmlBinding("viewmode")]
	public bool ViewMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_EulaWindow.viewMode;
		}
	}

	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x06006D66 RID: 28006 RVA: 0x002A7944 File Offset: 0x002A5B44
	// (set) Token: 0x06006D67 RID: 28007 RVA: 0x002A794C File Offset: 0x002A5B4C
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

	// Token: 0x17000C18 RID: 3096
	// (get) Token: 0x06006D68 RID: 28008 RVA: 0x002A7966 File Offset: 0x002A5B66
	// (set) Token: 0x06006D69 RID: 28009 RVA: 0x002A796E File Offset: 0x002A5B6E
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

	// Token: 0x06006D6A RID: 28010 RVA: 0x002A7988 File Offset: 0x002A5B88
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

	// Token: 0x06006D6B RID: 28011 RVA: 0x002A79E5 File Offset: 0x002A5BE5
	public static void Open(XUi _xui, bool _viewMode = false)
	{
		XUiC_EulaWindow.viewMode = _viewMode;
		_xui.playerUI.windowManager.Open(XUiC_EulaWindow.id, true, true);
	}

	// Token: 0x06006D6C RID: 28012 RVA: 0x002A7A04 File Offset: 0x002A5C04
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

	// Token: 0x06006D6D RID: 28013 RVA: 0x002A7A51 File Offset: 0x002A5C51
	public override void OnClose()
	{
		base.OnClose();
		this.scrollBar.ScrollPosition = 0f;
		this.scrollView.ResetPosition();
	}

	// Token: 0x06006D6E RID: 28014 RVA: 0x002A7A74 File Offset: 0x002A5C74
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

	// Token: 0x06006D6F RID: 28015 RVA: 0x002A7ACC File Offset: 0x002A5CCC
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

	// Token: 0x06006D70 RID: 28016 RVA: 0x002A7B78 File Offset: 0x002A5D78
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

	// Token: 0x06006D71 RID: 28017 RVA: 0x002A7C60 File Offset: 0x002A5E60
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

	// Token: 0x06006D72 RID: 28018 RVA: 0x002A7CD6 File Offset: 0x002A5ED6
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

	// Token: 0x06006D73 RID: 28019 RVA: 0x002A7CE8 File Offset: 0x002A5EE8
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

	// Token: 0x06006D74 RID: 28020 RVA: 0x002A7DF3 File Offset: 0x002A5FF3
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

	// Token: 0x06006D75 RID: 28021 RVA: 0x002A7E2C File Offset: 0x002A602C
	[XuiBindEvent("OnPress", "btnDecline")]
	[XuiBindEvent("OnPress", "btnDone")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDecline_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.close();
	}

	// Token: 0x0400530B RID: 21259
	[PublicizedFrom(EAccessModifier.Private)]
	public const int AlphanumericPageCharacterLimit = 2000;

	// Token: 0x0400530C RID: 21260
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ScriptPageCharacterLimit = 1000;

	// Token: 0x0400530D RID: 21261
	public static string RetrievedEula;

	// Token: 0x0400530E RID: 21262
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x0400530F RID: 21263
	[XuiBindComponent("btnAccept", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnAccept;

	// Token: 0x04005310 RID: 21264
	[XuiBindComponent("btnDecline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDecline;

	// Token: 0x04005311 RID: 21265
	[XuiBindComponent("btnDone", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDone;

	// Token: 0x04005312 RID: 21266
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView scrollView;

	// Token: 0x04005313 RID: 21267
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollBar scrollBar;

	// Token: 0x04005314 RID: 21268
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Table textTable;

	// Token: 0x04005315 RID: 21269
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultEula;

	// Token: 0x04005316 RID: 21270
	[PublicizedFrom(EAccessModifier.Private)]
	public int defaultEulaVersion = -1;

	// Token: 0x04005317 RID: 21271
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, string> pagesDict = new Dictionary<string, string>();

	// Token: 0x04005318 RID: 21272
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pageFormatted;

	// Token: 0x04005319 RID: 21273
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool viewMode;

	// Token: 0x0400531A RID: 21274
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowAccept;

	// Token: 0x0400531B RID: 21275
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uiVisible;
}
