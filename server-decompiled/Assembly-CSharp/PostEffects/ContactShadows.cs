using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PostEffects
{
	// Token: 0x02001DC5 RID: 7621
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public sealed class ContactShadows : MonoBehaviour
	{
		// Token: 0x17001CAF RID: 7343
		// (get) Token: 0x0600E0F5 RID: 57589 RVA: 0x005026C3 File Offset: 0x005008C3
		// (set) Token: 0x0600E0F6 RID: 57590 RVA: 0x005026CB File Offset: 0x005008CB
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

		// Token: 0x0600E0F7 RID: 57591 RVA: 0x005026D4 File Offset: 0x005008D4
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

		// Token: 0x0600E0F8 RID: 57592 RVA: 0x00502766 File Offset: 0x00500966
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

		// Token: 0x0600E0F9 RID: 57593 RVA: 0x005027A8 File Offset: 0x005009A8
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

		// Token: 0x0600E0FA RID: 57594 RVA: 0x005027FD File Offset: 0x005009FD
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x0600E0FB RID: 57595 RVA: 0x00502814 File Offset: 0x00500A14
		[PublicizedFrom(EAccessModifier.Private)]
		public static Matrix4x4 CalculateVPMatrix()
		{
			Camera current = Camera.current;
			Matrix4x4 nonJitteredProjectionMatrix = current.nonJitteredProjectionMatrix;
			Matrix4x4 worldToCameraMatrix = current.worldToCameraMatrix;
			return GL.GetGPUProjectionMatrix(nonJitteredProjectionMatrix, true) * worldToCameraMatrix;
		}

		// Token: 0x0600E0FC RID: 57596 RVA: 0x00502840 File Offset: 0x00500A40
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2Int GetScreenSize()
		{
			Camera current = Camera.current;
			int num = (this._downsample ? 2 : 1);
			return new Vector2Int(current.pixelWidth / num, current.pixelHeight / num);
		}

		// Token: 0x0600E0FD RID: 57597 RVA: 0x00502878 File Offset: 0x00500A78
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

		// Token: 0x0600E0FE RID: 57598 RVA: 0x00502A60 File Offset: 0x00500C60
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

		// Token: 0x0400AA87 RID: 43655
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public Light _light;

		// Token: 0x0400AA88 RID: 43656
		[SerializeField]
		[Range(0f, 5f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public float _rejectionDepth = 0.05f;

		// Token: 0x0400AA89 RID: 43657
		[SerializeField]
		[Range(4f, 32f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public int _sampleCount = 16;

		// Token: 0x0400AA8A RID: 43658
		[SerializeField]
		[Range(0f, 1f)]
		[PublicizedFrom(EAccessModifier.Private)]
		public float _temporalFilter = 0.5f;

		// Token: 0x0400AA8B RID: 43659
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool _downsample = true;

		// Token: 0x0400AA8C RID: 43660
		[SerializeField]
		[HideInInspector]
		[PublicizedFrom(EAccessModifier.Private)]
		public Shader _shader;

		// Token: 0x0400AA8D RID: 43661
		[SerializeField]
		[HideInInspector]
		[PublicizedFrom(EAccessModifier.Private)]
		public NoiseTextureSet _noiseTextures;

		// Token: 0x0400AA8E RID: 43662
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Material _material;

		// Token: 0x0400AA8F RID: 43663
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RenderTexture _prevMaskRT1;

		// Token: 0x0400AA90 RID: 43664
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public RenderTexture _prevMaskRT2;

		// Token: 0x0400AA91 RID: 43665
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _command1;

		// Token: 0x0400AA92 RID: 43666
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public CommandBuffer _command2;

		// Token: 0x0400AA93 RID: 43667
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Matrix4x4 _previousVP = Matrix4x4.identity;
	}
}
