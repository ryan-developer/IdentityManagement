using System;
using System.Net;

namespace IdentityManagement.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public int? StatusCode { get; set; }
    }
}
