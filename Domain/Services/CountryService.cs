using Microsoft.EntityFrameworkCore;
using ShoppingAPI_Jueves_2024II.DAL;
using ShoppingAPI_Jueves_2024II.DAL.Entities;
using ShoppingAPI_Jueves_2024II.Domain.Interfaces;

namespace ShoppingAPI_Jueves_2024II.Domain.Services
{
    public class CountryService : ICountryService
    {
        private readonly DataBaseContext _context;

        public CountryService(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return await _context.Countries
                .Include(c => c.States) //include countries and state list
                .ToListAsync();
        }

        public async Task<Country> CreateCountryAsync(Country country)
        {
            try
            {
                country.Id = Guid.NewGuid();
                country.CreatedDate = DateTime.Now;

                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                return country;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }
        }

        public async Task<Country> GetCountryByIdAsync(Guid id)
        {
            
            return await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == id); //FirstOrDefaultAsync es un método de EF CORE
        }

        public async Task<Country> GetCountryByNameAsync(string name)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Country> EditCountryAsync(Country country)
        {
            try
            {
                country.ModifiedDate = DateTime.Now;

                _context.Countries.Update(country); 
                await _context.SaveChangesAsync();

                return country;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }
        }

        public async Task<Country> DeleteCountryAsync(Guid id)
        {
            try
            {
               
                var country = await _context.Countries
                    .Include(c => c.States) 
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (country == null) return null; //Si el país no existe, entonces me retorna un NULL

                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                return country;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }
        }
    }
}
