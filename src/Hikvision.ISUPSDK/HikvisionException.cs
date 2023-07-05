﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace Hikvision.ISUPSDK
{
    public class HikvisionException : Exception
    {
        private string _Message;
        public int ErrorCode { get; }
        public override string Message => _Message;
        
        public HikvisionException(int errorCode)
        {
            ErrorCode = errorCode;
            _Message = GetEnumDescription((Errors)ErrorCode);
        }

        private static string GetEnumDescription(Errors value)
        {
            string val = value.ToString();
            FieldInfo fi = value.GetType().GetField(val);
            val = fi?.GetCustomAttribute<DescriptionAttribute>()?.Description;
            if (string.IsNullOrEmpty(val))
                val = value.ToString();
            return val;
        }
    }
}
