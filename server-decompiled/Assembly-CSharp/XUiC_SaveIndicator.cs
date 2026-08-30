using System;
using System.Diagnostics;
using UnityEngine.Scripting;

// Token: 0x02001005 RID: 4101
[Preserve]
public class XUiC_SaveIndicator : XUiController
{
	// Token: 0x0600817E RID: 33150 RVA: 0x00313864 File Offset: 0x00311A64
	public override void Init()
	{
		base.Init();
		this.m_tailTimer = new Stopwatch();
		this.m_saveDataManager = SaveDataUtils.SaveDataManager;
		this.m_saveDataManager.CommitStarted += this.OnCommitStarted;
		this.m_saveDataManager.CommitFinished += this.OnCommitFinished;
		this.xui.SaveIndicator = this;
	}

	// Token: 0x0600817F RID: 33151 RVA: 0x003138C8 File Offset: 0x00311AC8
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.m_saveDataManager != null)
		{
			this.m_saveDataManager.CommitStarted -= this.OnCommitStarted;
			this.m_saveDataManager.CommitFinished -= this.OnCommitFinished;
			this.m_saveDataManager = null;
		}
		this.xui.SaveIndicator = null;
	}

	// Token: 0x06008180 RID: 33152 RVA: 0x00313924 File Offset: 0x00311B24
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitStarted()
	{
		this.m_commitInProgress = true;
	}

	// Token: 0x06008181 RID: 33153 RVA: 0x0031392D File Offset: 0x00311B2D
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitFinished()
	{
		this.m_tailTimer.Restart();
		this.m_commitInProgress = false;
	}

	// Token: 0x06008182 RID: 33154 RVA: 0x00313944 File Offset: 0x00311B44
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!this.m_commitInProgress && this.m_tailTimer.IsRunning && this.m_tailTimer.Elapsed >= XUiC_SaveIndicator.TailDuration)
		{
			this.m_tailTimer.Stop();
		}
		this.shouldShow = this.m_commitInProgress || this.m_tailTimer.IsRunning;
		base.RefreshBindings();
	}

	// Token: 0x06008183 RID: 33155 RVA: 0x003139B1 File Offset: 0x00311BB1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "isSaving")
		{
			_value = this.shouldShow.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x04005FD1 RID: 24529
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan TailDuration = TimeSpan.FromSeconds(2.0);

	// Token: 0x04005FD2 RID: 24530
	[PublicizedFrom(EAccessModifier.Private)]
	public ISaveDataManager m_saveDataManager;

	// Token: 0x04005FD3 RID: 24531
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch m_tailTimer;

	// Token: 0x04005FD4 RID: 24532
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgress;

	// Token: 0x04005FD5 RID: 24533
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldShow;
}
