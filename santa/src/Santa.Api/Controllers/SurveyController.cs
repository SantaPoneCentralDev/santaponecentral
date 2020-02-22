﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {

        private readonly IRepository repository;
        public SurveyController(IRepository _repository)
        {
            repository = _repository;
        }
        // GET: api/Survey
        /// <summary>
        /// Gets list of surveys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Logic.Objects.Survey>> Get()
        {
            try
            {
                return Ok(repository.GetAllSurveys());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Survey/5
        /// <summary>
        /// Gets survey by ID
        /// </summary>
        /// <param name="surveyID"></param>
        /// <returns></returns>
        [HttpGet("{surveyID}")]
        public async Task<ActionResult<Logic.Objects.Survey>> GetAsync(Guid surveyID)
        {
            try
            {
                return Ok(await repository.GetSurveyByID(surveyID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Survey/5/SurveyQuestions
        /// <summary>
        /// Gets surveyquestions within a given survey by surveyID
        /// </summary>
        /// <param name="surveyID"></param>
        /// <returns></returns>
        [HttpGet("{surveyID}/SurveyQuestions")]
        public async Task<ActionResult<Logic.Objects.Survey>> GetQuestionsAsync(Guid surveyID)
        {
            try
            {
                return Ok(await repository.GetSurveyByID(surveyID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        //GET: api/Survey/5/SurveyQuestions/5/SurveyOptions
        [HttpGet("{surveyID}/SurveyQuestions/{surveyQuestionID}/SurveyOptions")]
        public async Task<ActionResult<List<Logic.Objects.Option>>> GetQuestionOptionAsync(Guid surveyID, Guid surveyQuestionID)
        {
            try
            {
                return Ok(await repository.GetSurveyOptionsBySurveyQuestionIDAsync(surveyID));
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // POST: api/Survey
        /// <summary>
        /// Posts new survey. Binds on the ApiSurvey model
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiSurvey>> Post([FromBody, Bind("eventTypeID, surveyDescription, active")] Api.Models.ApiSurvey survey)
        {
            try
            {
                Logic.Objects.Survey newSurvey = new Logic.Objects.Survey()
                {
                    surveyID = Guid.NewGuid(),
                    eventTypeID = survey.eventTypeID,
                    surveyDescription = survey.surveyDescription,
                    active = survey.active
                };

                try
                {
                    await repository.CreateSurveyAsync(newSurvey);
                    await repository.SaveAsync();
                    return Created($"api/Survey/{newSurvey.surveyID}", newSurvey);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // POST: api/Survey/5/SurveyQuestions
        /// <summary>
        /// Posts new question to a survey using its surveyID. Binds to the ApiQuestion model.
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost("{surveyID}/SurveyQuestions")]
        public async Task<ActionResult<Logic.Objects.Question>> PostSurveyQuestions(Guid surveyID, [FromBody, Bind("questionText, isSurveyOptionList, sortOrder, isActive")] Models.ApiQuestion question)
        {
            try
            {
                Logic.Objects.Question newQuestion = new Logic.Objects.Question(surveyID)
                {
                    questionID = Guid.NewGuid(),
                    questionText = question.questionText,
                    isSurveyOptionList = question.isSurveyOptionList,
                    isActive = question.isActive,
                    sortOrder = question.sortOrder,
                };

                //gives the new GUID in the question to send to the creation of the Xref
                question.questionID = newQuestion.questionID;

                try
                {
                    await repository.CreateSurveyQuestionAsync(newQuestion);
                    await repository.CreateSurveyQuestionXrefAsync(newQuestion);
                    await repository.SaveAsync();
                    return Created($"api/Survey/{surveyID}", surveyID);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        //POST: api/Survey/5/SurveyQuestions/5/SurveyOptions
        [HttpPost("{surveyID}/SurveyQuestions/{surveyQuestionID}/SurveyOptions")]
        public async Task<ActionResult<Logic.Objects.Option>> PostSurveyQuestionOption(Guid surveyID, Guid surveyQuestionID, [FromBody, Bind("surveyOptionID, displayText, surveyOptionValue, sortOrder, isActive")] Models.ApiQuestionOption questionOption)
        {
            try
            {
                Logic.Objects.Option logicSurveyOption = new Logic.Objects.Option(surveyQuestionID)
                {
                    surveyOptionID = Guid.NewGuid(),
                    displayText = questionOption.displayText,
                    surveyOptionValue = questionOption.surveyOptionValue,
                    sortOrder = questionOption.sortOrder,
                    isActive = questionOption.isActive
                };
                try
                {
                    await repository.CreateSurveyOptionAsync(logicSurveyOption);
                    await repository.SaveAsync();
                    await repository.CreateSurveyQuestionOptionXrefAsync(logicSurveyOption);
                    await repository.SaveAsync();
                    return Ok(logicSurveyOption);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/Survey/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}