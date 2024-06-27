using EduPortal.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface IAddressRepository:  IGenericRepository<Ad_IcKapi, int>
    {
        /// <summary>
        /// Asynchronously creates counter numbers for internal doors and adds them to the database.
        /// If a counter number already exists for an internal door, it updates the existing one.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateCounterNumberAsync();

        /// <summary>
        /// Asynchronously retrieves address-related data from the database.
        /// </summary>
        /// <returns>A collection of objects containing address-related data, including districts, neighborhoods, streets, external doors, and internal doors.</returns>
        Task<IEnumerable<object>> GetAddressData();

        /// <summary>
        /// Asynchronously retrieves address-counter data from the database.
        /// </summary>
        /// <param name="doorNumber"></param>
        /// <returns></returns>
        Task<Ad_Sayac> GetCounterNumber(string doorNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mahalleAdi"></param>
        /// <returns></returns>
        Task<int> GetMahalleKimlikNoByNameAsync(string mahalleAdi);

        
        List<Subscriber> GetSubscriberList(List<long> mahalleKimlikNumarasi);

        Task<Ad_IcKapi> GetAsync(int neighborhoodId);


    }

}
