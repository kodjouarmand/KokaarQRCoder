using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KokaarQrCoder.BusinessLogic.Queries.Contracts
{
    public interface ICompanyQuery : IBaseQuery<CompanyDto, Guid>
    {
        void GenerateQRCode(CompanyDto company, string path = null, bool save = true);
        IEnumerable<CompanyDto> GetAll(ApplicationUser user);
        void GetCompanyPayload(CompanyDto company, ref StringBuilder payload);
    }
}