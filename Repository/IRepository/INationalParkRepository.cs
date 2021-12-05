using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository.IRepository
{
    public interface INationalParkRepository
    {

        List<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int nationalParkId);
        bool NationalParkExists(int id);

        bool NationalParkExists(string name);

        bool CreateNationalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(NationalPark nationalPark);

        bool Save();

    }
}
