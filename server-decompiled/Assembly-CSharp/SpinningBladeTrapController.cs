using System;
using System.Globalization;
using Audio;
using UnityEngine;

// Token: 0x020003DB RID: 987
public class SpinningBladeTrapController : MonoBehaviour
{
	// Token: 0x17000391 RID: 913
	// (get) Token: 0x06001DBF RID: 7615 RVA: 0x000B520B File Offset: 0x000B340B
	// (set) Token: 0x06001DC0 RID: 7616 RVA: 0x000B5213 File Offset: 0x000B3413
	public float HealthRatio
	{
		get
		{
			return this.healthRatio;
		}
		set
		{
			this.lastHealthRatio = this.healthRatio;
			this.healthRatio = value;
			if (this.healthRatio != this.lastHealthRatio)
			{
				this.CheckHealthChanged();
			}
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x000B523C File Offset: 0x000B343C
	public float CurrentSpeedRatio
	{
		get
		{
			return this.windUpDownTime / this.windUpTimeMax * (this.windUpDownTime / this.windUpTimeMax);
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x000B5259 File Offset: 0x000B3459
	// (set) Token: 0x06001DC3 RID: 7619 RVA: 0x000B5261 File Offset: 0x000B3461
	public bool IsOn
	{
		get
		{
			return this.isOn;
		}
		set
		{
			this.lastIsOn = this.isOn;
			this.isOn = value;
			this.BladeController.IsOn = value;
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x000B5282 File Offset: 0x000B3482
	// (set) Token: 0x06001DC5 RID: 7621 RVA: 0x000B528A File Offset: 0x000B348A
	public SpinningBladeTrapController.BladeTrapStates CurrentState
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentState;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.EnterState(this.currentState, value);
			this.currentState = value;
		}
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x000B52A0 File Offset: 0x000B34A0
	public void Init(DynamicProperties _properties, Block _block)
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.breakingPercentage = 0.5f;
		if (_properties.Values.ContainsKey("BreakingPercentage"))
		{
			this.breakingPercentage = Mathf.Clamp01(StringParsers.ParseFloat(_properties.Values["BreakingPercentage"], 0, -1, NumberStyles.Any));
		}
		this.brokenPercentage = 0.25f;
		if (_properties.Values.ContainsKey("BrokenPercentage"))
		{
			this.brokenPercentage = Mathf.Clamp01(StringParsers.ParseFloat(_properties.Values["BrokenPercentage"], 0, -1, NumberStyles.Any));
		}
		if (_properties.Values.ContainsKey("StartSound"))
		{
			this.startSound = _properties.Values["StartSound"];
		}
		if (_properties.Values.ContainsKey("StopSound"))
		{
			this.stopSound = _properties.Values["StopSound"];
		}
		if (_properties.Values.ContainsKey("RunningSound"))
		{
			this.runningSound = _properties.Values["RunningSound"];
		}
		if (_properties.Values.ContainsKey("RunningSoundBreaking"))
		{
			this.runningSoundPartlyBroken = _properties.Values["RunningSoundBreaking"];
		}
		if (_properties.Values.ContainsKey("RunningSoundBroken"))
		{
			this.runningSoundBroken = _properties.Values["RunningSoundBroken"];
		}
		if (this.BladeController != null)
		{
			this.BladeController.Init(_properties, _block);
		}
		this.randomStartDelayMax = GameManager.Instance.World.GetGameRandom().RandomFloat * this.randomStartDelayMax;
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x000B544C File Offset: 0x000B364C
	public void DamageSelf(float damage)
	{
		this.totalDamage += damage;
		if (this.totalDamage < 1f)
		{
			return;
		}
		damage = (float)((int)this.totalDamage);
		this.totalDamage = 0f;
		if (this.chunk == null)
		{
			this.chunk = (Chunk)GameManager.Instance.World.GetChunkFromWorldPos(this.BlockPosition);
		}
		BlockValue block = GameManager.Instance.World.GetBlock(this.BlockPosition);
		this.HealthRatio = 1f - (float)block.damage / (float)block.Block.MaxDamage;
		block.damage = Mathf.Clamp(block.damage + (int)damage, 0, block.Block.MaxDamage);
		GameManager.Instance.World.SetBlock(this.BlockPosition, block, false, false);
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000B5524 File Offset: 0x000B3724
	public void StopAllSounds()
	{
		Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSound);
		Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSoundPartlyBroken);
		Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSoundBroken);
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000B5574 File Offset: 0x000B3774
	[PublicizedFrom(EAccessModifier.Private)]
	public void EnterState(SpinningBladeTrapController.BladeTrapStates oldState, SpinningBladeTrapController.BladeTrapStates newState)
	{
		switch (newState)
		{
		case SpinningBladeTrapController.BladeTrapStates.IsOff:
			this.StopAllSounds();
			break;
		case SpinningBladeTrapController.BladeTrapStates.RandomWaitToStart:
			this.randomStartDelay = 0f;
			return;
		case SpinningBladeTrapController.BladeTrapStates.IsStarting:
			this.StopAllSounds();
			Manager.BroadcastPlay(this.BlockPosition.ToVector3(), this.startSound, 0f);
			return;
		case SpinningBladeTrapController.BladeTrapStates.IsOn:
			Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSoundPartlyBroken);
			Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSoundBroken);
			Manager.BroadcastPlay(this.BlockPosition.ToVector3(), this.runningSound, 0f);
			this.degreesPerSecond = this.degreesPerSecondMax;
			return;
		case SpinningBladeTrapController.BladeTrapStates.IsOnPartlyBroken:
			Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSound);
			Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSoundBroken);
			Manager.BroadcastPlay(this.BlockPosition.ToVector3(), this.runningSoundPartlyBroken, 0f);
			this.degreesPerSecond = this.degreesPerSecondMax;
			return;
		case SpinningBladeTrapController.BladeTrapStates.IsOnBroken:
			Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSound);
			Manager.BroadcastStop(this.BlockPosition.ToVector3(), this.runningSoundPartlyBroken);
			Manager.BroadcastPlay(this.BlockPosition.ToVector3(), this.runningSoundBroken, 0f);
			this.degreesPerSecond = this.degreesPerSecondMax;
			return;
		case SpinningBladeTrapController.BladeTrapStates.IsStopping:
			this.StopAllSounds();
			Manager.BroadcastPlay(this.BlockPosition.ToVector3(), this.stopSound, 0f);
			this.windUpDownTime = this.windDownTimeMax;
			if (this.HealthRatio <= this.brokenPercentage)
			{
				this.HandleParticlesForBroken();
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x000B5718 File Offset: 0x000B3918
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		BlockValue block = GameManager.Instance.World.GetBlock(this.BlockPosition);
		if (block.isair)
		{
			return;
		}
		this.HealthRatio = 1f - (float)block.damage / (float)block.Block.MaxDamage;
		if (this.lastIsOn)
		{
			bool flag = !this.isOn;
		}
		if (this.isOn)
		{
			bool flag2 = !this.lastIsOn;
		}
		SpinningBladeTrapController.BladeTrapStates initialState = this.GetInitialState();
		if (initialState != this.currentState)
		{
			this.CurrentState = initialState;
			return;
		}
		switch (this.currentState)
		{
		case SpinningBladeTrapController.BladeTrapStates.IsOff:
			if (this.IsOn && this.HealthRatio >= this.brokenPercentage)
			{
				this.CurrentState = SpinningBladeTrapController.BladeTrapStates.RandomWaitToStart;
			}
			break;
		case SpinningBladeTrapController.BladeTrapStates.RandomWaitToStart:
			if (this.randomStartDelay < this.randomStartDelayMax)
			{
				this.randomStartDelay += Time.deltaTime;
			}
			else
			{
				this.windUpDownTime = 0f;
				this.CurrentState = SpinningBladeTrapController.BladeTrapStates.IsStarting;
			}
			break;
		case SpinningBladeTrapController.BladeTrapStates.IsStarting:
			if (this.degreesPerSecond < this.degreesPerSecondMax)
			{
				if (this.HealthRatio > this.breakingPercentage)
				{
					this.degreesPerSecond = Mathf.Lerp(0f, this.degreesPerSecondMax, this.CurrentSpeedRatio);
				}
				else
				{
					this.degreesPerSecond = Mathf.Lerp(0f, this.degreesPerSecondMax * (Mathf.Clamp(this.HealthRatio, 0f, this.breakingPercentage) * 2f), this.CurrentSpeedRatio);
					if (this.HealthRatio <= this.brokenPercentage)
					{
						this.degreesPerSecond = 0f;
					}
				}
			}
			this.windUpDownTime += Time.deltaTime;
			this.windUpDownTime = Mathf.Clamp(this.windUpDownTime, 0f, this.windUpTimeMax);
			if (this.degreesPerSecond == this.degreesPerSecondMax)
			{
				this.CheckHealthChanged();
			}
			if (!this.isOn)
			{
				this.CurrentState = SpinningBladeTrapController.BladeTrapStates.IsStopping;
			}
			break;
		case SpinningBladeTrapController.BladeTrapStates.IsOn:
		case SpinningBladeTrapController.BladeTrapStates.IsOnPartlyBroken:
		case SpinningBladeTrapController.BladeTrapStates.IsOnBroken:
			if (this.degreesPerSecond < this.degreesPerSecondMax)
			{
				if (this.HealthRatio > this.breakingPercentage)
				{
					this.degreesPerSecond = Mathf.Lerp(0f, this.degreesPerSecondMax, this.CurrentSpeedRatio);
				}
				else
				{
					this.degreesPerSecond = Mathf.Lerp(0f, this.degreesPerSecondMax * (Mathf.Clamp(this.HealthRatio, 0f, this.breakingPercentage) * 2f), this.CurrentSpeedRatio);
					if (this.HealthRatio <= this.brokenPercentage)
					{
						this.degreesPerSecond = 0f;
					}
				}
			}
			if (!this.isOn)
			{
				this.CurrentState = SpinningBladeTrapController.BladeTrapStates.IsStopping;
			}
			break;
		case SpinningBladeTrapController.BladeTrapStates.IsStopping:
			if (this.degreesPerSecond > 0f)
			{
				this.degreesPerSecond = Mathf.Lerp(0f, this.degreesPerSecond, this.CurrentSpeedRatio);
			}
			this.windUpDownTime -= Time.deltaTime;
			this.windUpDownTime = Mathf.Clamp(this.windUpDownTime, 0f, this.windDownTimeMax);
			this.degreesPerSecond = Mathf.Lerp(0f, this.degreesPerSecond, this.CurrentSpeedRatio);
			if (this.windUpDownTime <= 0f)
			{
				this.CurrentState = SpinningBladeTrapController.BladeTrapStates.IsOff;
			}
			if (this.IsOn && this.HealthRatio > this.brokenPercentage)
			{
				this.CurrentState = SpinningBladeTrapController.BladeTrapStates.IsStarting;
			}
			break;
		}
		float num = this.BladeControllerTransform.localRotation.eulerAngles.y - this.degreesPerSecond * Time.deltaTime;
		float num2 = Utils.FastLerp(-15f, 0f, Utils.FastClamp(this.HealthRatio, 0f, this.breakingPercentage) * 2f);
		this.BladeControllerTransform.localRotation = Quaternion.Euler(num2, num, 0f);
		this.BladeBottomTransform.localRotation = Quaternion.Euler(0f, num, 0f);
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x000B5AF0 File Offset: 0x000B3CF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckHealthChanged()
	{
		if (this.CurrentState == SpinningBladeTrapController.BladeTrapStates.IsStarting || this.CurrentState == SpinningBladeTrapController.BladeTrapStates.IsOn || this.currentState == SpinningBladeTrapController.BladeTrapStates.IsOnPartlyBroken || this.currentState == SpinningBladeTrapController.BladeTrapStates.IsOnBroken)
		{
			SpinningBladeTrapController.BladeTrapStates stateByHealthRange = this.GetStateByHealthRange();
			if (stateByHealthRange != this.currentState)
			{
				this.CurrentState = stateByHealthRange;
			}
		}
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x000B5B38 File Offset: 0x000B3D38
	[PublicizedFrom(EAccessModifier.Private)]
	public SpinningBladeTrapController.BladeTrapStates GetInitialState()
	{
		if (this.isOn)
		{
			if (this.HealthRatio >= 0.75f)
			{
				return SpinningBladeTrapController.BladeTrapStates.IsOn;
			}
			if (this.HealthRatio >= this.breakingPercentage)
			{
				return SpinningBladeTrapController.BladeTrapStates.IsOnPartlyBroken;
			}
			if (this.HealthRatio > this.brokenPercentage)
			{
				return SpinningBladeTrapController.BladeTrapStates.IsOnBroken;
			}
		}
		return this.currentState;
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x000B5B78 File Offset: 0x000B3D78
	[PublicizedFrom(EAccessModifier.Private)]
	public SpinningBladeTrapController.BladeTrapStates GetStateByHealthRange()
	{
		if (!this.isOn)
		{
			return this.currentState;
		}
		if (this.HealthRatio >= 0.75f)
		{
			return SpinningBladeTrapController.BladeTrapStates.IsOn;
		}
		if (this.HealthRatio >= this.breakingPercentage)
		{
			return SpinningBladeTrapController.BladeTrapStates.IsOnPartlyBroken;
		}
		if (this.HealthRatio > this.brokenPercentage)
		{
			return SpinningBladeTrapController.BladeTrapStates.IsOnBroken;
		}
		if (this.currentState == SpinningBladeTrapController.BladeTrapStates.IsOff)
		{
			return SpinningBladeTrapController.BladeTrapStates.IsOff;
		}
		return SpinningBladeTrapController.BladeTrapStates.IsStopping;
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x000B5BD0 File Offset: 0x000B3DD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleParticlesForBroken()
	{
		float num = GameManager.Instance.World.GetLightBrightness(World.worldToBlockPos(this.BlockPosition.ToVector3())) / 2f;
		ParticleEffect particleEffect = new ParticleEffect("big_smoke", new Vector3(0f, 0.25f, 0f), num, new Color(1f, 1f, 1f, 0.3f), null, base.transform, false, 1f, "");
		GameManager.Instance.SpawnParticleEffectServer(particleEffect, -1, false, false);
		ParticleEffect particleEffect2 = new ParticleEffect("electric_fence_sparks", new Vector3(0f, 0.25f, 0f), num, new Color(1f, 1f, 1f, 0.3f), "electric_fence_impact", base.transform, false, 1f, "");
		GameManager.Instance.SpawnParticleEffectServer(particleEffect2, -1, false, false);
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x000B5CB9 File Offset: 0x000B3EB9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.Cleanup();
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x000B5CC1 File Offset: 0x000B3EC1
	public void Cleanup()
	{
		this.StopAllSounds();
		this.IsOn = false;
		this.lastIsOn = false;
		this.currentState = SpinningBladeTrapController.BladeTrapStates.IsOff;
		this.degreesPerSecond = 0f;
		this.windUpDownTime = 0f;
		this.initialized = false;
	}

	// Token: 0x040013C7 RID: 5063
	public Transform BladeControllerTransform;

	// Token: 0x040013C8 RID: 5064
	public Transform BladeBottomTransform;

	// Token: 0x040013C9 RID: 5065
	public SpinningBladeTrapBladeController BladeController;

	// Token: 0x040013CA RID: 5066
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastHealthRatio;

	// Token: 0x040013CB RID: 5067
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float healthRatio = 1f;

	// Token: 0x040013CC RID: 5068
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lastIsOn;

	// Token: 0x040013CD RID: 5069
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isOn;

	// Token: 0x040013CE RID: 5070
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecondMax = 720f;

	// Token: 0x040013CF RID: 5071
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond;

	// Token: 0x040013D0 RID: 5072
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float windUpTimeMax = 5f;

	// Token: 0x040013D1 RID: 5073
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float windDownTimeMax = 7.5f;

	// Token: 0x040013D2 RID: 5074
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float windUpDownTime;

	// Token: 0x040013D3 RID: 5075
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float randomStartDelayMax = 0.5f;

	// Token: 0x040013D4 RID: 5076
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float randomStartDelay;

	// Token: 0x040013D5 RID: 5077
	public Vector3i BlockPosition;

	// Token: 0x040013D6 RID: 5078
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Chunk chunk;

	// Token: 0x040013D7 RID: 5079
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string startSound = "Electricity/BladeTrap/bladetrap_startup";

	// Token: 0x040013D8 RID: 5080
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string stopSound = "Electricity/BladeTrap/bladetrap_stop";

	// Token: 0x040013D9 RID: 5081
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string runningSound = "Electricity/BladeTrap/bladetrap_fire_lp";

	// Token: 0x040013DA RID: 5082
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string runningSoundPartlyBroken = "Electricity/BladeTrap/bladetrap_dm1_lp";

	// Token: 0x040013DB RID: 5083
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string runningSoundBroken = "Electricity/BladeTrap/bladetrap_dm2_lp";

	// Token: 0x040013DC RID: 5084
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initialized;

	// Token: 0x040013DD RID: 5085
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float brokenPercentage;

	// Token: 0x040013DE RID: 5086
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float breakingPercentage;

	// Token: 0x040013DF RID: 5087
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SpinningBladeTrapController.BladeTrapStates currentState;

	// Token: 0x040013E0 RID: 5088
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float totalDamage;

	// Token: 0x040013E1 RID: 5089
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string currentRunningSound;

	// Token: 0x020003DC RID: 988
	[PublicizedFrom(EAccessModifier.Private)]
	public enum BladeTrapStates
	{
		// Token: 0x040013E3 RID: 5091
		IsOff,
		// Token: 0x040013E4 RID: 5092
		RandomWaitToStart,
		// Token: 0x040013E5 RID: 5093
		IsStarting,
		// Token: 0x040013E6 RID: 5094
		IsOn,
		// Token: 0x040013E7 RID: 5095
		IsOnPartlyBroken,
		// Token: 0x040013E8 RID: 5096
		IsOnBroken,
		// Token: 0x040013E9 RID: 5097
		IsStopping
	}
}
