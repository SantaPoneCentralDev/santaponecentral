﻿using Xunit;
using System;
using System.Collections.Generic;

namespace Santa.Data.Repository.Tests
{
    public class MapperTests
    {
        /// <summary>
        /// Logic Client to context client
        /// </summary>
        [Fact()]
        public void MapLogicClientToContextClientTest()
        {
            // Arrange    
            Test.SantasLittleHelper helper = new Test.SantasLittleHelper();
            Logic.Objects.Client logicClient = helper.Clients[0];

            // Act
            var mappedClient = Mapper.MapClient(logicClient);

            // Assert 
            Assert.NotNull(mappedClient);
            Assert.IsType<Entities.Client>(mappedClient);

            Assert.NotNull(mappedClient.AddressLine1);
            Assert.NotNull(mappedClient.AddressLine2);
            Assert.NotNull(mappedClient.City);
            Assert.NotNull(mappedClient.Country);
            Assert.NotNull(mappedClient.PostalCode);
            Assert.NotNull(mappedClient.State);
            Assert.NotNull(mappedClient.SurveyResponse);
            Assert.NotNull(mappedClient.ClientName);
            Assert.NotNull(mappedClient.ClientStatus);
            Assert.NotNull(mappedClient.Email);
            Assert.NotNull(mappedClient.Nickname);

            Assert.NotEqual(Guid.Empty, mappedClient.ClientId);
            Assert.NotEqual(Guid.Empty, mappedClient.ClientStatusId);
        }

        /// <summary>
        /// Context client to logic client
        /// </summary>
        [Fact()]
        public void MapContextClientToLogicTest()
        {
            // Arrange
            Guid clientStatusID = Guid.NewGuid();
            Entities.Client contextClient = new Entities.Client()
            {
                ClientId = Guid.NewGuid(),
                ClientStatusId = clientStatusID,
                Email = "test email",
                Nickname = "test nickname",
                ClientName = "test client name",

                AddressLine1 = "Test Address 1",
                AddressLine2 = "Test Address 2",
                City = "Test City",
                Country = "Test Country",
                State = "Test State",
                PostalCode = "Test Postal Code",

                ClientStatus = new Entities.ClientStatus()
                {
                    ClientStatusId = clientStatusID,
                    StatusDescription = "test status description"
                },
                ClientRelationXrefRecipientClient = new List<Entities.ClientRelationXref>(),
                ClientRelationXrefSenderClient = new List<Entities.ClientRelationXref>()
            };

            // Act
            var mappedClient = Mapper.MapClient(contextClient);

            // Assert

            Assert.NotNull(mappedClient);
            Assert.IsType<Logic.Objects.Client>(mappedClient);

            Assert.NotNull(mappedClient.address.addressLineOne);
            Assert.NotNull(mappedClient.address.addressLineTwo);
            Assert.NotNull(mappedClient.address.city);
            Assert.NotNull(mappedClient.address.country);
            Assert.NotNull(mappedClient.address.postalCode);
            Assert.NotNull(mappedClient.address.state);
            Assert.NotNull(mappedClient.clientName);
            Assert.NotNull(mappedClient.email);
            Assert.NotNull(mappedClient.nickname);
            Assert.NotNull(mappedClient.recipients);
            Assert.NotNull(mappedClient.senders);

            Assert.NotEqual(Guid.Empty, mappedClient.clientID);
        }

        /// <summary>
        /// Logic event to context event
        /// </summary>
        [Fact()]
        public void MapLogicEventToContextEventTest()
        {
            // Arrange
            Logic.Objects.Event logicEvent = new Logic.Objects.Event()
            {
                eventTypeID = Guid.NewGuid(),
                eventDescription = "Test event description",
                active = true,
            };
            // Act
            var mappedEvent = Mapper.MapEvent(logicEvent);

            // Assert
            Assert.NotNull(mappedEvent);
            Assert.IsType<Entities.EventType>(mappedEvent);

            Assert.NotNull(logicEvent.eventDescription);
            Assert.IsType<bool>(logicEvent.active);
            Assert.NotEqual(Guid.Empty,logicEvent.eventTypeID);
        }
        /// <summary>
        /// Context event to logic event
        /// </summary>
        [Fact()]
        public void MapContextEventToContextEventTest()
        {
            // Arrange
            Entities.EventType contextEvent = new Entities.EventType()
            {
                EventTypeId = Guid.NewGuid(),
                IsActive = true,
                EventDescription = "Test Event Description"
            };

            // Act
            var mappedEvent = Mapper.MapEvent(contextEvent);

            // Assert
            Assert.NotNull(mappedEvent);
            Assert.IsType<Logic.Objects.Event>(mappedEvent);

            Assert.NotNull(mappedEvent.eventDescription);
            Assert.NotEqual(Guid.Empty, mappedEvent.eventTypeID);
            Assert.IsType<bool>(mappedEvent.active);
        }

        /// <summary>
        /// Logic suvey to context survey
        /// </summary>
        [Fact()]
        public void MapLogicSurveyToContextSurveyTest()
        {
            // Arrange
            Test.SantasLittleHelper helper = new Test.SantasLittleHelper();
            Logic.Objects.Survey logicSurvey = helper.Surveys[0];

            // Act
            var mappedSurvey = Mapper.MapSurvey(logicSurvey);

            // Assert
            Assert.NotNull(mappedSurvey);
            Assert.IsType<Entities.Survey>(mappedSurvey);

            Assert.NotEqual(Guid.Empty, mappedSurvey.SurveyId);
            Assert.NotNull(mappedSurvey.SurveyDescription);
            Assert.NotEqual(Guid.Empty, mappedSurvey.EventTypeId);
            Assert.IsType<bool>(mappedSurvey.IsActive);

        }

        /// <summary>
        /// Context survey to logic survey
        /// </summary>
        [Fact()]
        public void MapSurveyTest1()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logic question to context question
        /// </summary>
        [Fact()]
        public void MapQuestionTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Context question to logic question
        /// </summary>
        [Fact()]
        public void MapQuestionTest1()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Context Question Xref to logic question
        /// </summary>
        [Fact()]
        public void MapQuestionXrefTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logic option to context Question Option Xref
        /// </summary>
        [Fact()]
        public void MapQuestionOptionXrefTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Context Question Option Xref to logic option
        /// </summary>
        [Fact()]
        public void MapQuestionOptionTest()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Logic option to context survey option
        /// </summary>
        [Fact()]
        public void MapSurveyOptionTest()
        {
            throw new NotImplementedException();
        }
    }
}