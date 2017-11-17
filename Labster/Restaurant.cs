using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labster
{
    public class Restaurant
    {
        public WeekCollection<OpeningHour> OpeningHours { get; private set; }

        public Restaurant()
        {
            // No opening hours available for restaurant
        }

        public Restaurant(OpeningHour monday, OpeningHour tuesday, OpeningHour wednesday, OpeningHour thursday, 
                          OpeningHour friday, OpeningHour saturday, OpeningHour sunday)
        {
            OpeningHours = new WeekCollection<OpeningHour>(monday, tuesday, wednesday, thursday, friday, saturday, sunday);
        }

        public string GetOpeningHours()
        {
            const string dayHoursTemplate = "{0}: {1}-{2}";
            StringBuilder fullWeekOpeningHours = new StringBuilder();
            IList<Tuple<DayOfWeek, OpeningHour>> hours = new List<Tuple<DayOfWeek, OpeningHour>>();
            int[] daysOfWeekValues = (int[])Enum.GetValues(typeof(DayOfWeek));
            foreach (int dayOfWeek in daysOfWeekValues)
            {
                DayOfWeek day = (DayOfWeek) dayOfWeek;
                hours.Add(new Tuple<DayOfWeek, OpeningHour>(day,OpeningHours.Get(day)));
            }

            //IList<IList<DayOfWeek>> sameClosing = new List<IList<DayOfWeek>>();
            var sameClosing = hours.GroupBy((item => item.Item2.ClosingTime.Hours)).ToList();
            IList<DayOfWeek> processedDays = new List<DayOfWeek>();
            foreach (Tuple<DayOfWeek, OpeningHour> dayHours in hours)
            {
                if (sameClosing.Count > 0)
                {
                    IList<IGrouping<Int32, Tuple<DayOfWeek, OpeningHour>>> same = sameClosing.Where(item => item.Key == dayHours.Item2.ClosingTime.Hours).ToList();
                    if (same.Count == 1)
                    {
                        StringBuilder days = new StringBuilder();
                        IList<Tuple<DayOfWeek, OpeningHour>> sameItems = same[0].ToList();
                        bool nothingToAdd = true;
                        for (int i = 0; i < sameItems.Count; i++)
                        {
                            if (processedDays.Contains(sameItems[i].Item1))
                                continue;
                            processedDays.Add(sameItems[i].Item1);
                            nothingToAdd = false;
                            if (i > 0 && sameItems[i].Item1 == sameItems[i-1].Item1 + 1)
                            {
                                if (i + 1 == sameItems.Count)
                                {
                                    days.Append(" - ");
                                    days.Append(sameItems[i].Item1.ToString().Substring(0, 3));
                                    continue;
                                }
                                if (sameItems[i + 1].Item1 - sameItems[i].Item1 != 1)
                                {
                                    days.Append(" - ");
                                    days.Append(sameItems[i].Item1.ToString().Substring(0, 3));
                                }
                                continue;
                            }
                            if (i > 0) days.Append(", ");
                            days.Append(sameItems[i].Item1.ToString().Substring(0, 3));
                        }
                        if (!nothingToAdd)
                        {
                            fullWeekOpeningHours.Append(String.Format(dayHoursTemplate, days,
                                                                      dayHours.Item2.OpeningTime.Hours, 
                                                                      dayHours.Item2.ClosingTime.Hours));
                            if (processedDays.Count != daysOfWeekValues.Length)
                                fullWeekOpeningHours.Append(", ");
                        }
                    }
                }
            }
            return fullWeekOpeningHours.ToString();
        }
    }

    public class OpeningHour
    {
        public TimeSpan OpeningTime { get; private set; }
        public TimeSpan ClosingTime { get; private set; }

        public OpeningHour(TimeSpan openingTime, TimeSpan closingTime)
        {
            OpeningTime = openingTime;
            ClosingTime = closingTime;
        }

        public OpeningHour(int openingHour, int closingHour)
        {
            OpeningTime = TimeSpan.FromHours(openingHour);
            ClosingTime = TimeSpan.FromHours(closingHour);
        }

    }

    public class WeekCollection<T>
    {
        private Dictionary<DayOfWeek, T> _collection;

        public WeekCollection(T sunday, T monday, T tuesday, T wednesday, T thursday, T friday, T saturday)
        {
            _collection = new Dictionary<DayOfWeek, T>();
            _collection.Add(DayOfWeek.Sunday, sunday);
            _collection.Add(DayOfWeek.Monday, monday);
            _collection.Add(DayOfWeek.Tuesday, tuesday);
            _collection.Add(DayOfWeek.Wednesday, wednesday);
            _collection.Add(DayOfWeek.Thursday, thursday);
            _collection.Add(DayOfWeek.Friday, friday);
            _collection.Add(DayOfWeek.Saturday, saturday);
        }

        public T Get(DayOfWeek dayOfWeek)
        {
            return _collection[dayOfWeek];
        }
    }
}
