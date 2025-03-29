using Hotels.Models.Dto.Guest;
using Hotels.Models.Dto.GuestReservation;
using Hotels.Models.Dto.Hotel;
using Hotels.Models.Dto.Manager;
using Hotels.Models.Dto.Reservation;
using Hotels.Models.Dto.Room;
using Hotels.Models.Entities;
using Hotels.Repository.Interfaces;
using Hotels.Service.Exceptions;
using Hotels.Service.Implementations;
using Hotels.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement_Project.Controllers
{
    [Route("api/hotel")]
    [ApiController]
   
    public class HotelController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        private readonly IManagerService _managerService;
        private readonly IGuestService _guestService;
        private readonly IReservationService _reservationService;
        private readonly IGuestReservationService _guestReservationService;
        public HotelController(IHotelService hotelService, 
            IManagerService managerService , 
            IRoomService roomService,
            IGuestService guestService, 
            IReservationService reservationService,
            IGuestReservationService guestReservationService)
        {
            _hotelService = hotelService;
            _managerService = managerService;
            _roomService = roomService;
            _guestService = guestService;
            _reservationService = reservationService;
            _guestReservationService = guestReservationService;
        }

      

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddHotelAsync([FromBody] HotelForCreateDto hotelDto)
        {
            try
            {
                await _hotelService.AddHotelAsync(hotelDto);
                await _hotelService.SaveHotel();
                return Ok("Hotel added successfully.");
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
      
        
        [HttpDelete("{hotelId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHotel(int hotelId)
        {
            try
            {
                await _hotelService.DeleteHotelAsync(hotelId);
                await _hotelService.SaveHotel();
                return Ok("Hotel deleted successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<HotelForGettingDto>>> GetAllHotels([FromQuery] string country, [FromQuery] string city, [FromQuery] int? rating)
        {
            var hotels = await _hotelService.GetAllHotelsAsync(country, city, rating);
            return Ok(hotels);
        }

       


        [HttpGet("{hotelId}")]

        public async Task<ActionResult<HotelForGettingDto>> GetHotel(int hotelId)
        {
            try
            {
                var hotel = await _hotelService.GetHotelAsync(hotelId);
                return Ok(hotel);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut("{hotelId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHotel(int hotelId, [FromBody] HotelForUpdateingDto hotelDto)
        {
            if (hotelId != hotelDto.Id)
            {
                return BadRequest("Hotel ID mismatch.");
            }

            try
            {
                await _hotelService.UpdateHotelAsync(hotelDto);
                await _hotelService.SaveHotel();
                return Ok("Hotel Update successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }

        }



        [HttpGet("country/{country}")]
        public async Task<IActionResult> GetHotelsByCountry(string country)
        {
            var hotels = await _hotelService.GetHotelsByCountryAsync(country);
            return Ok(hotels);
        }


        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetHotelsByCity(string city)
        {
            var hotels = await _hotelService.GetHotelsByCityAsync(city);
            return Ok(hotels);
        }

        [HttpGet("rating/{rating}")]
        public async Task<IActionResult> GetHotelsByRating(int rating)
        {
            var hotels = await _hotelService.GetHotelsByRatingAsync(rating);
            return Ok(hotels);
        }
        //  ------------

        [HttpPost("Manager")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddManager([FromBody] ManagerForCreateDto managerDto)
        {
            if (managerDto == null)
            {
                return BadRequest("Manager details not provided.");
            }

            try
            {
                await _managerService.AddManagerAsync(managerDto);
                await _managerService.SaveManager();
                return Ok("Manager added successfully");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500 );
            }
        }

        [HttpPut("Manager")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateManager([FromBody] ManagerForUpdateDto managerDto)
        {
            try
            {
                await _managerService.UpdateManagerAsync(managerDto);
                await _managerService.SaveManager();
                return Ok("Manager updated successfully");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("Manager/{managerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteManager(int managerId)
        {
            try
            {
                await _managerService.DeleteManagerAsync(managerId);
                await _managerService.SaveManager();  
                return Ok("Manager deleted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("manager")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllManagers()
        {
            try
            {
                var managers = await _managerService.GetAllManagersAsync();
                await _managerService.SaveManager();
                return Ok(managers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }



        //  ------------

        [HttpPost("room")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddRoom([FromBody] RoomForCreateDto roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest("Invalid room data.");
            }
            try
            {
                await _roomService.AddRoomAsync(roomDto);
                await _roomService.SaveRoom();
                return Ok("Room added successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("room/{roomId}")]
      
        public async Task<IActionResult> GetRoomById(int roomId)
        {
            try
            {
               
                var room = await _roomService.GetRoomByIdAsync(roomId);
                await _roomService.SaveRoom();
                return Ok(room); 
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); 
            }
        }


        [HttpPut("room/{roomId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateRoom(int roomId, [FromBody] RoomForUpdateDto roomDto)
        {
            if (roomId != roomDto.Id)
            {
                return BadRequest("Room ID mismatch."); 
            }

            try
            {
                await _roomService.UpdateRoomAsync(roomDto);
                await _roomService.SaveRoom();
                return Ok("Room update successfully");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpDelete("room/{roomId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            try
            {
                await _roomService.DeleteRoomAsync(roomId);
                await _roomService.SaveRoom();
                return Ok("Room delete successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Is not available"); 
            }
        }

        [HttpGet("room")]
        [Authorize(Roles = "Admin,Manager,Guest")]
        public async Task<IActionResult> GetAllRoomsAsync([FromBody] RoomForFilterDto filter)
        {
            var result = await _roomService.GetAllRoomsAsync(filter);
            await _roomService.SaveRoom();
            return Ok(result);
        }


        //  ------------


        [HttpPost("guest")]
      
        public async Task<IActionResult> AddGuest([FromBody] GuestForCreateDto guestDto)
        {
            try
            {
                await _guestService.AddGuestAsync(guestDto);
                await _guestService.SaveGuest();
                return Ok("Guest added successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("guest")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateGuest([FromBody] GuestForUpdateDto guestDto)
        {
            try
            {
                await _guestService.UpdateGuestAsync(guestDto);
                await _guestService.SaveGuest();
                return Ok("Guest updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("guest")]
         [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllGuestsAsync()
        {
            try
            {
                var guests = await _guestService.GetAllGuestsAsync();
                await _guestService.SaveGuest();// Get all guests via service
                return Ok(guests);  // Return the list of guests as a successful response
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Handle any errors and return 500 status code
            }
        }

        [HttpGet("guest/{guestId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetGuestAsync(int guestId)
        {
            try
            {
                var guestDto = await _guestService.GetGuestAsync(guestId);
                await _guestService.SaveGuest();
                return Ok(guestDto);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("guest/{guestId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteGuest(int guestId)
        {
            try
            {
                await _guestService.DeleteGuestAsync(guestId);
                await _guestService.SaveGuest();
                return Ok("Guest deleted successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }



      //  ------------


        [HttpPost("reservation")]
        [Authorize(Roles = "Admin,Manager,Guest")]
        public async Task<IActionResult> CreateReservationAsync([FromBody] ReservationForCreateDto reservationDto)
        {
            try
            {
                await _reservationService.CreateReservationAsync(reservationDto);
                await _reservationService.SaveReservation();
                return Ok("Reservation created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        
        }


        [HttpGet("reservation")]
        [Authorize(Roles = "Admin,Manager,Guest")]
        public async Task<IActionResult> GetReservations([FromQuery] ReservationFilterDto filter)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsAsync(filter);
                await _reservationService.SaveReservation();
                if (reservations == null || !reservations.Any())
                {
                    return NotFound("No reservations found matching the filter.");
                }
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
       


        [HttpPut("reservation")]
        [Authorize(Roles = "Admin,Manager,Guest")]
        public async Task<IActionResult> UpdateReservation([FromBody] ReservationForUpdateDto reservationDto)
        {
            if (reservationDto == null)
            {
                return BadRequest("Reservation data is required.");
            }

            try
            {
                await _reservationService.UpdateReservationAsync(reservationDto);
                await _reservationService.SaveReservation();
                return Ok("Reservation updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("reservation/{id}")]
        [Authorize(Roles = "Admin,Manager,Guest")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            try
            {
                await _reservationService.CancelReservationAsync(id);
                await _reservationService.SaveReservation();
                return Ok("Reservation canceled successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("reservationfilter")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<List<ReservationForGettingDto>>> GetFilteredReservations(
          [FromQuery] int? hotelId,
          [FromQuery] string? PersonalNumber,
          [FromQuery] int? roomId)
        {
            var reservations = await _reservationService.GetFilteredReservationsAsync(hotelId, PersonalNumber, roomId);
            await _reservationService.SaveReservation();
            if (reservations == null || !reservations.Any())
            {
                return NotFound("No reservations found.");
            }

            return Ok(reservations);
        }

        //----------------

        [HttpGet("guestReservation")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<List<GuestReservationForGettingDto>>> GetAllGuestReservations()
        {
            var guestReservations = await _guestReservationService.GetAllGuestReservationsAsync();
            if (guestReservations == null || !guestReservations.Any())
            {
                return NotFound("Guest reservations not found.");
            }
            return Ok(guestReservations);
        }




    }
}
