using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extension
{
    public static class DateTimeExtension
    {
        public static int calculateAge(this DateTime dob){
          var Today=DateTime.Today;
          var age=Today.Year-dob.Year;
          if(dob.Date>Today.AddYears(-age)) age--;
          return age;
        }
    }
}