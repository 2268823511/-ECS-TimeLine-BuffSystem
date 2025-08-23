namespace XJXInterface
{
	public interface IBuff
	{
		abstract void ApplyBuff();
		abstract void EnterBuff(float time);
		abstract void UpdateBuff(float time);
		abstract void ExitBuff(float time);
		abstract void RemoveBuff();
	}

}