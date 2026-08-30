using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class QuestGeneratorController : MonoBehaviour
{
	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x0002B9D0 File Offset: 0x00029BD0
	public bool IsRunning
	{
		get
		{
			return this.currentState == QuestGeneratorController.GeneratorStates.On;
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0002B9DB File Offset: 0x00029BDB
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

	// Token: 0x06000622 RID: 1570 RVA: 0x0002BA10 File Offset: 0x00029C10
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

	// Token: 0x06000623 RID: 1571 RVA: 0x0002BB33 File Offset: 0x00029D33
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.updateStateDisplay();
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x040006CB RID: 1739
	public Light MainLight;

	// Token: 0x040006CC RID: 1740
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public QuestGeneratorController.GeneratorStates currentState;

	// Token: 0x040006CD RID: 1741
	public QuestGeneratorController.GeneratorStates TestingCurrentState = QuestGeneratorController.GeneratorStates.Off;

	// Token: 0x040006CE RID: 1742
	public GameObject OffState;

	// Token: 0x040006CF RID: 1743
	public GameObject RebootState;

	// Token: 0x040006D0 RID: 1744
	public GameObject EnteringOnState;

	// Token: 0x040006D1 RID: 1745
	public GameObject OnState;

	// Token: 0x020000F0 RID: 240
	public enum GeneratorStates
	{
		// Token: 0x040006D3 RID: 1747
		OnNoQuest,
		// Token: 0x040006D4 RID: 1748
		Off,
		// Token: 0x040006D5 RID: 1749
		RebootState,
		// Token: 0x040006D6 RID: 1750
		EnteringOnState,
		// Token: 0x040006D7 RID: 1751
		On
	}
}
