using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E28 RID: 3624
[Preserve]
public class XUiC_LoadingScreen : XUiController
{
	// Token: 0x06006E69 RID: 28265 RVA: 0x002B0C7A File Offset: 0x002AEE7A
	[XuiXmlAttribute("browse_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.browseSound = _o;
		});
	}

	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x06006E6A RID: 28266 RVA: 0x002B0C94 File Offset: 0x002AEE94
	[XuiXmlBinding("background_texture")]
	public string CurrentBackground
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentBackground;
		}
	}

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x06006E6B RID: 28267 RVA: 0x002B0C9C File Offset: 0x002AEE9C
	[XuiXmlBinding("show_tips")]
	public bool ShowTips
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.showTips;
		}
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x06006E6C RID: 28268 RVA: 0x002B0CA4 File Offset: 0x002AEEA4
	[XuiXmlBinding("count")]
	public int TipCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_LoadingScreen.tips.Count;
		}
	}

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x06006E6D RID: 28269 RVA: 0x002B0CB0 File Offset: 0x002AEEB0
	[XuiXmlBinding("index")]
	public int TipIndex
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentTipIndex + 1;
		}
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x06006E6E RID: 28270 RVA: 0x002B0CBA File Offset: 0x002AEEBA
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

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x06006E6F RID: 28271 RVA: 0x002B0CEC File Offset: 0x002AEEEC
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

	// Token: 0x06006E70 RID: 28272 RVA: 0x002B0D14 File Offset: 0x002AEF14
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

	// Token: 0x06006E71 RID: 28273 RVA: 0x002B0D8C File Offset: 0x002AEF8C
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

	// Token: 0x06006E72 RID: 28274 RVA: 0x002B0E2C File Offset: 0x002AF02C
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

	// Token: 0x06006E73 RID: 28275 RVA: 0x002B0EB4 File Offset: 0x002AF0B4
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

	// Token: 0x06006E74 RID: 28276 RVA: 0x002B0F69 File Offset: 0x002AF169
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

	// Token: 0x06006E75 RID: 28277 RVA: 0x002B0FA8 File Offset: 0x002AF1A8
	public void SetTipsVisible(bool _visible)
	{
		if (this.showTips == _visible)
		{
			return;
		}
		this.showTips = _visible;
		this.IsDirty = true;
	}

	// Token: 0x06006E76 RID: 28278 RVA: 0x002B0FC2 File Offset: 0x002AF1C2
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

	// Token: 0x0400543C RID: 21564
	public static string ID = "";

	// Token: 0x0400543D RID: 21565
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentTipIndex = -1;

	// Token: 0x0400543E RID: 21566
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentBackground = "";

	// Token: 0x0400543F RID: 21567
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTips = true;

	// Token: 0x04005440 RID: 21568
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip browseSound;

	// Token: 0x04005441 RID: 21569
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture backgroundTextureView;

	// Token: 0x04005442 RID: 21570
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> backgrounds = new List<string>();

	// Token: 0x04005443 RID: 21571
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> tips = new List<string>();
}
