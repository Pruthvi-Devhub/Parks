using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace WebApi.Repository
{


    public class TrailRepository : ITrailRepository
    {

        private readonly ApplicationDbContext _db;
        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(x => x.NationalPark).FirstOrDefault<Trail>(a => a.Id == trailId);

        }

        public List<Trail> GetTrails()
        {
            return _db.Trails.Include(x => x.NationalPark).OrderBy(a => a.Name).ToList();
        }

        //to use FromSqlRaw need to import microsoft.entityframeworkcore
        public List<Trail> GetTrailsinNationalPark(int nationalparkid)
        {
            //Eager loading
            return _db.Trails.Include(x => x.NationalPark).Where(x => x.NationalParkId == nationalparkid).ToList();
          //  return _db.Trails.FromSqlRaw<Trail>("select * from Trails where NationalParkId = nationalparkid").ToList<Trail>();
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Find(id) == null ? false : true;
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool Save()
        {

            return _db.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }
    }
}


