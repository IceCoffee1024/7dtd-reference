using System;

// Token: 0x0200112E RID: 4398
public class ParentControllerState
{
	// Token: 0x06008B0F RID: 35599 RVA: 0x00351330 File Offset: 0x0034F530
	public ParentControllerState(XUiController parentController)
	{
		if (parentController == null)
		{
			return;
		}
		if (parentController.ViewComponent == null)
		{
			this.m_parentControllers = new XUiController[parentController.Children.Count];
			for (int i = 0; i < parentController.Children.Count; i++)
			{
				this.m_parentControllers[i] = parentController.Children[i];
			}
		}
		else
		{
			this.m_parentControllers = new XUiController[] { parentController };
		}
		this.m_isVisible = new bool[this.m_parentControllers.Length];
		this.m_isEscClosable = new bool[this.m_parentControllers.Length];
		for (int j = 0; j < this.m_parentControllers.Length; j++)
		{
			this.m_isVisible[j] = this.m_parentControllers[j].ViewComponent.IsVisible;
			this.m_isEscClosable[j] = this.m_parentControllers[j].WindowGroup.isEscClosable;
		}
	}

	// Token: 0x06008B10 RID: 35600 RVA: 0x00351410 File Offset: 0x0034F610
	public void Hide()
	{
		if (this.m_parentControllers == null)
		{
			return;
		}
		for (int i = 0; i < this.m_parentControllers.Length; i++)
		{
			this.m_parentControllers[i].ViewComponent.IsVisible = false;
			this.m_parentControllers[i].WindowGroup.isEscClosable = false;
		}
	}

	// Token: 0x06008B11 RID: 35601 RVA: 0x00351460 File Offset: 0x0034F660
	public void Restore()
	{
		if (this.m_parentControllers == null)
		{
			return;
		}
		for (int i = 0; i < this.m_parentControllers.Length; i++)
		{
			this.m_parentControllers[i].ViewComponent.IsVisible = this.m_isVisible[i];
			this.m_parentControllers[i].WindowGroup.isEscClosable = this.m_isEscClosable[i];
		}
	}

	// Token: 0x0400674F RID: 26447
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController[] m_parentControllers;

	// Token: 0x04006750 RID: 26448
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool[] m_isVisible;

	// Token: 0x04006751 RID: 26449
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool[] m_isEscClosable;
}
