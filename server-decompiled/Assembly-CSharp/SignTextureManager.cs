using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000ABA RID: 2746
public sealed class SignTextureManager
{
	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x0600525F RID: 21087 RVA: 0x001F64F0 File Offset: 0x001F46F0
	public static SignTextureManager Instance
	{
		get
		{
			return SignTextureManager.m_instance;
		}
	}

	// Token: 0x06005260 RID: 21088 RVA: 0x001F64F8 File Offset: 0x001F46F8
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager()
	{
		this._prioritizer = new SignPrioritizer();
		this._store = new SignTextureStore();
		this._scheduler = new BakeScheduler();
	}

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x06005261 RID: 21089 RVA: 0x001F6581 File Offset: 0x001F4781
	public bool IsEnabled
	{
		get
		{
			return this._initialised;
		}
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x06005262 RID: 21090 RVA: 0x001F6589 File Offset: 0x001F4789
	public SignTextureManager.SignTextureQuality CurrentQuality
	{
		get
		{
			return this._qualityLevel;
		}
	}

	// Token: 0x06005263 RID: 21091 RVA: 0x001F6594 File Offset: 0x001F4794
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

	// Token: 0x06005264 RID: 21092 RVA: 0x001F663C File Offset: 0x001F483C
	public void SetTileSize(int tileSize)
	{
		if (tileSize > 0)
		{
			this._tileSize = tileSize;
		}
	}

	// Token: 0x06005265 RID: 21093 RVA: 0x001F6649 File Offset: 0x001F4849
	public void SetTileSizeForCurrentQuality()
	{
		if (this._activeTiers != null && this._activeTiers.Length != 0)
		{
			this._tileSize = this._activeTiers[0].Resolution;
		}
	}

	// Token: 0x06005266 RID: 21094 RVA: 0x001F6673 File Offset: 0x001F4873
	public void SetShowProgress(bool showProgress)
	{
		this._showProgress = showProgress;
	}

	// Token: 0x06005267 RID: 21095 RVA: 0x001F667C File Offset: 0x001F487C
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

	// Token: 0x06005268 RID: 21096 RVA: 0x001F6764 File Offset: 0x001F4964
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

	// Token: 0x06005269 RID: 21097 RVA: 0x001F6878 File Offset: 0x001F4A78
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

	// Token: 0x0600526A RID: 21098 RVA: 0x001F68B6 File Offset: 0x001F4AB6
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

	// Token: 0x0600526B RID: 21099 RVA: 0x001F68F0 File Offset: 0x001F4AF0
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

	// Token: 0x0600526C RID: 21100 RVA: 0x001F69E4 File Offset: 0x001F4BE4
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

	// Token: 0x0600526D RID: 21101 RVA: 0x001F6B04 File Offset: 0x001F4D04
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 GetPlayerPos()
	{
		return GameManager.Instance.World.GetPrimaryPlayer().position + new Vector3(0f, 1.5f, 0f);
	}

	// Token: 0x0600526E RID: 21102 RVA: 0x001F6B34 File Offset: 0x001F4D34
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

	// Token: 0x0600526F RID: 21103 RVA: 0x001F6B9C File Offset: 0x001F4D9C
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

	// Token: 0x06005270 RID: 21104 RVA: 0x001F6F96 File Offset: 0x001F5196
	[Conditional("SIGN_TEXTURE_DEBUG_LOG_INIT")]
	public static void DebugLog(string message)
	{
		Log.Out("[STM] " + message);
	}

	// Token: 0x06005272 RID: 21106 RVA: 0x001F7170 File Offset: 0x001F5370
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

	// Token: 0x04003FE2 RID: 16354
	[PublicizedFrom(EAccessModifier.Private)]
	public const string kShaderName = "Game/SignTech/UI";

	// Token: 0x04003FE3 RID: 16355
	public static readonly ProfilerMarker s_SignTextureManagerInitialize = new ProfilerMarker("SignTextureManager.Initialize");

	// Token: 0x04003FE4 RID: 16356
	public static readonly ProfilerMarker s_SignTextureManagerCleanup = new ProfilerMarker("SignTextureManager.Cleanup");

	// Token: 0x04003FE5 RID: 16357
	public static readonly ProfilerMarker s_SignTextureManagerInvalidate = new ProfilerMarker("SignTextureManager.Invalidate");

	// Token: 0x04003FE6 RID: 16358
	public static readonly ProfilerMarker s_SignTextureManagerMainThreadUpdate = new ProfilerMarker("SignTextureManager.MainThreadUpdate");

	// Token: 0x04003FE7 RID: 16359
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_RegisteredSigns = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "STM Registered Signs", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003FE8 RID: 16360
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager m_instance = new SignTextureManager();

	// Token: 0x04003FE9 RID: 16361
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersLowest = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(64, 32, 32),
		new SignTextureManager.TierSpec(128, 32, 16),
		new SignTextureManager.TierSpec(512, 8, 8)
	};

	// Token: 0x04003FEA RID: 16362
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersLow = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(128, 32, 32),
		new SignTextureManager.TierSpec(512, 8, 8),
		new SignTextureManager.TierSpec(1024, 3, 4)
	};

	// Token: 0x04003FEB RID: 16363
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersMedium = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(256, 32, 16),
		new SignTextureManager.TierSpec(512, 8, 4),
		new SignTextureManager.TierSpec(1024, 3, 4)
	};

	// Token: 0x04003FEC RID: 16364
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersHigh = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(256, 32, 16),
		new SignTextureManager.TierSpec(1024, 10, 6),
		new SignTextureManager.TierSpec(2048, 1, 1)
	};

	// Token: 0x04003FED RID: 16365
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureManager.TierSpec[] s_tiersUltra = new SignTextureManager.TierSpec[]
	{
		new SignTextureManager.TierSpec(512, 32, 16),
		new SignTextureManager.TierSpec(1024, 10, 6),
		new SignTextureManager.TierSpec(2048, 2, 2)
	};

	// Token: 0x04003FEE RID: 16366
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<SignCanvas> _canvases = new HashSet<SignCanvas>();

	// Token: 0x04003FEF RID: 16367
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignPrioritizer _prioritizer;

	// Token: 0x04003FF0 RID: 16368
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignTextureStore _store;

	// Token: 0x04003FF1 RID: 16369
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly BakeScheduler _scheduler;

	// Token: 0x04003FF2 RID: 16370
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignBakeRequest> _tmpRequests = new List<SignBakeRequest>(256);

	// Token: 0x04003FF3 RID: 16371
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader _shader;

	// Token: 0x04003FF4 RID: 16372
	[PublicizedFrom(EAccessModifier.Private)]
	public Material _material;

	// Token: 0x04003FF5 RID: 16373
	[PublicizedFrom(EAccessModifier.Private)]
	public CommandBuffer _cmd;

	// Token: 0x04003FF6 RID: 16374
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _initialised;

	// Token: 0x04003FF7 RID: 16375
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager.SignTextureQuality _qualityLevel = SignTextureManager.SignTextureQuality.Medium;

	// Token: 0x04003FF8 RID: 16376
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureManager.TierSpec[] _activeTiers = SignTextureManager.s_tiersMedium;

	// Token: 0x04003FF9 RID: 16377
	[PublicizedFrom(EAccessModifier.Private)]
	public int _tileSize = 256;

	// Token: 0x04003FFA RID: 16378
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _requestsDirty = true;

	// Token: 0x04003FFB RID: 16379
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 _lastUpdatePosition = Vector3.zero;

	// Token: 0x04003FFC RID: 16380
	[PublicizedFrom(EAccessModifier.Private)]
	public uint _framesSinceRefreshed;

	// Token: 0x04003FFD RID: 16381
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint kMinFramesSinceRefreshed = 10U;

	// Token: 0x04003FFE RID: 16382
	[PublicizedFrom(EAccessModifier.Private)]
	public const float kMinSqrDistanceSinceRefreshed = 1f;

	// Token: 0x04003FFF RID: 16383
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _showProgress;

	// Token: 0x04004000 RID: 16384
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder sb = new StringBuilder();

	// Token: 0x02000ABB RID: 2747
	public enum SignTextureQuality
	{
		// Token: 0x04004002 RID: 16386
		Lowest,
		// Token: 0x04004003 RID: 16387
		Low,
		// Token: 0x04004004 RID: 16388
		Medium,
		// Token: 0x04004005 RID: 16389
		High,
		// Token: 0x04004006 RID: 16390
		Ultra,
		// Token: 0x04004007 RID: 16391
		Infinite
	}

	// Token: 0x02000ABC RID: 2748
	public readonly struct TierSpec
	{
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06005273 RID: 21107 RVA: 0x001F71F2 File Offset: 0x001F53F2
		public int TotalCount
		{
			get
			{
				return this.ActiveCount + this.BufferCount;
			}
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x001F7201 File Offset: 0x001F5401
		public TierSpec(int resolution, int activeCount, int bufferCount)
		{
			this.Resolution = resolution;
			this.ActiveCount = activeCount;
			this.BufferCount = bufferCount;
		}

		// Token: 0x04004008 RID: 16392
		public readonly int Resolution;

		// Token: 0x04004009 RID: 16393
		public readonly int ActiveCount;

		// Token: 0x0400400A RID: 16394
		public readonly int BufferCount;
	}
}
