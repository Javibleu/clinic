using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        
        /*
         * Injects IMapper to map Dtos from whole entities
         */
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _mapper = mapper;
        }

        [HttpGet]
        // Return MemberDto instead of AppUser
        // 154 add (parameters) to know witch page show
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams) 
        {

            PagedList<MemberDto> users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
            // 154- Uses HttpExtension to add header into Response
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }


        /*
         * Returns MemberDto instead of AppUser
         */

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _unitOfWork.UserRepository.GetMemberAsync(username); //97 GetMemberAsync instead of GetUserByUsername
        }




        /* EndPoint to update user profile from (member-edit)
         * just Task<ActionResult> because it doesnt return anything
         * 
         * */
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //gets AppUser using UserRepository and using ClaimsPrincipleExtension call as an argument(gets username from token)
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user); // maps DTo to User

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent(); // If all ok, return NoContent

            return BadRequest("Failed to update user");           // If error, bad request
        }




        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            //gets AppUser using UserRepository and using ClaimsPrincipleExtension call as an argument(gets username from token)
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            
            //Uses IPhotoservice injected and call it to pass file
            var result = await _photoService.AddPhotoAsync(file);

            //if previous methods gets an error, return BadRequest
            if (result.Error != null) return BadRequest(result.Error.Message);

            //instance new Photo and assign it data received from Cloudinary API
            var photo = new Photo {Url = result.SecureUrl.AbsoluteUri, PublicId = result.PublicId };

            //check if user doesnt have any Photo set this photo as Main
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            //add photo to photo[] belonging to user
            user.Photos.Add(photo);


            if (await _unitOfWork.Complete())
            {   //returns HTTP 201 with file location "GetUser", an User, and a PhotoDto just uploaded (all in the header)
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }

            //if fails return BadRequest
            return BadRequest("Problem addding photo");
        }




        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            //gets AppUser using UserRepository and using ClaimsPrincipleExtension call as an argument(gets username from token)
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            //as previous method GetUserByUsernameAsync is eager, user have all the photos
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            //if photo is mail --> Bad Request
            if (photo.IsMain) return BadRequest("This is already your main photo");

            //gets current Main Photo
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            
            //if there is a Main Photo make it false
            if (currentMain != null) currentMain.IsMain = false;
            
            //make photo Main
            photo.IsMain = true;

            //if all ok, return NoContent HTTP
            if (await _unitOfWork.Complete()) return NoContent();
            
            //if not, return BadRequest
            return BadRequest("Failed to set main photo");
        }



        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}