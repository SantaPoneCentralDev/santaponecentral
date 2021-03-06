﻿using Santa.Api.Models.Survey_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Client_Models
{
    public class NewClientWithResponsesModel
    {
        public Guid clientStatusID { get; set; }
        public string clientName { get; set; }
        public string clientEmail { get; set; }
        public string clientNickname { get; set; }
        public string clientAddressLine1 { get; set; }
        public string clientAddressLine2 { get; set; }
        public string clientCity { get; set; }
        public string clientState { get; set; }
        public string clientPostalCode { get; set; }
        public string clientCountry { get; set; }
        public List<ApiSurveyResponse> responses { get; set; }
        public bool isAdmin { get; set; }
        public bool hasAccount { get; set; }
    }
}
