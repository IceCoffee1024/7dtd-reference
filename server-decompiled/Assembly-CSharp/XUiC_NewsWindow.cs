using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E3F RID: 3647
[Preserve]
public class XUiC_NewsWindow : XUiController
{
	// Token: 0x17000C93 RID: 3219
	// (get) Token: 0x06006F5C RID: 28508 RVA: 0x002B4ABF File Offset: 0x002B2CBF
	// (set) Token: 0x06006F5D RID: 28509 RVA: 0x002B4AC7 File Offset: 0x002B2CC7
	[XuiXmlBinding("has_news_provider")]
	public bool HasProviders
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C94 RID: 3220
	// (get) Token: 0x06006F5E RID: 28510 RVA: 0x002B4AD0 File Offset: 0x002B2CD0
	// (set) Token: 0x06006F5F RID: 28511 RVA: 0x002B4AD8 File Offset: 0x002B2CD8
	[XuiXmlAttribute("max_entries", false)]
	public int MaxEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = int.MaxValue;

	// Token: 0x17000C95 RID: 3221
	// (get) Token: 0x06006F60 RID: 28512 RVA: 0x002B4AE1 File Offset: 0x002B2CE1
	// (set) Token: 0x06006F61 RID: 28513 RVA: 0x002B4AE9 File Offset: 0x002B2CE9
	[XuiXmlAttribute("auto_cycle_time_per_entry", false)]
	public float AutoCycleTimePerEntry
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x06006F62 RID: 28514 RVA: 0x002B4AF4 File Offset: 0x002B2CF4
	// (remove) Token: 0x06006F63 RID: 28515 RVA: 0x002B4B2C File Offset: 0x002B2D2C
	public event XUiC_NewsWindow.NewsEntryClickedDelegate NewsEntryClicked;

	// Token: 0x17000C96 RID: 3222
	// (get) Token: 0x06006F64 RID: 28516 RVA: 0x002B4B61 File Offset: 0x002B2D61
	// (set) Token: 0x06006F65 RID: 28517 RVA: 0x002B4B6C File Offset: 0x002B2D6C
	public int CurrentIndex
	{
		get
		{
			return this.currentIndex;
		}
		set
		{
			if (value >= this.entries.Count)
			{
				value = this.entries.Count - 1;
			}
			if (value >= this.MaxEntries)
			{
				value = this.MaxEntries - 1;
			}
			if (value < 0)
			{
				value = 0;
			}
			if (value == this.currentIndex)
			{
				return;
			}
			this.currentIndex = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C97 RID: 3223
	// (get) Token: 0x06006F66 RID: 28518 RVA: 0x002B4BC8 File Offset: 0x002B2DC8
	public NewsManager.NewsEntry CurrentEntry
	{
		get
		{
			if (this.CurrentIndex < 0)
			{
				return null;
			}
			if (this.CurrentIndex >= this.entries.Count)
			{
				return null;
			}
			object obj = this.newsLock;
			NewsManager.NewsEntry newsEntry;
			lock (obj)
			{
				newsEntry = this.entries[this.CurrentIndex];
			}
			return newsEntry;
		}
	}

	// Token: 0x17000C98 RID: 3224
	// (get) Token: 0x06006F67 RID: 28519 RVA: 0x002B4C38 File Offset: 0x002B2E38
	public NewsManager.NewsEntry CurrentEntryOrEmpty
	{
		get
		{
			return this.CurrentEntry ?? NewsManager.EmptyEntry;
		}
	}

	// Token: 0x06006F68 RID: 28520 RVA: 0x002B4C49 File Offset: 0x002B2E49
	[XuiXmlAttribute("browse_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.browseSound = _o;
		});
	}

	// Token: 0x06006F69 RID: 28521 RVA: 0x002B4C64 File Offset: 0x002B2E64
	[PublicizedFrom(EAccessModifier.Private)]
	public void parseSourcesString(string _value)
	{
		string[] array = _value.Split(',', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.Length != 0)
			{
				this.newsProviders.Add(text);
				NewsManager.Instance.RegisterNewsSource(text);
			}
		}
	}

	// Token: 0x06006F6A RID: 28522 RVA: 0x002B4CB1 File Offset: 0x002B2EB1
	[XuiXmlAttribute("additional_sources", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribAdditionalSources(string _value)
	{
		this.parseSourcesString(_value);
	}

	// Token: 0x06006F6B RID: 28523 RVA: 0x002B4CB1 File Offset: 0x002B2EB1
	[XuiXmlAttribute("sources", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribSources(string _value)
	{
		this.parseSourcesString(_value);
	}

	// Token: 0x17000C99 RID: 3225
	// (get) Token: 0x06006F6C RID: 28524 RVA: 0x002B4CBA File Offset: 0x002B2EBA
	[XuiXmlBinding("news_count")]
	public int NewsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.Min(this.entries.Count, this.MaxEntries);
		}
	}

	// Token: 0x17000C9A RID: 3226
	// (get) Token: 0x06006F6D RID: 28525 RVA: 0x002B4CD2 File Offset: 0x002B2ED2
	[XuiXmlBinding("is_custom")]
	public bool NewsIsCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.IsCustom;
		}
	}

	// Token: 0x17000C9B RID: 3227
	// (get) Token: 0x06006F6E RID: 28526 RVA: 0x002B4CDF File Offset: 0x002B2EDF
	[XuiXmlBinding("source")]
	public string NewsSource
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.CustomListName ?? "";
		}
	}

	// Token: 0x17000C9C RID: 3228
	// (get) Token: 0x06006F6F RID: 28527 RVA: 0x002B4CF5 File Offset: 0x002B2EF5
	[XuiXmlBinding("has_text")]
	public bool NewsHasText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(this.CurrentEntryOrEmpty.Text);
		}
	}

	// Token: 0x17000C9D RID: 3229
	// (get) Token: 0x06006F70 RID: 28528 RVA: 0x002B4D0A File Offset: 0x002B2F0A
	[XuiXmlBinding("headline")]
	public string NewsHeadline
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Headline ?? "";
		}
	}

	// Token: 0x17000C9E RID: 3230
	// (get) Token: 0x06006F71 RID: 28529 RVA: 0x002B4D20 File Offset: 0x002B2F20
	[XuiXmlBinding("headline2")]
	public string NewsHeadline2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Headline2 ?? "";
		}
	}

	// Token: 0x17000C9F RID: 3231
	// (get) Token: 0x06006F72 RID: 28530 RVA: 0x002B4D38 File Offset: 0x002B2F38
	[XuiXmlBinding("date")]
	public string NewsDate
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Date.ToString("yyyy-MM-dd");
		}
	}

	// Token: 0x17000CA0 RID: 3232
	// (get) Token: 0x06006F73 RID: 28531 RVA: 0x002B4D5D File Offset: 0x002B2F5D
	[XuiXmlBinding("age")]
	public string NewsAge
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ValueDisplayFormatters.DateAge(this.CurrentEntryOrEmpty.Date);
		}
	}

	// Token: 0x17000CA1 RID: 3233
	// (get) Token: 0x06006F74 RID: 28532 RVA: 0x002B4D6F File Offset: 0x002B2F6F
	[XuiXmlBinding("text")]
	public string NewsText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Text ?? "";
		}
	}

	// Token: 0x17000CA2 RID: 3234
	// (get) Token: 0x06006F75 RID: 28533 RVA: 0x002B4D85 File Offset: 0x002B2F85
	[XuiXmlBinding("has_link")]
	public bool NewsHasLink
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(this.CurrentEntryOrEmpty.Url);
		}
	}

	// Token: 0x17000CA3 RID: 3235
	// (get) Token: 0x06006F76 RID: 28534 RVA: 0x002B4D9A File Offset: 0x002B2F9A
	[XuiXmlBinding("link")]
	public string NewsLink
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Url ?? "";
		}
	}

	// Token: 0x17000CA4 RID: 3236
	// (get) Token: 0x06006F77 RID: 28535 RVA: 0x002B4DB0 File Offset: 0x002B2FB0
	[XuiXmlBinding("has_younger")]
	public bool HasYounger
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.entries.Count > 0 && this.CurrentIndex > 0;
		}
	}

	// Token: 0x17000CA5 RID: 3237
	// (get) Token: 0x06006F78 RID: 28536 RVA: 0x002B4DCB File Offset: 0x002B2FCB
	[XuiXmlBinding("has_older")]
	public bool HasOlder
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.entries.Count > 0 && this.CurrentIndex < this.entries.Count - 1;
		}
	}

	// Token: 0x06006F79 RID: 28537 RVA: 0x002B4DF4 File Offset: 0x002B2FF4
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("btnYounger");
		if (childById != null)
		{
			childById.OnPress += delegate(XUiController _, int _)
			{
				this.cycle(-1, false);
			};
		}
		XUiController childById2 = base.GetChildById("btnOlder");
		if (childById2 != null)
		{
			childById2.OnPress += delegate(XUiController _, int _)
			{
				this.cycle(1, false);
			};
		}
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnLink") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.BtnLink_OnPressed;
		}
		else
		{
			XUiController childById3 = base.GetChildById("btnLink");
			if (childById3 != null && childById3.ViewComponent is XUiV_Button)
			{
				childById3.OnPress += this.BtnLink_OnPressed;
				childById3.OnScroll += this.OnScrollEvent;
			}
		}
		base.OnScroll += this.OnScrollEvent;
		List<XUiV_Texture> list = new List<XUiV_Texture>();
		foreach (XUiController xuiController in base.GetChildrenById("newsImage", null))
		{
			XUiV_Texture xuiV_Texture = ((xuiController != null) ? xuiController.ViewComponent : null) as XUiV_Texture;
			if (xuiV_Texture != null)
			{
				list.Add(xuiV_Texture);
			}
		}
		this.bannerTextures = list.ToArray();
		this.selector = base.GetChildByType<XUiC_ComboBoxFloat>();
		if (this.selector != null)
		{
			this.selector.OnValueChanged += this.Selector_OnValueChanged;
		}
		this.HasProviders = this.newsProviders.Count > 0;
		if (!this.HasProviders)
		{
			Log.Warning(string.Concat(new string[]
			{
				"[XUi] News controller with no sources specified (window group '",
				base.WindowGroup.Id,
				"', window '",
				base.ViewComponent.ID,
				"')"
			}));
		}
		NewsManager.Instance.Updated += this.newsUpdated;
		this.newsUpdated(NewsManager.Instance);
	}

	// Token: 0x06006F7A RID: 28538 RVA: 0x002B4FCB File Offset: 0x002B31CB
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrollEvent(XUiController _sender, float _delta)
	{
		if (this.selector == null)
		{
			this.cycle(Math.Sign(_delta), true);
			return;
		}
		this.selector.ScrollEvent(_sender, _delta);
	}

	// Token: 0x06006F7B RID: 28539 RVA: 0x002B4FF0 File Offset: 0x002B31F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Selector_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.CurrentIndex = Mathf.CeilToInt((float)_newValue) - 1;
		this.selector.Value = (double)(this.CurrentIndex + 1);
		this.autoCycle = false;
	}

	// Token: 0x06006F7C RID: 28540 RVA: 0x002B501C File Offset: 0x002B321C
	[PublicizedFrom(EAccessModifier.Private)]
	public void newsUpdated(NewsManager _newsManager)
	{
		NewsManager.NewsEntry currentEntry = this.CurrentEntry;
		object obj = this.newsLock;
		lock (obj)
		{
			_newsManager.GetNewsData(this.newsProviders, this.entries);
		}
		int num = 0;
		while (num < this.MaxEntries && num < this.entries.Count)
		{
			this.entries[num].RequestImage();
			num++;
		}
		this.IsDirty = true;
		if (currentEntry == null || !currentEntry.Equals(this.CurrentEntry))
		{
			this.resetIndex();
		}
	}

	// Token: 0x06006F7D RID: 28541 RVA: 0x002B50C0 File Offset: 0x002B32C0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.doAutoCycle(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			if (this.bannerTextures != null && this.bannerTextures.Length != 0)
			{
				NewsManager.NewsEntry currentEntry = this.CurrentEntry;
				Texture2D texture2D = ((currentEntry != null) ? currentEntry.Image : null);
				XUiV_Texture[] array = this.bannerTextures;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Texture = texture2D;
				}
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x06006F7E RID: 28542 RVA: 0x002B5134 File Offset: 0x002B3334
	[PublicizedFrom(EAccessModifier.Private)]
	public void doAutoCycle(float _dt)
	{
		if (this.selector == null || !this.autoCycle)
		{
			return;
		}
		NewsManager.NewsEntry currentEntry = this.CurrentEntry;
		if (currentEntry == null || (currentEntry.HasImage && !currentEntry.ImageLoaded))
		{
			return;
		}
		float num = (float)this.selector.Value + _dt / this.AutoCycleTimePerEntry;
		if ((double)num > this.selector.Max)
		{
			num = 0f;
		}
		this.selector.Value = (double)num;
		this.CurrentIndex = Mathf.CeilToInt((float)this.selector.Value) - 1;
	}

	// Token: 0x06006F7F RID: 28543 RVA: 0x002B51C0 File Offset: 0x002B33C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLink_OnPressed(XUiController _sender, int _mousebutton)
	{
		NewsManager.NewsEntry currentEntry = this.CurrentEntry;
		if (currentEntry == null)
		{
			return;
		}
		if (this.NewsEntryClicked != null)
		{
			this.NewsEntryClicked(currentEntry);
			return;
		}
		string url = currentEntry.Url;
		if (url == null || !url.StartsWith("openstore://"))
		{
			XUiC_MessageBoxWindowGroup.ShowUrlConfirmationDialog(this.xui, currentEntry.Url, false, null, null, null, "", null, false);
			return;
		}
		EntitlementSetEnum entitlementSetEnum;
		try
		{
			entitlementSetEnum = Enum.Parse<EntitlementSetEnum>(currentEntry.Url.Substring("openstore://".Length), true);
		}
		catch (Exception)
		{
			Log.Error("DLC link uses incorrect value!");
			entitlementSetEnum = EntitlementSetEnum.None;
		}
		if (entitlementSetEnum != EntitlementSetEnum.None)
		{
			EntitlementManager.Instance.OpenStore(entitlementSetEnum, delegate(EntitlementSetEnum _)
			{
				Log.Out("DLC dialog complete!");
			});
		}
	}

	// Token: 0x06006F80 RID: 28544 RVA: 0x002B5290 File Offset: 0x002B3490
	[PublicizedFrom(EAccessModifier.Private)]
	public void cycle(int _increment, bool _sound)
	{
		object obj = this.newsLock;
		lock (obj)
		{
			int num = this.CurrentIndex;
			this.CurrentIndex += _increment;
			if (this.CurrentIndex != num && _sound && this.browseSound != null)
			{
				Manager.PlayXUiSound(this.browseSound, 1f);
			}
			this.autoCycle = false;
			if (this.selector != null)
			{
				this.selector.Value = (double)(this.CurrentIndex + 1);
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x06006F81 RID: 28545 RVA: 0x002B5338 File Offset: 0x002B3538
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetIndex()
	{
		this.CurrentIndex = 0;
		if (this.selector != null)
		{
			this.selector.Value = (double)(this.autoCycle ? 0 : 1);
		}
	}

	// Token: 0x06006F82 RID: 28546 RVA: 0x002B5361 File Offset: 0x002B3561
	public override void OnOpen()
	{
		base.OnOpen();
		this.autoCycle = this.AutoCycleTimePerEntry > 0f;
		this.resetIndex();
		base.RefreshBindings();
	}

	// Token: 0x040054B0 RID: 21680
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PlatformStoreLink = "openstore://";

	// Token: 0x040054B1 RID: 21681
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip browseSound;

	// Token: 0x040054B2 RID: 21682
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat selector;

	// Token: 0x040054B3 RID: 21683
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture[] bannerTextures;

	// Token: 0x040054B4 RID: 21684
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object newsLock = new object();

	// Token: 0x040054B5 RID: 21685
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x040054B6 RID: 21686
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NewsManager.NewsEntry> entries = new List<NewsManager.NewsEntry>();

	// Token: 0x040054B8 RID: 21688
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> newsProviders = new List<string>();

	// Token: 0x040054BB RID: 21691
	[PublicizedFrom(EAccessModifier.Private)]
	public bool autoCycle;

	// Token: 0x02000E40 RID: 3648
	// (Invoke) Token: 0x06006F88 RID: 28552
	public delegate void NewsEntryClickedDelegate(NewsManager.NewsEntry _newsEntry);
}
