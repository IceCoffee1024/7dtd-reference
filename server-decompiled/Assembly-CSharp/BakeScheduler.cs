using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AD8 RID: 2776
public sealed class BakeScheduler
{
	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x060052B6 RID: 21174 RVA: 0x001FBFBD File Offset: 0x001FA1BD
	public BakeScheduler.BakeJobState CurrentState
	{
		get
		{
			return this._current.State;
		}
	}

	// Token: 0x060052B7 RID: 21175 RVA: 0x001FBFCC File Offset: 0x001FA1CC
	public void Clear()
	{
		using (BakeScheduler.s_SignTextureManagerClear.Auto())
		{
			this._requests.Clear();
			this._index = 0;
			this._current.End();
			BakeScheduler.s_PendingBakeJobs.Value = 0;
			BakeScheduler.s_BakeJobProgress.Value = 0.0;
		}
	}

	// Token: 0x060052B8 RID: 21176 RVA: 0x001FC044 File Offset: 0x001FA244
	public void ResetRequests(List<SignBakeRequest> newRequests)
	{
		using (BakeScheduler.s_SignTextureManagerResetRequests.Auto())
		{
			this._requests.Clear();
			for (int i = 0; i < newRequests.Count; i++)
			{
				this._requests.Add(newRequests[i]);
			}
			this._index = 0;
			this._current.End();
		}
	}

	// Token: 0x060052B9 RID: 21177 RVA: 0x001FC0C0 File Offset: 0x001FA2C0
	public bool TickOnce(int tileSize, Material material, CommandBuffer cmd, SignTextureStore store, SignPrioritizer prioritizer, bool showProgress = false)
	{
		bool flag;
		using (BakeScheduler.s_SignTextureManagerTickOnce.Auto())
		{
			BakeScheduler.s_PendingBakeJobs.Value = this._requests.Count - this._index;
			if (this._current.State == BakeScheduler.BakeJobState.None)
			{
				while (this._index < this._requests.Count)
				{
					List<SignBakeRequest> requests = this._requests;
					int index = this._index;
					this._index = index + 1;
					SignBakeRequest signBakeRequest = requests[index];
					RenderTexture renderTexture = store.AcquireForBake(signBakeRequest.Tier);
					if (!(renderTexture == null))
					{
						this._current.Begin(signBakeRequest, renderTexture);
						break;
					}
					Log.Warning(string.Format("[STM] Pool exhausted at tier {0}; skipping requested bake.", signBakeRequest.Tier));
				}
				if (this._current.State == BakeScheduler.BakeJobState.None)
				{
					BakeScheduler.s_BakeJobProgress.Value = 0.0;
					return false;
				}
			}
			if (this._current.RT == null)
			{
				this._current.End();
				flag = true;
			}
			else
			{
				RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(this._current.RT);
				cmd.Clear();
				if (this._current.State == BakeScheduler.BakeJobState.Pending)
				{
					GlobalSignId groupSignId = prioritizer.GetGroupSignId(this._current.Request.GroupIndex);
					if (!SignDataManager.Instance.TryApplyRenderingData(groupSignId, 1f, material, null, SignUIStyle.Baked))
					{
						Debug.LogError(string.Format("Failed to retrieve rendering data for sign ID: {0}", groupSignId));
						if (!SignDataManager.Instance.TryApplyRenderingData(SignDataManager.ErrorSignId, 1f, material, null, SignUIStyle.Baked))
						{
							Debug.LogError("Failed to retrieve fallback error sign data.");
							cmd.SetRenderTarget(renderTargetIdentifier);
							cmd.ClearRenderTarget(true, true, Color.magenta);
							Graphics.ExecuteCommandBuffer(cmd);
							cmd.Clear();
							this._current.State = BakeScheduler.BakeJobState.Complete;
							return true;
						}
					}
					material.SetVector(SignShaderIDs._CanvasAspect, Vector2.one);
					cmd.SetRenderTarget(renderTargetIdentifier);
					cmd.ClearRenderTarget(true, true, Color.clear);
					if (showProgress)
					{
						prioritizer.ApplyToGroupCanvases(this._current.Request.GroupIndex, this._current.RT);
					}
					this._current.State = BakeScheduler.BakeJobState.InProgress;
				}
				if (this._current.State == BakeScheduler.BakeJobState.InProgress)
				{
					int num = Mathf.Max(this._current.RT.width / tileSize, 1);
					int num2 = num * num;
					int num3 = this._current.Step % num;
					int num4 = this._current.Step / num;
					Vector2 vector = Vector2.one / (float)num * 2f;
					Vector2 vector2 = new Vector2(-1f + (float)num3 * vector.x, -1f + (float)num4 * vector.y);
					material.SetVector(SignShaderIDs._MinUV, vector2);
					material.SetVector(SignShaderIDs._MaxUV, vector2 + vector);
					cmd.Blit(BuiltinRenderTextureType.None, renderTargetIdentifier, material, 0);
					Graphics.ExecuteCommandBuffer(cmd);
					this._current.Step = this._current.Step + 1;
					BakeScheduler.s_BakeJobProgress.Value = (double)(100f * (float)this._current.Step / (float)num2);
					if (this._current.Step >= num2)
					{
						this._current.State = BakeScheduler.BakeJobState.Complete;
					}
					flag = true;
				}
				else if (this._current.State == BakeScheduler.BakeJobState.Complete)
				{
					GlobalSignId groupSignId2 = prioritizer.GetGroupSignId(this._current.Request.GroupIndex);
					store.SetBaked(groupSignId2, this._current.Request.Tier, this._current.RT);
					prioritizer.ApplyToGroupCanvases(this._current.Request.GroupIndex, this._current.RT);
					this._current.End();
					flag = true;
				}
				else
				{
					Log.Error(string.Format("Unexpected case in SignTextureManager: TickOnce reached failsafe return case. Current job state is: {0}.", this._current.State));
					this._current.State = BakeScheduler.BakeJobState.None;
					flag = false;
				}
			}
		}
		return flag;
	}

	// Token: 0x0400407A RID: 16506
	public static readonly ProfilerMarker s_SignTextureManagerClear = new ProfilerMarker("SignTextureManager.BakeScheduler.Clear");

	// Token: 0x0400407B RID: 16507
	public static readonly ProfilerMarker s_SignTextureManagerResetRequests = new ProfilerMarker("SignTextureManager.BakeScheduler.ResetRequests");

	// Token: 0x0400407C RID: 16508
	public static readonly ProfilerMarker s_SignTextureManagerTickOnce = new ProfilerMarker("SignTextureManager.BakeScheduler.TickOnce");

	// Token: 0x0400407D RID: 16509
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<double> s_BakeJobProgress = new ProfilerCounterValue<double>(ProfilerCategory.Scripts, "STM Job Progress", ProfilerMarkerDataUnit.Percent, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x0400407E RID: 16510
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_PendingBakeJobs = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "STM Pending Jobs", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x0400407F RID: 16511
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignBakeRequest> _requests = new List<SignBakeRequest>(256);

	// Token: 0x04004080 RID: 16512
	[PublicizedFrom(EAccessModifier.Private)]
	public int _index;

	// Token: 0x04004081 RID: 16513
	[PublicizedFrom(EAccessModifier.Private)]
	public BakeScheduler.BakeJob _current;

	// Token: 0x02000AD9 RID: 2777
	public enum BakeJobState
	{
		// Token: 0x04004083 RID: 16515
		None,
		// Token: 0x04004084 RID: 16516
		Pending,
		// Token: 0x04004085 RID: 16517
		InProgress,
		// Token: 0x04004086 RID: 16518
		GeneratingMips,
		// Token: 0x04004087 RID: 16519
		Complete
	}

	// Token: 0x02000ADA RID: 2778
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BakeJob
	{
		// Token: 0x060052BC RID: 21180 RVA: 0x001FC572 File Offset: 0x001FA772
		public void Begin(SignBakeRequest request, RenderTexture rt)
		{
			this.Request = request;
			this.RT = rt;
			this.Step = 0;
			this.State = BakeScheduler.BakeJobState.Pending;
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x001FC590 File Offset: 0x001FA790
		public void End()
		{
			this.RT = null;
			this.Step = 0;
			this.State = BakeScheduler.BakeJobState.None;
			this.Request = default(SignBakeRequest);
		}

		// Token: 0x04004088 RID: 16520
		public SignBakeRequest Request;

		// Token: 0x04004089 RID: 16521
		public RenderTexture RT;

		// Token: 0x0400408A RID: 16522
		public int Step;

		// Token: 0x0400408B RID: 16523
		public BakeScheduler.BakeJobState State;
	}
}
