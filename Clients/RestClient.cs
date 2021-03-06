﻿using System;
using System.Threading.Tasks;
using Flurl.Http;
using HomeAutomation.LightingSystem.LocalServer.Dto;
using HomeAutomation.LightingSystem.LocalServer.Utilities.Models;
using Newtonsoft.Json;

namespace HomeAutomation.LightingSystem.LocalServer.Clients
{
    internal class RestClient
    {
        private readonly string _homeAutomationLightSystemApi;
        private readonly string homeLightSystemUrl = "HomeLightSystem";
        private readonly string lightPointUrl = "LightPoint";

        public RestClient(
            string homeAutomationLightSystemApi)
        {
            _homeAutomationLightSystemApi = homeAutomationLightSystemApi;
        }

        public async Task<string> GetToken(string baseApiUrl, Authorizationcredentials authorizationCredentials)
        {
            var respone = await $"{baseApiUrl}/api/Authentication".
                PostJsonAsync(new {
                    Email = authorizationCredentials.UserEmail,
                    Password = authorizationCredentials.UserPassword });

            var contentInJson = respone.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TokenDto>(contentInJson).Token;
        }

        public async Task AddLightPoint(Guid homeLightSystemId, LightPointDto lightPointDto)
        {
            try
            {
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/{homeLightSystemId}".
                PostJsonAsync(lightPointDto);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
              //  throw;
            }
        }
        
        public async Task DisableLightPoint(Guid lightPointId)
        {
            try
            {
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/disableLightPoint/{lightPointId}".PostAsync(null);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task EnableLightPoint(Guid lightPointId)
        {
            try
            {
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/enableLightPoint/{lightPointId}".PostAsync(null);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task DeleteLightPoint(Guid lightPointId)
        {
            try
            {
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/{lightPointId}".DeleteAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task DisableAllLightPoints(Guid homeLightSystemId)
        {
            try
            {
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/disableAllLightPoints/{homeLightSystemId}".PostAsync(null);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task EnableAllLightPoints(Guid homeLightSystemId)
        {
            try
            {
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/enableAllLightPoints/{homeLightSystemId}".PostAsync(null);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task<LightPointDto> GetLightPoints(Guid Id)
        {
            try
            {
                return await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/{Id}".GetJsonAsync<LightPointDto>();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }
    }
}
