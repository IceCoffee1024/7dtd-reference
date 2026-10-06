using System;
using UnityEngine;

// Token: 0x0200138C RID: 5004
[ExecuteInEditMode]
public class ShaderGlobalsHelper : MonoBehaviour
{
	// Token: 0x06009D3F RID: 40255 RVA: 0x003B6878 File Offset: 0x003B4A78
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		Shader.SetGlobalFloat("_ElectricShockIntensity", this.electricShockIntensity);
		Shader.SetGlobalColor("_ElectricShockColor", this.electricShockColor);
		Shader.SetGlobalFloat("_ElectricShockSpeed", this.electricShockSpeed);
		Shader.SetGlobalFloat("_ElectricShockScale", this.electricShockScale);
		Shader.SetGlobalTexture("_ElectricShockTexture", this.electricShockTexture);
		Shader.SetGlobalVector("_ElectricShockTexture_ST", this.electricShockTexture_ST);
		Shader.SetGlobalFloat("_ElectricShockTexturePanSpeed", this.electricShockTexturePanSpeed);
	}

	// Token: 0x06009D40 RID: 40256 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x040076D4 RID: 30420
	[Header("Electric Shock Shader Properties")]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockIntensity = 1f;

	// Token: 0x040076D5 RID: 30421
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Color electricShockColor = new Color(0.5f, 0.8f, 1f, 1f);

	// Token: 0x040076D6 RID: 30422
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockSpeed = 5f;

	// Token: 0x040076D7 RID: 30423
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockScale = 2.5f;

	// Token: 0x040076D8 RID: 30424
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture electricShockTexture;

	// Token: 0x040076D9 RID: 30425
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector4 electricShockTexture_ST;

	// Token: 0x040076DA RID: 30426
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockTexturePanSpeed = 10f;

	// Token: 0x040076DB RID: 30427
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateEveryFrame;
}
