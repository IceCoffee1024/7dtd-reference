using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class QuestGeneratorController : MonoBehaviour
{
	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000627 RID: 1575 RVA: 0x0002BB8C File Offset: 0x00029D8C
	public bool IsRunning
	{
		get
		{
			return this.currentState == QuestGeneratorController.GeneratorStates.On;
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0002BB97 File Offset: 0x00029D97
	public void SetGeneratorState(QuestGeneratorController.GeneratorStates state, bool isInit)
	{
		if (state != this.currentState)
		{
			this.currentState = state;
			this.updateStateDisplay();
			if (!isInit)
			{
				PrefabInstance.RefreshTriggersInContainingPoi(base.transform.position + Origin.position);
			}
		}
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0002BBCC File Offset: 0x00029DCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateStateDisplay()
	{
		switch (this.currentState)
		{
		case QuestGeneratorController.GeneratorStates.OnNoQuest:
			this.OffState.SetActive(false);
			this.RebootState.SetActive(false);
			this.EnteringOnState.SetActive(false);
			this.OnState.SetActive(true);
			return;
		case QuestGeneratorController.GeneratorStates.Off:
			this.OffState.SetActive(true);
			this.RebootState.SetActive(false);
			this.EnteringOnState.SetActive(false);
			this.OnState.SetActive(false);
			return;
		case QuestGeneratorController.GeneratorStates.RebootState:
			this.OffState.SetActive(false);
			this.RebootState.SetActive(true);
			this.EnteringOnState.SetActive(false);
			this.OnState.SetActive(false);
			return;
		case QuestGeneratorController.GeneratorStates.EnteringOnState:
			this.OffState.SetActive(false);
			this.RebootState.SetActive(false);
			this.EnteringOnState.SetActive(true);
			this.OnState.SetActive(false);
			return;
		case QuestGeneratorController.GeneratorStates.On:
			this.OffState.SetActive(false);
			this.RebootState.SetActive(false);
			this.EnteringOnState.SetActive(false);
			this.OnState.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0002BCEF File Offset: 0x00029EEF
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.updateStateDisplay();
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x040006CE RID: 1742
	public Light MainLight;

	// Token: 0x040006CF RID: 1743
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public QuestGeneratorController.GeneratorStates currentState;

	// Token: 0x040006D0 RID: 1744
	public QuestGeneratorController.GeneratorStates TestingCurrentState = QuestGeneratorController.GeneratorStates.Off;

	// Token: 0x040006D1 RID: 1745
	public GameObject OffState;

	// Token: 0x040006D2 RID: 1746
	public GameObject RebootState;

	// Token: 0x040006D3 RID: 1747
	public GameObject EnteringOnState;

	// Token: 0x040006D4 RID: 1748
	public GameObject OnState;

	// Token: 0x020000F2 RID: 242
	public enum GeneratorStates
	{
		// Token: 0x040006D6 RID: 1750
		OnNoQuest,
		// Token: 0x040006D7 RID: 1751
		Off,
		// Token: 0x040006D8 RID: 1752
		RebootState,
		// Token: 0x040006D9 RID: 1753
		EnteringOnState,
		// Token: 0x040006DA RID: 1754
		On
	}
}
