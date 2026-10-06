using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E1D RID: 3613
[Preserve]
public class XUiC_LoadingScreen : XUiController
{
	// Token: 0x06006E44 RID: 28228 RVA: 0x002AF70E File Offset: 0x002AD90E
	[XuiXmlAttribute("browse_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.browseSound = _o;
		});
	}

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x06006E45 RID: 28229 RVA: 0x002AF728 File Offset: 0x002AD928
	[XuiXmlBinding("background_texture")]
	public string CurrentBackground
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentBackground;
		}
	}

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x06006E46 RID: 28230 RVA: 0x002AF730 File Offset: 0x002AD930
	[XuiXmlBinding("show_tips")]
	public bool ShowTips
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.showTips;
		}
	}

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x06006E47 RID: 28231 RVA: 0x002AF738 File Offset: 0x002AD938
	[XuiXmlBinding("count")]
	public int TipCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_LoadingScreen.tips.Count;
		}
	}

	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x06006E48 RID: 28232 RVA: 0x002AF744 File Offset: 0x002AD944
	[XuiXmlBinding("index")]
	public int TipIndex
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentTipIndex + 1;
		}
	}

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x06006E49 RID: 28233 RVA: 0x002AF74E File Offset: 0x002AD94E
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

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x06006E4A RID: 28234 RVA: 0x002AF780 File Offset: 0x002AD980
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

	// Token: 0x06006E4B RID: 28235 RVA: 0x002AF7A8 File Offset: 0x002AD9A8
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

	// Token: 0x06006E4C RID: 28236 RVA: 0x002AF820 File Offset: 0x002ADA20
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !XUiC_SpawnSelectionWindow.IsOpenInUI(this.xui.playerUI))
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

	// Token: 0x06006E4D RID: 28237 RVA: 0x002AF8C0 File Offset: 0x002ADAC0
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

	// Token: 0x06006E4E RID: 28238 RVA: 0x002AF948 File Offset: 0x002ADB48
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

	// Token: 0x06006E4F RID: 28239 RVA: 0x002AF9FD File Offset: 0x002ADBFD
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

	// Token: 0x06006E50 RID: 28240 RVA: 0x002AFA3C File Offset: 0x002ADC3C
	public void SetTipsVisible(bool _visible)
	{
		if (this.showTips == _visible)
		{
			return;
		}
		this.showTips = _visible;
		this.IsDirty = true;
	}

	// Token: 0x06006E51 RID: 28241 RVA: 0x002AFA56 File Offset: 0x002ADC56
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

	// Token: 0x04005417 RID: 21527
	public static string ID = "";

	// Token: 0x04005418 RID: 21528
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentTipIndex = -1;

	// Token: 0x04005419 RID: 21529
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentBackground = "";

	// Token: 0x0400541A RID: 21530
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTips = true;

	// Token: 0x0400541B RID: 21531
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip browseSound;

	// Token: 0x0400541C RID: 21532
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture backgroundTextureView;

	// Token: 0x0400541D RID: 21533
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> backgrounds = new List<string>();

	// Token: 0x0400541E RID: 21534
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> tips = new List<string>();
}
