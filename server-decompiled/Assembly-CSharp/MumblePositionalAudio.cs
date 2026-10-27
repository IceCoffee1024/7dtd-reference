using System;
using mumblelib;
using UnityEngine;

// Token: 0x0200147F RID: 5247
public class MumblePositionalAudio : SingletonMonoBehaviour<MumblePositionalAudio>
{
	// Token: 0x0600A43A RID: 42042 RVA: 0x003D7054 File Offset: 0x003D5254
	public static void Init()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance != null)
		{
			return;
		}
		new GameObject("MumbleLink").AddComponent<MumblePositionalAudio>().IsPersistant = true;
	}

	// Token: 0x0600A43B RID: 42043 RVA: 0x003D7081 File Offset: 0x003D5281
	public static void Destroy()
	{
		if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance == null)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(SingletonMonoBehaviour<MumblePositionalAudio>.Instance.gameObject);
	}

	// Token: 0x0600A43C RID: 42044 RVA: 0x003D70A0 File Offset: 0x003D52A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setCommonValues()
	{
		if (this.mumbleLink == null || this.player == null)
		{
			return;
		}
		this.mumbleLink.Name = "7 Days To Die";
		this.mumbleLink.Description = "7 Days To Die Positional Audio";
		this.mumbleLink.UIVersion = 2U;
		string text = this.player.entityId.ToString();
		Log.Out("[Mumble] Setting Mumble ID to " + text);
		this.mumbleLink.Identity = text;
		string text2 = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient ? GamePrefs.GetString(EnumGamePrefs.GameGuidClient) : this.player.world.Guid);
		Log.Out("[Mumble] Setting context to " + text2);
		this.mumbleLink.Context = text2;
	}

	// Token: 0x0600A43D RID: 42045 RVA: 0x003D7162 File Offset: 0x003D5362
	[PublicizedFrom(EAccessModifier.Private)]
	public void initShm()
	{
		this.mumbleLink = LinkFileManager.Open();
		this.setCommonValues();
		Log.Out("[Mumble] Shared Memory initialized");
	}

	// Token: 0x0600A43E RID: 42046 RVA: 0x003D717F File Offset: 0x003D537F
	public void ReinitShm()
	{
		if (this.mumbleLink == null)
		{
			this.initShm();
			return;
		}
		this.setCommonValues();
	}

	// Token: 0x0600A43F RID: 42047 RVA: 0x003D7196 File Offset: 0x003D5396
	public void SetPlayer(EntityPlayerLocal player)
	{
		this.player = player;
	}

	// Token: 0x0600A440 RID: 42048 RVA: 0x003D71A0 File Offset: 0x003D53A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.player == null)
		{
			if (this.mumbleLink != null)
			{
				if (!this.contextCleared)
				{
					this.contextCleared = true;
					this.mumbleLink.Context = "";
					this.mumbleLink.Tick();
					return;
				}
				this.mumbleLink.Dispose();
				this.mumbleLink = null;
			}
			return;
		}
		if (this.mumbleLink == null)
		{
			try
			{
				this.initShm();
				this.initErrorLoggedOnce = false;
				this.contextCleared = false;
			}
			catch (Exception ex)
			{
				if (!this.initErrorLoggedOnce)
				{
					this.initErrorLoggedOnce = true;
					Log.Error("[Mumble] Error initializing Mumble link:");
					Log.Exception(ex);
				}
				return;
			}
		}
		float unscaledTime = Time.unscaledTime;
		if (unscaledTime - this.lastUpdateTime < 0.02f)
		{
			return;
		}
		this.lastUpdateTime = unscaledTime;
		if (this.mumbleLink.UIVersion == 0U)
		{
			Log.Warning("[Mumble] Mumble disconnected, reinit");
			this.ReinitShm();
		}
		this.mumbleLink.AvatarPosition = (this.mumbleLink.CameraPosition = this.player.position);
		this.mumbleLink.AvatarForward = (this.mumbleLink.CameraForward = this.player.cameraTransform.forward);
		this.mumbleLink.Tick();
	}

	// Token: 0x0600A441 RID: 42049 RVA: 0x003D72E8 File Offset: 0x003D54E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void singletonDestroy()
	{
		if (this.mumbleLink != null)
		{
			if (!this.contextCleared)
			{
				this.contextCleared = true;
				this.mumbleLink.Context = "";
				this.mumbleLink.Tick();
			}
			this.mumbleLink.Dispose();
			this.mumbleLink = null;
			Log.Out("[Mumble] Shared Memory disposed");
		}
		Log.Out("[Mumble] Link destroyed");
	}

	// Token: 0x0600A442 RID: 42050 RVA: 0x003D734D File Offset: 0x003D554D
	public void printUiVersion()
	{
		if (this.mumbleLink == null)
		{
			Log.Out("[Mumble] MumbleLink == null!");
			return;
		}
		Log.Out(string.Format("[Mumble] UiVersion = {0}", this.mumbleLink.UIVersion));
	}

	// Token: 0x04007AFE RID: 31486
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float updateInterval = 0.02f;

	// Token: 0x04007AFF RID: 31487
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ILinkFile mumbleLink;

	// Token: 0x04007B00 RID: 31488
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal player;

	// Token: 0x04007B01 RID: 31489
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initErrorLoggedOnce;

	// Token: 0x04007B02 RID: 31490
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool contextCleared;

	// Token: 0x04007B03 RID: 31491
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastUpdateTime;
}
