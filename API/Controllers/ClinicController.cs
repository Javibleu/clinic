using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/clinic")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClinicController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClinicDto>>> GetClinics()
        {

            var clinics = await _unitOfWork.ClinicRepository.GetClinicsAsync();

            var clinicsDto = _mapper.Map<IEnumerable<ClinicDto>>(clinics);

            return Ok(clinicsDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Clinic>> Register([FromBody] ClinicDto clinicDto)
        {
            var clinics = await _unitOfWork.ClinicRepository.GetClinicsAsync();

            Clinic clinic = _mapper.Map<Clinic>(clinicDto);                                   //map registerDto to AppUser

            clinic.ClinicName = clinicDto.ClinicName.ToLower();                                     //set username to lowercase

             _unitOfWork.ClinicRepository.AddClinic(clinic);      //Add to Role with member as default

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to add clinic");
        }


    }
}
