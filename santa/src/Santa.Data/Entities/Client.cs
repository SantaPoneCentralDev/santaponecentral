﻿using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class Client
    {
        public Client()
        {
            ClientRelationXrefRecipientClient = new HashSet<ClientRelationXref>();
            ClientRelationXrefSenderClient = new HashSet<ClientRelationXref>();
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public Guid ClientId { get; set; }
        public Guid ClientStatusId { get; set; }
        public string ClientName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public virtual ClientStatus ClientStatus { get; set; }
        public virtual ICollection<ClientRelationXref> ClientRelationXrefRecipientClient { get; set; }
        public virtual ICollection<ClientRelationXref> ClientRelationXrefSenderClient { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}