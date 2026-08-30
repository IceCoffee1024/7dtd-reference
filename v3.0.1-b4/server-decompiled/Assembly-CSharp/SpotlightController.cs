using System;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public class SpotlightController : MonoBehaviour, IPowerSystemCamera
{
	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06001DBD RID: 7613 RVA: 0x000B542D File Offset: 0x000B362D
	// (set) Token: 0x06001DBE RID: 7614 RVA: 0x000B5435 File Offset: 0x000B3635
	public int UserAccessingId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x06001DBF RID: 7615 RVA: 0x000B5440 File Offset: 0x000B3640
	public void Init(DynamicProperties _properties)
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		if (this.Cone != null)
		{
			MeshRenderer component = this.Cone.GetComponent<MeshRenderer>();
			if (component != null)
			{
				if (component.material != null)
				{
					this.ConeMaterial = component.material;
					this.ConeColor = this.ConeMaterial.GetColor("_Color");
				}
				else if (component.sharedMaterial != null)
				{
					this.ConeMaterial = component.sharedMaterial;
					this.ConeColor = this.ConeMaterial.GetColor("_Color");
				}
			}
			this.Cone.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x000B54F8 File Offset: 0x000B36F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.TileEntity == null)
		{
			return;
		}
		if (this.TileEntity.IsPowered && this.UserAccessingId == -1)
		{
			if (this.TileEntity.IsPowered)
			{
				if (this.YawController.Yaw != this.TileEntity.CenteredYaw)
				{
					this.YawController.Yaw = Mathf.Lerp(this.YawController.Yaw, this.TileEntity.CenteredYaw, Time.deltaTime * this.degreesPerSecond);
					this.YawController.UpdateYaw();
				}
				if (this.PitchController.Pitch != this.TileEntity.CenteredPitch)
				{
					this.PitchController.Pitch = Mathf.Lerp(this.PitchController.Pitch, this.TileEntity.CenteredPitch, Time.deltaTime * this.degreesPerSecond);
					this.PitchController.UpdatePitch();
				}
			}
			this.IsOn &= this.TileEntity.IsPowered;
			if (this.LightScript.bSwitchedOn != this.IsOn)
			{
				this.UpdateEmissionColor(this.IsOn);
				this.LightScript.bSwitchedOn = this.IsOn;
			}
			return;
		}
		if (this.UserAccessingId != -1)
		{
			this.YawController.Yaw = this.TileEntity.CenteredYaw;
			this.YawController.UpdateYaw();
			this.PitchController.Pitch = this.TileEntity.CenteredPitch;
			this.PitchController.UpdatePitch();
			return;
		}
		if (!this.TileEntity.IsPowered)
		{
			if (this.YawController.Yaw != this.TileEntity.CenteredYaw)
			{
				this.YawController.Yaw = this.TileEntity.CenteredYaw;
				this.YawController.SetYaw();
			}
			if (this.PitchController.Pitch != this.TileEntity.CenteredPitch)
			{
				this.PitchController.Pitch = this.TileEntity.CenteredPitch;
				this.PitchController.SetPitch();
			}
		}
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x000B56F8 File Offset: 0x000B38F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEmissionColor(bool isPowered)
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].material != componentsInChildren[i].sharedMaterial)
				{
					componentsInChildren[i].material = new Material(componentsInChildren[i].sharedMaterial);
				}
				if (isPowered)
				{
					componentsInChildren[i].material.SetColor("_EmissionColor", Color.white);
				}
				else
				{
					componentsInChildren[i].material.SetColor("_EmissionColor", Color.black);
				}
				componentsInChildren[i].sharedMaterial = componentsInChildren[i].material;
			}
		}
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x000B5791 File Offset: 0x000B3991
	public void SetPitch(float pitch)
	{
		this.TileEntity.CenteredPitch = pitch;
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x000B579F File Offset: 0x000B399F
	public void SetYaw(float yaw)
	{
		this.TileEntity.CenteredYaw = yaw;
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x000B57AD File Offset: 0x000B39AD
	public float GetPitch()
	{
		return this.TileEntity.CenteredPitch;
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000B57BA File Offset: 0x000B39BA
	public float GetYaw()
	{
		return this.TileEntity.CenteredYaw;
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public Transform GetCameraTransform()
	{
		return null;
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x000B57C7 File Offset: 0x000B39C7
	public void SetUserAccessing(int userAccessingId)
	{
		this.UserAccessingId = userAccessingId;
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000B57D0 File Offset: 0x000B39D0
	public void SetConeColor(Color _color)
	{
		if (this.ConeMaterial != null)
		{
			this.ConeMaterial.SetColor("_Color", _color);
		}
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000B57F1 File Offset: 0x000B39F1
	public Color GetOriginalConeColor()
	{
		return this.ConeColor;
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x000B57F9 File Offset: 0x000B39F9
	public void SetConeActive(bool _active)
	{
		if (this.Cone != null)
		{
			this.Cone.gameObject.SetActive(_active);
		}
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x000B581A File Offset: 0x000B3A1A
	public bool GetConeActive()
	{
		return this.Cone != null && this.Cone.gameObject.activeSelf;
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x000B583C File Offset: 0x000B3A3C
	public bool HasCone()
	{
		return this.Cone != null;
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool HasLaser()
	{
		return false;
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserColor(Color _color)
	{
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x000AEC5D File Offset: 0x000ACE5D
	public Color GetOriginalLaserColor()
	{
		return Color.black;
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetLaserActive(bool _active)
	{
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool GetLaserActive()
	{
		return false;
	}

	// Token: 0x040013DC RID: 5084
	public AutoTurretYawLerp YawController;

	// Token: 0x040013DD RID: 5085
	public AutoTurretPitchLerp PitchController;

	// Token: 0x040013DE RID: 5086
	public LightLOD LightScript;

	// Token: 0x040013DF RID: 5087
	public Transform Cone;

	// Token: 0x040013E0 RID: 5088
	public Material ConeMaterial;

	// Token: 0x040013E1 RID: 5089
	public Color ConeColor;

	// Token: 0x040013E2 RID: 5090
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x040013E3 RID: 5091
	public bool IsOn;

	// Token: 0x040013E4 RID: 5092
	public TileEntityPowered TileEntity;

	// Token: 0x040013E6 RID: 5094
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initialized;
}
