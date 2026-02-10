using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface IStoneService
    {
        Task<LoginStoneResponseModel> LoginStone(LoginStoneResquestModel request);
    }
}
