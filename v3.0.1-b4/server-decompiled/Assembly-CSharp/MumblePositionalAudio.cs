using System;
using mumblelib;
using UnityEngine;

// Token: 0x02001432 RID: 5170
public class MumblePositionalAudio : SingletonMonoBehaviour<MumblePositionalAudio>
{
	// Token: 0x0600A239 RID: 41529 RVA: 0x003CACFC File Offset: 0x003C8EFC
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

	// Token: 0x0600A23A RID: 41530 RVA: 0x003CAD29 File Offset: 0x003C8F29
	public static void Destroy()
	{
		if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance == null)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(SingletonMonoBehaviour<MumblePositionalAudio>.Instance.gameObject);
	}

	// Token: 0x0600A23B RID: 41531 RVA: 0x003CAD48 File Offset: 0x003C8F48
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

	// Token: 0x0600A23C RID: 41532 RVA: 0x003CAE0A File Offset: 0x003C900A
	[PublicizedFrom(EAccessModifier.Private)]
	public void initShm()
	{
		this.mumbleLink = LinkFileManager.Open();
		this.setCommonValues();
		Log.Out("[Mumble] Shared Memory initialized");
	}

	// Token: 0x0600A23D RID: 41533 RVA: 0x003CAE27 File Offset: 0x003C9027
	public void ReinitShm()
	{
		if (this.mumbleLink == null)
		{
			this.initShm();
			return;
		}
		this.setCommonValues();
	}

	// Token: 0x0600A23E RID: 41534 RVA: 0x003CAE3E File Offset: 0x003C903E
	public void SetPlayer(EntityPlayerLocal player)
	{
		this.player = player;
	}

	// Token: 0x0600A23F RID: 41535 RVA: 0x003CAE48 File Offset: 0x003C9048
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

	// Token: 0x0600A240 RID: 41536 RVA: 0x003CAF90 File Offset: 0x003C9190
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

	// Token: 0x0600A241 RID: 41537 RVA: 0x003CAFF5 File Offset: 0x003C91F5
	public void printUiVersion()
	{
		if (this.mumbleLink == null)
		{
			Log.Out("[Mumble] MumbleLink == null!");
			return;
		}
		Log.Out(string.Format("[Mumble] UiVersion = {0}", this.mumbleLink.UIVersion));
	}

	// Token: 0x04007969 RID: 31081
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float updateInterval = 0.02f;

	// Token: 0x0400796A RID: 31082
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ILinkFile mumbleLink;

	// Token: 0x0400796B RID: 31083
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal player;

	// Token: 0x0400796C RID: 31084
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initErrorLoggedOnce;

	// Token: 0x0400796D RID: 31085
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool contextCleared;

	// Token: 0x0400796E RID: 31086
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastUpdateTime;
}
