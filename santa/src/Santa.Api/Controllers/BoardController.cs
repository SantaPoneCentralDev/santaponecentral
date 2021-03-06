﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models.Board_Entry_Models;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BoardController : ControllerBase
    {
        private readonly IRepository repository;

        public BoardController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // GET: api/Board
        /// <summary>
        /// Gets a list of all the board entries posted
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<BoardEntry>>> GetAllEntries()
        {
            try
            {
                List<BoardEntry> logicBoardEntries = await repository.GetAllBoardEntriesAsync();
                return Ok(logicBoardEntries.OrderBy(e => e.threadNumber).ThenBy(e => e.postNumber));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }

        }

        // GET: api/Board/5
        /// <summary>
        /// Gets a specific board entry by its board entry ID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <returns></returns>
        [HttpGet("{boardEntryID}")]
        [AllowAnonymous]
        public async Task<ActionResult<BoardEntry>> GetBoardEntryByID(Guid boardEntryID)
        {
            try
            {
                BoardEntry logicBoardEntry = await repository.GetBoardEntryByIDAsync(boardEntryID);
                return logicBoardEntry;
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // GET: api/Board/ThreadNumber/5/PostNumber/5
        /// <summary>
        /// Gets a board entry by it's thread and post numbers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ThreadNumber/{threadNumber}/PostNumber/{postNumber}")]
        [AllowAnonymous]
        public async Task<ActionResult<BoardEntry>> GetBoardEntryByPostNumber(int threadNumber, int postNumber)
        {
            try
            {
                BoardEntry logicBoardEntry = await repository.GetBoardEntryByThreadAndPostNumberAsync(threadNumber, postNumber);
                return logicBoardEntry;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // POST: api/Board
        /// <summary>
        /// Posts a new board entry object to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "create:boardEntries")]
        public async Task<ActionResult<BoardEntry>> PostNewBoardEntry([FromBody] NewBoardEntryModel model)
        {
            try
            {
                BoardEntry newLogicBoardEntry = new BoardEntry()
                {
                    boardEntryID = Guid.NewGuid(),
                    entryType = new EntryType()
                    {
                        entryTypeID = model.entryTypeID
                    },
                    dateTimeEntered = DateTime.UtcNow,
                    threadNumber = model.threadNumber,
                    postNumber = model.postNumber,
                    postDescription = model.postDescription
                };
                await repository.CreateBoardEntryAsync(newLogicBoardEntry);
                await repository.SaveAsync();

                return Ok(await repository.GetBoardEntryByIDAsync(newLogicBoardEntry.boardEntryID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/Board/5/PostNumber
        /// <summary>
        /// Updates a board entry's post number by its boardEntryID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{boardEntryID}/PostNumber")]
        [Authorize(Policy = "update:boardEntries")]
        public async Task<ActionResult<BoardEntry>> PutPostNumber(Guid boardEntryID, [FromBody] EditPostNumberModel model)
        {
            try
            {
                BoardEntry newLogicBoardEntry = new BoardEntry()
                {
                    boardEntryID = boardEntryID,
                    postNumber = model.postNumber
                };
                await repository.UpdateBoardEntryPostNumberAsync(newLogicBoardEntry);
                await repository.SaveAsync();
                return Ok(await repository.GetBoardEntryByIDAsync(boardEntryID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/Board/5/ThreadNumber
        /// <summary>
        /// Updates a board entry's thread number by its boardEntryID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{boardEntryID}/ThreadNumber")]
        [Authorize(Policy = "update:boardEntries")]
        public async Task<ActionResult<BoardEntry>> PutThreadNumber(Guid boardEntryID, [FromBody] EditThreadNumberModel model)
        {
            try
            {
                BoardEntry newLogicBoardEntry = new BoardEntry()
                {
                    boardEntryID = boardEntryID,
                    threadNumber = model.threadNumber
                };
                await repository.UpdateBoardEntryPostNumberAsync(newLogicBoardEntry);
                await repository.SaveAsync();
                return Ok(await repository.GetBoardEntryByIDAsync(boardEntryID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/Board/5/PostDescription
        /// <summary>
        /// Updates a board entry's post description by its boardEntryID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{boardEntryID}/PostDescription")]
        [Authorize(Policy = "update:boardEntries")]
        public async Task<ActionResult<BoardEntry>> PutPostDescription(Guid boardEntryID, [FromBody] EditPostDescriptionModel model)
        {
            try
            {
                BoardEntry newLogicBoardEntry = new BoardEntry()
                {
                    boardEntryID = boardEntryID,
                    postDescription = model.postDescription
                };
                await repository.UpdateBoardEntryPostDescriptionAsync(newLogicBoardEntry);
                await repository.SaveAsync();
                return Ok(await repository.GetBoardEntryByIDAsync(boardEntryID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }
        // PUT: api/Board/5/EntryType
        /// <summary>
        /// Updates a board entry's type by boardEntryID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{boardEntryID}/EntryType")]
        [Authorize(Policy = "update:boardEntries")]
        public async Task<ActionResult<BoardEntry>> PutEntryType(Guid boardEntryID, [FromBody] EditEntryTypeModel model)
        {
            try
            {
                BoardEntry newLogicBoardEntry = new BoardEntry()
                {
                    boardEntryID = boardEntryID,
                    entryType = new EntryType()
                    {
                        entryTypeID = model.entryTypeID
                    }
                };
                await repository.UpdateBoardEntryTypeAsync(newLogicBoardEntry);
                await repository.SaveAsync();
                return Ok(await repository.GetBoardEntryByIDAsync(boardEntryID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // DELETE: api/Board/5
        /// <summary>
        /// Deletes a board entry by its ID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <returns></returns>
        [HttpDelete("{boardEntryID}")]
        [Authorize(Policy = "delete:boardEntries")]
        public async Task<ActionResult> DeleteBoardEntry(Guid boardEntryID)
        {
            try
            {
                await repository.DeleteBoardEntryByIDAsync(boardEntryID);
                await repository.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }
    }
}
