using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200117B RID: 4475
public class XUiV_Texture : XUiV_TextureBased
{
	// Token: 0x17001134 RID: 4404
	// (set) Token: 0x06008EBB RID: 36539 RVA: 0x0035D96E File Offset: 0x0035BB6E
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

	// Token: 0x17001135 RID: 4405
	// (get) Token: 0x06008EBC RID: 36540 RVA: 0x0035D9A3 File Offset: 0x0035BBA3
	// (set) Token: 0x06008EBD RID: 36541 RVA: 0x0035D9AB File Offset: 0x0035BBAB
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

	// Token: 0x17001136 RID: 4406
	// (get) Token: 0x06008EBE RID: 36542 RVA: 0x0035D9DB File Offset: 0x0035BBDB
	// (set) Token: 0x06008EBF RID: 36543 RVA: 0x0035D9E5 File Offset: 0x0035BBE5
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

	// Token: 0x17001137 RID: 4407
	// (get) Token: 0x06008EC0 RID: 36544 RVA: 0x0035DA23 File Offset: 0x0035BC23
	// (set) Token: 0x06008EC1 RID: 36545 RVA: 0x0035DA2B File Offset: 0x0035BC2B
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

	// Token: 0x06008EC2 RID: 36546 RVA: 0x0035DA3E File Offset: 0x0035BC3E
	public XUiV_Texture(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008EC3 RID: 36547 RVA: 0x0035DA54 File Offset: 0x0035BC54
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

	// Token: 0x06008EC4 RID: 36548 RVA: 0x0035DB88 File Offset: 0x0035BD88
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

	// Token: 0x06008EC6 RID: 36550 RVA: 0x0035DBD8 File Offset: 0x0035BDD8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <loadTexture>g__FetchWwwTexture|15_0(int _currentIndex, string _fetchUri)
	{
		_fetchUri = _fetchUri.Replace("#", "%23").Replace("+", "%2B");
		UnityWebRequest texture = UnityWebRequestTexture.GetTexture(_fetchUri);
		texture.SendWebRequest();
		ThreadManager.StartCoroutine(this.<loadTexture>g__WaitForWwwData|15_1(_currentIndex, texture, _fetchUri));
	}

	// Token: 0x06008EC7 RID: 36551 RVA: 0x0035DC23 File Offset: 0x0035BE23
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

	// Token: 0x040068B6 RID: 26806
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] textureUris = Array.Empty<string>();

	// Token: 0x040068B7 RID: 26807
	[PublicizedFrom(EAccessModifier.Private)]
	public bool autoUnload;

	// Token: 0x040068B8 RID: 26808
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isExternalTexture;
}
