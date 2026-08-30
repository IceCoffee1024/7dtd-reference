using System;

// Token: 0x02001375 RID: 4981
public class WeatherPackage
{
	// Token: 0x06009D0B RID: 40203 RVA: 0x003B5620 File Offset: 0x003B3820
	public WeatherPackage()
	{
		this.param = new float[5];
	}

	// Token: 0x06009D0C RID: 40204 RVA: 0x003B5634 File Offset: 0x003B3834
	public void CopyTo(WeatherManager.BiomeWeather _bw)
	{
		_bw.remainingSeconds = this.remainingSeconds;
		for (int i = 0; i < this.param.Length; i++)
		{
			_bw.parameters[i].target = this.param[i];
		}
	}

	// Token: 0x06009D0D RID: 40205 RVA: 0x003B5678 File Offset: 0x003B3878
	public override string ToString()
	{
		string text = string.Format("id {0}, grp {1}, rsec {2}, params ", this.biomeId, this.groupIndex, this.remainingSeconds);
		for (int i = 0; i < this.param.Length; i++)
		{
			text += string.Format("{0}, ", this.param[i]);
		}
		return text;
	}

	// Token: 0x04007703 RID: 30467
	public byte biomeId;

	// Token: 0x04007704 RID: 30468
	public byte groupIndex;

	// Token: 0x04007705 RID: 30469
	public byte remainingSeconds;

	// Token: 0x04007706 RID: 30470
	public float[] param;
}
