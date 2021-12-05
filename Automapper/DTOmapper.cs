using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Automapper
{
    //mapping DTO classes to main domain models
    public class DTOmapper :Profile
    {

        public DTOmapper()
        {
            //Here we do all the model classes mapping with dto classes
            //In future if we add more models we need to mapp here for respective dtos
            //Reverse mapping indicates mapping in both directions to convert DTO to originalmodel and viceVesa
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
        
    }
}
