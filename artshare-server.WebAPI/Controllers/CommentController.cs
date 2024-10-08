﻿using artshare_server.ApiModels.DTOs;
using artshare_server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentAsync()
        {
            var listComments = await _commentService.GetComments();
            if (listComments == null)
            {
                return NotFound();
            }
            return Ok(listComments);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCommentByArtWorkIdAsync(int artwordId)
        {
            var listComments = await _commentService.GetCommentByArtworkId(artwordId);
            if (listComments == null)
            {
                return NotFound();
            }
            return Ok(listComments);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO createCommentDTO)
        {
            var result = await _commentService.CreateCommentAsync(createCommentDTO);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}