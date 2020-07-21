using System.IO;
using Microsoft.Extensions.Configuration;


namespace EazySDK
{
    public class SettingsManager 
    {
        public static IConfiguration CreateSettings(string filename)
        {
            
            if (String.IsNullOrEmpty(filename)) { filename = "appSettings.json"; }
            if (!File.Exists(Directory.GetCurrentDirectory() + "/"+filename))
            {
                SettingsWriter writer = new SettingsWriter();
            }


            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(filename, optional: false, reloadOnChange: true)
                .Build();
            return configuration;
        }
        
        public static IConfiguration CreateSettingsInMemory()
        {
            IConfiguration configuration = GetDefaultSettings();
            return configuration;
        }
        
        public static IConfiguration GetDefaultSettings()
        {
            var defaultOptions = new Dictionary<string, string>
            {
                { "currentEnvironment:Environment","ecm3" },

                { "sandboxClientDetails:ApiKey","" },
                { "sandboxClientDetails:ClientCode","" },

                { "ecm3ClientDetails:ApiKey","" },
                { "ecm3ClientDetails:ClientCode","" },

                { "directDebitProcessingDays:InitialProcessingDays","10" },
                { "directDebitProcessingDays:OngoingProcessingDays","5" },

                { "contracts:AutoFixStartDate","false" },
                { "contracts:AutoFixTerminationTypeAdHoc","false" },
                { "contracts:AutoFixAtTheEndAdHoc","false" },
                { "contracts:AutoFixPaymentDayInMonth","false" },
                { "contracts:AutoFixPaymentMonthInYear","false" },

                { "payments:AutoFixPaymentDate","false" },
                { "payments:IsCreditAllowed","false" },

                { "warnings:CustomerSearchWarning","false" },

                { "other:BankHolidayUpdateDays","30" },                
                { "other:ForceUpdateSchedulesOnRun","false" },
            };
            IConfiguration Settings = new ConfigurationBuilder().AddInMemoryCollection(defaultOptions).Build();                  
            return Settings; 
    }
}
