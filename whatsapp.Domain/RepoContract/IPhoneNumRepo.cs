using whatsapp.Domain.Entities;

namespace whatsapp.Domain.RepoContract
{
    public interface IPhoneNumRepo
    {
        Task AddPhoneNums(IList<PhoneNumber> phoneNumbers);
        Task<IList<PhoneNumber>> GetPhoneNums();
    }
}
