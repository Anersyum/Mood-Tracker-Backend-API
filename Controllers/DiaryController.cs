using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialSite.API.Data;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DiaryController : ControllerBase
    {
        private readonly IDiaryRepository diaryRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration config;
        public DiaryController(IDiaryRepository diaryRepository, IConfiguration config, IMapper mapper)
        {
            this.config = config;
            this.mapper = mapper;
            this.diaryRepository = diaryRepository;
        }

        [HttpGet("get/user/{userId}/entries")]
        public async Task<IActionResult> GetAllDiaryEntriesAction(int userId)
        {
            // maybe check if the user exists?
            var diaryEntries = await this.diaryRepository.GetAllUserDiaryEntries(userId);
            var mappedEntries = this.mapper.Map<ICollection<DiaryToReturnDto>>(diaryEntries);

            return Ok(mappedEntries);
        }

        [HttpGet("get/user/{userId}/entry/{diaryId}")]
        public async Task<IActionResult> GetDiaryEntryAction(int userId, int diaryId)
        {
            var diaryEntry = await this.diaryRepository.GetDiaryEntry(diaryId, userId);
            var mappedEntry = this.mapper.Map<DiaryToReturnDto>(diaryEntry);

            return Ok(mappedEntry);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveDiaryEntryAction(DiaryDto diaryDto)
        {
            Diary diary = new Diary
            {
                Title = diaryDto.Title,
                Entry = diaryDto.Entry,
                UserId = diaryDto.UserId
            };

            await this.diaryRepository.SaveNewDiaryEntry(diary);

            var mappedDiaryEntry = this.mapper.Map<DiaryToReturnDto>(diary);

            return StatusCode(201, mappedDiaryEntry);
        }
    }
}