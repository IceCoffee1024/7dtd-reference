using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E34 RID: 3636
[Preserve]
public class XUiC_NewsWindow : XUiController
{
	// Token: 0x17000C8D RID: 3213
	// (get) Token: 0x06006F37 RID: 28471 RVA: 0x002B3553 File Offset: 0x002B1753
	// (set) Token: 0x06006F38 RID: 28472 RVA: 0x002B355B File Offset: 0x002B175B
	[XuiXmlBinding("has_news_provider")]
	public bool HasProviders
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C8E RID: 3214
	// (get) Token: 0x06006F39 RID: 28473 RVA: 0x002B3564 File Offset: 0x002B1764
	// (set) Token: 0x06006F3A RID: 28474 RVA: 0x002B356C File Offset: 0x002B176C
	[XuiXmlAttribute("max_entries", false)]
	public int MaxEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = int.MaxValue;

	// Token: 0x17000C8F RID: 3215
	// (get) Token: 0x06006F3B RID: 28475 RVA: 0x002B3575 File Offset: 0x002B1775
	// (set) Token: 0x06006F3C RID: 28476 RVA: 0x002B357D File Offset: 0x002B177D
	[XuiXmlAttribute("auto_cycle_time_per_entry", false)]
	public float AutoCycleTimePerEntry
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x06006F3D RID: 28477 RVA: 0x002B3588 File Offset: 0x002B1788
	// (remove) Token: 0x06006F3E RID: 28478 RVA: 0x002B35C0 File Offset: 0x002B17C0
	public event XUiC_NewsWindow.NewsEntryClickedDelegate NewsEntryClicked;

	// Token: 0x17000C90 RID: 3216
	// (get) Token: 0x06006F3F RID: 28479 RVA: 0x002B35F5 File Offset: 0x002B17F5
	// (set) Token: 0x06006F40 RID: 28480 RVA: 0x002B3600 File Offset: 0x002B1800
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

	// Token: 0x17000C91 RID: 3217
	// (get) Token: 0x06006F41 RID: 28481 RVA: 0x002B365C File Offset: 0x002B185C
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

	// Token: 0x17000C92 RID: 3218
	// (get) Token: 0x06006F42 RID: 28482 RVA: 0x002B36CC File Offset: 0x002B18CC
	public NewsManager.NewsEntry CurrentEntryOrEmpty
	{
		get
		{
			return this.CurrentEntry ?? NewsManager.EmptyEntry;
		}
	}

	// Token: 0x06006F43 RID: 28483 RVA: 0x002B36DD File Offset: 0x002B18DD
	[XuiXmlAttribute("browse_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.browseSound = _o;
		});
	}

	// Token: 0x06006F44 RID: 28484 RVA: 0x002B36F8 File Offset: 0x002B18F8
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

	// Token: 0x06006F45 RID: 28485 RVA: 0x002B3745 File Offset: 0x002B1945
	[XuiXmlAttribute("additional_sources", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribAdditionalSources(string _value)
	{
		this.parseSourcesString(_value);
	}

	// Token: 0x06006F46 RID: 28486 RVA: 0x002B3745 File Offset: 0x002B1945
	[XuiXmlAttribute("sources", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribSources(string _value)
	{
		this.parseSourcesString(_value);
	}

	// Token: 0x17000C93 RID: 3219
	// (get) Token: 0x06006F47 RID: 28487 RVA: 0x002B374E File Offset: 0x002B194E
	[XuiXmlBinding("news_count")]
	public int NewsCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.Min(this.entries.Count, this.MaxEntries);
		}
	}

	// Token: 0x17000C94 RID: 3220
	// (get) Token: 0x06006F48 RID: 28488 RVA: 0x002B3766 File Offset: 0x002B1966
	[XuiXmlBinding("is_custom")]
	public bool NewsIsCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.IsCustom;
		}
	}

	// Token: 0x17000C95 RID: 3221
	// (get) Token: 0x06006F49 RID: 28489 RVA: 0x002B3773 File Offset: 0x002B1973
	[XuiXmlBinding("source")]
	public string NewsSource
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.CustomListName ?? "";
		}
	}

	// Token: 0x17000C96 RID: 3222
	// (get) Token: 0x06006F4A RID: 28490 RVA: 0x002B3789 File Offset: 0x002B1989
	[XuiXmlBinding("has_text")]
	public bool NewsHasText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(this.CurrentEntryOrEmpty.Text);
		}
	}

	// Token: 0x17000C97 RID: 3223
	// (get) Token: 0x06006F4B RID: 28491 RVA: 0x002B379E File Offset: 0x002B199E
	[XuiXmlBinding("headline")]
	public string NewsHeadline
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Headline ?? "";
		}
	}

	// Token: 0x17000C98 RID: 3224
	// (get) Token: 0x06006F4C RID: 28492 RVA: 0x002B37B4 File Offset: 0x002B19B4
	[XuiXmlBinding("headline2")]
	public string NewsHeadline2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Headline2 ?? "";
		}
	}

	// Token: 0x17000C99 RID: 3225
	// (get) Token: 0x06006F4D RID: 28493 RVA: 0x002B37CC File Offset: 0x002B19CC
	[XuiXmlBinding("date")]
	public string NewsDate
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Date.ToString("yyyy-MM-dd");
		}
	}

	// Token: 0x17000C9A RID: 3226
	// (get) Token: 0x06006F4E RID: 28494 RVA: 0x002B37F1 File Offset: 0x002B19F1
	[XuiXmlBinding("age")]
	public string NewsAge
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ValueDisplayFormatters.DateAge(this.CurrentEntryOrEmpty.Date);
		}
	}

	// Token: 0x17000C9B RID: 3227
	// (get) Token: 0x06006F4F RID: 28495 RVA: 0x002B3803 File Offset: 0x002B1A03
	[XuiXmlBinding("text")]
	public string NewsText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Text ?? "";
		}
	}

	// Token: 0x17000C9C RID: 3228
	// (get) Token: 0x06006F50 RID: 28496 RVA: 0x002B3819 File Offset: 0x002B1A19
	[XuiXmlBinding("has_link")]
	public bool NewsHasLink
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(this.CurrentEntryOrEmpty.Url);
		}
	}

	// Token: 0x17000C9D RID: 3229
	// (get) Token: 0x06006F51 RID: 28497 RVA: 0x002B382E File Offset: 0x002B1A2E
	[XuiXmlBinding("link")]
	public string NewsLink
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentEntryOrEmpty.Url ?? "";
		}
	}

	// Token: 0x17000C9E RID: 3230
	// (get) Token: 0x06006F52 RID: 28498 RVA: 0x002B3844 File Offset: 0x002B1A44
	[XuiXmlBinding("has_younger")]
	public bool HasYounger
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.entries.Count > 0 && this.CurrentIndex > 0;
		}
	}

	// Token: 0x17000C9F RID: 3231
	// (get) Token: 0x06006F53 RID: 28499 RVA: 0x002B385F File Offset: 0x002B1A5F
	[XuiXmlBinding("has_older")]
	public bool HasOlder
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.entries.Count > 0 && this.CurrentIndex < this.entries.Count - 1;
		}
	}

	// Token: 0x06006F54 RID: 28500 RVA: 0x002B3888 File Offset: 0x002B1A88
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

	// Token: 0x06006F55 RID: 28501 RVA: 0x002B3A5F File Offset: 0x002B1C5F
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

	// Token: 0x06006F56 RID: 28502 RVA: 0x002B3A84 File Offset: 0x002B1C84
	[PublicizedFrom(EAccessModifier.Private)]
	public void Selector_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.CurrentIndex = Mathf.CeilToInt((float)_newValue) - 1;
		this.selector.Value = (double)(this.CurrentIndex + 1);
		this.autoCycle = false;
	}

	// Token: 0x06006F57 RID: 28503 RVA: 0x002B3AB0 File Offset: 0x002B1CB0
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

	// Token: 0x06006F58 RID: 28504 RVA: 0x002B3B54 File Offset: 0x002B1D54
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

	// Token: 0x06006F59 RID: 28505 RVA: 0x002B3BC8 File Offset: 0x002B1DC8
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

	// Token: 0x06006F5A RID: 28506 RVA: 0x002B3C54 File Offset: 0x002B1E54
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

	// Token: 0x06006F5B RID: 28507 RVA: 0x002B3D24 File Offset: 0x002B1F24
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

	// Token: 0x06006F5C RID: 28508 RVA: 0x002B3DCC File Offset: 0x002B1FCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetIndex()
	{
		this.CurrentIndex = 0;
		if (this.selector != null)
		{
			this.selector.Value = (double)(this.autoCycle ? 0 : 1);
		}
	}

	// Token: 0x06006F5D RID: 28509 RVA: 0x002B3DF5 File Offset: 0x002B1FF5
	public override void OnOpen()
	{
		base.OnOpen();
		this.autoCycle = this.AutoCycleTimePerEntry > 0f;
		this.resetIndex();
		base.RefreshBindings();
	}

	// Token: 0x0400548B RID: 21643
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PlatformStoreLink = "openstore://";

	// Token: 0x0400548C RID: 21644
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip browseSound;

	// Token: 0x0400548D RID: 21645
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat selector;

	// Token: 0x0400548E RID: 21646
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture[] bannerTextures;

	// Token: 0x0400548F RID: 21647
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object newsLock = new object();

	// Token: 0x04005490 RID: 21648
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x04005491 RID: 21649
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NewsManager.NewsEntry> entries = new List<NewsManager.NewsEntry>();

	// Token: 0x04005493 RID: 21651
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> newsProviders = new List<string>();

	// Token: 0x04005496 RID: 21654
	[PublicizedFrom(EAccessModifier.Private)]
	public bool autoCycle;

	// Token: 0x02000E35 RID: 3637
	// (Invoke) Token: 0x06006F63 RID: 28515
	public delegate void NewsEntryClickedDelegate(NewsManager.NewsEntry _newsEntry);
}
