using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200132B RID: 4907
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Rendering/Colorize")]
public class ImageEffectManager : MonoBehaviour
{
	// Token: 0x06009ADC RID: 39644 RVA: 0x003A7F1C File Offset: 0x003A611C
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

	// Token: 0x06009ADD RID: 39645 RVA: 0x003A7F50 File Offset: 0x003A6150
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

	// Token: 0x06009ADE RID: 39646 RVA: 0x003A7F98 File Offset: 0x003A6198
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

	// Token: 0x06009ADF RID: 39647 RVA: 0x003A7FD8 File Offset: 0x003A61D8
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

	// Token: 0x06009AE0 RID: 39648 RVA: 0x003A8034 File Offset: 0x003A6234
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

	// Token: 0x06009AE1 RID: 39649 RVA: 0x003A8068 File Offset: 0x003A6268
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

	// Token: 0x06009AE2 RID: 39650 RVA: 0x003A8133 File Offset: 0x003A6333
	public bool SetEffect(string _effectGroupName, float _newIntensity = 1f)
	{
		return this.SetEffect(_effectGroupName, 0, _newIntensity);
	}

	// Token: 0x06009AE3 RID: 39651 RVA: 0x003A8140 File Offset: 0x003A6340
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

	// Token: 0x06009AE4 RID: 39652 RVA: 0x003A8197 File Offset: 0x003A6397
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

	// Token: 0x06009AE5 RID: 39653 RVA: 0x003A81B3 File Offset: 0x003A63B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		ImageEffectManager.ValidateStaticClassEffects();
		ImageEffectManager.staticClass == null;
	}

	// Token: 0x06009AE6 RID: 39654 RVA: 0x003A81C8 File Offset: 0x003A63C8
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

	// Token: 0x06009AE7 RID: 39655 RVA: 0x003A8274 File Offset: 0x003A6474
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

	// Token: 0x06009AE8 RID: 39656 RVA: 0x003A82CC File Offset: 0x003A64CC
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

	// Token: 0x06009AE9 RID: 39657 RVA: 0x003A82FF File Offset: 0x003A64FF
	public void SetFloat(ImageEffectManager.EffectGroup _effectGroup, int _effectIndex, string _propertyName, float _value)
	{
		ImageEffectManager.ValidateStaticClassEffects();
		if (_effectGroup.effects.Length > _effectIndex)
		{
			_effectGroup.effects[_effectIndex].SetFloat(_propertyName, _value);
		}
	}

	// Token: 0x06009AEA RID: 39658 RVA: 0x003A8324 File Offset: 0x003A6524
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

	// Token: 0x040074BA RID: 29882
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject staticGameObject;

	// Token: 0x040074BB RID: 29883
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static ImageEffectManager staticClass;

	// Token: 0x040074BC RID: 29884
	public static Dictionary<string, ImageEffectManager.EffectGroup> staticEffectGroups;

	// Token: 0x040074BD RID: 29885
	public ImageEffectManager.EffectGroup[] effectGroups;

	// Token: 0x040074BE RID: 29886
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, Dictionary<int, float>> enabledEffects;

	// Token: 0x040074BF RID: 29887
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numEnabledEffects;

	// Token: 0x040074C0 RID: 29888
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool validated;

	// Token: 0x0200132C RID: 4908
	[Serializable]
	public class Effect
	{
		// Token: 0x06009AEC RID: 39660 RVA: 0x003A85B4 File Offset: 0x003A67B4
		public Effect()
		{
			this.instantiatedMtrl = null;
			this.hasProperty = new Dictionary<string, bool>();
			this.floatPropertyUpdates = new Dictionary<string, float>();
		}

		// Token: 0x06009AED RID: 39661 RVA: 0x003A85E0 File Offset: 0x003A67E0
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

		// Token: 0x06009AEE RID: 39662 RVA: 0x003A86A0 File Offset: 0x003A68A0
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

		// Token: 0x06009AEF RID: 39663 RVA: 0x003A8768 File Offset: 0x003A6968
		public Material GetMaterial()
		{
			if (this.instantiatedMtrl == null)
			{
				this.instantiatedMtrl = global::UnityEngine.Object.Instantiate<Material>(this.material);
			}
			return this.instantiatedMtrl;
		}

		// Token: 0x040074C1 RID: 29889
		public string name;

		// Token: 0x040074C2 RID: 29890
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public bool needToCheckMaterial = true;

		// Token: 0x040074C3 RID: 29891
		public Material material;

		// Token: 0x040074C4 RID: 29892
		public Material instantiatedMtrl;

		// Token: 0x040074C5 RID: 29893
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public GameObject materialHolder;

		// Token: 0x040074C6 RID: 29894
		public Dictionary<string, bool> hasProperty;

		// Token: 0x040074C7 RID: 29895
		public Dictionary<string, float> floatPropertyUpdates;
	}

	// Token: 0x0200132D RID: 4909
	[Serializable]
	public class EffectGroup
	{
		// Token: 0x06009AF0 RID: 39664 RVA: 0x003A878F File Offset: 0x003A698F
		public int GetNumEffects()
		{
			if (this.effects == null)
			{
				return 0;
			}
			return this.effects.Length;
		}

		// Token: 0x06009AF1 RID: 39665 RVA: 0x003A87A3 File Offset: 0x003A69A3
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

		// Token: 0x040074C8 RID: 29896
		public string name;

		// Token: 0x040074C9 RID: 29897
		public ImageEffectManager.Effect[] effects;
	}
}
