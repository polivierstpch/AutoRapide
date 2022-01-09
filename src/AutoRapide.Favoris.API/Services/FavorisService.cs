﻿using AutoRapide.Favoris.API.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AutoRapide.Favoris.API.Services
{
    public class FavorisService : IFavorisService
    {
        private readonly ICacheService _cacheService;
        private readonly IMemoryCache _memoryCache;

        private readonly string _cacheKey = "favoris";

        public FavorisService(IMemoryCache memoryCache, ICacheService cacheService)
        {
            _memoryCache = memoryCache;
            _cacheService = cacheService;
        }
        public IEnumerable<int> ObtenirLesFavoris() 
        {
            if (!_memoryCache.TryGetValue(_cacheKey, out List<int> idsVehicules))
            {
                return new List<int>() { };
            }
            else
            {
                return idsVehicules;
            }
        }
        public void AjouterFavori(int idVehicule)
        {
            if (!_memoryCache.TryGetValue(_cacheKey, out List<int> idsVehicules))
            {
                idsVehicules = new List<int>() { idVehicule };
            }
            else
            {
                idsVehicules.Add(idVehicule);
            }
            _cacheService.SetValuesCacheFavoris(idsVehicules);
        }
        public void EffacerFavori(int idVehicule)
        {
            if (_memoryCache.TryGetValue(_cacheKey, out List<int> idsVehicules))
            {
                if (!idsVehicules.Contains(idVehicule))
                {
                    throw new InvalidDataException("Le véhicule ne se trouve pas dans les favoris.");
                }
                idsVehicules.Remove(idVehicule);
            }
            if(idsVehicules == null)
            {
                throw new NullReferenceException("Il n'y a aucun favori d'enregistré.");
            }
            _cacheService.SetValuesCacheFavoris(idsVehicules);
        }
    }
}
