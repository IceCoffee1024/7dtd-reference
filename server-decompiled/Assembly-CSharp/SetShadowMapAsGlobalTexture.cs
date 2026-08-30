using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200136B RID: 4971
[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class SetShadowMapAsGlobalTexture : MonoBehaviour
{
	// Token: 0x06009C8E RID: 40078 RVA: 0x003B1C58 File Offset: 0x003AFE58
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.lightComponent = base.GetComponent<Light>();
		this.SetupCommandBuffer();
	}

	// Token: 0x06009C8F RID: 40079 RVA: 0x003B1C6C File Offset: 0x003AFE6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.lightComponent.RemoveCommandBuffer(LightEvent.AfterShadowMap, this.commandBuffer);
		this.ReleaseCommandBuffer();
	}

	// Token: 0x06009C90 RID: 40080 RVA: 0x003B1C88 File Offset: 0x003AFE88
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupCommandBuffer()
	{
		this.commandBuffer = new CommandBuffer();
		this.commandBuffer.name = "SetShadowMapAsGlobalTexture";
		RenderTargetIdentifier renderTargetIdentifier = BuiltinRenderTextureType.CurrentActive;
		this.commandBuffer.SetGlobalTexture(this.textureSemanticName, renderTargetIdentifier);
		this.lightComponent.AddCommandBuffer(LightEvent.AfterShadowMap, this.commandBuffer);
	}

	// Token: 0x06009C91 RID: 40081 RVA: 0x003B1CDB File Offset: 0x003AFEDB
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReleaseCommandBuffer()
	{
		this.commandBuffer.Clear();
	}

	// Token: 0x04007655 RID: 30293
	public string textureSemanticName = "_SunCascadedShadowMap";

	// Token: 0x04007656 RID: 30294
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public RenderTexture shadowMapRenderTexture;

	// Token: 0x04007657 RID: 30295
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer commandBuffer;

	// Token: 0x04007658 RID: 30296
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light lightComponent;
}
