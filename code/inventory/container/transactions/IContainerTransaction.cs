
namespace Eden;

public interface IContainerTransaction
{
	public bool CanDo();
	public bool Execute();
}
