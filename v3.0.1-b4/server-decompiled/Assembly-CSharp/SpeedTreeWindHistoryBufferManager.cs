using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02001359 RID: 4953
public class SpeedTreeWindHistoryBufferManager
{
	// Token: 0x06009C05 RID: 39941 RVA: 0x003AF9CB File Offset: 0x003ADBCB
	[PublicizedFrom(EAccessModifier.Private)]
	public SpeedTreeWindHistoryBufferManager()
	{
	}

	// Token: 0x1700125E RID: 4702
	// (get) Token: 0x06009C06 RID: 39942 RVA: 0x003AFA0A File Offset: 0x003ADC0A
	public static SpeedTreeWindHistoryBufferManager Instance
	{
		get
		{
			if (SpeedTreeWindHistoryBufferManager.m_Instance == null)
			{
				SpeedTreeWindHistoryBufferManager.m_Instance = new SpeedTreeWindHistoryBufferManager();
			}
			return SpeedTreeWindHistoryBufferManager.m_Instance;
		}
	}

	// Token: 0x06009C07 RID: 39943 RVA: 0x003AFA24 File Offset: 0x003ADC24
	public bool TryRegisterActiveRenderer(Renderer renderer)
	{
		bool flag;
		using (SpeedTreeWindHistoryBufferManager.s_ManagerTotal.Auto())
		{
			using (SpeedTreeWindHistoryBufferManager.s_ManagerRegistrations.Auto())
			{
				if (renderer == null)
				{
					Debug.LogError("Cannot register a null renderer.");
					flag = false;
				}
				else
				{
					SpeedTreeWindHistoryBufferManager.SharedMaterialGroup sharedMaterialGroup;
					if (!this.rendererToGroupMap.TryGetValue(renderer, out sharedMaterialGroup))
					{
						this.newMaterialsSet.Clear();
						this.tempMaterialsList.Clear();
						BillboardRenderer billboardRenderer = renderer as BillboardRenderer;
						if (billboardRenderer != null)
						{
							this.tempMaterialsList.Add(billboardRenderer.billboard.material);
						}
						else
						{
							renderer.GetSharedMaterials(this.tempMaterialsList);
						}
						foreach (Material material in this.tempMaterialsList)
						{
							if (!(material == null) && !this.materialToGroupMap.TryGetValue(material, out sharedMaterialGroup))
							{
								this.newMaterialsSet.Add(material);
							}
						}
						if (sharedMaterialGroup == null)
						{
							if (this.newMaterialsSet.Count == 0)
							{
								return false;
							}
							sharedMaterialGroup = new SpeedTreeWindHistoryBufferManager.SharedMaterialGroup();
							this.sharedMaterialGroups.Add(sharedMaterialGroup);
						}
						if (this.newMaterialsSet.Count > 0)
						{
							sharedMaterialGroup.MergeMaterials(this.newMaterialsSet);
							foreach (Material material2 in this.newMaterialsSet)
							{
								this.materialToGroupMap[material2] = sharedMaterialGroup;
							}
						}
						this.rendererToGroupMap[renderer] = sharedMaterialGroup;
						this.newMaterialsSet.Clear();
						this.tempMaterialsList.Clear();
					}
					sharedMaterialGroup.RegisterActiveRenderer(renderer);
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x06009C08 RID: 39944 RVA: 0x003AFC54 File Offset: 0x003ADE54
	public void DeregisterActiveRenderer(Renderer renderer)
	{
		using (SpeedTreeWindHistoryBufferManager.s_ManagerTotal.Auto())
		{
			using (SpeedTreeWindHistoryBufferManager.s_ManagerDeregistrations.Auto())
			{
				SpeedTreeWindHistoryBufferManager.SharedMaterialGroup sharedMaterialGroup;
				if (this.rendererToGroupMap.TryGetValue(renderer, out sharedMaterialGroup))
				{
					sharedMaterialGroup.DeregisterActiveRenderer(renderer);
				}
			}
		}
	}

	// Token: 0x06009C09 RID: 39945 RVA: 0x003AFCD0 File Offset: 0x003ADED0
	public void Update()
	{
		using (SpeedTreeWindHistoryBufferManager.s_ManagerTotal.Auto())
		{
			using (SpeedTreeWindHistoryBufferManager.s_ManagerUpdate.Auto())
			{
				foreach (SpeedTreeWindHistoryBufferManager.SharedMaterialGroup sharedMaterialGroup in this.sharedMaterialGroups)
				{
					sharedMaterialGroup.Update();
				}
			}
		}
	}

	// Token: 0x040075FA RID: 30202
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_GetMatProps = new ProfilerMarker("SpeedTreeWindPropertyBuffer.GetMatProps");

	// Token: 0x040075FB RID: 30203
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_SetMatProps = new ProfilerMarker("SpeedTreeWindPropertyBuffer.SetMatProps");

	// Token: 0x040075FC RID: 30204
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_CheckVis = new ProfilerMarker("SpeedTreeWindPropertyBuffer.CheckVis");

	// Token: 0x040075FD RID: 30205
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerUpdate = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerUpdate");

	// Token: 0x040075FE RID: 30206
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerRegistrations = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerRegistrations");

	// Token: 0x040075FF RID: 30207
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerDeregistrations = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerDeregistrations");

	// Token: 0x04007600 RID: 30208
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerGetFirstRenderer = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerGetFirstRenderer");

	// Token: 0x04007601 RID: 30209
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerTotal = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerTotal");

	// Token: 0x04007602 RID: 30210
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindVector = Shader.PropertyToID("_ST_WindVector");

	// Token: 0x04007603 RID: 30211
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindGlobal = Shader.PropertyToID("_ST_WindGlobal");

	// Token: 0x04007604 RID: 30212
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranch = Shader.PropertyToID("_ST_WindBranch");

	// Token: 0x04007605 RID: 30213
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchTwitch = Shader.PropertyToID("_ST_WindBranchTwitch");

	// Token: 0x04007606 RID: 30214
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchWhip = Shader.PropertyToID("_ST_WindBranchWhip");

	// Token: 0x04007607 RID: 30215
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchAnchor = Shader.PropertyToID("_ST_WindBranchAnchor");

	// Token: 0x04007608 RID: 30216
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchAdherences = Shader.PropertyToID("_ST_WindBranchAdherences");

	// Token: 0x04007609 RID: 30217
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindTurbulences = Shader.PropertyToID("_ST_WindTurbulences");

	// Token: 0x0400760A RID: 30218
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Ripple = Shader.PropertyToID("_ST_WindLeaf1Ripple");

	// Token: 0x0400760B RID: 30219
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Tumble = Shader.PropertyToID("_ST_WindLeaf1Tumble");

	// Token: 0x0400760C RID: 30220
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Twitch = Shader.PropertyToID("_ST_WindLeaf1Twitch");

	// Token: 0x0400760D RID: 30221
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Ripple = Shader.PropertyToID("_ST_WindLeaf2Ripple");

	// Token: 0x0400760E RID: 30222
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Tumble = Shader.PropertyToID("_ST_WindLeaf2Tumble");

	// Token: 0x0400760F RID: 30223
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Twitch = Shader.PropertyToID("_ST_WindLeaf2Twitch");

	// Token: 0x04007610 RID: 30224
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindFrondRipple = Shader.PropertyToID("_ST_WindFrondRipple");

	// Token: 0x04007611 RID: 30225
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindAnimation = Shader.PropertyToID("_ST_WindAnimation");

	// Token: 0x04007612 RID: 30226
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindVector = Shader.PropertyToID("_ST_PF_WindVector");

	// Token: 0x04007613 RID: 30227
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindGlobal = Shader.PropertyToID("_ST_PF_WindGlobal");

	// Token: 0x04007614 RID: 30228
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranch = Shader.PropertyToID("_ST_PF_WindBranch");

	// Token: 0x04007615 RID: 30229
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchTwitch = Shader.PropertyToID("_ST_PF_WindBranchTwitch");

	// Token: 0x04007616 RID: 30230
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchWhip = Shader.PropertyToID("_ST_PF_WindBranchWhip");

	// Token: 0x04007617 RID: 30231
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchAnchor = Shader.PropertyToID("_ST_PF_WindBranchAnchor");

	// Token: 0x04007618 RID: 30232
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchAdherences = Shader.PropertyToID("_ST_PF_WindBranchAdherences");

	// Token: 0x04007619 RID: 30233
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindTurbulences = Shader.PropertyToID("_ST_PF_WindTurbulences");

	// Token: 0x0400761A RID: 30234
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Ripple = Shader.PropertyToID("_ST_PF_WindLeaf1Ripple");

	// Token: 0x0400761B RID: 30235
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Tumble = Shader.PropertyToID("_ST_PF_WindLeaf1Tumble");

	// Token: 0x0400761C RID: 30236
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Twitch = Shader.PropertyToID("_ST_PF_WindLeaf1Twitch");

	// Token: 0x0400761D RID: 30237
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Ripple = Shader.PropertyToID("_ST_PF_WindLeaf2Ripple");

	// Token: 0x0400761E RID: 30238
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Tumble = Shader.PropertyToID("_ST_PF_WindLeaf2Tumble");

	// Token: 0x0400761F RID: 30239
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Twitch = Shader.PropertyToID("_ST_PF_WindLeaf2Twitch");

	// Token: 0x04007620 RID: 30240
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindFrondRipple = Shader.PropertyToID("_ST_PF_WindFrondRipple");

	// Token: 0x04007621 RID: 30241
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindAnimation = Shader.PropertyToID("_ST_PF_WindAnimation");

	// Token: 0x04007622 RID: 30242
	[PublicizedFrom(EAccessModifier.Private)]
	public static SpeedTreeWindHistoryBufferManager m_Instance;

	// Token: 0x04007623 RID: 30243
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Renderer, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> rendererToGroupMap = new Dictionary<Renderer, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x04007624 RID: 30244
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Material, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> materialToGroupMap = new Dictionary<Material, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x04007625 RID: 30245
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Material> tempMaterialsList = new List<Material>();

	// Token: 0x04007626 RID: 30246
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Material> newMaterialsSet = new HashSet<Material>();

	// Token: 0x04007627 RID: 30247
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> sharedMaterialGroups = new HashSet<SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x0200135A RID: 4954
	public class SharedMaterialGroup
	{
		// Token: 0x06009C0B RID: 39947 RVA: 0x003AFFD5 File Offset: 0x003AE1D5
		public SharedMaterialGroup()
		{
			this.previousProperties = new MaterialPropertyBlock();
		}

		// Token: 0x06009C0C RID: 39948 RVA: 0x003AFFFE File Offset: 0x003AE1FE
		public void MergeMaterials(HashSet<Material> newMaterialsSet)
		{
			this.sharedMaterials.UnionWith(newMaterialsSet);
		}

		// Token: 0x06009C0D RID: 39949 RVA: 0x003B000C File Offset: 0x003AE20C
		public void RegisterActiveRenderer(Renderer renderer)
		{
			this.activeRenderers.Add(renderer);
		}

		// Token: 0x06009C0E RID: 39950 RVA: 0x003B001B File Offset: 0x003AE21B
		public void DeregisterActiveRenderer(Renderer renderer)
		{
			this.activeRenderers.Remove(renderer);
		}

		// Token: 0x06009C0F RID: 39951 RVA: 0x003B002C File Offset: 0x003AE22C
		public void Update()
		{
			using (SpeedTreeWindHistoryBufferManager.s_CheckVis.Auto())
			{
				if (this.activeRenderers.Count == 0)
				{
					return;
				}
			}
			Renderer renderer = null;
			using (SpeedTreeWindHistoryBufferManager.s_ManagerGetFirstRenderer.Auto())
			{
				using (HashSet<Renderer>.Enumerator enumerator = this.activeRenderers.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						return;
					}
					renderer = enumerator.Current;
				}
			}
			using (SpeedTreeWindHistoryBufferManager.s_GetMatProps.Auto())
			{
				renderer.GetPropertyBlock(this.previousProperties);
			}
			using (SpeedTreeWindHistoryBufferManager.s_SetMatProps.Auto())
			{
				foreach (Material material in this.sharedMaterials)
				{
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindVector, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindVector));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindGlobal, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindGlobal));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranch, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranch));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranchTwitch, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranchTwitch));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranchWhip, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranchWhip));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranchAnchor, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranchAnchor));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranchAdherences, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranchAdherences));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindTurbulences, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindTurbulences));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf1Ripple, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf1Ripple));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf1Tumble, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf1Tumble));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf1Twitch, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf1Twitch));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf2Ripple, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf2Ripple));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf2Tumble, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf2Tumble));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf2Twitch, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf2Twitch));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindFrondRipple, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindFrondRipple));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindAnimation, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindAnimation));
				}
			}
		}

		// Token: 0x04007628 RID: 30248
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Renderer> activeRenderers = new HashSet<Renderer>();

		// Token: 0x04007629 RID: 30249
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Material> sharedMaterials = new HashSet<Material>();

		// Token: 0x0400762A RID: 30250
		[PublicizedFrom(EAccessModifier.Private)]
		public MaterialPropertyBlock previousProperties;
	}
}
