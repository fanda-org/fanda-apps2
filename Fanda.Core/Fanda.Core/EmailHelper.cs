﻿using System;
using System.Net.Mail;

namespace Fanda.Core
{
    public static class EmailHelper
    {
        public static bool IsValid(string emailaddress)
        {
            try
            {
                var m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}