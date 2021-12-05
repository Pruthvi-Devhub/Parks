using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository.IRepository
{
    public interface ITrailRepository
    {
        List<Trail> GetTrails();
        List<Trail> GetTrailsinNationalPark(int nationalparkid);

        Trail GetTrail(int trailId);
        bool TrailExists(int id);

        bool TrailExists(string name);

        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(Trail trail);

        bool Save();
    }
}
