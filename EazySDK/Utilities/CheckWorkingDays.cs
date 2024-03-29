﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace EazySDK.Utilities
{
    public class CheckWorkingDays
    {
        private WorkingDaysReader Reader { get; set; }
        private List<string> BankHolidaysList { get; set; }
        private int WorkingDays { get; set; }
        private int CalendarDays { get; set; }
        private DateTime DateToday { get; set; }
        private DateTime DateStart { get; set; }
        private DateTime WorkingDate { get; set; }
        private DateTime FinalDate { get; set; }
        private bool StartToday { get; set; }

        public DateTime CheckWorkingDaysInFuture(int NumberOfDays, IConfiguration Settings)
        {
            if (NumberOfDays <= 0)
            {
                throw new Exceptions.InvalidSettingsConfigurationException("The number of days in the future must be above 0.");
            }

            Reader = new WorkingDaysReader(Settings);
            BankHolidaysList = Reader.ReadWorkingDaysFile();

            WorkingDays = 0;
            CalendarDays = 0;
            DateToday = DateTime.UtcNow.Date;
            DateStart = DateToday;

            while (WorkingDays <= (NumberOfDays-1))
            {
                WorkingDate = DateStart.AddDays(CalendarDays);
                if (WorkingDate.DayOfWeek != DayOfWeek.Saturday && WorkingDate.DayOfWeek != DayOfWeek.Sunday && !BankHolidaysList.Contains(WorkingDate.ToString("yyyy-MM-dd")))
                {
                    WorkingDays++;
                }
                CalendarDays++;
            }
            FinalDate = DateStart.AddDays(CalendarDays);

            while (FinalDate.DayOfWeek == DayOfWeek.Saturday || FinalDate.DayOfWeek == DayOfWeek.Sunday || BankHolidaysList.Contains(FinalDate.ToString("yyyy-MM-dd"))) {
                FinalDate = FinalDate.AddDays(1);
                CalendarDays++;
            }

            return FinalDate;
        }
    }
}
