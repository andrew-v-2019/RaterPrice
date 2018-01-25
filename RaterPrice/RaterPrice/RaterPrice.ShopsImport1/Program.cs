using RaterPrice.Persistence.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;


namespace RaterPrice.ShopsImport
{
    class Program
    {
        private static string _connectionString;

        static void Main(string[] args)
        {       
            _connectionString = ConfigurationManager.ConnectionStrings["raterPriceConnectionString"].ConnectionString;
            
            var path = ConfigurationManager.AppSettings["file"];
            var shopsInStrings = File.ReadAllLines(path, Encoding.Default);
            shopsInStrings = shopsInStrings.Skip(1).ToArray();
            

            var weekDays = ImportWeekDays();
            var paymentTypes = ImrportPaymentTypes(shopsInStrings);
            var groups = ImrportGroups(shopsInStrings);
            var cities = ImrportCities(shopsInStrings);
            var shops = ImportShops(shopsInStrings, weekDays, paymentTypes, groups, cities);

            Console.WriteLine("Done. " + shops.Count() +" shops have been imported");
        }

        private static List<Shop> ImportShops(string[] shopsInStrings, List<Weekday> weekdays, List<PaymentType> paymentTypes, List<ShopGroup> groups, List<City> cities)
        {
            var domainShops = new List<Shop>();
            foreach (var shopStr in shopsInStrings)
            {
                var domainShop = MapOneShopFromString(shopStr, groups, cities);
                using (RaterPriceContext context = RaterPriceContext.Create())
                {
                    domainShop.Id = context.Shops.Add(domainShop).Id;
                    context.SaveChanges();
                    var paymentTypesForShop = GetPaymentTypesForShop(shopStr, paymentTypes, domainShop.Id);
                    context.ShopPaymentTypes.AddRange(paymentTypesForShop);
                    context.SaveChanges();
                    var shopWorkingDays = GetShopWeekdays(shopStr, weekdays, domainShop.Id);
                    context.ShopWeekdays.AddRange(shopWorkingDays);
                    context.SaveChanges();
                }
                domainShops.Add(domainShop);
                Console.WriteLine("Shop has been imported: " + domainShop.Name);
            }
            return domainShops;
        }

        private static List<ShopWeekDay> GetShopWeekdays(string shopStr, List<Weekday> weekdays, int shopId)
        {
            var shopWeekdays = new List<ShopWeekDay>();
            weekdays = weekdays.OrderBy(w => w.Id).ToList();
            var cellIdx = 16;
            for (var i = 0; i < 7; i++)
            {
                var oneWorkingDay = new ShopWeekDay()
                {
                    ShopId = shopId,
                    WeekdayId = weekdays[i].Id,
                    StartWorkHour = ExtractTimeSpanFromString(shopStr.Split(';')[cellIdx])[0],
                    EndWorkHour = ExtractTimeSpanFromString(shopStr.Split(';')[cellIdx])[1],
                    DinnerBreakStartHour = ExtractTimeSpanFromString(shopStr.Split(';')[cellIdx + 1])[0],
                    DinnerBreakStopHour = ExtractTimeSpanFromString(shopStr.Split(';')[cellIdx + 1])[1]
                };
                cellIdx = cellIdx + 2;
                shopWeekdays.Add(oneWorkingDay);
            }
            return shopWeekdays;
        }

        private static TimeSpan?[] ExtractTimeSpanFromString(string timeRange)
        {
            if (string.IsNullOrEmpty(timeRange.Trim())) return new TimeSpan?[] { null, null };
            if (timeRange.ToLower().IndexOf("выходн")!=-1) return new TimeSpan?[] { null, null };
            if (timeRange.Trim().Equals("00:00-24:00")) return new TimeSpan?[] { null, null };
            var time1HourStr = timeRange.Substring(0, timeRange.IndexOf(":"));
            var time1MinStr = timeRange.Substring(timeRange.IndexOf(":") + 1,2);
            var time2HourStr = timeRange.Substring(timeRange.IndexOf("-") + 1, 2);
            var time2MinStr = timeRange.Substring(timeRange.LastIndexOf(":") + 1, 2);
            var time1Hour = Convert.ToInt32(time1HourStr);
            var time2Hour = Convert.ToInt32(time2HourStr);
            var time1Min = Convert.ToInt32(time1MinStr);
            var time2Min = Convert.ToInt32(time2MinStr);
            var time1 = new TimeSpan(time1Hour, time1Min, 0);
            var time2 = new TimeSpan(time2Hour, time2Min, 0);
            var result = new TimeSpan?[] { time1, time2 };
            return result;
        }

        private static List<ShopPaymentType> GetPaymentTypesForShop(string shopStr, List<PaymentType> paymentTypes, int shopId)
        {
            var paymentTypesStr = shopStr.Split(';')[30].Split(',');

            var domainList = (from str in paymentTypesStr
                              join pt in paymentTypes on str.Trim() equals pt.Name.Trim()
                              select new ShopPaymentType { PaymentTypeId = pt.Id, ShopId = shopId }).ToList();
            return domainList;
        }

        private static Shop MapOneShopFromString(string shopStr, List<ShopGroup> groups, List<City> cities)
        {
            var shopInArray = shopStr.Split(';');
            var domainShop = new Shop()
            {
                Name = shopInArray[1].Trim(),
                Address = shopInArray[6].Trim() + ",Zip: " + shopInArray[15].Trim(),
                Phones = (shopInArray[7] + "," + shopInArray[8] + "," + shopInArray[9]).Trim().Replace(",,",","),
                Site = shopInArray[11].Trim(),
                Email = shopInArray[10].Trim(),
                Latitude = shopInArray[12].Trim(),
                Longitude = shopInArray[13].Trim(),
                Description = shopInArray[14].Trim(),
            };
            var groupId = groups.Where(g => shopInArray[2].IndexOf(g.Name) > -1).Select(g => g.Id).First();
            var cityId = cities.Where(c => shopInArray[5].IndexOf(c.Name) > -1).Select(c => c.Id).First();
            var subGroupId = groups.Where(g => shopInArray[3].IndexOf(g.Name) > -1).Select(g => g.Id).First();

            domainShop.GroupId = groupId;
            domainShop.SubGroupId = subGroupId;
            domainShop.CityId = cityId;
            return domainShop;
        }

        private static List<Weekday> ImportWeekDays()
        {
            var days = Enum.GetValues(typeof(DaysOfWeek)).Cast<DaysOfWeek>().ToList();
            var domainList = days.Select(d => new Weekday()
            {
                Id = (int) d,
                Name = d.ToString()
            }).ToList();
            domainList = domainList.OrderBy(d => d.Id).ToList();
            foreach (var d in domainList)
            {
                using (RaterPriceContext context = RaterPriceContext.Create())
                {
                    if (context.Weekdays.Any(da => da.Name == d.Name)) continue;
                    d.Id = context.Weekdays.Add(d).Id;
                    context.SaveChanges();
                    Console.WriteLine("Working day has been imported: " + d.Name);
                }
            }         
            return domainList;
        }

        private static List<PaymentType> ImrportPaymentTypes(string[] shops)
        {
            var paymentTypeList = new List<string>();
            foreach(var shop in shops)
            {
                var paymentTypesForOneItem = shop.Split(';')[30].Split(',');
                paymentTypeList.AddRange(paymentTypesForOneItem);
            }
            var groupedList = paymentTypeList.GroupBy(x => x).Select(x => x.Key).ToList();
            groupedList = groupedList.Where(g => !string.IsNullOrWhiteSpace(g)).Select(s => s).ToList();
            var domainList = new List<PaymentType>();
            using (RaterPriceContext context = RaterPriceContext.Create())
            {
                foreach (var pt in groupedList)
                {
                    var domainItem = new PaymentType()
                    {
                        Name = pt.Trim()
                    };
                    if(!context.PaymentTypes.Any(p=>p.Name.Equals(domainItem.Name)))
                    {
                        domainItem.Id = context.PaymentTypes.Add(domainItem).Id;
                        context.SaveChanges();
                        Console.WriteLine("Payment type has been imported: " + domainItem.Name);
                    }
                    else
                    {
                        domainItem.Id = context.PaymentTypes.Where(p=>p.Name.Equals(domainItem.Name)).Select(p=>p.Id).First();
                    }
                    domainList.Add(domainItem);
                }
            }
            return domainList;
        }


        private static List<ShopGroup> ImrportGroups(string[] shops)
        {
            var groupsList = new List<string>();
            foreach (var shop in shops)
            {
                var groupsForOneItem1 = shop.Split(';')[2];
                var groupsForOneItem2 = shop.Split(';')[3];
                groupsList.Add(groupsForOneItem1);
                groupsList.Add(groupsForOneItem2);
            }
            var groupedList = groupsList.GroupBy(x => x).Select(x => x.Key).ToList();
            groupedList = groupedList.Where(g => !string.IsNullOrWhiteSpace(g)).Select(s => s).ToList();
            var domainList = new List<ShopGroup>();
            using (RaterPriceContext context = RaterPriceContext.Create())
            {
                foreach (var gr in groupedList)
                {
                    var domainItem = new ShopGroup()
                    {
                        Name = gr.Trim()
                    };
                    if (!context.ShopGroups.Any(g => g.Name.Equals(domainItem.Name)))
                    {
                        domainItem.Id = context.ShopGroups.Add(domainItem).Id;
                        context.SaveChanges();
                        Console.WriteLine("Group has been imported: " + domainItem.Name);
                    }
                    else
                    {
                        domainItem.Id = context.ShopGroups.Where(p => p.Name.Equals(domainItem.Name)).Select(p => p.Id).First();
                    }
                    domainList.Add(domainItem);
                }
            }
            return domainList;
        }

        private static List<City> ImrportCities(string[] shops)
        {
            var citiesList = shops.Select(shop => shop.Split(';')[5]).ToList();
            var groupedList = citiesList.GroupBy(x => x).Select(x => x.Key).ToList();
            groupedList = groupedList.Where(g => !string.IsNullOrWhiteSpace(g)).Select(s => s).ToList();
            var domainList = new List<City>();
            using (RaterPriceContext context = RaterPriceContext.Create())
            {
                foreach (var gr in groupedList)
                {
                    var domainItem = new City()
                    {
                        Name = gr.Trim()
                    };
                    if (!context.Cities.Any(g => g.Name.Equals(domainItem.Name)))
                    {
                        domainItem.Id = context.Cities.Add(domainItem).Id;
                        context.SaveChanges();
                        Console.WriteLine("City has been imported: " + domainItem.Name);
                    }
                    else
                    {
                        domainItem.Id = context.Cities.Where(p => p.Name.Equals(domainItem.Name)).Select(p => p.Id).First();
                    }
                    domainList.Add(domainItem);
                }
            }
            return domainList;
        }
    }
}
