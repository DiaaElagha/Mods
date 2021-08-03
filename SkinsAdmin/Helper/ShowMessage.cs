using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Helper
{
    public static class ShowMessage
    {
        public static string AddSuccessResult(string msg = null)
        {
            return JsonConvert.SerializeObject(
                new
                {
                    status = 1,
                    msg = String.IsNullOrEmpty(msg) ? "s: Added successfully" : msg,
                    close = 1
                });
        }
        public static string SendSuccessResult()
        {
            return JsonConvert.SerializeObject(new { status = 1, msg = "s: Send successfully", close = 1 });
        }

        public static string EditSuccessResult()
        {
            return JsonConvert.SerializeObject(new { status = 1, msg = "s: Edit successfully", close = 1 });
        }

        public static string DeleteSuccessResult()
        {
            return JsonConvert.SerializeObject(new { status = 1, msg = "s: Deleted successfully", close = 1 });
        }

        public static string FailedResult(string msg = null)
        {
            return JsonConvert.SerializeObject(new
            {
                status = 1,
                msg = String.IsNullOrEmpty(msg) ? "e: The operation failed" : "e: " + msg,
                close = 2
            });
        }

        public static string DuplicationResult()
        {
            return JsonConvert.SerializeObject(new { status = 1, msg = "e: The item already exists", close = 2 });
        }

    }


}