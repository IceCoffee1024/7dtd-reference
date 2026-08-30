using System;
using System.Diagnostics;
using UnityEngine.Scripting;

// Token: 0x0200101E RID: 4126
[Preserve]
public class XUiC_SaveIndicator : XUiController
{
	// Token: 0x06008270 RID: 33392 RVA: 0x0031808C File Offset: 0x0031628C
	public override void Init()
	{
		base.Init();
		this.m_tailTimer = new Stopwatch();
		this.m_saveDataManager = SaveDataUtils.SaveDataManager;
		this.m_saveDataManager.CommitStarted += this.OnCommitStarted;
		this.m_saveDataManager.CommitFinished += this.OnCommitFinished;
		this.xui.SaveIndicator = this;
	}

	// Token: 0x06008271 RID: 33393 RVA: 0x003180F0 File Offset: 0x003162F0
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

	// Token: 0x06008272 RID: 33394 RVA: 0x0031814C File Offset: 0x0031634C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitStarted()
	{
		this.m_commitInProgress = true;
	}

	// Token: 0x06008273 RID: 33395 RVA: 0x00318155 File Offset: 0x00316355
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitFinished()
	{
		this.m_tailTimer.Restart();
		this.m_commitInProgress = false;
	}

	// Token: 0x06008274 RID: 33396 RVA: 0x0031816C File Offset: 0x0031636C
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

	// Token: 0x06008275 RID: 33397 RVA: 0x003181D9 File Offset: 0x003163D9
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

	// Token: 0x04006098 RID: 24728
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan TailDuration = TimeSpan.FromSeconds(2.0);

	// Token: 0x04006099 RID: 24729
	[PublicizedFrom(EAccessModifier.Private)]
	public ISaveDataManager m_saveDataManager;

	// Token: 0x0400609A RID: 24730
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch m_tailTimer;

	// Token: 0x0400609B RID: 24731
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgress;

	// Token: 0x0400609C RID: 24732
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldShow;
}
