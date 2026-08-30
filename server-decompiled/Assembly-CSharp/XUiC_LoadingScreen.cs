using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DF9 RID: 3577
[Preserve]
public class XUiC_LoadingScreen : XUiController
{
	// Token: 0x06006DAC RID: 28076 RVA: 0x002A8E52 File Offset: 0x002A7052
	[XuiXmlAttribute("browse_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.browseSound = _o;
		});
	}

	// Token: 0x17000C27 RID: 3111
	// (get) Token: 0x06006DAD RID: 28077 RVA: 0x002A8E6C File Offset: 0x002A706C
	[XuiXmlBinding("background_texture")]
	public string CurrentBackground
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentBackground;
		}
	}

	// Token: 0x17000C28 RID: 3112
	// (get) Token: 0x06006DAE RID: 28078 RVA: 0x002A8E74 File Offset: 0x002A7074
	[XuiXmlBinding("show_tips")]
	public bool ShowTips
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.showTips;
		}
	}

	// Token: 0x17000C29 RID: 3113
	// (get) Token: 0x06006DAF RID: 28079 RVA: 0x002A8E7C File Offset: 0x002A707C
	[XuiXmlBinding("count")]
	public int TipCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_LoadingScreen.tips.Count;
		}
	}

	// Token: 0x17000C2A RID: 3114
	// (get) Token: 0x06006DB0 RID: 28080 RVA: 0x002A8E88 File Offset: 0x002A7088
	[XuiXmlBinding("index")]
	public int TipIndex
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentTipIndex + 1;
		}
	}

	// Token: 0x17000C2B RID: 3115
	// (get) Token: 0x06006DB1 RID: 28081 RVA: 0x002A8E92 File Offset: 0x002A7092
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

	// Token: 0x17000C2C RID: 3116
	// (get) Token: 0x06006DB2 RID: 28082 RVA: 0x002A8EC4 File Offset: 0x002A70C4
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

	// Token: 0x06006DB3 RID: 28083 RVA: 0x002A8EEC File Offset: 0x002A70EC
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

	// Token: 0x06006DB4 RID: 28084 RVA: 0x002A8F64 File Offset: 0x002A7164
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

	// Token: 0x06006DB5 RID: 28085 RVA: 0x002A9004 File Offset: 0x002A7204
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

	// Token: 0x06006DB6 RID: 28086 RVA: 0x002A908C File Offset: 0x002A728C
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

	// Token: 0x06006DB7 RID: 28087 RVA: 0x002A9141 File Offset: 0x002A7341
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

	// Token: 0x06006DB8 RID: 28088 RVA: 0x002A9180 File Offset: 0x002A7380
	public void SetTipsVisible(bool _visible)
	{
		if (this.showTips == _visible)
		{
			return;
		}
		this.showTips = _visible;
		this.IsDirty = true;
	}

	// Token: 0x06006DB9 RID: 28089 RVA: 0x002A919A File Offset: 0x002A739A
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

	// Token: 0x0400533D RID: 21309
	public static string ID = "";

	// Token: 0x0400533E RID: 21310
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentTipIndex = -1;

	// Token: 0x0400533F RID: 21311
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentBackground = "";

	// Token: 0x04005340 RID: 21312
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTips = true;

	// Token: 0x04005341 RID: 21313
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip browseSound;

	// Token: 0x04005342 RID: 21314
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture backgroundTextureView;

	// Token: 0x04005343 RID: 21315
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> backgrounds = new List<string>();

	// Token: 0x04005344 RID: 21316
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> tips = new List<string>();
}
