using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AB5 RID: 2741
public sealed class BakeScheduler
{
	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x06005217 RID: 21015 RVA: 0x001F631D File Offset: 0x001F451D
	public BakeScheduler.BakeJobState CurrentState
	{
		get
		{
			return this._current.State;
		}
	}

	// Token: 0x06005218 RID: 21016 RVA: 0x001F632C File Offset: 0x001F452C
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

	// Token: 0x06005219 RID: 21017 RVA: 0x001F63A4 File Offset: 0x001F45A4
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

	// Token: 0x0600521A RID: 21018 RVA: 0x001F6420 File Offset: 0x001F4620
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

	// Token: 0x04003FAC RID: 16300
	public static readonly ProfilerMarker s_SignTextureManagerClear = new ProfilerMarker("SignTextureManager.BakeScheduler.Clear");

	// Token: 0x04003FAD RID: 16301
	public static readonly ProfilerMarker s_SignTextureManagerResetRequests = new ProfilerMarker("SignTextureManager.BakeScheduler.ResetRequests");

	// Token: 0x04003FAE RID: 16302
	public static readonly ProfilerMarker s_SignTextureManagerTickOnce = new ProfilerMarker("SignTextureManager.BakeScheduler.TickOnce");

	// Token: 0x04003FAF RID: 16303
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<double> s_BakeJobProgress = new ProfilerCounterValue<double>(ProfilerCategory.Scripts, "STM Job Progress", ProfilerMarkerDataUnit.Percent, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003FB0 RID: 16304
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounterValue<int> s_PendingBakeJobs = new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "STM Pending Jobs", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.FlushOnEndOfFrame);

	// Token: 0x04003FB1 RID: 16305
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<SignBakeRequest> _requests = new List<SignBakeRequest>(256);

	// Token: 0x04003FB2 RID: 16306
	[PublicizedFrom(EAccessModifier.Private)]
	public int _index;

	// Token: 0x04003FB3 RID: 16307
	[PublicizedFrom(EAccessModifier.Private)]
	public BakeScheduler.BakeJob _current;

	// Token: 0x02000AB6 RID: 2742
	public enum BakeJobState
	{
		// Token: 0x04003FB5 RID: 16309
		None,
		// Token: 0x04003FB6 RID: 16310
		Pending,
		// Token: 0x04003FB7 RID: 16311
		InProgress,
		// Token: 0x04003FB8 RID: 16312
		GeneratingMips,
		// Token: 0x04003FB9 RID: 16313
		Complete
	}

	// Token: 0x02000AB7 RID: 2743
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BakeJob
	{
		// Token: 0x0600521D RID: 21021 RVA: 0x001F687A File Offset: 0x001F4A7A
		public void Begin(SignBakeRequest request, RenderTexture rt)
		{
			this.Request = request;
			this.RT = rt;
			this.Step = 0;
			this.State = BakeScheduler.BakeJobState.Pending;
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x001F6898 File Offset: 0x001F4A98
		public void End()
		{
			this.RT = null;
			this.Step = 0;
			this.State = BakeScheduler.BakeJobState.None;
			this.Request = default(SignBakeRequest);
		}

		// Token: 0x04003FBA RID: 16314
		public SignBakeRequest Request;

		// Token: 0x04003FBB RID: 16315
		public RenderTexture RT;

		// Token: 0x04003FBC RID: 16316
		public int Step;

		// Token: 0x04003FBD RID: 16317
		public BakeScheduler.BakeJobState State;
	}
}
