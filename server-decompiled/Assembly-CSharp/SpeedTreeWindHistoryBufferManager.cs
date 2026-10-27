using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

// Token: 0x020013A5 RID: 5029
public class SpeedTreeWindHistoryBufferManager
{
	// Token: 0x06009E04 RID: 40452 RVA: 0x003BBBF3 File Offset: 0x003B9DF3
	[PublicizedFrom(EAccessModifier.Private)]
	public SpeedTreeWindHistoryBufferManager()
	{
	}

	// Token: 0x170012E8 RID: 4840
	// (get) Token: 0x06009E05 RID: 40453 RVA: 0x003BBC32 File Offset: 0x003B9E32
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

	// Token: 0x06009E06 RID: 40454 RVA: 0x003BBC4C File Offset: 0x003B9E4C
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

	// Token: 0x06009E07 RID: 40455 RVA: 0x003BBE7C File Offset: 0x003BA07C
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

	// Token: 0x06009E08 RID: 40456 RVA: 0x003BBEF8 File Offset: 0x003BA0F8
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

	// Token: 0x0400778E RID: 30606
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_GetMatProps = new ProfilerMarker("SpeedTreeWindPropertyBuffer.GetMatProps");

	// Token: 0x0400778F RID: 30607
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_SetMatProps = new ProfilerMarker("SpeedTreeWindPropertyBuffer.SetMatProps");

	// Token: 0x04007790 RID: 30608
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_CheckVis = new ProfilerMarker("SpeedTreeWindPropertyBuffer.CheckVis");

	// Token: 0x04007791 RID: 30609
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerUpdate = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerUpdate");

	// Token: 0x04007792 RID: 30610
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerRegistrations = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerRegistrations");

	// Token: 0x04007793 RID: 30611
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerDeregistrations = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerDeregistrations");

	// Token: 0x04007794 RID: 30612
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerGetFirstRenderer = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerGetFirstRenderer");

	// Token: 0x04007795 RID: 30613
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerTotal = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerTotal");

	// Token: 0x04007796 RID: 30614
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindVector = Shader.PropertyToID("_ST_WindVector");

	// Token: 0x04007797 RID: 30615
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindGlobal = Shader.PropertyToID("_ST_WindGlobal");

	// Token: 0x04007798 RID: 30616
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranch = Shader.PropertyToID("_ST_WindBranch");

	// Token: 0x04007799 RID: 30617
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchTwitch = Shader.PropertyToID("_ST_WindBranchTwitch");

	// Token: 0x0400779A RID: 30618
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchWhip = Shader.PropertyToID("_ST_WindBranchWhip");

	// Token: 0x0400779B RID: 30619
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchAnchor = Shader.PropertyToID("_ST_WindBranchAnchor");

	// Token: 0x0400779C RID: 30620
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchAdherences = Shader.PropertyToID("_ST_WindBranchAdherences");

	// Token: 0x0400779D RID: 30621
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindTurbulences = Shader.PropertyToID("_ST_WindTurbulences");

	// Token: 0x0400779E RID: 30622
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Ripple = Shader.PropertyToID("_ST_WindLeaf1Ripple");

	// Token: 0x0400779F RID: 30623
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Tumble = Shader.PropertyToID("_ST_WindLeaf1Tumble");

	// Token: 0x040077A0 RID: 30624
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Twitch = Shader.PropertyToID("_ST_WindLeaf1Twitch");

	// Token: 0x040077A1 RID: 30625
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Ripple = Shader.PropertyToID("_ST_WindLeaf2Ripple");

	// Token: 0x040077A2 RID: 30626
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Tumble = Shader.PropertyToID("_ST_WindLeaf2Tumble");

	// Token: 0x040077A3 RID: 30627
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Twitch = Shader.PropertyToID("_ST_WindLeaf2Twitch");

	// Token: 0x040077A4 RID: 30628
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindFrondRipple = Shader.PropertyToID("_ST_WindFrondRipple");

	// Token: 0x040077A5 RID: 30629
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindAnimation = Shader.PropertyToID("_ST_WindAnimation");

	// Token: 0x040077A6 RID: 30630
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindVector = Shader.PropertyToID("_ST_PF_WindVector");

	// Token: 0x040077A7 RID: 30631
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindGlobal = Shader.PropertyToID("_ST_PF_WindGlobal");

	// Token: 0x040077A8 RID: 30632
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranch = Shader.PropertyToID("_ST_PF_WindBranch");

	// Token: 0x040077A9 RID: 30633
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchTwitch = Shader.PropertyToID("_ST_PF_WindBranchTwitch");

	// Token: 0x040077AA RID: 30634
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchWhip = Shader.PropertyToID("_ST_PF_WindBranchWhip");

	// Token: 0x040077AB RID: 30635
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchAnchor = Shader.PropertyToID("_ST_PF_WindBranchAnchor");

	// Token: 0x040077AC RID: 30636
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchAdherences = Shader.PropertyToID("_ST_PF_WindBranchAdherences");

	// Token: 0x040077AD RID: 30637
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindTurbulences = Shader.PropertyToID("_ST_PF_WindTurbulences");

	// Token: 0x040077AE RID: 30638
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Ripple = Shader.PropertyToID("_ST_PF_WindLeaf1Ripple");

	// Token: 0x040077AF RID: 30639
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Tumble = Shader.PropertyToID("_ST_PF_WindLeaf1Tumble");

	// Token: 0x040077B0 RID: 30640
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Twitch = Shader.PropertyToID("_ST_PF_WindLeaf1Twitch");

	// Token: 0x040077B1 RID: 30641
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Ripple = Shader.PropertyToID("_ST_PF_WindLeaf2Ripple");

	// Token: 0x040077B2 RID: 30642
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Tumble = Shader.PropertyToID("_ST_PF_WindLeaf2Tumble");

	// Token: 0x040077B3 RID: 30643
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Twitch = Shader.PropertyToID("_ST_PF_WindLeaf2Twitch");

	// Token: 0x040077B4 RID: 30644
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindFrondRipple = Shader.PropertyToID("_ST_PF_WindFrondRipple");

	// Token: 0x040077B5 RID: 30645
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindAnimation = Shader.PropertyToID("_ST_PF_WindAnimation");

	// Token: 0x040077B6 RID: 30646
	[PublicizedFrom(EAccessModifier.Private)]
	public static SpeedTreeWindHistoryBufferManager m_Instance;

	// Token: 0x040077B7 RID: 30647
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Renderer, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> rendererToGroupMap = new Dictionary<Renderer, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x040077B8 RID: 30648
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Material, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> materialToGroupMap = new Dictionary<Material, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x040077B9 RID: 30649
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Material> tempMaterialsList = new List<Material>();

	// Token: 0x040077BA RID: 30650
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Material> newMaterialsSet = new HashSet<Material>();

	// Token: 0x040077BB RID: 30651
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> sharedMaterialGroups = new HashSet<SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x020013A6 RID: 5030
	public class SharedMaterialGroup
	{
		// Token: 0x06009E0A RID: 40458 RVA: 0x003BC1FD File Offset: 0x003BA3FD
		public SharedMaterialGroup()
		{
			this.previousProperties = new MaterialPropertyBlock();
		}

		// Token: 0x06009E0B RID: 40459 RVA: 0x003BC226 File Offset: 0x003BA426
		public void MergeMaterials(HashSet<Material> newMaterialsSet)
		{
			this.sharedMaterials.UnionWith(newMaterialsSet);
		}

		// Token: 0x06009E0C RID: 40460 RVA: 0x003BC234 File Offset: 0x003BA434
		public void RegisterActiveRenderer(Renderer renderer)
		{
			this.activeRenderers.Add(renderer);
		}

		// Token: 0x06009E0D RID: 40461 RVA: 0x003BC243 File Offset: 0x003BA443
		public void DeregisterActiveRenderer(Renderer renderer)
		{
			this.activeRenderers.Remove(renderer);
		}

		// Token: 0x06009E0E RID: 40462 RVA: 0x003BC254 File Offset: 0x003BA454
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

		// Token: 0x040077BC RID: 30652
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Renderer> activeRenderers = new HashSet<Renderer>();

		// Token: 0x040077BD RID: 30653
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Material> sharedMaterials = new HashSet<Material>();

		// Token: 0x040077BE RID: 30654
		[PublicizedFrom(EAccessModifier.Private)]
		public MaterialPropertyBlock previousProperties;
	}
}
