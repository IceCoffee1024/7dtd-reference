using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PostEffects
{
	// Token: 0x02001D94 RID: 7572
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public sealed class ContactShadows : MonoBehaviour
	{
		// Token: 0x17001C67 RID: 7271
		// (get) Token: 0x0600DFE1 RID: 57313 RVA: 0x004FEB2B File Offset: 0x004FCD2B
		// (set) Token: 0x0600DFE2 RID: 57314 RVA: 0x004FEB33 File Offset: 0x004FCD33
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

		// Token: 0x0600DFE3 RID: 57315 RVA: 0x004FEB3C File Offset: 0x004FCD3C
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

		// Token: 0x0600DFE4 RID: 57316 RVA: 0x004FEBCE File Offset: 0x004FCDCE
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

		// Token: 0x0600DFE5 RID: 57317 RVA: 0x004FEC10 File Offset: 0x004FCE10
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

		// Token: 0x0600DFE6 RID: 57318 RVA: 0x004FEC65 File Offset: 0x004FCE65
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x0600DFE7 RID: 57319 RVA: 0x004FEC7C File Offset: 0x004FCE7C
		[PublicizedFrom(EAccessModifier.Private)]
		public static Matrix4x4 CalculateVPMatrix()
		{
			Camera current = Camera.current;
			Matrix4x4 nonJitteredProjectionMatrix = current.nonJitteredProjectionMatrix;
			Matrix4x4 worldToCameraMatrix = current.worldToCameraMatrix;
			return GL.GetGPUProjectionMatrix(nonJitteredProjectionMatrix, true) * worldToCameraMatrix;
		}

		// Token: 0x0600DFE8 RID: 57320 RVA: 0x004FECA8 File Offset: 0x004FCEA8
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2Int GetScreenSize()
		{
			Camera current = Camera.current;
			int num = (this._downsample ? 2 : 1);
			return new Vector2Int(current.pixelWidth / num, current.pixelHeight / num);
		}

		// Token: 0x0600DFE9 RID: 57321 RVA: 0x004FECE0 File Offset: 0x004FCEE0
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

		// Token: 0x0600DFEA RID: 57322 RVA: 0x004FEEC8 File Offset: 0x004FD0C8
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

		// Token: 0x0400A99E RID: 43422
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public Light _light;

		// Token: 0x0400A99F RID: 43423
		[SerializeField]
		[Range(0f, 5f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public float _rejectionDepth = 0.05f;

		// Token: 0x0400A9A0 RID: 43424
		[SerializeField]
		[Range(4f, 32f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public int _sampleCount = 16;

		// Token: 0x0400A9A1 RID: 43425
		[SerializeField]
		[Range(0f, 1f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public float _temporalFilter = 0.5f;

		// Token: 0x0400A9A2 RID: 43426
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool _downsample = true;

		// Token: 0x0400A9A3 RID: 43427
		[SerializeField]
		[HideInInspector]
		[PublicizedFrom(EAccessModifier.Private)]
		public Shader _shader;

		// Token: 0x0400A9A4 RID: 43428
		[SerializeField]
		[HideInInspector]
		[PublicizedFrom(EAccessModifier.Private)]
		public NoiseTextureSet _noiseTextures;

		// Token: 0x0400A9A5 RID: 43429
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Material _material;

		// Token: 0x0400A9A6 RID: 43430
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RenderTexture _prevMaskRT1;

		// Token: 0x0400A9A7 RID: 43431
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RenderTexture _prevMaskRT2;

		// Token: 0x0400A9A8 RID: 43432
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _command1;

		// Token: 0x0400A9A9 RID: 43433
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _command2;

		// Token: 0x0400A9AA RID: 43434
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Matrix4x4 _previousVP = Matrix4x4.identity;
	}
}
