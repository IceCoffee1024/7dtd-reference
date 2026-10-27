using System;
using System.Diagnostics;
using UnityEngine.Scripting;

// Token: 0x0200103D RID: 4157
[Preserve]
public class XUiC_SaveIndicator : XUiController
{
	// Token: 0x060082D1 RID: 33489 RVA: 0x0031CDD4 File Offset: 0x0031AFD4
	public override void Init()
	{
		base.Init();
		this.m_tailTimer = new Stopwatch();
		this.m_saveDataManager = SaveDataUtils.SaveDataManager;
		this.m_saveDataManager.CommitStarted += this.OnCommitStarted;
		this.m_saveDataManager.CommitFinished += this.OnCommitFinished;
		this.xui.SaveIndicator = this;
	}

	// Token: 0x060082D2 RID: 33490 RVA: 0x0031CE38 File Offset: 0x0031B038
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

	// Token: 0x060082D3 RID: 33491 RVA: 0x0031CE94 File Offset: 0x0031B094
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitStarted()
	{
		this.m_commitInProgress = true;
	}

	// Token: 0x060082D4 RID: 33492 RVA: 0x0031CE9D File Offset: 0x0031B09D
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitFinished()
	{
		this.m_tailTimer.Restart();
		this.m_commitInProgress = false;
	}

	// Token: 0x060082D5 RID: 33493 RVA: 0x0031CEB4 File Offset: 0x0031B0B4
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

	// Token: 0x060082D6 RID: 33494 RVA: 0x0031CF21 File Offset: 0x0031B121
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

	// Token: 0x040060FF RID: 24831
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan TailDuration = TimeSpan.FromSeconds(2.0);

	// Token: 0x04006100 RID: 24832
	[PublicizedFrom(EAccessModifier.Private)]
	public ISaveDataManager m_saveDataManager;

	// Token: 0x04006101 RID: 24833
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch m_tailTimer;

	// Token: 0x04006102 RID: 24834
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgress;

	// Token: 0x04006103 RID: 24835
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldShow;
}
