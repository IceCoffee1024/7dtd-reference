using System;

// Token: 0x02001395 RID: 5013
public class WeatherPackage
{
	// Token: 0x06009DB7 RID: 40375 RVA: 0x003BA19C File Offset: 0x003B839C
	public WeatherPackage()
	{
		this.param = new float[5];
	}

	// Token: 0x06009DB8 RID: 40376 RVA: 0x003BA1B0 File Offset: 0x003B83B0
	public void CopyTo(WeatherManager.BiomeWeather _bw)
	{
		_bw.remainingSeconds = this.remainingSeconds;
		for (int i = 0; i < this.param.Length; i++)
		{
			_bw.parameters[i].target = this.param[i];
		}
	}

	// Token: 0x06009DB9 RID: 40377 RVA: 0x003BA1F4 File Offset: 0x003B83F4
	public override string ToString()
	{
		string text = string.Format("id {0}, grp {1}, rsec {2}, params ", this.biomeId, this.groupIndex, this.remainingSeconds);
		for (int i = 0; i < this.param.Length; i++)
		{
			text += string.Format("{0}, ", this.param[i]);
		}
		return text;
	}

	// Token: 0x0400777E RID: 30590
	public byte biomeId;

	// Token: 0x0400777F RID: 30591
	public byte groupIndex;

	// Token: 0x04007780 RID: 30592
	public byte remainingSeconds;

	// Token: 0x04007781 RID: 30593
	public float[] param;
}
