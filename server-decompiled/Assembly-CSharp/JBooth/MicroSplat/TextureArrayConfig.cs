using System;
using System.Collections.Generic;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D13 RID: 7443
	[CreateAssetMenu(menuName = "MicroSplat/Texture Array Config", order = 1)]
	[ExecuteInEditMode]
	public class TextureArrayConfig : ScriptableObject
	{
		// Token: 0x0600DC03 RID: 56323 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsScatter()
		{
			return false;
		}

		// Token: 0x0600DC04 RID: 56324 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsDecal()
		{
			return false;
		}

		// Token: 0x0600DC05 RID: 56325 RVA: 0x004EDC8C File Offset: 0x004EBE8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			TextureArrayConfig.sAllConfigs.Add(this);
		}

		// Token: 0x0600DC06 RID: 56326 RVA: 0x004EDC99 File Offset: 0x004EBE99
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDestroy()
		{
			TextureArrayConfig.sAllConfigs.Remove(this);
		}

		// Token: 0x0600DC07 RID: 56327 RVA: 0x004EDCA8 File Offset: 0x004EBEA8
		public static TextureArrayConfig FindConfig(Texture2DArray diffuse)
		{
			for (int i = 0; i < TextureArrayConfig.sAllConfigs.Count; i++)
			{
				if (TextureArrayConfig.sAllConfigs[i].diffuseArray == diffuse)
				{
					return TextureArrayConfig.sAllConfigs[i];
				}
			}
			return null;
		}

		// Token: 0x0400A62D RID: 42541
		public bool diffuseIsLinear;

		// Token: 0x0400A62E RID: 42542
		[HideInInspector]
		public bool antiTileArray;

		// Token: 0x0400A62F RID: 42543
		[HideInInspector]
		public bool emisMetalArray;

		// Token: 0x0400A630 RID: 42544
		public bool traxArray;

		// Token: 0x0400A631 RID: 42545
		[HideInInspector]
		public TextureArrayConfig.TextureMode textureMode = TextureArrayConfig.TextureMode.PBR;

		// Token: 0x0400A632 RID: 42546
		[HideInInspector]
		public TextureArrayConfig.ClusterMode clusterMode;

		// Token: 0x0400A633 RID: 42547
		[HideInInspector]
		public TextureArrayConfig.PackingMode packingMode;

		// Token: 0x0400A634 RID: 42548
		[HideInInspector]
		public TextureArrayConfig.PBRWorkflow pbrWorkflow;

		// Token: 0x0400A635 RID: 42549
		[HideInInspector]
		public int hash;

		// Token: 0x0400A636 RID: 42550
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static List<TextureArrayConfig> sAllConfigs = new List<TextureArrayConfig>();

		// Token: 0x0400A637 RID: 42551
		[HideInInspector]
		public Texture2DArray splatArray;

		// Token: 0x0400A638 RID: 42552
		[HideInInspector]
		public Texture2DArray diffuseArray;

		// Token: 0x0400A639 RID: 42553
		[HideInInspector]
		public Texture2DArray normalSAOArray;

		// Token: 0x0400A63A RID: 42554
		[HideInInspector]
		public Texture2DArray smoothAOArray;

		// Token: 0x0400A63B RID: 42555
		[HideInInspector]
		public Texture2DArray specularArray;

		// Token: 0x0400A63C RID: 42556
		[HideInInspector]
		public Texture2DArray diffuseArray2;

		// Token: 0x0400A63D RID: 42557
		[HideInInspector]
		public Texture2DArray normalSAOArray2;

		// Token: 0x0400A63E RID: 42558
		[HideInInspector]
		public Texture2DArray smoothAOArray2;

		// Token: 0x0400A63F RID: 42559
		[HideInInspector]
		public Texture2DArray specularArray2;

		// Token: 0x0400A640 RID: 42560
		[HideInInspector]
		public Texture2DArray diffuseArray3;

		// Token: 0x0400A641 RID: 42561
		[HideInInspector]
		public Texture2DArray normalSAOArray3;

		// Token: 0x0400A642 RID: 42562
		[HideInInspector]
		public Texture2DArray smoothAOArray3;

		// Token: 0x0400A643 RID: 42563
		[HideInInspector]
		public Texture2DArray specularArray3;

		// Token: 0x0400A644 RID: 42564
		[HideInInspector]
		public Texture2DArray emisArray;

		// Token: 0x0400A645 RID: 42565
		[HideInInspector]
		public Texture2DArray emisArray2;

		// Token: 0x0400A646 RID: 42566
		[HideInInspector]
		public Texture2DArray emisArray3;

		// Token: 0x0400A647 RID: 42567
		public TextureArrayConfig.TextureArrayGroup defaultTextureSettings = new TextureArrayConfig.TextureArrayGroup();

		// Token: 0x0400A648 RID: 42568
		public List<TextureArrayConfig.PlatformTextureOverride> platformOverrides = new List<TextureArrayConfig.PlatformTextureOverride>();

		// Token: 0x0400A649 RID: 42569
		public TextureArrayConfig.SourceTextureSize sourceTextureSize;

		// Token: 0x0400A64A RID: 42570
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelHeight = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A64B RID: 42571
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelSmoothness = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A64C RID: 42572
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelAO = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A64D RID: 42573
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x0400A64E RID: 42574
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures2 = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x0400A64F RID: 42575
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures3 = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x02001D14 RID: 7444
		public enum AllTextureChannel
		{
			// Token: 0x0400A651 RID: 42577
			R,
			// Token: 0x0400A652 RID: 42578
			G,
			// Token: 0x0400A653 RID: 42579
			B,
			// Token: 0x0400A654 RID: 42580
			A,
			// Token: 0x0400A655 RID: 42581
			Custom
		}

		// Token: 0x02001D15 RID: 7445
		public enum TextureChannel
		{
			// Token: 0x0400A657 RID: 42583
			R,
			// Token: 0x0400A658 RID: 42584
			G,
			// Token: 0x0400A659 RID: 42585
			B,
			// Token: 0x0400A65A RID: 42586
			A
		}

		// Token: 0x02001D16 RID: 7446
		public enum Compression
		{
			// Token: 0x0400A65C RID: 42588
			AutomaticCompressed,
			// Token: 0x0400A65D RID: 42589
			ForceDXT,
			// Token: 0x0400A65E RID: 42590
			ForcePVR,
			// Token: 0x0400A65F RID: 42591
			ForceETC2,
			// Token: 0x0400A660 RID: 42592
			ForceASTC,
			// Token: 0x0400A661 RID: 42593
			ForceCrunch,
			// Token: 0x0400A662 RID: 42594
			Uncompressed
		}

		// Token: 0x02001D17 RID: 7447
		public enum TextureSize
		{
			// Token: 0x0400A664 RID: 42596
			k4096 = 4096,
			// Token: 0x0400A665 RID: 42597
			k2048 = 2048,
			// Token: 0x0400A666 RID: 42598
			k1024 = 1024,
			// Token: 0x0400A667 RID: 42599
			k512 = 512,
			// Token: 0x0400A668 RID: 42600
			k256 = 256,
			// Token: 0x0400A669 RID: 42601
			k128 = 128,
			// Token: 0x0400A66A RID: 42602
			k64 = 64,
			// Token: 0x0400A66B RID: 42603
			k32 = 32
		}

		// Token: 0x02001D18 RID: 7448
		[Serializable]
		public class TextureArraySettings
		{
			// Token: 0x0600DC0A RID: 56330 RVA: 0x004EDD62 File Offset: 0x004EBF62
			public TextureArraySettings(TextureArrayConfig.TextureSize s, TextureArrayConfig.Compression c, FilterMode f, int a = 1)
			{
				this.textureSize = s;
				this.compression = c;
				this.filterMode = f;
				this.Aniso = a;
			}

			// Token: 0x0400A66C RID: 42604
			public TextureArrayConfig.TextureSize textureSize;

			// Token: 0x0400A66D RID: 42605
			public TextureArrayConfig.Compression compression;

			// Token: 0x0400A66E RID: 42606
			public FilterMode filterMode;

			// Token: 0x0400A66F RID: 42607
			[Range(0f, 16f)]
			public int Aniso = 1;
		}

		// Token: 0x02001D19 RID: 7449
		public enum PBRWorkflow
		{
			// Token: 0x0400A671 RID: 42609
			Metallic,
			// Token: 0x0400A672 RID: 42610
			Specular
		}

		// Token: 0x02001D1A RID: 7450
		public enum PackingMode
		{
			// Token: 0x0400A674 RID: 42612
			Fastest,
			// Token: 0x0400A675 RID: 42613
			Quality
		}

		// Token: 0x02001D1B RID: 7451
		public enum SourceTextureSize
		{
			// Token: 0x0400A677 RID: 42615
			Unchanged,
			// Token: 0x0400A678 RID: 42616
			k32 = 32,
			// Token: 0x0400A679 RID: 42617
			k256 = 256
		}

		// Token: 0x02001D1C RID: 7452
		public enum TextureMode
		{
			// Token: 0x0400A67B RID: 42619
			Basic,
			// Token: 0x0400A67C RID: 42620
			PBR
		}

		// Token: 0x02001D1D RID: 7453
		public enum ClusterMode
		{
			// Token: 0x0400A67E RID: 42622
			None,
			// Token: 0x0400A67F RID: 42623
			TwoVariations,
			// Token: 0x0400A680 RID: 42624
			ThreeVariations
		}

		// Token: 0x02001D1E RID: 7454
		[Serializable]
		public class TextureArrayGroup
		{
			// Token: 0x0400A681 RID: 42625
			public TextureArrayConfig.TextureArraySettings diffuseSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A682 RID: 42626
			public TextureArrayConfig.TextureArraySettings normalSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Trilinear, 1);

			// Token: 0x0400A683 RID: 42627
			public TextureArrayConfig.TextureArraySettings smoothSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A684 RID: 42628
			public TextureArrayConfig.TextureArraySettings antiTileSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A685 RID: 42629
			public TextureArrayConfig.TextureArraySettings emissiveSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A686 RID: 42630
			public TextureArrayConfig.TextureArraySettings specularSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A687 RID: 42631
			public TextureArrayConfig.TextureArraySettings traxDiffuseSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A688 RID: 42632
			public TextureArrayConfig.TextureArraySettings traxNormalSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A689 RID: 42633
			public TextureArrayConfig.TextureArraySettings decalSplatSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);
		}

		// Token: 0x02001D1F RID: 7455
		[Serializable]
		public class PlatformTextureOverride
		{
			// Token: 0x0400A68A RID: 42634
			public TextureArrayConfig.TextureArrayGroup settings = new TextureArrayConfig.TextureArrayGroup();
		}

		// Token: 0x02001D20 RID: 7456
		[Serializable]
		public class TextureEntry
		{
			// Token: 0x0600DC0D RID: 56333 RVA: 0x004EDE64 File Offset: 0x004EC064
			public void Reset()
			{
				this.diffuse = null;
				this.height = null;
				this.normal = null;
				this.smoothness = null;
				this.specular = null;
				this.ao = null;
				this.isRoughness = false;
				this.detailNoise = null;
				this.distanceNoise = null;
				this.metal = null;
				this.emis = null;
				this.heightChannel = TextureArrayConfig.TextureChannel.G;
				this.smoothnessChannel = TextureArrayConfig.TextureChannel.G;
				this.aoChannel = TextureArrayConfig.TextureChannel.G;
				this.distanceChannel = TextureArrayConfig.TextureChannel.G;
				this.detailChannel = TextureArrayConfig.TextureChannel.G;
				this.traxDiffuse = null;
				this.traxNormal = null;
				this.traxHeight = null;
				this.traxSmoothness = null;
				this.traxAO = null;
				this.traxHeightChannel = TextureArrayConfig.TextureChannel.G;
				this.traxSmoothnessChannel = TextureArrayConfig.TextureChannel.G;
				this.traxAOChannel = TextureArrayConfig.TextureChannel.G;
				this.splat = null;
			}

			// Token: 0x0600DC0E RID: 56334 RVA: 0x004EDF20 File Offset: 0x004EC120
			public bool HasTextures(TextureArrayConfig.PBRWorkflow wf)
			{
				if (wf == TextureArrayConfig.PBRWorkflow.Specular)
				{
					return this.diffuse != null || this.height != null || this.normal != null || this.smoothness != null || this.specular != null || this.ao != null;
				}
				return this.diffuse != null || this.height != null || this.normal != null || this.smoothness != null || this.metal != null || this.ao != null;
			}

			// Token: 0x0400A68B RID: 42635
			public Texture2D diffuse;

			// Token: 0x0400A68C RID: 42636
			public Texture2D height;

			// Token: 0x0400A68D RID: 42637
			public TextureArrayConfig.TextureChannel heightChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A68E RID: 42638
			public Texture2D normal;

			// Token: 0x0400A68F RID: 42639
			public Texture2D smoothness;

			// Token: 0x0400A690 RID: 42640
			public TextureArrayConfig.TextureChannel smoothnessChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A691 RID: 42641
			public bool isRoughness;

			// Token: 0x0400A692 RID: 42642
			public Texture2D ao;

			// Token: 0x0400A693 RID: 42643
			public TextureArrayConfig.TextureChannel aoChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A694 RID: 42644
			public Texture2D emis;

			// Token: 0x0400A695 RID: 42645
			public Texture2D metal;

			// Token: 0x0400A696 RID: 42646
			public TextureArrayConfig.TextureChannel metalChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A697 RID: 42647
			public Texture2D specular;

			// Token: 0x0400A698 RID: 42648
			public Texture2D noiseNormal;

			// Token: 0x0400A699 RID: 42649
			public Texture2D detailNoise;

			// Token: 0x0400A69A RID: 42650
			public TextureArrayConfig.TextureChannel detailChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A69B RID: 42651
			public Texture2D distanceNoise;

			// Token: 0x0400A69C RID: 42652
			public TextureArrayConfig.TextureChannel distanceChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A69D RID: 42653
			public Texture2D traxDiffuse;

			// Token: 0x0400A69E RID: 42654
			public Texture2D traxHeight;

			// Token: 0x0400A69F RID: 42655
			public TextureArrayConfig.TextureChannel traxHeightChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A6A0 RID: 42656
			public Texture2D traxNormal;

			// Token: 0x0400A6A1 RID: 42657
			public Texture2D traxSmoothness;

			// Token: 0x0400A6A2 RID: 42658
			public TextureArrayConfig.TextureChannel traxSmoothnessChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A6A3 RID: 42659
			public bool traxIsRoughness;

			// Token: 0x0400A6A4 RID: 42660
			public Texture2D traxAO;

			// Token: 0x0400A6A5 RID: 42661
			public TextureArrayConfig.TextureChannel traxAOChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A6A6 RID: 42662
			public Texture2D splat;
		}
	}
}
