using System;
using mumblelib;
using UnityEngine;

// Token: 0x0200146D RID: 5229
public class MumblePositionalAudio : SingletonMonoBehaviour<MumblePositionalAudio>
{
	// Token: 0x0600A3E2 RID: 41954 RVA: 0x003D4C50 File Offset: 0x003D2E50
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

	// Token: 0x0600A3E3 RID: 41955 RVA: 0x003D4C7D File Offset: 0x003D2E7D
	public static void Destroy()
	{
		if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance == null)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(SingletonMonoBehaviour<MumblePositionalAudio>.Instance.gameObject);
	}

	// Token: 0x0600A3E4 RID: 41956 RVA: 0x003D4C9C File Offset: 0x003D2E9C
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

	// Token: 0x0600A3E5 RID: 41957 RVA: 0x003D4D5E File Offset: 0x003D2F5E
	[PublicizedFrom(EAccessModifier.Private)]
	public void initShm()
	{
		this.mumbleLink = LinkFileManager.Open();
		this.setCommonValues();
		Log.Out("[Mumble] Shared Memory initialized");
	}

	// Token: 0x0600A3E6 RID: 41958 RVA: 0x003D4D7B File Offset: 0x003D2F7B
	public void ReinitShm()
	{
		if (this.mumbleLink == null)
		{
			this.initShm();
			return;
		}
		this.setCommonValues();
	}

	// Token: 0x0600A3E7 RID: 41959 RVA: 0x003D4D92 File Offset: 0x003D2F92
	public void SetPlayer(EntityPlayerLocal player)
	{
		this.player = player;
	}

	// Token: 0x0600A3E8 RID: 41960 RVA: 0x003D4D9C File Offset: 0x003D2F9C
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

	// Token: 0x0600A3E9 RID: 41961 RVA: 0x003D4EE4 File Offset: 0x003D30E4
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

	// Token: 0x0600A3EA RID: 41962 RVA: 0x003D4F49 File Offset: 0x003D3149
	public void printUiVersion()
	{
		if (this.mumbleLink == null)
		{
			Log.Out("[Mumble] MumbleLink == null!");
			return;
		}
		Log.Out(string.Format("[Mumble] UiVersion = {0}", this.mumbleLink.UIVersion));
	}

	// Token: 0x04007ABD RID: 31421
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float updateInterval = 0.02f;

	// Token: 0x04007ABE RID: 31422
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ILinkFile mumbleLink;

	// Token: 0x04007ABF RID: 31423
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal player;

	// Token: 0x04007AC0 RID: 31424
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initErrorLoggedOnce;

	// Token: 0x04007AC1 RID: 31425
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool contextCleared;

	// Token: 0x04007AC2 RID: 31426
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastUpdateTime;
}
