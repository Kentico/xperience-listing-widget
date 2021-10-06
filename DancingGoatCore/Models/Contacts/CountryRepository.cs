using System.Collections.Generic;

using CMS.Globalization;

using DancingGoat.Infrastructure;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of countries and states.
    /// </summary>
    public class CountryRepository
    {
        private readonly ICountryInfoProvider countryInfoProvider;
        private readonly IStateInfoProvider stateInfoProvider;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="countryInfoProvider">Provider for <see cref="CountryInfo"/> management.</param>
        /// <param name="stateInfoProvider">Provider for <see cref="StateInfo"/> management.</param>
        public CountryRepository(ICountryInfoProvider countryInfoProvider, IStateInfoProvider stateInfoProvider, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.countryInfoProvider = countryInfoProvider;
            this.stateInfoProvider = stateInfoProvider;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns all available countries.
        /// </summary>
        /// <returns>Collection of all available countries</returns>
        public IEnumerable<CountryInfo> GetAllCountries()
        {
            return repositoryCacheHelper.CacheObjects(() =>
            {
                return countryInfoProvider.Get();
            }, $"{nameof(CountryRepository)}|{nameof(GetAllCountries)}");
        }


        /// <summary>
        /// Returns the country with the specified ID.
        /// </summary>
        /// <param name="countryId">The identifier of the country.</param>
        /// <returns>The country with the specified ID, if found; otherwise, null.</returns>
        public CountryInfo GetCountry(int countryId)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return countryInfoProvider.Get(countryId);
            }, $"{nameof(CountryRepository)}|{nameof(GetCountry)}|{countryId}");
        }


        /// <summary>
        /// Returns the country with the specified code name.
        /// </summary>
        /// <param name="countryName">The code name of the country.</param>
        /// <returns>The country with the specified code name, if found; otherwise, null.</returns>
        public CountryInfo GetCountry(string countryName)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return countryInfoProvider.Get(countryName);
            }, $"{nameof(CountryRepository)}|{nameof(GetCountry)}|{countryName}");
        }

        
        /// <summary>
        /// Returns all states in country with given ID.
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Collection of all states in county.</returns>
        public IEnumerable<StateInfo> GetCountryStates(int countryId)
        {
            return repositoryCacheHelper.CacheObjects(() =>
            {
                return stateInfoProvider.Get().WhereEquals("CountryID", countryId);
            }, $"{nameof(CountryRepository)}|{nameof(GetCountryStates)}|{countryId}");
        }


        /// <summary>
        /// Returns the state with the specified code name.
        /// </summary>
        /// <param name="stateName">The code name of the state.</param>
        /// <returns>The state with the specified code name, if found; otherwise, null.</returns>
        public StateInfo GetState(string stateName)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return stateInfoProvider.Get(stateName);
            }, $"{nameof(CountryRepository)}|{nameof(GetState)}|{stateName}");
        }


        /// <summary>
        /// Returns the state with the specified ID.
        /// </summary>
        /// <param name="stateId">The identifier of the state.</param>
        /// <returns>The state with the specified ID, if found; otherwise, null.</returns>
        public StateInfo GetState(int stateId)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return stateInfoProvider.Get(stateId);
            }, $"{nameof(CountryRepository)}|{nameof(GetState)}|{stateId}");
        }
    }
}