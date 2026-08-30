using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E0E RID: 3598
[Preserve]
public class XUiC_LoadingScreen : XUiController
{
	// Token: 0x06006E4C RID: 28236 RVA: 0x002AC366 File Offset: 0x002AA566
	[XuiXmlAttribute("browse_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.browseSound = _o;
		});
	}

	// Token: 0x17000C40 RID: 3136
	// (get) Token: 0x06006E4D RID: 28237 RVA: 0x002AC380 File Offset: 0x002AA580
	[XuiXmlBinding("background_texture")]
	public string CurrentBackground
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentBackground;
		}
	}

	// Token: 0x17000C41 RID: 3137
	// (get) Token: 0x06006E4E RID: 28238 RVA: 0x002AC388 File Offset: 0x002AA588
	[XuiXmlBinding("show_tips")]
	public bool ShowTips
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.showTips;
		}
	}

	// Token: 0x17000C42 RID: 3138
	// (get) Token: 0x06006E4F RID: 28239 RVA: 0x002AC390 File Offset: 0x002AA590
	[XuiXmlBinding("count")]
	public int TipCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_LoadingScreen.tips.Count;
		}
	}

	// Token: 0x17000C43 RID: 3139
	// (get) Token: 0x06006E50 RID: 28240 RVA: 0x002AC39C File Offset: 0x002AA59C
	[XuiXmlBinding("index")]
	public int TipIndex
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentTipIndex + 1;
		}
	}

	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x06006E51 RID: 28241 RVA: 0x002AC3A6 File Offset: 0x002AA5A6
	[XuiXmlBinding("title")]
	public string TipTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.currentTipIndex >= 0)
			{
				return Localization.Get(XUiC_LoadingScreen.tips[this.currentTipIndex] + "_title", false, null);
			}
			return "";
		}
	}

	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x06006E52 RID: 28242 RVA: 0x002AC3D8 File Offset: 0x002AA5D8
	[XuiXmlBinding("text")]
	public string TipText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.currentTipIndex >= 0)
			{
				return Localization.Get(XUiC_LoadingScreen.tips[this.currentTipIndex], false, null);
			}
			return "";
		}
	}

	// Token: 0x06006E53 RID: 28243 RVA: 0x002AC400 File Offset: 0x002AA600
	public override void Init()
	{
		base.Init();
		XUiC_LoadingScreen.ID = base.WindowGroup.Id;
		XUiController childById = base.GetChildById("loading_image");
		if (childById != null)
		{
			this.backgroundTextureView = childById.ViewComponent as XUiV_Texture;
		}
		XUiController childById2 = base.GetChildById("pnlBlack");
		if (childById2 != null)
		{
			childById2.OnPress += delegate(XUiController _, int _)
			{
				this.cycle(1);
			};
			childById2.OnRightPress += delegate(XUiController _, int _)
			{
				this.cycle(-1);
			};
		}
	}

	// Token: 0x06006E54 RID: 28244 RVA: 0x002AC478 File Offset: 0x002AA678
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !XUiC_SpawnSelectionWindow.IsInSpawnSelection(this.xui.playerUI))
		{
			if (this.xui.playerUI.playerInput.PermanentActions.PageTipsForward.WasPressed)
			{
				this.cycle(1);
			}
			else if (this.xui.playerUI.playerInput.PermanentActions.PageTipsBack.WasPressed)
			{
				this.cycle(-1);
			}
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06006E55 RID: 28245 RVA: 0x002AC518 File Offset: 0x002AA718
	[PublicizedFrom(EAccessModifier.Private)]
	public void cycle(int _increment)
	{
		if (!this.showTips || XUiC_VideoPlayer.IsVideoPlaying)
		{
			return;
		}
		this.currentTipIndex += _increment;
		if (this.currentTipIndex >= XUiC_LoadingScreen.tips.Count)
		{
			this.currentTipIndex = 0;
		}
		else if (this.currentTipIndex < 0)
		{
			this.currentTipIndex = XUiC_LoadingScreen.tips.Count - 1;
		}
		if (this.browseSound != null)
		{
			Manager.PlayXUiSound(this.browseSound, 1f);
		}
		this.IsDirty = true;
	}

	// Token: 0x06006E56 RID: 28246 RVA: 0x002AC5A0 File Offset: 0x002AA7A0
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)base.ViewComponent).ForceVisible(1f);
		if (XUiC_LoadingScreen.backgrounds.Count > 0)
		{
			global::UnityEngine.Random.InitState(Time.frameCount);
			this.currentBackground = XUiC_LoadingScreen.backgrounds[global::UnityEngine.Random.Range(0, XUiC_LoadingScreen.backgrounds.Count)];
		}
		this.showTips = true;
		this.currentTipIndex = GamePrefs.GetInt(EnumGamePrefs.LastLoadingTipRead) + 1;
		if (this.currentTipIndex >= XUiC_LoadingScreen.tips.Count)
		{
			this.currentTipIndex = 0;
		}
		this.currentTipIndex = Mathf.Clamp(this.currentTipIndex, 0, XUiC_LoadingScreen.tips.Count - 1);
		base.RefreshBindings();
	}

	// Token: 0x06006E57 RID: 28247 RVA: 0x002AC655 File Offset: 0x002AA855
	public override void OnClose()
	{
		base.OnClose();
		GamePrefs.Set(EnumGamePrefs.LastLoadingTipRead, this.currentTipIndex);
		XUiV_Texture xuiV_Texture = this.backgroundTextureView;
		if (xuiV_Texture != null)
		{
			xuiV_Texture.UnloadTexture();
		}
		this.xui.playerUI.CursorController.SetCursorHidden(false);
	}

	// Token: 0x06006E58 RID: 28248 RVA: 0x002AC694 File Offset: 0x002AA894
	public void SetTipsVisible(bool _visible)
	{
		if (this.showTips == _visible)
		{
			return;
		}
		this.showTips = _visible;
		this.IsDirty = true;
	}

	// Token: 0x06006E59 RID: 28249 RVA: 0x002AC6AE File Offset: 0x002AA8AE
	public static IEnumerator LoadXml(XmlFile _xmlFile)
	{
		XContainer root = _xmlFile.XmlDoc.Root;
		XUiC_LoadingScreen.backgrounds.Clear();
		XUiC_LoadingScreen.tips.Clear();
		using (IEnumerator<XElement> enumerator = root.Elements().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XElement xelement = enumerator.Current;
				if (xelement.Name == "backgrounds")
				{
					using (IEnumerator<XElement> enumerator2 = xelement.Elements("tex").GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							XElement xelement2 = enumerator2.Current;
							if (!xelement2.HasAttribute("file"))
							{
								Log.Warning("Backgrounds entry is missing file attribute, skipping.");
							}
							else
							{
								XUiC_LoadingScreen.backgrounds.Add(xelement2.GetAttribute("file"));
							}
						}
						continue;
					}
				}
				if (xelement.Name == "tips")
				{
					foreach (XElement xelement3 in xelement.Elements("tip"))
					{
						if (!xelement3.HasAttribute("key"))
						{
							Log.Warning("Loading tips entry is missing file attribute, skipping.");
						}
						else
						{
							XUiC_LoadingScreen.tips.Add(xelement3.GetAttribute("key"));
						}
					}
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x040053E0 RID: 21472
	public static string ID = "";

	// Token: 0x040053E1 RID: 21473
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentTipIndex = -1;

	// Token: 0x040053E2 RID: 21474
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentBackground = "";

	// Token: 0x040053E3 RID: 21475
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTips = true;

	// Token: 0x040053E4 RID: 21476
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip browseSound;

	// Token: 0x040053E5 RID: 21477
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture backgroundTextureView;

	// Token: 0x040053E6 RID: 21478
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> backgrounds = new List<string>();

	// Token: 0x040053E7 RID: 21479
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> tips = new List<string>();
}
