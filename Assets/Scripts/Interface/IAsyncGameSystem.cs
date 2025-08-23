using System.Threading.Tasks;

namespace XJXInterface
{
	public interface IAsyncGameSystem : IGameSystem
	{
		Task InitializeAsync();
	}

}
