using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200114A RID: 4426
public class XUiV_Texture : XUiV_TextureBased
{
	// Token: 0x170010B7 RID: 4279
	// (set) Token: 0x06008D3E RID: 36158 RVA: 0x0035498E File Offset: 0x00352B8E
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

	// Token: 0x170010B8 RID: 4280
	// (get) Token: 0x06008D3F RID: 36159 RVA: 0x003549C3 File Offset: 0x00352BC3
	// (set) Token: 0x06008D40 RID: 36160 RVA: 0x003549CB File Offset: 0x00352BCB
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

	// Token: 0x170010B9 RID: 4281
	// (get) Token: 0x06008D41 RID: 36161 RVA: 0x003549FB File Offset: 0x00352BFB
	// (set) Token: 0x06008D42 RID: 36162 RVA: 0x00354A05 File Offset: 0x00352C05
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

	// Token: 0x170010BA RID: 4282
	// (get) Token: 0x06008D43 RID: 36163 RVA: 0x00354A43 File Offset: 0x00352C43
	// (set) Token: 0x06008D44 RID: 36164 RVA: 0x00354A4B File Offset: 0x00352C4B
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

	// Token: 0x06008D45 RID: 36165 RVA: 0x00354A5E File Offset: 0x00352C5E
	public XUiV_Texture(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D46 RID: 36166 RVA: 0x00354A74 File Offset: 0x00352C74
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

	// Token: 0x06008D47 RID: 36167 RVA: 0x00354BA8 File Offset: 0x00352DA8
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

	// Token: 0x06008D49 RID: 36169 RVA: 0x00354BF8 File Offset: 0x00352DF8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <loadTexture>g__FetchWwwTexture|15_0(int _currentIndex, string _fetchUri)
	{
		_fetchUri = _fetchUri.Replace("#", "%23").Replace("+", "%2B");
		UnityWebRequest texture = UnityWebRequestTexture.GetTexture(_fetchUri);
		texture.SendWebRequest();
		ThreadManager.StartCoroutine(this.<loadTexture>g__WaitForWwwData|15_1(_currentIndex, texture, _fetchUri));
	}

	// Token: 0x06008D4A RID: 36170 RVA: 0x00354C43 File Offset: 0x00352E43
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

	// Token: 0x04006787 RID: 26503
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] textureUris = Array.Empty<string>();

	// Token: 0x04006788 RID: 26504
	[PublicizedFrom(EAccessModifier.Private)]
	public bool autoUnload;

	// Token: 0x04006789 RID: 26505
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isExternalTexture;
}
