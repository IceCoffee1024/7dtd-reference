using System;

// Token: 0x020013A7 RID: 5031
public class WeatherPackage
{
	// Token: 0x06009E0F RID: 40463 RVA: 0x003BC5A0 File Offset: 0x003BA7A0
	public WeatherPackage()
	{
		this.param = new float[5];
	}

	// Token: 0x06009E10 RID: 40464 RVA: 0x003BC5B4 File Offset: 0x003BA7B4
	public void CopyTo(WeatherManager.BiomeWeather _bw)
	{
		_bw.remainingSeconds = this.remainingSeconds;
		for (int i = 0; i < this.param.Length; i++)
		{
			_bw.parameters[i].target = this.param[i];
		}
	}

	// Token: 0x06009E11 RID: 40465 RVA: 0x003BC5F8 File Offset: 0x003BA7F8
	public override string ToString()
	{
		string text = string.Format("id {0}, grp {1}, rsec {2}, params ", this.biomeId, this.groupIndex, this.remainingSeconds);
		for (int i = 0; i < this.param.Length; i++)
		{
			text += string.Format("{0}, ", this.param[i]);
		}
		return text;
	}

	// Token: 0x040077BF RID: 30655
	public byte biomeId;

	// Token: 0x040077C0 RID: 30656
	public byte groupIndex;

	// Token: 0x040077C1 RID: 30657
	public byte remainingSeconds;

	// Token: 0x040077C2 RID: 30658
	public float[] param;
}
