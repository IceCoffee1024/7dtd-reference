using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200118B RID: 4491
public class XUiV_Texture : XUiV_TextureBased
{
	// Token: 0x1700113F RID: 4415
	// (set) Token: 0x06008F0F RID: 36623 RVA: 0x0035FA5E File Offset: 0x0035DC5E
	public override Texture Texture
	{
		set
		{
			if (this.AutoUnload && base.Texture != null)
			{
				this.UnloadTexture();
			}
			base.Texture = value;
			if (value == null)
			{
				this.isExternalTexture = false;
			}
		}
	}

	// Token: 0x17001140 RID: 4416
	// (get) Token: 0x06008F10 RID: 36624 RVA: 0x0035FA93 File Offset: 0x0035DC93
	// (set) Token: 0x06008F11 RID: 36625 RVA: 0x0035FA9B File Offset: 0x0035DC9B
	[XuiXmlAttribute("textures", false)]
	public string[] TextureUris
	{
		get
		{
			return this.textureUris;
		}
		set
		{
			if (value == null || value.Length == 0)
			{
				this.textureUris = Array.Empty<string>();
				this.Texture = null;
				base.SetDirty();
				return;
			}
			this.textureUris = value;
			this.loadTexture(0);
		}
	}

	// Token: 0x17001141 RID: 4417
	// (get) Token: 0x06008F12 RID: 36626 RVA: 0x0035FACB File Offset: 0x0035DCCB
	// (set) Token: 0x06008F13 RID: 36627 RVA: 0x0035FAD5 File Offset: 0x0035DCD5
	[XuiXmlAttribute("texture", false)]
	public string Path
	{
		get
		{
			return this.textureUris[0];
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				this.TextureUris = null;
				return;
			}
			if (this.TextureUris.Length == 1 && this.TextureUris[0] == value)
			{
				return;
			}
			this.TextureUris = new string[] { value };
		}
	}

	// Token: 0x17001142 RID: 4418
	// (get) Token: 0x06008F14 RID: 36628 RVA: 0x0035FB13 File Offset: 0x0035DD13
	// (set) Token: 0x06008F15 RID: 36629 RVA: 0x0035FB1B File Offset: 0x0035DD1B
	[XuiXmlAttribute("autounload", false)]
	public bool AutoUnload
	{
		get
		{
			return this.autoUnload;
		}
		set
		{
			if (this.autoUnload == value)
			{
				return;
			}
			this.autoUnload = value;
		}
	}

	// Token: 0x06008F16 RID: 36630 RVA: 0x0035FB2E File Offset: 0x0035DD2E
	public XUiV_Texture(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008F17 RID: 36631 RVA: 0x0035FB44 File Offset: 0x0035DD44
	[PublicizedFrom(EAccessModifier.Private)]
	public void loadTexture(int _index = 0)
	{
		if (_index >= this.TextureUris.Length)
		{
			return;
		}
		string text = this.TextureUris[_index];
		try
		{
			string text2;
			if (ModManager.TryPatchModPathString(text, out text2))
			{
				this.<loadTexture>g__FetchWwwTexture|15_0(_index, "file://" + text2);
			}
			else if (text[0] == '@' && text[1] != ':')
			{
				string text3 = text.Substring(1);
				if (text3.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
				{
					string text4 = text3.Substring(5);
					if (text4[0] != '/' && text4[0] != '\\')
					{
						text3 = new Uri(((Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXServer) ? (Application.dataPath + "/../../") : (Application.dataPath + "/../")) + text4).AbsoluteUri;
					}
				}
				this.<loadTexture>g__FetchWwwTexture|15_0(_index, text3);
			}
			else
			{
				this.xui.LoadData<Texture>(text, delegate(Texture _o)
				{
					this.Texture = _o;
					this.isExternalTexture = false;
				});
			}
		}
		catch (Exception ex)
		{
			Log.Error("[XUi] Could not load texture: " + text + ", on " + base.GetXuiHierarchy());
			Log.Exception(ex);
		}
		base.SetDirty();
	}

	// Token: 0x06008F18 RID: 36632 RVA: 0x0035FC78 File Offset: 0x0035DE78
	public override void UnloadTexture()
	{
		Texture texture = this.Texture;
		base.UnloadTexture();
		this.TextureUris = null;
		if (texture == null)
		{
			return;
		}
		if (!this.isExternalTexture)
		{
			Resources.UnloadAsset(texture);
			return;
		}
		global::UnityEngine.Object.DestroyImmediate(texture);
	}

	// Token: 0x06008F1A RID: 36634 RVA: 0x0035FCC8 File Offset: 0x0035DEC8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <loadTexture>g__FetchWwwTexture|15_0(int _currentIndex, string _fetchUri)
	{
		_fetchUri = _fetchUri.Replace("#", "%23").Replace("+", "%2B");
		UnityWebRequest texture = UnityWebRequestTexture.GetTexture(_fetchUri);
		texture.SendWebRequest();
		ThreadManager.StartCoroutine(this.<loadTexture>g__WaitForWwwData|15_1(_currentIndex, texture, _fetchUri));
	}

	// Token: 0x06008F1B RID: 36635 RVA: 0x0035FD13 File Offset: 0x0035DF13
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator <loadTexture>g__WaitForWwwData|15_1(int _currentIndex, UnityWebRequest _www, string _fetchUri)
	{
		while (!_www.isDone)
		{
			yield return null;
		}
		if (_www.result != UnityWebRequest.Result.Success)
		{
			if (this.TextureUris.Length == 1)
			{
				Log.Warning(string.Concat(new string[] { "[XUi] Retrieving texture file from '", _fetchUri, "' failed (", _www.error, ")." }));
			}
			else if (_currentIndex + 1 < this.TextureUris.Length)
			{
				Log.Warning(string.Format("[XUi] Retrieving texture file {0} from '{1}' failed: ({2}). Trying next URI.", _currentIndex + 1, _fetchUri, _www.error));
				this.loadTexture(_currentIndex + 1);
			}
			else
			{
				Log.Warning(string.Format("[XUi] Retrieving texture file {0} from '{1}' failed: ({2}). No URIs successful.", _currentIndex + 1, _fetchUri, _www.error));
			}
			yield break;
		}
		Texture2D texture = ((DownloadHandlerTexture)_www.downloadHandler).texture;
		this.Texture = TextureUtils.CloneTexture(texture, false, false, true);
		global::UnityEngine.Object.DestroyImmediate(texture);
		_www.Dispose();
		this.isExternalTexture = true;
		yield break;
	}

	// Token: 0x040068F6 RID: 26870
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] textureUris = Array.Empty<string>();

	// Token: 0x040068F7 RID: 26871
	[PublicizedFrom(EAccessModifier.Private)]
	public bool autoUnload;

	// Token: 0x040068F8 RID: 26872
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isExternalTexture;
}
