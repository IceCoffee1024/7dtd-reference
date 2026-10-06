using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001365 RID: 4965
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Rendering/Colorize")]
public class ImageEffectManager : MonoBehaviour
{
	// Token: 0x06009C83 RID: 40067 RVA: 0x003B1D40 File Offset: 0x003AFF40
	public int GetNumEffects(string _effectGroupName)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (ImageEffectManager.staticEffectGroups == null)
		{
			return 0;
		}
		ImageEffectManager.EffectGroup effectGroup;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroupName, out effectGroup))
		{
			return effectGroup.GetNumEffects();
		}
		return 0;
	}

	// Token: 0x06009C84 RID: 40068 RVA: 0x003B1D74 File Offset: 0x003AFF74
	public string GetEffectName(string _effectGroupName, int _index)
	{
		if (_index <= 0)
		{
			return "Off";
		}
		ImageEffectManager.ValidateStaticClassEffects();
		if (ImageEffectManager.staticEffectGroups == null)
		{
			return "";
		}
		ImageEffectManager.EffectGroup effectGroup;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroupName, out effectGroup))
		{
			return effectGroup.GetEffectName(_index);
		}
		return "";
	}

	// Token: 0x06009C85 RID: 40069 RVA: 0x003B1DBC File Offset: 0x003AFFBC
	public void DisableEffectGroup(string _effectGroupName)
	{
		if (this.enabledEffects == null)
		{
			return;
		}
		Dictionary<int, float> dictionary;
		if (this.enabledEffects.TryGetValue(_effectGroupName, out dictionary))
		{
			this.numEnabledEffects -= dictionary.Count;
			dictionary.Clear();
		}
	}

	// Token: 0x06009C86 RID: 40070 RVA: 0x003B1DFC File Offset: 0x003AFFFC
	public bool SetEffect_Slow(string _effectGroupName, string _effectName, float _newIntensity = 1f)
	{
		if (!this.ValidateEffects())
		{
			return false;
		}
		ImageEffectManager.EffectGroup effectGroup;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroupName, out effectGroup))
		{
			int num = 0;
			ImageEffectManager.Effect[] effects = effectGroup.effects;
			for (int i = 0; i < effects.Length; i++)
			{
				if (effects[i].name.Equals(_effectName))
				{
					return this.SetEffect(_effectGroupName, num, _newIntensity);
				}
				num++;
			}
		}
		return true;
	}

	// Token: 0x06009C87 RID: 40071 RVA: 0x003B1E58 File Offset: 0x003B0058
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ValidateEffects()
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (ImageEffectManager.staticClass == null)
		{
			return false;
		}
		if (ImageEffectManager.staticEffectGroups == null)
		{
			return false;
		}
		if (this.enabledEffects == null)
		{
			this.enabledEffects = new Dictionary<string, Dictionary<int, float>>();
		}
		return true;
	}

	// Token: 0x06009C88 RID: 40072 RVA: 0x003B1E8C File Offset: 0x003B008C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetEffectIntenal(string _effectGroupName, int _index, float _newIntensity = 1f)
	{
		Dictionary<int, float> dictionary;
		if (this.enabledEffects.TryGetValue(_effectGroupName, out dictionary))
		{
			float num = 0f;
			if (dictionary.TryGetValue(_index, out num))
			{
				if (_newIntensity == 0f)
				{
					dictionary.Remove(_index);
					if (dictionary.Count == 0)
					{
						this.enabledEffects.Remove(_effectGroupName);
					}
					this.numEnabledEffects--;
					return;
				}
				dictionary[_index] = _newIntensity;
				return;
			}
			else if (_newIntensity > 0f)
			{
				dictionary.Add(_index, _newIntensity);
				this.numEnabledEffects++;
			}
		}
		else if (_newIntensity > 0f)
		{
			Dictionary<int, float> dictionary2 = new Dictionary<int, float>();
			dictionary2.Add(_index, _newIntensity);
			this.enabledEffects.Add(_effectGroupName, dictionary2);
			this.numEnabledEffects++;
		}
		base.enabled = this.numEnabledEffects > 0;
	}

	// Token: 0x06009C89 RID: 40073 RVA: 0x003B1F57 File Offset: 0x003B0157
	public bool SetEffect(string _effectGroupName, float _newIntensity = 1f)
	{
		return this.SetEffect(_effectGroupName, 0, _newIntensity);
	}

	// Token: 0x06009C8A RID: 40074 RVA: 0x003B1F64 File Offset: 0x003B0164
	public bool SetEffect(string _effectGroupName, string _effectName, float _newIntensity = 1f)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		ImageEffectManager.EffectGroup effectGroup;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroupName, out effectGroup))
		{
			int num = 0;
			ImageEffectManager.Effect[] effects = effectGroup.effects;
			for (int i = 0; i < effects.Length; i++)
			{
				if (effects[i].name.Equals(_effectName))
				{
					return this.SetEffect(_effectGroupName, num, _newIntensity);
				}
				num++;
			}
		}
		return false;
	}

	// Token: 0x06009C8B RID: 40075 RVA: 0x003B1FBB File Offset: 0x003B01BB
	public bool SetEffect(string _effectGroupName, int _index, float _newIntensity = 1f)
	{
		if (_index < 0)
		{
			return false;
		}
		if (!this.ValidateEffects())
		{
			return false;
		}
		this.SetEffectIntenal(_effectGroupName, _index, _newIntensity);
		return true;
	}

	// Token: 0x06009C8C RID: 40076 RVA: 0x003B1FD7 File Offset: 0x003B01D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		ImageEffectManager.ValidateStaticClassEffects();
		ImageEffectManager.staticClass == null;
	}

	// Token: 0x06009C8D RID: 40077 RVA: 0x003B1FEC File Offset: 0x003B01EC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateStaticClassEffects()
	{
		if (ImageEffectManager.validated)
		{
			return;
		}
		if (ImageEffectManager.staticGameObject == null)
		{
			ImageEffectManager.staticGameObject = Resources.Load<GameObject>("Prefabs/ImageEffectsPrefab");
			if (ImageEffectManager.staticGameObject != null)
			{
				ImageEffectManager.staticClass = ImageEffectManager.staticGameObject.GetComponent<ImageEffectManager>();
			}
		}
		if (ImageEffectManager.staticClass == null)
		{
			return;
		}
		if (ImageEffectManager.staticEffectGroups == null)
		{
			ImageEffectManager.staticEffectGroups = new Dictionary<string, ImageEffectManager.EffectGroup>();
		}
		if (ImageEffectManager.staticClass.effectGroups != null)
		{
			foreach (ImageEffectManager.EffectGroup effectGroup in ImageEffectManager.staticClass.effectGroups)
			{
				ImageEffectManager.staticEffectGroups.Add(effectGroup.name, effectGroup);
			}
		}
		ImageEffectManager.validated = true;
	}

	// Token: 0x06009C8E RID: 40078 RVA: 0x003B2098 File Offset: 0x003B0298
	public void SetFloat_Slow(string _effectGroup, string _effectName, string _propertyName, float _value)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		ImageEffectManager.EffectGroup effectGroup = null;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroup, out effectGroup))
		{
			for (int i = 0; i < effectGroup.effects.Length; i++)
			{
				if (effectGroup.effects[i].name.EqualsCaseInsensitive(_effectName))
				{
					this.SetFloat(effectGroup, i, _propertyName, _value);
					return;
				}
			}
		}
	}

	// Token: 0x06009C8F RID: 40079 RVA: 0x003B20F0 File Offset: 0x003B02F0
	public void SetFloat(string _effectGroup, int _effectIndex, string _propertyName, float _value)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (_effectIndex < 0)
		{
			return;
		}
		ImageEffectManager.EffectGroup effectGroup = null;
		if (ImageEffectManager.staticEffectGroups.TryGetValue(_effectGroup, out effectGroup))
		{
			this.SetFloat(effectGroup, _effectIndex, _propertyName, _value);
		}
	}

	// Token: 0x06009C90 RID: 40080 RVA: 0x003B2123 File Offset: 0x003B0323
	public void SetFloat(ImageEffectManager.EffectGroup _effectGroup, int _effectIndex, string _propertyName, float _value)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (_effectGroup.effects.Length > _effectIndex)
		{
			_effectGroup.effects[_effectIndex].SetFloat(_propertyName, _value);
		}
	}

	// Token: 0x06009C91 RID: 40081 RVA: 0x003B2148 File Offset: 0x003B0348
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (source == null)
		{
			return;
		}
		if (this.enabledEffects == null || this.enabledEffects.Count == 0)
		{
			base.enabled = false;
			return;
		}
		ImageEffectManager.ValidateStaticClassEffects();
		RenderTexture renderTexture = null;
		RenderTexture renderTexture2 = null;
		if (this.enabledEffects.Count > 1)
		{
			renderTexture = RenderTexture.GetTemporary(source.width, source.height);
		}
		if (this.enabledEffects.Count > 2)
		{
			renderTexture2 = RenderTexture.GetTemporary(source.width, source.height);
		}
		bool flag = true;
		int num = 0;
		RenderTexture renderTexture3 = source;
		foreach (KeyValuePair<string, Dictionary<int, float>> keyValuePair in this.enabledEffects)
		{
			foreach (KeyValuePair<int, float> keyValuePair2 in keyValuePair.Value)
			{
				if (ImageEffectManager.staticEffectGroups.ContainsKey(keyValuePair.Key))
				{
					ImageEffectManager.Effect effect = ImageEffectManager.staticEffectGroups[keyValuePair.Key].effects[keyValuePair2.Key];
					if (!(effect.material == null))
					{
						effect.GetMaterial().SetFloat("Intensity", Mathf.Clamp01(keyValuePair2.Value));
						Material material = effect.GetMaterial();
						BlendMode blendMode = BlendMode.OneMinusDstAlpha;
						bool flag2 = false;
						if (effect.hasProperty.TryGetValue("BlendSrc", out flag2) && flag2)
						{
							blendMode = (BlendMode)material.GetInt("BlendSrc");
						}
						effect.UpdateMaterial();
						if (renderTexture == null)
						{
							Graphics.Blit(renderTexture3, destination, effect.GetMaterial());
						}
						else if (flag)
						{
							if (blendMode == BlendMode.Zero)
							{
								Graphics.Blit(renderTexture3, renderTexture, effect.GetMaterial());
								renderTexture3 = renderTexture;
							}
							else
							{
								Graphics.Blit(renderTexture3, renderTexture3, effect.GetMaterial());
							}
							flag = false;
						}
						else if (num == this.numEnabledEffects - 1)
						{
							Graphics.Blit(renderTexture3, destination, effect.GetMaterial());
						}
						else if (blendMode == BlendMode.Zero)
						{
							RenderTexture renderTexture4 = ((renderTexture3 == renderTexture) ? renderTexture2 : renderTexture);
							Graphics.Blit(renderTexture3, renderTexture4, effect.GetMaterial());
							renderTexture3 = renderTexture4;
						}
						else
						{
							Graphics.Blit(renderTexture3, renderTexture3, effect.GetMaterial());
						}
						num++;
					}
				}
			}
		}
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
		if (renderTexture2 != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture2);
		}
	}

	// Token: 0x0400760D RID: 30221
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject staticGameObject;

	// Token: 0x0400760E RID: 30222
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ImageEffectManager staticClass;

	// Token: 0x0400760F RID: 30223
	public static Dictionary<string, ImageEffectManager.EffectGroup> staticEffectGroups;

	// Token: 0x04007610 RID: 30224
	public ImageEffectManager.EffectGroup[] effectGroups;

	// Token: 0x04007611 RID: 30225
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, Dictionary<int, float>> enabledEffects;

	// Token: 0x04007612 RID: 30226
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numEnabledEffects;

	// Token: 0x04007613 RID: 30227
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool validated;

	// Token: 0x02001366 RID: 4966
	[Serializable]
	public class Effect
	{
		// Token: 0x06009C93 RID: 40083 RVA: 0x003B23D8 File Offset: 0x003B05D8
		public Effect()
		{
			this.instantiatedMtrl = null;
			this.hasProperty = new Dictionary<string, bool>();
			this.floatPropertyUpdates = new Dictionary<string, float>();
		}

		// Token: 0x06009C94 RID: 40084 RVA: 0x003B2404 File Offset: 0x003B0604
		public void UpdateMaterial()
		{
			if (this.material == null)
			{
				return;
			}
			foreach (KeyValuePair<string, float> keyValuePair in this.floatPropertyUpdates)
			{
				if (this.needToCheckMaterial)
				{
					if (this.GetMaterial().HasProperty(keyValuePair.Key))
					{
						this.GetMaterial().SetFloat(keyValuePair.Key, keyValuePair.Value);
					}
				}
				else
				{
					this.GetMaterial().SetFloat(keyValuePair.Key, keyValuePair.Value);
				}
			}
			this.floatPropertyUpdates.Clear();
			this.needToCheckMaterial = false;
		}

		// Token: 0x06009C95 RID: 40085 RVA: 0x003B24C4 File Offset: 0x003B06C4
		public void SetFloat(string _propertyName, float _value)
		{
			bool flag = false;
			if (this.hasProperty.TryGetValue(_propertyName, out flag))
			{
				if (flag)
				{
					float num = 0f;
					if (!this.floatPropertyUpdates.TryGetValue(_propertyName, out num))
					{
						this.floatPropertyUpdates.Add(_propertyName, _value);
						return;
					}
					this.floatPropertyUpdates[_propertyName] = _value;
					return;
				}
			}
			else if (this.material != null)
			{
				bool flag2 = this.GetMaterial().HasProperty(_propertyName);
				this.hasProperty.Add(_propertyName, flag2);
				if (flag2)
				{
					this.floatPropertyUpdates.Add(_propertyName, _value);
					return;
				}
			}
			else
			{
				this.needToCheckMaterial = true;
				float num2 = 0f;
				if (!this.floatPropertyUpdates.TryGetValue(_propertyName, out num2))
				{
					this.floatPropertyUpdates.Add(_propertyName, _value);
					return;
				}
				this.floatPropertyUpdates[_propertyName] = _value;
			}
		}

		// Token: 0x06009C96 RID: 40086 RVA: 0x003B258C File Offset: 0x003B078C
		public Material GetMaterial()
		{
			if (this.instantiatedMtrl == null)
			{
				this.instantiatedMtrl = global::UnityEngine.Object.Instantiate<Material>(this.material);
			}
			return this.instantiatedMtrl;
		}

		// Token: 0x04007614 RID: 30228
		public string name;

		// Token: 0x04007615 RID: 30229
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool needToCheckMaterial = true;

		// Token: 0x04007616 RID: 30230
		public Material material;

		// Token: 0x04007617 RID: 30231
		public Material instantiatedMtrl;

		// Token: 0x04007618 RID: 30232
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public GameObject materialHolder;

		// Token: 0x04007619 RID: 30233
		public Dictionary<string, bool> hasProperty;

		// Token: 0x0400761A RID: 30234
		public Dictionary<string, float> floatPropertyUpdates;
	}

	// Token: 0x02001367 RID: 4967
	[Serializable]
	public class EffectGroup
	{
		// Token: 0x06009C97 RID: 40087 RVA: 0x003B25B3 File Offset: 0x003B07B3
		public int GetNumEffects()
		{
			if (this.effects == null)
			{
				return 0;
			}
			return this.effects.Length;
		}

		// Token: 0x06009C98 RID: 40088 RVA: 0x003B25C7 File Offset: 0x003B07C7
		public string GetEffectName(int _index)
		{
			if (this.effects == null)
			{
				return "";
			}
			if (_index < 0)
			{
				return "";
			}
			if (_index >= this.effects.Length)
			{
				return "";
			}
			return this.effects[_index].name;
		}

		// Token: 0x0400761B RID: 30235
		public string name;

		// Token: 0x0400761C RID: 30236
		public ImageEffectManager.Effect[] effects;
	}
}
