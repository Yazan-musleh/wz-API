using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whatsapp.Domain.DTOs;
using whatsapp.Domain.Entities;
using whatsapp.Domain.RepoContract;
using whatsapp.Infastructure.Database;

namespace whatsapp.Application.Repo
{
    public class PhoneNumRepo : IPhoneNumRepo
    {
        private readonly WhatsappApiContext _context;

        public PhoneNumRepo(WhatsappApiContext context)
        {
            _context = context;
        }

        public async Task AddPhoneNums(IList<PhoneNumber> phoneNumbers)
        {
            await _context.PhoneNumbers.AddRangeAsync(phoneNumbers);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<PhoneNumber>> GetPhoneNums()
        {
            return await _context.PhoneNumbers.ToListAsync();
        }

    }
}
