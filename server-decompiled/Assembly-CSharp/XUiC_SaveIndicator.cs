using System;
using System.Diagnostics;
using UnityEngine.Scripting;

// Token: 0x0200102D RID: 4141
[Preserve]
public class XUiC_SaveIndicator : XUiController
{
	// Token: 0x0600827F RID: 33407 RVA: 0x0031B0D0 File Offset: 0x003192D0
	public override void Init()
	{
		base.Init();
		this.m_tailTimer = new Stopwatch();
		this.m_saveDataManager = SaveDataUtils.SaveDataManager;
		this.m_saveDataManager.CommitStarted += this.OnCommitStarted;
		this.m_saveDataManager.CommitFinished += this.OnCommitFinished;
		this.xui.SaveIndicator = this;
	}

	// Token: 0x06008280 RID: 33408 RVA: 0x0031B134 File Offset: 0x00319334
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

	// Token: 0x06008281 RID: 33409 RVA: 0x0031B190 File Offset: 0x00319390
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitStarted()
	{
		this.m_commitInProgress = true;
	}

	// Token: 0x06008282 RID: 33410 RVA: 0x0031B199 File Offset: 0x00319399
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitFinished()
	{
		this.m_tailTimer.Restart();
		this.m_commitInProgress = false;
	}

	// Token: 0x06008283 RID: 33411 RVA: 0x0031B1B0 File Offset: 0x003193B0
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

	// Token: 0x06008284 RID: 33412 RVA: 0x0031B21D File Offset: 0x0031941D
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

	// Token: 0x040060C1 RID: 24769
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan TailDuration = TimeSpan.FromSeconds(2.0);

	// Token: 0x040060C2 RID: 24770
	[PublicizedFrom(EAccessModifier.Private)]
	public ISaveDataManager m_saveDataManager;

	// Token: 0x040060C3 RID: 24771
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch m_tailTimer;

	// Token: 0x040060C4 RID: 24772
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgress;

	// Token: 0x040060C5 RID: 24773
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldShow;
}
