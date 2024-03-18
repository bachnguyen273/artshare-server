using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService            ;

        public LikeController(ILikeService likeService)
        {
            _likeService= likeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLikeByArtworkId(int artworkId)
        {
            var listLike = await _likeService.GetAllLikeByArtworkId(artworkId);
            if (listLike == null)
            {
                return NotFound();
            }
            return Ok(listLike);
        }
        [HttpGet]
        public async Task<IActionResult> CountLikeByArtWorkId(int artworkId)
        {
            var likes = await _likeService.CountLikeByArtWorkId(artworkId);
            if (likes == null)
            {
                return NotFound();
            }
            return Ok(likes);
        }
        [HttpGet]
        public async Task<IActionResult> CountDisLikeByArtWorkId(int artworkId)
        {
            var Dislikes = await _likeService.CountDisLikeByArtWorkId(artworkId);
            if (Dislikes == null)
            {
                return NotFound();
            }
            return Ok(Dislikes);
        }
        [HttpGet]
        public async Task<IActionResult> GetLikeByAccountIdAndArtworkId(int accountId, int artworkId)
        {
            var listLike = await _likeService.GetLikeByAccountIdAndArtworkId(accountId, artworkId);
            if (listLike == null)
            {
                return NotFound();
            }
            return Ok(listLike);
        }
        [HttpPut]
        public async Task<IActionResult> Like(int accountId, int artworkId)
        {
            var like = await _likeService.GetLikeByAccountIdAndArtworkId(accountId, artworkId);
            bool result = false; 
            if (like == null)
            {
                result = await _likeService.CreateLikeAsync(new Like() { AccountId = accountId, ArtworkId = artworkId,IsLike = true });
            }
            else
            {
                if(like.IsLike==true)
                {
                    result = await _likeService.DeleteLikeAsync(like);
                }
                else
                {
                    like.IsLike = true;
                    result = await _likeService.UpdateLikeAsync(like);
                }
            }
            if (result == false) 
                return BadRequest();
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> DisLike(int accountId, int artworkId)
        {
            var like = await _likeService.GetLikeByAccountIdAndArtworkId(accountId, artworkId);
            bool result = false;
            if (like == null)
            {
                result = await _likeService.CreateLikeAsync(new Like() { AccountId = accountId, ArtworkId = artworkId, IsLike = false });
            }
            else
            {
                if (like.IsLike == false)
                {
                    result = await _likeService.DeleteLikeAsync(like);
                }
                else
                {
                    like.IsLike = false;
                    result = await _likeService.UpdateLikeAsync(like);
                }
            }
            if (result == false)
                return BadRequest();
            return Ok();
        }
    }
}
