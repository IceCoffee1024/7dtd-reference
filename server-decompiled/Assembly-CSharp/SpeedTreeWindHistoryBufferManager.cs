using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02001393 RID: 5011
public class SpeedTreeWindHistoryBufferManager
{
	// Token: 0x06009DAC RID: 40364 RVA: 0x003B97EF File Offset: 0x003B79EF
	[PublicizedFrom(EAccessModifier.Private)]
	public SpeedTreeWindHistoryBufferManager()
	{
	}

	// Token: 0x170012DC RID: 4828
	// (get) Token: 0x06009DAD RID: 40365 RVA: 0x003B982E File Offset: 0x003B7A2E
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

	// Token: 0x06009DAE RID: 40366 RVA: 0x003B9848 File Offset: 0x003B7A48
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

	// Token: 0x06009DAF RID: 40367 RVA: 0x003B9A78 File Offset: 0x003B7C78
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

	// Token: 0x06009DB0 RID: 40368 RVA: 0x003B9AF4 File Offset: 0x003B7CF4
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

	// Token: 0x0400774D RID: 30541
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_GetMatProps = new ProfilerMarker("SpeedTreeWindPropertyBuffer.GetMatProps");

	// Token: 0x0400774E RID: 30542
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_SetMatProps = new ProfilerMarker("SpeedTreeWindPropertyBuffer.SetMatProps");

	// Token: 0x0400774F RID: 30543
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_CheckVis = new ProfilerMarker("SpeedTreeWindPropertyBuffer.CheckVis");

	// Token: 0x04007750 RID: 30544
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerUpdate = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerUpdate");

	// Token: 0x04007751 RID: 30545
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerRegistrations = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerRegistrations");

	// Token: 0x04007752 RID: 30546
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerDeregistrations = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerDeregistrations");

	// Token: 0x04007753 RID: 30547
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerGetFirstRenderer = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerGetFirstRenderer");

	// Token: 0x04007754 RID: 30548
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerTotal = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerTotal");

	// Token: 0x04007755 RID: 30549
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindVector = Shader.PropertyToID("_ST_WindVector");

	// Token: 0x04007756 RID: 30550
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindGlobal = Shader.PropertyToID("_ST_WindGlobal");

	// Token: 0x04007757 RID: 30551
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranch = Shader.PropertyToID("_ST_WindBranch");

	// Token: 0x04007758 RID: 30552
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchTwitch = Shader.PropertyToID("_ST_WindBranchTwitch");

	// Token: 0x04007759 RID: 30553
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchWhip = Shader.PropertyToID("_ST_WindBranchWhip");

	// Token: 0x0400775A RID: 30554
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchAnchor = Shader.PropertyToID("_ST_WindBranchAnchor");

	// Token: 0x0400775B RID: 30555
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchAdherences = Shader.PropertyToID("_ST_WindBranchAdherences");

	// Token: 0x0400775C RID: 30556
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindTurbulences = Shader.PropertyToID("_ST_WindTurbulences");

	// Token: 0x0400775D RID: 30557
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Ripple = Shader.PropertyToID("_ST_WindLeaf1Ripple");

	// Token: 0x0400775E RID: 30558
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Tumble = Shader.PropertyToID("_ST_WindLeaf1Tumble");

	// Token: 0x0400775F RID: 30559
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Twitch = Shader.PropertyToID("_ST_WindLeaf1Twitch");

	// Token: 0x04007760 RID: 30560
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Ripple = Shader.PropertyToID("_ST_WindLeaf2Ripple");

	// Token: 0x04007761 RID: 30561
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Tumble = Shader.PropertyToID("_ST_WindLeaf2Tumble");

	// Token: 0x04007762 RID: 30562
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Twitch = Shader.PropertyToID("_ST_WindLeaf2Twitch");

	// Token: 0x04007763 RID: 30563
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindFrondRipple = Shader.PropertyToID("_ST_WindFrondRipple");

	// Token: 0x04007764 RID: 30564
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindAnimation = Shader.PropertyToID("_ST_WindAnimation");

	// Token: 0x04007765 RID: 30565
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindVector = Shader.PropertyToID("_ST_PF_WindVector");

	// Token: 0x04007766 RID: 30566
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindGlobal = Shader.PropertyToID("_ST_PF_WindGlobal");

	// Token: 0x04007767 RID: 30567
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranch = Shader.PropertyToID("_ST_PF_WindBranch");

	// Token: 0x04007768 RID: 30568
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchTwitch = Shader.PropertyToID("_ST_PF_WindBranchTwitch");

	// Token: 0x04007769 RID: 30569
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchWhip = Shader.PropertyToID("_ST_PF_WindBranchWhip");

	// Token: 0x0400776A RID: 30570
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchAnchor = Shader.PropertyToID("_ST_PF_WindBranchAnchor");

	// Token: 0x0400776B RID: 30571
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchAdherences = Shader.PropertyToID("_ST_PF_WindBranchAdherences");

	// Token: 0x0400776C RID: 30572
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindTurbulences = Shader.PropertyToID("_ST_PF_WindTurbulences");

	// Token: 0x0400776D RID: 30573
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Ripple = Shader.PropertyToID("_ST_PF_WindLeaf1Ripple");

	// Token: 0x0400776E RID: 30574
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Tumble = Shader.PropertyToID("_ST_PF_WindLeaf1Tumble");

	// Token: 0x0400776F RID: 30575
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Twitch = Shader.PropertyToID("_ST_PF_WindLeaf1Twitch");

	// Token: 0x04007770 RID: 30576
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Ripple = Shader.PropertyToID("_ST_PF_WindLeaf2Ripple");

	// Token: 0x04007771 RID: 30577
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Tumble = Shader.PropertyToID("_ST_PF_WindLeaf2Tumble");

	// Token: 0x04007772 RID: 30578
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Twitch = Shader.PropertyToID("_ST_PF_WindLeaf2Twitch");

	// Token: 0x04007773 RID: 30579
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindFrondRipple = Shader.PropertyToID("_ST_PF_WindFrondRipple");

	// Token: 0x04007774 RID: 30580
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindAnimation = Shader.PropertyToID("_ST_PF_WindAnimation");

	// Token: 0x04007775 RID: 30581
	[PublicizedFrom(EAccessModifier.Private)]
	public static SpeedTreeWindHistoryBufferManager m_Instance;

	// Token: 0x04007776 RID: 30582
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Renderer, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> rendererToGroupMap = new Dictionary<Renderer, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x04007777 RID: 30583
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Material, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> materialToGroupMap = new Dictionary<Material, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x04007778 RID: 30584
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Material> tempMaterialsList = new List<Material>();

	// Token: 0x04007779 RID: 30585
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Material> newMaterialsSet = new HashSet<Material>();

	// Token: 0x0400777A RID: 30586
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> sharedMaterialGroups = new HashSet<SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x02001394 RID: 5012
	public class SharedMaterialGroup
	{
		// Token: 0x06009DB2 RID: 40370 RVA: 0x003B9DF9 File Offset: 0x003B7FF9
		public SharedMaterialGroup()
		{
			this.previousProperties = new MaterialPropertyBlock();
		}

		// Token: 0x06009DB3 RID: 40371 RVA: 0x003B9E22 File Offset: 0x003B8022
		public void MergeMaterials(HashSet<Material> newMaterialsSet)
		{
			this.sharedMaterials.UnionWith(newMaterialsSet);
		}

		// Token: 0x06009DB4 RID: 40372 RVA: 0x003B9E30 File Offset: 0x003B8030
		public void RegisterActiveRenderer(Renderer renderer)
		{
			this.activeRenderers.Add(renderer);
		}

		// Token: 0x06009DB5 RID: 40373 RVA: 0x003B9E3F File Offset: 0x003B803F
		public void DeregisterActiveRenderer(Renderer renderer)
		{
			this.activeRenderers.Remove(renderer);
		}

		// Token: 0x06009DB6 RID: 40374 RVA: 0x003B9E50 File Offset: 0x003B8050
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

		// Token: 0x0400777B RID: 30587
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Renderer> activeRenderers = new HashSet<Renderer>();

		// Token: 0x0400777C RID: 30588
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Material> sharedMaterials = new HashSet<Material>();

		// Token: 0x0400777D RID: 30589
		[PublicizedFrom(EAccessModifier.Private)]
		public MaterialPropertyBlock previousProperties;
	}
}
