﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Santa.Logic.Objects;

namespace Santa.Logic.Interfaces
{
    public interface IRepository
    {
        #region Client
        Task<Logic.Objects.Client> CreateClientAsync();
        Task<Logic.Objects.Client> GetClientByID(Guid clientId);
        Task<Logic.Objects.Client> GetClientByEmailAsync();
        List<Logic.Objects.Client> GetAllClients();
        Task<Logic.Objects.Client> UpdateClientByIDAsync();
        Task<Logic.Objects.Client> DeleteClientByIDAsync();
        #endregion

        #region Event
        Task<Logic.Objects.Event> CreateEventAsync();
        List<Logic.Objects.Event> GetAllEvents();
        Task<Logic.Objects.Event> GetEventByIDAsync();
        Task<Logic.Objects.Event> UpdateEventByIDAsync();
        Task<Logic.Objects.Event> DeleteEventByIDAsync();
        #endregion

        #region Surveys

        Task<Logic.Objects.Survey> getSurveyByID(Guid id);

        #region SurveyOptions
        Task<Logic.Objects.Event> CreateSurveyOptionAsync();
        Task<Logic.Objects.Event> GetSurveyOptionByIDAsync();
        Task<Logic.Objects.Event> UpdateSurveyOptionByIDAsync();
        Task<Logic.Objects.Event> DeleteSurveyOptionByIDAsync();
        #endregion

        #region SurveyQuestions
        Task<Logic.Objects.Event> CreateSurveyQuestionAsync();
        List<Logic.Objects.Survey> getAllSurveys();
        Task<Logic.Objects.Event> GetSurveyQuestionByIDAsync();
        Task<Logic.Objects.Event> UpdateSurveyQuestionByIDAsync();
        Task<Logic.Objects.Event> DeleteSurveyQuestionByIDAsync();
        #endregion

        #region SurveyResponses
        Task<Logic.Objects.Event> CreateSurveyResponseAsync();
        Task<Logic.Objects.Event> GetSurveyResponseByIDAsync();
        Task<Logic.Objects.Event> UpdateSurveyResponseByIDAsync();
        Task<Logic.Objects.Event> DeleteSurveyResponseByIDAsync();
        #endregion
        #endregion
    }
}