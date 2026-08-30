using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AA9 RID: 2729
public sealed class SignTextureManager
{
	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x060051D9 RID: 20953 RVA: 0x001F3A58 File Offset: 0x001F1C58
	public static SignTextureManager Instance
	{
		get
		{
			return SignTextureManager.m_instance;
		}
	}

	// Token: 0x060051DA RID: 20954 RVA: 0x001F3A60 File Offset: 0x001F1C60
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager()
	{
		this._prioritizer = new SignPrioritizer();
		this._store = new SignTextureStore();
		this._scheduler = new BakeScheduler();
	}

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x060051DB RID: 20955 RVA: 0x001F3AE9 File Offset: 0x001F1CE9
	public bool IsEnabled
	{
		get
		{
			return this._initialised;
		}
	}

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x060051DC RID: 20956 RVA: 0x001F3AF1 File Offset: 0x001F1CF1
	public SignTextureManager.SignTextureQuality CurrentQuality
	{
		get
		{
			return this._qualityLevel;
		}
	}

	// Token: 0x060051DD RID: 20957 RVA: 0x001F3AFC File Offset: 0x001F1CFC
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

	// Token: 0x060051DE RID: 20958 RVA: 0x001F3BA4 File Offset: 0x001F1DA4
	public void SetTileSize(int tileSize)
	{
		if (tileSize > 0)
		{
			this._tileSize = tileSize;
		}
	}

	// Token: 0x060051DF RID: 20959 RVA: 0x001F3BB1 File Offset: 0x001F1DB1
	public void SetTileSizeForCurrentQuality()
	{
		if (this._activeTiers != null && this._activeTiers.Length != 0)
		{
			this._tileSize = this._activeTiers[0].Resolution;
		}
	}

	// Token: 0x060051E0 RID: 20960 RVA: 0x001F3BDB File Offset: 0x001F1DDB
	public void SetShowProgress(bool showProgress)
	{
		this._showProgress = showProgress;
	}

	// Token: 0x060051E1 RID: 20961 RVA: 0x001F3BE4 File Offset: 0x001F1DE4
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

	// Token: 0x060051E2 RID: 20962 RVA: 0x001F3CCC File Offset: 0x001F1ECC
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

	// Token: 0x060051E3 RID: 20963 RVA: 0x001F3DE0 File Offset: 0x001F1FE0
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

	// Token: 0x060051E4 RID: 20964 RVA: 0x001F3E1E File Offset: 0x001F201E
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

	// Token: 0x060051E5 RID: 20965 RVA: 0x001F3E58 File Offset: 0x001F2058
	public void Invalidate(SignCanvas canvas)
	{
		using (SignTextureManager.s_SignTextureManagerInvalidate.Auto())
		{
			if (this._initialised)
			{
				if (!(canvas == null))
				{
					if (canvas.DisplaySignId.IsValid)
					{
						GlobalSignId displaySignId = canvas.DisplaySignId;
						foreach (SignCanvas signCanvas in this._canvases)
						{
							if (signCanvas != null && signCanvas.DisplaySignId.Equals(displaySignId))
							{
								signCanvas.SetTexture(null);
							}
						}
						this._store.InvalidateSign(displaySignId);
						this._scheduler.Clear();
						this._requestsDirty = true;
					}
				}
			}
		}
	}

	// Token: 0x060051E6 RID: 20966 RVA: 0x001F3F4C File Offset: 0x001F214C
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

	// Token: 0x060051E7 RID: 20967 RVA: 0x001F406C File Offset: 0x001F226C
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 GetPlayerPos()
	{
		return GameManager.Instance.World.GetPrimaryPlayer().position + new Vector3(0f, 1.5f, 0f);
	}

	// Token: 0x060051E8 RID: 20968 RVA: 0x001F409C File Offset: 0x001F229C
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

	// Token: 0x060051E9 RID: 20969 RVA: 0x001F4104 File Offset: 0x001F2304
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

	// Token: 0x060051EA RID: 20970 RVA: 0x001F44FE File Offset: 0x001F26FE
	[Conditional("SIGN_TEXTURE_DEBUG_LOG_INIT")]
	public static void DebugLog(string message)
	{
		Log.Out("[STM] " + message);
	}

	// Token: 0x060051EC RID: 20972 RVA: 0x001F46D8 File Offset: 0x001F28D8
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

	// Token: 0x04003F51 RID: 16209
	[PublicizedFrom(EAccessModifier.Private)]
	public const string kShaderName = "Game/SignTech/UI";

	// Token: 0x04003F52 RID: 16210
	public static readonly ProfilerMarker s_SignTextureManagerInitialize = new ProfilerMarker("SignTextureManager.Initialize");

	// Token: 0x04003F53 RID: 16211
	public static readonly ProfilerMarker s_SignTextureManagerCleanup = new ProfilerMarker("SignTextureManager.Cleanup");

	// Token: 0x04003F54 RID: 16212
	public static readonly ProfilerMarker s_SignTextureManagerInvalidate = new ProfilerMarker("SignTextureManager.Invalidate");

	// Token: 0x04003F55 RID: 16213
	public static readonly ProfilerMarker s_SignTextureManagerMainThreadUpdate = new ProfilerMarker("SignTextureManager.MainThreadUpdate");

	// Token: 0x04003F56 RID: 16214
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_RegisteredSigns = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "STM Registered Signs", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003F57 RID: 16215
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager m_instance = new SignTextureManager();

	// Token: 0x04003F58 RID: 16216
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersLowest = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(64, 32, 32),
		new SignTextureManager.TierSpec(128, 32, 16),
		new SignTextureManager.TierSpec(512, 8, 8)
	};

	// Token: 0x04003F59 RID: 16217
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersLow = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(128, 32, 32),
		new SignTextureManager.TierSpec(512, 8, 8),
		new SignTextureManager.TierSpec(1024, 3, 4)
	};

	// Token: 0x04003F5A RID: 16218
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersMedium = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(256, 32, 16),
		new SignTextureManager.TierSpec(512, 8, 4),
		new SignTextureManager.TierSpec(1024, 3, 4)
	};

	// Token: 0x04003F5B RID: 16219
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersHigh = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(256, 32, 16),
		new SignTextureManager.TierSpec(1024, 10, 6),
		new SignTextureManager.TierSpec(2048, 1, 1)
	};

	// Token: 0x04003F5C RID: 16220
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersUltra = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(512, 32, 16),
		new SignTextureManager.TierSpec(1024, 10, 6),
		new SignTextureManager.TierSpec(2048, 2, 2)
	};

	// Token: 0x04003F5D RID: 16221
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<SignCanvas> _canvases = new HashSet<SignCanvas>();

	// Token: 0x04003F5E RID: 16222
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignPrioritizer _prioritizer;

	// Token: 0x04003F5F RID: 16223
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignTextureStore _store;

	// Token: 0x04003F60 RID: 16224
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BakeScheduler _scheduler;

	// Token: 0x04003F61 RID: 16225
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignBakeRequest> _tmpRequests = new List<SignBakeRequest>(256);

	// Token: 0x04003F62 RID: 16226
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader _shader;

	// Token: 0x04003F63 RID: 16227
	[PublicizedFrom(EAccessModifier.Private)]
	public Material _material;

	// Token: 0x04003F64 RID: 16228
	[PublicizedFrom(EAccessModifier.Private)]
	public CommandBuffer _cmd;

	// Token: 0x04003F65 RID: 16229
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _initialised;

	// Token: 0x04003F66 RID: 16230
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager.SignTextureQuality _qualityLevel = SignTextureManager.SignTextureQuality.Medium;

	// Token: 0x04003F67 RID: 16231
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager.TierSpec[] _activeTiers = SignTextureManager.s_tiersMedium;

	// Token: 0x04003F68 RID: 16232
	[PublicizedFrom(EAccessModifier.Private)]
	public int _tileSize = 256;

	// Token: 0x04003F69 RID: 16233
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _requestsDirty = true;

	// Token: 0x04003F6A RID: 16234
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 _lastUpdatePosition = Vector3.zero;

	// Token: 0x04003F6B RID: 16235
	[PublicizedFrom(EAccessModifier.Private)]
	public uint _framesSinceRefreshed;

	// Token: 0x04003F6C RID: 16236
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint kMinFramesSinceRefreshed = 10U;

	// Token: 0x04003F6D RID: 16237
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kMinSqrDistanceSinceRefreshed = 1f;

	// Token: 0x04003F6E RID: 16238
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _showProgress;

	// Token: 0x04003F6F RID: 16239
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder sb = new StringBuilder();

	// Token: 0x02000AAA RID: 2730
	public enum SignTextureQuality
	{
		// Token: 0x04003F71 RID: 16241
		Lowest,
		// Token: 0x04003F72 RID: 16242
		Low,
		// Token: 0x04003F73 RID: 16243
		Medium,
		// Token: 0x04003F74 RID: 16244
		High,
		// Token: 0x04003F75 RID: 16245
		Ultra,
		// Token: 0x04003F76 RID: 16246
		Infinite
	}

	// Token: 0x02000AAB RID: 2731
	public readonly struct TierSpec
	{
		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060051ED RID: 20973 RVA: 0x001F475A File Offset: 0x001F295A
		public int TotalCount
		{
			get
			{
				return this.ActiveCount + this.BufferCount;
			}
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x001F4769 File Offset: 0x001F2969
		public TierSpec(int resolution, int activeCount, int bufferCount)
		{
			this.Resolution = resolution;
			this.ActiveCount = activeCount;
			this.BufferCount = bufferCount;
		}

		// Token: 0x04003F77 RID: 16247
		public readonly int Resolution;

		// Token: 0x04003F78 RID: 16248
		public readonly int ActiveCount;

		// Token: 0x04003F79 RID: 16249
		public readonly int BufferCount;
	}
}
