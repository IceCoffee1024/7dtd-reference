using System;

// Token: 0x020000E7 RID: 231
public class GorePrefab : RootTransformRefEntity
{
	// Token: 0x17000062 RID: 98
	// (set) Token: 0x060005D3 RID: 1491 RVA: 0x00029FA9 File Offset: 0x000281A9
	public bool restoreState
	{
		set
		{
			this._restoreState = value;
		}
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00029FB4 File Offset: 0x000281B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (!this._restoreState && this.RootTransform != null && this.Sound != null && this.Sound != string.Empty)
		{
			this.RootTransform.GetComponent<Entity>().PlayOneShot(this.Sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x04000684 RID: 1668
	public string Sound;

	// Token: 0x04000685 RID: 1669
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _restoreState;
}
