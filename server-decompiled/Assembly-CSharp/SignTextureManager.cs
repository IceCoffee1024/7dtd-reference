using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000ACC RID: 2764
public sealed class SignTextureManager
{
	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06005278 RID: 21112 RVA: 0x001F9718 File Offset: 0x001F7918
	public static SignTextureManager Instance
	{
		get
		{
			return SignTextureManager.m_instance;
		}
	}

	// Token: 0x06005279 RID: 21113 RVA: 0x001F9720 File Offset: 0x001F7920
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager()
	{
		this._prioritizer = new SignPrioritizer();
		this._store = new SignTextureStore();
		this._scheduler = new BakeScheduler();
	}

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x0600527A RID: 21114 RVA: 0x001F97A9 File Offset: 0x001F79A9
	public bool IsEnabled
	{
		get
		{
			return this._initialised;
		}
	}

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x0600527B RID: 21115 RVA: 0x001F97B1 File Offset: 0x001F79B1
	public SignTextureManager.SignTextureQuality CurrentQuality
	{
		get
		{
			return this._qualityLevel;
		}
	}

	// Token: 0x0600527C RID: 21116 RVA: 0x001F97BC File Offset: 0x001F79BC
	public void SetQuality(SignTextureManager.SignTextureQuality quality)
	{
		if (this._qualityLevel == quality)
		{
			return;
		}
		this._qualityLevel = quality;
		this.SelectActiveTiers();
		if (this._initialised)
		{
			if (this._qualityLevel == SignTextureManager.SignTextureQuality.Infinite)
			{
				foreach (SignCanvas signCanvas in this._canvases)
				{
					if (signCanvas != null)
					{
						signCanvas.SetTexture(null);
					}
				}
			}
			this._store.RebuildPools(this._activeTiers, this._material);
			this._scheduler.Clear();
			this._requestsDirty = true;
		}
	}

	// Token: 0x0600527D RID: 21117 RVA: 0x001F9864 File Offset: 0x001F7A64
	public void SetTileSize(int tileSize)
	{
		if (tileSize > 0)
		{
			this._tileSize = tileSize;
		}
	}

	// Token: 0x0600527E RID: 21118 RVA: 0x001F9871 File Offset: 0x001F7A71
	public void SetTileSizeForCurrentQuality()
	{
		if (this._activeTiers != null && this._activeTiers.Length != 0)
		{
			this._tileSize = this._activeTiers[0].Resolution;
		}
	}

	// Token: 0x0600527F RID: 21119 RVA: 0x001F989B File Offset: 0x001F7A9B
	public void SetShowProgress(bool showProgress)
	{
		this._showProgress = showProgress;
	}

	// Token: 0x06005280 RID: 21120 RVA: 0x001F98A4 File Offset: 0x001F7AA4
	public bool Initialize()
	{
		bool flag;
		using (SignTextureManager.s_SignTextureManagerInitialize.Auto())
		{
			this._initialised = false;
			if (GameManager.IsDedicatedServer)
			{
				flag = false;
			}
			else
			{
				this._shader = GlobalAssets.FindShader("Game/SignTech/UI");
				if (this._shader == null)
				{
					Log.Error("Failed to initialize SignTextureManager. Shader not found: Game/SignTech/UI");
					flag = false;
				}
				else
				{
					if (this._material == null)
					{
						this._material = new Material(this._shader)
						{
							hideFlags = HideFlags.HideAndDontSave
						};
					}
					if (this._cmd == null)
					{
						this._cmd = new CommandBuffer
						{
							name = "SignTech_Bake"
						};
					}
					this.SelectActiveTiers();
					this._store.RebuildPools(this._activeTiers, this._material);
					this._initialised = true;
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x06005281 RID: 21121 RVA: 0x001F998C File Offset: 0x001F7B8C
	public void Cleanup(bool preserveRegisteredCanvases = false)
	{
		using (SignTextureManager.s_SignTextureManagerCleanup.Auto())
		{
			foreach (SignCanvas signCanvas in this._canvases)
			{
				if (signCanvas != null)
				{
					signCanvas.SetTexture(null);
				}
			}
			if (!preserveRegisteredCanvases)
			{
				this._canvases.Clear();
				SignTextureManager.s_RegisteredSigns.Value = this._canvases.Count;
			}
			this._scheduler.Clear();
			this._prioritizer.Clear();
			this._store.Clear();
			if (this._material != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this._material);
				this._material = null;
			}
			if (this._cmd != null)
			{
				this._cmd.Release();
				this._cmd = null;
			}
			this._shader = null;
			this._initialised = false;
			this._requestsDirty = true;
		}
	}

	// Token: 0x06005282 RID: 21122 RVA: 0x001F9AA0 File Offset: 0x001F7CA0
	public void Register(SignCanvas canvas)
	{
		if (!this._initialised)
		{
			return;
		}
		if (canvas == null)
		{
			return;
		}
		this._canvases.Add(canvas);
		this._requestsDirty = true;
		SignTextureManager.s_RegisteredSigns.Value = this._canvases.Count;
	}

	// Token: 0x06005283 RID: 21123 RVA: 0x001F9ADE File Offset: 0x001F7CDE
	public void Deregister(SignCanvas canvas)
	{
		if (!this._initialised)
		{
			return;
		}
		if (canvas == null)
		{
			return;
		}
		this._canvases.Remove(canvas);
		SignTextureManager.s_RegisteredSigns.Value = this._canvases.Count;
	}

	// Token: 0x06005284 RID: 21124 RVA: 0x001F9B10 File Offset: 0x001F7D10
	public void Invalidate(GlobalSignId signId)
	{
		using (SignTextureManager.s_SignTextureManagerInvalidate.Auto())
		{
			if (this._initialised)
			{
				if (signId.IsValid)
				{
					foreach (SignCanvas signCanvas in this._canvases)
					{
						if (signCanvas != null && signCanvas.DisplaySignId.Equals(signId))
						{
							signCanvas.SetTexture(null);
						}
					}
					this._store.InvalidateSign(signId);
					this._scheduler.Clear();
					this._requestsDirty = true;
				}
			}
		}
	}

	// Token: 0x06005285 RID: 21125 RVA: 0x001F9BE0 File Offset: 0x001F7DE0
	public void MainThreadUpdate()
	{
		SignTextureManager.<>c__DisplayClass48_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		using (SignTextureManager.s_SignTextureManagerMainThreadUpdate.Auto())
		{
			if (this._initialised)
			{
				if (!(this._material == null) && this._cmd != null)
				{
					if (this._qualityLevel != SignTextureManager.SignTextureQuality.Infinite)
					{
						this._framesSinceRefreshed += 1U;
						CS$<>8__locals1.playerPos = SignTextureManager.GetPlayerPos();
						float sqrMagnitude = (CS$<>8__locals1.playerPos - this._lastUpdatePosition).sqrMagnitude;
						this._requestsDirty |= this._framesSinceRefreshed > 10U && sqrMagnitude > 1f;
						if (this._requestsDirty && this._scheduler.CurrentState == BakeScheduler.BakeJobState.None)
						{
							this.<MainThreadUpdate>g__RefreshRequestQueue|48_0(ref CS$<>8__locals1);
						}
						else
						{
							this._scheduler.TickOnce(this._tileSize, this._material, this._cmd, this._store, this._prioritizer, this._showProgress);
						}
					}
				}
			}
		}
	}

	// Token: 0x06005286 RID: 21126 RVA: 0x001F9D00 File Offset: 0x001F7F00
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 GetPlayerPos()
	{
		return GameManager.Instance.World.GetPrimaryPlayer().position + new Vector3(0f, 1.5f, 0f);
	}

	// Token: 0x06005287 RID: 21127 RVA: 0x001F9D30 File Offset: 0x001F7F30
	[PublicizedFrom(EAccessModifier.Private)]
	public void SelectActiveTiers()
	{
		SignTextureManager.TierSpec[] array;
		switch (this._qualityLevel)
		{
		case SignTextureManager.SignTextureQuality.Low:
			array = SignTextureManager.s_tiersLow;
			break;
		case SignTextureManager.SignTextureQuality.Medium:
			array = SignTextureManager.s_tiersMedium;
			break;
		case SignTextureManager.SignTextureQuality.High:
			array = SignTextureManager.s_tiersHigh;
			break;
		case SignTextureManager.SignTextureQuality.Ultra:
			array = SignTextureManager.s_tiersUltra;
			break;
		case SignTextureManager.SignTextureQuality.Infinite:
			array = Array.Empty<SignTextureManager.TierSpec>();
			break;
		default:
			array = SignTextureManager.s_tiersLowest;
			break;
		}
		this._activeTiers = array;
	}

	// Token: 0x06005288 RID: 21128 RVA: 0x001F9D98 File Offset: 0x001F7F98
	public void LogDebugInfo(bool saveToDisk)
	{
		this.sb.Clear();
		this.sb.AppendLine("[STM] LogDebugInfo");
		bool flag = true;
		if (!this._initialised)
		{
			flag = false;
			this.sb.AppendLine("Cannot log sign group info: SignTextureManager is not initialized.");
		}
		else if (this._material == null || this._cmd == null)
		{
			flag = false;
			this.sb.AppendLine("Cannot log sign group info: SignTextureManager has null material or command buffer.");
		}
		else if (this._qualityLevel == SignTextureManager.SignTextureQuality.Infinite)
		{
			flag = false;
			this.sb.AppendLine("Cannot log sign group info: SignTextureManager is in Infinite quality mode.");
		}
		if (flag)
		{
			Vector3 playerPos = SignTextureManager.GetPlayerPos();
			float sqrMagnitude = (playerPos - this._lastUpdatePosition).sqrMagnitude;
			this.sb.AppendLine();
			this.sb.AppendLine("**************************************************");
			this.sb.AppendLine("Manager (Enabled)");
			this.sb.AppendLine("**************************************************");
			this.sb.AppendLine();
			this.sb.AppendLine(string.Format("Registered Canvases: {0}", this._canvases.Count));
			this.sb.AppendLine();
			this.sb.AppendLine(string.Format("Bake Job State: {0}", this._scheduler.CurrentState));
			this.sb.AppendLine(string.Format("Player Pos: {0}", playerPos));
			this.sb.AppendLine(string.Format("Last Update Pos: {0}", this._lastUpdatePosition));
			this.sb.AppendLine(string.Format("Sq Dist Since Update: {0} (min: {1}, ready: {2})", sqrMagnitude.ToString("00.000"), 1f, sqrMagnitude > 1f));
			this.sb.AppendLine(string.Format("Frames Since Update: {0} (min: {1}, ready: {2})", this._framesSinceRefreshed, 10U, this._framesSinceRefreshed > 10U));
			this.sb.AppendLine(string.Format("Requests Dirty: {0}", this._requestsDirty));
			this.sb.AppendLine();
			this.sb.AppendLine(string.Format("Tile Size: {0}", this._tileSize));
			this.sb.AppendLine(string.Format("Quality: {0}", this._qualityLevel));
			if (this._activeTiers == null)
			{
				this.sb.AppendLine("Active Tiers: NULL");
			}
			else if (this._activeTiers.Length == 0)
			{
				this.sb.AppendLine("Active Tiers: NONE");
			}
			else
			{
				this.sb.AppendLine("Active Tiers:");
				for (int i = 0; i < this._activeTiers.Length; i++)
				{
					SignTextureManager.TierSpec tierSpec = this._activeTiers[i];
					this.sb.AppendLine(string.Format("\t{0}: {1}px, {2} active, {3} buffer", new object[] { i, tierSpec.Resolution, tierSpec.ActiveCount, tierSpec.BufferCount }));
				}
			}
			this.sb.AppendLine();
			this._store.WriteTextureStoreInfo(this.sb);
			this.sb.AppendLine();
			this._prioritizer.WriteGroupInfo(this.sb, playerPos, this._activeTiers);
			Log.Out(this.sb.ToString());
			if (saveToDisk)
			{
				string text = string.Format("STM_Dump_{0:yyyyMMdd_HHmmss}.txt", DateTime.Now);
				string text2 = Path.Combine(Application.dataPath + "/../", text);
				File.WriteAllText(text2, this.sb.ToString());
				Log.Out("[STM] Full dump saved to " + text2);
			}
		}
		else
		{
			Log.Warning(this.sb.ToString());
		}
		this.sb.Clear();
	}

	// Token: 0x06005289 RID: 21129 RVA: 0x001FA192 File Offset: 0x001F8392
	[Conditional("SIGN_TEXTURE_DEBUG_LOG_INIT")]
	public static void DebugLog(string message)
	{
		Log.Out("[STM] " + message);
	}

	// Token: 0x0600528B RID: 21131 RVA: 0x001FA36C File Offset: 0x001F856C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <MainThreadUpdate>g__RefreshRequestQueue|48_0(ref SignTextureManager.<>c__DisplayClass48_0 A_1)
	{
		this._store.BeginFrameMarkAllNotInUse();
		this._tmpRequests.Clear();
		this._prioritizer.RebuildRequests(this._canvases, A_1.playerPos, this._activeTiers, this._store, this._tmpRequests);
		this._store.EvictUnused();
		this._scheduler.ResetRequests(this._tmpRequests);
		this._lastUpdatePosition = A_1.playerPos;
		this._framesSinceRefreshed = 0U;
		this._requestsDirty = false;
	}

	// Token: 0x0400401F RID: 16415
	[PublicizedFrom(EAccessModifier.Private)]
	public const string kShaderName = "Game/SignTech/UI";

	// Token: 0x04004020 RID: 16416
	public static readonly ProfilerMarker s_SignTextureManagerInitialize = new ProfilerMarker("SignTextureManager.Initialize");

	// Token: 0x04004021 RID: 16417
	public static readonly ProfilerMarker s_SignTextureManagerCleanup = new ProfilerMarker("SignTextureManager.Cleanup");

	// Token: 0x04004022 RID: 16418
	public static readonly ProfilerMarker s_SignTextureManagerInvalidate = new ProfilerMarker("SignTextureManager.Invalidate");

	// Token: 0x04004023 RID: 16419
	public static readonly ProfilerMarker s_SignTextureManagerMainThreadUpdate = new ProfilerMarker("SignTextureManager.MainThreadUpdate");

	// Token: 0x04004024 RID: 16420
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_RegisteredSigns = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "STM Registered Signs", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04004025 RID: 16421
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager m_instance = new SignTextureManager();

	// Token: 0x04004026 RID: 16422
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersLowest = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(64, 32, 32),
		new SignTextureManager.TierSpec(128, 32, 16),
		new SignTextureManager.TierSpec(512, 8, 8)
	};

	// Token: 0x04004027 RID: 16423
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersLow = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(128, 32, 32),
		new SignTextureManager.TierSpec(512, 8, 8),
		new SignTextureManager.TierSpec(1024, 3, 4)
	};

	// Token: 0x04004028 RID: 16424
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersMedium = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(256, 32, 16),
		new SignTextureManager.TierSpec(512, 8, 4),
		new SignTextureManager.TierSpec(1024, 3, 4)
	};

	// Token: 0x04004029 RID: 16425
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersHigh = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(256, 32, 16),
		new SignTextureManager.TierSpec(1024, 10, 6),
		new SignTextureManager.TierSpec(2048, 1, 1)
	};

	// Token: 0x0400402A RID: 16426
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersUltra = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(512, 32, 16),
		new SignTextureManager.TierSpec(1024, 10, 6),
		new SignTextureManager.TierSpec(2048, 2, 2)
	};

	// Token: 0x0400402B RID: 16427
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<SignCanvas> _canvases = new HashSet<SignCanvas>();

	// Token: 0x0400402C RID: 16428
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignPrioritizer _prioritizer;

	// Token: 0x0400402D RID: 16429
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignTextureStore _store;

	// Token: 0x0400402E RID: 16430
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BakeScheduler _scheduler;

	// Token: 0x0400402F RID: 16431
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignBakeRequest> _tmpRequests = new List<SignBakeRequest>(256);

	// Token: 0x04004030 RID: 16432
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader _shader;

	// Token: 0x04004031 RID: 16433
	[PublicizedFrom(EAccessModifier.Private)]
	public Material _material;

	// Token: 0x04004032 RID: 16434
	[PublicizedFrom(EAccessModifier.Private)]
	public CommandBuffer _cmd;

	// Token: 0x04004033 RID: 16435
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _initialised;

	// Token: 0x04004034 RID: 16436
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager.SignTextureQuality _qualityLevel = SignTextureManager.SignTextureQuality.Medium;

	// Token: 0x04004035 RID: 16437
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager.TierSpec[] _activeTiers = SignTextureManager.s_tiersMedium;

	// Token: 0x04004036 RID: 16438
	[PublicizedFrom(EAccessModifier.Private)]
	public int _tileSize = 256;

	// Token: 0x04004037 RID: 16439
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _requestsDirty = true;

	// Token: 0x04004038 RID: 16440
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 _lastUpdatePosition = Vector3.zero;

	// Token: 0x04004039 RID: 16441
	[PublicizedFrom(EAccessModifier.Private)]
	public uint _framesSinceRefreshed;

	// Token: 0x0400403A RID: 16442
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint kMinFramesSinceRefreshed = 10U;

	// Token: 0x0400403B RID: 16443
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kMinSqrDistanceSinceRefreshed = 1f;

	// Token: 0x0400403C RID: 16444
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _showProgress;

	// Token: 0x0400403D RID: 16445
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder sb = new StringBuilder();

	// Token: 0x02000ACD RID: 2765
	public enum SignTextureQuality
	{
		// Token: 0x0400403F RID: 16447
		Lowest,
		// Token: 0x04004040 RID: 16448
		Low,
		// Token: 0x04004041 RID: 16449
		Medium,
		// Token: 0x04004042 RID: 16450
		High,
		// Token: 0x04004043 RID: 16451
		Ultra,
		// Token: 0x04004044 RID: 16452
		Infinite
	}

	// Token: 0x02000ACE RID: 2766
	public readonly struct TierSpec
	{
		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x0600528C RID: 21132 RVA: 0x001FA3EE File Offset: 0x001F85EE
		public int TotalCount
		{
			get
			{
				return this.ActiveCount + this.BufferCount;
			}
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x001FA3FD File Offset: 0x001F85FD
		public TierSpec(int resolution, int activeCount, int bufferCount)
		{
			this.Resolution = resolution;
			this.ActiveCount = activeCount;
			this.BufferCount = bufferCount;
		}

		// Token: 0x04004045 RID: 16453
		public readonly int Resolution;

		// Token: 0x04004046 RID: 16454
		public readonly int ActiveCount;

		// Token: 0x04004047 RID: 16455
		public readonly int BufferCount;
	}
}
