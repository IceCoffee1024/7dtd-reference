using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200138B RID: 5003
[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class SetShadowMapAsGlobalTexture : MonoBehaviour
{
	// Token: 0x06009D3A RID: 40250 RVA: 0x003B67D4 File Offset: 0x003B49D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.lightComponent = base.GetComponent<Light>();
		this.SetupCommandBuffer();
	}

	// Token: 0x06009D3B RID: 40251 RVA: 0x003B67E8 File Offset: 0x003B49E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.lightComponent.RemoveCommandBuffer(LightEvent.AfterShadowMap, this.commandBuffer);
		this.ReleaseCommandBuffer();
	}

	// Token: 0x06009D3C RID: 40252 RVA: 0x003B6804 File Offset: 0x003B4A04
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupCommandBuffer()
	{
		this.commandBuffer = new CommandBuffer();
		this.commandBuffer.name = "SetShadowMapAsGlobalTexture";
		RenderTargetIdentifier renderTargetIdentifier = BuiltinRenderTextureType.CurrentActive;
		this.commandBuffer.SetGlobalTexture(this.textureSemanticName, renderTargetIdentifier);
		this.lightComponent.AddCommandBuffer(LightEvent.AfterShadowMap, this.commandBuffer);
	}

	// Token: 0x06009D3D RID: 40253 RVA: 0x003B6857 File Offset: 0x003B4A57
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReleaseCommandBuffer()
	{
		this.commandBuffer.Clear();
	}

	// Token: 0x040076D0 RID: 30416
	public string textureSemanticName = "_SunCascadedShadowMap";

	// Token: 0x040076D1 RID: 30417
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public RenderTexture shadowMapRenderTexture;

	// Token: 0x040076D2 RID: 30418
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer commandBuffer;

	// Token: 0x040076D3 RID: 30419
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light lightComponent;
}
