using System;
using mumblelib;
using UnityEngine;

// Token: 0x0200144D RID: 5197
public class MumblePositionalAudio : SingletonMonoBehaviour<MumblePositionalAudio>
{
	// Token: 0x0600A336 RID: 41782 RVA: 0x003D0040 File Offset: 0x003CE240
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

	// Token: 0x0600A337 RID: 41783 RVA: 0x003D006D File Offset: 0x003CE26D
	public static void Destroy()
	{
		if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance == null)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(SingletonMonoBehaviour<MumblePositionalAudio>.Instance.gameObject);
	}

	// Token: 0x0600A338 RID: 41784 RVA: 0x003D008C File Offset: 0x003CE28C
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

	// Token: 0x0600A339 RID: 41785 RVA: 0x003D014E File Offset: 0x003CE34E
	[PublicizedFrom(EAccessModifier.Private)]
	public void initShm()
	{
		this.mumbleLink = LinkFileManager.Open();
		this.setCommonValues();
		Log.Out("[Mumble] Shared Memory initialized");
	}

	// Token: 0x0600A33A RID: 41786 RVA: 0x003D016B File Offset: 0x003CE36B
	public void ReinitShm()
	{
		if (this.mumbleLink == null)
		{
			this.initShm();
			return;
		}
		this.setCommonValues();
	}

	// Token: 0x0600A33B RID: 41787 RVA: 0x003D0182 File Offset: 0x003CE382
	public void SetPlayer(EntityPlayerLocal player)
	{
		this.player = player;
	}

	// Token: 0x0600A33C RID: 41788 RVA: 0x003D018C File Offset: 0x003CE38C
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

	// Token: 0x0600A33D RID: 41789 RVA: 0x003D02D4 File Offset: 0x003CE4D4
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

	// Token: 0x0600A33E RID: 41790 RVA: 0x003D0339 File Offset: 0x003CE539
	public void printUiVersion()
	{
		if (this.mumbleLink == null)
		{
			Log.Out("[Mumble] MumbleLink == null!");
			return;
		}
		Log.Out(string.Format("[Mumble] UiVersion = {0}", this.mumbleLink.UIVersion));
	}

	// Token: 0x04007A42 RID: 31298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float updateInterval = 0.02f;

	// Token: 0x04007A43 RID: 31299
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ILinkFile mumbleLink;

	// Token: 0x04007A44 RID: 31300
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal player;

	// Token: 0x04007A45 RID: 31301
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initErrorLoggedOnce;

	// Token: 0x04007A46 RID: 31302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool contextCleared;

	// Token: 0x04007A47 RID: 31303
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastUpdateTime;
}
