using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PostEffects
{
	// Token: 0x02001D3A RID: 7482
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public sealed class ContactShadows : MonoBehaviour
	{
		// Token: 0x17001B4F RID: 6991
		// (get) Token: 0x0600DCC7 RID: 56519 RVA: 0x004F34FB File Offset: 0x004F16FB
		// (set) Token: 0x0600DCC8 RID: 56520 RVA: 0x004F3503 File Offset: 0x004F1703
		public Light Light
		{
			get
			{
				return this._light;
			}
			set
			{
				this._light = value;
			}
		}

		// Token: 0x0600DCC9 RID: 56521 RVA: 0x004F350C File Offset: 0x004F170C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDestroy()
		{
			if (this._material != null)
			{
				if (Application.isPlaying)
				{
					global::UnityEngine.Object.Destroy(this._material);
				}
				else
				{
					global::UnityEngine.Object.DestroyImmediate(this._material);
				}
			}
			if (this._prevMaskRT1 != null)
			{
				RenderTexture.ReleaseTemporary(this._prevMaskRT1);
			}
			if (this._prevMaskRT2 != null)
			{
				RenderTexture.ReleaseTemporary(this._prevMaskRT2);
			}
			if (this._command1 != null)
			{
				this._command1.Release();
			}
			if (this._command2 != null)
			{
				this._command2.Release();
			}
		}

		// Token: 0x0600DCCA RID: 56522 RVA: 0x004F359E File Offset: 0x004F179E
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPreCull()
		{
			this.UpdateTempObjects();
			if (this._light != null)
			{
				this.BuildCommandBuffer();
				this._light.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this._command1);
				this._light.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this._command2);
			}
		}

		// Token: 0x0600DCCB RID: 56523 RVA: 0x004F35E0 File Offset: 0x004F17E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPreRender()
		{
			if (this._light != null)
			{
				this._light.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, this._command1);
				this._light.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, this._command2);
				this._command1.Clear();
				this._command2.Clear();
			}
		}

		// Token: 0x0600DCCC RID: 56524 RVA: 0x004F3635 File Offset: 0x004F1835
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x0600DCCD RID: 56525 RVA: 0x004F364C File Offset: 0x004F184C
		[PublicizedFrom(EAccessModifier.Private)]
		public static Matrix4x4 CalculateVPMatrix()
		{
			Camera current = Camera.current;
			Matrix4x4 nonJitteredProjectionMatrix = current.nonJitteredProjectionMatrix;
			Matrix4x4 worldToCameraMatrix = current.worldToCameraMatrix;
			return GL.GetGPUProjectionMatrix(nonJitteredProjectionMatrix, true) * worldToCameraMatrix;
		}

		// Token: 0x0600DCCE RID: 56526 RVA: 0x004F3678 File Offset: 0x004F1878
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2Int GetScreenSize()
		{
			Camera current = Camera.current;
			int num = (this._downsample ? 2 : 1);
			return new Vector2Int(current.pixelWidth / num, current.pixelHeight / num);
		}

		// Token: 0x0600DCCF RID: 56527 RVA: 0x004F36B0 File Offset: 0x004F18B0
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateTempObjects()
		{
			if (this._prevMaskRT2 != null)
			{
				RenderTexture.ReleaseTemporary(this._prevMaskRT2);
				this._prevMaskRT2 = null;
			}
			if (this._light == null)
			{
				return;
			}
			if (this._material == null)
			{
				this._material = new Material(this._shader);
				this._material.hideFlags = HideFlags.DontSave;
			}
			if (this._command1 == null)
			{
				this._command1 = new CommandBuffer();
				this._command2 = new CommandBuffer();
				this._command1.name = "Contact Shadow Ray Tracing";
				this._command2.name = "Contact Shadow Temporal Filter";
			}
			else
			{
				this._command1.Clear();
				this._command2.Clear();
			}
			this._material.SetFloat("_RejectionDepth", this._rejectionDepth);
			this._material.SetInt("_SampleCount", this._sampleCount);
			float num = Mathf.Pow(1f - this._temporalFilter, 2f);
			this._material.SetFloat("_Convergence", num);
			this._material.SetVector("_LightVector", base.transform.InverseTransformDirection(-this._light.transform.forward) * this._light.shadowBias / ((float)this._sampleCount - 1.5f));
			Texture2D texture = this._noiseTextures.GetTexture();
			Vector2 vector = this.GetScreenSize() / (float)texture.width;
			this._material.SetVector("_NoiseScale", vector);
			this._material.SetTexture("_NoiseTex", texture);
			this._material.SetMatrix("_Reprojection", this._previousVP * base.transform.localToWorldMatrix);
			this._previousVP = ContactShadows.CalculateVPMatrix();
		}

		// Token: 0x0600DCD0 RID: 56528 RVA: 0x004F3898 File Offset: 0x004F1A98
		[PublicizedFrom(EAccessModifier.Private)]
		public void BuildCommandBuffer()
		{
			Vector2Int screenSize = this.GetScreenSize();
			RenderTextureFormat renderTextureFormat = RenderTextureFormat.R8;
			RenderTexture temporary = RenderTexture.GetTemporary(screenSize.x, screenSize.y, 0, renderTextureFormat);
			if (this._temporalFilter == 0f)
			{
				this._command1.SetGlobalTexture(Shader.PropertyToID("_ShadowMask"), BuiltinRenderTextureType.CurrentActive);
				this._command1.SetRenderTarget(temporary);
				this._command1.DrawProcedural(Matrix4x4.identity, this._material, 0, MeshTopology.Triangles, 3);
			}
			else
			{
				int num = Shader.PropertyToID("_UnfilteredMask");
				this._command1.SetGlobalTexture(Shader.PropertyToID("_ShadowMask"), BuiltinRenderTextureType.CurrentActive);
				this._command1.GetTemporaryRT(num, screenSize.x, screenSize.y, 0, FilterMode.Point, renderTextureFormat);
				this._command1.SetRenderTarget(num);
				this._command1.DrawProcedural(Matrix4x4.identity, this._material, 0, MeshTopology.Triangles, 3);
				this._command1.SetGlobalTexture(Shader.PropertyToID("_PrevMask"), this._prevMaskRT1);
				this._command1.SetRenderTarget(temporary);
				this._command1.DrawProcedural(Matrix4x4.identity, this._material, 1 + (Time.frameCount & 1), MeshTopology.Triangles, 3);
			}
			if (this._downsample)
			{
				this._command2.SetGlobalTexture(Shader.PropertyToID("_TempMask"), temporary);
				this._command2.DrawProcedural(Matrix4x4.identity, this._material, 3, MeshTopology.Triangles, 3);
			}
			else
			{
				this._command2.Blit(temporary, BuiltinRenderTextureType.CurrentActive);
			}
			this._prevMaskRT2 = this._prevMaskRT1;
			this._prevMaskRT1 = temporary;
		}

		// Token: 0x0400A792 RID: 42898
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public Light _light;

		// Token: 0x0400A793 RID: 42899
		[SerializeField]
		[Range(0f, 5f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public float _rejectionDepth = 0.05f;

		// Token: 0x0400A794 RID: 42900
		[SerializeField]
		[Range(4f, 32f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public int _sampleCount = 16;

		// Token: 0x0400A795 RID: 42901
		[SerializeField]
		[Range(0f, 1f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public float _temporalFilter = 0.5f;

		// Token: 0x0400A796 RID: 42902
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool _downsample = true;

		// Token: 0x0400A797 RID: 42903
		[SerializeField]
		[HideInInspector]
		[PublicizedFrom(EAccessModifier.Private)]
		public Shader _shader;

		// Token: 0x0400A798 RID: 42904
		[SerializeField]
		[HideInInspector]
		[PublicizedFrom(EAccessModifier.Private)]
		public NoiseTextureSet _noiseTextures;

		// Token: 0x0400A799 RID: 42905
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Material _material;

		// Token: 0x0400A79A RID: 42906
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RenderTexture _prevMaskRT1;

		// Token: 0x0400A79B RID: 42907
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RenderTexture _prevMaskRT2;

		// Token: 0x0400A79C RID: 42908
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _command1;

		// Token: 0x0400A79D RID: 42909
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _command2;

		// Token: 0x0400A79E RID: 42910
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Matrix4x4 _previousVP = Matrix4x4.identity;
	}
}
