using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E25 RID: 3621
[Preserve]
public class XUiC_NewsWindow : XUiController
{
	// Token: 0x17000C72 RID: 3186
	// (get) Token: 0x06006F3F RID: 28479 RVA: 0x002B01AB File Offset: 0x002AE3AB
	// (set) Token: 0x06006F40 RID: 28480 RVA: 0x002B01B3 File Offset: 0x002AE3B3
	[XuiXmlBinding("has_news_provider")]
	public bool HasProviders
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x06006F41 RID: 28481 RVA: 0x002B01BC File Offset: 0x002AE3BC
	// (set) Token: 0x06006F42 RID: 28482 RVA: 0x002B01C4 File Offset: 0x002AE3C4
	[XuiXmlAttribute("max_entries", false)]
	public int MaxEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = int.MaxValue;

	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x06006F43 RID: 28483 RVA: 0x002B01CD File Offset: 0x002AE3CD
	// (set) Token: 0x06006F44 RID: 28484 RVA: 0x002B01D5 File Offset: 0x002AE3D5
	[XuiXmlAttribute("auto_cycle_time_per_entry", false)]
	public float AutoCycleTimePerEntry
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1400009E RID: 158
	// (add) Token: 0x06006F45 RID: 28485 RVA: 0x002B01E0 File Offset: 0x002AE3E0
	// (remove) Token: 0x06006F46 RID: 28486 RVA: 0x002B0218 File Offset: 0x002AE418
	public event XUiC_NewsWindow.NewsEntryClickedDelegate NewsEntryClicked;

	// Token: 0x17000C75 RID: 3189
	// (get) Token: 0x06006F47 RID: 28487 RVA: 0x002B024D File Offset: 0x002AE44D
	// (set) Token: 0x06006F48 RID: 28488 RVA: 0x002B0258 File Offset: 0x002AE458
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

	// Token: 0x17000C76 RID: 3190
	// (get) Token: 0x06006F49 RID: 28489 RVA: 0x002B02AC File Offset: 0x002AE4AC
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

	// Token: 0x17000C77 RID: 3191
	// (get) Token: 0x06006F4A RID: 28490 RVA: 0x002B031C File Offset: 0x002AE51C
	public NewsManager.NewsEntry CurrentEntryOrEmpty
	{
		get
		{
			return this.CurrentEntry ?? NewsManager.EmptyEntry;
		}
	}

	// Token: 0x06006F4B RID: 28491 RVA: 0x002B032D File Offset: 0x002AE52D
	[XuiXmlAttribute("browse_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.browseSound = _o;
		});
	}

	// Token: 0x06006F4C RID: 28492 RVA: 0x002B0348 File Offset: 0x002AE548
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

	// Token: 0x06006F4D RID: 28493 RVA: 0x002B0395 File Offset: 0x002AE595
	[XuiXmlAttribute("additional_sources", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribAdditionalSources(string _value)
	{
		this.parseSourcesString(_value);
	}

	// Token: 0x06006F4E RID: 28494 RVA: 0x002B0395 File Offset: 0x002AE595
	[XuiXmlAttribute("sources", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribSources(string _value)
	{
		this.parseSourcesString(_value);
	}

	// Token: 0x17000C78 RID: 3192
	// (get) Token: 0x06006F4F RID: 28495 RVA: 0x002B039E File Offset: 0x002AE59E
	[XuiXmlBinding("news_count")]
	public int NewsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.Min(this.entries.Count, this.MaxEntries);
		}
	}

	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x06006F50 RID: 28496 RVA: 0x002B03B6 File Offset: 0x002AE5B6
	[XuiXmlBinding("is_custom")]
	public bool NewsIsCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.IsCustom;
		}
	}

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x06006F51 RID: 28497 RVA: 0x002B03C3 File Offset: 0x002AE5C3
	[XuiXmlBinding("source")]
	public string NewsSource
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.CustomListName ?? "";
		}
	}

	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x06006F52 RID: 28498 RVA: 0x002B03D9 File Offset: 0x002AE5D9
	[XuiXmlBinding("has_text")]
	public bool NewsHasText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(this.CurrentEntryOrEmpty.Text);
		}
	}

	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x06006F53 RID: 28499 RVA: 0x002B03EE File Offset: 0x002AE5EE
	[XuiXmlBinding("headline")]
	public string NewsHeadline
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Headline ?? "";
		}
	}

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x06006F54 RID: 28500 RVA: 0x002B0404 File Offset: 0x002AE604
	[XuiXmlBinding("headline2")]
	public string NewsHeadline2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Headline2 ?? "";
		}
	}

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x06006F55 RID: 28501 RVA: 0x002B041C File Offset: 0x002AE61C
	[XuiXmlBinding("date")]
	public string NewsDate
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Date.ToString("yyyy-MM-dd");
		}
	}

	// Token: 0x17000C7F RID: 3199
	// (get) Token: 0x06006F56 RID: 28502 RVA: 0x002B0441 File Offset: 0x002AE641
	[XuiXmlBinding("age")]
	public string NewsAge
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ValueDisplayFormatters.DateAge(this.CurrentEntryOrEmpty.Date);
		}
	}

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x06006F57 RID: 28503 RVA: 0x002B0453 File Offset: 0x002AE653
	[XuiXmlBinding("text")]
	public string NewsText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Text ?? "";
		}
	}

	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x06006F58 RID: 28504 RVA: 0x002B0469 File Offset: 0x002AE669
	[XuiXmlBinding("has_link")]
	public bool NewsHasLink
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(this.CurrentEntryOrEmpty.Url);
		}
	}

	// Token: 0x17000C82 RID: 3202
	// (get) Token: 0x06006F59 RID: 28505 RVA: 0x002B047E File Offset: 0x002AE67E
	[XuiXmlBinding("link")]
	public string NewsLink
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Url ?? "";
		}
	}

	// Token: 0x17000C83 RID: 3203
	// (get) Token: 0x06006F5A RID: 28506 RVA: 0x002B0494 File Offset: 0x002AE694
	[XuiXmlBinding("has_younger")]
	public bool HasYounger
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.entries.Count > 0 && this.CurrentIndex > 0;
		}
	}

	// Token: 0x17000C84 RID: 3204
	// (get) Token: 0x06006F5B RID: 28507 RVA: 0x002B04AF File Offset: 0x002AE6AF
	[XuiXmlBinding("has_older")]
	public bool HasOlder
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.entries.Count > 0 && this.CurrentIndex < this.entries.Count - 1;
		}
	}

	// Token: 0x06006F5C RID: 28508 RVA: 0x002B04D8 File Offset: 0x002AE6D8
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

	// Token: 0x06006F5D RID: 28509 RVA: 0x002B06AF File Offset: 0x002AE8AF
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

	// Token: 0x06006F5E RID: 28510 RVA: 0x002B06D4 File Offset: 0x002AE8D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Selector_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.CurrentIndex = Mathf.CeilToInt((float)_newValue) - 1;
		this.selector.Value = (double)(this.CurrentIndex + 1);
		this.autoCycle = false;
	}

	// Token: 0x06006F5F RID: 28511 RVA: 0x002B0700 File Offset: 0x002AE900
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

	// Token: 0x06006F60 RID: 28512 RVA: 0x002B07A4 File Offset: 0x002AE9A4
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

	// Token: 0x06006F61 RID: 28513 RVA: 0x002B0818 File Offset: 0x002AEA18
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

	// Token: 0x06006F62 RID: 28514 RVA: 0x002B08A4 File Offset: 0x002AEAA4
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

	// Token: 0x06006F63 RID: 28515 RVA: 0x002B0974 File Offset: 0x002AEB74
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

	// Token: 0x06006F64 RID: 28516 RVA: 0x002B0A1C File Offset: 0x002AEC1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetIndex()
	{
		this.CurrentIndex = 0;
		if (this.selector != null)
		{
			this.selector.Value = (double)(this.autoCycle ? 0 : 1);
		}
	}

	// Token: 0x06006F65 RID: 28517 RVA: 0x002B0A45 File Offset: 0x002AEC45
	public override void OnOpen()
	{
		base.OnOpen();
		this.autoCycle = this.AutoCycleTimePerEntry > 0f;
		this.resetIndex();
		base.RefreshBindings();
	}

	// Token: 0x04005454 RID: 21588
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PlatformStoreLink = "openstore://";

	// Token: 0x04005455 RID: 21589
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip browseSound;

	// Token: 0x04005456 RID: 21590
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat selector;

	// Token: 0x04005457 RID: 21591
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture[] bannerTextures;

	// Token: 0x04005458 RID: 21592
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object newsLock = new object();

	// Token: 0x04005459 RID: 21593
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x0400545A RID: 21594
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NewsManager.NewsEntry> entries = new List<NewsManager.NewsEntry>();

	// Token: 0x0400545C RID: 21596
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> newsProviders = new List<string>();

	// Token: 0x0400545F RID: 21599
	[PublicizedFrom(EAccessModifier.Private)]
	public bool autoCycle;

	// Token: 0x02000E26 RID: 3622
	// (Invoke) Token: 0x06006F6B RID: 28523
	public delegate void NewsEntryClickedDelegate(NewsManager.NewsEntry _newsEntry);
}
