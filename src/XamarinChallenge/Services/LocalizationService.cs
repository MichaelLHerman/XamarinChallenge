using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinChallenge.Services
{
    public class LocalizationService : ILocalizationService
    {
        public string? GetString(string v)
        {
            return Resources.ResourceManager.GetString(v);
        }
    }
}
