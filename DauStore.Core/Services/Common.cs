using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DauStore.Core.Services
{
    public class Common
    {
        /// <summary>
        /// Kiểm tra một email có hợp lệ hay không
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// Author hieunv 16/08/2021
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// Kiểm tra một số điện thoại có hợp lệ hay không
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// Author hieunv 
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$");
        }

        /// <summary>
        /// Kiểm tra một số điện thoại có hợp lệ hay không
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// Author hieunv 
        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^[^@$!%*#?&]{6,10}$");
        }
    }
}
