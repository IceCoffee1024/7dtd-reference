using System;

// Token: 0x020000E5 RID: 229
public class GorePrefab : RootTransformRefEntity
{
	// Token: 0x17000062 RID: 98
	// (set) Token: 0x060005CC RID: 1484 RVA: 0x00029DED File Offset: 0x00027FED
	public bool restoreState
	{
		set
		{
			this._restoreState = value;
		}
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00029DF8 File Offset: 0x00027FF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (!this._restoreState && this.RootTransform != null && this.Sound != null && this.Sound != string.Empty)
		{
			this.RootTransform.GetComponent<Entity>().PlayOneShot(this.Sound, false, false, false, null, 1f);
		}
	}

	// Token: 0x04000681 RID: 1665
	public string Sound;

	// Token: 0x04000682 RID: 1666
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _restoreState;
}
