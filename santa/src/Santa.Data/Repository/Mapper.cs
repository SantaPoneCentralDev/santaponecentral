﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Santa.Data.Entities;
using Santa.Logic.Objects;

namespace Santa.Data.Repository
{
    public static class Mapper
    {
        #region Client
        /// <summary>
        /// maps a logic client to a context client
        /// </summary>
        /// <param name="logicClient"></param>
        /// <returns></returns>
        public static Data.Entities.Client MapClient(Logic.Objects.Client logicClient)
        {
            Entities.Client contextClient = new Entities.Client()
            {
                ClientId = logicClient.clientID,
                ClientName = logicClient.clientName,
                Email = logicClient.email,
                Nickname = logicClient.nickname,
                ClientStatusId = logicClient.clientStatusID,

                AddressLine1 = logicClient.address.addressLineOne,
                AddressLine2 = logicClient.address.addressLineTwo,
                City = logicClient.address.city,
                State = logicClient.address.state,
                PostalCode = logicClient.address.postalCode,
                Country = logicClient.address.country,

            };
            return contextClient;
        }

        /// <summary>
        /// Maps a context client to a logic client
        /// </summary>
        /// <param name="contextCharacter"></param>
        /// <returns></returns>
        public static Logic.Objects.Client MapClient(Entities.Client contextCharacter)
        {
            Logic.Objects.Client logicClient = new Logic.Objects.Client()
            {
                clientID = contextCharacter.ClientId,
                clientStatusID = contextCharacter.ClientStatusId,
                email = contextCharacter.Email,
                nickname = contextCharacter.Nickname,
                clientName = contextCharacter.ClientName,
                address = new Address
                {
                    addressLineOne = contextCharacter.AddressLine1,
                    addressLineTwo = contextCharacter.AddressLine2,
                    city = contextCharacter.City,
                    country = contextCharacter.Country,
                    state = contextCharacter.State,
                    postalCode = contextCharacter.State
                },

                recipients = contextCharacter.ClientRelationXrefSenderClient.Select(s => s.RecipientClientId).ToList(),
                senders = contextCharacter.ClientRelationXrefRecipientClient.Select(r => r.SenderClientId).ToList()
            };

            return logicClient;
        }
        #endregion

        #region Event

        public static Logic.Objects.Event MapEvent(Entities.EventType contextEventType)
        {
            Logic.Objects.Event logicEvent = new Logic.Objects.Event()
            {
                eventTypeID = contextEventType.EventTypeId,
                eventDescription = contextEventType.EventDescription,
                active = contextEventType.IsActive
            };
            return logicEvent;
        }
        #endregion
        #region Survey
        /// <summary>
        /// Maps context survey to a logic survey
        /// </summary>
        /// <param name="contextSurvey"></param>
        /// <returns></returns>
        public static Logic.Objects.Survey MapSurvey(Entities.Survey contextSurvey)
        {
            Logic.Objects.Survey logicSurvey = new Logic.Objects.Survey()
            {
                surveyID = contextSurvey.SurveyId,
                eventTypeID = contextSurvey.EventTypeId,
                surveyDescription = contextSurvey.SurveyDescription,
                active = contextSurvey.IsActive,
                surveyQuestions = contextSurvey.SurveyQuestionXref.Select(Mapper.MapQuestion).ToList(),
            };
            return logicSurvey;
        }
        public static Entities.Survey MapSurvey(Logic.Objects.Survey logicSurvey)
        {
            Data.Entities.Survey contextSurvey = new Entities.Survey()
            {
                SurveyId = logicSurvey.surveyID,
                EventTypeId = logicSurvey.eventTypeID,
                SurveyDescription = logicSurvey.surveyDescription,
                IsActive = logicSurvey.active
            };
            return contextSurvey;
        }
        #endregion
        #region Question
        /// <summary>
        /// Maps a context question to a logic question
        /// </summary>
        /// <param name="contextSurveyQuestion"></param>
        /// <returns></returns>
        public static Logic.Objects.Question MapQuestion(Entities.SurveyQuestionXref contextSurveyQuestion)
        {

            Logic.Objects.Question logicQuestion = new Question(contextSurveyQuestion.SurveyId)
            {
                questionID = contextSurveyQuestion.SurveyQuestionId,
                questionText = contextSurveyQuestion.SurveyQuestion.QuestionText,
                isSurveyOptionList = contextSurveyQuestion.SurveyQuestion.IsSurveyOptionList,
                surveyOptionList = contextSurveyQuestion.SurveyQuestion.SurveyQuestionOptionXref.Select(Mapper.MapQuestionOption).ToList()
            };
            return logicQuestion;
        }

        /// <summary>
        /// maps a logic question to a context question
        /// </summary>
        /// <param name="newQuestion"></param>
        /// <returns></returns>
        public static Data.Entities.SurveyQuestion MapQuestion(Logic.Objects.Question newQuestion)
        {
            
            Entities.SurveyQuestion contextQuestion = new SurveyQuestion()
            {
                SurveyQuestionId = newQuestion.questionID,
                QuestionText = newQuestion.questionText,
                IsSurveyOptionList = newQuestion.isSurveyOptionList
            };
            return contextQuestion;
        }
        public static SurveyQuestionXref MapQuestionXref(Logic.Objects.Question logicQuestion)
        {
            Data.Entities.SurveyQuestionXref contextQuestionXref = new SurveyQuestionXref()
            {
                SurveyId = logicQuestion.surveyID,
                SurveyQuestionId = logicQuestion.questionID,
                IsActive = logicQuestion.isActive,
                SortOrder = logicQuestion.sortOrder
            };
            return contextQuestionXref;
        }
        #region QuestionOption

        public static SurveyQuestionOptionXref MapQuestionOptionXref(Option newQuestionOption)
        {
            Data.Entities.SurveyQuestionOptionXref contextQuestionOptionXref = new SurveyQuestionOptionXref()
            {
                SurveyQuestionId = newQuestionOption.questionID,
                SurveyOptionId = newQuestionOption.surveyOptionID,
                SortOrder = newQuestionOption.sortOrder,
                IsActive = newQuestionOption.isActive
            };
            return contextQuestionOptionXref;
        }
        public static Logic.Objects.Option MapQuestionOption(SurveyQuestionOptionXref contextQuestionOption)
        {
            Logic.Objects.Option logicOption = new Option(contextQuestionOption.SurveyQuestionId)
            {
                surveyOptionID = contextQuestionOption.SurveyOption.SurveyOptionId,
                displayText = contextQuestionOption.SurveyOption.DisplayText,
                surveyOptionValue = contextQuestionOption.SurveyOption.SurveyOptionValue
            };
            return logicOption;
        }
        public static SurveyOption MapSurveyOption(Option newSurveyOption)
        {
            Entities.SurveyOption contextSurveyOption = new SurveyOption()
            {
                SurveyOptionId = newSurveyOption.surveyOptionID,
                DisplayText = newSurveyOption.displayText,
                SurveyOptionValue = newSurveyOption.surveyOptionValue
            };
            return contextSurveyOption;
        }
        #endregion
        #endregion
    }
}