using System.Threading.Tasks;

namespace GraphicCardChecker
{
    public interface IHandler
    {
        Task HandleAsync();
    }
}
