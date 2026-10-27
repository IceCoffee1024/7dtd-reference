using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AD6 RID: 2774
public sealed class SignTextureManager
{
	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x0600529C RID: 21148 RVA: 0x001FA7F4 File Offset: 0x001F89F4
	public static SignTextureManager Instance
	{
		get
		{
			return SignTextureManager.m_instance;
		}
	}

	// Token: 0x0600529D RID: 21149 RVA: 0x001FA7FC File Offset: 0x001F89FC
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager()
	{
		this._prioritizer = new SignPrioritizer();
		this._store = new SignTextureStore();
		this._scheduler = new BakeScheduler();
	}

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x0600529E RID: 21150 RVA: 0x001FA885 File Offset: 0x001F8A85
	public bool IsEnabled
	{
		get
		{
			return this._initialised;
		}
	}

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x0600529F RID: 21151 RVA: 0x001FA88D File Offset: 0x001F8A8D
	public SignTextureManager.SignTextureQuality CurrentQuality
	{
		get
		{
			return this._qualityLevel;
		}
	}

	// Token: 0x060052A0 RID: 21152 RVA: 0x001FA898 File Offset: 0x001F8A98
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

	// Token: 0x060052A1 RID: 21153 RVA: 0x001FA940 File Offset: 0x001F8B40
	public void SetTileSize(int tileSize)
	{
		if (tileSize > 0)
		{
			this._tileSize = tileSize;
		}
	}

	// Token: 0x060052A2 RID: 21154 RVA: 0x001FA94D File Offset: 0x001F8B4D
	public void SetTileSizeForCurrentQuality()
	{
		if (this._activeTiers != null && this._activeTiers.Length != 0)
		{
			this._tileSize = this._activeTiers[0].Resolution;
		}
	}

	// Token: 0x060052A3 RID: 21155 RVA: 0x001FA977 File Offset: 0x001F8B77
	public void SetShowProgress(bool showProgress)
	{
		this._showProgress = showProgress;
	}

	// Token: 0x060052A4 RID: 21156 RVA: 0x001FA980 File Offset: 0x001F8B80
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

	// Token: 0x060052A5 RID: 21157 RVA: 0x001FAA68 File Offset: 0x001F8C68
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

	// Token: 0x060052A6 RID: 21158 RVA: 0x001FAB7C File Offset: 0x001F8D7C
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

	// Token: 0x060052A7 RID: 21159 RVA: 0x001FABBA File Offset: 0x001F8DBA
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

	// Token: 0x060052A8 RID: 21160 RVA: 0x001FABEC File Offset: 0x001F8DEC
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

	// Token: 0x060052A9 RID: 21161 RVA: 0x001FACBC File Offset: 0x001F8EBC
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

	// Token: 0x060052AA RID: 21162 RVA: 0x001FADDC File Offset: 0x001F8FDC
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 GetPlayerPos()
	{
		return GameManager.Instance.World.GetPrimaryPlayer().position + new Vector3(0f, 1.5f, 0f);
	}

	// Token: 0x060052AB RID: 21163 RVA: 0x001FAE0C File Offset: 0x001F900C
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

	// Token: 0x060052AC RID: 21164 RVA: 0x001FAE74 File Offset: 0x001F9074
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

	// Token: 0x060052AD RID: 21165 RVA: 0x001FB26E File Offset: 0x001F946E
	[Conditional("SIGN_TEXTURE_DEBUG_LOG_INIT")]
	public static void DebugLog(string message)
	{
		Log.Out("[STM] " + message);
	}

	// Token: 0x060052AF RID: 21167 RVA: 0x001FB448 File Offset: 0x001F9648
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

	// Token: 0x04004040 RID: 16448
	[PublicizedFrom(EAccessModifier.Private)]
	public const string kShaderName = "Game/SignTech/UI";

	// Token: 0x04004041 RID: 16449
	public static readonly ProfilerMarker s_SignTextureManagerInitialize = new ProfilerMarker("SignTextureManager.Initialize");

	// Token: 0x04004042 RID: 16450
	public static readonly ProfilerMarker s_SignTextureManagerCleanup = new ProfilerMarker("SignTextureManager.Cleanup");

	// Token: 0x04004043 RID: 16451
	public static readonly ProfilerMarker s_SignTextureManagerInvalidate = new ProfilerMarker("SignTextureManager.Invalidate");

	// Token: 0x04004044 RID: 16452
	public static readonly ProfilerMarker s_SignTextureManagerMainThreadUpdate = new ProfilerMarker("SignTextureManager.MainThreadUpdate");

	// Token: 0x04004045 RID: 16453
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_RegisteredSigns = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "STM Registered Signs", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04004046 RID: 16454
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager m_instance = new SignTextureManager();

	// Token: 0x04004047 RID: 16455
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersLowest = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(64, 32, 32),
		new SignTextureManager.TierSpec(128, 32, 16),
		new SignTextureManager.TierSpec(512, 8, 8)
	};

	// Token: 0x04004048 RID: 16456
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersLow = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(128, 32, 32),
		new SignTextureManager.TierSpec(512, 8, 8),
		new SignTextureManager.TierSpec(1024, 3, 4)
	};

	// Token: 0x04004049 RID: 16457
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersMedium = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(256, 32, 16),
		new SignTextureManager.TierSpec(512, 8, 4),
		new SignTextureManager.TierSpec(1024, 3, 4)
	};

	// Token: 0x0400404A RID: 16458
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersHigh = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(256, 32, 16),
		new SignTextureManager.TierSpec(1024, 10, 6),
		new SignTextureManager.TierSpec(2048, 1, 1)
	};

	// Token: 0x0400404B RID: 16459
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersUltra = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(512, 32, 16),
		new SignTextureManager.TierSpec(1024, 10, 6),
		new SignTextureManager.TierSpec(2048, 2, 2)
	};

	// Token: 0x0400404C RID: 16460
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<SignCanvas> _canvases = new HashSet<SignCanvas>();

	// Token: 0x0400404D RID: 16461
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignPrioritizer _prioritizer;

	// Token: 0x0400404E RID: 16462
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignTextureStore _store;

	// Token: 0x0400404F RID: 16463
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BakeScheduler _scheduler;

	// Token: 0x04004050 RID: 16464
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignBakeRequest> _tmpRequests = new List<SignBakeRequest>(256);

	// Token: 0x04004051 RID: 16465
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader _shader;

	// Token: 0x04004052 RID: 16466
	[PublicizedFrom(EAccessModifier.Private)]
	public Material _material;

	// Token: 0x04004053 RID: 16467
	[PublicizedFrom(EAccessModifier.Private)]
	public CommandBuffer _cmd;

	// Token: 0x04004054 RID: 16468
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _initialised;

	// Token: 0x04004055 RID: 16469
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager.SignTextureQuality _qualityLevel = SignTextureManager.SignTextureQuality.Medium;

	// Token: 0x04004056 RID: 16470
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager.TierSpec[] _activeTiers = SignTextureManager.s_tiersMedium;

	// Token: 0x04004057 RID: 16471
	[PublicizedFrom(EAccessModifier.Private)]
	public int _tileSize = 256;

	// Token: 0x04004058 RID: 16472
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _requestsDirty = true;

	// Token: 0x04004059 RID: 16473
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 _lastUpdatePosition = Vector3.zero;

	// Token: 0x0400405A RID: 16474
	[PublicizedFrom(EAccessModifier.Private)]
	public uint _framesSinceRefreshed;

	// Token: 0x0400405B RID: 16475
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint kMinFramesSinceRefreshed = 10U;

	// Token: 0x0400405C RID: 16476
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kMinSqrDistanceSinceRefreshed = 1f;

	// Token: 0x0400405D RID: 16477
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _showProgress;

	// Token: 0x0400405E RID: 16478
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder sb = new StringBuilder();

	// Token: 0x02000AD7 RID: 2775
	public enum SignTextureQuality
	{
		// Token: 0x04004060 RID: 16480
		Lowest,
		// Token: 0x04004061 RID: 16481
		Low,
		// Token: 0x04004062 RID: 16482
		Medium,
		// Token: 0x04004063 RID: 16483
		High,
		// Token: 0x04004064 RID: 16484
		Ultra,
		// Token: 0x04004065 RID: 16485
		Infinite
	}

	// Token: 0x02000AD8 RID: 2776
	public readonly struct TierSpec
	{
		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060052B0 RID: 21168 RVA: 0x001FB4CA File Offset: 0x001F96CA
		public int TotalCount
		{
			get
			{
				return this.ActiveCount + this.BufferCount;
			}
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x001FB4D9 File Offset: 0x001F96D9
		public TierSpec(int resolution, int activeCount, int bufferCount)
		{
			this.Resolution = resolution;
			this.ActiveCount = activeCount;
			this.BufferCount = bufferCount;
		}

		// Token: 0x04004066 RID: 16486
		public readonly int Resolution;

		// Token: 0x04004067 RID: 16487
		public readonly int ActiveCount;

		// Token: 0x04004068 RID: 16488
		public readonly int BufferCount;
	}
}
