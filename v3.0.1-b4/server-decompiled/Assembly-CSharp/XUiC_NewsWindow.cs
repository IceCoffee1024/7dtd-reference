using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E10 RID: 3600
[Preserve]
public class XUiC_NewsWindow : XUiController
{
	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x06006E9F RID: 28319 RVA: 0x002ACC97 File Offset: 0x002AAE97
	// (set) Token: 0x06006EA0 RID: 28320 RVA: 0x002ACC9F File Offset: 0x002AAE9F
	[XuiXmlBinding("has_news_provider")]
	public bool HasProviders
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x06006EA1 RID: 28321 RVA: 0x002ACCA8 File Offset: 0x002AAEA8
	// (set) Token: 0x06006EA2 RID: 28322 RVA: 0x002ACCB0 File Offset: 0x002AAEB0
	[XuiXmlAttribute("max_entries", false)]
	public int MaxEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = int.MaxValue;

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x06006EA3 RID: 28323 RVA: 0x002ACCB9 File Offset: 0x002AAEB9
	// (set) Token: 0x06006EA4 RID: 28324 RVA: 0x002ACCC1 File Offset: 0x002AAEC1
	[XuiXmlAttribute("auto_cycle_time_per_entry", false)]
	public float AutoCycleTimePerEntry
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x06006EA5 RID: 28325 RVA: 0x002ACCCC File Offset: 0x002AAECC
	// (remove) Token: 0x06006EA6 RID: 28326 RVA: 0x002ACD04 File Offset: 0x002AAF04
	public event XUiC_NewsWindow.NewsEntryClickedDelegate NewsEntryClicked;

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x06006EA7 RID: 28327 RVA: 0x002ACD39 File Offset: 0x002AAF39
	// (set) Token: 0x06006EA8 RID: 28328 RVA: 0x002ACD44 File Offset: 0x002AAF44
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
			this.currentIndex = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x06006EA9 RID: 28329 RVA: 0x002ACD98 File Offset: 0x002AAF98
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

	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x06006EAA RID: 28330 RVA: 0x002ACE08 File Offset: 0x002AB008
	public NewsManager.NewsEntry CurrentEntryOrEmpty
	{
		get
		{
			return this.CurrentEntry ?? NewsManager.EmptyEntry;
		}
	}

	// Token: 0x06006EAB RID: 28331 RVA: 0x002ACE19 File Offset: 0x002AB019
	[XuiXmlAttribute("browse_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.browseSound = _o;
		});
	}

	// Token: 0x06006EAC RID: 28332 RVA: 0x002ACE34 File Offset: 0x002AB034
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

	// Token: 0x06006EAD RID: 28333 RVA: 0x002ACE81 File Offset: 0x002AB081
	[XuiXmlAttribute("additional_sources", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribAdditionalSources(string _value)
	{
		this.parseSourcesString(_value);
	}

	// Token: 0x06006EAE RID: 28334 RVA: 0x002ACE81 File Offset: 0x002AB081
	[XuiXmlAttribute("sources", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribSources(string _value)
	{
		this.parseSourcesString(_value);
	}

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x06006EAF RID: 28335 RVA: 0x002ACE8A File Offset: 0x002AB08A
	[XuiXmlBinding("news_count")]
	public int NewsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.Min(this.entries.Count, this.MaxEntries);
		}
	}

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x06006EB0 RID: 28336 RVA: 0x002ACEA2 File Offset: 0x002AB0A2
	[XuiXmlBinding("is_custom")]
	public bool NewsIsCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.IsCustom;
		}
	}

	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x06006EB1 RID: 28337 RVA: 0x002ACEAF File Offset: 0x002AB0AF
	[XuiXmlBinding("source")]
	public string NewsSource
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.CustomListName ?? "";
		}
	}

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x06006EB2 RID: 28338 RVA: 0x002ACEC5 File Offset: 0x002AB0C5
	[XuiXmlBinding("has_text")]
	public bool NewsHasText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(this.CurrentEntryOrEmpty.Text);
		}
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x06006EB3 RID: 28339 RVA: 0x002ACEDA File Offset: 0x002AB0DA
	[XuiXmlBinding("headline")]
	public string NewsHeadline
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Headline ?? "";
		}
	}

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x06006EB4 RID: 28340 RVA: 0x002ACEF0 File Offset: 0x002AB0F0
	[XuiXmlBinding("headline2")]
	public string NewsHeadline2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Headline2 ?? "";
		}
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x06006EB5 RID: 28341 RVA: 0x002ACF08 File Offset: 0x002AB108
	[XuiXmlBinding("date")]
	public string NewsDate
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Date.ToString("yyyy-MM-dd");
		}
	}

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x06006EB6 RID: 28342 RVA: 0x002ACF2D File Offset: 0x002AB12D
	[XuiXmlBinding("age")]
	public string NewsAge
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ValueDisplayFormatters.DateAge(this.CurrentEntryOrEmpty.Date);
		}
	}

	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x06006EB7 RID: 28343 RVA: 0x002ACF3F File Offset: 0x002AB13F
	[XuiXmlBinding("text")]
	public string NewsText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Text ?? "";
		}
	}

	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x06006EB8 RID: 28344 RVA: 0x002ACF55 File Offset: 0x002AB155
	[XuiXmlBinding("has_link")]
	public bool NewsHasLink
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(this.CurrentEntryOrEmpty.Url);
		}
	}

	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x06006EB9 RID: 28345 RVA: 0x002ACF6A File Offset: 0x002AB16A
	[XuiXmlBinding("link")]
	public string NewsLink
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Url ?? "";
		}
	}

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x06006EBA RID: 28346 RVA: 0x002ACF80 File Offset: 0x002AB180
	[XuiXmlBinding("has_younger")]
	public bool HasYounger
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.entries.Count > 0 && this.CurrentIndex > 0;
		}
	}

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x06006EBB RID: 28347 RVA: 0x002ACF9B File Offset: 0x002AB19B
	[XuiXmlBinding("has_older")]
	public bool HasOlder
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.entries.Count > 0 && this.CurrentIndex < this.entries.Count - 1;
		}
	}

	// Token: 0x06006EBC RID: 28348 RVA: 0x002ACFC4 File Offset: 0x002AB1C4
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

	// Token: 0x06006EBD RID: 28349 RVA: 0x002AD19B File Offset: 0x002AB39B
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

	// Token: 0x06006EBE RID: 28350 RVA: 0x002AD1C0 File Offset: 0x002AB3C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Selector_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.CurrentIndex = Mathf.CeilToInt((float)_newValue) - 1;
		this.selector.Value = (double)(this.CurrentIndex + 1);
		this.autoCycle = false;
	}

	// Token: 0x06006EBF RID: 28351 RVA: 0x002AD1EC File Offset: 0x002AB3EC
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

	// Token: 0x06006EC0 RID: 28352 RVA: 0x002AD290 File Offset: 0x002AB490
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

	// Token: 0x06006EC1 RID: 28353 RVA: 0x002AD304 File Offset: 0x002AB504
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

	// Token: 0x06006EC2 RID: 28354 RVA: 0x002AD390 File Offset: 0x002AB590
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

	// Token: 0x06006EC3 RID: 28355 RVA: 0x002AD460 File Offset: 0x002AB660
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

	// Token: 0x06006EC4 RID: 28356 RVA: 0x002AD508 File Offset: 0x002AB708
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetIndex()
	{
		this.CurrentIndex = 0;
		if (this.selector != null)
		{
			this.selector.Value = (double)(this.autoCycle ? 0 : 1);
		}
	}

	// Token: 0x06006EC5 RID: 28357 RVA: 0x002AD531 File Offset: 0x002AB731
	public override void OnOpen()
	{
		base.OnOpen();
		this.autoCycle = this.AutoCycleTimePerEntry > 0f;
		this.resetIndex();
		base.RefreshBindings();
	}

	// Token: 0x040053B0 RID: 21424
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PlatformStoreLink = "openstore://";

	// Token: 0x040053B1 RID: 21425
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip browseSound;

	// Token: 0x040053B2 RID: 21426
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat selector;

	// Token: 0x040053B3 RID: 21427
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture[] bannerTextures;

	// Token: 0x040053B4 RID: 21428
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object newsLock = new object();

	// Token: 0x040053B5 RID: 21429
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x040053B6 RID: 21430
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NewsManager.NewsEntry> entries = new List<NewsManager.NewsEntry>();

	// Token: 0x040053B8 RID: 21432
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> newsProviders = new List<string>();

	// Token: 0x040053BB RID: 21435
	[PublicizedFrom(EAccessModifier.Private)]
	public bool autoCycle;

	// Token: 0x02000E11 RID: 3601
	// (Invoke) Token: 0x06006ECB RID: 28363
	public delegate void NewsEntryClickedDelegate(NewsManager.NewsEntry _newsEntry);
}
